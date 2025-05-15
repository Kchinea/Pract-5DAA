namespace Pract5DAA.LocalSearch;

public class InterSwapLocalSearch : ILocalSearch {
    public string GetName => "InterSwapLocalSearch";

    public Solution Solve(Solution solution, PathMap map) {
        Solution bestSolution = new Solution(solution.Trucks.Select(t => t.Clone()).ToList());
        bool improvement = true;

        while (improvement) {
            improvement = false;

            foreach (Truck truckA in bestSolution.Trucks.ToList()) {
                for (int i = 1; i < truckA.Path.Count - 1; i++) { // Evita depósitos
                    foreach (Truck truckB in bestSolution.Trucks.ToList()) {
                        if (truckA.Id == truckB.Id) continue;
                        for (int j = 1; j < truckB.Path.Count - 1; j++) { // Evita depósitos
                            Solution newSolution = MovementDoBetter(truckA, i, truckB, j, bestSolution);
                            if (IsBetterSolution(newSolution, bestSolution)) {
                                bestSolution = newSolution;
                                improvement = true;
                                goto Restart;
                            }
                        }
                    }
                }
            }
        Restart:;
        }

        return bestSolution;
    }

public Solution MovementDoBetter(Truck truckA, int idxA, Truck truckB, int idxB, Solution currentSolution) {
    // Clonar la lista de camiones para modificar
    List<Truck> trucks = currentSolution.Trucks.Select(t => t.Clone()).ToList();
    Truck truckACopy = trucks.First(t => t.Id == truckA.Id);
    Truck truckBCopy = trucks.First(t => t.Id == truckB.Id);

    // Extrae la ruta real (solo zonas)
    var pathA = truckACopy.SetPath.Select(t => t.Item1).ToList();
    var pathB = truckBCopy.SetPath.Select(t => t.Item1).ToList();

    // Intercambiar las zonas
    Zone temp = pathA[idxA];
    pathA[idxA] = pathB[idxB];
    pathB[idxB] = temp;

    // Reconstruye las rutas internas (cargas y tiempos a 0, se recalculan)
    truckACopy.SetPath = pathA.Select(z => (z, 0, 0)).ToList();
    truckBCopy.SetPath = pathB.Select(z => (z, 0, 0)).ToList();

    truckACopy.RecalculatePath();
    truckBCopy.RecalculatePath();

    // Comprobar factibilidad
    if (truckACopy.CurrentTime > truckACopy.MaximumTime || truckACopy.CurrentLoad > truckACopy.MaximumLoad)
        return currentSolution;
    if (truckBCopy.CurrentTime > truckBCopy.MaximumTime || truckBCopy.CurrentLoad > truckBCopy.MaximumLoad)
        return currentSolution;

    // Reemplazar los camiones en la solución
    trucks.RemoveAll(t => t.Id == truckA.Id || t.Id == truckB.Id);
    trucks.Add(truckACopy);
    trucks.Add(truckBCopy);

    Solution newSolution = new Solution(trucks);
    newSolution.TotalTime = currentSolution.TotalTime - truckA.CurrentTime - truckB.CurrentTime
                            + truckACopy.CurrentTime + truckBCopy.CurrentTime;
    newSolution.NumVehicles = currentSolution.NumVehicles;

    return newSolution;
}
    // public Solution MovementDoBetter(Truck truckA, int idxA, Truck truckB, int idxB, Solution currentSolution) {
    //     // Clonar la lista de camiones para modificar
    //     List<Truck> trucks = currentSolution.Trucks.Select(t => t.Clone()).ToList();
    //     Truck truckACopy = trucks.First(t => t.Id == truckA.Id);
    //     Truck truckBCopy = trucks.First(t => t.Id == truckB.Id);

    //     // Intercambiar las zonas
    //     Zone tempZone = truckACopy.Path[idxA];
    //     truckACopy.SetPath[idxA] = (truckBCopy.Path[idxB], (int)truckACopy.Path[idxA].Position.CalculateDistance(truckBCopy.Path[idxB].Position) / truckACopy.Speed, truckACopy.Path[idxA].Load);
    //     truckBCopy.SetPath[idxB] = (tempZone, (int)truckBCopy.Path[idxB].Position.CalculateDistance(tempZone.Position) / truckBCopy.Speed, tempZone.Load);
    //     Console.WriteLine($"ANTESSSS");
    //     Console.WriteLine($"TruckA: Id={truckACopy.Id}, CurrentTime={truckACopy.CurrentTime}, MaximumTime={truckACopy.MaximumTime}, CurrentLoad={truckACopy.CurrentLoad}, MaximumLoad={truckACopy.MaximumLoad}");
    //     Console.WriteLine("Truck A Zones: " + string.Join(", ", truckACopy.Path.Select(z => z.Id)));
    //     Console.WriteLine($"TruckB: Id={truckBCopy.Id}, CurrentTime={truckBCopy.CurrentTime}, MaximumTime={truckBCopy.MaximumTime}, CurrentLoad={truckBCopy.CurrentLoad}, MaximumLoad={truckBCopy.MaximumLoad}");
    //     Console.WriteLine("Truck B Zones: " + string.Join(", ", truckBCopy.Path.Select(z => z.Id)));
    //     // Recalcular ambos camiones
    //     truckACopy.RecalculatePath();
    //     truckBCopy.RecalculatePath();
    //     Console.WriteLine($"DESPUESSSS");
    //     // Comprobar factibilidad
    //     Console.WriteLine($"TruckA: Id={truckACopy.Id}, CurrentTime={truckACopy.CurrentTime}, MaximumTime={truckACopy.MaximumTime}, CurrentLoad={truckACopy.CurrentLoad}, MaximumLoad={truckACopy.MaximumLoad}");
    //     Console.WriteLine("Truck A Zones: " + string.Join(", ", truckACopy.Path.Select(z => z.Id)));
    //     Console.WriteLine($"TruckB: Id={truckBCopy.Id}, CurrentTime={truckBCopy.CurrentTime}, MaximumTime={truckBCopy.MaximumTime}, CurrentLoad={truckBCopy.CurrentLoad}, MaximumLoad={truckBCopy.MaximumLoad}");
    //     Console.WriteLine("Truck B Zones: " + string.Join(", ", truckBCopy.Path.Select(z => z.Id)));
    //     if (truckACopy.CurrentTime > truckACopy.MaximumTime || truckACopy.CurrentLoad > truckACopy.MaximumLoad)
    //         return currentSolution;
    //     if (truckBCopy.CurrentTime > truckBCopy.MaximumTime || truckBCopy.CurrentLoad > truckBCopy.MaximumLoad)
    //         return currentSolution;

    //     // Reemplazar los camiones en la solución
    //     trucks.RemoveAll(t => t.Id == truckA.Id || t.Id == truckB.Id);
    //     trucks.Add(truckACopy);
    //     trucks.Add(truckBCopy);

    //     Solution newSolution = new Solution(trucks);
    //     newSolution.TotalTime = currentSolution.TotalTime - truckA.CurrentTime - truckB.CurrentTime
    //                             + truckACopy.CurrentTime + truckBCopy.CurrentTime;
    //     newSolution.NumVehicles = currentSolution.NumVehicles;

    //     Console.WriteLine($"Swapped zone {truckA.Path[idxA].Id} (Truck {truckA.Id}) with zone {truckB.Path[idxB].Id} (Truck {truckB.Id})");

    //     return newSolution;
    // }

    private bool IsBetterSolution(Solution newSolution, Solution bestSolution) {
        return newSolution.TotalTime < bestSolution.TotalTime;
    }
}