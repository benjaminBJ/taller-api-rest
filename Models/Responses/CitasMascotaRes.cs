namespace rest_api_veterinaria.Models.Responses;

public class CitasMascotaRes : MascotaTO
{
    public List<CitaTO> Citas { get; set; } = new List<CitaTO>();
}
