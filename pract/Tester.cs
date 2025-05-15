using Pract5DAA.Algorithm;
using Spectre.Console;
using Pract5DAA.LocalSearch;

namespace Pract5DAA;

public class Tester {
  private List<IAlgorithm> _algorithms;
  private List<Instance> _instance;
  private Table _table;
  public Tester(List<IAlgorithm> algorithms, List<Instance> instance) {
    _algorithms = algorithms;
    _instance = instance;
    _table = new Table();
  }
  public void DoTest(){
    for(int i = 0; i < _algorithms.Count; i++) {
      Console.WriteLine(new string('-', 152));
      Console.WriteLine($"Algorithm: {_algorithms[i].GetName}");
      Console.WriteLine(new string('-', 152));
      if(_algorithms[i].GetName == "GRASP") {
        PrintGrasp(i);
        continue;
      } else if(_algorithms[i].GetName == "Voraz") {
        PrintGreedy(i);
      }
    }
  }
  private void PrintGrasp(int iter){
    Algorithm2 algorithm = (Algorithm2)_algorithms[iter];
    DoTable(new List<string> { "Instance", "#Zonas", "|LRC|", "Ejecucion", "#CV", "Totally_Time", "#TV", "CPU_Time" });
    for(int i = 0; i < _instance.Count; i++) {
      for(int ejecution = 1; ejecution <= algorithm.NumEjecutions; ejecution++) {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Solution solution = algorithm.Solve(_instance[i]);
        IntraAddLocalSearch IntraAddLocalSearch = new IntraAddLocalSearch();
        IntraSwapLocalSearch IntraSwapLocalSearch = new IntraSwapLocalSearch();
        InterAddLocalSearch InterAddLocalSearch = new InterAddLocalSearch();
        InterSwapLocalSearch InterSwapLocalSearch = new InterSwapLocalSearch();
        Searcher searcher = new Searcher(new List<ILocalSearch> { IntraAddLocalSearch, IntraSwapLocalSearch});
        // Solution solutionMov = new Solution(solution.Trucks.Select(t => t.Clone()).ToList());
        watch.Stop();
        var elapsedNs = watch.Elapsed.Microseconds;
        _table.AddRow(_instance[i].Name, _instance[i].Zones.Zones.Count.ToString(), "0", ejecution.ToString(), solution.NumVehicles.ToString(), solution.TotalTime.ToString(), "0", elapsedNs.ToString());
        var watchPlus = System.Diagnostics.Stopwatch.StartNew();
        solution = searcher.Run(solution, _instance[i]);
        watchPlus.Stop();
        // Console.WriteLine($"Algorithm sin: {_instance[i].Name}");
        // Solution solutionMov = InterSwapLocalSearch.Solve(solution, _instance[i].Zones);
        // Solution solutionMov = InterAddLocalSearch.Solve(solution, _instance[i].Zones);
        var elapsedNsPlus = watchPlus.Elapsed.Microseconds;

        _table.AddRow(_instance[i].Name, _instance[i].Zones.Zones.Count.ToString(), "0", ejecution.ToString(), solution.NumVehicles.ToString(), solution.TotalTime.ToString(), "0", elapsedNsPlus.ToString());
      }
    }
    AnsiConsole.Write(_table);
    Console.WriteLine();
  }
  private void PrintGreedy(int iter){
    Algorithm1 algorithm = (Algorithm1)_algorithms[iter];
    DoTable(new List<string> { "Instance", "#Zonas", "#CV","Totally_Time", "#TV", "CPU_Time" });
    for(int i = 0; i < _instance.Count; i++) {
      var watch = System.Diagnostics.Stopwatch.StartNew();
      Solution solution = algorithm.Solve(_instance[i]);
      watch.Stop();
      var elapsedNs = watch.Elapsed.Microseconds;
      _table.AddRow(_instance[i].Name, _instance[i].Zones.Zones.Count.ToString(), solution.NumVehicles.ToString(), solution.TotalTime.ToString(), "0", elapsedNs.ToString());
    }
    AnsiConsole.Write(_table);
    Console.WriteLine();
  }
  private void DoTable(List<string> column){
    _table = new Table();
    foreach(string element in column) {
      _table.AddColumn(element);
    }
  }
}