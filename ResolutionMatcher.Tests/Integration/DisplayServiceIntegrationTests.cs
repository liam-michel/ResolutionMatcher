using ResolutionMatcher.Display;

namespace ResolutionMatcher.Tests;

public class DisplayServiceIntegrationTests
{
  [Fact(Skip = "Integration test - requires display settings modification")]
  [Trait("Category", "Integration")]
  public void SetResolution_Changes_DisplayResolution()
  {
    // Arrange
    var service = new DisplayService();
    var originalRes = service.GetCurrentResolution();
    var newWidth = 1920;
    var newHeight = 1080;


    try
    {
      
    // Act
    var result = service.SetResolution(newWidth, newHeight);
    Thread.Sleep(500);
      Console.WriteLine($"Attempted to set resolution to {newWidth}x{newHeight}, result: {result}");
      // Assert
      Assert.True(result, "SetResolution should return true for valid resolution change.");
      var currentRes = service.GetCurrentResolution();
      Console.WriteLine($"Current resolution after change: {currentRes.Width}x{currentRes.Height}");
      Assert.Equal(newWidth, currentRes.Width);
      Assert.Equal(newHeight, currentRes.Height);
    }
    finally
    {
      //cleanup
      var resetResult = service.SetResolution(originalRes.Width, originalRes.Height);
      Console.WriteLine($"Resetting resolution to original {originalRes.Width}x{originalRes.Height}, result: {resetResult}");
      Assert.True(resetResult, "Failed to reset resolution to original settings.");
    }
    



  }
}