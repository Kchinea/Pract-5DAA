﻿using Pract5DAA.Algorithm;

namespace Pract5DAA;
internal class Program {
  static void Main(string[] args) {
    if(args.Length != 1) {
      Console.WriteLine(args.Length);
      return;
    }
    string file = args[0];
    Algorithm1 algorithm1 = new Algorithm1();
    Algorithm2 algorithm2 = new Algorithm2();
    Algorithm3 algorithm3 = new Algorithm3();

    List<IAlgorithm> algorithms = new List<IAlgorithm> { algorithm1, algorithm2, algorithm3 };
    Reader reader = new Reader(file);
    Instance instance = reader.Read();
    Console.WriteLine("");
    foreach(IAlgorithm algorithm in algorithms) {
      algorithm.Solve(instance);
    }
    Truck truck = new Truck(1, 600, 40, 30);
    Console.WriteLine(truck);
  }
}