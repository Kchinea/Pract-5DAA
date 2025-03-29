namespace Pract5DAA;

public class PathMap {
  private List<Zone> _zones;
  public PathMap(List<Zone> zones) {
    _zones = zones;
  }
  public List<Zone> Zones {
    get => _zones;
  }
  public bool RemoveZone(Zone zone) {
    return _zones.Remove(zone);
  }
}