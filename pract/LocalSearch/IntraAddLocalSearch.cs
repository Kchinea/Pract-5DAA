// // namespace Pract5DAA.LocalSearch;

// // public class IntraAddLocalSearch : ILocalSearch {
// //   public string GetName => "IntraAddLocalSearch";

// //   public Solution Solve(Solution solution, PathMap map) {
// //     List<Truck> trucks = solution.Trucks;
// //     Solution bestSolution = new Solution(trucks);
// //     Solution newSolution = new Solution(trucks);
// //     foreach(Truck truck in solution.Trucks) {
// //       for(int i = 1; i < truck.Path.Count - 2; i++) {
// //         for(int j = i; j < truck.Path.Count - 2; j++) {
// //           if (i != j) {
// //             newSolution = MovementDoBetter(truck, i, j,truck.CurrentTime, bestSolution);
// //           }
// //         }
// //       }
// //     }
// //     return bestSolution;
// //   }
// //   public Solution MovementDoBetter(Truck truck, int i, int j, int lastTime, Solution solution) {
// //     if(i == 0) {
// //       return solution;
// //     }
// //     List<(Zone, int,int)> newPath = new List<(Zone, int,int)>(truck.SetPath);
// //     Solution result = new Solution(solution.Trucks);
// //     double time = truck.CurrentTime;
// //     int load = 0;
// //     Zone anteriorPrevia = truck.Path[i - 1];
// //     Zone editado = truck.Path[i];
// //     Zone posteriorPrevia = truck.Path[i + 1];
// //     time = time - anteriorPrevia.Position.CalculateDistance(editado.Position) / truck.Speed;
// //     time = time - posteriorPrevia.Position.CalculateDistance(editado.Position) / truck.Speed;
// //     time = time + anteriorPrevia.Position.CalculateDistance(posteriorPrevia.Position) / truck.Speed;
// //     Zone anteriorPosterior = truck.Path[j - 1];
// //     Zone actualPosterior = truck.Path[j];
// //     time = time + anteriorPosterior.Position.CalculateDistance(editado.Position) / truck.Speed;
// //     time = time + actualPosterior.Position.CalculateDistance(editado.Position) / truck.Speed;
// //     time = time - anteriorPosterior.Position.CalculateDistance(actualPosterior.Position) / truck.Speed;
// //     Zone lastStation = truck.Path[truck.Path.Count - 1];
// //     int lastStationStep = 0;
// //     for(int k = 0; k < j; k++) {
// //       if( truck.Path[k].Id < 0) {
// //         lastStation = truck.Path[k];
// //         lastStationStep = k;
// //       }
// //     }
// //     Truck newTruck = new Truck(truck.Id, truck.Speed, truck.MaximumLoad, truck.MaximumTime, truck.Map);
// //     for(int x = 0; x < truck.Path.Count; x++) {
// //       if (x == i) {
// //         continue;
// //       } else if (x == j) {
// //         newTruck.AddZone(truck.Path[i], (int)truck.Path[x - 1].Position.CalculateDistance(truck.Path[i].Position) / truck.Speed, truck.Path[i].Load);
// //       }
// //         newTruck.AddZone(truck.Path[x], (int)truck.Path[x - 1].Position.CalculateDistance(truck.Path[x].Position) / truck.Speed, truck.Path[x].Load);
// //     }
// //     for(int y = lastStationStep; y < truck.Path.Count; y++) {
// //       if (truck.Path[y].Id < 0) {
// //         if(time <= truck.MaximumTime && load <= truck.MaximumLoad) {
// //           result.NumVehicles = solution.NumVehicles;
// //           result.TotalTime = solution.TotalTime - lastTime + time;
// //           result.Trucks.Remove(truck);
// //           result.Trucks.Add(newTruck);
// //           return result;
// //         }
// //       }
// //       if(y == i) {
// //         continue;
// //       } else if(y == j) {
// //         load += truck.Path[i].Load;
// //       }
// //       load += truck.Path[y].Load;
// //     }
// //     return solution;
// //   }
// // }

// namespace Pract5DAA.LocalSearch;

// public class IntraAddLocalSearch : ILocalSearch {
//   public string GetName => "IntraAddLocalSearch";

//   public Solution Solve(Solution solution, PathMap map) {
//     Solution bestSolution = new Solution(solution.Trucks); // Copia inicial de la solución
//     // Hacer una copia de la lista de camiones para iterar
//     var trucksCopy = new List<Truck>(solution.Trucks);
//     foreach (Truck truck in trucksCopy) {
//         for (int i = 1; i < truck.Path.Count - 2; i++) {
//             for (int j = 1; j < truck.Path.Count - 2; j++) {
//                 if (i != j) {
//                     Solution newSolution = MovementDoBetter(truck, i, j, truck.CurrentTime, bestSolution);
//                     if (IsBetterSolution(newSolution, bestSolution)) {
//                         bestSolution = newSolution;
//                     }
//                 }
//             }
//         }
//     }
//     return bestSolution;
// }

//   public Solution MovementDoBetter(Truck truck, int i, int j, int lastTime, Solution solution) {
//     // Realizar una copia profunda de los camiones
//     List<Truck> trucks = solution.Trucks.Select(t => t.Clone()).ToList();
//     Solution result = new Solution(trucks);
//     double time = truck.CurrentTime;
//     int load = 0;

//     // Cálculo del tiempo antes y después del movimiento
//     Zone anteriorPrevia = truck.Path[i - 1];
//     Zone editado = truck.Path[i];
//     Zone posteriorPrevia = truck.Path[i + 1];
//     time -= anteriorPrevia.Position.CalculateDistance(editado.Position) / truck.Speed;
//     time -= posteriorPrevia.Position.CalculateDistance(editado.Position) / truck.Speed;
//     time += anteriorPrevia.Position.CalculateDistance(posteriorPrevia.Position) / truck.Speed;

//     Zone anteriorPosterior = truck.Path[j - 1];
//     Zone actualPosterior = truck.Path[j];
//     time += anteriorPosterior.Position.CalculateDistance(editado.Position) / truck.Speed;
//     time += actualPosterior.Position.CalculateDistance(editado.Position) / truck.Speed;
//     time -= anteriorPosterior.Position.CalculateDistance(actualPosterior.Position) / truck.Speed;

//     // Crear un nuevo camión con la ruta modificada
//     Truck newTruck = new Truck(truck.Id, truck.Speed, truck.MaximumLoad, truck.MaximumTime, truck.Map);
//     for (int x = 0; x < truck.Path.Count; x++) {
//         if (x == i) continue;
//         if (x == j) {
//             if (x - 1 >= 0)
//                 newTruck.AddZone(truck.Path[i], (int)truck.Path[x - 1].Position.CalculateDistance(truck.Path[i].Position) / truck.Speed, truck.Path[i].Load);
//             else
//                 newTruck.AddZone(truck.Path[i], 0, truck.Path[i].Load); // O el valor que corresponda para el primer nodo
//         }
//         if (x - 1 >= 0)
//             newTruck.AddZone(truck.Path[x], (int)truck.Path[x - 1].Position.CalculateDistance(truck.Path[x].Position) / truck.Speed, truck.Path[x].Load);
//         else
//             newTruck.AddZone(truck.Path[x], 0, truck.Path[x].Load); // O el valor que corresponda para el primer nodo
//     }
//     Console.WriteLine($"time: {time}");
//     Console.WriteLine($"anterior time: {truck.CurrentTime}");
//     // Verificar factibilidad
//     for (int y = 0; y < truck.Path.Count; y++) {
//       load += truck.Path[y].Load;
//       if (load > truck.MaximumLoad || time > truck.MaximumTime) {
//         return solution; // Si no es factible, devolver la solución original
//       }
//     }

//     // Actualizar la solución si es factible
//     result.NumVehicles = solution.NumVehicles;
//     result.TotalTime = solution.TotalTime - lastTime + time;
//     result.Trucks.Remove(truck);
//     result.Trucks.Add(newTruck);
//     Console.WriteLine($"Truck {truck.Id} moved from {i} to {j} with new time: {time}");
//     return result;
//   }

//   private bool IsBetterSolution(Solution newSolution, Solution bestSolution) {
//     // Comparar soluciones basadas en el tiempo total
//     return newSolution.TotalTime < bestSolution.TotalTime;
//   }
// }

namespace Pract5DAA.LocalSearch;

public class IntraAddLocalSearch : ILocalSearch {
    public string GetName => "IntraAddLocalSearch";

    public Solution Solve(Solution solution, PathMap map) {
        Solution bestSolution = new Solution(solution.Trucks.Select(t => t.Clone()).ToList());

        bool improvement = true;
        while (improvement) {
            improvement = false;

            foreach (Truck truck in bestSolution.Trucks.ToList()) {
                for (int i = 1; i < truck.Path.Count - 2; i++) {
                    for (int j = 1; j < truck.Path.Count - 2; j++) {
                        if (i == j) continue;

                        Solution newSolution = MovementDoBetter(truck, i, j, bestSolution);
                        if (IsBetterSolution(newSolution, bestSolution)) {
                            bestSolution = newSolution;
                            improvement = true;
                            goto Restart; // reiniciar el bucle para reevaluar con la solución mejorada
                        }
                    }
                }
            }

        Restart:;
        }

        return bestSolution;
    }

    public Solution MovementDoBetter(Truck truck, int i, int j, Solution currentSolution) {
        // Clonar la lista de camiones para modificar uno
        List<Truck> trucks = currentSolution.Trucks.Select(t => t.Clone()).ToList();
        Truck truckCopy = trucks.First(t => t.Id == truck.Id);

        // Quitar la zona en i
        Zone movedZone = truckCopy.Path[i];
        truckCopy.Path.RemoveAt(i);

        // Insertar en la nueva posición j (ajustando si i < j porque ya se ha eliminado un elemento)
        if (i < j) j--;
        truckCopy.Path.Insert(j, movedZone);

        // Recalcular tiempo y carga
        truckCopy.RecalculatePath(); // este método debe actualizar CurrentTime y carga interna
        
        if (truckCopy.CurrentTime > truckCopy.MaximumTime || truckCopy.CurrentLoad > truckCopy.MaximumLoad)
            return currentSolution; // No es factible, devolvemos solución original

        // Reemplazar el camión en la solución
        trucks.RemoveAll(t => t.Id == truck.Id);
        trucks.Add(truckCopy);

        Solution newSolution = new Solution(trucks);
        newSolution.TotalTime = currentSolution.TotalTime - truck.CurrentTime + truckCopy.CurrentTime;
        newSolution.NumVehicles = currentSolution.NumVehicles;

        Console.WriteLine($"Truck {truck.Id} moved {movedZone.Id} from {i} to {j}, new time: {truckCopy.CurrentTime}");

        return newSolution;
    }

    private bool IsBetterSolution(Solution newSolution, Solution bestSolution) {
        return newSolution.TotalTime < bestSolution.TotalTime;
    }
}
