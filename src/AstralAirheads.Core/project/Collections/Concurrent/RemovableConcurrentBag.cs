// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AstralAirheads.Collections.Concurrent;

/// <summary>
/// *sigh*
/// A dumb implementation of <seealso cref="ConcurrentBag{T}"/>.
/// Too bad!
/// </summary>
public class RemovableConcurrentBag<T> : ConcurrentBag<T>
{
	// allow this to be overriden for no reason, maybe i could make use of this soon.
	/// <inheritdoc cref="ConcurrentBag{T}.Add(T)"/>
	public virtual new void Add(T item)
	{
		base.Add(item);
	}

    /// <summary>
    /// Removes an item from the <seealso cref="ConcurrentBag{T}"/>.
    /// </summary>
    /// <param name="item">The value of the item.</param>
    /// <returns><see langword="true"/></returns>
    public virtual bool Remove(T item)
    {
        var naFoHahaha = false; // see if na-FO yung item sa list.
        var tempList = new List<T>();
        while (TryTake(out T? removed))
        {
            if (removed == null)
                return false;

            if (!removed.Equals(item))
                tempList.Add(removed);
            else
                naFoHahaha = true; // oh no na-FO na sya, too bad!
        }

        return naFoHahaha;
    }
}
