﻿using Pract5DAA.Algorithm;
using Spectre.Console;

namespace Pract5DAA;
internal class Program {
  static void Main(string[] args) {
    if(args.Length != 1) {
      Console.WriteLine(args.Length);
      return;
    }
    string file = args[0];
    Algorithm1 algorithm1 = new Algorithm1();
    int num_ejecutions = 3;
    Algorithm2 algorithm2 = new Algorithm2(num_ejecutions);
    List<IAlgorithm> algorithms = new List<IAlgorithm> { algorithm1, algorithm2};

    Reader reader = new Reader(file);
    List<Instance> instances = reader.ReadAll();
    // List<Instance> instances = new List<Instance> { instance };

    Tester tester = new Tester(algorithms, instances);
    tester.DoTest();
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