namespace Entity.DTOs;

public class UserCreateDTO
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password {get; set;}
    public bool State { get; set; }
    public int PersonId { get; set; } //Lave foranea
}
