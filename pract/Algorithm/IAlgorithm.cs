namespace Pract5DAA.Algorithm;

public interface IAlgorithm {
  public Solution Solve(Instance instance);
  string GetName { get; }
}