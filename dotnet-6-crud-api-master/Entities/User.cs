namespace WebApi.Entities;

using System.Text.Json.Serialization;
using WebApi.Entities.Base;

public class User 
{    
    public int Id { get; set; } 

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime ModifiedDate { get; set; }    
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }

    [JsonIgnore]
    public string PasswordHash { get; set; }  

}