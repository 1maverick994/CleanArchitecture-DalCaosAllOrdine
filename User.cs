public class User{

    public HttpResponse InsertUser(HttpRequest httpRequest){

        try{
            if(httpRequest.Body == null) ReturnError("Empty body");
            if(IsValidJson(httpRequest.Body) == false) ReturnError("Body is not a valid json");
            // other request validation

            string name = jsonRequest.GetFieldValue("name");
            string surname = jsonRequest.GetFieldValue("surname");
            string email = jsonRequest.GetFieldValue("email");
            bool subscribeNewsletter = jsonRequest.GetFieldValue("subscribeNewsletter");
            //...other fields...

            if (string.IsNullOrWhiteSpace(name)) ReturnError("Name is required");
            if (string.IsNullOrWhiteSpace(surname)) ReturnError("Surname is required");
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

            var response = new HttpResponse(StatusCode.Ok, new { UserId = userId});
            return response;

        }
        catch (ex){
            throw;
        }
    }
}
