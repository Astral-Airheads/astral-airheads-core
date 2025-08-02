// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

namespace AstralAirheads.IPC;

/// <summary>
/// The default JSON message.
/// </summary>
public class DefaultPipeMessage(string message, int id = 1) : PipeMessage
{
    /// <summary>
    /// The ID number of the message.
    /// </summary>
    public override int Id { get; set; } = id;

    /// <summary>
    /// The actual string value of the message.
    /// </summary>
    public override string Message { get; set; } = message;
}
