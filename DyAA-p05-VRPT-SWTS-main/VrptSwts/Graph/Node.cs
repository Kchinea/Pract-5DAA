/*
 * Node.cs
 * 
 * This file defines the `Node` class, which is a core component of the graph used in the
 * VRPT-SWTS (Vehicle Routing Problem with Trash collection and Soft Time and Service Windows).
 * 
 * A `Node` represents a point in the graph, typically an area or location to be visited.
 * It is identified by a unique string ID and has 2D integer coordinates (x, y).
 * 
 * The class also provides a static method to compute the Euclidean distance between any
 * two nodes.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

namespace VrptSwts.Graph;

/// <summary>
/// Represents a node in the VRPT-SWTS graph with a unique ID and coordinates.
/// </summary>
/// <param name="id">Unique identifier for the node.</param>
/// <param name="coordinates">Tuple representing (x, y) coordinates of the node.</param>
internal class Node(string id, Tuple<int, int> coordinates)
{

  /// <summary>
  /// Calculates the Euclidean distance between two nodes using their coordinates.
  /// </summary>
  /// <param name="firstNode">The first node.</param>
  /// <param name="secondNode">The second node.</param>
  /// <returns>Euclidean distance between the two nodes.</returns>
  public static double DistanceBetween(Node firstNode, Node secondNode)
  {
    Tuple<int, int> currentCoords = firstNode.Coordinates;
    Tuple<int, int> nextCoords = secondNode.Coordinates;
    double firtTerm = Math.Pow(nextCoords.Item1 - currentCoords.Item1, 2);
    double secondTerm = Math.Pow(nextCoords.Item2 - currentCoords.Item2, 2);
    return Math.Sqrt(firtTerm + secondTerm);
  }

  /// <summary>
  /// Gets the identifier of the node.
  /// </summary>
  public string Id { get; } = id;

  /// <summary>
  /// Gets the (x, y) coordinates of the node.
  /// </summary>
  public Tuple<int, int> Coordinates { get; } = coordinates;
}