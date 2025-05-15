using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace Pract5DAA;

public class TransportPart{
  private List<Truck> _CollectionTrucks;
  private List<TransporTruck> _TransportTrucks;
  private int _totalTime;
  private int _totalLoad;
  private int _numVehicles;
  private Solution _solution;
  private Instance _instance;
  private List<SWTS> _stations;
  private SWTS _dumpsite;
  public TransportPart(Solution solution, Instance instance) {
    _instance = instance;
    _stations = new List<SWTS>{new SWTS(instance.Stations[0].Id, instance.Stations[0].Position), new SWTS(instance.Stations[1].Id, instance.Stations[1].Position)};
    _dumpsite = new SWTS(0, instance.DumpPosition);
    _solution = solution;
    _CollectionTrucks = solution.Trucks;
    _TransportTrucks = new List<TransporTruck>();
    _totalTime = 0;
    _totalLoad = 0;
    _numVehicles = 0;
  }
  public List<(SWTS, int ,int)> CreateWorkflow() {
    List<(SWTS,int ,int)> workflow = new List<(SWTS,int ,int)>();
     foreach (Truck truck in _CollectionTrucks)
{
    Zone? prevZone = null;
    int time = 0;
    int load = 0;
    foreach (var zone in truck.SetPath)
    {
        if (prevZone != null)
        {
            double travelTime = prevZone.Position.CalculateDistance(zone.Item1.Position) / _instance.speed * 60;
            // Console.WriteLine($"Travel time from {prevZone} to {zone.Item1}: {travelTime}");
            time += (int)travelTime;
            if(prevZone.Id > 0)
            {
                time += zone.Item1.CollectionTime;
            }
        }
        load += zone.Item3;

        if (zone.Item1.Id == -1)
        {
            workflow.Add((_stations[0], time, load));
            // time = 0;
            load = 0;
        }
        else if (zone.Item1.Id == -2)
        {
            workflow.Add((_stations[1], time, load));
            // time = 0;
            // load = 0;
        }

        prevZone = zone.Item1;
    }
}
    return workflow;
  }
  public (List<TransporTruck>, int) DoTransport() {
    List<(SWTS, int ,int)> workflow = CreateWorkflow();
    List<TransporTruck> transport = new List<TransporTruck>();
    workflow = workflow.OrderBy(w => w.Item2).ToList();
    foreach (var zone in workflow) {
      // Console.WriteLine($"Zone: {zone.Item2} {zone.Item3}");
      TransporTruck? bestTruck = HowBestTruck(transport, zone);
      if (bestTruck == null) {
        bestTruck = new TransporTruck(_numVehicles, _instance.maxDeliveryCapacity, _instance.maxDeliveryDuration, _instance.speed, _instance.Zones);
        bestTruck.CurrentTime = zone.Item2;
        bestTruck.CurrentLoad = 0;
        bestTruck.AddZone(_dumpsite, 0, 0);
        bestTruck.AddZone(zone.Item1, zone.Item2, zone.Item3);
        transport.Add(bestTruck);
        _numVehicles++;
      } else {
        bestTruck.CurrentTime = zone.Item2;
        bestTruck.CurrentLoad = zone.Item3;
        bestTruck.AddZone(zone.Item1, zone.Item2, zone.Item3);
      }
    }
    foreach (TransporTruck truck in transport) {
      if (truck.LastZone != _dumpsite) {
        truck.AddZone(_dumpsite, (int)truck.LastZone.Position.CalculateDistance(_dumpsite.Position) / _instance.speed * 60, 0);
      }
    }
    return (transport, _totalTime);
  }
  public TransporTruck? HowBestTruck( List<TransporTruck> transport, (SWTS, int ,int) zone) {
    TransporTruck? bestTruck = null;
    double minCost = double.MaxValue;
    foreach (TransporTruck truck in transport) {
      double cost = GetCost(truck, zone);
      if (cost < minCost) {
        minCost = cost;
        bestTruck = truck;
      }
    }
    return bestTruck;
  }
  public double GetCost(TransporTruck transportTruck, (SWTS, int ,int) zone) {
    double travelTime = transportTruck.LastZone.Position.CalculateDistance(zone.Item1.Position) / _instance.speed * 60;
    if(transportTruck.CurrentTime + travelTime > _instance.maxDeliveryDuration) {
      return double.MaxValue;
    }
    travelTime = zone.Item2 + _dumpsite.Position.CalculateDistance(zone.Item1.Position) / _instance.speed * 60 ;
    if( travelTime > _instance.maxDeliveryDuration || transportTruck.CurrentLoad > _instance.maxDeliveryCapacity) {
      return double.MaxValue;
    }
    return travelTime + zone.Item2;
  } 
  public List<TransporTruck> TransportTrucks {
    get => _TransportTrucks;
    set => _TransportTrucks = value;
  }
  public int TotalTime {
    get => _totalTime;
    set => _totalTime = value;
  }
  public int TotalLoad {
    get => _totalLoad;
    set => _totalLoad = value;
  }
  public int NumVehicles {
    get => _numVehicles;
    set => _numVehicles = value;
  }
}



// using System.Reflection.Metadata.Ecma335;
// using System.Runtime.InteropServices;

// namespace Pract5DAA;

// public class TransportPart{
//   private List<Truck> _CollectionTrucks;
//   private List<TransporTruck> _TransportTrucks;
//   private int _totalTime;
//   private int _totalLoad;
//   private int _numVehicles;
//   private Solution _solution;
//   private Instance _instance;
//   private List<SWTS> _stations;
//   private SWTS _dumpsite;
//   public TransportPart(Solution solution, Instance instance) {
//     _instance = instance;
//     _stations = new List<SWTS>{new SWTS(instance.Stations[0].Id, instance.Stations[0].Position), new SWTS(instance.Stations[1].Id, instance.Stations[1].Position)};
//     _dumpsite = new SWTS(0, instance.DumpPosition);
//     _solution = solution;
//     _CollectionTrucks = solution.Trucks;
//     _TransportTrucks = new List<TransporTruck>();
//     _totalTime = 0;
//     _totalLoad = 0;
//     _numVehicles = 0;
//   }
//   public List<(SWTS, int ,int)> CreateWorkflow() {
//     List<(SWTS,int ,int)> workflow = new List<(SWTS,int ,int)>();
//     foreach (Truck truck in _CollectionTrucks)
// {
//     Zone? prevZone = null;
//     int time = 0;
//     int load = 0;
//     foreach (var zone in truck.SetPath)
//     {
//         if (prevZone != null)
//         {
//             int travelTime = (int)Math.Ceiling(prevZone.Position.CalculateDistance(zone.Item1.Position) / _instance.speed);
//             time += travelTime;
//         }
//         load += zone.Item3;

//         if (zone.Item1.Id == -1)
//         {
//             workflow.Add((_stations[0], time, load));
//             // time = 0;
//             load = 0;
//         }
//         else if (zone.Item1.Id == -2)
//         {
//             workflow.Add((_stations[1], time, load));
//             // time = 0;
//             load = 0;
//         }

//         prevZone = zone.Item1;
//     }
// }
//     return workflow;
//   }
//   public (List<TransporTruck>, int) DoTransport() {
//     List<(SWTS, int ,int)> workflow = CreateWorkflow();
//     List<TransporTruck> transport = new List<TransporTruck>();
//     workflow = workflow.OrderBy(w => w.Item2).ToList();
//     foreach (var zone in workflow) {
//         TransporTruck? bestTruck = HowBestTruck(transport, zone);
//         if (bestTruck == null) {
//             bestTruck = new TransporTruck(_numVehicles, _instance.maxDeliveryCapacity, _instance.maxDeliveryDuration, _instance.speed, _instance.Zones);
//             // El camión está vacío, así que los incrementos son los valores absolutos
//             bestTruck.AddZone(_dumpsite, 0, 0);
//             bestTruck.AddZone(zone.Item1, zone.Item2, zone.Item3);
//             transport.Add(bestTruck);
//             _numVehicles++;
//         } else {
//             int deltaTime = zone.Item2;
//             int deltaLoad = zone.Item3 + bestTruck.CurrentLoad;
//             bestTruck.AddZone(zone.Item1, deltaTime, deltaLoad);
//         }
//     }
//     foreach (TransporTruck truck in transport) {
//       if (truck.LastZone != _dumpsite) {
//         truck.AddZone(_dumpsite, truck.LastZone.TimeToNext(_dumpsite, _instance.speed), 0);
//       }
//     }
//     return (transport, _totalTime);
//   }
//   public TransporTruck? HowBestTruck( List<TransporTruck> transport, (SWTS, int ,int) zone) {
//     TransporTruck? bestTruck = null;
//     double minCost = double.MaxValue;
//     foreach (TransporTruck truck in transport) {
//       double cost = GetCost(truck, zone);
//       if (cost < minCost) {
//         minCost = cost;
//         bestTruck = truck;
//       }
//     }
//     return bestTruck;
//   }
//   public double GetCost(TransporTruck transportTruck, (SWTS, int ,int) zone) {
//     double travelTime = transportTruck.LastZone.TimeToNext(zone.Item1, _instance.speed);

//     int deltaTime = zone.Item2 - transportTruck.CurrentTime;
//     int deltaLoad = zone.Item3 - transportTruck.CurrentLoad;

//     if(transportTruck.CurrentTime + deltaTime > _instance.maxDeliveryDuration)
//         return double.MaxValue;
//     if(transportTruck.CurrentLoad + deltaLoad > _instance.maxDeliveryCapacity)
//         return double.MaxValue;

//     return deltaTime;
//   } 
//   public List<TransporTruck> TransportTrucks {
//     get => _TransportTrucks;
//     set => _TransportTrucks = value;
//   }
//   public int TotalTime {
//     get => _totalTime;
//     set => _totalTime = value;
//   }
//   public int TotalLoad {
//     get => _totalLoad;
//     set => _totalLoad = value;
//   }
//   public int NumVehicles {
//     get => _numVehicles;
//     set => _numVehicles = value;
//   }
// }