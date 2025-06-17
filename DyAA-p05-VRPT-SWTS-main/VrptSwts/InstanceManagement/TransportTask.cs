/*
 * TransportTask.cs
 * 
 * This class represents a transport task in the VRPT-SWTS (Vehicle Routing Problem 
 * with Trash Collection and Soft Time Windows). A transport task encapsulates the 
 * unloading of waste at a transfer station, including the amount of waste, the target
 * station, and the arrival time.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using System.Security.Principal;
using VrptSwts.Graph;

/// <summary>
/// Represents a waste transport task to a transfer station.
/// </summary>
internal class TransportTask
{

  /// <summary>
  /// Quantity of waste to be unloaded at the transfer station.
  /// </summary>
  public int UnloadedQuantity { get; }

  /// <summary>
  /// Node representing the transfer station location.
  /// </summary>
  public Node TransferStation { get; }

  /// <summary>
  /// Time of arrival at the transfer station.
  /// </summary>
  public double ArrivalTime { get; }

  /// <summary>
  /// Initializes a new instance of the TransportTask class.
  /// </summary>
  /// <param name="unloadedQuantity">The amount of waste to unload.</param>
  /// <param name="transferStation">The transfer station where unloading takes place.</param>
  /// <param name="arrivalTime">Time of arrival at the station.</param>
  public TransportTask(int unloadedQuantity,
                       Node transferStation,
                       double arrivalTime)
  {
    UnloadedQuantity = unloadedQuantity;
    TransferStation = transferStation;
    ArrivalTime = arrivalTime;
  }

  /// <summary>
  /// Prints the transport task details to the console.
  /// </summary>
  public void print() {
    Console.WriteLine($"Transfer Station: {TransferStation.Id}");
    Console.WriteLine($"Unloaded Quantity: {UnloadedQuantity}");
    Console.WriteLine($"Arrival Time: {ArrivalTime}");
  }
}