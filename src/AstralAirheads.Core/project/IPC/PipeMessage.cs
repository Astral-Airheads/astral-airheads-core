// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.Text.Json.Serialization;

namespace AstralAirheads.IPC;

/// <summary>
/// Represents the base of a JSON message.
/// </summary>
public abstract class PipeMessage
{
    /// <summary>
    /// The ID number of the message.
    /// </summary>
    [JsonPropertyName("id")]
    public abstract int Id { get; set; }

    /// <summary>
    /// The actual value of the message.
    /// </summary>
    public abstract string Message { get; set; }
}
