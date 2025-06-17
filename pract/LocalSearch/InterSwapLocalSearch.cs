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
  

    private bool IsBetterSolution(Solution newSolution, Solution bestSolution) {
        return newSolution.TotalTime < bestSolution.TotalTime;
    }
}