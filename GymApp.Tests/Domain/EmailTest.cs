using GymApp.Domain.Users;
using GymApp.Domain.Users.Exceptions;
using Xunit;

namespace GymApp.Tests.Domain;

public class EmailTests
{
    private const string ValidEmail = "john.doe@test.com";

    [Fact]
    public void Constructor_WithValidEmail_ShouldCreateEmail()
    {
       
        var email = new Email(ValidEmail);
        
        Assert.Equal(ValidEmail, email.Value);
    }

    [Fact]
    public void Constructor_WithBlankEmail_ShouldThrowInvalidEmailException()
    {
        const string blankEmail = "";
       
        var act = () => new Email(blankEmail);
        
        Assert.Throws<InvalidEmailException>(act);
    }

    [Fact]
    public void Constructor_WithEmailWithoutAtSymbol_ShouldThrowInvalidEmailException()
    {
        
        const string invalidEmail = "john.doetest.com";
        
        var act = () => new Email(invalidEmail);
        
        Assert.Throws<InvalidEmailException>(act);
    }

    [Fact]
    public void Constructor_WithEmailWithoutDomainName_ShouldThrowInvalidEmailException()
    {
        const string invalidEmail = "john.doe@.com";
        
        var act = () => new Email(invalidEmail);
        
        Assert.Throws<InvalidEmailException>(act);
    }

    [Fact]
    public void Constructor_WithEmailWithoutDomainExtension_ShouldThrowInvalidEmailException()
    {
        const string invalidEmail = "john.doe@test";
        
        var act = () => new Email(invalidEmail);
        
        Assert.Throws<InvalidEmailException>(act);
    }

    [Fact]
    public void Constructor_WithEmailContainingSpace_ShouldThrowInvalidEmailException()
    {
        const string invalidEmail = "john doe@test.com";
        
        var act = () => new Email(invalidEmail);

        
        Assert.Throws<InvalidEmailException>(act);
    }

    [Fact]
    public void Constructor_WithEmailContainingTwoAtSymbols_ShouldThrowInvalidEmailException()
    {
        const string invalidEmail = "john.doe@@test.com";
        
        var act = () => new Email(invalidEmail);
        
        Assert.Throws<InvalidEmailException>(act);
    }

    [Fact]
    public void Constructor_WithUppercaseEmail_ShouldConvertEmailToLowercase()
    {
       
        const string uppercaseEmail = "John.Doe@TEST.COM";
        const string expectedEmail = "john.doe@test.com";
        
        var email = new Email(uppercaseEmail);
        
        Assert.Equal(expectedEmail, email.Value);
    }

    [Fact]
    public void ToString_ShouldReturnEmailValue()
    {
       
        var email = new Email(ValidEmail);

       
        var result = email.ToString();
        
        Assert.Equal(ValidEmail, result);
    }
}