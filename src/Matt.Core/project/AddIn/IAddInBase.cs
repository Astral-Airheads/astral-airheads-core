using System;

namespace Matt.AddIn;

/// <summary>
/// Represents the base of the add-in that can be initialized or disposed by <seealso cref="AddInManager"/>.
/// </summary>
public interface IAddInBase : IDisposable
{
    /// <summary>
    /// The name of the add-in.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The prefix of the add-in.
    /// </summary>
    string Prefix { get; }

    /// <summary>
    /// Author of the add-in.
    /// </summary>
    string Author { get; }

    /// <summary>
    /// Optional description of the add-in.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// The version of the add-in.
    /// </summary>
    Version Version { get; }

    /// <summary>
    /// Where most add-in stuff are initialized.
    /// </summary>
    /// <returns></returns>
    bool Initialize();
}
