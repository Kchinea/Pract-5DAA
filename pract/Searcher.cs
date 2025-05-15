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
  public Solution Run(Solution solution, Instance instance) {
    while (_active.Contains(true)) {
        int actives = _active.FindAll(x => x == true).Count;
        int index = _rand.Next(0, actives);
        int counter = 0;
        for(int i = 0; i < _active.Count; i++) {
            if (_active[i]) {
                counter++;
                if (counter == index) {
                    bool improve = true;
                    while (improve) {
                        ILocalSearch localSearch = _localSearches[i];
                        Solution newSolution = localSearch.Solve(solution, instance.Zones);
                        Console.WriteLine($"LocalSearch: {localSearch.GetName} - {newSolution.TotalTime}, {solution.TotalTime}");
                        if (newSolution.TotalTime < solution.TotalTime) {
                            solution = newSolution;
                            for (int j = 0; j < _active.Count; j++) {
                                _active[j] = true;
                            }
                        } else {
                            _active[i] = false;
                            improve = false;
                        }
                    }
                }
            }
        }
    }
    return solution;
}

}