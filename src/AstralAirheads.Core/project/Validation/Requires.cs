// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Copyright (c) 2025 Microsoft, all rights reserved.
// Some existing validation methods are from the Microsoft.VisualStudio.Validation library.
//=====================================================================================
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.IO;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace AstralAirheads.Validation;

/// <summary>
/// Just a typical validation class. You can use this for assertion.
/// </summary>
public sealed class Requires
{
    /// <summary>
    /// Throws an exception if an object is null.
    /// </summary>
    /// <param name="value">The value of the object.</param>
    /// <param name="paramName">The value of the object's name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the object is returned as null.</exception>
    [DebuggerStepThrough]
    public static void NotNull([NotNull] object? value, string? paramName = null)
    {
        if (value != null) return;

        throw new ArgumentNullException(paramName ?? nameof(value),
            string.Format(ExcStrs.Validation_ValueMustNotBeNull, paramName ?? nameof(value)));
    }

    /// <summary>
    /// Throws an exception if an object is null.
    /// </summary>
    /// <param name="value">The value of the object.</param>
    /// <param name="paramName">The value of the object's name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the object is returned as null.</exception>
    [DebuggerStepThrough]
    public static void NotNull([NotNull] bool? value, string? paramName = null)
    {
        if (value != null) return;

        throw new ArgumentNullException(paramName ?? nameof(value),
            string.Format(ExcStrs.Validation_ValueMustNotBeNull, paramName ?? nameof(value)));
    }

    /// <summary>
    /// Throws an exception if an object is null.
    /// </summary>
    /// <param name="value">The value of the object.</param>
    /// <param name="paramName">The value of the object's name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the object is returned as null.</exception>
    [DebuggerStepThrough]
    public static void NotNull([NotNull] int? value, string? paramName = null)
    {
        if (value != null) return;

        throw new ArgumentNullException(paramName ?? nameof(value),
            string.Format(ExcStrs.Validation_ValueMustNotBeNull, paramName ?? nameof(value)));
    }

    /// <summary>
    /// Throws an exception if an object is null.
    /// </summary>
    /// <param name="value">The value of the object.</param>
    /// <param name="paramName">The value of the object's name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the object is returned as null.</exception>
    [DebuggerStepThrough]
    public static void NotNull([NotNull] float? value, string? paramName = null)
    {
        if (value != null) return;

        throw new ArgumentNullException(paramName ?? nameof(value),
            string.Format(ExcStrs.Validation_ValueMustNotBeNull, paramName ?? nameof(value)));
    }

    /// <summary>
    /// Throws an exception if the pointer is <seealso cref="IntPtr.Zero"/>.
    /// </summary>
    /// <param name="value">The value of the object.</param>
    /// <param name="paramName">The value of the object's name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the pointer is <seealso cref="IntPtr.Zero"/>.</exception>
    [DebuggerStepThrough]
    public static void NotNull([NotNull] IntPtr value, string? paramName = null)
    {
        if (value != IntPtr.Zero) return;

        throw new ArgumentNullException(paramName ?? nameof(value),
            string.Format(ExcStrs.Validation_ValueMustNotBeNull, paramName ?? nameof(value)));
    }

    /// <summary>
    /// Throws an exception if an object is null.
    /// </summary>
    /// <param name="value">The value of the object.</param>
    /// <param name="paramName">The value of the object's name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the object is returned as null.</exception>
    [DebuggerStepThrough]
    public static void NotNull([NotNull] string? value, string? paramName = null)
    {
        if (value != null) return;

        throw new ArgumentNullException(paramName ?? nameof(value),
            string.Format(ExcStrs.Validation_ValueMustNotBeNull, paramName ?? nameof(value)));
    }

    /// <summary>
    /// Throws an exception if an object is null.
    /// </summary>
    /// <param name="value">The value of the object.</param>
    /// <param name="paramName">The value of the object's name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the object is returned as null.</exception>
    [DebuggerStepThrough]
    public static void NotNull<T>([NotNull] T? value, string? paramName = null) where T : class, new()
    {
        if (value != null) return;

        throw new ArgumentNullException(paramName ?? nameof(value),
            string.Format(ExcStrs.Validation_ValueMustNotBeNull, paramName ?? nameof(value)));
    }

    /// <summary>
    /// Throws an exception if an string is null or an empty whitespace.
    /// </summary>
    /// <param name="value">The value of the object.</param>
    /// <param name="paramName">The value of the object's name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the string is null or an empty whitespace.</exception>
    public static void NotNullOrWhitespace([NotNull] string? value, string? paramName = null)
    {
        NotNull(value, nameof(value));

        if (!string.IsNullOrWhiteSpace(value)) return;

        throw new ArgumentNullException(paramName ?? nameof(value),
            string.Format(ExcStrs.Validation_ValueMustNotBeNull, paramName ?? nameof(value)));
    }

    /// <summary>
    /// Throws an exception if an string is null or empty.
    /// </summary>
    /// <param name="value">The value of the object.</param>
    /// <param name="paramName">The value of the object's name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the string is null or an empty whitespace.</exception>
    public static void NotNullOrEmpty([NotNull] string? value, string? paramName = null)
    {
        NotNull(value, nameof(value));

        if (!string.IsNullOrEmpty(value)) return;

        throw new ArgumentNullException(paramName ?? nameof(value),
            string.Format(ExcStrs.Validation_ValueMustNotBeNull, paramName ?? nameof(value)));
    }

    /// <summary>
    /// Throws an exception if an collection is null or empty.
    /// </summary>
    /// <param name="values">The value of the object.</param>
    /// <param name="paramName">The value of the object's name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the collection is null or an empty whitespace.</exception>
    public static void NotNullOrEmpty<T>([NotNull] IEnumerable<T>? values, string? paramName = null)
    {
        NotNull(values, nameof(values));

		if (values.First() != null) return;

		bool isEmpty;
		if (values is ICollection<T> collection)
		{
			isEmpty = collection.Count == 0;
		}
		else if (values is IReadOnlyCollection<T> readOnlyCollection)
		{
			isEmpty = readOnlyCollection.Count == 0;
		}
		else
		{
			using IEnumerator<T> enumerator = values.GetEnumerator();
			isEmpty = !enumerator.MoveNext();
		}

		if (isEmpty)
		{
			throw new ArgumentNullException(paramName ?? nameof(values),
			string.Format(ExcStrs.Validation_ValueMustNotBeNull, paramName ?? nameof(values)));
		}
    }

    /// <summary>
    /// Throws an exception if the specified file doesn't exist.
    /// </summary>
    /// <param name="filePath">The value of the file path.</param>
    /// <param name="fileName">The value of the file name.</param>
    /// <exception cref="FileNotFoundException">Thrown when the file doesn't exist.</exception>
    [DebuggerStepThrough]
    public static void FileExists(string filePath, string? fileName = null)
    {
        if (File.Exists(filePath)) return;

        throw new FileNotFoundException(
            string.Format(ExcStrs.Validation_FileDoesNotExist, fileName ?? Path.GetFileName(filePath)));
    }

	/// <summary>
	/// Throws an exception if the specified <see langword="int"/> value is not in range in between
	/// the specified minimum and maximum value.
	/// </summary>
	/// <param name="actual">The value of the actual value.</param>
	/// <param name="min">The value of the minimum value.</param>
	/// <param name="max">The value of the maximum value.</param>
	/// <param name="paramName">The value of the object's name.</param>
	[DebuggerStepThrough]
	public static void MustBeInRange(int actual, int min, int max, string? paramName = null)
	{
		if (actual < max) return;
		if (actual > min) return;

		throw new ArgumentOutOfRangeException(paramName ?? nameof(actual));
	}

	/// <summary>
	/// Throws an exception if the specified <see langword="float"/> value is not in range in between
	/// the specified minimum and maximum value.
	/// </summary>
	/// <param name="actual">The value of the actual value.</param>
	/// <param name="min">The value of the minimum value.</param>
	/// <param name="max">The value of the maximum value.</param>
	/// <param name="paramName">The value of the object's name.</param>
	[DebuggerStepThrough]
	public static void MustBeInRange(float actual, float min, float max, string? paramName = null)
	{
		if (actual < max) return;
		if (actual > min) return;

		throw new ArgumentOutOfRangeException(paramName ?? nameof(actual));
	}

	/// <summary>
	/// Throws an exception if the specified <see langword="double"/> value is not in range in between
	/// the specified minimum and maximum value.
	/// </summary>
	/// <param name="actual">The value of the actual value.</param>
	/// <param name="min">The value of the minimum value.</param>
	/// <param name="max">The value of the maximum value.</param>
	/// <param name="paramName">The value of the object's name.</param>
	[DebuggerStepThrough]
	public static void MustBeInRange(double actual, double min, double max, string? paramName = null)
	{
		if (actual < max) return;
		if (actual > min) return;

		throw new ArgumentOutOfRangeException(paramName ?? nameof(actual));
	}

	/// <summary>
	/// Throws an exception if the specified <see langword="long"/> value is not in range in between
	/// the specified minimum and maximum value.
	/// </summary>
	/// <param name="actual">The value of the actual value.</param>
	/// <param name="min">The value of the minimum value.</param>
	/// <param name="max">The value of the maximum value.</param>
	/// <param name="paramName">The value of the object's name.</param>
	[DebuggerStepThrough]
	public static void MustBeInRange(long actual, long min, long max, string? paramName = null)
	{
		if (actual < max) return;
		if (actual > min) return;

		throw new ArgumentOutOfRangeException(paramName ?? nameof(actual));
	}

	/// <summary>
	/// Throws an exception if the specified <see langword="uint"/> value is not in range in between
	/// the specified minimum and maximum value.
	/// </summary>
	/// <param name="actual">The value of the actual value.</param>
	/// <param name="min">The value of the minimum value.</param>
	/// <param name="max">The value of the maximum value.</param>
	/// <param name="paramName">The value of the object's name.</param>
	[DebuggerStepThrough]
	public static void MustBeInRange(uint actual, uint min, uint max, string? paramName = null)
	{
		if (actual < max) return;
		if (actual > min) return;

		throw new ArgumentOutOfRangeException(paramName ?? nameof(actual));
	}

	/// <summary>
	/// Throws an exception if the specified <see langword="nint"/> value is not in range in between
	/// the specified minimum and maximum value.
	/// </summary>
	/// <param name="actual">The value of the actual value.</param>
	/// <param name="min">The value of the minimum value.</param>
	/// <param name="max">The value of the maximum value.</param>
	/// <param name="paramName">The value of the object's name.</param>
	[DebuggerStepThrough]
	public static void MustBeInRange(nint actual, nint min, nint max, string? paramName = null)
	{
		if (actual < max) return;
		if (actual > min) return;

		throw new ArgumentOutOfRangeException(paramName ?? nameof(actual));
	}

    /// <summary>
    /// Throws an exception if the specified objects are not equal to eachother.
    /// </summary>
    /// <param name="actual">The value of the actual object.</param>
    /// <param name="expected">The value of the expected object.</param>
    /// <exception cref="MustBeEqualException">Thrown when both of the objects are not equal to eachother.</exception>
    [DebuggerStepThrough]
    public static void MustBeEqual(object actual, object expected)
    {
        if (actual.Equals(expected)) return;

        throw new MustBeEqualException($"Object {nameof(actual)} must be equal to expected parameter: {nameof(expected)}.");
    }

    /// <summary>
    /// Throws an exception if the specified strings are not equal to eachother.
    /// </summary>
    /// <param name="actual">The value of the actual object.</param>
    /// <param name="expected">The value of the expected object.</param>
    /// <exception cref="MustBeEqualException">Thrown when both of the objects are not equal to eachother.</exception>
    [DebuggerStepThrough]
    public static void MustBeEqual(string actual, string expected)
    {
        if (actual.Equals(expected)) return;

        throw new MustBeEqualException($"String {nameof(actual)} must be equal to expected parameter: {nameof(expected)}.");
    }

    /// <summary>
    /// Throws an exception if the specified objects are not equal to eachother.
    /// </summary>
    /// <param name="actual">The value of the actual object.</param>
    /// <param name="expected">The value of the expected object.</param>
    /// <exception cref="MustBeEqualException">Thrown when both of the objects are not equal to eachother.</exception>
    [DebuggerStepThrough]
    public static void MustBeEqual<T>(T actual, T expected) where T : class, IEquatable<T>
    {
        if (actual.Equals(expected)) return;

        throw new MustBeEqualException($"{typeof(T).Name} {nameof(actual)} must be equal to expected parameter: {nameof(expected)}.");
    }

	/// <summary>
	/// Throws an exception if the specified boolean is not equal to the value of <see langword="true"/>.
	/// </summary>
	/// <param name="condition">The value of the condition.</param>
	/// <param name="paramName">The value of the object's name.</param>
	public static void True(bool condition, string? paramName = null)
	{
		if (string.IsNullOrEmpty(paramName)) paramName = nameof(condition);
		if (condition) return;

		throw new ArgumentNullException(paramName, $"The condition {paramName} must return true.");
	}

	/// <summary>
	/// Throws an exception if the specified boolean is not equal to the value of <see langword="false"/>.
	/// </summary>
	/// <param name="condition">The value of the condition.</param>
	/// <param name="paramName">The value of the object's name.</param>
	public static void False(bool condition, string? paramName = null)
	{
		if (string.IsNullOrEmpty(paramName)) paramName = nameof(condition);
		if (!condition) return;

		throw new ArgumentNullException(paramName, $"The condition {paramName} must return false.");
	}
}
