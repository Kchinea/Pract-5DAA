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
    List<IAlgorithm> algorithms = new List<IAlgorithm> { algorithm1, algorithm2};

    Reader reader = new Reader(file);
    Instance instance = reader.Read();
    List<Instance> instances = new List<Instance> { instance };

    Tester tester = new Tester(algorithms, instances);
    tester.DoTest();
  }
}