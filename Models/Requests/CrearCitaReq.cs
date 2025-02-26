namespace rest_api_veterinaria.Models.Requests;

public class CrearCitaReq
{
    public DateTime Fecha { get; set; }
    public string Veterinario { get; set; } = string.Empty;
    public int MascotaId { get; set; }
}
