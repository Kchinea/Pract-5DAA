

using Pract5DAA.LocalSearch;

namespace Pract5DAA;

public class Searcher {
  private List<ILocalSearch> _localSearches;
  private Random _rand = new Random();

  public Searcher(List<ILocalSearch> localSearches) {
    _localSearches = localSearches;
  }

  public Solution Run(Solution solution, Instance instance) {
    // Copia la lista de búsquedas locales para manipularla
    var activeSearches = new List<ILocalSearch>(_localSearches);
    Solution bestSolution = new Solution(solution.Trucks.Select(t => t.Clone()).ToList());

    while (activeSearches.Count > 0) {
      // Selecciona una búsqueda local al azar
      int idx = _rand.Next(activeSearches.Count);
      var localSearch = activeSearches[idx];

      // Aplica la búsqueda local
      Solution neighbor = localSearch.Solve(bestSolution, instance.Zones);

      // Si mejora, actualiza la mejor solución y reinicia la lista de búsquedas
      if (neighbor.TotalTime < bestSolution.TotalTime) {
        bestSolution = neighbor;
        activeSearches = new List<ILocalSearch>(_localSearches);
      } else {
        // Si no mejora, elimina esta búsqueda local de la lista activa
        activeSearches.RemoveAt(idx);
      }
    }

    return bestSolution;
  }
}