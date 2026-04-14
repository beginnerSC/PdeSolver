namespace PdeSolver;

/// <summary>
/// Represents a uniform 1D computational grid over a <see cref="Domain1D"/>.
/// The grid has <c>Nx</c> spatial points and <c>Nt</c> time steps.
/// </summary>
public class Grid1D
{
    /// <summary>Gets the underlying domain.</summary>
    public Domain1D Domain { get; }

    /// <summary>Gets the number of spatial grid points (including boundaries).</summary>
    public int Nx { get; }

    /// <summary>Gets the number of time steps.</summary>
    public int Nt { get; }

    /// <summary>Gets the spatial step size.</summary>
    public double Dx { get; }

    /// <summary>Gets the temporal step size.</summary>
    public double Dt { get; }

    /// <summary>
    /// Initializes a new <see cref="Grid1D"/> over the given domain.
    /// </summary>
    /// <param name="domain">The spatial-temporal domain.</param>
    /// <param name="nx">Number of spatial grid points (must be ≥ 3).</param>
    /// <param name="nt">Number of time steps (must be ≥ 1).</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="nx"/> &lt; 3 or <paramref name="nt"/> &lt; 1.</exception>
    public Grid1D(Domain1D domain, int nx, int nt)
    {
        if (nx < 3)
            throw new ArgumentException("nx must be >= 3.", nameof(nx));
        if (nt < 1)
            throw new ArgumentException("nt must be >= 1.", nameof(nt));

        Domain = domain;
        Nx = nx;
        Nt = nt;
        Dx = domain.LengthX / (nx - 1);
        Dt = domain.LengthT / nt;
    }

    /// <summary>
    /// Returns the spatial coordinate of the <paramref name="i"/>-th grid point.
    /// </summary>
    /// <param name="i">Spatial index (0-based).</param>
    public double X(int i) => Domain.XLeft + i * Dx;

    /// <summary>
    /// Returns the time at the <paramref name="k"/>-th time step.
    /// </summary>
    /// <param name="k">Time step index (0-based).</param>
    public double T(int k) => Domain.T0 + k * Dt;
}
