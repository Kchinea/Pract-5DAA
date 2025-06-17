/*
 * Rvnd.cs
 * 
 * The `SequentialSearch` class implements the a local search algorithm.
 * It is used as a local search method within the broader search framework for the Vehicle Routing Problem with Time Windows and Soft Constraints (RPT-SWTS).
 * SequentialSearch runs sequentially a serie of different local searches (the output of the previous is the input of the next one)
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.Searchs.LocalSearchs;
using VrptSwts.InstanceManagement;

namespace VrptSwts.Searchs;

/// <summary>
/// The `SequentialSearch` class implements a sequential local search algorithm
/// </summary>
internal class SequentialSearch(List<LocalSearch> localSearches)
{

  /// <summary>
  /// List of local searches that are used in the Sequential Search algorithm.
  /// </summary>
  private List<LocalSearch> _localSearches = localSearches;
  
  /// <summary>
  /// Performs the Sequential Search algorithm.
  /// This method iteratively improves an initial solution by applying local searches.
  /// </summary>
  /// <param name="initialRoutes">The initial solution, represented as a set of routes.</param>
  /// <returns>Returns the set of routes after applying all the local searches.</returns>
  public Routes Search(Routes initialRoutes)
  {
    Routes localMinimum = initialRoutes;
    foreach(LocalSearch localSearch in _localSearches)
      localMinimum = localSearch.SearchLocalMinimum(localMinimum);
    return localMinimum;
  }

}