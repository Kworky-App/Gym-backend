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
}