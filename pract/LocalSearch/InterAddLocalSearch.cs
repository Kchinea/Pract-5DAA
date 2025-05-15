namespace Pract5DAA.LocalSearch;

public class InterAddLocalSearch : ILocalSearch {
    public string GetName => "InterAddLocalSearch";

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
                            if(truckA.Path[i].Id < 0) continue; // Evita zonas IFS
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

    // public Solution MovementDoBetter(Truck truckA, int idxA, Truck truckB, int idxB, Solution currentSolution) {
    //     // Clonar la lista de camiones para modificar
    //     List<Truck> trucks = currentSolution.Trucks.Select(t => t.Clone()).ToList();
    //     Truck truckACopy = trucks.First(t => t.Id == truckA.Id);
    //     Truck truckBCopy = trucks.First(t => t.Id == truckB.Id);

    //     // Intercambiar las zonas
    //     // Insertar la zona de truckA en la posición idxB de truckB y eliminarla de truckA
    //     Zone zoneToMove = truckACopy.Path[idxA];
    //     Console.WriteLine($"ANTESSSS");
    //     Console.WriteLine($"Truck A: {truckACopy.CurrentTime} {truckACopy.CurrentLoad}");
    //     Console.WriteLine($"Truck A Zones: {string.Join(", ", truckACopy.Path.Select(z => z.Id))}");
    //     Console.WriteLine($"Truck B: {truckACopy.CurrentTime} {truckACopy.CurrentLoad}");
    //     Console.WriteLine($"Truck B Zones: {string.Join(", ", truckBCopy.Path.Select(z => z.Id))}");  
    //     truckBCopy.SetPath.Insert(idxB, (zoneToMove, (int)truckBCopy.Path[idxB].Position.CalculateDistance(truckACopy.Path[idxA].Position) / truckACopy.Speed, truckACopy.Path[idxA].Load));
    //     truckACopy.SetPath.RemoveAt(idxA);

    //     // Recalcular ambos camiones
    //     truckACopy.RecalculatePath();
    //     truckBCopy.RecalculatePath();
    //     Console.WriteLine($"DESPUESSSS");
    //     // Comprobar factibilidad
    //     Console.WriteLine($"Truck A: {truckACopy.CurrentTime} {truckACopy.CurrentLoad}");
    //     Console.WriteLine($"Truck A Zones: {string.Join(", ", truckACopy.Path.Select(z => z.Id))}");
    //     Console.WriteLine($"Truck B: {truckBCopy.CurrentTime} {truckBCopy.CurrentLoad}");
    //     Console.WriteLine($"Truck B Zones: {string.Join(", ", truckBCopy.Path.Select(z => z.Id))}");
    //     if (truckACopy.CurrentTime > truckACopy.MaximumTime || truckACopy.CurrentLoad > truckACopy.MaximumLoad)
    //         return currentSolution;
    //     if (truckBCopy.CurrentTime > truckBCopy.MaximumTime || truckBCopy.CurrentLoad > truckBCopy.MaximumLoad)
    //         return currentSolution;

    //     // Reemplazar los camiones en la solución
    //     Console.WriteLine($"Trucks antes: {string.Join(", ", trucks.Select(z => z.Id))}");
    //     trucks.RemoveAll(t => t.Id == truckA.Id || t.Id == truckB.Id);
    //     Console.WriteLine($"Trucks borrado: {string.Join(", ", trucks.Select(z => z.Id))}");

    //     trucks.Add(truckACopy);
    //     trucks.Add(truckBCopy);
    //     Console.WriteLine($"Trucks añadido: {string.Join(", ", trucks.Select(z => z.Id))}");


    //     Solution newSolution = new Solution(trucks);
    //     newSolution.TotalTime = currentSolution.TotalTime - truckA.CurrentTime - truckB.CurrentTime
    //                             + truckACopy.CurrentTime + truckBCopy.CurrentTime;
    //     newSolution.NumVehicles = currentSolution.NumVehicles;

    //     Console.WriteLine($"Swapped zone {truckA.Path[idxA].Id} (Truck {truckA.Id}) with zone {truckB.Path[idxB].Id} (Truck {truckB.Id})");

    //     return newSolution;
    // }

public Solution MovementDoBetter(Truck truckA, int idxA, Truck truckB, int idxB, Solution currentSolution) {
    List<Truck> trucks = currentSolution.Trucks.Select(t => t.Clone()).ToList();
    Truck truckACopy = trucks.First(t => t.Id == truckA.Id);
    Truck truckBCopy = trucks.First(t => t.Id == truckB.Id);

    var pathA = truckACopy.SetPath.Select(t => t.Item1).ToList();
    var pathB = truckBCopy.SetPath.Select(t => t.Item1).ToList();

    Zone movedZone = pathA[idxA];
    pathA.RemoveAt(idxA);
    pathB.Insert(idxB, movedZone);

    truckACopy.SetPath = pathA.Select(z => (z, 0, 0)).ToList();
    truckBCopy.SetPath = pathB.Select(z => (z, 0, 0)).ToList();

    truckACopy.RecalculatePath();
    truckBCopy.RecalculatePath();
    // Console.WriteLine($"Truck A: {truckACopy.CurrentTime} {truckACopy.CurrentLoad}");
    // Console.WriteLine($"Truck A Zones: {string.Join(", ", truckACopy.Path.Select(z => z.Id))}");
    // Console.WriteLine($"Truck B: {truckBCopy.CurrentTime} {truckBCopy.CurrentLoad}");
    // Console.WriteLine($"Truck B Zones: {string.Join(", ", truckBCopy.Path.Select(z => z.Id))}");

    if (truckACopy.CurrentTime > truckACopy.MaximumTime || truckACopy.CurrentLoad > truckACopy.MaximumLoad ||
        truckBCopy.CurrentTime > truckBCopy.MaximumTime || truckBCopy.CurrentLoad > truckBCopy.MaximumLoad) {
        return currentSolution;
    }

    trucks.RemoveAll(t => t.Id == truckA.Id || t.Id == truckB.Id);
    trucks.Add(truckACopy);
    trucks.Add(truckBCopy);

    Solution newSolution = new Solution(trucks);
    newSolution.TotalTime = currentSolution.TotalTime - truckA.CurrentTime - truckB.CurrentTime + truckACopy.CurrentTime + truckBCopy.CurrentTime;
    newSolution.NumVehicles = currentSolution.NumVehicles;

    return newSolution;
}
    private bool IsBetterSolution(Solution newSolution, Solution bestSolution) {
        return newSolution.TotalTime < bestSolution.TotalTime;
    }
}