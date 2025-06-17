/*
 * LocalSearchesList.cs
 * 
 * This file defines the `LocalSearchesList` class, which manages a set of local search strategies
 * used within metaheuristic algorithms like RVND (Randomized Variable Neighborhood Descent).
 * 
 * The class maintains two separate lists of local search operators:
 * - Active searches: available to be randomly selected and applied.
 * - Inactive searches: those already used in the current iteration.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.Searchs.LocalSearchs;

namespace VrptSwts.InstanceManagement;

/// <summary>
/// Manages a collection of local search operators, providing mechanisms for random selection and reset.
/// </summary>
internal class LocalSearchesList(List<LocalSearch> localSearches)
{

  /// <summary>
  /// List of currently active local searches available for use.
  /// </summary>
  private List<LocalSearch> _activeSearches = localSearches;

  /// <summary>
  /// List of local searches that have already been used and are temporarily inactive.
  /// </summary>
  private List<LocalSearch> _inactiveSearches = new();

  /// <summary>
  /// Selects and removes a random local search from the active list,
  /// adding it to the inactive list to prevent reuse in the same iteration.
  /// </summary>
  /// <returns>A randomly selected local search operator.</returns>
  public LocalSearch GetRandomSearch()
  {
    Random random = new();
    int randomIndex = random.Next(_activeSearches.Count);
    LocalSearch localSearch = _activeSearches[randomIndex];
    _activeSearches.RemoveAt(randomIndex);
    _inactiveSearches.Add(localSearch);
    return localSearch;
  }

  /// <summary>
  /// Moves all but the last inactive local search back to the active list.
  /// Useful for partially reinitializing the neighborhood list.
  /// </summary>
  public void Reset()
  {
    while (_inactiveSearches.Count > 1)
    {
      LocalSearch localSearch = _inactiveSearches[0];
      _inactiveSearches.RemoveAt(0);
      _activeSearches.Add(localSearch);
    }
  }

  /// <summary>
  /// Moves all inactive local searches back to the active list.
  /// Used to fully restart the local search sequence.
  /// </summary>
  public void ResetAll()
  {
    while (_inactiveSearches.Count > 0)
    {
      LocalSearch localSearch = _inactiveSearches[0];
      _inactiveSearches.RemoveAt(0);
      _activeSearches.Add(localSearch);
    }
  }

  /// <summary>
  /// Checks whether there are no remaining active local search operators.
  /// </summary>
  /// <returns>True if the active search list is empty, false otherwise.</returns>
  public bool IsEmpty()
  {
    return _activeSearches.Count == 0;
  }
}