namespace Pract5DAA;

public class SWTS {
  private int _id;
  private Point _cord;
  private int _D1;
  private int _D2;
  public SWTS(int id, Point cord) {
    this._id = id;
    this._cord = cord;
    this._D1 = 0;
    this._D2 = 0;
  }
    public int TimeToNext(SWTS SWTS, int speed) {
    double distance = _cord.CalculateDistance(SWTS.Position);
    int time = (int)Math.Ceiling(distance / speed * 60);
    return time;
  }
  public override string ToString() {
    return $"SWTS {_id}: Position {_cord}, Time={_D1}, Load={_D2}";
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