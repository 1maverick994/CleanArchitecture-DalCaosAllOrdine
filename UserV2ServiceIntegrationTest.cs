using Microsoft.EntityFrameworkCore;
using Xunit;

public class UserServiceIntegrationTests
{
    private ApplicationDbContext _context;
    private UserService _userService;

    public UserServiceIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);

        var userRepository = new UserRepository(_context);
        var emailService = new EmailService(); // Mock or real implementation
        var newsletterService = new NewsletterService(); // Mock or real implementation

        _userService = new UserService(userRepository, emailService, newsletterService);
    }

    [Fact]
    public void InsertUser_ValidUser_InsertsUserIntoDatabase()
    {
        // Arrange
        var user = new User("John", "Doe", "john.doe@example.com");

        // Act
        var userId = _userService.InsertUser(user, false);

        // Assert
        var savedUser = _context.Users.Find(userId);
        Assert.NotNull(savedUser);
        Assert.Equal("John", savedUser.Name);
        Assert.Equal("Doe", savedUser.Surname);
        Assert.Equal("john.doe@example.com", savedUser.Email);
    }

    [Fact]
    public void InsertUser_DuplicateEmail_ThrowsException()
    {
        // Arrange
        var user1 = new User("John", "Doe", "john.doe@example.com");
        var user2 = new User("Jane", "Doe", "john.doe@example.com");
        _userService.InsertUser(user1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _userService.InsertUser(user2));
        Assert.Equal("Email must be unique", exception.Message);
    }
}
