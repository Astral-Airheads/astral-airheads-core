using System;
using System.IO.Pipes;

namespace AstralAirheads.IPC.Host;

/// <summary>
/// Represents the main loop event's arguments.
/// </summary>
public class HostLoopEventArgs(string pipeName, NamedPipeClientStream stream) : EventArgs
{
    /// <summary>
    /// Gets the pipe name of the current IPC client and server.
    /// </summary>
    public string PipeName => pipeName;

    /// <summary>
    /// Gets the current client stream used for connecting to the server.
    /// </summary>
    public NamedPipeClientStream Client => stream;
}
