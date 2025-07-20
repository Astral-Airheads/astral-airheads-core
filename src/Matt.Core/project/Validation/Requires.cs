using System;
using System.IO;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Matt.Validation;

/// <summary>
/// Just a typical validation class.
/// </summary>
public sealed class Requires
{
    /// <summary>
    /// Throws an exception if an object is null.
    /// </summary>
    /// <param name="value">The value of the object.</param>
    /// <param name="paramName">The value of the object's name.</param>
    /// <exception cref="ArgumentNullException">Throws when the object is returned as null.</exception>
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
    /// <exception cref="ArgumentNullException">Throws when the object is returned as null.</exception>
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
    /// <exception cref="ArgumentNullException">Throws when the object is returned as null.</exception>
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
    /// <exception cref="ArgumentNullException">Throws when the object is returned as null.</exception>
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
    /// <exception cref="ArgumentNullException">Throws when the pointer is <seealso cref="IntPtr.Zero"/>.</exception>
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
    /// <exception cref="ArgumentNullException">Throws when the object is returned as null.</exception>
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
    /// <exception cref="ArgumentNullException">Throws when the object is returned as null.</exception>
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
    /// <exception cref="ArgumentNullException">Throws when the string is null or an empty whitespace.</exception>
    public static void NotNullOrWhitespace([NotNull] string? value, string? paramName = null)
    {
        NotNull(value, nameof(value));

        if (!string.IsNullOrWhiteSpace(value)) return;

        throw new ArgumentNullException(paramName ?? nameof(value),
            string.Format(ExcStrs.Validation_ValueMustNotBeNull, paramName ?? nameof(value)));
    }

    /// <summary>
    /// Throws an exception if the specified file doesn't exist.
    /// </summary>
    /// <param name="filePath">The value of the file path.</param>
    /// <param name="fileName">The value of the file name.</param>
    /// <exception cref="FileNotFoundException">Throws when the file doesn't exist.</exception>
    [DebuggerStepThrough]
    public static void FileExists(string filePath, string? fileName = null)
    {
        if (File.Exists(filePath)) return;

        throw new FileNotFoundException(
            string.Format(ExcStrs.Validation_FileDoesNotExist, fileName ?? Path.GetFileName(filePath)));
    }
}
