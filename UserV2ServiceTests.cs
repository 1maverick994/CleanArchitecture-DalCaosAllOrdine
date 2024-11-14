using Moq;
using Xunit;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<INewsletterService> _newsletterServiceMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        _newsletterServiceMock = new Mock<INewsletterService>();
        _userService = new UserService(_userRepositoryMock.Object, _emailServiceMock.Object, _newsletterServiceMock.Object);
    }

    [Fact]
    public void InsertUser_ValidUser_InsertsUserAndSendsEmail()
    {
        // Arrange
        var user = new User("John", "Doe", "john.doe@example.com");
        _userRepositoryMock.Setup(repo => repo.Insert(user)).Returns(1);

        // Act
        var userId = _userService.InsertUser(user);

        // Assert
        Assert.Equal(1, userId);
        _userRepositoryMock.Verify(repo => repo.Insert(user), Times.Once);
        _emailServiceMock.Verify(service => service.SendWelcomeMessage(user.Email), Times.Once);
        _newsletterServiceMock.Verify(service => service.AddUserToNewsletter(userId), Times.Once);
    }

    [Fact]
    public void InsertUser_MissingName_ThrowsArgumentException()
    {
        // Arrange
        var user = new User("", "Doe", "john.doe@example.com");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _userService.InsertUser(user));
        Assert.Equal("Name is required", exception.Message);
    }

    [Fact]
    public void InsertUser_MissingSurname_ThrowsArgumentException()
    {
        // Arrange
        var user = new User("John", "", "john.doe@example.com");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _userService.InsertUser(user));
        Assert.Equal("Surname is required", exception.Message);
    }
}
