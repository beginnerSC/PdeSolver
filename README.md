# PdeSolver

A .NET library for solving 1D parabolic partial differential equations using the Crank-Nicolson finite difference method.

## PDE Form

The solver handles PDEs of the form:

$$u_t = a(t,x)\, u_{xx} + b(t,x)\, u_x + c(t,x)\, u + f(t,x)$$

where:
- `a(t,x)` — diffusion coefficient
- `b(t,x)` — advection coefficient
- `c(t,x)` — reaction coefficient
- `f(t,x)` — source term

## Installation

```bash
dotnet add package PdeSolver
```

## Quick Start

```csharp
using PdeSolver;

// Define domain: x in [0,1], t in [0,0.1]
var domain = new Domain1D(0.0, 1.0, 0.0, 0.1);

// Create grid: 101 spatial points, 1000 time steps
var grid = new Grid1D(domain, 101, 1000);

// Define PDE (heat equation with sin(πx) initial condition)
var pde = new Pde1D
{
    Diffusion    = (t, x) => 1.0,
    Initial      = (t, x) => Math.Sin(Math.PI * x),
    BoundaryLeft  = (t, x) => 0.0,
    BoundaryRight = (t, x) => 0.0
};

// Solve
var solver = PdeSolverFactory.CreateSolver1D(grid, pde);
double[] solution = solver.Solve();
```

## Features

- **Crank-Nicolson scheme** — second-order accurate in both time and space
- **Configurable theta parameter** — set `solver.Theta` to 0.0 (explicit), 0.5 (Crank-Nicolson), or 1.0 (implicit)
- **Flexible coefficients** — diffusion, advection, reaction, and source can be functions of `(t, x)`
- **Dirichlet boundary conditions** — left and right boundary values as functions of `(t, x)`
- **NuGet-ready** — packaged for distribution via NuGet

## API Reference

### Core Types

| Type | Description |
|------|-------------|
| `Domain1D` | Defines spatial `[xL, xR]` and temporal `[t0, t1]` bounds |
| `Grid1D` | Uniform discretization with `Nx` spatial points and `Nt` time steps |
| `Pde1D` | PDE definition: coefficients, initial condition, boundary conditions |
| `BoundaryType` | Enum: `Dirichlet`, `Neumann`, `Gamma` |
| `BoundaryConfig1D` | Stores boundary types for left/right boundaries |
| `CoefficientFunction` | Delegate: `double CoefficientFunction(double t, double x)` |

### Solver

| Type | Description |
|------|-------------|
| `CrankNicolsonSolver1D` | Main solver class (in `PdeSolver.Schemes` namespace) |
| `PdeSolverFactory` | Factory with `CreateSolver1D(grid, pde)` method |

### Examples

See the `examples/PdeSolver.Examples/Program.cs` file for complete working examples:
1. **Heat equation** — with exact solution comparison
2. **Advection-diffusion** — Gaussian bump transport
3. **Reaction-diffusion with source** — approach to steady state

## Building

```bash
# Restore and build
dotnet build

# Run tests
dotnet test

# Run examples
dotnet run --project examples/PdeSolver.Examples

# Create NuGet package
dotnet pack src/PdeSolver -c Release
```

## Project Structure

```
PdeSolver/
├── PdeSolver.sln
├── src/
│   └── PdeSolver/              # Class library (NuGet package)
│       ├── BoundaryType.cs
│       ├── BoundaryConfig1D.cs
│       ├── CoefficientFunction.cs
│       ├── Domain1D.cs
│       ├── Grid1D.cs
│       ├── Pde1D.cs
│       ├── PdeSolverFactory.cs
│       ├── TridiagonalSolver.cs
│       └── Schemes/
│           └── CrankNicolsonSolver1D.cs
├── tests/
│   └── PdeSolver.Tests/        # xUnit test project
└── examples/
    └── PdeSolver.Examples/     # Console demo app
```

## License

MIT
