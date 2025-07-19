using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Matt.Util;

namespace Matt.AddIn;

public class AddInManager : IDisposable
{
    private bool _disposed = false;
    private readonly List<IAddInBase> _addins = [];

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

    public void LoadAddInFromFile(string dllPath)
    {
        FileUtility.ThrowIfFileNotExist(dllPath);
        var dll = Assembly.LoadFrom(dllPath);
        ArgumentNullException.ThrowIfNull(dll);

        var type = dll.GetTypes()
            .First(t => 
            typeof(IAddInBase).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        var addin = Activator.CreateInstance(type) as IAddInBase;
        ArgumentNullException.ThrowIfNull(addin);

        LoadAddIn(addin);
    }

    public void LoadAddIn(IAddInBase addin)
    {
        ArgumentNullException.ThrowIfNull(addin);

        if (!addin.Initialize())
            throw new AddInException($"Add-in \"{addin.Name}\" failed to initialize!");

        _addins.Add(addin);
    }

    public void UnloadAddIn(IAddInBase addin)
    {
        ArgumentNullException.ThrowIfNull(addin);

        if (!_addins.Contains(addin))
            return;

        addin.Dispose();
        _addins.Remove(addin);
    }

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
