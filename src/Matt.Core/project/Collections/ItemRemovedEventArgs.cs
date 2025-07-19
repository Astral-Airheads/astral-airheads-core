using System;

namespace Matt.Collections;

/// <summary>
/// An event argument every time an item is added to the collection.
/// </summary>
/// <remarks>
/// Initializes a new <seealso cref="ItemRemovedEventArgs"/> with an specific type.
/// </remarks>
/// <param name="itemType"></param>
public class ItemRemovedEventArgs(object itemType) : EventArgs
{
    private object _type = itemType;

    /// <summary>
    /// The current type removed from the collection.
    /// </summary>
    public object Type => _type;
}
