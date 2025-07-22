using System;

namespace Matt.Util.CLI;

/// <summary>
/// Parses command-line parameters and it's values.
/// </summary>
/// <param name="prefix">The value of the command-line prefix to use.</param>
/// <param name="args">The value of the actual command-line arguments.</param>
public class CommandLineParser(string prefix, string[] args)
{
    /// <summary>
    /// Trys to find the specific parameter in the command-line arguments of the application.
    /// </summary>
    /// <param name="parm">The value of the parameter to look for.</param>
    /// <returns><see langword="true"/> if the parameter does exist in the command-line arguments.</returns>
    public bool FindParm(string parm)
    {
        if (!parm.StartsWith(prefix, StringComparison.Ordinal))
            parm = prefix + parm;

        // @imstilljustmatt:
        // if you don't want to line more extra lines, please remember that .NET Framework v4.7.2
        // doesn't have the StringComparison overload for "string.Contains()".
        parm = parm.ToLowerInvariant();

        foreach (var arg in args)
            if (arg.Contains(parm))
                return true;

        return false;
    }

    /// <summary>
    /// Finds the parameter and trys to get it's value.
    /// </summary>
    /// <param name="parm">The value of the parameter to look for.</param>
    /// <returns><see langword="null"/> if the parameter does not exist.</returns>
    public string? GetParmValue(string parm)
    {
        if (!parm.StartsWith(prefix, StringComparison.Ordinal))
            parm = prefix + parm;

        // @imstilljustmatt:
        // if you don't want to line more extra lines, please remember that .NET Framework v4.7.2
        // doesn't have the StringComparison overload for "string.Contains()".
        parm = parm.ToLowerInvariant();

        foreach (var arg in args)
            if (arg.Contains(parm))
                return arg.Substring(parm.Length).Trim([':', '=', ' ']);

        return null;
    }
}
