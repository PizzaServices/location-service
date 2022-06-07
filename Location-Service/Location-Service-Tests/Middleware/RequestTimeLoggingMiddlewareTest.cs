using Location_Service.Middleware;
using Microsoft.AspNetCore.Http;

namespace Location_Service_Tests.Middleware;

public class RequestTimeLoggingMiddlewareTest
{
    private Mock<HttpContext> _httpContextMock = new();
    private Mock<RequestDelegate> _requestDelegate = new();

    [SetUp]
    public void SetUp()
    {
        _requestDelegate
            .Setup(_ => _(It.IsAny<HttpContext>()))
            .Returns(Task.CompletedTask);
    }

    [Test]
    public async Task ShouldCallNext()
    {
        // Arrange
        var middleware = GetMiddleware();
        
        // Act
        await middleware.InvokeAsync(_httpContextMock.Object, _requestDelegate.Object);
        
        // Assert
        _requestDelegate.Verify(_ => _(_httpContextMock.Object), Times.Once);
        _requestDelegate.VerifyNoOtherCalls();
    }
    
    private static RequestTimeLoggingMiddleware GetMiddleware()
    {
        return new RequestTimeLoggingMiddleware();
    }
}