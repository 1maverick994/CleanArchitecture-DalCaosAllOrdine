using Moq;
using Xunit;

public class UserRepositoryTests
{
    private readonly Mock<IDatabase> _databaseMock;
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        _databaseMock = new Mock<IDatabase>();
        _userRepository = new UserRepository(_databaseMock.Object);
    }

    [Fact]
    public void Insert_ValidUser_InsertsUserAndReturnsId()
    {
        // Arrange
        var user = new User("John", "Doe", "john.doe@example.com");
        _databaseMock.Setup(db => db.Insert(It.IsAny<string>(), user.Name, user.Surname, user.Email)).Returns(1);

        // Act
        var userId = _userRepository.Insert(user);

        // Assert
        Assert.Equal(1, userId);
        _databaseMock.Verify(db => db.Insert("INSERT INTO Users (Name, Surname, Email) VALUES (@p1, @p2, @p3)", user.Name, user.Surname, user.Email), Times.Once);
    }
}
