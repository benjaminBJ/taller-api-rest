using rest_api_veterinaria.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using rest_api_veterinaria.Models.Requests;

namespace rest_api_veterinaria.Dao;

public class VeterinariaDAO
{
    private readonly string connectionString;

    public VeterinariaDAO(string connectionString)
    {
        this.connectionString = connectionString;
    }

    private SqlConnection ObtenerConexion()
    {
        var connection = new SqlConnection(connectionString);
        connection.Open();
        return connection;
    }

    // Obtener todas las citas
    public List<CitaTO> GetCitas()
    {
        using var connection = ObtenerConexion();
        return connection.Query<CitaTO>("dbo.ObtenerTodasLasCitas", commandType: CommandType.StoredProcedure).ToList();
    }

    // Obtener una cita por ID
    public CitaTO GetCitaPorId(int id)
    {
        using var connection = ObtenerConexion();
        var cita = connection.QueryFirstOrDefault<CitaTO>(
            "dbo.ObtenerCitaPorId",
            new { Id = id },
            commandType: CommandType.StoredProcedure
        );
        return cita;
    }

    // Crear una nueva cita
    public int CrearCita(CrearCitaReq cita)
    {
        using var connection = ObtenerConexion();
        var parameters = new DynamicParameters();
        parameters.Add("Fecha", cita.Fecha);
        parameters.Add("Veterinario", cita.Veterinario);
        parameters.Add("MascotaId", cita.MascotaId);

        var idCita = connection.ExecuteScalar<int>(
            "dbo.CrearCita",
            parameters,
            commandType: CommandType.StoredProcedure
        );
        return idCita;
    }

    // Editar una cita existente
    public void EditarCita(int id, DateTime fecha, string veterinario, int mascotaId)
    {
        using var connection = ObtenerConexion();
        connection.Execute(
            "dbo.EditarCita",
            new { Id = id, Fecha = fecha, Veterinario = veterinario, MascotaId = mascotaId },
            commandType: CommandType.StoredProcedure
        );
    }

    // Eliminar una cita
    public void EliminarCita(int id)
    {
        using var connection = ObtenerConexion();
        connection.Execute(
            "dbo.EliminarCita",
            new { Id = id },
            commandType: CommandType.StoredProcedure
        );
    }

    public List<PersonaTO> GetPersonas()
    {
        using var connection = ObtenerConexion();
        return connection.Query<PersonaTO>("dbo.ObtenerPersonas", commandType: CommandType.StoredProcedure).ToList();
    }

    public PersonaTO GetPersonaPorId(int id)
    {
        using var connection = ObtenerConexion();

        var persona = connection.QueryFirstOrDefault<PersonaTO>(
            "dbo.ObtenerPersonaPorId",
            new { PersonaId = id },
            commandType: CommandType.StoredProcedure
        );
        return persona;
    }

    public int CrearPersona(CrearPersonaReq persona)
    {
        using var connection = ObtenerConexion();
        var parameters = new DynamicParameters();
        parameters.Add("Nombre", persona.Nombre);
        parameters.Add("Correo", persona.Correo);
        parameters.Add("Telefono", persona.Telefono);
        var fueCreado =  connection.ExecuteScalar<int>(
            "dbo.CrearPersona",
            parameters,
            commandType: CommandType.StoredProcedure
        );
        return fueCreado;
    }

    public void EditarPersona(int id, string nombre, string correo, string telefono)
    {
        using var connection = ObtenerConexion();
        connection.Execute(
            "dbo.EditarPersona",
            new { Id = id, Nombre = nombre, Correo = correo, Telefono = telefono },
            commandType: CommandType.StoredProcedure
        );
    }

    public void EliminarPersona(int id)
    {
        using var connection = ObtenerConexion();
        connection.Execute(
            "dbo.EliminarPersona",
            new { Id = id },
            commandType: CommandType.StoredProcedure
        );
    }

    public List<MascotaTO> GetMascotasPorPersona(int personaId)
    {
        using var connection = ObtenerConexion();
        var parameters = new DynamicParameters();
        parameters.Add("PersonaId", personaId);

        return connection.Query<MascotaTO>(
            "dbo.ObtenerMascotasPorPersona",
            parameters,
            commandType: CommandType.StoredProcedure
        ).ToList();
    }

    // Crear una mascota
    public int CrearMascota(CrearMascotaReq mascota)
    {
        using var connection = ObtenerConexion();
        var parameters = new DynamicParameters();
        parameters.Add("Nombre", mascota.Nombre);
        parameters.Add("Raza", mascota.Raza);
        parameters.Add("PersonaId", mascota.PersonaId);

        // Ejecutar el procedimiento almacenado y obtener el ID del recurso creado
        var fueCreada = connection.ExecuteScalar<int>(
            "dbo.CrearMascota",
            parameters,
            commandType: CommandType.StoredProcedure
        );
        return fueCreada;
    }

    // Editar mascota
    public void EditarMascota(int id, string nombre, string raza, int personaId)
    {
        using var connection = ObtenerConexion();
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
        parameters.Add("Nombre", nombre);
        parameters.Add("Raza", raza);
        parameters.Add("PersonaId", personaId);

        connection.Execute(
            "dbo.EditarMascota",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    // Eliminar mascota
    public void EliminarMascota(int id)
    {
        using var connection = ObtenerConexion();
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        connection.Execute(
            "dbo.EliminarMascota",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    // Función para obtener las citas de una mascota específica
    public List<CitaTO> GetCitasPorMascota(int mascotaId)
    {
        string prc = "dbo.ObtenerCitasPorMascota";
        using var connection = ObtenerConexion();
        var citas = connection.Query<CitaTO>(
            prc,
            new { MascotaId = mascotaId },
            commandType: CommandType.StoredProcedure
        ).ToList();
        return citas;
    }
}
