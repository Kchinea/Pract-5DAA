/*
 * SwapInter.cs
 * 
 * The `SwapInter` class implements a local search algorithm using the swap method to find a local minimum for the RPT-SWTS
 * (Route Planning for Trash Collection with Soft Time and Service Windows) problem. This strategy aims to improve 
 * the current solution by swapping collection areas between different routes to minimize the total collection time. 
 * The class extends the `LocalSearch` base class and overrides the `SearchLocalMinimum` method to perform the local search.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.InstanceManagement;
using VrptSwts.Graph;

namespace VrptSwts.Searchs.LocalSearchs;

/// <summary>
/// This class implements the local search strategy based on swapping nodes between two routes
/// to minimize the total collection time. It performs an iterative search to find the best route 
/// configuration by swapping nodes between different collection routes.
/// </summary>
internal class SwapInter : LocalSearch
{
  
  /// <summary>
  /// Initializes a new instance of the <see cref="SwapInter"/> class.
  /// </summary>
  /// <param name="problemInstance">The problem instance containing the truck and route data.</param>
  public SwapInter(VrptSwtsInstance problemInstance) : base(problemInstance) {}

  /// <summary>
  /// Performs a local search to find the minimum collection time by swapping nodes between routes.
  /// </summary>
  /// <param name="initialRoutes">The initial set of routes to be optimized.</param>
  /// <returns>Returns the optimized set of routes.</returns>
  public override Routes SearchLocalMinimum(Routes initialRoutes)
  {
    do { 
      for (int route1 = 0; route1 < initialRoutes.GetNumberCollectionRoutes() - 1; ++route1)
      {
        for (int route2 = route1 + 1; route2 < initialRoutes.GetNumberCollectionRoutes(); ++route2)
        {
          GenerateNeighbours(initialRoutes, initialRoutes.GetCollectionRoute(route1), initialRoutes.GetCollectionRoute(route2), route1, route2);
        }
      }
      Routes? bestNeighbour = GetBestNeighbour();
      if (bestNeighbour == null || initialRoutes.GetCollectionTime() <= bestNeighbour.GetCollectionTime())
        break;
      initialRoutes = bestNeighbour;
      _factibleNeighbours.Clear();
    } while (true);
    return initialRoutes;
  }

  /// <summary>
  /// Generates possible neighbour solutions by swapping nodes between two routes.
  /// </summary>
  /// <param name="routes">The current set of routes.</param>
  /// <param name="collectionRoute1">The first collection route.</param>
  /// <param name="collectionRoute2">The second collection route.</param>
  /// <param name="route1">Index of the first route.</param>
  /// <param name="route2">Index of the second route.</param>
  private void GenerateNeighbours(Routes routes, CollectionRoute collectionRoute1, CollectionRoute collectionRoute2, int route1, int route2)
  {
    for (int node = 1; node < collectionRoute1.GetRouteLength() - 2; ++node)
    {
      if (collectionRoute1.GetArea(node).Id.StartsWith("TS")) continue;
      for (int changePosition = 1; changePosition < collectionRoute2.GetRouteLength() - 2; ++changePosition)
      {
        if (collectionRoute2.GetArea(changePosition).Id.StartsWith("TS")) continue;
        List<CollectionRoute> newRoutes = routes.GetCollectionRoutesCopy();
        Tuple<CollectionRoute, CollectionRoute>? neighbour = BuildNeighbour(collectionRoute1, collectionRoute2, node, changePosition);
        if (neighbour != null) 
        {
          int maxIndexRoute = Math.Max(route1, route2);
          int minIndexRoute = Math.Min(route1, route2);
          newRoutes.RemoveAt(maxIndexRoute);
          newRoutes.RemoveAt(minIndexRoute);
          newRoutes.Add(neighbour.Item1);
          newRoutes.Add(neighbour.Item2);
          _factibleNeighbours.Add(new Routes(newRoutes));
        }
      }
    }
  }

  /// <summary>
  /// Builds a new neighbour solution by swapping two nodes between two collection routes.
  /// </summary>
  /// <param name="collectionRoute1">The first collection route.</param>
  /// <param name="collectionRoute2">The second collection route.</param>
  /// <param name="swapPosition1">The position of the node to be swapped in the first route.</param>
  /// <param name="swapPosition2">The position of the node to be swapped in the second route.</param>
  /// <returns>A tuple of two new collection routes after the swap, or null if the swap is not feasible.</returns>
  private Tuple<CollectionRoute, CollectionRoute>? BuildNeighbour(CollectionRoute collectionRoute1, CollectionRoute collectionRoute2, int swapPosition1, int swapPosition2)
  {
    List<Node> newRoute1 = collectionRoute1.GetRouteCopy();
    List<Node> newRoute2 = collectionRoute2.GetRouteCopy();
    (bool isFactible, double newRouteTime1, double newRouteTime2) = IsFactibleRoute(newRoute1, newRoute2, collectionRoute1.RouteTime, collectionRoute2.RouteTime , swapPosition1, swapPosition2);
    if (!isFactible)
      return null;
    return new(new CollectionRoute(newRoute1, new(), newRouteTime1), new CollectionRoute(newRoute2, new(), newRouteTime2));
  }

  /// <summary>
  /// Checks if the swap of nodes between two routes is feasible in terms of route time and capacity.
  /// </summary>
  /// <param name="route1">The first route.</param>
  /// <param name="route2">The second route.</param>
  /// <param name="currentRouteTime1">The current time of the first route.</param>
  /// <param name="currentRouteTime2">The current time of the second route.</param>
  /// <param name="swapPosition1">The position of the node to be swapped in the first route.</param>
  /// <param name="swapPosition2">The position of the node to be swapped in the second route.</param>
  /// <returns>A tuple containing a boolean indicating if the swap is feasible and the new route times.</returns>
  private Tuple<bool, double, double> IsFactibleRoute(List<Node> route1, List<Node> route2, double currentRouteTime1, double currentRouteTime2, int swapPosition1, int swapPosition2)
  {
    double newRouteTime1 = CalculateNewRouteTime(route1, route2, currentRouteTime1, swapPosition1, swapPosition2);
    if (newRouteTime1 > _problemInstance.CollectionTruck.WorkDuration)
      return new(false, newRouteTime1, currentRouteTime2);

    double newRouteTime2 = CalculateNewRouteTime(route2, route1, currentRouteTime2, swapPosition2, swapPosition1);
    if (newRouteTime2 > _problemInstance.CollectionTruck.WorkDuration)
      return new(false, newRouteTime1, newRouteTime2);

    Node swapNode1 = route1[swapPosition1];
    Node swapNode2 = route2[swapPosition2];
    route1[swapPosition1] = swapNode2;
    route2[swapPosition2] = swapNode1;

    if (IsFactibleCapacity(route1, swapPosition1) && IsFactibleCapacity(route2, swapPosition2))
      return new(true, newRouteTime1, newRouteTime2);
    return new(false, newRouteTime1, newRouteTime2);
  }

  /// <summary>
  /// Calculates the new time for a route after swapping two nodes.
  /// </summary>
  /// <param name="route1">The first route.</param>
  /// <param name="route2">The second route.</param>
  /// <param name="currentRouteTime">The current time of the route.</param>
  /// <param name="swapPosition1">The position of the node to be swapped in the first route.</param>
  /// <param name="swapPosition2">The position of the node to be swapped in the second route.</param>
  /// <returns>The new time for the route after the swap.</returns>
  private double CalculateNewRouteTime(List<Node> route1, List<Node> route2, double currentRouteTime, int swapPosition1, int swapPosition2)
  {
    int truckSpeed = _problemInstance.CollectionTruck.Speed;
    CollectionArea swapNode1 = (CollectionArea)route1[swapPosition1];
    CollectionArea swapNode2 = (CollectionArea)route2[swapPosition2];
    Node previousSwapNode1 = route1[swapPosition1 - 1];
    Node nextSwapNode1 = route1[swapPosition1 + 1];
    double newRouteTime = currentRouteTime -
                           (Node.DistanceBetween(previousSwapNode1, swapNode1) / truckSpeed * 60) -
                           (Node.DistanceBetween(swapNode1, nextSwapNode1) / truckSpeed * 60) +
                           (Node.DistanceBetween(previousSwapNode1, swapNode2) / truckSpeed * 60) +
                           (Node.DistanceBetween(swapNode2, nextSwapNode1) / truckSpeed * 60) -
                           swapNode1.ProcessingTime + swapNode2.ProcessingTime;
    return newRouteTime;
  }
}