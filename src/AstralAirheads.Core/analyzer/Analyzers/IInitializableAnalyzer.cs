using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AstralAirheads.Analyzers;

/// <summary>
/// Analyzer for IInitializable interface to enforce proper usage patterns. Boring, I know.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class IInitializableAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "AAC001";

    private static readonly LocalizableString Title = "IInitializable usage violation";
    private static readonly LocalizableString MessageFormat = "{0}";
    private static readonly LocalizableString Description = "Enforces proper usage patterns for IInitializable interface.";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        Title,
        MessageFormat,
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
		ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.InvocationExpression);
        context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
        context.RegisterSyntaxNodeAction(AnalyzeUsingStatement, SyntaxKind.UsingStatement);
    }

    private void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;
        var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation);
        
        if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
            return;

        var receiverType = GetReceiverType(context.SemanticModel, invocation);
        if (receiverType == null || !ImplementsIInitializable(receiverType))
            return;

        // check for usage before initialization
        if (IsMethodCallBeforeInitialization(invocation, methodSymbol, context))
        {
            var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation(), 
                "Method called on IInitializable object before initialization. Call Initialize() first.");
            context.ReportDiagnostic(diagnostic);
        }

        // check for double initialization
        if (methodSymbol.Name == "Initialize" && IsDoubleInitialization(invocation, context))
        {
            var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation(), 
                "Initialize() called multiple times on the same object. Check IsInitialized() first.");
            context.ReportDiagnostic(diagnostic);
        }

        // check for usage after disposal
        if (IsMethodCallAfterDisposal(invocation, methodSymbol, context))
        {
            var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation(), 
                "Method called on IInitializable object after disposal.");
            context.ReportDiagnostic(diagnostic);
        }
    }

    private void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
    {
        var objectCreation = (ObjectCreationExpressionSyntax)context.Node;
        var typeInfo = context.SemanticModel.GetTypeInfo(objectCreation);
        
        if (typeInfo.Type == null || !ImplementsIInitializable(typeInfo.Type))
            return;

        // Check if Initialize() is called immediately after creation
        var parent = objectCreation.Parent;
        if (parent is ExpressionStatementSyntax expressionStatement)
        {
            var nextStatement = GetNextStatement(expressionStatement);
            if (nextStatement == null || !IsInitializeCall(nextStatement, objectCreation, context))
            {
                var diagnostic = Diagnostic.Create(Rule, objectCreation.GetLocation(), 
                    "IInitializable object created but Initialize() not called immediately. Consider calling Initialize() right after creation.");
                context.ReportDiagnostic(diagnostic);
            }
        }
    }

    private void AnalyzeUsingStatement(SyntaxNodeAnalysisContext context)
    {
        var usingStatement = (UsingStatementSyntax)context.Node;
        var typeInfo = context.SemanticModel.GetTypeInfo(usingStatement.Declaration?.Type ?? usingStatement.Expression!);
        
        if (typeInfo.Type == null || !ImplementsIInitializable(typeInfo.Type))
            return;

        // check if Initialize() is called within the using block
        if (!IsInitializeCalledInBlock(usingStatement.Statement, context))
        {
            var diagnostic = Diagnostic.Create(Rule, usingStatement.GetLocation(), 
                "IInitializable object in using statement but Initialize() not called. Call Initialize() within the using block.");
            context.ReportDiagnostic(diagnostic);
        }
    }

    private static ITypeSymbol? GetReceiverType(SemanticModel semanticModel, InvocationExpressionSyntax invocation)
    {
        if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
        {
            var expressionType = semanticModel.GetTypeInfo(memberAccess.Expression);
            return expressionType.Type;
        }
        return null;
    }

    private static bool ImplementsIInitializable(ITypeSymbol type)
    {
        return type.AllInterfaces.Any(i => i.Name == "IInitializable" && i.ContainingNamespace?.Name == "AstralAirheads" && i.ContainingNamespace?.ContainingNamespace?.Name == "Util");
    }

    private static bool IsMethodCallBeforeInitialization(InvocationExpressionSyntax invocation, IMethodSymbol methodSymbol, SyntaxNodeAnalysisContext context)
    {
        // skip Initialize() and IsInitialized() methods
        if (methodSymbol.Name == "Initialize" || methodSymbol.Name == "IsInitialized" || methodSymbol.Name == "Dispose")
            return false;

        // check if Initialize() was called before this method
        var methodBlock = GetContainingMethodBlock(invocation);
        if (methodBlock == null)
            return false;

        var statements = GetStatementsInBlock(methodBlock);
        var currentIndex = GetStatementIndex(statements, invocation);
        
        // look for Initialize() call before current statement
        for (int i = 0; i < currentIndex; i++)
        {
            if (IsInitializeCall(statements[i], invocation, context))
                return false;
        }

        return true;
    }

    private static bool IsDoubleInitialization(InvocationExpressionSyntax invocation, SyntaxNodeAnalysisContext context)
    {
        var methodBlock = GetContainingMethodBlock(invocation);
        if (methodBlock == null)
            return false;

        var statements = GetStatementsInBlock(methodBlock);
        var currentIndex = GetStatementIndex(statements, invocation);
        var initializeCallCount = 0;

        // count Initialize() calls before current statement
        for (int i = 0; i < currentIndex; i++)
        {
            if (IsInitializeCall(statements[i], invocation, context))
                initializeCallCount++;
        }

        return initializeCallCount > 0;
    }

    private static bool IsMethodCallAfterDisposal(InvocationExpressionSyntax invocation, IMethodSymbol methodSymbol, SyntaxNodeAnalysisContext context)
    {
        // skip Dispose() method itself
        if (methodSymbol.Name == "Dispose")
            return false;

        var methodBlock = GetContainingMethodBlock(invocation);
        if (methodBlock == null)
            return false;

        var statements = GetStatementsInBlock(methodBlock);
        var currentIndex = GetStatementIndex(statements, invocation);

        // look for Dispose() call before current statement
        for (int i = 0; i < currentIndex; i++)
        {
            if (IsDisposeCall(statements[i], invocation, context))
                return true;
        }

        return false;
    }

    private static bool IsInitializeCall(StatementSyntax statement, ExpressionSyntax targetExpression, SyntaxNodeAnalysisContext context)
    {
        if (statement is ExpressionStatementSyntax expressionStatement)
        {
            if (expressionStatement.Expression is InvocationExpressionSyntax invocation)
            {
                var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation);
                if (symbolInfo.Symbol is IMethodSymbol methodSymbol && methodSymbol.Name == "Initialize")
                {
                    // check if it's called on the same object
                    if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
                    {
                        return IsSameObject(memberAccess.Expression, targetExpression, context);
                    }
                }
            }
        }
        return false;
    }

    private static bool IsDisposeCall(StatementSyntax statement, ExpressionSyntax targetExpression, SyntaxNodeAnalysisContext context)
    {
        if (statement is ExpressionStatementSyntax expressionStatement)
        {
            if (expressionStatement.Expression is InvocationExpressionSyntax invocation)
            {
                var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation);
                if (symbolInfo.Symbol is IMethodSymbol methodSymbol && methodSymbol.Name == "Dispose")
                {
                    // check if it's called on the same object
                    if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
                    {
                        return IsSameObject(memberAccess.Expression, targetExpression, context);
                    }
                }
            }
        }
        return false;
    }

    private static bool IsSameObject(ExpressionSyntax expr1, ExpressionSyntax expr2, SyntaxNodeAnalysisContext context) => 
		expr1.ToString() == expr2.ToString();

    private static BlockSyntax? GetContainingMethodBlock(SyntaxNode node)
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

    private static StatementSyntax[] GetStatementsInBlock(BlockSyntax block) =>
		[.. block.Statements];

    private static int GetStatementIndex(StatementSyntax[] statements, SyntaxNode node)
    {
        for (int i = 0; i < statements.Length; i++)
        {
            if (statements[i].Contains(node))
                return i;
        }
        return -1;
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

    private static bool IsInitializeCalledInBlock(StatementSyntax statement, SyntaxNodeAnalysisContext context)
    {
        if (statement is BlockSyntax block)
        {
            foreach (var stmt in block.Statements)
            {
                if (stmt is ExpressionStatementSyntax expressionStatement)
                {
                    if (expressionStatement.Expression is InvocationExpressionSyntax invocation)
                    {
                        var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation);
                        if (symbolInfo.Symbol is IMethodSymbol methodSymbol && methodSymbol.Name == "Initialize")
                            return true;
                    }
                }
            }
        }
        return false;
    }
} 