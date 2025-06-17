/*
 * SwapIntra.cs
 * 
 * The `SwapIntra` class implements a local search algorithm using the swap method to find a local minimum for the RPT-SWTS
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
/// This class performs an intra-route swap operation for local search in the VRPTW problem.
/// It tries to find a local minimum by swapping nodes within a given route to improve the overall solution.
/// </summary>
internal class SwapIntra : LocalSearch
{

  /// <summary>
  /// Initializes a new instance of the SwapIntra class with the given problem instance.
  /// </summary>
  /// <param name="problemInstance">The VRPTW problem instance containing the necessary problem data.</param>
  public SwapIntra(VrptSwtsInstance problemInstance) : base(problemInstance) {}

  /// <summary>
  /// Performs the local search to find a local minimum by swapping nodes within the routes.
  /// </summary>
  /// <param name="initialRoutes">The initial routes to be optimized.</param>
  /// <returns>The optimized set of routes after applying intra-route swaps.</returns>
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
  /// Generates neighbouring solutions by swapping nodes within a specific route.
  /// </summary>
  /// <param name="routes">The current set of routes.</param>
  /// <param name="collectionRoute">The specific collection route to generate neighbours for.</param>
  /// <param name="route">The index of the collection route.</param>
  private void GenerateNeighbours(Routes routes, CollectionRoute collectionRoute, int route)
  {
    for (int node = 1; node < collectionRoute.GetRouteLength() - 2; ++node)
    {
      if (collectionRoute.GetArea(node).Id.StartsWith("TS")) continue;
      for (int changePosition = node + 1; changePosition < collectionRoute.GetRouteLength() - 2; ++changePosition)
      {
        if (node == changePosition) continue;
        if (collectionRoute.GetArea(changePosition).Id.StartsWith("TS")) continue;
        List<CollectionRoute> newRoutes = routes.GetCollectionRoutesCopy();
        CollectionRoute? neighbour = BuildNeighbour(collectionRoute, node, changePosition);
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
  /// Builds a new neighbour route by swapping two nodes in the collection route.
  /// </summary>
  /// <param name="collectionRoute">The original collection route.</param>
  /// <param name="swapPosition1">The first position to swap.</param>
  /// <param name="swapPosition2">The second position to swap.</param>
  /// <returns>A new collection route if the swap is feasible, otherwise null.</returns>
  private CollectionRoute? BuildNeighbour(CollectionRoute collectionRoute, int swapPosition1, int swapPosition2)
  {
    List<Node> newRoute = collectionRoute.GetRouteCopy();
    (bool isFactible, double newRouteTime) = IsFactibleRoute(newRoute, collectionRoute.RouteTime, swapPosition1, swapPosition2);
    if (!isFactible)
      return null;
    return new CollectionRoute(newRoute, new(), newRouteTime);
  }

  /// <summary>
  /// Checks if a route is feasible after swapping two nodes.
  /// </summary>
  /// <param name="route">The route with swapped nodes.</param>
  /// <param name="currentRouteTime">The time of the original route.</param>
  /// <param name="swapPosition1">The first node position to swap.</param>
  /// <param name="swapPosition2">The second node position to swap.</param>
  /// <returns>A tuple containing a boolean indicating if the route is feasible and the new route time.</returns>
  private Tuple<bool, double> IsFactibleRoute(List<Node> route, double currentRouteTime, int swapPosition1, int swapPosition2)
  {
    double newRouteTime = CalculateNewRouteTime(route, currentRouteTime, swapPosition1, swapPosition2);
    if (newRouteTime > _problemInstance.CollectionTruck.WorkDuration)
      return new(false, newRouteTime);

    Node swapNode1 = route[swapPosition1];
    Node swapNode2 = route[swapPosition2];
    route[swapPosition1] = swapNode2;
    route[swapPosition2] = swapNode1;

    if (IsFactibleCapacity(route, swapPosition1) && IsFactibleCapacity(route, swapPosition2))
      return new(true, newRouteTime);
    return new(false, newRouteTime);
  }

  /// <summary>
  /// Calculates the new route time after swapping two nodes.
  /// </summary>
  /// <param name="route">The route with swapped nodes.</param>
  /// <param name="currentRouteTime">The time of the original route.</param>
  /// <param name="swapPosition1">The first node position to swap.</param>
  /// <param name="swapPosition2">The second node position to swap.</param>
  /// <returns>The new route time after the swap.</returns>
  private double CalculateNewRouteTime(List<Node> route, double currentRouteTime, int swapPosition1, int swapPosition2)
  {
    int truckSpeed = _problemInstance.CollectionTruck.Speed;
    double newRouteTime;
    Node swapNode1 = route[swapPosition1];
    Node swapNode2 = route[swapPosition2];
    if (swapPosition2 - swapPosition1 == 1)
    {
      Node previousSwapNode = route[swapPosition1 - 1];
      Node nextSwapNode = route[swapPosition2 + 1];
      newRouteTime = currentRouteTime -
                     (Node.DistanceBetween(previousSwapNode, swapNode1) / truckSpeed * 60) -
                     (Node.DistanceBetween(swapNode2, nextSwapNode) / truckSpeed * 60) +
                     (Node.DistanceBetween(previousSwapNode, swapNode2) / truckSpeed * 60) +
                     (Node.DistanceBetween(swapNode1, nextSwapNode) / truckSpeed * 60);
    }
    else {
      Node previousSwapNode1 = route[swapPosition1 - 1];
      Node nextSwapNode1 = route[swapPosition1 + 1];
      Node previousSwapNode2 = route[swapPosition2 - 1];
      Node nextSwapNode2 = route[swapPosition2 + 1];
      newRouteTime = currentRouteTime -
                     (Node.DistanceBetween(previousSwapNode1, swapNode1) / truckSpeed * 60) -
                     (Node.DistanceBetween(swapNode1, nextSwapNode1) / truckSpeed * 60) +
                     (Node.DistanceBetween(previousSwapNode1, swapNode2) / truckSpeed * 60) +
                     (Node.DistanceBetween(swapNode2, nextSwapNode1) / truckSpeed * 60) -
                     (Node.DistanceBetween(previousSwapNode2, swapNode2) / truckSpeed * 60) -
                     (Node.DistanceBetween(swapNode2, nextSwapNode2) / truckSpeed * 60) +
                     (Node.DistanceBetween(previousSwapNode2, swapNode1) / truckSpeed * 60) +
                     (Node.DistanceBetween(swapNode1, nextSwapNode2) / truckSpeed * 60);
    }
    return newRouteTime;
  }
}