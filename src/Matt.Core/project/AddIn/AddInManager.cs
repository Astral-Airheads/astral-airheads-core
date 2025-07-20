using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Matt.Validation;

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

    public void LoadAddIn(IAddInBase addin)
    {
        Requires.NotNull(addin);

        if (!addin.Initialize())
            throw new AddInException($"Add-in \"{addin.Name}\" failed to initialize!");

        _addins.Add(addin);
    }

    public void UnloadAddIn(IAddInBase addin)
    {
        Requires.NotNull(addin);

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
