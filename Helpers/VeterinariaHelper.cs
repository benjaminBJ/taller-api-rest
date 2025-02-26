

using rest_api_veterinaria.Dao;
using rest_api_veterinaria.Models;
using rest_api_veterinaria.Models.Requests;
using rest_api_veterinaria.Models.Responses;

public class VeterinariaHelper
{
    private readonly VeterinariaDAO _veterinariaDAO;
    private readonly HttpClient _httpClient;

    public VeterinariaHelper(IConfiguration configuration)
    {
        // Obtener la conexión de appsettings.json
        string connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";

        _veterinariaDAO = new VeterinariaDAO(connectionString);
        _httpClient = new HttpClient();
    }

    //  Personas
    public List<PersonaTO> GetPersonas() => _veterinariaDAO.GetPersonas();

    //  Obtener una persona por su ID
    public PersonaTO GetPersonaPorId(int id) => _veterinariaDAO.GetPersonaPorId(id);

    //  Crear persona
    public int CrearPersona(CrearPersonaReq persona)
    {
        return _veterinariaDAO.CrearPersona(persona);
    }

    //  Editar persona
    public void EditarPersona(int id, string nombre, string correo, string telefono)
    {
        _veterinariaDAO.EditarPersona(id, nombre, correo, telefono);
    }

    // 🔹 Eliminar persona
    public void EliminarPersona(int id)
    {
        _veterinariaDAO.EliminarPersona(id);
    }

    // Mascotas
    public List<MascotaTO> GetMascotasPorPersona(int personaId) => _veterinariaDAO.GetMascotasPorPersona(personaId);

    //  Crear mascota
    public int CrearMascota(CrearMascotaReq mascota)
    {
        return _veterinariaDAO.CrearMascota(mascota);
    }

    //  Editar mascota
    public void EditarMascota(int id, string nombre, string raza, int personaId)
    {
        _veterinariaDAO.EditarMascota(id, nombre, raza, personaId);
    }

    //  Eliminar mascota
    public void EliminarMascota(int id)
    {
        _veterinariaDAO.EliminarMascota(id);
    }

    // Citas
    public List<CitaTO> GetCitas()
    {
        return _veterinariaDAO.GetCitas();
    }

    public CitaTO GetCitaPorId(int id)
    {
        return _veterinariaDAO.GetCitaPorId(id);
    }

    // Crear cita
    public int CrearCita(CrearCitaReq cita)
    {
        return _veterinariaDAO.CrearCita(cita);
    }

    // Editar cita
    public void EditarCita(int id, DateTime fecha, string veterinario, int mascotaId)
    {
        _veterinariaDAO.EditarCita(id, fecha, veterinario, mascotaId);
    }

    // Eliminar cita
    public void EliminarCita(int id)
    {
        _veterinariaDAO.EliminarCita(id);
    }

    // Obtener todos los datos de una persona, incluyendo mascotas y citas
    public MisDatosResponse ObtenerDatosDePersona(int personaId)
    {
        // Obtener la persona
        var persona = _veterinariaDAO.GetPersonaPorId(personaId);

        if (persona == null)
            return null;

        // Crear la respuesta
        var respuesta = new MisDatosResponse
        {
            Id = persona.Id,
            Nombre = persona.Nombre,
            Correo = persona.Correo,
            Telefono = persona.Telefono
        };

        // Obtener las mascotas de la persona
        var mascotas = _veterinariaDAO.GetMascotasPorPersona(personaId);

        foreach (var mascota in mascotas)
        {
            var citas = _veterinariaDAO.GetCitasPorMascota(mascota.Id);
            var mascotaRes = new CitasMascotaRes
            {
                Id = mascota.Id,
                Nombre = mascota.Nombre,
                Raza = mascota.Raza,
                Citas = citas
            };

            respuesta.Mascotas.Add(mascotaRes);
        }

        return respuesta;
    }
}