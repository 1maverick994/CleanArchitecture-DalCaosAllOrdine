//Entity
public class User {
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Email { get; private set; } 
}

// Use case
public class UserService{   

    public UserService(IUserRepository userRepository, IEmailService emailService, INewsletterService newsletterService) {
       // ...
    }

    public int InsertUser(User user) {
        if (string.IsNullOrWhiteSpace(user.Name)) throw new ArgumentException("Name is required");
        if (string.IsNullOrWhiteSpace(user.Surname)) throw new ArgumentException("Surname is required");

        int id = _userRepository.Insert(user);

        _emailService.SendWelcomeMessage(user.Email);
        _newsletterService.AddUserToNewsletter(id);

        return id;
    }
    
}

// Repository
public class UserRepository : IUserRepository {

    public int Insert(User user) {
        // Implement the database insert logic
        return _database.Insert("INSERT INTO Users (Name, Surname, Email) VALUES (@p1, @p2, @p3)", user.Name, user.Surname, user.Email);
    }
}

// Controller/presenter
public class UserController {   

    public void InsertUser()
    {
        try
        {
            spinner.Show();

            string name = txtName.Text;
            string surname = txtSurname.Text;
            string email = txtEmail.Text;

            User user = new User(name, surname, email);

            _userService.InsertUser(user);
        }
        catch (Exception ex)
        {
            DisplayError(ex.Message);
        }
        finally
        {
            spinner.Hide();
        }
    }
}

