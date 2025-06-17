/*
 * TwoOpt.cs
 * 
 * The `TwoOpt` class implements a local search algorithm using the 2-opt method to find a local minimum for the RPT-SWTS
 * (Route Planning for Trash Collection with Soft Time and Service Windows) problem. This strategy aims to improve 
 * the current solution by reversing range of collection areas.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.InstanceManagement;
using VrptSwts.Graph;

namespace VrptSwts.Searchs.LocalSearchs;

/// <summary>
/// Class representing the TwoOpt local search heuristic for solving the VRP with SWTS (Soft Time and Service Windows) problem.
/// The TwoOpt algorithm is used to improve the quality of the current solution by iteratively swapping two edges in the route
/// and checking if the new route reduces the total collection time. If an improvement is found, the route is updated.
/// </summary>
internal class TwoOpt : LocalSearch
{

  /// <summary>
  /// Initializes a new instance of the <see cref="TwoOpt"/> class.
  /// </summary>
  /// <param name="problemInstance">The instance of the VRP-SWTS problem to be solved.</param>
  public TwoOpt(VrptSwtsInstance problemInstance) : base(problemInstance) {}
  
  /// <summary>
  /// Performs the local search to find the best possible solution by applying the TwoOpt algorithm.
  /// </summary>
  /// <param name="initialRoutes">The initial solution (set of routes) to improve.</param>
  /// <returns>The improved set of routes after applying the TwoOpt algorithm.</returns>
  public override Routes SearchLocalMinimum(Routes initialRoutes)
  {
    do { 
      for (int route = 0; route < initialRoutes.GetNumberCollectionRoutes(); ++route)
        GenerateNeighbours(initialRoutes, initialRoutes.GetCollectionRoute(route), route);
      Routes? bestNeighbour = GetBestNeighbour();
      if (bestNeighbour == null || initialRoutes.GetCollectionTime() <= bestNeighbour.GetCollectionTime())
        break;
      initialRoutes = bestNeighbour;
      _factibleNeighbours.Clear();
    } while (true);
    return initialRoutes;
  }

  /// <summary>
  /// Generates new neighbour solutions by performing 2-opt swaps on a single route.
  /// </summary>
  /// <param name="routes">The current set of routes.</param>
  /// <param name="collectionRoute">The specific route to perform the swap on.</param>
  /// <param name="route">The index of the route in the list of routes.</param>
  private void GenerateNeighbours(Routes routes, CollectionRoute collectionRoute, int route)
  {
    for (int lowerBound = 0; lowerBound < collectionRoute.GetRouteLength() - 3; ++lowerBound)
    {
      for (int upperBound = lowerBound + 2; upperBound < collectionRoute.GetRouteLength() - 1; ++upperBound)
      {
        List<CollectionRoute> newRoutes = routes.GetCollectionRoutesCopy();
        CollectionRoute? neighbour = BuildNeighbour(collectionRoute, lowerBound, upperBound);
        if (neighbour != null)
        {
          newRoutes.RemoveAt(route);
          newRoutes.Add(neighbour);
          _factibleNeighbours.Add(new Routes(newRoutes));
        }
      }
    }
  }

  /// <summary>
  /// Builds a neighbour route by performing a 2-opt swap on the given route between two positions.
  /// </summary>
  /// <param name="collectionRoute">The route to perform the swap on.</param>
  /// <param name="lowerBound">The first position to swap.</param>
  /// <param name="upperBound">The second position to swap.</param>
  /// <returns>A new CollectionRoute representing the swapped route, or null if the swap is not feasible.</returns>
  private CollectionRoute? BuildNeighbour(CollectionRoute collectionRoute, int lowerBound, int upperBound)
  {
    List<Node> newRoute = collectionRoute.GetRouteCopy();
    (bool isFactible, double newRouteTime) = IsFactibleRoute(newRoute, collectionRoute.RouteTime, lowerBound, upperBound);
    if (!isFactible)
      return null;
    newRouteTime = RemoveUnnecessaryNodes(newRoute, lowerBound, newRouteTime);
    newRouteTime = RemoveUnnecessaryNodes(newRoute, upperBound, newRouteTime);
    return new CollectionRoute(newRoute, new(), newRouteTime);
  }

  /// <summary>
  /// Checks whether the new route formed by swapping two positions is feasible.
  /// </summary>
  /// <param name="route">The current route.</param>
  /// <param name="currentRouteTime">The current time taken by the route.</param>
  /// <param name="lowerBound">The first position to swap.</param>
  /// <param name="upperBound">The second position to swap.</param>
  /// <returns>A tuple indicating whether the route is feasible and the new time taken by the route.</returns>
  private Tuple<bool, double> IsFactibleRoute(List<Node> route, double currentRouteTime, int lowerBound, int upperBound)
  {
    double newRouteTime = CalculateNewRouteTime(route, currentRouteTime, lowerBound, upperBound);
    if (newRouteTime > _problemInstance.CollectionTruck.WorkDuration)
      return new(false, newRouteTime);

    // List<Node> auxRoute = new();
    // auxRoute.AddRange(route.GetRange(0, lowerBound + 1));
    // List<Node> invertedSegment = route.GetRange(lowerBound + 1, upperBound - lowerBound);
    // invertedSegment.Reverse();
    // auxRoute.AddRange(invertedSegment);
    // auxRoute.AddRange(route.GetRange(upperBound + 1, route.Count - (upperBound + 1)));
    // route = new(auxRoute);

    route.Reverse(lowerBound + 1, upperBound - lowerBound);

    if (route[route.Count - 2] is CollectionArea)
      return new(false, newRouteTime);

    return new(IsFactibleCapacity(route, lowerBound + 1), newRouteTime);
  }

  /// <summary>
  /// Calculates the new time taken by the route after performing a 2-opt swap.
  /// </summary>
  /// <param name="route">The current route.</param>
  /// <param name="currentRouteTime">The current time of the route.</param>
  /// <param name="lowerBound">The first position to swap.</param>
  /// <param name="upperBound">The second position to swap.</param>
  /// <returns>The new time of the route after the swap.</returns>
  private double CalculateNewRouteTime(List<Node> route, double currentRouteTime, int lowerBound, int upperBound)
  {
    int truckSpeed = _problemInstance.CollectionTruck.Speed;
    Node lowerBoundNode = route[lowerBound + 1];
    Node upperBoundNode = route[upperBound];
    Node previousLowerNode = route[lowerBound];
    Node nextUpperNode = route[upperBound + 1];
    double newRouteTime = currentRouteTime -
                          (Node.DistanceBetween(previousLowerNode, lowerBoundNode) / truckSpeed * 60) -
                          (Node.DistanceBetween(upperBoundNode, nextUpperNode) / truckSpeed * 60) +
                          (Node.DistanceBetween(previousLowerNode, upperBoundNode) / truckSpeed * 60) +
                          (Node.DistanceBetween(lowerBoundNode, nextUpperNode) / truckSpeed * 60);
    return newRouteTime;
  }

  /// <summary>
  /// Removes unnecessary nodes from the route if they don't contribute to the collection.
  /// </summary>
  /// <param name="route">The current route.</param>
  /// <param name="bound">The position of the node to remove.</param>
  /// <param name="currentRouteTime">The current time of the route.</param>
  /// <returns>The updated route time after removing unnecessary nodes.</returns>
  private double RemoveUnnecessaryNodes(List<Node> route, int bound, double currentRouteTime)
  {
    if (bound == route.Count - 2)
      return currentRouteTime;

    if (route[bound] is not CollectionArea &&
        route[bound + 1] is not CollectionArea)
    {
      int removePosition = bound + 1;
      int truckSpeed = _problemInstance.CollectionTruck.Speed;
      Node removeNode = route[removePosition];
      Node previousNode = route[removePosition - 1];
      Node nextNode = route[removePosition + 1];
      double newRouteTime = currentRouteTime -
                            (Node.DistanceBetween(previousNode, removeNode) / truckSpeed * 60) -
                            (Node.DistanceBetween(removeNode, nextNode) / truckSpeed * 60) +
                            (Node.DistanceBetween(previousNode, nextNode) / truckSpeed * 60);
      route.RemoveAt(removePosition);
      return newRouteTime;
    }
    return currentRouteTime;
  }
}