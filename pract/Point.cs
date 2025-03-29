namespace Pract5DAA;

public class Point {
  public int _X;
  public int _Y;
  public Point(int x, int y){
    this._X = x;
    this._Y = y;
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