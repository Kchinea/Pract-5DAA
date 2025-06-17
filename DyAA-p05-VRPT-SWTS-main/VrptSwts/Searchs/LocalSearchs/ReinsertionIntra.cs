/*
 * ReinsertionIntra.cs
 * 
 * The `ReinsertionIntra` class is an implementation of a local search algorithm using the reinsertion method 
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
/// The ReinsertionIntra class implements a local search method using reinsertion to find a local minimum solution.
/// It operates on the RPT-SWTS problem, where collection areas are moved between different routes to minimize collection time.
/// </summary>
internal class ReinsertionIntra : LocalSearch
{

  /// <summary>
  /// Constructor for ReinsertionIntra.
  /// </summary>
  /// <param name="problemInstance">The problem instance that contains parameters such as truck speed, work duration, etc.</param>
  public ReinsertionIntra(VrptSwtsInstance problemInstance) : base(problemInstance) {}
  
  /// <summary>
  /// Searches for a local minimum by iterating over the routes and attempting reinsertion of nodes between routes.
  /// The process continues until no improvement can be found.
  /// </summary>
  /// <param name="initialRoutes">The initial set of collection routes to optimize.</param>
  /// <returns>The optimized collection routes.</returns>
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
  /// Generates neighboring solutions by reinserting nodes between different positions within the same route.
  /// </summary>
  /// <param name="routes">The current set of collection routes.</param>
  /// <param name="collectionRoute">The collection route that will be modified.</param>
  /// <param name="route">The index of the route being modified.</param>
  private void GenerateNeighbours(Routes routes, CollectionRoute collectionRoute, int route)
  {
    for (int node = 1; node < collectionRoute.GetRouteLength() - 2; ++node)
    {
      if (collectionRoute.GetArea(node).Id.StartsWith("TS")) continue;
      for (int changePosition = 1; changePosition < collectionRoute.GetRouteLength() - 2; ++changePosition)
      {
        if (node == changePosition) continue;
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
  /// Builds a new neighbor solution by reinserting a node at a different position within the route.
  /// </summary>
  /// <param name="collectionRoute">The collection route to modify.</param>
  /// <param name="changingPosition">The index of the node to move.</param>
  /// <param name="insertionPosition">The new position where the node will be inserted.</param>
  /// <returns>A new collection route with the node reinserted, or null if the move is not feasible.</returns>
  private CollectionRoute? BuildNeighbour(CollectionRoute collectionRoute, int changingPosition, int insertionPosition)
  {
    List<Node> newRoute = collectionRoute.GetRouteCopy();
    (bool isFactible, double newRouteTime) = IsFactibleRoute(newRoute, collectionRoute.RouteTime, changingPosition, insertionPosition);
    if (!isFactible)
      return null;
    newRouteTime = RemoveUnnecessaryNodes(newRoute, changingPosition, newRouteTime);
    return new CollectionRoute(newRoute, new(), newRouteTime);
    
  }

  /// <summary>
  /// Checks if a proposed reinsertion of a node is feasible by evaluating route time and capacity constraints.
  /// </summary>
  /// <param name="route">The route to evaluate.</param>
  /// <param name="currentRouteTime">The current time of the route before the reinsertion.</param>
  /// <param name="changingPosition">The index of the node to move.</param>
  /// <param name="insertionPosition">The new position for the node.</param>
  /// <returns>A tuple with a boolean indicating whether the move is feasible and the new route time.</returns>
  private Tuple<bool, double> IsFactibleRoute(List<Node> route, double currentRouteTime, int changingPosition, int insertionPosition)
  {
    double newRouteTime = CalculateNewRouteTime(route, currentRouteTime, changingPosition, insertionPosition);
    if (newRouteTime > _problemInstance.CollectionTruck.WorkDuration)
      return new(false, newRouteTime);

    Node changingNode = route[changingPosition];
    route.RemoveAt(changingPosition);
    route.Insert(insertionPosition, changingNode);
    return new(IsFactibleCapacity(route, insertionPosition), newRouteTime);
  }

  /// <summary>
  /// Calculates the new total collection time of the route after the reinsertion of a node.
  /// </summary>
  /// <param name="route">The route to evaluate.</param>
  /// <param name="currentRouteTime">The current collection time of the route.</param>
  /// <param name="changingPosition">The position of the node being moved.</param>
  /// <param name="insertionPosition">The new position for the node.</param>
  /// <returns>The new collection time after the reinsertion.</returns>
  private double CalculateNewRouteTime(List<Node> route, double currentRouteTime, int changingPosition, int insertionPosition)
  {
    int fixedInsertionPosition = (insertionPosition < changingPosition) ? insertionPosition - 1 : insertionPosition;
    int truckSpeed = _problemInstance.CollectionTruck.Speed;
    Node changingNode = route[changingPosition];
    Node previousChangingNode = route[changingPosition - 1];
    Node nextChangingNode = route[changingPosition + 1];
    Node previousNewNode = route[fixedInsertionPosition];
    Node nextNewNode = route[fixedInsertionPosition + 1];
    double newRouteTime = currentRouteTime - 
                          (Node.DistanceBetween(previousChangingNode, changingNode) / truckSpeed * 60) -
                          (Node.DistanceBetween(changingNode, nextChangingNode) /truckSpeed * 60) +
                          (Node.DistanceBetween(previousChangingNode, nextChangingNode) / truckSpeed * 60) -
                          (Node.DistanceBetween(previousNewNode, nextNewNode) / truckSpeed * 60) + 
                          (Node.DistanceBetween(previousNewNode, changingNode) / truckSpeed * 60) +
                          (Node.DistanceBetween(changingNode, nextNewNode) / truckSpeed * 60);
    return newRouteTime;
  }

  /// <summary>
  /// Removes unnecessary nodes from the route to optimize the collection time.
  /// </summary>
  /// <param name="route">The current route.</param>
  /// <param name="changingPosition">The position of the node being considered for removal.</param>
  /// <param name="currentRouteTime">The current collection time of the route.</param>
  /// <returns>The updated collection time after the unnecessary node removal.</returns>
  private double RemoveUnnecessaryNodes(List<Node> route, int changingPosition, double currentRouteTime)
  {
    if (route[changingPosition] is not CollectionArea)
    {
      int removePosition;
      if (route[changingPosition - 1] is not CollectionArea)
        removePosition = changingPosition;
      else if (route[changingPosition + 1] is not CollectionArea)
        removePosition = changingPosition + 1;
      else
        return currentRouteTime;
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