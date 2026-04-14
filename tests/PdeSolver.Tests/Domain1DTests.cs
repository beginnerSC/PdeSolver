using Xunit;

namespace PdeSolver.Tests;

public class Domain1DTests
{
    [Fact]
    public void Constructor_ValidParameters_SetsProperties()
    {
        var domain = new Domain1D(0.0, 1.0, 0.0, 2.0);

        Assert.Equal(0.0, domain.XLeft);
        Assert.Equal(1.0, domain.XRight);
        Assert.Equal(0.0, domain.T0);
        Assert.Equal(2.0, domain.T1);
    }

    [Fact]
    public void LengthX_ReturnsCorrectValue()
    {
        var domain = new Domain1D(0.5, 2.5, 0.0, 1.0);
        Assert.Equal(2.0, domain.LengthX);
    }

    [Fact]
    public void LengthT_ReturnsCorrectValue()
    {
        var domain = new Domain1D(0.0, 1.0, 0.5, 3.5);
        Assert.Equal(3.0, domain.LengthT);
    }

    [Fact]
    public void Constructor_XRightLessThanOrEqualXLeft_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Domain1D(1.0, 0.0, 0.0, 1.0));
        Assert.Throws<ArgumentException>(() => new Domain1D(1.0, 1.0, 0.0, 1.0));
    }

    [Fact]
    public void Constructor_T1LessThanOrEqualT0_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Domain1D(0.0, 1.0, 1.0, 0.0));
        Assert.Throws<ArgumentException>(() => new Domain1D(0.0, 1.0, 1.0, 1.0));
    }
}
