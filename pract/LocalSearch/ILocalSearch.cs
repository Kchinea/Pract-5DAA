namespace Pract5DAA.LocalSearch;

public interface ILocalSearch {
  public Solution Solve(Solution solution, PathMap map);
  string GetName { get; }
}