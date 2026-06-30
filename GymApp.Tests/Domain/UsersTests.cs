using GymApp.Domain.Users;
using GymApp.Domain.Users.Exceptions;
using Xunit;

namespace GymApp.Tests.Domain;

public class UsersTests
{
   private const string ValidName = "John doe";
   private const string ValidPasswordHash = "hashed-password";

   private static readonly DateOnly ValidDateOfBirth = new(2000, 1, 1);
   private static readonly Email ValidEmail = new("john.doe@test.com");
   
   private const Gender ValidGender = Gender.Male;

   private static User CreateValidUser()
   {
      return new User(ValidName, ValidDateOfBirth, ValidEmail, ValidGender, ValidPasswordHash);
   }

   [Fact]
   public void Constructor_WithValidValues_ShouldCreateUser()
   {
      var user = CreateValidUser();
      Assert.Equal(ValidName, user.Name);
      Assert.Equal(ValidDateOfBirth, user.DateOfBirth);
      Assert.Equal(ValidEmail, user.Email);
      Assert.Equal(ValidGender, user.Gender);
      Assert.Equal(ValidPasswordHash, user.PasswordHash);
   }

   [Fact]
   public void Constructor_WithBlankName_ShouldThrowInvalidUserException()
   {
      const string invalidName = "";
      
      var blankUsername = () => new User(invalidName, ValidDateOfBirth, ValidEmail, ValidGender, ValidPasswordHash);

      Assert.Throws<InvalidUserException>(blankUsername);
   }
   
   [Fact]
   public void Constructor_WithNullEmail_ShouldThrowInvalidUserException()
   {
     
      var act = () => new User(
         ValidName,
         ValidDateOfBirth,
         null!,
         ValidGender,
         ValidPasswordHash);
      
      Assert.Throws<InvalidUserException>(act);
   }
   [Fact]
   public void Constructor_WithBlankPasswordHash_ShouldThrowInvalidUserException()
   {
      const string invalidPasswordHash = "";
      
      var act = () => new User(
         ValidName,
         ValidDateOfBirth,
         ValidEmail,
         ValidGender,
         invalidPasswordHash);
      
      Assert.Throws<InvalidUserException>(act);
   }
   [Fact]
   public void Constructor_ShouldGenerateId()
   {
      
      var user = CreateValidUser();
      
      Assert.NotEqual(Guid.Empty, user.Id);
   }
 
   [Fact]
   public void Constructor_ShouldSetCreatedAtToCurrentUtcTime()
   {
     
      var beforeCreation = DateTime.UtcNow;

    
      var user = CreateValidUser();

      var afterCreation = DateTime.UtcNow;

      
      Assert.InRange(
         user.CreatedAt,
         beforeCreation,
         afterCreation);
   }
}