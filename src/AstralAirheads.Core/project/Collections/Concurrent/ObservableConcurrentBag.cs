using System;
using System.Collections.Specialized;

namespace AstralAirheads.Collections.Concurrent;

/// <summary>
/// An observable <seealso cref="RemovableConcurrentBag{T}"/>.
/// </summary>
public class ObservableConcurrentBag<T> : RemovableConcurrentBag<T>, INotifyCollectionChanged where T : class
{
	/// <summary>
	/// An event to be invoked every time an item has been added to the collection.
	/// </summary>
	public event EventHandler<ItemAddedEventArgs>? ItemAdded;

	/// <summary>
	/// An event to be invoked every time an item has been removed from the collection.
	/// </summary>
	public event EventHandler<ItemRemovedEventArgs>? ItemRemoved;

	/// <inheritdoc/>
	public event NotifyCollectionChangedEventHandler? CollectionChanged;

	/// <inheritdoc/>
	public override void Add(T item)
	{
		base.Add(item);
		ItemAdded?.Invoke(this, new ItemAddedEventArgs(item));
		CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
	}

	/// <inheritdoc/>
	public override bool Remove(T item)
	{
		if (!base.Remove(item)) return false;

		ItemRemoved?.Invoke(this, new ItemRemovedEventArgs(item));
		CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
		return true;
	}
}
