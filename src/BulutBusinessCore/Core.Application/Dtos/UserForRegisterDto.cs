namespace BulutBusinessCore.Core.Application.Dtos;
public class UserForRegisterDto : IDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public string Password { get; set; }

    public UserForRegisterDto()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        PhoneNumber = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
    }

    public UserForRegisterDto(string firstName, string lastName, string phoneNumber, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        Password = password;
    }
}