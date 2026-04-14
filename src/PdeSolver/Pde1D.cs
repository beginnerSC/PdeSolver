namespace PdeSolver;

/// <summary>
/// Defines a 1D parabolic PDE of the form:
/// <code>u_t = a(t,x) u_xx + b(t,x) u_x + c(t,x) u + f(t,x)</code>
/// along with initial and boundary conditions.
/// </summary>
public class Pde1D
{
    /// <summary>
    /// Gets or sets the diffusion coefficient a(t, x). Default: 1.0.
    /// </summary>
    public CoefficientFunction Diffusion { get; set; } = (t, x) => 1.0;

    /// <summary>
    /// Gets or sets the advection coefficient b(t, x). Default: 0.0.
    /// </summary>
    public CoefficientFunction Advection { get; set; } = (t, x) => 0.0;

    /// <summary>
    /// Gets or sets the reaction coefficient c(t, x). Default: 0.0.
    /// </summary>
    public CoefficientFunction Reaction { get; set; } = (t, x) => 0.0;

    /// <summary>
    /// Gets or sets the source term f(t, x). Default: 0.0.
    /// </summary>
    public CoefficientFunction Source { get; set; } = (t, x) => 0.0;

    /// <summary>
    /// Gets or sets the initial condition u(t0, x). Default: 0.0.
    /// </summary>
    public CoefficientFunction Initial { get; set; } = (t, x) => 0.0;

    /// <summary>
    /// Gets or sets the left boundary condition value. Default: 0.0.
    /// </summary>
    public CoefficientFunction BoundaryLeft { get; set; } = (t, x) => 0.0;

    /// <summary>
    /// Gets or sets the right boundary condition value. Default: 0.0.
    /// </summary>
    public CoefficientFunction BoundaryRight { get; set; } = (t, x) => 0.0;

    /// <summary>
    /// Gets or sets the boundary condition configuration. Default: Dirichlet on both sides.
    /// </summary>
    public BoundaryConfig1D BoundaryConfig { get; set; } = new();
}
