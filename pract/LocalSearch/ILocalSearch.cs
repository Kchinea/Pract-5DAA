namespace Pract5DAA.LocalSearch;
using Pract5DAA.Algorithm;

public interface ILocalSearch {
  public Solution Solve(Solution solution);
  string GetName { get; }
}