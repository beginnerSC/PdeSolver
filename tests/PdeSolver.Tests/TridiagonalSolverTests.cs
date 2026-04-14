using Xunit;

namespace PdeSolver.Tests;

public class TridiagonalSolverTests
{
    [Fact]
    public void SimpleSystem_2x2()
    {
        // [[2,1],[1,3]] * x = [5,11] -> x = [0.8, 3.4]
        double[] a = [0.0, 1.0];
        double[] b = [2.0, 3.0];
        double[] c = [1.0, 0.0];
        double[] d = [5.0, 11.0];

        TridiagonalSolver.Solve(a, b, c, d);

        Assert.Equal(0.8, d[0], 12);
        Assert.Equal(3.4, d[1], 12);
    }

    [Fact]
    public void IdentitySystem()
    {
        // Identity matrix: solution equals RHS
        int n = 5;
        double[] a = new double[n];
        double[] b = Enumerable.Repeat(1.0, n).ToArray();
        double[] c = new double[n];
        double[] d = [1.0, 2.0, 3.0, 4.0, 5.0];

        TridiagonalSolver.Solve(a, b, c, d);

        Assert.Equal(1.0, d[0], 12);
        Assert.Equal(2.0, d[1], 12);
        Assert.Equal(3.0, d[2], 12);
        Assert.Equal(4.0, d[3], 12);
        Assert.Equal(5.0, d[4], 12);
    }

    [Fact]
    public void TridiagonalSystem_3x3()
    {
        // [[2,-1,0],[-1,2,-1],[0,-1,2]] * x = [1,0,1] -> x = [1,1,1]
        double[] a = [0.0, -1.0, -1.0];
        double[] b = [2.0, 2.0, 2.0];
        double[] c = [-1.0, -1.0, 0.0];
        double[] d = [1.0, 0.0, 1.0];

        TridiagonalSolver.Solve(a, b, c, d);

        Assert.Equal(1.0, d[0], 12);
        Assert.Equal(1.0, d[1], 12);
        Assert.Equal(1.0, d[2], 12);
    }

    [Fact]
    public void EmptySystem_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            TridiagonalSolver.Solve([], [], [], []));
    }
}
