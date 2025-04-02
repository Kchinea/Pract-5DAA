namespace Pract5DAA;

public class Zone {
  private int _id;
  private Point _cord;
  private int _D1;
  private int _D2;
  public Zone(int id, Point cord, int D1, int D2) {
    this._id = id;
    this._cord = cord;
    this._D1 = D1;
    this._D2 = D2;
  }
    public int TimeToNext(Zone zone, int speed) {
    double distance = _cord.CalculateDistance(zone.Position);
    int time = (int)Math.Ceiling(distance / speed * 60);
    return time;
  }
  public override string ToString() {
    return $"Zone {_id}: Position {_cord}, D1={_D1}, D2={_D2}";
  }
  public int Id {
    get => _id;
  }
  public int CollectionTime {
    get => _D1;
  }
  public int Load {
    get => _D2;
  }
  public Point Position {
    get => _cord;
  }
  // public int Load {
  //   get => _D2 - _D1;
  // }
}