using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace AstralAirheads.Analyzers;

/// <summary>
/// Code fix provider for IInitializable analyzer issues, yadda-yadda.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(IInitializableCodeFixProvider)), Shared]
public class IInitializableCodeFixProvider : CodeFixProvider
{
    public const string AddInitializeCallTitle = "Add Initialize() call";
    public const string AddIsInitializedCheckTitle = "Add IsInitialized() check";
    public const string AddUsingStatementTitle = "Wrap in using statement";

    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(IInitializableAnalyzer.DiagnosticId);

    public sealed override FixAllProvider GetFixAllProvider()
    {
        return WellKnownFixAllProviders.BatchFixer;
    }

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var node = root?.FindNode(diagnosticSpan);

        if (node == null)
            return;

        var message = diagnostic.GetMessage();
        
        if (message.Contains("Initialize() not called immediately"))
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: AddInitializeCallTitle,
                    createChangedDocument: c => AddInitializeCallAsync(context.Document, node, c),
                    equivalenceKey: AddInitializeCallTitle),
                diagnostic);
        }
        else if (message.Contains("Initialize() called multiple times"))
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: AddIsInitializedCheckTitle,
                    createChangedDocument: c => AddIsInitializedCheckAsync(context.Document, node, c),
                    equivalenceKey: AddIsInitializedCheckTitle),
                diagnostic);
        }
        else if (message.Contains("using statement but Initialize() not called"))
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: AddUsingStatementTitle,
                    createChangedDocument: c => AddUsingStatementAsync(context.Document, node, c),
                    equivalenceKey: AddUsingStatementTitle),
                diagnostic);
        }
    }

    private static async Task<Document> AddInitializeCallAsync(Document document, SyntaxNode node, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var editor = new SyntaxEditor(root!, document.Project.Solution.Workspace.Services);

        if (node is ObjectCreationExpressionSyntax objectCreation)
        {
            var parent = objectCreation.Parent;
            if (parent is ExpressionStatementSyntax expressionStatement)
            {
                var nextStatement = GetNextStatement(expressionStatement);
                if (nextStatement == null)
                {
                    // Add Initialize() call after object creation
                    var initializeCall = SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                objectCreation,
                                SyntaxFactory.IdentifierName("Initialize"))));

                    var newBlock = SyntaxFactory.Block()
                        .AddStatements(expressionStatement)
                        .AddStatements(initializeCall);

                    var containingBlock = GetContainingBlock(expressionStatement);
                    if (containingBlock != null)
                    {
                        var statements = containingBlock.Statements.ToList();
                        var index = statements.IndexOf(expressionStatement);
                        statements.RemoveAt(index);
                        statements.Insert(index, newBlock);

                        var newContainingBlock = containingBlock.WithStatements(SyntaxFactory.List(statements));
                        editor.ReplaceNode(containingBlock, newContainingBlock);
                    }
                }
            }
        }

        var newRoot = editor.GetChangedRoot();
        return document.WithSyntaxRoot(newRoot);
    }

    private static async Task<Document> AddIsInitializedCheckAsync(Document document, SyntaxNode node, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var editor = new SyntaxEditor(root!, document.Project.Solution.Workspace.Services);

        if (node is InvocationExpressionSyntax invocation)
        {
            var memberAccess = invocation.Expression as MemberAccessExpressionSyntax;
            if (memberAccess?.Name.Identifier.Text == "Initialize")
            {
                var objectExpression = memberAccess.Expression;
                
                // create if statement to check IsInitialized()
                var isInitializedCall = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        objectExpression,
                        SyntaxFactory.IdentifierName("IsInitialized")));

                var ifStatement = SyntaxFactory.IfStatement(
                    SyntaxFactory.PrefixUnaryExpression(
                        SyntaxKind.LogicalNotExpression,
                        isInitializedCall),
                    SyntaxFactory.Block().AddStatements(
                        SyntaxFactory.ExpressionStatement(invocation)));

                var containingStatement = GetContainingStatement(invocation);
                if (containingStatement != null)
                {
                    editor.ReplaceNode(containingStatement, ifStatement);
                }
            }
        }

        var newRoot = editor.GetChangedRoot();
        return document.WithSyntaxRoot(newRoot);
    }

    private static async Task<Document> AddUsingStatementAsync(Document document, SyntaxNode node, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var editor = new SyntaxEditor(root!, document.Project.Solution.Workspace.Services);

        if (node is UsingStatementSyntax usingStatement)
        {
            var block = usingStatement.Statement as BlockSyntax;
            if (block != null)
            {
                // add Initialize() call at the beginning of the using block
                var variableName = GetVariableName(usingStatement);
                if (!string.IsNullOrEmpty(variableName))
                {
                    var initializeCall = SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(variableName ?? "myVar"),
                                SyntaxFactory.IdentifierName("Initialize"))));

                    var newBlock = block.AddStatements(initializeCall);
                    var newUsingStatement = usingStatement.WithStatement(newBlock);
                    editor.ReplaceNode(usingStatement, newUsingStatement);
                }
            }
        }

        var newRoot = editor.GetChangedRoot();
        return document.WithSyntaxRoot(newRoot);
    }

    private static StatementSyntax? GetNextStatement(StatementSyntax statement)
    {
        var parent = statement.Parent;
        if (parent is BlockSyntax block)
        {
            var statements = block.Statements;
            for (int i = 0; i < statements.Count - 1; i++)
            {
                if (statements[i] == statement)
                    return statements[i + 1];
            }
        }
        return null;
    }

    private static BlockSyntax? GetContainingBlock(SyntaxNode node)
    {
        var current = node;
        while (current != null)
        {
            if (current is BlockSyntax block)
                return block;
            current = current.Parent;
        }
        return null;
    }

    private static StatementSyntax? GetContainingStatement(SyntaxNode node)
    {
        var current = node;
        while (current != null)
        {
            if (current is StatementSyntax statement)
                return statement;
            current = current.Parent;
        }
        return null;
    }

    private static string? GetVariableName(UsingStatementSyntax usingStatement)
    {
        if (usingStatement.Declaration != null)
        {
            var variable = usingStatement.Declaration.Variables.FirstOrDefault();
            return variable?.Identifier.Text;
        }
        else if (usingStatement.Expression is IdentifierNameSyntax identifier)
        {
            return identifier.Identifier.Text;
        }
        return null;
    }
} 