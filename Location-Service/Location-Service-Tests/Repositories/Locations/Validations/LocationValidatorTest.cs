using Location_Service.Repositories.Locations.Contracts;
using Location_Service.Repositories.Locations.Validations;
using Location_Service.Repositories.Validations;

namespace Location_Service_Tests.Repositories.Locations.Validations;

public class LocationValidatorTest
{
    private Mock<ILocationRuleSet> _locationRuleSetMock = null!;
    private RuleSetValidationResponse _ruleSetValidationResponse = null!;
    
    [SetUp]
    public void SetUp()
    {
        _ruleSetValidationResponse = new RuleSetValidationResponse
        {
            Successful = true,
        };
    }

    [Test]
    public void ShouldCallRuleSetOnValidateCreation()
    {
        // Arrange
        var validator = GetValidator();
        
        // Act
        var response = validator.ValidateCreation( new CreateLocationRequest
        {
            Latitude = 50d,
            Longitude = 60d,
        });
        
        // Assert
        Assert.That(response.Successful, Is.True);
        Assert.That(response.ErrorMessages.Any, Is.False);
        
        _locationRuleSetMock.Verify(set => set.ValidateLatitude(50d), Times.AtLeastOnce);
        _locationRuleSetMock.Verify(set => set.ValidateLongitude(60d), Times.AtLeastOnce);
    }
    
    [Test]
    public void ShouldReturnErrorMessagesOnValidateCreation()
    {
        // Arrange
        _ruleSetValidationResponse.Successful = false;
        _ruleSetValidationResponse.ErrorMessage = "TestErrorMessage";
        var validator = GetValidator();
        
        // Act
        var response = validator.ValidateCreation(new CreateLocationRequest
        {
            Latitude = 50d,
            Longitude = 60d,
        });
        
        // Assert
        Assert.That(response.Successful, Is.False);
        
        Assert.That(response.ErrorMessages.Count, Is.EqualTo(2));
        Assert.That(response.ErrorMessages[0], Is.EqualTo("TestErrorMessage"));
        Assert.That(response.ErrorMessages[1], Is.EqualTo("TestErrorMessage"));
        
        _locationRuleSetMock.Verify(set => set.ValidateLatitude(50d), Times.AtLeastOnce);
        _locationRuleSetMock.Verify(set => set.ValidateLongitude(60d), Times.AtLeastOnce);
    }
    
    private LocationValidator GetValidator()
    {
        _locationRuleSetMock = new Mock<ILocationRuleSet>();

        _locationRuleSetMock
            .Setup(set => set.ValidateLatitude(It.IsAny<double>()))
            .Returns(_ruleSetValidationResponse);

        _locationRuleSetMock
            .Setup(set => set.ValidateLongitude(It.IsAny<double>()))
            .Returns(_ruleSetValidationResponse);

        return new LocationValidator(_locationRuleSetMock.Object);
    }
}