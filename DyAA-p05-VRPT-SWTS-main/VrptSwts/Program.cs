using VrptSwts.Testing;
using VrptSwts.Utils;
using VrptSwts.InstanceManagement;
using VrptSwts.Algorithms;
using VrptSwts.Searchs.LocalSearchs;
using VrptSwts.Searchs;
using VrptSwts.Vehicles;

namespace VrptSwts;

class Program
{
  /// <summary>
  /// Punto de entrada principal del programa.
  /// Se encarga de generar instancias de configurar los algoritmos y ejecutar las pruebas.
  /// </summary>
  /// <param name="args">Argumentos de línea de comandos proporcionados al programa.</param>
  /// <returns>Un código de salida: 0 si la ejecución fue exitosa, 1 si ocurrió un error.</returns>
  static int Main(string[] args)
  {
    try
    {
      ArgumentParser parser = new(args);
      VrptSwtsTester tester = new VrptSwtsTester("./Instances");
      if (parser.ExecutionMode == "greedy")
        tester.RunGreedy();
      else if (parser.ExecutionMode == "grasp")
        tester.RunGrasp(parser.ExecutionTimes, parser.LocalSearch);
      else
        tester.RunMultiStart(parser.ExecutionTimes);
    }
    catch (Exception error)
    {
      Console.WriteLine(error.Message);
      return 1;
    }
    return 0;
  }
}