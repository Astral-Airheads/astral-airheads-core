// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using AstralAirheads.Validation;

namespace AstralAirheads.AddIn;

/// <summary>
/// Just a simple add-in manager. :D
/// </summary>
public class AddInManager : IDisposable
{
    private bool _disposed = false;
    private readonly List<IAddInBase> _addins = [];

    /// <summary>
    /// Gets an loaded add-in by its name. (Not case-sensitive)
    /// </summary>
    /// <param name="name">The value of the add-in name to find.</param>
    /// <returns>The found add-in.</returns>
    public IAddInBase? GetAddInFromName(string name)
    {
        foreach (var addin in _addins)
        {
            if (!addin.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                continue;
                
            return addin;
        }
        return null;
    }

    /// <summary>
    /// Gets an loaded add-in by its prefix. (Not case-sensitive)
    /// </summary>
    /// <param name="prefix">The value of the add-in prefix to find.</param>
    /// <returns>The found add-in.</returns>
    public IAddInBase? GetAddInFromPrefix(string prefix)
    {
        foreach (var addin in _addins)
        {
            if (!addin.Prefix.Equals(prefix, StringComparison.OrdinalIgnoreCase))
                continue;
                
            return addin;
        }
        return null;
    }
    
    /// <summary>
    /// Loads an add-in from a class library (*.dll) file.
    /// </summary>
    /// <param name="dllPath">The value of the DLL path.</param>
    public void LoadAddInFromFile(string dllPath)
    {
        Requires.FileExists(dllPath);
        var dll = Assembly.LoadFrom(dllPath);
        Requires.NotNull(dll);

        var type = dll.GetTypes()
            .First(t => 
            typeof(IAddInBase).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        var addin = Activator.CreateInstance(type) as IAddInBase;
        Requires.NotNull(addin);

        LoadAddIn(addin);
    }

    /// <summary>
    /// Loads an add-in from an existing <seealso cref="IAddInBase"/>, whether
    /// from an DLL or whatever.
    /// </summary>
    /// <param name="addin">The value of the add-in class.</param>
    /// <exception cref="AddInException">Thrown when an add-in failed to initialize.</exception>
    public void LoadAddIn(IAddInBase addin)
    {
        Requires.NotNull(addin);

        if (!addin.Initialize())
            throw new AddInException(string.Format(ExcStrs.AddIn_FailedToInitialize, addin.Name));

        _addins.Add(addin);
    }

    /// <summary>
    /// Unloads an add-in if it's found from the loaded add-ins list.
    /// </summary>
    /// <param name="addin">The value of the add-in class.</param>
    public void UnloadAddIn(IAddInBase addin)
    {
        Requires.NotNull(addin);

        if (!_addins.Contains(addin))
            return;

        addin.Dispose();
        _addins.Remove(addin);
    }

    /// <summary>
    /// Performs a task that disposes every resources used by the loaded add-ins and this manager.
    /// </summary>
    /// <param name="disposing">The value of whether to actually dispose them or nah.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                foreach (var addin in _addins)
                    UnloadAddIn(addin);
            }
            _disposed = true;
        }
    }

    /// <summary>
    /// Performs a task that disposes every resources used by the loaded add-ins and this manager.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
