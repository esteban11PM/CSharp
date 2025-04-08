namespace Entity.DTOs;

public class RolUserCreateDTO
{
    public int Id {get; set;}
    public bool Active {get; set;}

    public int UserId {get; set;} //Llave foranea
    public int RoleId {get; set;} //Llave foranea
}
