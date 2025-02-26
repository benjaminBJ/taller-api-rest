namespace rest_api_veterinaria.Models;

public class CitaTO
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Veterinario { get; set; } = string.Empty;
    public int MascotaId { get; set; }
}
