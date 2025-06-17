/*
 * VrptSwtsInstance.cs
 * 
 * This class represents an instance of the VRPT-SWTS (Vehicle Routing Problem 
 * with Trash Collection and Soft Time Windows). It encapsulates the problem 
 * parameters such as the number of vehicles, truck details, depot, dumpsite, 
 * transfer stations, and collection areas. The class also includes a method 
 * to clone the instance for optimization algorithms.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.Graph;
using VrptSwts.Vehicles;

namespace VrptSwts.InstanceManagement;

internal class VrptSwtsInstance
{

  /// <summary>
  /// Number of vehicles available for collection and transportation.
  /// </summary>
  public int NumberVehicles { get; }

  /// <summary>
  /// Truck used for waste collection.
  /// </summary>
  public Truck CollectionTruck { get; }

  /// <summary>
  /// Truck used for transporting the waste to the dumpsite.
  /// </summary>
  public Truck TransportTruck { get; }

  /// <summary>
  /// The depot node where the trucks start their routes.
  /// </summary>
  public Node Depot { get; }

  /// <summary>
  /// The dumpsite node where the waste is eventually disposed of.
  /// </summary>
  public Node Dumpsite { get; }

  /// <summary>
  /// A list of transfer station nodes that are part of the waste collection network.
  /// </summary>
  public List<Node> TransferStations;

  /// <summary>
  /// A list of collection area nodes where waste is collected.
  /// </summary>
  public List<Node> CollectionAreas;

  /// <summary>
  /// Initializes a new instance of the VrptSwtsInstance class.
  /// </summary>
  /// <param name="numberVehicles">The number of available vehicles for collection and transport.</param>
  /// <param name="collectionTruck">The truck used for collection tasks.</param>
  /// <param name="transportTruck">The truck used for transportation tasks.</param>
  /// <param name="depot">The depot node from which the trucks start.</param>
  /// <param name="dumpsite">The dumpsite node where waste is disposed of.</param>
  /// <param name="transferStations">The list of transfer station nodes.</param>
  /// <param name="collectionAreas">The list of collection area nodes.</param>
  public VrptSwtsInstance(int numberVehicles, Truck collectionTruck,
                          Truck transportTruck, Node depot, Node dumpsite,
                          List<Node> transferStations, List<Node> collectionAreas)
  {
    NumberVehicles = numberVehicles;
    CollectionTruck = collectionTruck;
    TransportTruck = transportTruck;
    Depot = depot;
    Dumpsite = dumpsite;
    TransferStations = transferStations;
    CollectionAreas = collectionAreas;
  }

  /// <summary>
  /// Creates a clone of the current instance of VrptSwtsInstance.
  /// </summary>
  /// <returns>A new instance of VrptSwtsInstance with the same values as the current instance.</returns>
  public VrptSwtsInstance Clone()
  {
    return new VrptSwtsInstance(
      NumberVehicles,
      CollectionTruck,
      TransportTruck,
      Depot,
      Dumpsite,
      new List<Node>(TransferStations),
      new List<Node>(CollectionAreas)
    );
  }
}