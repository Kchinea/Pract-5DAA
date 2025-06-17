/*
 * VrptSwtsInstanceManager.cs
 * 
 * This class manages the loading and creation of a VrptSwtsInstance from a file. 
 * It follows the Singleton design pattern to ensure only one instance of the 
 * manager exists. The class provides methods to read a problem description from 
 * a file and convert it into a `VrptSwtsInstance` object, which contains the data 
 * for the vehicle routing problem with trash collection and soft time windows.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.Graph;
using VrptSwts.Utils;
using VrptSwts.Vehicles;

namespace VrptSwts.InstanceManagement;

internal class VrptSwtsInstanceManager
{

  /// <summary>
  /// Singleton instance of VrptSwtsInstanceManager.
  /// Ensures that only one instance of the manager exists throughout the program.
  /// </summary>
  private static VrptSwtsInstanceManager? instance;

  /// <summary>
  /// Gets the singleton instance of VrptSwtsInstanceManager.
  /// This method follows the Singleton pattern to ensure that only one instance exists.
  /// </summary>
  /// <returns>Returns the singleton instance of VrptSwtsInstanceManager.</returns>
  public static VrptSwtsInstanceManager getManager()
  {
    if (VrptSwtsInstanceManager.instance == null)
      VrptSwtsInstanceManager.instance = new VrptSwtsInstanceManager();
    return VrptSwtsInstanceManager.instance;
  }

  /// <summary>
  /// Loads a VrptSwtsInstance from a file and creates the corresponding object.
  /// This method reads the problem description from a configuration file and builds a
  /// `VrptSwtsInstance` object containing all the necessary data such as vehicles, 
  /// transfer stations, collection areas, etc.
  /// </summary>
  /// <param name="filePath">Path to the input file that contains the problem definition.</param>
  /// <returns>Returns a VrptSwtsInstance constructed from the file data.</returns>
  public VrptSwtsInstance FileToInstance(string filePath)
  {
    string[] fileLines = File.ReadAllLines(filePath);
    int numberVehicles = ExceptionStringToNumber.ConvertToInt(fileLines[2].Split(' ')[1]);
    Truck collectionTruck = BuildTruck(fileLines[0], fileLines[6], fileLines[8]);
    Truck transportTruck = BuildTruck(fileLines[1], fileLines[7], fileLines[8]);
    Node depot = BuildArea("Depot", fileLines[9]);
    Node dumpsite = BuildArea("Dumpsite", fileLines[12]);
    
    List<Node> transferStations = new List<Node>();
    transferStations.Add(BuildArea("TS1", fileLines[10]));
    transferStations.Add(BuildArea("TS2", fileLines[11]));
    
    int numberAreas = ExceptionStringToNumber.ConvertToInt(fileLines[3].Split(' ')[1]);
    List<Node> collectionAreas = new List<Node>();
    for (int i = 16; (i - 16) < numberAreas; ++i)
      collectionAreas.Add(BuildCollectionArea(fileLines[i]));
      
    return new VrptSwtsInstance(numberVehicles, collectionTruck, transportTruck,
                                depot, dumpsite, transferStations, collectionAreas);
  }

  /// <summary>
  /// Builds a truck object using data from the input file.
  /// This helper method parses the truck's capacity, work duration, and speed from 
  /// the provided file lines and constructs a `Truck` object accordingly.
  /// </summary>
  /// <param name="timeLine">Line containing the truck work duration.</param>
  /// <param name="capacityLine">Line containing the truck capacity.</param>
  /// <param name="speedLine">Line containing the truck speed.</param>
  /// <returns>Returns a newly created Truck object.</returns>
  private Truck BuildTruck(string timeLine,
                           string capacityLine,
                           string speedLine)
  {
    int capacity = ExceptionStringToNumber.ConvertToInt(capacityLine.Split(' ')[1]);
    int workDuration = ExceptionStringToNumber.ConvertToInt(timeLine.Split(' ')[1]);
    int speed = ExceptionStringToNumber.ConvertToInt(speedLine.Split(' ')[1]);
    return new Truck(capacity, workDuration, speed);
  }

  /// <summary>
  /// Builds a node object representing an area (Depot, Dumpsite, Transfer Station).
  /// This helper method creates a `Node` object based on the data (coordinates) 
  /// extracted from a line in the file.
  /// </summary>
  /// <param name="id">ID of the area (e.g., "Depot", "Dumpsite").</param>
  /// <param name="areaLine">Line from the file containing the area information.</param>
  /// <returns>Returns a Node object representing the area.</returns>
  private Node BuildArea(string id, string areaLine)
  {
    string[] areaArray = areaLine.Split(' ');
    int coordinateX = ExceptionStringToNumber.ConvertToInt(areaArray[1]);
    int coordinateY = ExceptionStringToNumber.ConvertToInt(areaArray[2]);
    return new Node(id, new(coordinateX, coordinateY));
  }

  /// <summary>
  /// Builds a collection area object using data from the file.
  /// This helper method parses a line in the file to construct a `CollectionArea` object 
  /// which represents areas where trash is collected.
  /// </summary>
  /// <param name="areaLine">Line from the file containing the collection area data.</param>
  /// <returns>Returns a new CollectionArea object.</returns>
  private CollectionArea BuildCollectionArea(string areaLine)
  {
    string[] areaArray = areaLine.Split(' ');
    int id = ExceptionStringToNumber.ConvertToInt(areaArray[0]);
    int coordinateX = (int)ExceptionStringToNumber.ConvertToDouble(areaArray[1]);
    int coordinateY = (int)ExceptionStringToNumber.ConvertToDouble(areaArray[2]);
    int processingTime = (int)ExceptionStringToNumber.ConvertToDouble(areaArray[3]);
    int demand = (int)ExceptionStringToNumber.ConvertToDouble(areaArray[4]);
    return new CollectionArea("Area" + id, new(coordinateX, coordinateY), demand,
                              processingTime);
  }
}