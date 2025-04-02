using Pract5DAA.Algorithm;

namespace Pract5DAA;

public class Tester {
  private List<IAlgorithm> _algorithms;
  private List<Instance> _instance;
  public Tester(List<IAlgorithm> algorithms, List<Instance> instance) {
    _algorithms = algorithms;
    _instance = instance;
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
    Console.WriteLine($"| {"Instance", -22} | {"#Zonas", -18} |{"|LRC|", -18} | {"Ejecucion", -18}| {"#CV", -18} | {"#TV", -19} | {"CPU_Time", -19} |");
    Console.WriteLine(new string('-', 152));
    for(int i = 0; i < _instance.Count; i++) {
      for(int ejecution = 1; ejecution <= algorithm.NumEjecutions; ejecution++) {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        int solution = algorithm.Solve(_instance[i]);
        watch.Stop();
        var elapsedNs = watch.Elapsed.Microseconds;
        Console.WriteLine($"| {_instance[i].Name, -22} | {_instance[i].Zones.Zones.Count, -18} | {"0", -18} | {ejecution, -18} | {solution, -18} | {"0", -18} | {elapsedNs, -18} |");
        Console.WriteLine(new string('-', 152));
      }
    }
    Console.WriteLine();
  }
  private void PrintGreedy(int iter){
    Algorithm1 algorithm = (Algorithm1)_algorithms[iter];
    Console.WriteLine($"| {"Instance", -27} | {"#Zonas", -27} | {"#CV", -27} | {"#TV", -27} | {"CPU_Time", -28} |");
    Console.WriteLine(new string('-', 152));
    for(int i = 0; i < _instance.Count; i++) {
      var watch = System.Diagnostics.Stopwatch.StartNew();
      int solution = algorithm.Solve(_instance[i]);
      watch.Stop();
      var elapsedNs = watch.Elapsed.Microseconds;
      Console.WriteLine($"| {_instance[i].Name, -27} | {_instance[i].Zones.Zones.Count, -27} | {solution, -27} | {"0", -27} | {elapsedNs, -28} |");
      Console.WriteLine(new string('-', 152));
    }
    Console.WriteLine();
  }
}