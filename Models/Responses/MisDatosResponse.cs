namespace rest_api_veterinaria.Models.Responses;

public class MisDatosResponse : PersonaTO
{
    public List<CitasMascotaRes> Mascotas { get; set; } = new List<CitasMascotaRes>();
}
