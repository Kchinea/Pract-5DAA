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
        // for(int j = 0; j < _instance.Count; j++) {
        //   Console.WriteLine(new string('-', 152));
        //   Console.WriteLine($"Algorithm: {_algorithms[i].GetName}");
        //   Console.WriteLine(new string('-', 152));
        //   _algorithms[i].Solve(_instance[j]);
        // }
    // }
  }
  private void PrintGrasp(int i){
    Console.WriteLine();
  }
  private void PrintGreedy(int iter){
    Console.WriteLine($"| {"Instance", -27} | {"#Zonas", -27} | {"#CV", -27} | {"#TV", -27} | {"CPU_Time", -28} |");
    Console.WriteLine(new string('-', 152));
    for(int i = 0; i < _instance.Count; i++) {
    Console.WriteLine($"| {_instance[i].Name, -27} | {_instance[i].Zones.Zones.Count, -27} | {_algorithms[iter].Solve(_instance[i]), -27} | {"0", -27} | {"0", -28} |");
    Console.WriteLine();
    }
  }
}