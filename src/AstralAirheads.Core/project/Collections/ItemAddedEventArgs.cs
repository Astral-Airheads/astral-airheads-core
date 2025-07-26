using System;

namespace AstralAirheads.Collections;

/// <summary>
/// An event argument every time an item is added to the collection.
/// </summary>
/// <remarks>
/// Initializes a new <seealso cref="ItemAddedEventArgs"/> with an specific type.
/// </remarks>
/// <param name="itemType"></param>
public class ItemAddedEventArgs(object itemType) : EventArgs
{
    /// <summary>
    /// The current type added to the collection.
    /// </summary>
    public object Type => itemType;
}
