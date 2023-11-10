using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.ComponentModel.DataAnnotations;
using ManeroBackend.Models.DTOs;
using System.Collections.Generic;

namespace ManeroBackend.Tests.UnitTests
{
    public class UserRegistrationDto_Tests
    {
        [Fact]
        public void UserRegistrationDto_ValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new UserRegistrationDto
            {
                Email = "test@example.com",
                Password = "Password!2",
                ConfirmPassword = "StrongPassword!8",
                FirstName = "Linn",
                LastName = "Rodin"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("", "Password", "Password")] // Missing email
        [InlineData("test@example.com", "", "")] // Missing password and confirmation
        [InlineData("test@example.com", "Password", "Mismatch")] // Non-matching passwords
        public void UserRegistrationDto_InvalidData_ShouldFailValidation(string email, string password, string confirmPassword)
        {
            // Arrange
            var dto = new UserRegistrationDto
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword,
                FirstName = "John",
                LastName = "Doe"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.False(isValid);
        }
    }
}

