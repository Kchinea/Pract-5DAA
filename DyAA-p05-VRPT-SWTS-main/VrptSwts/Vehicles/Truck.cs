namespace VrptSwts.Vehicles;

/// <summary>
/// Represents a basic truck with a fixed capacity, working duration, and speed.
/// Used as a base data model in the VRPT-SWTS system to describe the characteristics of a truck.
/// </summary>
internal class Truck(int capacity, int workDuration, int speed)
{
  /// <summary>
  /// Gets the maximum capacity of the truck (e.g., maximum load it can carry).
  /// </summary>
  public int Capacity { get; } = capacity;

  /// <summary>
  /// Gets the maximum time (in minutes) the truck is allowed to work.
  /// </summary>
  public int WorkDuration { get; } = workDuration;

  /// <summary>
  /// Gets the truck's speed (typically used to compute travel time between locations).
  /// </summary>
  public int Speed { get; } = speed;
}