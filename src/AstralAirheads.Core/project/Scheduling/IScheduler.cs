// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AstralAirheads.Scheduling;

/// <summary>
/// Provides a task scheduling system for executing tasks at specified intervals.
/// </summary>
public interface IScheduler
{
	/// <summary>
	/// Gets the number of scheduled tasks.
	/// </summary>
	int TaskCount { get; }

	/// <summary>
	/// Schedules a task to be executed according to the specified schedule.
	/// </summary>
	/// <param name="id">The unique identifier for the task.</param>
	/// <param name="name">The name of the task.</param>
	/// <param name="action">The action to execute.</param>
	/// <param name="schedule">The schedule for task execution.</param>
	/// <param name="autoStart">Whether to start the task immediately.</param>
	/// <returns>The scheduled task.</returns>
	IScheduledTask ScheduleTask(string id, string name, Func<CancellationToken, Task> action, ISchedule schedule, bool autoStart = true);

	/// <summary>
	/// Schedules a task to be executed at a specific time.
	/// </summary>
	/// <param name="id">The unique identifier for the task.</param>
	/// <param name="name">The name of the task.</param>
	/// <param name="action">The action to execute.</param>
	/// <param name="executionTime">The time when the task should be executed.</param>
	/// <param name="autoStart">Whether to start the task immediately.</param>
	/// <returns>The scheduled task.</returns>
	IScheduledTask ScheduleTask(string id, string name, Func<CancellationToken, Task> action, DateTime executionTime, bool autoStart = true);

	/// <summary>
	/// Schedules a task to be executed at regular intervals.
	/// </summary>
	/// <param name="id">The unique identifier for the task.</param>
	/// <param name="name">The name of the task.</param>
	/// <param name="action">The action to execute.</param>
	/// <param name="interval">The interval between executions.</param>
	/// <param name="autoStart">Whether to start the task immediately.</param>
	/// <returns>The scheduled task.</returns>
	IScheduledTask ScheduleTask(string id, string name, Func<CancellationToken, Task> action, TimeSpan interval, bool autoStart = true);

	/// <summary>
	/// Gets a scheduled task by its identifier.
	/// </summary>
	/// <param name="id">The task identifier.</param>
	/// <returns>The scheduled task, or null if not found.</returns>
	IScheduledTask? GetTask(string id);

	/// <summary>
	/// Gets all scheduled tasks.
	/// </summary>
	/// <returns>An enumerable of all scheduled tasks.</returns>
	IEnumerable<IScheduledTask> GetAllTasks();

	/// <summary>
	/// Removes a scheduled task.
	/// </summary>
	/// <param name="id">The task identifier.</param>
	/// <returns>True if the task was removed, false if it didn't exist.</returns>
	bool RemoveTask(string id);

	/// <summary>
	/// Starts all scheduled tasks.
	/// </summary>
	/// <param name="cancellationToken">Optional cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task StartAllAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Stops all scheduled tasks.
	/// </summary>
	/// <param name="cancellationToken">Optional cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task StopAllAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Clears all scheduled tasks.
	/// </summary>
	void Clear();
}
 