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

        // Console.WriteLine($"Truck {truck.Id} moved {movedZone.Id} from {i} to {j}, new time: {truckCopy.CurrentTime}");

        return newSolution;
    }

    private bool IsBetterSolution(Solution newSolution, Solution bestSolution) {
        return newSolution.TotalTime < bestSolution.TotalTime;
    }
}
