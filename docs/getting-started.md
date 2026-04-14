# Getting Started

## Installation

```bash
dotnet add package PdeSolver
```

## Basic Usage

PdeSolver solves 1D parabolic PDEs of the form:

$$u_t = a(t,x)\, u_{xx} + b(t,x)\, u_x + c(t,x)\, u + f(t,x)$$

### Step 1: Define a Domain

The domain specifies the spatial interval `[xLeft, xRight]` and time interval `[t0, t1]`.

```csharp
using PdeSolver;

var domain = new Domain1D(xLeft: 0.0, xRight: 1.0, t0: 0.0, t1: 0.1);
```

### Step 2: Create a Grid

The grid discretizes the domain into `nx` spatial points and `nt` time steps.

```csharp
var grid = new Grid1D(domain, nx: 101, nt: 1000);
```

### Step 3: Define the PDE

Set the coefficient functions, initial condition, and boundary conditions.
All coefficients are functions of `(t, x)` — use C# lambdas.

```csharp
var pde = new Pde1D
{
    Diffusion    = (t, x) => 1.0,                    // a(t,x)
    Advection    = (t, x) => 0.0,                    // b(t,x)
    Reaction     = (t, x) => 0.0,                    // c(t,x)
    Source       = (t, x) => 0.0,                    // f(t,x)
    Initial      = (t, x) => Math.Sin(Math.PI * x),  // u(0, x)
    BoundaryLeft  = (t, x) => 0.0,                   // u(t, 0)
    BoundaryRight = (t, x) => 0.0                    // u(t, 1)
};
```

### Step 4: Solve

```csharp
var solver = PdeSolverFactory.CreateSolver1D(grid, pde);
double[] solution = solver.Solve();
```

The returned `double[]` has length `nx` — one value per spatial grid point at the final time `t1`.

## Theta Parameter

The solver uses a theta-scheme. The default is `theta = 0.5` (Crank-Nicolson).

| Theta | Scheme | Properties |
|-------|--------|------------|
| 0.0 | Forward Euler (explicit) | Conditionally stable |
| 0.5 | Crank-Nicolson | Unconditionally stable, 2nd-order |
| 1.0 | Backward Euler (implicit) | Unconditionally stable, 1st-order |

```csharp
var solver = PdeSolverFactory.CreateSolver1D(grid, pde);
solver.Theta = 1.0; // Use backward Euler instead
double[] solution = solver.Solve();
```

## Boundary Conditions

Currently only **Dirichlet** boundary conditions are supported (specify the solution value at boundaries). Neumann and Gamma types are defined in the enum for future use.
