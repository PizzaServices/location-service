using Location_Service.Repositories.Locations.Validations;

namespace Location_Service_Tests.Repositories.Locations.Validations;

public class LocationRuleSetTest
{
    [Test]
    [TestCase(-90d)]
    [TestCase(90d)]
    public void ShouldReturnSuccessIfLatitudeIsValid(double latitude)
    {
        // Arrange
        var ruleSet = GetRuleSet();
        
        // Act
        var response = ruleSet.ValidateLatitude(latitude);
        
        // Assert
        Assert.That(response.Successful, Is.True);
        Assert.That(response.ErrorMessage, Is.Null);
    }
    
    [Test]
    [TestCase(-90.0000001d)]
    [TestCase(90.00000001d)]
    public void ShouldReturnNotSuccessIfLatitudeIsNotValid(double latitude)
    {
        // Arrange
        var ruleSet = GetRuleSet();
        
        // Act
        var response = ruleSet.ValidateLatitude(latitude);
        
        // Assert
        Assert.That(response.Successful, Is.False);
        Assert.That(response.ErrorMessage, Is.EqualTo("Latitude is not valid"));
    }
    
    [Test]
    [TestCase(-180d)]
    [TestCase(180d)]
    public void ShouldReturnSuccessIfLongitudeIsValid(double longitude)
    {
        // Arrange
        var ruleSet = GetRuleSet();
        
        // Act
        var response = ruleSet.ValidateLongitude(longitude);
        
        // Assert
        Assert.That(response.Successful, Is.True);
        Assert.That(response.ErrorMessage, Is.Null);
    }
    
    [Test]
    [TestCase(-180.0000001d)]
    [TestCase(180.00000001d)]
    public void ShouldReturnNotSuccessIfLongitudeIsNotValid(double longitude)
    {
        // Arrange
        var ruleSet = GetRuleSet();
        
        // Act
        var response = ruleSet.ValidateLongitude(longitude);
        
        // Assert
        Assert.That(response.Successful, Is.False);
        Assert.That(response.ErrorMessage, Is.EqualTo("Longitude is not valid"));
    }

    private LocationRuleSet GetRuleSet()
    {
        return new LocationRuleSet();
    }
}