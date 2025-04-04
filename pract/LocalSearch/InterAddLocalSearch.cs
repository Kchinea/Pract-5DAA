namespace Pract5DAA.LocalSearch;

public class InterAddLocalSearch : ILocalSearch {
  public string GetName => "InterAddLocalSearch";

  public Solution Solve(Solution solution) {
    Solution bestSolution = new Solution();
    return bestSolution;
  }
  public bool FactibleMovement(Solution solution, int i, int j) {
    return true;
  }
}