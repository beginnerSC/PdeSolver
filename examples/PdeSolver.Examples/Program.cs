using PdeSolver;

// === Example 1: Pure Heat Equation ===
// u_t = u_xx on [0,1], t in [0,0.1]
// IC: u(0,x) = sin(pi*x)
// BC: u(t,0) = 0, u(t,1) = 0
// Exact: u(t,x) = exp(-pi^2*t) * sin(pi*x)

Console.WriteLine("=== Heat Equation ===");

var heatDomain = new Domain1D(0.0, 1.0, 0.0, 0.1);
var heatGrid = new Grid1D(heatDomain, 101, 1000);

var heatPde = new Pde1D
{
    Diffusion = (t, x) => 1.0,
    Initial = (t, x) => Math.Sin(Math.PI * x),
    BoundaryLeft = (t, x) => 0.0,
    BoundaryRight = (t, x) => 0.0
};

var heatSolver = PdeSolverFactory.CreateSolver1D(heatGrid, heatPde);
double[] heatU = heatSolver.Solve();

double maxErr = 0.0;
for (int i = 0; i < heatGrid.Nx; i++)
{
    double exact = Math.Exp(-Math.PI * Math.PI * heatDomain.T1) * Math.Sin(Math.PI * heatGrid.X(i));
    double err = Math.Abs(heatU[i] - exact);
    if (err > maxErr) maxErr = err;
}
Console.WriteLine($"  Grid: {heatGrid.Nx} points, {heatGrid.Nt} steps");
Console.WriteLine($"  Max error vs exact: {maxErr:E6}");
Console.WriteLine();

// === Example 2: Advection-Diffusion ===
// u_t = 0.01*u_xx - u_x on [0,2], t in [0,0.5]
// IC: Gaussian bump centred at x=0.5
// BC: Dirichlet zero

Console.WriteLine("=== Advection-Diffusion ===");

var advDomain = new Domain1D(0.0, 2.0, 0.0, 0.5);
var advGrid = new Grid1D(advDomain, 201, 5000);

var advPde = new Pde1D
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

var advSolver = PdeSolverFactory.CreateSolver1D(advGrid, advPde);
double[] advU = advSolver.Solve();

Console.WriteLine("  Solution at t=0.5 (selected points):");
for (int i = 0; i < advGrid.Nx; i += 20)
{
    Console.WriteLine($"    x={advGrid.X(i):F3}  u={advU[i]:E6}");
}
Console.WriteLine();

// === Example 3: Reaction-Diffusion with Source ===
// u_t = 0.1*u_xx - 0.5*u + 1.0 on [0,1], t in [0,2]
// IC: u(0,x) = 0
// BC: u(t,0) = 2, u(t,1) = 2

Console.WriteLine("=== Reaction-Diffusion with Source ===");

var rxnDomain = new Domain1D(0.0, 1.0, 0.0, 2.0);
var rxnGrid = new Grid1D(rxnDomain, 51, 2000);

var rxnPde = new Pde1D
{
    Diffusion = (t, x) => 0.1,
    Reaction = (t, x) => -0.5,
    Source = (t, x) => 1.0,
    Initial = (t, x) => 0.0,
    BoundaryLeft = (t, x) => 2.0,
    BoundaryRight = (t, x) => 2.0
};

var rxnSolver = PdeSolverFactory.CreateSolver1D(rxnGrid, rxnPde);
double[] rxnU = rxnSolver.Solve();

Console.WriteLine("  Solution at t=2.0 (selected points):");
for (int i = 0; i < rxnGrid.Nx; i += 5)
{
    Console.WriteLine($"    x={rxnGrid.X(i):F3}  u={rxnU[i]:F6}");
}
Console.WriteLine();
