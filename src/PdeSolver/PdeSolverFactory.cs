using PdeSolver.Schemes;

namespace PdeSolver;

/// <summary>
/// Provides factory methods for creating PDE solvers.
/// </summary>
public static class PdeSolverFactory
{
    /// <summary>
    /// Creates a Crank-Nicolson solver for a 1D PDE on the specified grid.
    /// </summary>
    /// <param name="grid">The computational grid.</param>
    /// <param name="pde">The PDE definition.</param>
    /// <returns>A configured <see cref="CrankNicolsonSolver1D"/> instance.</returns>
    public static CrankNicolsonSolver1D CreateSolver1D(Grid1D grid, Pde1D pde)
    {
        return new CrankNicolsonSolver1D(grid, pde);
    }
}
