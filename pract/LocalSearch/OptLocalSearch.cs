namespace Pract5DAA.LocalSearch;

public class OptLocalSearch : ILocalSearch {
  public string GetName => "2-OptLocalSearch";

  public Solution Solve(Solution solution) {
    Solution bestSolution = new Solution();
    return bestSolution;
  }
  public bool FactibleMovement(Solution solution, int i, int j) {
    return true;
  }
}