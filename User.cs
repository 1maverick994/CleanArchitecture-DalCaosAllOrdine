public class User{

    public void insertUser(){

        try{
            spinner.Show();

            string name = txtName.Text;
            string surname = txtSurname.Text;
            string email = txtEmail.Text;

            if(string.IsNullOrWhiteSpace(name)) DisplayError("Name is required");
            if(string.IsNullOrWhiteSpace(surname)) DisplayError("Surname is required");
            
            int id = database.Insert("INSERT INTO Users (Name, Surname, Email) VALUES @p1, @p2, @p3", name, surname, email);

            sendWelcomeMessage(email);
            addUserToNewsletter(id);

        }
        catch(ex){
            throw;
        }
        finally{
            spinner.Hide();
        }
    }
}
