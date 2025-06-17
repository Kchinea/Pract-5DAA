/*
 * VrptSwtsResolver.cs
 * 
 * Abstract base class for solving the RPT-SWTS (Route Planning for Trash Collection 
 * with Soft Time and Service Windows) problem using heuristic algorithms.
 *
 * This class provides general utilities and a framework for constructing collection 
 * and transport routes, computing associated transport tasks, and assigning tasks 
 * to vehicles under time and capacity constraints.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

using VrptSwts.InstanceManagement;
using VrptSwts.Graph;
using VrptSwts.Vehicles;

namespace VrptSwts.Algorithms;

/// <summary>
/// Abstract class that defines the structure of a solver for the RPT-SWTS problem.
/// </summary>
internal abstract class VrptSwtsResolver(VrptSwtsInstance problemInstance)
{
  protected VrptSwtsInstance _problemInstance = problemInstance;
  protected List<TransportTask> _transportTasks = new();

  /// <summary>
  /// Generates a list of transport tasks from the given collection routes.
  /// These tasks are used to model the transport of waste from transfer stations to dumpsites.
  /// </summary>
  public List<TransportTask> GenerateTransportTasks(List<CollectionRoute> collectionRoutes)
  {
    int truckSpeed = _problemInstance.CollectionTruck.Speed;
    List<TransportTask> transportTasks = new();
    foreach (CollectionRoute route in collectionRoutes)
    {
      double timeCount = 0;
      int wasteAmount = 0;
      for (int i = 0; i < route.GetRouteLength() - 1; ++i)
      {
        Node currentNode = route.GetArea(i);
        Node nextNode = route.GetArea(i + 1);
        if (currentNode.Id.StartsWith("TS"))
        {
          transportTasks.Add(new TransportTask(wasteAmount, currentNode, timeCount));
          wasteAmount = 0;
        }
        if (currentNode is CollectionArea collectionArea) {
          timeCount += collectionArea.ProcessingTime;
          wasteAmount += collectionArea.Demand;
        }
        timeCount += Node.DistanceBetween(currentNode, nextNode) / truckSpeed * 60;
      }
    }
    _transportTasks = transportTasks;
    return transportTasks;
  }

  /// <summary>
  /// Builds both collection and transport routes and returns the final routes object.
  /// </summary>
  public virtual Routes BuildRoutes()
  {
    List<CollectionRoute> collectionRoutes = BuildCollectionRoutes();
    List<TransportTruck> transportTrucks = BuildTransportRoutes();
    return new(collectionRoutes, transportTrucks);
  }

  /// <summary>
  /// Constructs collection routes based on the remaining collection areas,
  /// while satisfying truck capacity and working time constraints.
  /// </summary>
  public virtual List<CollectionRoute> BuildCollectionRoutes()
  {
    List<CollectionRoute> routes = new();
    while (_problemInstance.CollectionAreas.Count != 0)
    {
      List<Node> route = new List<Node>([_problemInstance.Depot]);
      List<int> subRoutes = new List<int>();
      int truckCapacity = _problemInstance.CollectionTruck.Capacity;
      double routeTime = _problemInstance.CollectionTruck.WorkDuration;
      int truckSpeed = _problemInstance.CollectionTruck.Speed;
      Node currentArea = _problemInstance.Depot;
      double timeCount = 0;
      while (_problemInstance.CollectionAreas.Count != 0)
      {
        (Node closestNeighbour, double closestDistance, int closestIndex) = 
            GetClosestNeighbour(currentArea, _problemInstance.CollectionAreas);
        double secureTime = GetSecureTime(closestNeighbour, closestDistance);

        CollectionArea closestArea = (CollectionArea)closestNeighbour;
        if (closestArea.Demand <= truckCapacity && secureTime <= routeTime)
        {
          route.Add(closestArea);
          truckCapacity -= closestArea.Demand;
          double subRouteTime = (closestDistance / truckSpeed * 60) + closestArea.ProcessingTime;
          timeCount += subRouteTime;
          routeTime -= subRouteTime;
          _problemInstance.CollectionAreas.RemoveAt(closestIndex);
          currentArea = closestArea;
        }
        else
        {
          if (secureTime <= routeTime)
          {
            (Node closestTs, double closestTsDistance, _) =
                GetClosestNeighbour(currentArea, _problemInstance.TransferStations);
            route.Add(closestTs);
            subRoutes.Add(route.Count - 1);
            double subRouteTime = closestTsDistance / truckSpeed * 60;
            timeCount += subRouteTime;
            int unloadedQuantity = _problemInstance.CollectionTruck.Capacity - truckCapacity;
            _transportTasks.Add(new(unloadedQuantity, closestTs, timeCount));
            truckCapacity = _problemInstance.CollectionTruck.Capacity;
            routeTime -= subRouteTime;
            currentArea = closestTs;
          }
          else
          {
            break;
          }
        }
      }
      if (!currentArea.Id.StartsWith("TS"))
      {
        (Node closestTs, double closestTsDistance, _) =
            GetClosestNeighbour(currentArea, _problemInstance.TransferStations);
        route.Add(closestTs);
        subRoutes.Add(route.Count - 1);
        double subRouteTime = closestTsDistance / truckSpeed * 60;
        timeCount += subRouteTime;
        int unloadedQuantity = _problemInstance.CollectionTruck.Capacity - truckCapacity;
        _transportTasks.Add(new(unloadedQuantity, closestTs, timeCount));
        route.Add(_problemInstance.Depot);
        subRouteTime = Node.DistanceBetween(closestTs, _problemInstance.Depot) / truckSpeed * 60;
        timeCount += subRouteTime;
      }
      else
      {
        route.Add(_problemInstance.Depot);
      }
      routes.Add(new CollectionRoute(route, subRoutes, timeCount));
    }
    return routes;
  }

  /// <summary>
  /// Builds transport routes by assigning transport tasks to available transport trucks.
  /// </summary>
  public List<TransportTruck> BuildTransportRoutes()
  {
    List<TransportTask> sortedTransportTasks = _transportTasks.OrderBy(t => t.ArrivalTime).ToList();
    List<TransportTruck> transportTrucks = new();
    int truckSpeed = _problemInstance.TransportTruck.Speed;
    Node dumpsite = _problemInstance.Dumpsite;
    int minimumWaste = _transportTasks.Min(t => t.UnloadedQuantity);
    while(sortedTransportTasks.Count != 0)
    {
      TransportTask nextTask = sortedTransportTasks[0];
      sortedTransportTasks.RemoveAt(0);
      TransportTruck? chosenTruck = BestTransportTruck(transportTrucks, nextTask);
      if (chosenTruck == null)
      {
        chosenTruck = new TransportTruck(_problemInstance.TransportTruck.Capacity, _problemInstance.TransportTruck.WorkDuration, new());
        TransportTask dumpsiteTask = new(0, dumpsite, 0);
        chosenTruck.TransportTasks.AddRange([dumpsiteTask, nextTask]);
        chosenTruck.CurrentCapacity = chosenTruck.CurrentCapacity - nextTask.UnloadedQuantity;
        chosenTruck.RemainingTime = chosenTruck.RemainingTime - (Node.DistanceBetween(dumpsite, nextTask.TransferStation) / truckSpeed * 60);
        transportTrucks.Add(chosenTruck);
      }
      else
      {
        chosenTruck.CurrentCapacity = chosenTruck.CurrentCapacity - nextTask.UnloadedQuantity;
        TransportTask lastTask = chosenTruck.TransportTasks[chosenTruck.TransportTasks.Count - 1];
        double subRouteTime = nextTask.ArrivalTime - lastTask.ArrivalTime;
        chosenTruck.RemainingTime = chosenTruck.RemainingTime - subRouteTime;
        chosenTruck.TransportTasks.Add(nextTask);
        if (chosenTruck.CurrentCapacity < minimumWaste)
        {
          chosenTruck.CurrentCapacity = _problemInstance.TransportTruck.Capacity;
          lastTask = chosenTruck.TransportTasks[chosenTruck.TransportTasks.Count - 1];
          double timeToDumpsite = Node.DistanceBetween(lastTask.TransferStation, dumpsite) / truckSpeed * 60;
          chosenTruck.RemainingTime = chosenTruck.RemainingTime - timeToDumpsite;
          TransportTask dumpsiteTask = new(0, dumpsite, lastTask.ArrivalTime + timeToDumpsite);
          chosenTruck.TransportTasks.Add(dumpsiteTask);
        }
      }
    }
    foreach (TransportTruck transportTruck in transportTrucks)
    {
      Node lastNode = transportTruck.TransportTasks[transportTruck.TransportTasks.Count - 1].TransferStation;
      if (lastNode.Id != "Dumpsite")
      {
        double timeToDumpsite = Node.DistanceBetween(lastNode, dumpsite) / truckSpeed * 60;
        transportTruck.RemainingTime -= timeToDumpsite;
        transportTruck.TransportTasks.Add(new(0, dumpsite, 0));
      }
      transportTruck.TimeWorked = _problemInstance.TransportTruck.WorkDuration - transportTruck.RemainingTime;
    }
    return transportTrucks;
  }

  /// <summary>
  /// Selects the best transport truck for a new transport task based on cost.
  /// </summary>
  private TransportTruck? BestTransportTruck(List<TransportTruck> transportTrucks, TransportTask nextTask)
  {
    TransportTruck? chosenTruck = null;
    double bestInsertionCost = double.PositiveInfinity;
    foreach (TransportTruck transportTruck in transportTrucks)
    {
      double cost = CalculateTransportCost(transportTruck, nextTask);
      if (cost < bestInsertionCost)
      {
        chosenTruck = transportTruck;
        bestInsertionCost = cost;
      }
    }
    return chosenTruck;
  }

  /// <summary>
  /// Calculates the feasibility and cost of inserting a transport task into a truck's route.
  /// </summary>
  private double CalculateTransportCost(TransportTruck transportTruck, TransportTask nextTask)
  {
    int truckSpeed = _problemInstance.TransportTruck.Speed;
    TransportTask lastTask = transportTruck.TransportTasks[transportTruck.TransportTasks.Count - 1];
    double timeBetweenNodes = Node.DistanceBetween(lastTask.TransferStation, nextTask.TransferStation) / truckSpeed * 60;
    if (timeBetweenNodes > nextTask.ArrivalTime - lastTask.ArrivalTime)
      return double.PositiveInfinity;
    
    if (transportTruck.CurrentCapacity - nextTask.UnloadedQuantity < 0)
      return double.PositiveInfinity;

    double saveTime = nextTask.ArrivalTime - lastTask.ArrivalTime +
                      (Node.DistanceBetween(nextTask.TransferStation, _problemInstance.Dumpsite) / truckSpeed * 60);

    if (transportTruck.RemainingTime - saveTime < 0)
      return double.PositiveInfinity;
    
    return timeBetweenNodes + (nextTask.ArrivalTime - lastTask.ArrivalTime);
  }

  /// <summary>
  /// Estimates the time needed to safely reach a transfer station and return to the depot.
  /// </summary>
  protected double GetSecureTime(Node closestArea, double closestDistance)
  {
    (Node closestTs, double closestTsDist, _) = 
        GetClosestNeighbour(closestArea, _problemInstance.TransferStations);
    double distanceToDepot = Node.DistanceBetween(closestTs, _problemInstance.Depot);

    double totalDistance = closestDistance + closestTsDist + distanceToDepot;
    int speed = _problemInstance.CollectionTruck.Speed;
    CollectionArea closestCollectionArea = (CollectionArea)closestArea;
    return (totalDistance / speed * 60) + closestCollectionArea.ProcessingTime;
  }

  /// <summary>
  /// Abstract method to obtain the closest neighbour from a given node.
  /// Must be implemented by subclasses.
  /// </summary>
  protected abstract Tuple<Node, double, int> GetClosestNeighbour(Node currentArea, 
                                                                  List<Node> neightbours);
}