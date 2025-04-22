namespace Pract5DAA.LocalSearch;

public interface ILocalSearch {
  public Solution Solve();
  string GetName { get; }
}