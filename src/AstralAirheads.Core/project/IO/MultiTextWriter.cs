// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace AstralAirheads.IO;

/// <summary>
/// A <seealso cref="TextWriter"/> that can write to all outputs.
/// </summary>
public class MultiTextWriter : TextWriter
{
    /// <inheritdoc/>
    public override Encoding Encoding => Encoding.ASCII;

    private IEnumerable<TextWriter> _writers;

    /// <summary>
    /// Initializes a new <seealso cref="MultiTextWriter"/> without any initial writers.
    /// </summary>
    public MultiTextWriter()
    {
        _writers = [];
    }

    /// <summary>
    /// Initializes a new <seealso cref="MultiTextWriter"/> with multiple writers.
    /// </summary>
    public MultiTextWriter(params TextWriter[] writers)
    {
        _writers = writers;
    }

    /// <summary>
    /// Initializes a new <seealso cref="MultiTextWriter"/> with multiple writers.
    /// </summary>
    public MultiTextWriter(IEnumerable<TextWriter> writers)
    {
        _writers = writers;
    }

    /// <summary>
    /// Initializes a new <seealso cref="MultiTextWriter"/> without initial writers,
    /// but with an new <seealso cref="IFormatProvider"/>.
    /// </summary>
    public MultiTextWriter(IFormatProvider? formatProvider) : base(formatProvider)
    {
        _writers = [];
    }

    /// <summary>
    /// Adds an existing <seealso cref="TextWriter"/> into the <seealso cref="MultiTextWriter"/>.
    /// </summary>
    /// <param name="writer">The value of the writer.</param>
    public void AddWriter(TextWriter writer)
    {
        _writers = _writers.Append(writer);
    }

    /// <inheritdoc/>
    public override void Write(bool value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(char value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(char[]? buffer)
    {
        foreach (var writer in _writers)
            writer.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(char[] buffer, int index, int count)
    {
        foreach (var writer in _writers)
            writer.Write(buffer, index, count);
    }

#if NET8_0_OR_GREATER
    /// <inheritdoc/>
    public override void Write(ReadOnlySpan<char> buffer)
    {
        foreach (var writer in _writers)
            writer.Write(buffer);
    }
#endif

    /// <inheritdoc/>
    public override void Write(int value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(string? value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
    {
        foreach (var writer in _writers)
            writer.Write(format, arg0);
    }

    /// <inheritdoc/>
    public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
    {
        foreach (var writer in _writers)
            writer.Write(format, arg0);
    }

    /// <inheritdoc/>
    public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1,
        object? arg2)
    {
        foreach (var writer in _writers)
            writer.Write(format, arg0, arg1, arg2);
    }

    /// <inheritdoc/>
    public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args)
    {
        foreach (var writer in _writers)
            writer.Write(format, args);
    }

    /// <inheritdoc/>
    public override void Write(object? value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

#if NET8_0_OR_GREATER
    /// <inheritdoc/>
    public override void Write(StringBuilder? value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }
#endif

    /// <inheritdoc/>
    public override void Write(long value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(ulong value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(float value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(double value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(decimal value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    /// <inheritdoc/>
    public override void WriteLine()
    {
        foreach (var writer in _writers)
            writer.WriteLine();
    }

    /// <inheritdoc/>
    public override void WriteLine(char value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    /// <inheritdoc/>
    public override void WriteLine(char[]? buffer)
    {
        foreach (var writer in _writers)
            writer.WriteLine(buffer);
    }

    /// <inheritdoc/>
    public override void WriteLine(char[] buffer, int index, int count)
    {
        foreach (var writer in _writers)
            writer.WriteLine(buffer, index, count);
    }

#if NET8_0_OR_GREATER
    /// <inheritdoc/>
    public override void WriteLine(ReadOnlySpan<char> buffer)
    {
        foreach (var writer in _writers)
            writer.WriteLine(buffer);
    }
#endif

    /// <inheritdoc/>
    public override void WriteLine(bool value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    /// <inheritdoc/>
    public override void WriteLine(int value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    /// <inheritdoc/>
    public override void WriteLine(uint value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    /// <inheritdoc/>
    public override void WriteLine(long value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    /// <inheritdoc/>
    public override void WriteLine(ulong value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    /// <inheritdoc/>
    public override void WriteLine(float value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    /// <inheritdoc/>
    public override void WriteLine(double value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    /// <inheritdoc/>
    public override void WriteLine(decimal value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    /// <inheritdoc/>
    public override void WriteLine(string? value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

#if NET8_0_OR_GREATER
    /// <inheritdoc/>
    public override void WriteLine(StringBuilder? value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }
#endif

    /// <inheritdoc/>
    public override void WriteLine(object? value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    /// <inheritdoc/>
    public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
    {
        foreach (var writer in _writers)
            writer.WriteLine(format, arg0);
    }

    /// <inheritdoc/>
    public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
    {
        foreach (var writer in _writers)
            writer.WriteLine(format, arg0, arg1);
    }

    /// <inheritdoc/>
    public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
    {
        foreach (var writer in _writers)
            writer.WriteLine(format, arg0, arg1, arg2);
    }

    /// <inheritdoc/>
    public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args)
    {
        foreach (var writer in _writers)
            writer.WriteLine(format, args);
    }


    /// <inheritdoc/>
    public override void Flush()
    {
        foreach (var writer in _writers)
            writer.Flush();
    }

    /// <inheritdoc/>
    public override void Close()
    {
        foreach (var writer in _writers)
            writer.Close();
    }
}
