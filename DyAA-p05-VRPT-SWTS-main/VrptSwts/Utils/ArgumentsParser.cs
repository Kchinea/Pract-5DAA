/*
 * Universidad de La Laguna
 * Escuela Superior de Ingeniería y Tecnología
 * Grado en Ingeniería Informática
 * Diseño y Análisis de Algoritmos 2024-2025
 *
 * File: ArgumentParser.cs
 * Authors: Roberto Padrón Castañeda & Adrián García Rodríguez
 * Date: 09/03/2025
 * Description: 
 *     This file contains the implementation of the ArgumentParser struct, 
 *     which processes command-line arguments for configuring the Binary Divide
 *     and Conquer Framework. 
 */

using VrptSwts.Searchs.LocalSearchs;

namespace VrptSwts.Utils;

/// <summary>
/// Parses command-line arguments to configure the RAM Machine simulator.
/// Extracts the program file, input and output tapes, and optionally the 
/// data memory size.
/// </summary>
internal struct ArgumentParser
{
  /// <summary>
  /// Initializes an instance of the <see cref="ArgumentParser"/> struct.
  /// Parses command-line arguments and sets corresponding properties.
  /// If the help flag (<c>-h</c>) is provided, the program displays usage 
  /// information and exits.
  /// </summary>
  /// <param name="args">Array of command-line arguments.</param>
  /// <exception cref="ArgumentException">
  /// Thrown if required arguments are missing or if argument formatting is incorrect.
  /// </exception>
  public ArgumentParser(string[] args)
  {
    for (int i = 0; i < args.Length; ++i)
    {
      string argument = args[i];
      if (argument == "-h" || argument == "--help")
      {
        Console.WriteLine(helpText);
        Environment.Exit(0);
      }
      if ((argument == "--execution-mode" || argument == "-m") && (i + 1 < args.Length))
      {
        string mode = args[++i];
        if (mode == "greedy" || mode == "grasp" || mode == "multi-start")
        {
          ExecutionMode = mode;
        }
        else
        {
          Console.WriteLine(useText);
          throw new ArgumentException("ERROR: unrecognized mode type");
        }
      }
      else if ((argument == "--execution-times" || argument == "-t") && (i + 1 < args.Length))
      {
        ExecutionTimes = ExceptionStringToNumber.ConvertToInt(args[++i]);
      }
      else if ((argument == "--local-search" || argument == "-l") && (i + 1 < args.Length))
      {
        string localSearch = args[++i];
        if (localSearch == "reinsertion-intra")
        {
          LocalSearch = localSearch;
        }
        else if (localSearch == "reinsertion-inter")
        {
          LocalSearch = localSearch;
        }
        else if (localSearch == "swap-intra")
        {
          LocalSearch = localSearch;
        }
        else if (localSearch == "swap-inter")
        {
          LocalSearch = localSearch;
        }
        else if (localSearch == "2-opt")
        {
          LocalSearch = localSearch;
        }
        else
        {
          Console.WriteLine(useText);
          throw new ArgumentException("ERROR: unrecognized local search type");
        }
      }
    }
  }

  /// <summary>
  /// Gets the execution mode.
  /// By default it is run the greedy mode
  /// </summary>
  public string ExecutionMode { get; } = "greedy";

  /// <summary>
  /// Gets the number of times to execute the grasp mode.
  /// By default it is 3.
  /// </summary>
  public int ExecutionTimes { get; } = 3;

  public string LocalSearch { get; } = "reinsertion-intra";

  /// <summary>
  /// Usage instructions for running the TSP Tester.
  /// </summary>
  private const string useText =
    "Usage:\n"
  + "  Greedy mode: dotnet run -- -m greedy\n"
  + "  Grasp mode:  dotnet run -- -m grasp -t [execution-times]\n\n"
  + "For more details, use: dotnet run -- -h";
  
  /// <summary>
  /// Help text explaining the TSP tester's functionality and supported arguments.
  /// </summary>
  private const string helpText =
    "VrptSwts Tester Framework for evaluating different building collecting routes algorithms.\n\n"
  + "Usage:\n"
  + "  dotnet run -- [OPTIONS]\n\n"
  + "Options:\n"
  + "  -h, --help              Show this help message and exit.\n"
  + "  -m, --execution-mode    Execution mode: 'greedy' (default) or 'grasp'.\n"
  + "  -t, --execution-times   Execution times: number of times to execute the grasp mode.\n"
  + "Examples:\n"
  + "  dotnet run -- -m grasp -t 5\n\n  # Run grasp mode 5 times per instance and random window\n";
}