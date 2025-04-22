namespace Pract5DAA.LocalSearch {

  public class InterAddLocalSearch : ILocalSearch {
    public string GetName => "InterAddLocalSearch";
    
    private Solution _original;
    private Instance _instance;

    public InterAddLocalSearch(Solution original, Instance instance) {
      _original = original;
      _instance = instance;
    }

    public Solution Solve() {
      // Copiar la solución original para evitar modificarla directamente
      Solution bestSolution = _original.Clone();
  
      // Variable para comparar el tiempo total de la mejor solución encontrada
      int bestTotalTime = bestSolution.TotalTime;
      Console.WriteLine("Tiempo total de la mejor solución inicial: " + bestTotalTime);

      // Recorremos cada camión de la solución
      for (int i = 0; i < bestSolution.Trucks.Count; i++) {
        Truck truck = bestSolution.Trucks[i];
        Console.WriteLine($"Procesando camión {i + 1} con ruta: {string.Join(" -> ", truck.Path.Select(z => z.Id))}");

        // Recorremos cada zona del camión para moverla
        for (int j = 1; j < truck.Path.Count - 1; j++) {
          Zone zoneToMove = truck.Path[j];

          // Si la zona es una estación, saltamos
          if (zoneToMove.Id <= 0) {
            continue;
          }

          // Recorremos todos los demás camiones para intentar añadir la zona en cada ruta
          for (int k = 0; k < bestSolution.Trucks.Count; k++) {
            if (i == k) continue;  // No podemos moverla dentro del mismo camión

            Truck otherTruck = bestSolution.Trucks[k];
            Console.WriteLine($"Probando añadir zona {zoneToMove.Id} en el camión {k + 1}");

            // Probar insertar la zona en cada posición de la ruta del otro camión
            for (int m = 1; m < otherTruck.Path.Count; m++) {
              Console.WriteLine($"Intentando insertar zona {zoneToMove.Id} en la posición {m} de la ruta del camión {k + 1}");
  
              // Crear un clon del camión para probar el movimiento
              Truck truckClone = otherTruck.Clone();
              truckClone.Path.Insert(m, zoneToMove); // Insertamos la zona en la nueva posición

              // Verificar si el movimiento es factible
              if (!FactibleMovement(truckClone)) {
                Console.WriteLine("Movimiento no factible, se omite.");
                continue; // Si no es factible, saltamos
              }

              // Crear un clon del camión original que vamos a modificar
              Truck truckCloneOriginal = truck.Clone();
              truckCloneOriginal.Path.RemoveAt(j); // Eliminamos la zona de su posición original

              // Calcular el nuevo tiempo total de la solución con este movimiento
              int newTotalTime = 0;
              foreach (var t in bestSolution.Trucks) {
                newTotalTime += t.CurrentTime;
              }

              // Si encontramos una solución mejor, la actualizamos
              if (newTotalTime < bestTotalTime) {
                Console.WriteLine($"Nueva mejor solución encontrada con tiempo total: {newTotalTime}");
                bestTotalTime = newTotalTime;
                bestSolution = bestSolution.Clone(); // Clonamos la solución para evitar modificar la original
                bestSolution.Trucks[i] = truckCloneOriginal; // Reemplazamos el camión modificado
                bestSolution.Trucks[k] = truckClone; // Reemplazamos el camión que recibió la zona
                // bestSolution.TotalTime = newTotalTime; // Actualizamos el tiempo total
              }
            }
          }
        }
      }

      // Devolver la mejor solución encontrada
      Console.WriteLine("Solución final con el mejor tiempo total: " + bestSolution.TotalTime);
      return bestSolution;
    }

    public bool FactibleMovement(Truck truck) {
      int currentLoad = 0;
      int currentTime = 0;

      // Verificar desde la primera zona hasta la primera estación (Id == -1 o -2)
      bool firstStationFound = false;
      for (int i = 0; i < truck.Path.Count; i++) {
        Zone currentZone = truck.Path[i];
        // Verificamos si llegamos a una estación
        if (currentZone.Id == -1 || currentZone.Id == -2) {
          firstStationFound = true;
          break; // Terminamos la validación hasta la primera estación
        }
        // Actualizamos la carga y el tiempo
        currentLoad += currentZone.Load;
        if (i + 1 < truck.Path.Count) {
          currentTime += currentZone.TimeToNext(truck.Path[i + 1], truck.Speed);
        }
      }
      // Si no encontramos la primera estación en la ruta, no es factible
      if (!firstStationFound) {
        return false;
      }

      // Verificar desde la última zona hasta la última estación (Id == -1 o -2)
      bool lastStationFound = false;
      for (int i = truck.Path.Count - 1; i >= 0; i--) {
        Zone currentZone = truck.Path[i];
        // Verificamos si llegamos a una estación
        if (currentZone.Id == -1 || currentZone.Id == -2) {
          lastStationFound = true;
          break; // Terminamos la validación hasta la última estación
        }
        // Actualizamos la carga y el tiempo
        currentLoad += currentZone.Load;
        if (i - 1 >= 0) {
          currentTime += currentZone.TimeToNext(truck.Path[i - 1], truck.Speed);
        }
      }
      // Si no encontramos la última estación en la ruta, no es factible
      if (!lastStationFound) {
        return false;
      }

      // Comprobar la carga total
      if (currentLoad > truck.MaximumLoad) {
        return false; // La carga excede el límite
      }

      // Comprobar el tiempo total
      if (currentTime > truck.MaximumTime) {
        return false; // El tiempo excede el límite
      }

      return true; // Si hemos pasado todas las comprobaciones, el movimiento es factible
    }
  }
}
