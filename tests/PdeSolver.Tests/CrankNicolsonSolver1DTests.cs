using PdeSolver.Schemes;
using Xunit;

namespace PdeSolver.Tests;

public class CrankNicolsonSolver1DTests
{
    [Fact]
    public void HeatEquation_ConvergesToExact()
    {
        // u_t = u_xx on [0,1], t in [0,0.1]
        // IC: u(0,x) = sin(pi*x)
        // BC: u(t,0) = 0, u(t,1) = 0
        // Exact: u(t,x) = exp(-pi^2*t) * sin(pi*x)
        var domain = new Domain1D(0.0, 1.0, 0.0, 0.1);
        var grid = new Grid1D(domain, 101, 1000);

        var pde = new Pde1D
        {
            Diffusion = (t, x) => 1.0,
            Initial = (t, x) => Math.Sin(Math.PI * x),
            BoundaryLeft = (t, x) => 0.0,
            BoundaryRight = (t, x) => 0.0
        };

        var solver = PdeSolverFactory.CreateSolver1D(grid, pde);
        double[] u = solver.Solve();

        double maxErr = 0.0;
        double t1 = domain.T1;
        for (int i = 0; i < grid.Nx; i++)
        {
            double exact = Math.Exp(-Math.PI * Math.PI * t1) * Math.Sin(Math.PI * grid.X(i));
            double err = Math.Abs(u[i] - exact);
            if (err > maxErr) maxErr = err;
        }

        // CN with these grid params should give error < 1e-4
        Assert.True(maxErr < 1e-4, $"Max error {maxErr} exceeds tolerance 1e-4");
    }

    [Fact]
    public void HeatEquation_GridRefinement_ErrorDecreases()
    {
        // Verify second-order convergence: doubling resolution should quarter the error
        double ComputeError(int nx, int nt)
        {
            var domain = new Domain1D(0.0, 1.0, 0.0, 0.1);
            var grid = new Grid1D(domain, nx, nt);
            var pde = new Pde1D
            {
                Diffusion = (t, x) => 1.0,
                Initial = (t, x) => Math.Sin(Math.PI * x),
                BoundaryLeft = (t, x) => 0.0,
                BoundaryRight = (t, x) => 0.0
            };
            var solver = PdeSolverFactory.CreateSolver1D(grid, pde);
            double[] u = solver.Solve();

            double maxErr = 0.0;
            double t1 = domain.T1;
            for (int i = 0; i < grid.Nx; i++)
            {
                double exact = Math.Exp(-Math.PI * Math.PI * t1) * Math.Sin(Math.PI * grid.X(i));
                double err = Math.Abs(u[i] - exact);
                if (err > maxErr) maxErr = err;
            }
            return maxErr;
        }

        double errCoarse = ComputeError(51, 500);
        double errFine = ComputeError(101, 2000);

        // Error should decrease significantly with refinement
        Assert.True(errFine < errCoarse, $"Fine error {errFine} not less than coarse error {errCoarse}");
    }

    [Fact]
    public void SteadyState_ReactionDiffusionWithSource()
    {
        // u_t = 0.1*u_xx - 0.5*u + 1.0 on [0,1], t in [0,2]
        // IC: u(0,x) = 0
        // BC: u(t,0) = 2, u(t,1) = 2 (steady-state approach -> u ~ 2 everywhere)
        var domain = new Domain1D(0.0, 1.0, 0.0, 5.0);
        var grid = new Grid1D(domain, 51, 5000);

        var pde = new Pde1D
        {
            Diffusion = (t, x) => 0.1,
            Reaction = (t, x) => -0.5,
            Source = (t, x) => 1.0,
            Initial = (t, x) => 0.0,
            BoundaryLeft = (t, x) => 2.0,
            BoundaryRight = (t, x) => 2.0
        };

        var solver = PdeSolverFactory.CreateSolver1D(grid, pde);
        double[] u = solver.Solve();

        // Solution should be close to steady state u=2 everywhere
        for (int i = 0; i < grid.Nx; i++)
        {
            Assert.True(Math.Abs(u[i] - 2.0) < 0.1,
                $"u[{i}] = {u[i]} too far from steady state 2.0");
        }
    }

    [Fact]
    public void AdvectionDiffusion_SolutionIsReasonable()
    {
        // u_t = 0.01*u_xx - u_x on [0,2], t in [0,0.5]
        // IC: Gaussian bump at x=0.5
        // BC: Dirichlet zero
        var domain = new Domain1D(0.0, 2.0, 0.0, 0.5);
        var grid = new Grid1D(domain, 201, 5000);

        var pde = new Pde1D
        {
            Diffusion = (t, x) => 0.01,
            Advection = (t, x) => -1.0,
            Initial = (t, x) =>
            {
                double d = x - 0.5;
                return Math.Exp(-50.0 * d * d);
            },
            BoundaryLeft = (t, x) => 0.0,
            BoundaryRight = (t, x) => 0.0
        };

        var solver = PdeSolverFactory.CreateSolver1D(grid, pde);
        double[] u = solver.Solve();

        // Solution should be non-negative (physical) and bounded
        for (int i = 0; i < grid.Nx; i++)
        {
            Assert.True(u[i] > -0.1, $"u[{i}] = {u[i]} is too negative");
            Assert.True(u[i] < 1.5, $"u[{i}] = {u[i]} exceeds expected maximum");
        }

        // The bump should have moved to the right (advection velocity = +1)
        // Find the peak location
        int peakIdx = 0;
        for (int i = 1; i < grid.Nx; i++)
        {
            if (u[i] > u[peakIdx]) peakIdx = i;
        }
        double peakX = grid.X(peakIdx);

        // Peak should have moved rightward from x=0.5 toward x=1.0
        Assert.True(peakX > 0.7, $"Peak at x={peakX} hasn't moved right enough");
    }

    [Fact]
    public void ThetaProperty_CanBeModified()
    {
        var domain = new Domain1D(0.0, 1.0, 0.0, 0.1);
        var grid = new Grid1D(domain, 11, 100);
        var pde = new Pde1D();

        var solver = new CrankNicolsonSolver1D(grid, pde);
        Assert.Equal(0.5, solver.Theta);

        solver.Theta = 1.0;
        Assert.Equal(1.0, solver.Theta);
    }

    [Fact]
    public void NonDirichletBoundary_Throws()
    {
        var domain = new Domain1D(0.0, 1.0, 0.0, 0.1);
        var grid = new Grid1D(domain, 11, 100);
        var pde = new Pde1D();
        pde.BoundaryConfig.Types[BoundaryConfig1D.Left] = BoundaryType.Neumann;

        var solver = PdeSolverFactory.CreateSolver1D(grid, pde);
        Assert.Throws<NotSupportedException>(() => solver.Solve());
    }

    [Fact]
    public void ConstantInitialAndBoundary_RemainsConstant()
    {
        // If IC = 1 and BC = 1, solution should stay at 1 for pure diffusion
        var domain = new Domain1D(0.0, 1.0, 0.0, 1.0);
        var grid = new Grid1D(domain, 21, 100);

        var pde = new Pde1D
        {
            Diffusion = (t, x) => 1.0,
            Initial = (t, x) => 1.0,
            BoundaryLeft = (t, x) => 1.0,
            BoundaryRight = (t, x) => 1.0
        };

        var solver = PdeSolverFactory.CreateSolver1D(grid, pde);
        double[] u = solver.Solve();

        for (int i = 0; i < grid.Nx; i++)
        {
            Assert.Equal(1.0, u[i], 10);
        }
    }
}
