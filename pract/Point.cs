  namespace Pract5DAA;

  public class Point {
    public int _X;
    public int _Y;
    public Point(int x, int y){
      this._X = x;
      this._Y = y;
    }
    public double CalculateDistance(Point other){
      double dx = this.X - other.X;
      double dy = this.Y - other.Y;
      return Math.Sqrt(dx * dx + dy * dy);
    }
    public int X {
      get => _X;
    }
    public int Y {
      get => _Y;
    }
    public override string ToString() {
      return $"x: {_X}, y: {_Y}";
    }
  }