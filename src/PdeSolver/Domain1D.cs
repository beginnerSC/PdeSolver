namespace PdeSolver;

/// <summary>
/// Defines a 1D spatial-temporal domain [xL, xR] × [t0, t1].
/// </summary>
public class Domain1D
{
    /// <summary>Gets the left spatial boundary.</summary>
    public double XLeft { get; }

    /// <summary>Gets the right spatial boundary.</summary>
    public double XRight { get; }

    /// <summary>Gets the initial time.</summary>
    public double T0 { get; }

    /// <summary>Gets the final time.</summary>
    public double T1 { get; }

    /// <summary>
    /// Initializes a new <see cref="Domain1D"/> with the specified spatial and temporal bounds.
    /// </summary>
    /// <param name="xLeft">Left spatial boundary.</param>
    /// <param name="xRight">Right spatial boundary. Must be greater than <paramref name="xLeft"/>.</param>
    /// <param name="t0">Initial time.</param>
    /// <param name="t1">Final time. Must be greater than <paramref name="t0"/>.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="xRight"/> ≤ <paramref name="xLeft"/> or <paramref name="t1"/> ≤ <paramref name="t0"/>.
    /// </exception>
    public Domain1D(double xLeft, double xRight, double t0, double t1)
    {
        if (xRight <= xLeft)
            throw new ArgumentException("xRight must be greater than xLeft.", nameof(xRight));
        if (t1 <= t0)
            throw new ArgumentException("t1 must be greater than t0.", nameof(t1));

        XLeft = xLeft;
        XRight = xRight;
        T0 = t0;
        T1 = t1;
    }

    /// <summary>Gets the length of the spatial domain (xRight − xLeft).</summary>
    public double LengthX => XRight - XLeft;

    /// <summary>Gets the length of the temporal domain (t1 − t0).</summary>
    public double LengthT => T1 - T0;
}
