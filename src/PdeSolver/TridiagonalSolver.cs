namespace PdeSolver;

/// <summary>
/// Solves tridiagonal linear systems Ax = d using the Thomas algorithm.
/// </summary>
public static class TridiagonalSolver
{
    /// <summary>
    /// Solves the tridiagonal system in-place. After the call, <paramref name="d"/>
    /// contains the solution vector.
    /// </summary>
    /// <param name="a">Sub-diagonal coefficients (size n). Index 0 is unused; indices 1..n-1 are the sub-diagonal.</param>
    /// <param name="b">Main diagonal coefficients (size n).</param>
    /// <param name="c">Super-diagonal coefficients (size n). Index n-1 is unused; indices 0..n-2 are the super-diagonal.</param>
    /// <param name="d">Right-hand side vector (size n). Overwritten with the solution.</param>
    /// <exception cref="ArgumentException">Thrown when the system is empty.</exception>
    public static void Solve(double[] a, double[] b, double[] c, double[] d)
    {
        int n = b.Length;
        if (n == 0)
            throw new ArgumentException("Cannot solve an empty tridiagonal system.");

        // Forward sweep
        for (int i = 1; i < n; i++)
        {
            double m = a[i] / b[i - 1];
            b[i] -= m * c[i - 1];
            d[i] -= m * d[i - 1];
        }

        // Back substitution
        d[n - 1] /= b[n - 1];
        for (int i = n - 2; i >= 0; i--)
        {
            d[i] = (d[i] - c[i] * d[i + 1]) / b[i];
        }
    }
}
