using Xunit;

namespace PdeSolver.Tests;

public class Grid1DTests
{
    [Fact]
    public void BasicProperties()
    {
        var domain = new Domain1D(0.0, 1.0, 0.0, 1.0);
        var grid = new Grid1D(domain, 11, 100);

        Assert.Equal(11, grid.Nx);
        Assert.Equal(100, grid.Nt);
        Assert.Equal(0.1, grid.Dx, 12);
        Assert.Equal(0.01, grid.Dt, 12);
        Assert.Equal(0.0, grid.X(0), 12);
        Assert.Equal(1.0, grid.X(10), 12);
    }

    [Fact]
    public void TimeCoordinates()
    {
        var domain = new Domain1D(0.0, 1.0, 0.0, 1.0);
        var grid = new Grid1D(domain, 11, 10);

        Assert.Equal(0.0, grid.T(0), 12);
        Assert.Equal(0.5, grid.T(5), 12);
        Assert.Equal(1.0, grid.T(10), 12);
    }

    [Fact]
    public void Constructor_NxLessThan3_Throws()
    {
        var domain = new Domain1D(0.0, 1.0, 0.0, 1.0);
        Assert.Throws<ArgumentException>(() => new Grid1D(domain, 2, 100));
    }

    [Fact]
    public void Constructor_NtLessThan1_Throws()
    {
        var domain = new Domain1D(0.0, 1.0, 0.0, 1.0);
        Assert.Throws<ArgumentException>(() => new Grid1D(domain, 11, 0));
    }

    [Fact]
    public void Domain_ReturnsOriginalDomain()
    {
        var domain = new Domain1D(0.0, 2.0, 0.0, 0.5);
        var grid = new Grid1D(domain, 21, 50);

        Assert.Same(domain, grid.Domain);
    }
}
