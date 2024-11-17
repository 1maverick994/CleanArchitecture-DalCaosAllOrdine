//Entity
public class User
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Email { get; private set; }
}

// Use case
public class UserService
{

    public UserService(IUserRepository userRepository, IEmailService emailService, INewsletterService newsletterService, IDocumentLibraryService documentLibraryService)
    {
        // ...
    }

    public int InsertUser(User user, bool subscribeNewsletter)
    {
        if (string.IsNullOrWhiteSpace(user.Name)) throw new ArgumentException("Name is required");
        if (string.IsNullOrWhiteSpace(user.Surname)) throw new ArgumentException("Surname is required");

        int id = _userRepository.Insert(user);

        _emailService.SendWelcomeMessage(user.Email);
        if (subscribeNewsletter)
            _newsletterService.AddUserToNewsletter(id);
        _documentLibraryService.Configure();

        return id;
    }

}

// Repository
public class UserRepository : IUserRepository
{

    public int Insert(User user)
    {
        // Implement the database insert logic
        return _database.Insert("INSERT INTO Users (Name, Surname, Email) VALUES (@p1, @p2, @p3)", user.Name, user.Surname, user.Email);
    }
}

// Controller
public class UserController{

    public void InsertUser(HttpRequest httpRequest){

        if (httpRequest.Body == null) ReturnError("Empty body");
        if (IsValidJson(httpRequest.Body) == false) ReturnError("Body is not a valid json");
        // other request validation

        string name = jsonRequest.GetFieldValue("name");
        string surname = jsonRequest.GetFieldValue("surname");
        string email = jsonRequest.GetFieldValue("email");
        bool subscribeNewsletter = jsonRequest.GetFieldValue("subscribeNewsletter");
        //...other fields...

        User user = new User(name, surname, email);

        var userId = _userService.InsertUser(user, subscribeNewsletter);

        var response = new HttpResponse(StatusCode.Ok, new { UserId = userId});
        return response;    
    }
}

