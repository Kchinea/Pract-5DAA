/*
 * VrptSwtsMultiStart.cs
 * 
 * This file defines the `VrptSwtsMultiStart` class which implements a Multi-Start metaheuristic
 * for solving the VRPT-SWTS (Vehicle Routing Problem with Trash collection and Soft Time and Service Windows).
 * 
 * The algorithm combines a GRASP (Greedy Randomized Adaptive Search Procedure) construction method with 
 * a local improvement procedure called SequentialSearch.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.InstanceManagement;
using VrptSwts.Searchs;
using VrptSwts.Searchs.LocalSearchs;
using VrptSwts.Vehicles;

namespace VrptSwts.Algorithms;

/// <summary>
/// Implements a Multi-Start heuristic combining GRASP and SequentialSearch for solving the VRPT-SWTS.
/// </summary>
internal class VrptSwtsMultiStart
{
  private readonly VrptSwtsGrasp _grasp;
  private readonly SequentialSearch _sequentialSearch;
  private readonly int ITERATIONS = 20;

  /// <summary>
  /// Constructor that initializes the GRASP builder and the local search methods for RVND.
  /// </summary>
  /// <param name="problemInstance">The VRPT-SWTS problem instance.</param>
  /// <param name="randomWindow">Window size for randomized greedy selection in GRASP.</param>
  public VrptSwtsMultiStart(VrptSwtsInstance problemInstance, int randomWindow)
  {
    _grasp = new(problemInstance.Clone(), randomWindow);
    List<LocalSearch> localSearches = new([
      new ReinsertionIntra(problemInstance.Clone()),
      new ReinsertionInter(problemInstance.Clone()),
      new SwapIntra(problemInstance.Clone()),
      new SwapInter(problemInstance.Clone()),
      new TwoOpt(problemInstance.Clone())
    ]);
    _sequentialSearch = new(localSearches);
  }

  /// <summary>
  /// Executes the Multi-Start search process.
  /// Builds and improves multiple solutions, retaining the best one.
  /// </summary>
  /// <returns>The best set of routes found, including collection and transport routes.</returns>
  public Routes Run()
  {
    Routes initialRoutes = _grasp.BuildRoutes();
    Routes bestRoutes = _sequentialSearch.Search(initialRoutes);
    for (int i = 0; i < ITERATIONS; ++i)
    {
      Routes graspRoutes = _grasp.BuildRoutes();
      Routes localMinimum = _sequentialSearch.Search(graspRoutes);
      if (bestRoutes.GetCollectionTime() > localMinimum.GetCollectionTime())
        bestRoutes = localMinimum;
    }
    _grasp.GenerateTransportTasks(bestRoutes.GetCollectionRoutesCopy());
    List<TransportTruck> transportTrucks = _grasp.BuildTransportRoutes();
    return new(bestRoutes.GetCollectionRoutesCopy(), transportTrucks);
  }
}