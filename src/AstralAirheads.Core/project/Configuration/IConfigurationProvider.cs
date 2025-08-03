// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System.Collections.Generic;

namespace AstralAirheads.Configuration;

/// <summary>
/// Provides a flexible configuration system for application settings.
/// </summary>
public interface IConfigurationProvider
{
	/// <summary>
	/// Gets a configuration value by key.
	/// </summary>
	/// <param name="key">The configuration key.</param>
	/// <returns>The configuration value, or null if not found.</returns>
	string? GetValue(string key);

	/// <summary>
	/// Gets a configuration value by key with a default value.
	/// </summary>
	/// <param name="key">The configuration key.</param>
	/// <param name="defaultValue">The default value if key is not found.</param>
	/// <returns>The configuration value or the default value.</returns>
	string GetValue(string key, string defaultValue);

	/// <summary>
	/// Gets a typed configuration value.
	/// </summary>
	/// <typeparam name="T">The type to convert the value to.</typeparam>
	/// <param name="key">The configuration key.</param>
	/// <param name="defaultValue">The default value if key is not found.</param>
	/// <returns>The typed configuration value or the default value.</returns>
	T GetValue<T>(string key, T defaultValue);

	/// <summary>
	/// Sets a configuration value.
	/// </summary>
	/// <param name="key">The configuration key.</param>
	/// <param name="value">The configuration value.</param>
	void SetValue(string key, string value);

	/// <summary>
	/// Checks if a configuration key exists.
	/// </summary>
	/// <param name="key">The configuration key.</param>
	/// <returns>True if the key exists, false otherwise.</returns>
	bool HasKey(string key);

	/// <summary>
	/// Gets all configuration keys.
	/// </summary>
	/// <returns>An enumerable of all configuration keys.</returns>
	IEnumerable<string> GetKeys();

	/// <summary>
	/// Reloads the configuration from the source.
	/// </summary>
	void Reload();

	/// <summary>
	/// Saves the current configuration to the source.
	/// </summary>
	void Save();
}
 