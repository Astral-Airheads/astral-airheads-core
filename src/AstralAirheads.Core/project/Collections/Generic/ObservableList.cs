// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.Collections.Generic;

namespace AstralAirheads.Collections.Generic;

/// <summary>
/// An <seealso cref="List{T}"/> that invokes an event every time an item has been added to
/// or removed from the collection. Dumb am I right?
/// </summary>
/// <typeparam name="T">The value of the item type.</typeparam>
public class ObservableList<T> : List<T> where T : class
{
    /// <summary>
    /// An event to be invoked every time an item has been added to the collection.
    /// </summary>
    public event EventHandler<ItemAddedEventArgs>? ItemAdded;

    /// <summary>
    /// An event to be invoked every time an item has been removed from the collection.
    /// </summary>
    public event EventHandler<ItemRemovedEventArgs>? ItemRemoved;

    /// <inheritdoc cref="List{T}.Add(T)"/>
    public new void Add(T item)
    {
        base.Add(item);
        ItemAdded?.Invoke(this, new ItemAddedEventArgs(item));
    }

    /// <inheritdoc cref="List{T}.Remove(T)"/>
    public new bool Remove(T item)
    {
        if (!base.Remove(item)) return false;

        ItemRemoved?.Invoke(this, new ItemRemovedEventArgs(item));

        return true;
    }
}
