using System;
using System.Diagnostics;
using System.IO;

namespace Matt.Util;

public sealed class FileUtility
{
    [DebuggerStepThrough]
    public static void ThrowIfFileNotExist(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"The searched file \"{Path.GetFileName(filePath)}\" doesn't exist.");
    }
}
