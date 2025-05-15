namespace Pract5DAA.LocalSearch;

public class IntraSwapLocalSearch : ILocalSearch {
    public string GetName => "IntraSwapLocalSearch";

    public Solution Solve(Solution solution, PathMap map) {
        Solution bestSolution = new Solution(solution.Trucks.Select(t => t.Clone()).ToList());

        bool improvement = true;
        while (improvement) {
            improvement = false;

            foreach (Truck truck in bestSolution.Trucks.ToList()) {
                for (int i = 1; i < truck.Path.Count - 1; i++) {
                    for (int j = i + 1; j < truck.Path.Count - 1; j++) {
                        Solution newSolution = MovementDoBetter(truck, i, j, bestSolution);
                        if (IsBetterSolution(newSolution, bestSolution)) {
                            bestSolution = newSolution;
                            improvement = true;
                            goto Restart;
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

        // Intercambiar las zonas en i y j
        var temp = truckCopy.Path[i];
        truckCopy.Path[i] = truckCopy.Path[j];
        truckCopy.Path[j] = temp;

        // Recalcular tiempo y carga
        truckCopy.RecalculatePath();

        if (truckCopy.CurrentTime > truckCopy.MaximumTime || truckCopy.CurrentLoad > truckCopy.MaximumLoad)
            return currentSolution;

        // Reemplazar el camión en la solución
        trucks.RemoveAll(t => t.Id == truck.Id);
        trucks.Add(truckCopy);

        Solution newSolution = new Solution(trucks);
        newSolution.TotalTime = currentSolution.TotalTime - truck.CurrentTime + truckCopy.CurrentTime;
        newSolution.NumVehicles = currentSolution.NumVehicles;

        // Console.WriteLine($"Truck {truck.Id} swapped {i} and {j}, new time: {truckCopy.CurrentTime}");

        return newSolution;
    }

    private bool IsBetterSolution(Solution newSolution, Solution bestSolution) {
        return newSolution.TotalTime < bestSolution.TotalTime;
    }
}