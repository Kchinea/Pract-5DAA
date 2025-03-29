namespace Pract5DAA;

public class Zone {
  private int _id;
  private Point _cord;
  private double _D1;
  private double _D2;
  public Zone(int id, Point cord, double D1, double D2) {
    this._id = id;
    this._cord = cord;
    this._D1 = D1;
    this._D2 = D2;
  }
  public override string ToString() {
    return $"Zone {_id}: Position {_cord}, D1={_D1}, D2={_D2}";
  }
}