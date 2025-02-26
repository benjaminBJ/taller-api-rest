namespace rest_api_veterinaria.Models;

public class MascotaTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Raza { get; set; } = string.Empty;
    public int PersonaId { get; set; }
}
