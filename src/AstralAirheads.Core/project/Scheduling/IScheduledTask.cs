using System;
using System.Threading;
using System.Threading.Tasks;

namespace AstralAirheads.Scheduling;

/// <summary>
/// Represents a scheduled task that can be executed at specified intervals.
/// </summary>
public interface IScheduledTask
{
	/// <summary>
	/// Gets the unique identifier for this scheduled task.
	/// </summary>
	string Id { get; }

	/// <summary>
	/// Gets the name of this scheduled task.
	/// </summary>
	string Name { get; }

	/// <summary>
	/// Gets the current status of this scheduled task.
	/// </summary>
	TaskStatus Status { get; }

	/// <summary>
	/// Gets the next execution time for this scheduled task.
	/// </summary>
	DateTime? NextExecutionTime { get; }

	/// <summary>
	/// Gets the last execution time for this scheduled task.
	/// </summary>
	DateTime? LastExecutionTime { get; }

	/// <summary>
	/// Gets the number of times this task has been executed.
	/// </summary>
	long ExecutionCount { get; }

	/// <summary>
	/// Gets the schedule for this task.
	/// </summary>
	ISchedule Schedule { get; }

	/// <summary>
	/// Starts the scheduled task.
	/// </summary>
	/// <param name="cancellationToken">Optional cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task StartAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Stops the scheduled task.
	/// </summary>
	/// <param name="cancellationToken">Optional cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task StopAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Executes the task immediately, regardless of the schedule.
	/// </summary>
	/// <param name="cancellationToken">Optional cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task ExecuteAsync(CancellationToken cancellationToken = default);
}
