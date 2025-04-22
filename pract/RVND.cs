using Pract5DAA.Algorithm;
using Pract5DAA.LocalSearch;

namespace Pract5DAA;

public class RVND {
  private List<ILocalSearch> _searchs;  // Lista de búsquedas locales
  private List<Instance> _instances;    // Lista de instancias (posiblemente no se use aquí)
  private Solution _currentSolution;     // Solución actual
  private Random _random = new Random(); // Para obtener índices aleatorios

  public RVND(List<ILocalSearch> searchs, List<Instance> instances, Solution initialSolution) {
    _searchs = searchs;
    _instances = instances;
    _currentSolution = initialSolution;
  }

  public void DoRVND() {
    // Vector de booleanos para indicar qué búsquedas están habilitadas
    bool[] searchEnabled = new bool[_searchs.Count];
    for (int i = 0; i < _searchs.Count; i++) {
      searchEnabled[i] = true;  // Al principio todas las búsquedas están habilitadas
    }

    bool improvementFound;

    do {
      improvementFound = false;

      // Elegimos una búsqueda local aleatoria que esté habilitada
      List<int> enabledSearches = new List<int>();
      for (int i = 0; i < searchEnabled.Length; i++) {
        if (searchEnabled[i]) {
          enabledSearches.Add(i);
        }
      }

      // Si hay búsquedas habilitadas
      if (enabledSearches.Count > 0) {
        int randomIndex = enabledSearches[_random.Next(enabledSearches.Count)];
        ILocalSearch selectedSearch = _searchs[randomIndex];
        Console.WriteLine($"Ejecutando búsqueda {selectedSearch.GetName}");

        // Ejecutamos la búsqueda local seleccionada
        Solution newSolution = selectedSearch.Solve();

        // Comparamos el tiempo de la nueva solución con la solución actual
        if (newSolution.TotalTime < _currentSolution.TotalTime) {
          Console.WriteLine($"Mejora encontrada: {newSolution.TotalTime} < {_currentSolution.TotalTime}");
          _currentSolution = newSolution;  // Actualizamos la solución
          improvementFound = true;         // Se ha encontrado una mejora

          // Si mejora, habilitamos todas las búsquedas locales (todos `true`)
          for (int i = 0; i < searchEnabled.Length; i++) {
            searchEnabled[i] = true;
          }
        } else {
          // Si no mejora, deshabilitamos la búsqueda que acabamos de probar
          searchEnabled[randomIndex] = false;
          Console.WriteLine($"No mejora, deshabilitada la búsqueda {selectedSearch.GetName}");
        }
      } else {
        // Si todas las búsquedas están deshabilitadas, terminamos
        Console.WriteLine("Todas las búsquedas están deshabilitadas. Terminando RVND.");
      }

    } while (improvementFound);

    // Resultado final
    Console.WriteLine("Proceso completado. Mejor solución encontrada con tiempo total: " + _currentSolution.TotalTime);
  }
}
