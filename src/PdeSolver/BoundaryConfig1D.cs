namespace PdeSolver;

/// <summary>
/// Stores the boundary condition types for the left and right ends of a 1D domain.
/// </summary>
public class BoundaryConfig1D
{
    /// <summary>Index constant for the left boundary.</summary>
    public const int Left = 0;

    /// <summary>Index constant for the right boundary.</summary>
    public const int Right = 1;

    /// <summary>
    /// Gets the boundary types array. Index 0 is the left boundary, index 1 is the right boundary.
    /// Defaults to Dirichlet on both sides.
    /// </summary>
    public BoundaryType[] Types { get; } = [BoundaryType.Dirichlet, BoundaryType.Dirichlet];
}
