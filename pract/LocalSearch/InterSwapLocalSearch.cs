namespace Pract5DAA.LocalSearch;

public class InterSwapLocalSearch : ILocalSearch {
  public string GetName => "InterSwapLocalSearch";

  public Solution Solve(Solution solution) {
    Solution bestSolution = new Solution();
    return bestSolution;
  }
  public bool FactibleMovement(Solution solution, int i, int j) {
    return true;
  }
}