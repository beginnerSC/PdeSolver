# Examples

## Heat Equation

The classical heat equation with an exact analytical solution.

$$u_t = u_{xx}, \quad x \in [0,1], \quad t \in [0, 0.1]$$

- **Initial condition:** $u(0, x) = \sin(\pi x)$
- **Boundary conditions:** $u(t, 0) = 0$, $u(t, 1) = 0$
- **Exact solution:** $u(t, x) = e^{-\pi^2 t} \sin(\pi x)$

```csharp
using PdeSolver;

var domain = new Domain1D(0.0, 1.0, 0.0, 0.1);
var grid = new Grid1D(domain, 101, 1000);

var pde = new Pde1D
{
    Diffusion    = (t, x) => 1.0,
    Initial      = (t, x) => Math.Sin(Math.PI * x),
    BoundaryLeft  = (t, x) => 0.0,
    BoundaryRight = (t, x) => 0.0
};

var solver = PdeSolverFactory.CreateSolver1D(grid, pde);
double[] u = solver.Solve();

// Compare with exact solution
double maxErr = 0.0;
for (int i = 0; i < grid.Nx; i++)
{
    double exact = Math.Exp(-Math.PI * Math.PI * domain.T1) * Math.Sin(Math.PI * grid.X(i));
    double err = Math.Abs(u[i] - exact);
    if (err > maxErr) maxErr = err;
}
Console.WriteLine($"Max error: {maxErr:E6}");
// Output: Max error: 3.022465E-005
```

## Advection-Diffusion

A Gaussian bump transported by advection with small diffusion.

$$u_t = 0.01\, u_{xx} - u_x, \quad x \in [0,2], \quad t \in [0, 0.5]$$

```csharp
var domain = new Domain1D(0.0, 2.0, 0.0, 0.5);
var grid = new Grid1D(domain, 201, 5000);

var pde = new Pde1D
{
    Diffusion = (t, x) => 0.01,
    Advection = (t, x) => -1.0,
    Initial   = (t, x) =>
    {
        double d = x - 0.5;
        return Math.Exp(-50.0 * d * d);
    },
    BoundaryLeft  = (t, x) => 0.0,
    BoundaryRight = (t, x) => 0.0
};

var solver = PdeSolverFactory.CreateSolver1D(grid, pde);
double[] u = solver.Solve();
```

## Reaction-Diffusion with Source

A reaction-diffusion equation that approaches a steady state.

$$u_t = 0.1\, u_{xx} - 0.5\, u + 1.0, \quad x \in [0,1], \quad t \in [0, 2]$$

- **Initial condition:** $u(0, x) = 0$
- **Boundary conditions:** $u(t, 0) = 2$, $u(t, 1) = 2$
- **Expected:** Solution approaches steady state $u \approx 2$

```csharp
var domain = new Domain1D(0.0, 1.0, 0.0, 2.0);
var grid = new Grid1D(domain, 51, 2000);

var pde = new Pde1D
{
    Diffusion    = (t, x) => 0.1,
    Reaction     = (t, x) => -0.5,
    Source       = (t, x) => 1.0,
    Initial      = (t, x) => 0.0,
    BoundaryLeft  = (t, x) => 2.0,
    BoundaryRight = (t, x) => 2.0
};

var solver = PdeSolverFactory.CreateSolver1D(grid, pde);
double[] u = solver.Solve();
```
