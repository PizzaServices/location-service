using Location_Service.Extensions;
using Location_Service.Repositories.Validations;

namespace Location_Service_Tests.Extensions;

public class ListExtensionsTest
{
    [Test]
    public void ShouldAddRuleSetValidationResponseToList()
    {
        // Arrange
        const string errorMessage = "ErrorMessage";
        
        IList<string> list = new List<string>();
        
        // Act
        list.AddRuleSetValidationResponseIfNotSuccessful(new RuleSetValidationResponse
        {
            Successful = false,
            ErrorMessage = errorMessage,
        });
        
        // Assert
        Assert.That(list, Has.Count.EqualTo(1));
        Assert.That(list[0], Is.EqualTo(errorMessage));
    }

    [Test]
    public void ShouldNotAddRuleSetValidationResponseToList()
    {
        // Arrange
        IList<string> list = new List<string>();
        
        // Act
        list.AddRuleSetValidationResponseIfNotSuccessful(new RuleSetValidationResponse
        {
            Successful = true,
            ErrorMessage = "This Message will disappear",
        });
        
        // Assert
        Assert.That(list, Has.Count.EqualTo(0));
    }
}