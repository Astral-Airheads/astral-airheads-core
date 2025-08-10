// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

namespace AstralAirheads.Util;

/// <summary>
/// Types of security hashes.
/// 
/// Hash generator methods from https://gist.github.com/rmacfie/828054/a2ed7ed1c023fbb7781e8f11ac69f2b0d780a717.
/// </summary>
public enum HashType
{
	/// <summary>
	/// MD5
	/// </summary>
	Md5,

	/// <summary>
	/// SHA-1
	/// </summary>
	Sha1,

	/// <summary>
	/// SHA-256
	/// </summary>
	Sha256,

	/// <summary>
	/// SHA-384
	/// </summary>
	Sha384,
	
	/// <summary>
	/// SHA-512
	/// </summary>
	Sha512
}
