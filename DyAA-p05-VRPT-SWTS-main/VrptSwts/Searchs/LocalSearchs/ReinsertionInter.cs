/*
 * ReinsertionInter.cs
 * 
 * The `ReinsertionInter` class is an implementation of a local search algorithm using the reinsertion method 
 * to find a local minimum for the RPT-SWTS (Route Planning for Trash Collection with Soft Time and Service Windows) problem.
 * This strategy attempts to improve the current solution by reinserting collection areas between different routes to minimize 
 * the total collection time. The class extends the abstract `LocalSearch` class and overrides the `SearchLocalMinimum` method.
 * 
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.InstanceManagement;
using VrptSwts.Graph;

namespace VrptSwts.Searchs.LocalSearchs;


/// <summary>
/// `ReinsertionInter` implements a local search algorithm based on the reinsertion strategy. This algorithm aims to improve the 
/// given routes by reinserting collection areas between different routes to minimize the total collection time. The search 
/// terminates when no further improvement is possible.
/// </summary>
internal class ReinsertionInter : LocalSearch
{

  /// <summary>
  /// Constructor that initializes the reinsertion local search with the given problem instance.
  /// </summary>
  /// <param name="problemInstance">The problem instance containing information about the vehicles, nodes, and constraints.</param>
  public ReinsertionInter(VrptSwtsInstance problemInstance) : base(problemInstance) {}
  
  /// <summary>
  /// Performs the local search to find the best set of routes by reinserting collection areas between routes.
  /// </summary>
  /// <param name="initialRoutes">The initial set of routes to start the local search from.</param>
  /// <returns>Returns the best set of routes found after applying the reinsertion local search strategy.</returns>
  public override Routes SearchLocalMinimum(Routes initialRoutes)
  {
    do { 
      for (int route1 = 0; route1 < initialRoutes.GetNumberCollectionRoutes(); ++route1)
      {
        for (int route2 = 0; route2 < initialRoutes.GetNumberCollectionRoutes(); ++route2)
        {
          if (route1 == route2) continue;
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
  /// Generates neighboring solutions by making reinsertion moves between two collection routes.
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
      for (int changePosition = 1; changePosition < collectionRoute2.GetRouteLength() - 1; ++changePosition)
      {
        List<CollectionRoute> newRoutes = routes.GetCollectionRoutesCopy();
        Tuple<CollectionRoute, CollectionRoute>? neighbour = BuildNeighbour(collectionRoute1, collectionRoute2, node, changePosition);
        if (neighbour != null)
        {
          int maxIndexRoute = Math.Max(route1, route2);
          int minIndexRoute = Math.Min(route1, route2);
          newRoutes.RemoveAt(maxIndexRoute);
          newRoutes.RemoveAt(minIndexRoute);
          if (neighbour.Item1.GetRouteLength() > 2)
            newRoutes.Add(neighbour.Item1);
          newRoutes.Add(neighbour.Item2);
          _factibleNeighbours.Add(new Routes(newRoutes));
        }
      }
    }
  }

  /// <summary>
  /// Builds a pair of new collection routes after a reinsertion move. Checks if the move is feasible in terms of capacity and time.
  /// </summary>
  /// <param name="collectionRoute1">The first collection route.</param>
  /// <param name="collectionRoute2">The second collection route.</param>
  /// <param name="changingPosition">The position of the node to be moved in the first route.</param>
  /// <param name="insertionPosition">The position where the node is to be inserted in the second route.</param>
  /// <returns>Returns a tuple of new collection routes, or null if the move is not feasible.</returns>
  private Tuple<CollectionRoute, CollectionRoute>? BuildNeighbour(CollectionRoute collectionRoute1, CollectionRoute collectionRoute2, int changingPosition, int insertionPosition)
  {
    List<Node> newRoute1 = collectionRoute1.GetRouteCopy();
    List<Node> newRoute2 = collectionRoute2.GetRouteCopy();
    (bool isFactible, double newRouteTime1, double newRouteTime2) = IsFactibleRoute(newRoute1, newRoute2, collectionRoute1.RouteTime, collectionRoute2.RouteTime, changingPosition, insertionPosition);
    if (!isFactible)
      return null;
    newRouteTime1 = RemoveUnnecessaryNodes(newRoute1, changingPosition, newRouteTime1);
    return new(new CollectionRoute(newRoute1, new(), newRouteTime1), new CollectionRoute(newRoute2, new(), newRouteTime2));
  }

  /// <summary>
  /// Checks if the resulting routes after a reinsertion move are feasible.
  /// </summary>
  /// <param name="route1">The first route after reinsertion.</param>
  /// <param name="route2">The second route after reinsertion.</param>
  /// <param name="currentRouteTime1">The current collection time for the first route.</param>
  /// <param name="currentRouteTime2">The current collection time for the second route.</param>
  /// <param name="changingPosition">The position of the node to be moved in the first route.</param>
  /// <param name="insertionPosition">The position where the node is to be inserted in the second route.</param>
  /// <returns>Returns a tuple with a boolean indicating feasibility and the updated route times.</returns>
  private Tuple<bool, double, double> IsFactibleRoute(List<Node> route1, List<Node> route2, double currentRouteTime1, double currentRouteTime2, int changingPosition, int insertionPosition)
  {
    double newRouteTime1 = CalculateChangingRouteTime(route1, currentRouteTime1, changingPosition);
    double newRouteTime2 = CalculateInsertionRouteTime(route1, route2, currentRouteTime2, changingPosition, insertionPosition);
                          
    if (newRouteTime2 > _problemInstance.CollectionTruck.WorkDuration)
      return new(false, newRouteTime1, newRouteTime2);

    Node changingNode = route1[changingPosition];
    route1.RemoveAt(changingPosition);
    route2.Insert(insertionPosition, changingNode);
    return new(IsFactibleCapacity(route2, insertionPosition), newRouteTime1, newRouteTime2);
  }
  
  /// <summary>
  /// Calculates the updated collection time for the first route after changing the position of a collection area.
  /// </summary>
  /// <param name="route">The route to calculate the time for.</param>
  /// <param name="currentRouteTime">The current collection time of the route.</param>
  /// <param name="changingPosition">The position of the node to be moved in the route.</param>
  /// <returns>Returns the updated collection time for the first route.</returns>
  private double CalculateChangingRouteTime(List<Node> route, double currentRouteTime, int changingPosition)
  {
    int truckSpeed = _problemInstance.CollectionTruck.Speed;
    CollectionArea changingNode = (CollectionArea)route[changingPosition];
    Node previousChangingNode = route[changingPosition - 1];
    Node nextChangingNode = route[changingPosition + 1];
    double newRouteTime = currentRouteTime -
                          (Node.DistanceBetween(previousChangingNode, changingNode) / truckSpeed * 60) -
                          (Node.DistanceBetween(changingNode, nextChangingNode) / truckSpeed * 60) +
                          (Node.DistanceBetween(previousChangingNode, nextChangingNode) / truckSpeed * 60) -
                          changingNode.ProcessingTime;
    return newRouteTime;
  }

  /// <summary>
  /// Calculates the updated collection time for the second route after inserting a node from the first route.
  /// </summary>
  /// <param name="route1">The first route from which the node is being moved.</param>
  /// <param name="route2">The second route where the node is being inserted.</param>
  /// <param name="currentRouteTime">The current collection time of the second route.</param>
  /// <param name="changingPosition">The position of the node in the first route.</param>
  /// <param name="insertionPosition">The position where the node is being inserted in the second route.</param>
  /// <returns>Returns the updated collection time for the second route.</returns>
  private double CalculateInsertionRouteTime(List<Node> route1, List<Node> route2, double currentRouteTime, int changingPosition, int insertionPosition)
  {
    int truckSpeed = _problemInstance.CollectionTruck.Speed;
    CollectionArea changingNode = (CollectionArea)route1[changingPosition];
    Node previousNewNode = route2[insertionPosition - 1];
    Node nextNewNode = route2[insertionPosition];
    double newRouteTime = currentRouteTime -
                          (Node.DistanceBetween(previousNewNode, nextNewNode) / truckSpeed * 60) +
                          (Node.DistanceBetween(previousNewNode, changingNode) / truckSpeed * 60) +
                          (Node.DistanceBetween(changingNode, nextNewNode) / truckSpeed * 60) +
                          changingNode.ProcessingTime;
    return newRouteTime;
  }

  /// <summary>
  /// Removes unnecessary nodes from a route and updates the collection time accordingly.
  /// </summary>
  /// <param name="route">The route to remove the unnecessary node from.</param>
  /// <param name="changingPosition">The position of the node to be removed.</param>
  /// <param name="currentRouteTime">The current collection time of the route.</param>
  /// <returns>Returns the updated collection time after removing the node.</returns>
  private double RemoveUnnecessaryNodes(List<Node> route, int changingPosition, double currentRouteTime)
  {
    if (route[changingPosition] is not CollectionArea &&
        route[changingPosition - 1] is not CollectionArea)
    {
      int removePosition = changingPosition;
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