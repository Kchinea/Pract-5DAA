using Spectre.Console;
using VrptSwts.InstanceManagement;

namespace VrptSwts.Testing;
/// <summary>
/// Clase que gestiona la ejecución y el registro de comparaciones y depuración
/// de algoritmos para el problema del Viajante de Comercio (TSP).
/// </summary>
internal class ExecutionLogger
{
  private readonly double COLOR_RANGE = 20;
  private readonly Table _loggerTable = new();

  /// <summary>
  /// Constructor de la clase ExecutionLogger.
  /// Configura la tabla con bordes redondeados.
  /// </summary>
  public ExecutionLogger()
  {
    _loggerTable.Border = TableBorder.Rounded;
    _loggerTable.ShowRowSeparators = true;
  }

  /// <summary>
  /// Agrega encabezados a la tabla de comparación de algoritmos.
  /// </summary>
  /// <param name="algorithms">Arreglo de algoritmos a comparar.</param>
  /// <param name="measure">Unidad de medida del tiempo (ejemplo: ms, s).</param>
  public void AddGreedyHeader(string measure)
  {
    _loggerTable.AddColumn("[bold]Source File[/]");
    _loggerTable.AddColumn("[bold]Zones[/]");
    _loggerTable.AddColumn("[bold]Routes[/]");
    _loggerTable.AddColumn("[bold]Resume[/]");
    _loggerTable.AddColumn($"[bold]Cpu Time ({measure})[/]");
  }

  /// <summary>
  /// Agrega una nueva comparación a la tabla de registros.
  /// </summary>
  /// <param name="filePath">Ruta del archivo de entrada.</param>
  /// <param name="numberNodes">Cantidad de nodos en el grafo.</param>
  public void AddGreedyRow(string filePath, 
                           int numberZones,
                           Routes routes,
                           double executionTime)
  {
    string[] tableRow = new string[5];
    tableRow[0] = filePath;
    tableRow[1] = "" + numberZones;
    tableRow[2] = routes.ToString();
    tableRow[3] = $"Nº CV: {routes.GetNumberCollectionRoutes()} - Time: {routes.GetCollectionTime():F3}\n\n" +
                  $"Nº TV: {routes.GetNumberTransportRoutes()} - Time: {routes.GetTransportTime():F3}";
    string timeString = executionTime.ToString("F5");
    string color = GetColorForTime(executionTime);
    tableRow[4] = $"[{color}]{timeString}[/]";
    _loggerTable.AddRow(tableRow);
  }

  public void AddGraspHeader(string measure)
  {
    _loggerTable.AddColumn("[bold]Source File[/]");
    _loggerTable.AddColumn("[bold]Zones[/]");
    _loggerTable.AddColumn("[bold]Rand Window[/]");
    _loggerTable.AddColumn("[bold]Execution[/]");
    _loggerTable.AddColumn("[bold]Routes[/]");
    _loggerTable.AddColumn("[bold]Resume[/]");
    _loggerTable.AddColumn($"[bold]Cpu Time ({measure})[/]");
  }

  public void AddGraspRow(string filePath, 
                           int numberZones,
                           int randWindow,
                           int executionIteration,
                           Routes routes,
                           double executionTime)
  {
    string[] tableRow = new string[7];
    tableRow[0] = filePath;
    tableRow[1] = "" + numberZones;
    tableRow[2] = "" + randWindow;
    tableRow[3] = "" + executionIteration;
    tableRow[4] = routes.ToString();
    tableRow[5] = $"Nº CV: {routes.GetNumberCollectionRoutes()} - Time: {routes.GetCollectionTime():F3}\n\n" +
                  $"Nº TV: {routes.GetNumberTransportRoutes()} - Time: {routes.GetTransportTime():F3}";
    string timeString = executionTime.ToString("F5");
    string color = GetColorForTime(executionTime);
    tableRow[6] = $"[{color}]{timeString}[/]";
    _loggerTable.AddRow(tableRow);
  }

  /// <summary>
  /// Determina el color del tiempo de ejecución según el tiempo límite establecido.
  /// </summary>
  /// <param name="time">Tiempo de ejecución.</param>
  /// <returns>Color en formato de Spectre.Console.</returns>
  private string GetColorForTime(double time)
  {
    if (time < COLOR_RANGE) return "green";
    else if (time < COLOR_RANGE * 2) return "lightgreen";
    else if (time < COLOR_RANGE * 3) return "yellow";
    else return "red";
  }

  /// <summary>
  /// Imprime la tabla de registros en la consola.
  /// </summary>
  public void PrintTable()
  {
    AnsiConsole.Write(_loggerTable);
  }
}
