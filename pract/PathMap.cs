namespace Pract5DAA;

public class PathMap {
  private List<Zone> _zones;
  public PathMap(List<Zone> zones) {
    _zones = new List<Zone>(zones.Select(z => new Zone(z.Id, z.Position, z.D1, z.D2)));
  }
  public List<Zone> Zones {
    get => _zones;
  }
  public Zone ClosestZone(Point point) {
    double minDistance = double.MaxValue;
    Zone closest = _zones[0];
    foreach (Zone z in _zones) {
      double distance = point.CalculateDistance(z.Position);
      if (distance < minDistance) {
        minDistance = distance;
        closest = z;
      }
    }
    return closest;
  }
public Zone RandomClosestZone(Point point) {
    List<(Zone zone, double distance)> distances = new List<(Zone, double)>();
    foreach (Zone z in _zones) {
      double distance = point.CalculateDistance(z.Position);
      distances.Add((z, distance));
    }
    for (int i = 0; i < Math.Min(5, distances.Count); i++) {
      int minIndex = i;
      for (int j = i + 1; j < distances.Count; j++) {
        if (distances[j].distance < distances[minIndex].distance) {
          minIndex = j;
        }
      }
      (Zone zone, double distance) temp = distances[i];
      distances[i] = distances[minIndex];
      distances[minIndex] = temp;
    }
    Random random = new Random();
    int randomIndex = random.Next(Math.Min(5, distances.Count));
    return distances[randomIndex].zone;
  }
  public int length {
    get => _zones.Count;
  }
  public bool RemoveZone(Zone zone) {
    return _zones.Remove(zone);
  }
  public bool IsEmpty() {
    return _zones.Count == 0;
  }
  public void AddZone(Zone zone) {
    _zones.Add(zone);
  }
}