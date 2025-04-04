using Pract5DAA.Algorithm;
using Spectre.Console;

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
    DoTable(new List<string> { "Instance", "#Zonas", "|LRC|", "Ejecucion", "#CV", "#TV", "CPU_Time" });
    // Console.WriteLine($"| {"Instance", -22} | {"#Zonas", -18} |{"|LRC|", -18} | {"Ejecucion", -18}| {"#CV", -18} | {"#TV", -19} | {"CPU_Time", -19} |");
    // Console.WriteLine(new string('-', 152));
    for(int i = 0; i < _instance.Count; i++) {
      for(int ejecution = 1; ejecution <= algorithm.NumEjecutions; ejecution++) {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        int solution = algorithm.Solve(_instance[i]);
        watch.Stop();
        var elapsedNs = watch.Elapsed.Microseconds;
        _table.AddRow(_instance[i].Name, _instance[i].Zones.Zones.Count.ToString(), "0", ejecution.ToString(), solution.ToString(), "0", elapsedNs.ToString());
        // Console.WriteLine($"| {_instance[i].Name, -22} | {_instance[i].Zones.Zones.Count, -18} | {"0", -18} | {ejecution, -18} | {solution, -18} | {"0", -18} | {elapsedNs, -18} |");
        // Console.WriteLine(new string('-', 152));
      }
    }
    AnsiConsole.Write(_table);
    Console.WriteLine();
  }
  private void PrintGreedy(int iter){
    Algorithm1 algorithm = (Algorithm1)_algorithms[iter];
    DoTable(new List<string> { "Instance", "#Zonas", "#CV", "#TV", "CPU_Time" });
    for(int i = 0; i < _instance.Count; i++) {
      var watch = System.Diagnostics.Stopwatch.StartNew();
      int solution = algorithm.Solve(_instance[i]);
      watch.Stop();
      var elapsedNs = watch.Elapsed.Microseconds;
      _table.AddRow(_instance[i].Name, _instance[i].Zones.Zones.Count.ToString(), solution.ToString(), "0", elapsedNs.ToString());
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