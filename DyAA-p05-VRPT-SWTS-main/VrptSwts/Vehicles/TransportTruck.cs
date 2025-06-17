using VrptSwts.Graph;
using VrptSwts.InstanceManagement;

namespace VrptSwts.Vehicles;

/// <summary>
/// Represents a transport truck used to perform a sequence of transport tasks in the SWTS VRP context.
/// Tracks the truck's current capacity, remaining available time, the list of transport tasks assigned,
/// and the total time it has worked.
/// </summary>
internal class TransportTruck(int currentCapacity, double remainingTime, List<TransportTask> route)
{
  /// <summary>
  /// Gets or sets the current capacity of the truck (e.g., how much waste it is currently carrying).
  /// </summary>
  public int CurrentCapacity { get; set; } = currentCapacity;

  /// <summary>
  /// Gets or sets the remaining time available for the truck to perform tasks (in minutes).
  /// </summary>
  public double RemainingTime { get; set; } = remainingTime;

  /// <summary>
  /// Gets or sets the list of transport tasks assigned to the truck.
  /// Each task typically includes a transfer station and other related info.
  /// </summary>
  public List<TransportTask> TransportTasks { get; set; } = route;

  /// <summary>
  /// Gets or sets the total time the truck has worked so far (in minutes).
  /// </summary>
  public double TimeWorked { get; set; } = 0;

  /// <summary>
  /// Returns a human-readable string representation of the truck's route, showing the sequence of transfer stations.
  /// </summary>
  /// <returns>A string in the form "Station1 -> Station2 -> ...".</returns>
  public string RouteToString()
  {
    string stringRoute = "";
    for (int i = 0; i < TransportTasks.Count; ++i)
    {
      stringRoute += TransportTasks[i].TransferStation.Id;
      if (i < TransportTasks.Count - 1)
        stringRoute += " -> ";
    }
    return stringRoute;
  }
}