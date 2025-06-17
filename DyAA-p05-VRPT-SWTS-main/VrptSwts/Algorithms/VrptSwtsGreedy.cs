/*
 * VrptSwtsGreedy.cs
 * 
 * This file implements a greedy algorithm for solving the Route Planning 
 * for Trash Collection with Soft Time and Service Windows (RPT-SWTS) problem.
 * The greedy strategy consists in choosing the closest neighbor at each step 
 * while building routes for the collection vehicles.
 * 
 * This class inherits from the abstract class `VrptSwtsResolver`, which defines 
 * the general structure for solving VRPT-SWTS instances, and it overrides the 
 * method `GetClosestNeighbour` to provide a greedy implementation.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.InstanceManagement;
using VrptSwts.Graph;

namespace VrptSwts.Algorithms;

/// <summary>
/// Implements a greedy heuristic to solve the VRPT-SWTS problem by always 
/// selecting the closest neighbor node (e.g., next collection area or transfer station).
/// </summary>
internal class VrptSwtsGreedy : VrptSwtsResolver
{

  /// <summary>
  /// Constructor for the greedy solver. It initializes the base resolver 
  /// with a given problem instance.
  /// </summary>
  /// <param name="problemInstance">An instance of the VRPT-SWTS problem.</param>
  public VrptSwtsGreedy(VrptSwtsInstance problemInstance) : base(problemInstance) {}

  /// <summary>
  /// Greedy implementation of the method to get the closest neighbor node 
  /// from a list of candidate nodes.
  /// </summary>
  /// <param name="currentArea">The current node (usually a collection area or depot).</param>
  /// <param name="neightbours">List of candidate nodes (e.g., remaining collection areas).</param>
  /// <returns>
  /// A tuple containing:
  /// - The closest neighbor node.
  /// - The distance to that node.
  /// - The index of the node in the original list.
  /// </returns>
  protected override Tuple<Node, double, int> GetClosestNeighbour(Node currentArea,
                                                                  List<Node> neightbours)
  {
    int closestNeighbourIndex = 0;
    double minDistance = Node.DistanceBetween(currentArea, neightbours[0]);
    for (int i = 1; i < neightbours.Count; ++i)
    {
      Node nextArea = neightbours[i];
      double distance = Node.DistanceBetween(currentArea, nextArea);
      if (distance < minDistance)
      {
        closestNeighbourIndex = i;
        minDistance = distance;
      }
    }
    Node closestNeighbour = neightbours[closestNeighbourIndex];
    return new(closestNeighbour, minDistance, closestNeighbourIndex);
  }
}