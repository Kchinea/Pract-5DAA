/*
 * CollectionRoute.cs
 * 
 * This file defines the `CollectionRoute` class, which models a route followed by a collection truck
 * to visit a sequence of nodes (areas) in the context of the VRPT-SWTS problem
 * (Vehicle Routing Problem with Trash collection and Soft Time and Service Windows).
 * 
 * A collection route includes:
 * - A list of nodes representing the path.
 * - A list of indices (`subRoutes`) marking sub-routes (e.g., segments between depot returns).
 * - The total time required to complete the entire route.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.Graph;

namespace VrptSwts.InstanceManagement;

/// <summary>
/// Represents a route used for waste collection, containing a list of visited nodes (areas),
/// sub-route delimiters, and the total time of execution.
/// </summary>
internal class CollectionRoute(List<Node> route, List<int> subRoutes, double routeTime)
{

  /// <summary>
  /// List of nodes (areas) visited in this route.
  /// </summary>
  private List<Node> _route = route;

  /// <summary>
  /// List of indices representing the start of each sub-route.
  /// Useful for distinguishing segments within the main route.
  /// </summary>
  private List<int> _subRoutes = subRoutes;

  /// <summary>
  /// Total time taken to complete the collection route.
  /// </summary>
  public double RouteTime { get; } = routeTime;

  /// <summary>
  /// Returns a shallow copy of the route.
  /// </summary>
  /// <returns>A new list containing the same node references as the internal route.</returns>
  public List<Node> GetRouteCopy()
  {
    return new(_route);
  }

  /// <summary>
  /// Retrieves a node from the route at the specified position.
  /// </summary>
  /// <param name="position">The index of the desired node.</param>
  /// <returns>The node at the given position.</returns>
  public Node GetArea(int position)
  {
    return _route[position];
  }

  /// <summary>
  /// Returns a reference to the internal route list.
  /// Warning: modifications to the returned list will affect the internal state.
  /// </summary>
  /// <returns>The internal list of nodes.</returns>
  public List<Node> GetRoute()
  {
    return _route;
  }

  /// <summary>
  /// Returns the number of nodes in the route.
  /// </summary>
  /// <returns>The count of visited areas.</returns>
  public int GetRouteLength()
  {
    return _route.Count;
  }

  /// <summary>
  /// Returns a string representation of the route.
  /// Format: node1 -> node2 -> node3 ...
  /// </summary>
  /// <returns>A string showing the sequence of visited area IDs.</returns>
  public override string ToString()
  {
    string stringRoute = "";
    for (int i = 0; i < _route.Count; ++i)
    {
      stringRoute += _route[i].Id;
      if (i < _route.Count - 1)
        stringRoute += " -> ";
    }
    return stringRoute;
  }
}