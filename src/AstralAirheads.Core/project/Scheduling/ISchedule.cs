using System;

namespace AstralAirheads.Scheduling;

/// <summary>
/// Defines a schedule for task execution.
/// </summary>
public interface ISchedule
{
	/// <summary>
	/// Calculates the next execution time based on the current time.
	/// </summary>
	/// <param name="from">The reference time to calculate from.</param>
	/// <returns>The next execution time, or null if no more executions are scheduled.</returns>
	DateTime? GetNextExecutionTime(DateTime from);

	/// <summary>
	/// Gets a description of this schedule.
	/// </summary>
	/// <returns>A human-readable description of the schedule.</returns>
	string GetDescription();
}
