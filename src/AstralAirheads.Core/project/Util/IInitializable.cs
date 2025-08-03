// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;

namespace AstralAirheads.Util;

/// <summary>
/// Represents an interface that could be initialized with an boolean function.
/// </summary>
public interface IInitializable : IDisposable
{
	/// <summary>
	/// Initializes <seealso cref="IInitializable"/>.
	/// </summary>
	/// <returns><see langword="true"/> on successful initialization, <see langword="false"/> on fail.</returns>
	bool Initialize();

	/// <summary>
	/// Specifies if this class is currently initialized.
	/// </summary>
	/// <returns><see langword="true"/> if it is actually initialized, <see langword="false"/> if it hasn't. (duh!)</returns>
	bool IsInitialized();
}
