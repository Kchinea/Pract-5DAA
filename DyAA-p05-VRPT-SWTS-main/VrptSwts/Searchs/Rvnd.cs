/*
 * Rvnd.cs
 * 
 * The `Rvnd` class implements the Restricted Variable Neighborhood Descent (RVND) algorithm.
 * It is used as a local search method within the broader search framework for the Vehicle Routing Problem with Time Windows and Soft Constraints (RPT-SWTS).
 * RVND is an iterative algorithm that explores different neighborhoods by applying a sequence of local search strategies, selecting the best solution from the available ones.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.Searchs.LocalSearchs;
using VrptSwts.InstanceManagement;

namespace VrptSwts.Searchs;

/// <summary>
/// The `Rvnd` class implements the Restricted Variable Neighborhood Descent (RVND) algorithm for solving the RPT-SWTS problem.
/// It iteratively explores different neighborhoods using a set of local search strategies until a local minimum is found.
/// </summary>
internal class Rvnd(LocalSearchesList localSearches)
{

  /// <summary>
  /// List of local searches that are used in the RVND algorithm.
  /// </summary>
  private LocalSearchesList _localSearches = localSearches;
  
  /// <summary>
  /// Performs the Restricted Variable Neighborhood Descent (RVND) algorithm.
  /// This method iteratively improves an initial solution by applying local searches.
  /// If a better solution (in terms of collection time) is found, it updates the best solution and resets the local search list.
  /// The process continues until no further improvements can be made.
  /// </summary>
  /// <param name="initialRoutes">The initial solution, represented as a set of routes.</param>
  /// <returns>Returns the best set of routes found after applying the RVND algorithm.</returns>
  public Routes Search(Routes initialRoutes)
  {
    Routes bestRoutes = initialRoutes;
    while(!_localSearches.IsEmpty())
    {
      LocalSearch localSearch = _localSearches.GetRandomSearch();
      Routes bestNeighbour = localSearch.SearchLocalMinimum(bestRoutes);
      if (bestRoutes.GetCollectionTime() > bestNeighbour.GetCollectionTime())
      {
        bestRoutes = bestNeighbour;
        _localSearches.Reset();
      }
    }
    _localSearches.ResetAll();
    return bestRoutes;
  }

}