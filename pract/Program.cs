﻿using Pract5DAA.Algorithm;
using Pract5DAA.LocalSearch;
// using Spectre.Console;

namespace Pract5DAA;
internal class Program {
  static void Main(string[] args) {
    // Verificar que se pasa el archivo como argumento
    if(args.Length != 1) {
      Console.WriteLine(args.Length);
      return;
    }
    
    // Lectura de la instancia
    string file = args[0];
    Reader reader = new Reader(file);
    List<Instance> instances = reader.ReadAll();

    // Ejemplo de ejecución de otros algoritmos
    Algorithm1 algorithm1 = new Algorithm1();
    int num_ejecutions = 3;
    Algorithm2 algorithm2 = new Algorithm2(num_ejecutions);
    List<IAlgorithm> algorithms = new List<IAlgorithm> { algorithm1, algorithm2 };

    // Seleccionamos una instancia y resolvemos usando el segundo algoritmo
    Solution solution = algorithms[1].Solve(instances[7]);
    Console.WriteLine($"Solution: {solution.NumVehicles}");
    Console.WriteLine($"Total Time: {solution.TotalTime}");
    Console.WriteLine($"Total Load: {solution.TotalLoad}");

    // Preparar el RVND con un conjunto de búsquedas locales
    // Aquí agregamos las búsquedas locales que quieres usar dentro del RVND
    List<ILocalSearch> localSearches = new List<ILocalSearch> {
      new IntraAddLocalSearch(solution, instances[7]),
      new IntraSwapLocalSearch(solution, instances[7]),
      new InterAddLocalSearch(solution, instances[7]),
      new InterSwapLocalSearch(solution, instances[7])
    };

    // Crear el objeto RVND con las búsquedas locales y las instancias
    RVND rvnd = new RVND(localSearches, instances, solution);

    // Ejecutar el RVND
    rvnd.DoRVND();
    
    // Como ejemplo, si quieres mostrar la mejor solución final obtenida por RVND:
    Console.WriteLine($"Solution after RVND: {solution.NumVehicles}");
    Console.WriteLine($"Total Time after RVND: {solution.TotalTime}");
    Console.WriteLine($"Total Load after RVND: {solution.TotalLoad}");

    // AlgoritmoFinal algoritmoFinal = new AlgoritmoFinal();
    // solution = algoritmoFinal.Solve(solution, instances[4]);

    // Console.WriteLine($"Solution after RVND: {solution.NumVehicles}");
    // Console.WriteLine($"Total Time after RVND: {solution.TotalTime}");
    // Console.WriteLine($"Total Load after RVND: {solution.TotalLoad}");





    // Comentar partes no utilizadas del código:
    // Tester tester = new Tester(algorithms, instances);
    // tester.DoTest();

    // Usar Spectre.Console para mostrar tablas u otros gráficos si es necesario
    // var table = new Table();
    // table.AddColumn("Truck ID");
    // table.AddColumn("Capacity");
    // table.AddColumn("Current Load");
    // table.AddRow("1", "100", "50");
    // table.AddRow("2", "200", "150");
    // table.AddRow("3", "300", "250");
    // AnsiConsole.Write(table);
    // AnsiConsole.Write(new BarChart()
    // .Width(60)
    // .Label("[green bold underline]Number of fruits[/]")
    // .CenterLabel()
    // .AddItem("Apple", 12, Color.Yellow)
    // .AddItem("Orange", 54, Color.Green)
    // .AddItem("Banana", 33, Color.Red));
  }
}
