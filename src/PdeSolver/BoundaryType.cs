namespace PdeSolver;

/// <summary>
/// Specifies the type of boundary condition applied at a domain boundary.
/// </summary>
public enum BoundaryType
{
    /// <summary>Specifies the value of the solution at the boundary: u = g.</summary>
    Dirichlet,

    /// <summary>Specifies the first derivative at the boundary: du/dx = g.</summary>
    Neumann,

    /// <summary>Specifies the second derivative at the boundary: d²u/dx² = g.</summary>
    Gamma
}
