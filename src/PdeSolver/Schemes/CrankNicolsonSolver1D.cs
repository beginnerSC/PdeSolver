namespace PdeSolver.Schemes;

/// <summary>
/// Solves 1D parabolic PDEs using the Crank-Nicolson (theta) scheme.
/// The general PDE form is:
/// <code>u_t = a(t,x) u_xx + b(t,x) u_x + c(t,x) u + f(t,x)</code>
/// </summary>
/// <remarks>
/// <para>
/// The solver uses a theta-scheme where theta = 0.5 (Crank-Nicolson) by default.
/// Setting theta = 1.0 gives the fully implicit (backward Euler) scheme; theta = 0.0
/// gives the fully explicit (forward Euler) scheme.
/// </para>
/// <para>Currently only Dirichlet boundary conditions are supported.</para>
/// </remarks>
public class CrankNicolsonSolver1D
{
    private readonly Grid1D _grid;
    private readonly Pde1D _pde;

    /// <summary>
    /// Gets or sets the theta parameter for the time-stepping scheme.
    /// Default is 0.5 (Crank-Nicolson). Must be in [0, 1].
    /// </summary>
    public double Theta { get; set; } = 0.5;

    /// <summary>
    /// Initializes a new solver for the given grid and PDE definition.
    /// </summary>
    /// <param name="grid">The computational grid.</param>
    /// <param name="pde">The PDE definition including coefficients, initial and boundary conditions.</param>
    public CrankNicolsonSolver1D(Grid1D grid, Pde1D pde)
    {
        _grid = grid;
        _pde = pde;
    }

    /// <summary>
    /// Solves the PDE and returns the solution at the final time as a <c>double[]</c>
    /// of length <see cref="Grid1D.Nx"/>.
    /// </summary>
    /// <returns>
    /// An array of length <see cref="Grid1D.Nx"/> containing the solution values
    /// at each spatial grid point at the final time.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown if non-Dirichlet boundary conditions are specified.
    /// </exception>
    public double[] Solve()
    {
        int nx = _grid.Nx;
        int nt = _grid.Nt;
        double dx = _grid.Dx;
        double dt = _grid.Dt;
        double theta = Theta;

        // Validate boundary conditions (Dirichlet only for now)
        foreach (var bt in _pde.BoundaryConfig.Types)
        {
            if (bt != BoundaryType.Dirichlet)
            {
                throw new NotSupportedException(
                    "CrankNicolsonSolver1D: only Dirichlet boundary conditions are supported.");
            }
        }

        // 1. Initialize u with initial condition
        double[] u = new double[nx];
        double t0 = _grid.Domain.T0;
        for (int i = 0; i < nx; i++)
        {
            u[i] = _pde.Initial(t0, _grid.X(i));
        }

        // Number of interior points
        int nInterior = nx - 2;

        // 2. Time-stepping
        for (int k = 0; k < nt; k++)
        {
            double tn = _grid.T(k);
            double tnp1 = _grid.T(k + 1);
            double tMid = 0.5 * (tn + tnp1);

            // Boundary values at new time level
            double bcL = _pde.BoundaryLeft(tnp1, _grid.X(0));
            double bcR = _pde.BoundaryRight(tnp1, _grid.X(nx - 1));

            // Build tridiagonal system for interior points i = 1..nx-2
            double[] sub = new double[nInterior];   // a (sub-diagonal)
            double[] diag = new double[nInterior];  // b (main diagonal)
            double[] sup = new double[nInterior];   // c (super-diagonal)
            double[] rhs = new double[nInterior];   // d (right-hand side)

            double r = dt / (dx * dx);
            double s = dt / (2.0 * dx);

            for (int j = 0; j < nInterior; j++)
            {
                int idx = j + 1; // actual spatial index
                double xi = _grid.X(idx);

                // Average coefficients at tMid
                double aCoeff = _pde.Diffusion(tMid, xi);
                double bCoeff = _pde.Advection(tMid, xi);
                double cCoeff = _pde.Reaction(tMid, xi);
                double fCoeff = _pde.Source(tMid, xi);

                // Implicit side coefficients (left-hand side)
                double alphaImpl = -theta * (aCoeff * r - bCoeff * s);
                double betaImpl = 1.0 + theta * (2.0 * aCoeff * r - cCoeff * dt);
                double gammaImpl = -theta * (aCoeff * r + bCoeff * s);

                // Explicit side coefficients
                double alphaExpl = (1.0 - theta) * (aCoeff * r - bCoeff * s);
                double betaExpl = 1.0 - (1.0 - theta) * (2.0 * aCoeff * r - cCoeff * dt);
                double gammaExpl = (1.0 - theta) * (aCoeff * r + bCoeff * s);

                // RHS from explicit side
                rhs[j] = alphaExpl * u[idx - 1]
                        + betaExpl * u[idx]
                        + gammaExpl * u[idx + 1]
                        + dt * fCoeff;

                // Fill tridiagonal coefficients
                sub[j] = alphaImpl;
                diag[j] = betaImpl;
                sup[j] = gammaImpl;
            }

            // Modify RHS for Dirichlet boundary contributions
            // First interior point (j=0, i=1): subtract sub[0]*bcL from LHS
            rhs[0] -= sub[0] * bcL;
            sub[0] = 0.0;

            // Last interior point (j=nInterior-1, i=nx-2): subtract sup[last]*bcR
            rhs[nInterior - 1] -= sup[nInterior - 1] * bcR;
            sup[nInterior - 1] = 0.0;

            // Solve tridiagonal system
            TridiagonalSolver.Solve(sub, diag, sup, rhs);

            // Write solution back
            u[0] = bcL;
            for (int j = 0; j < nInterior; j++)
            {
                u[j + 1] = rhs[j];
            }
            u[nx - 1] = bcR;
        }

        return u;
    }
}
