namespace rest_api_veterinaria.Models.Requests;

public class CrearPersonaReq
{
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
}
