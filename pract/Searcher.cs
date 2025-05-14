using Pract5DAA.LocalSearch;

namespace Pract5DAA;

public class Searcher {
  private List<bool> _active;
  private List<ILocalSearch> _localSearches;
  private Random _rand = new Random();

  public Searcher(List<ILocalSearch> localSearches) {
    _localSearches = localSearches;
    _active = new List<bool>();
    for (int i = 0; i < _localSearches.Count; i++) {
      _active.Add(true);
    }
  }
  public void Run(Solution solution, int iterations, Instance instance) {
    // Inicializa todas activas
    for (int i = 0; i < _localSearches.Count; i++) {
        _active[i] = true;
    }

    while (_active.Contains(true)) {
        // Elige aleatoriamente una búsqueda local activa
        var activeIndices = _active
            .Select((isActive, idx) => new { isActive, idx })
            .Where(x => x.isActive)
            .Select(x => x.idx)
            .ToList();

        int index = activeIndices[_rand.Next(activeIndices.Count)];
        ILocalSearch localSearch = _localSearches[index];

        Solution newSolution = localSearch.Solve(solution, instance.Zones);
        Console.WriteLine($"LocalSearch: {localSearch.GetName} - {newSolution.TotalTime}");
        Console.WriteLine($"Solution: {solution.TotalTime}");
        Console.WriteLine($"Solution: {newSolution.TotalTime}");
        if (newSolution != null && newSolution.TotalTime < solution.TotalTime) {
            // Si mejora, actualiza la solución y reactiva todas
            solution = newSolution;
            for (int i = 0; i < _active.Count; i++) _active[i] = true;
        } else {
            // Si no mejora, desactiva esta búsqueda local
            _active[index] = false;
        }
    }
  }

}