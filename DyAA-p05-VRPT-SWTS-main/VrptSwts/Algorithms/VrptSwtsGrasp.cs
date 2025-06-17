/*
 * VrptSwtsGrasp.cs
 * 
 * This file defines the implementation of the GRASP (Greedy Randomized Adaptive Search Procedure) 
 * algorithm for solving the Route Planning for Trash Collection with Soft Time and Service Windows (VRPT-SWTS) problem.
 * 
 * GRASP is a multi-start metaheuristic that builds multiple randomized greedy solutions and keeps
 * the best one found. Each iteration constructs a route solution using a randomized greedy approach 
 * and compares it with the current best based on the number of routes and total collection time.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.InstanceManagement;
using VrptSwts.Graph;
using VrptSwts.Searchs.LocalSearchs;
using VrptSwts.Vehicles;

namespace VrptSwts.Algorithms;

/// <summary>
/// Implements the GRASP heuristic for the VRPT-SWTS problem.
/// The algorithm iteratively generates randomized greedy solutions and selects the best one.
/// </summary>
internal class VrptSwtsGrasp : VrptSwtsResolver
{
  private const int ACURRACY  = 20;
  private readonly int _randomWindow;
  private readonly VrptSwtsInstance _instanceCopy;

  /// <summary>
  /// Constructor for the GRASP solver.
  /// </summary>
  /// <param name="problemInstance">The VRPT-SWTS problem instance.</param>
  /// <param name="randomWindow">The number of top candidates to randomly select from during neighbor choice.</param>
  public VrptSwtsGrasp(VrptSwtsInstance problemInstance,
                       int randomWindow) : base(problemInstance)
  {
    _randomWindow = randomWindow;
    _instanceCopy = problemInstance.Clone();
  }

  /// <summary>
  /// Constructs the routes using the GRASP strategy.
  /// It performs multiple iterations and retains the best set of collection routes.
  /// </summary>
  /// <returns>A complete set of collection and transport routes.</returns>
  public override Routes BuildRoutes()
  {
    _problemInstance = _instanceCopy.Clone();
    List<CollectionRoute> bestCollectionRoutes = BuildCollectionRoutes();
    for (int i = 0; i < ACURRACY; ++i)
    {
      _problemInstance = _instanceCopy.Clone();
      List<CollectionRoute> collectionRoutes = BuildCollectionRoutes();
      Routes bestRoutes = new(bestCollectionRoutes);
      Routes routes = new(collectionRoutes);
      if (routes.GetNumberCollectionRoutes() < bestRoutes.GetNumberCollectionRoutes())
      {
        bestCollectionRoutes = collectionRoutes;
      }
      if (routes.GetNumberCollectionRoutes() == bestRoutes.GetNumberCollectionRoutes() &&
          routes.GetCollectionTime() < bestRoutes.GetCollectionTime())
      {
        bestCollectionRoutes = collectionRoutes;
      }
    }
    GenerateTransportTasks(bestCollectionRoutes);
    List<TransportTruck> transportTrucks = BuildTransportRoutes();
    return new(bestCollectionRoutes, transportTrucks);
  }

  /// <summary>
  /// Selects a neighbor using a randomized greedy strategy.
  /// Builds a candidate list of the closest neighbors and chooses one at random within the top-k.
  /// </summary>
  /// <param name="currentArea">The current node (collection area or depot).</param>
  /// <param name="neightbours">List of candidate neighbors to choose from.</param>
  /// <returns>
  /// A tuple containing:
  /// - The selected neighbor node.
  /// - The distance to that node.
  /// - The index of the node in the original list.
  /// </returns>
  protected override Tuple<Node, double, int> GetClosestNeighbour(Node currentArea,
                                                                  List<Node> neightbours)
  {
    int randomWindow = Math.Min(neightbours.Count, _randomWindow);
    Tuple<Node, double, int>[] distances = new Tuple<Node, double, int>[neightbours.Count];
    for (int i = 0; i < neightbours.Count; ++i)
    {
      Node nextArea = neightbours[i];
      double distance = Node.DistanceBetween(currentArea, nextArea);
      distances[i] = new(nextArea, distance, i);
    }
    Array.Sort(distances, (a, b) => a.Item2.CompareTo(b.Item2));
    Random random = new();
    int randomIndex = random.Next(0, randomWindow);
    return distances[randomIndex];
  }
}