public class User{

    public void Sync(int id){
        
        var record = database.Select("SELECT Name, Surname, Email FROM Users WHERE ID = @p1", id);

        var user = new User(record["Name"], record["Surname"], record["Email"]);

        // Email is required in the external system; if not present, set a default
        if(user.Email == null) user.Email = "nomail@test.it";

        var externalServiceClient = new HttpClient();
        externalServiceClient.Endpoint = config.Endpoint;
        externalServiceClient.Body = new {
            Customer = new {
                Name = user.Name,
                Surname = user.Surname,                
                ReferenceCompany = config.ReferenceCompany,
                Addresses = new [new {
                    Type = "Mail",
                    Value = user.Email
                }]
            }
        };


    }

}