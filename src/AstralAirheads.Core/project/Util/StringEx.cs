// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.Text;
using System.Security.Cryptography;
using AstralAirheads.Validation;

namespace AstralAirheads.Util;

/// <summary>
/// Contains extensions for <see langword="string" />.
/// 
/// Hash generator methods from https://gist.github.com/rmacfie/828054/a2ed7ed1c023fbb7781e8f11ac69f2b0d780a717.
/// </summary>
public static class StringEx
{
	/// <summary>
	/// Calculates the MD5 hash for the given string.
	/// </summary>
	/// <returns>A 32 char long hash.</returns>
	/// <exception cref="ArgumentNullException">Throws if <paramref name="input"/> is null or empty.</exception>
	public static string GetHashMd5(this string input) =>
		ComputeHash(HashType.Md5, input);

	/// <summary>
	/// Calculates the SHA-1 hash for the given string.
	/// </summary>
	/// <returns>A 40 char long hash.</returns>
	/// <exception cref="ArgumentNullException">Throws if <paramref name="input"/> is null or empty.</exception>
	public static string GetHashSha1(this string input) =>
		ComputeHash(HashType.Sha1, input);

	/// <summary>
	/// Calculates the SHA-256 hash for the given string.
	/// </summary>
	/// <returns>A 64 char long hash.</returns>
	/// <exception cref="ArgumentNullException">Throws if <paramref name="input"/> is null or empty.</exception>
	public static string GetHashSha256(this string input) =>
		ComputeHash(HashType.Sha256, input);

	/// <summary>
	/// Calculates the SHA-384 hash for the given string.
	/// </summary>
	/// <returns>A 96 char long hash.</returns>
	/// <exception cref="ArgumentNullException">Throws if <paramref name="input"/> is null or empty.</exception>
	public static string GetHashSha384(this string input) =>
		ComputeHash(HashType.Sha384, input);

	/// <summary>
	/// Calculates the SHA-512 hash for the given string.
	/// </summary>
	/// <returns>A 128 char long hash.</returns>
	/// <exception cref="ArgumentNullException">Throws if <paramref name="input"/> is null or empty.</exception>
	public static string GetHashSha512(this string input) => 
		ComputeHash(HashType.Sha512, input);

	/// <summary>
	/// Computes a normal string into a hash string specified by a type of hash to be encrypted.
	/// </summary>
	/// <param name="hashType">The value of the hash type.</param>
	/// <param name="input">The value of the normal string.</param>
	/// <returns>The encrypted hash string.</returns>
	/// <exception cref="ArgumentNullException">Throws if <paramref name="input"/> is null or empty.</exception>
	public static string ComputeHash(HashType hashType, string input)
	{
		Requires.NotNullOrEmpty(input);

		var hasher = GetHasher(hashType);
		var inputBytes = Encoding.UTF8.GetBytes(input);

		var hashBytes = hasher.ComputeHash(inputBytes);
		var hash = new StringBuilder();
		foreach (var b in hashBytes)
		{
			hash.Append(string.Format("{0:x2}", b));
		}

		return hash.ToString();
	}

	private static HashAlgorithm GetHasher(HashType hashType) =>
		hashType switch
		{
			HashType.Md5 => MD5.Create(),
			HashType.Sha1 => SHA1.Create(),
			HashType.Sha256 => SHA256.Create(),
			HashType.Sha384 => SHA384.Create(),
			HashType.Sha512 => SHA512.Create(),
			_ => throw new ArgumentOutOfRangeException(nameof(hashType)),
		};
}
