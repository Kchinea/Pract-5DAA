namespace Pract5DAA.LocalSearch;

public class IntraAddLocalSearch : ILocalSearch {
  public string GetName => "IntraAddLocalSearch";

  public Solution Solve(Solution solution) {
    Solution bestSolution = new Solution();
    return bestSolution;
  }
  public bool FactibleMovement(Solution solution, int i, int j) {
    return true;
  }
}