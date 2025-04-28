namespace Entity.DTOs;

public class RolFormPermissionCreateDTO
{
    public int Id {get; set;}
    public bool Active {get; set;}
    public int RolId {get; set;}//llave foranea
    public int PermissionId {get; set;}//llave foranea
    public int FormId {get; set;} //llave foranea

}
