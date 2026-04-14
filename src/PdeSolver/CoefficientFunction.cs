namespace PdeSolver;

/// <summary>
/// Represents a coefficient function f(t, x) -> double used in PDE definitions.
/// </summary>
/// <param name="t">The time variable.</param>
/// <param name="x">The spatial variable.</param>
/// <returns>The function value at (t, x).</returns>
public delegate double CoefficientFunction(double t, double x);
