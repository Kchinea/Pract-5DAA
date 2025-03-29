namespace Pract5DAA;

public class PathMap {
  private List<Zone> _zones;
  public PathMap(List<Zone> zones) {
    _zones = zones;
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