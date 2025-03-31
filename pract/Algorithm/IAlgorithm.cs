namespace Pract5DAA.Algorithm;

public interface IAlgorithm {
  public int Solve(Instance instance);
  string GetName { get; }
}