namespace rest_api_veterinaria.Models.Requests;

public class CrearMascotaReq
{
    public string Nombre { get; set; } = string.Empty;
    public string Raza { get; set; } = string.Empty;
    public int PersonaId { get; set; }
}
