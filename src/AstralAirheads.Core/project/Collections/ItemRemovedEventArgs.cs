using System;

namespace AstralAirheads.Collections;

/// <summary>
/// An event argument every time an item is added to the collection.
/// </summary>
/// <remarks>
/// Initializes a new <seealso cref="ItemRemovedEventArgs"/> with an specific type.
/// </remarks>
/// <param name="itemType"></param>
public class ItemRemovedEventArgs(object itemType) : EventArgs
{
    /// <summary>
    /// The current type removed from the collection.
    /// </summary>
    public object Type => itemType;
}
