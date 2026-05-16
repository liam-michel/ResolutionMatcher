using ResolutionMatcher.Display;

namespace ResolutionMatcher.Tests;

public class DisplayServiceTests
{
  [Fact]
  [Trait("Category", "Unit")]

  public void GetCurrentResolution_Returns_PrimaryScreenResolution()
  {
    // Arrange
    var service = new DisplayService();
    var res = service.GetCurrentResolution();
    Assert.True(res.Width > 0);
    Assert.True(res.Height > 0);
  }
  [Fact]
  [Trait("Category", "Unit")]

  public void SetResolution_Returns_False_For_InvalidResolution()
  {
    // Arrange
    var service = new DisplayService();
    // Act
    var result = service.SetResolution(-1, -1);
    // Assert
    Assert.False(result);
  }

  [Fact]
  [Trait("Category", "Unit")]

  public void SetResolution_Returns_True_For_ValidResolution()
  {
    // Arrange
    var service = new DisplayService();
    var currentRes = service.GetCurrentResolution();
    // Act
    var result = service.SetResolution(currentRes.Width, currentRes.Height);
    // Assert
    Assert.True(result);
  }
}