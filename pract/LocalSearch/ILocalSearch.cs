namespace Pract5DAA.LocalSearch;

public interface ILocalSearch {
  public Solution Solve(Solution solution);
  public bool FactibleMovement(Solution solution, int i, int j);
  string GetName { get; }
}