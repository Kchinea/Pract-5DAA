public class Point {
  public int X { get; }
  public int Y { get; }

  public Point(int x, int y){
    X = x;
    Y = y;
  }

  public double CalculateDistance(Point other){
    double dx = X - other.X;
    double dy = Y - other.Y;
    return Math.Sqrt(dx * dx + dy * dy);
  }

  public override string ToString() {
    return $"x: {X}, y: {Y}";
  }

  public Point Clone() => new Point(X, Y);
}