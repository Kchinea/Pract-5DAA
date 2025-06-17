/*
 * LocalSearch.cs
 * 
 * The `LocalSearch` class is an abstract base class for implementing local search strategies for solving the 
 * Vehicle Routing Problem with Time Windows and Soft Constraints (RPT-SWTS).
 * 
 * This class provides a general framework for local search methods. It is used to explore and improve routes 
 * within the problem instance by evaluating feasible neighbors and searching for local minima. Specific 
 * search algorithms will inherit from this class and implement the `SearchLocalMinimum` method.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.InstanceManagement;
using VrptSwts.Graph;

namespace VrptSwts.Searchs.LocalSearchs;

/// <summary>
/// The `LocalSearch` class is an abstract base class for implementing local search strategies for the RPT-SWTS problem.
/// It provides common functionality to evaluate feasible neighbors and find local minima by improving routes.
/// </summary>
internal abstract class LocalSearch(VrptSwtsInstance problemInstance)
{

  /// <summary>
  /// The problem instance, containing information about the vehicles, nodes, and constraints.
  /// </summary>
  protected readonly VrptSwtsInstance _problemInstance = problemInstance;

  /// <summary>
  /// A list of feasible neighbors that can be considered in the local search.
  /// </summary>
  protected List<Routes> _factibleNeighbours = new();

  /// <summary>
  /// Performs a local search to find the best solution (local minimum) starting from the given initial routes.
  /// This method must be implemented by derived classes to define specific local search strategies.
  /// </summary>
  /// <param name="initialRoutes">The initial set of routes to start the local search from.</param>
  /// <returns>Returns the best set of routes found after performing the local search.</returns>
  public abstract Routes SearchLocalMinimum(Routes initialRoutes);

  /// <summary>
  /// Checks if a route is feasible in terms of vehicle capacity.
  /// It ensures that the total demand in the route does not exceed the truck's capacity.
  /// </summary>
  /// <param name="route">The route to check.</param>
  /// <param name="changedPosition">The position where the route was modified.</param>
  /// <returns>Returns true if the route is feasible, false otherwise.</returns>
  protected virtual bool IsFactibleCapacity(List<Node> route, int changedPosition)
  {
    int truckCapacity = _problemInstance.CollectionTruck.Capacity;
    int subRoutePosition = GetStartSubRouteIndex(route, changedPosition) + 1;
    while (route[subRoutePosition] is CollectionArea collectionArea)
    {
      truckCapacity -= collectionArea.Demand;
      if (truckCapacity < 0)
        return false;
      ++subRoutePosition;
    }
    return true;
  }

  /// <summary>
  /// Determines the starting index of the sub-route, considering the current position and any collection areas.
  /// </summary>
  /// <param name="route">The route to analyze.</param>
  /// <param name="currentPosition">The current position in the route.</param>
  /// <returns>Returns the index of the start of the sub-route.</returns>
  protected int GetStartSubRouteIndex(List<Node> route, int currentPosition)
  {
    while(route[currentPosition] is CollectionArea)
      --currentPosition;
    return currentPosition;
  }

  /// <summary>
  /// Finds and returns the best neighboring solution by comparing the collection times of all feasible neighbors.
  /// </summary>
  /// <returns>Returns the best neighboring solution (Routes) based on collection time, or null if no neighbors exist.</returns>
  protected Routes? GetBestNeighbour()
  {
    if (_factibleNeighbours.Count == 0)
      return null;
    Routes bestNeighbour = _factibleNeighbours[0];
    for (int i = 1; i < _factibleNeighbours.Count; ++i)
    {
      if (_factibleNeighbours[i].GetCollectionTime() < bestNeighbour.GetCollectionTime())
        bestNeighbour = _factibleNeighbours[i];
    }
    return bestNeighbour;
  }
}