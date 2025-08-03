// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;

namespace AstralAirheads.Util.CLI;

/// <summary>
/// Parses command-line parameters and it's values.
/// </summary>
/// <param name="prefix">The value of the command-line prefix to use.</param>
/// <param name="args">The value of the actual command-line arguments.</param>
public class CommandLineParser(char prefix, string[] args)
{
    /// <summary>
    /// Tries to find the specific parameter in the command-line arguments of the application.
    /// </summary>
    /// <param name="parameter">The value of the parameter to look for.</param>
    /// <returns><see langword="true"/> if the parameter does exist in the command-line arguments.</returns>
    public bool FindParm(string parameter)
    {
        if (!parameter.StartsWith(prefix.ToString(), StringComparison.Ordinal))
            parameter = prefix + parameter;

        // @imstilljustmatt:
        // if you don't want to line more extra lines by using arg.Contains(string, StringComparison),
		// please remember that .NET Framework v4.7.2 doesn't have the StringComparison overload
		// for "string.Contains()".
        parameter = parameter.ToLowerInvariant();

        foreach (var arg in args)
            if (arg.Contains(parameter))
                return true;

        return false;
    }

    /// <summary>
    /// Finds the parameter and tries to get it's value.
    /// </summary>
    /// <param name="parameter">The value of the parameter to look for.</param>
    /// <returns><see langword="null"/> if the parameter does not exist.</returns>
    public string? GetParmValue(string parameter)
    {
        if (!parameter.StartsWith(prefix.ToString(), StringComparison.Ordinal))
            parameter = prefix + parameter;

		// @imstilljustmatt:
		// if you don't want to line more extra lines by using arg.Contains(string, StringComparison),
		// please remember that .NET Framework v4.7.2 doesn't have the StringComparison overload
		// for "string.Contains()".
		parameter = parameter.ToLowerInvariant();

        foreach (var arg in args)
            if (arg.Contains(parameter))
                return arg.Substring(parameter.Length).Trim([':', '=', ' ']);

        return null;
    }
}
