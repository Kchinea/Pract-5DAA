using System.Diagnostics;
using VrptSwts.Algorithms;
using VrptSwts.InstanceManagement;
using VrptSwts.Searchs.LocalSearchs;
using VrptSwts.Vehicles;


namespace VrptSwts.Testing;

/// <summary>
/// Clase encargada de ejecutar pruebas de comparación y depuración de algoritmos para el problema del Viajante de Comercio (TSP).
/// </summary>
internal class VrptSwtsTester
{
  private readonly ExecutionLogger _logger = new ExecutionLogger();
  private readonly VrptSwtsInstanceManager _manager = VrptSwtsInstanceManager.getManager();
  private readonly string[] _instancesFilePath;

  /// <summary>
  /// Constructor de la clase TspTester.
  /// </summary>
  /// <param name="logger">Instancia del logger de ejecución.</param>
  /// <param name="instanceDirectory">Directorio que contiene los archivos de instancias de prueba.</param>
  public VrptSwtsTester(string instanceDirectoryPath)
  {
    // Ordenar los archivos de instancias por número extraído del nombre del archivo
    _instancesFilePath = Directory.GetFiles(instanceDirectoryPath).OrderBy(
      f => int.Parse(System.Text.RegularExpressions.Regex.Match(f, @"\d+").Value)
    ).ToArray();
  }

  /// <summary>
  /// Ejecuta el algoritmo greedy.
  /// </summary>
  public void RunGreedy()
  {
    Stopwatch stopwatch = new();
    _logger.AddGreedyHeader("ms");
    foreach (string instanceFile in _instancesFilePath)
    {
      VrptSwtsInstance instance = _manager.FileToInstance(instanceFile);
      int numberZones = instance.CollectionAreas.Count;
      VrptSwtsGreedy greedyResolver = new VrptSwtsGreedy(instance);
      stopwatch.Restart();
      Routes routes = greedyResolver.BuildRoutes();
      stopwatch.Stop();
      double executionTime = stopwatch.Elapsed.TotalMilliseconds;
      _logger.AddGreedyRow(instanceFile, numberZones, routes, executionTime);
    }
    _logger.PrintTable();
  }

  /// <summary>
  /// Ejecuta el enfoque grasp con mejor de búsqueda local.
  /// </summary>
  public void RunGrasp(int numberExecutions, string localSearchString)
  {
    Stopwatch stopwatch = new();
    _logger.AddGraspHeader("ms");
    foreach (string instanceFile in _instancesFilePath)
    {
      for (int randWindow = 2; randWindow <= 3; ++randWindow)
      {
        for (int execution = 0; execution < numberExecutions; ++execution)
        {
          VrptSwtsInstance instance = _manager.FileToInstance(instanceFile);
          int numberZones = instance.CollectionAreas.Count;
          LocalSearch? localSearch = null;
          switch (localSearchString)
          {
            case "reinsertion-intra":
              localSearch = new ReinsertionIntra(instance.Clone());
              break;
            
            case "reinsertion-inter":
              localSearch = new ReinsertionInter(instance.Clone());
              break;

            case "swap-intra":
              localSearch = new SwapIntra(instance.Clone());
              break;

            case "swap-inter":
              localSearch = new SwapInter(instance.Clone());
              break;

            case "2-opt":
              localSearch = new TwoOpt(instance.Clone());
              break;

            default:
              throw new ArgumentException("ERROR: unrecognized local search type");
          }
          VrptSwtsGrasp graspResolver = new VrptSwtsGrasp(instance, randWindow);
          stopwatch.Restart();
          Routes routes = graspResolver.BuildRoutes();
          Routes localMinimum = localSearch.SearchLocalMinimum(routes);
          graspResolver.GenerateTransportTasks(localMinimum.GetCollectionRoutesCopy());
          List<TransportTruck> transportTrucks = graspResolver.BuildTransportRoutes();
          stopwatch.Stop();
          double executionTime = stopwatch.Elapsed.TotalMilliseconds;
          Routes finalRoutes = new(localMinimum.GetCollectionRoutesCopy(), transportTrucks);
          _logger.AddGraspRow(instanceFile, numberZones, randWindow,
                              execution + 1, finalRoutes, executionTime);
        }
      }
    }
    _logger.PrintTable();
  }

  /// <summary>
  /// Ejecuta un multiarranque que utiliza RandomVND.
  /// </summary>
  public void RunMultiStart(int numberExecutions)
  {
    Stopwatch stopwatch = new();
    _logger.AddGraspHeader("ms");
    foreach (string instanceFile in _instancesFilePath)
    {
      for (int randWindow = 2; randWindow <= 3; ++randWindow)
      {
        for (int execution = 0; execution < numberExecutions; ++execution)
        {
          Console.Write("#");
          VrptSwtsInstance instance = _manager.FileToInstance(instanceFile);
          int numberZones = instance.CollectionAreas.Count;
          VrptSwtsMultiStart multiStart = new VrptSwtsMultiStart(instance, randWindow);
          stopwatch.Restart();
          Routes routes = multiStart.Run();
          stopwatch.Stop();
          double executionTime = stopwatch.Elapsed.TotalMilliseconds;
          _logger.AddGraspRow(instanceFile, numberZones, randWindow,
                              execution + 1, routes, executionTime);
        }
      }
    }
    _logger.PrintTable();
  }
}