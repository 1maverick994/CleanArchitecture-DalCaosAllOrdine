public class User{

    public void insertUser(){

        try{
            spinner.Show();

            string name = txtName.Text;
            string surname = txtSurname.Text;
            string email = txtEmail.Text;
            bool subscribeNewsletter = chbSubscribeNewsletter.Selected;
            //...other fields...

            if (string.IsNullOrWhiteSpace(name)) DisplayError("Name is required");
            if (string.IsNullOrWhiteSpace(surname)) DisplayError("Surname is required");
            // ...other fields validation...

            int id = database.Insert("INSERT INTO Users (Name, Surname, Email) VALUES @p1, @p2, @p3", name, surname, email);

            sendWelcomeMessage(email);

            if (subscribeNewsletter){
                var newsletterManager = new NewsletterManager();
                var newsletterSubscription = new NewsletterSubscription(type := "default", userId := id);
                newsletterManager.AddSubscription(newsletterSubscription);
            }

            // user document library configuration
            var documentLibraryClient = new DocumentLibraryClient();
            documentLibraryClient.Configure();
            //...ecc

        }
        catch (ex){
            throw;
        }
        finally{
            spinner.Hide();
        }
    }
}
