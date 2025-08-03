// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.Collections.Generic;

namespace AstralAirheads.Caching;

/// <summary>
/// Provides a flexible caching system for storing and retrieving data.
/// </summary>
/// <typeparam name="TKey">The type of the cache key.</typeparam>
/// <typeparam name="TValue">The type of the cached value.</typeparam>
public interface ICacheProvider<TKey, TValue> where TKey : notnull
{
	/// <summary>
	/// Gets the number of items in the cache.
	/// </summary>
	int Count { get; }

	/// <summary>
	/// Gets or sets the maximum number of items the cache can hold.
	/// </summary>
	int Capacity { get; set; }

	/// <summary>
	/// Gets a value from the cache.
	/// </summary>
	/// <param name="key">The cache key.</param>
	/// <param name="value">The cached value if found.</param>
	/// <returns>True if the value was found, false otherwise.</returns>
	bool TryGet(TKey key, out TValue? value);

	/// <summary>
	/// Gets a value from the cache, or adds it if it doesn't exist.
	/// </summary>
	/// <param name="key">The cache key.</param>
	/// <param name="factory">The factory function to create the value if not found.</param>
	/// <returns>The cached or newly created value.</returns>
	TValue GetOrAdd(TKey key, Func<TKey, TValue> factory);

	/// <summary>
	/// Adds or updates a value in the cache.
	/// </summary>
	/// <param name="key">The cache key.</param>
	/// <param name="value">The value to cache.</param>
	void Set(TKey key, TValue value);

	/// <summary>
	/// Adds or updates a value in the cache with an expiration time.
	/// </summary>
	/// <param name="key">The cache key.</param>
	/// <param name="value">The value to cache.</param>
	/// <param name="expiration">The time when the cache entry expires.</param>
	void Set(TKey key, TValue value, DateTime expiration);

	/// <summary>
	/// Adds or updates a value in the cache with a time-to-live duration.
	/// </summary>
	/// <param name="key">The cache key.</param>
	/// <param name="value">The value to cache.</param>
	/// <param name="timeToLive">The duration the cache entry should live.</param>
	void Set(TKey key, TValue value, TimeSpan timeToLive);

	/// <summary>
	/// Removes a value from the cache.
	/// </summary>
	/// <param name="key">The cache key.</param>
	/// <returns>True if the value was removed, false if it didn't exist.</returns>
	bool Remove(TKey key);

	/// <summary>
	/// Checks if a key exists in the cache.
	/// </summary>
	/// <param name="key">The cache key.</param>
	/// <returns>True if the key exists, false otherwise.</returns>
	bool Contains(TKey key);

	/// <summary>
	/// Clears all items from the cache.
	/// </summary>
	void Clear();

	/// <summary>
	/// Gets all keys in the cache.
	/// </summary>
	/// <returns>An enumerable of all cache keys.</returns>
	IEnumerable<TKey> GetKeys();

	/// <summary>
	/// Removes expired items from the cache.
	/// </summary>
	/// <returns>The number of items removed.</returns>
	int RemoveExpired();
}
 