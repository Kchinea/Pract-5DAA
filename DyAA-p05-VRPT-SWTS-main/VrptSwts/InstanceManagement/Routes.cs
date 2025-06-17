/*
 * Routes.cs
 * 
 * This class defines a wrapper for handling both collection and transport routes in the
 * context of the VRPT-SWTS (Vehicle Routing Problem with Trash Collection and Soft Time Windows).
 * 
 * It supports various operations such as calculating total collection and transport times,
 * retrieving individual routes, and formatting the solution as a string.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.Vehicles;

namespace VrptSwts.InstanceManagement;

/// <summary>
/// Represents a full set of routes including collection and transport phases.
/// </summary>
internal class Routes
{
  private List<CollectionRoute> _collectionRoutes = new List<CollectionRoute>();
  private List<TransportTruck> _transportTrucks = new List<TransportTruck>();

  /// <summary>
  /// Default constructor. Initializes empty route lists.
  /// </summary>
  public Routes() {}

  /// <summary>
  /// Constructor that only initializes the collection routes.
  /// </summary>
  /// <param name="collectionRoutes">List of collection routes.</param>
  public Routes(List<CollectionRoute> collectionRoutes)
  {
    _collectionRoutes = collectionRoutes;
  }

  /// <summary>
  /// Constructor that initializes both collection and transport routes.
  /// </summary>
  /// <param name="collectionRoutes">List of collection routes.</param>
  /// <param name="transportTrucks">List of transport routes (trucks).</param>
  public Routes(List<CollectionRoute> collectionRoutes, List<TransportTruck> transportTrucks)
  {
    _collectionRoutes = collectionRoutes;
    _transportTrucks = transportTrucks;
  }

  /// <summary>
  /// Returns a shallow copy of the collection routes list.
  /// </summary>
  public List<CollectionRoute> GetCollectionRoutesCopy()
  {
    return new(_collectionRoutes);
  } 

  /// <summary>
  /// Calculates the total collection time across all routes.
  /// </summary>
  /// <returns>Total time spent collecting waste.</returns>
  public double GetCollectionTime()
  {
    double totalTime = 0.0;
    foreach (CollectionRoute collectionRoute in _collectionRoutes)
      totalTime += collectionRoute.RouteTime;
    return totalTime;
  }

  /// <summary>
  /// Gets the number of collection routes.
  /// </summary>
  public int GetNumberCollectionRoutes()
  {
    return _collectionRoutes.Count();
  }

  /// <summary>
  /// Retrieves a specific collection route by its index.
  /// </summary>
  /// <param name="indexRoute">Index of the desired route.</param>
  public CollectionRoute GetCollectionRoute(int indexRoute)
  {
    return _collectionRoutes[indexRoute];
  }

  /// <summary>
  /// Calculates the total time spent by all transport trucks.
  /// </summary>
  /// <returns>Total transport time.</returns>
  public double GetTransportTime()
  {
    double totalTime = 0.0;
    foreach (TransportTruck transportTruck in _transportTrucks)
      totalTime += transportTruck.TimeWorked;
    return totalTime;
  }

  /// <summary>
  /// Gets the number of transport routes (trucks).
  /// </summary>
  public int GetNumberTransportRoutes()
  {
    return _transportTrucks.Count;
  }

  /// <summary>
  /// Returns a string representation of all collection and transport routes.
  /// </summary>
  public override string ToString()
  {
    string routesString = "";
    routesString += "Collection Routes:\n\n";
    int numberCollectionRoutes = _collectionRoutes.Count();
    for (int route = 0; route < numberCollectionRoutes; ++route)
    {
      routesString += $"Route {route + 1}:\n";
      CollectionRoute collectionRoute = _collectionRoutes[route];
      routesString += collectionRoute.ToString();
      routesString += "\n\n";
    }

    routesString += "Transport Routes:\n\n";
    int numberTransportRoutes = _transportTrucks.Count;
    for (int route = 0; route < numberTransportRoutes; ++route)
    {
      routesString += $"Route {route + 1}:\n";
      TransportTruck transportTruck = _transportTrucks[route];
      routesString += transportTruck.RouteToString();
      if (route < numberTransportRoutes - 1)
        routesString += "\n\n";
    }
    return routesString;
  }
}