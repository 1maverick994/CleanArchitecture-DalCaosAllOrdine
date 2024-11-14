using Moq;
using Xunit;

public class UserControllerTests
{
    private readonly Mock<UserService> _userServiceMock;
    private readonly UserController _userController;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<UserService>();
        _userController = new UserController(_userServiceMock.Object);
    }

    [Fact]
    public void InsertUser_ValidInput_CallsUserService()
    {
        // Arrange
        var txtName = "John";
        var txtSurname = "Doe";
        var txtEmail = "john.doe@example.com";

        // Act
        _userController.InsertUser();

        // Assert
        _userServiceMock.Verify(service => service.InsertUser(It.Is<User>(u => u.Name == txtName && u.Surname == txtSurname && u.Email == txtEmail)), Times.Once);
    }

    [Fact]
    public void InsertUser_MissingName_DisplaysError()
    {
        // Arrange
        var txtName = ""; // Missing name
        var txtSurname = "Doe";
        var txtEmail = "john.doe@example.com";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _userController.InsertUser());
        Assert.Equal("Name is required", exception.Message);
    }
}
