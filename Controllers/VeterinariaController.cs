using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rest_api_veterinaria.AuthModule;
using rest_api_veterinaria.Models;
using rest_api_veterinaria.Models.Requests;

namespace rest_api_veterinaria.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class VeterinariaController : ControllerBase
{
    private readonly JWTHelper _authHelper;
    private readonly ILogger<VeterinariaController> _logger;
    private readonly VeterinariaHelper _helper;

    public VeterinariaController(ILogger<VeterinariaController> logger, VeterinariaHelper helper, JWTHelper authHelper)
    {
        _logger = logger;
        _helper = helper;
        _authHelper = authHelper;
    }

    // Obtener todas las citas
    [HttpGet("citas")]
    public IActionResult GetCitas()
    {
        var citas = _helper.GetCitas();
        return Ok(citas); // Devuelve la lista de citas en formato JSON
    }

    // Obtener una cita por ID
    [HttpGet("citas/{id}")]
    public IActionResult GetCitaPorId(int id)
    {
        string user = _authHelper.GetUser(User.Claims);
        var cita = _helper.GetCitaPorId(id);
        if (cita == null)
        {
            return NotFound(); // Devuelve un 404 si la cita no existe
        }
        return Ok(cita);
    }

    // Crear una nueva cita
    [HttpPost("citas")]
    public IActionResult CrearCita([FromBody] CrearCitaReq cita)
    {
        string user = _authHelper.GetUser(User.Claims);
        if (user == "user")
        {
            return Unauthorized(new { message = "No tienes permisos para realizar esta acción" });
        }
        else if (user == "admin")
        {
            var id = _helper.CrearCita(cita);
            return Ok(id);
        }
        return Unauthorized(new { message = "No es un usuario válido" });

    }

    // Editar una cita existente
    [HttpPut("citas/{id}")]
    public IActionResult EditarCita(int id, [FromBody] CrearCitaReq cita)
    {
        if (cita == null)
        {
            return BadRequest("Datos inválidos.");
        }
        _helper.EditarCita(id, cita.Fecha, cita.Veterinario, cita.MascotaId);
        return NoContent(); // Devuelve un 204 si la cita fue editada correctamente
    }

    // Eliminar una cita
    [HttpDelete("citas/{id}")]
    public IActionResult EliminarCita(int id)
    {
        _helper.EliminarCita(id);
        return NoContent(); // Devuelve un 204 si la cita fue eliminada correctamente
    }

    // Obtener todas las personas
    [HttpGet("personas")]
    public IActionResult GetPersonas()
    {
        var personas = _helper.GetPersonas();
        return Ok(personas); // Devuelve la lista de personas en formato JSON
    }

    // Obtener una persona por su ID
    [HttpGet("personas/{id}")]
    public IActionResult GetPersonaPorId(int id)
    {
        var persona = _helper.GetPersonaPorId(id);
        if (persona == null)
        {
            return NotFound(); // Si no se encuentra la persona, devuelve 404
        }
        return Ok(persona); // Devuelve la persona en formato JSON
    }

    // Crear una nueva persona
    [HttpPost("personas")]
    public IActionResult CrearPersona([FromBody] CrearPersonaReq persona)
    {
        var id = _helper.CrearPersona(persona);
        return CreatedAtAction(nameof(GetPersonaPorId), new { id = id }, persona); // Devuelve 201 con el recurso creado
    }

    // Editar una persona
    [HttpPut("personas/{id}")]
    public IActionResult EditarPersona(int id, [FromBody] PersonaTO persona)
    {
        _helper.EditarPersona(id, persona.Nombre, persona.Correo, persona.Telefono);
        return NoContent(); // Devuelve 204 si la edición fue exitosa
    }

    // Eliminar una persona
    [HttpDelete("personas/{id}")]
    public IActionResult EliminarPersona(int id)
    {
        _helper.EliminarPersona(id);
        return NoContent(); // Devuelve 204 si la eliminación fue exitosa
    }

    // Obtener todas las mascotas de una persona
    [HttpGet("personas/{personaId}/mascotas")]
    public IActionResult GetMascotasPorPersona(int personaId)
    {
        var mascotas = _helper.GetMascotasPorPersona(personaId);
        return Ok(mascotas); // Devuelve la lista de mascotas en formato JSON
    }

    // Crear una mascota
    [HttpPost("mascotas")]
    public IActionResult CrearMascota([FromBody] CrearMascotaReq mascota)
    {
        var id = _helper.CrearMascota(mascota);
        return CreatedAtAction("", new { id = id }, mascota); // Devuelve 201 con el recurso creado
    }

    // Editar una mascota
    [HttpPut("mascotas/{id}")]
    public IActionResult EditarMascota(int id, [FromBody] MascotaTO mascota)
    {
        _helper.EditarMascota(id, mascota.Nombre, mascota.Raza, mascota.PersonaId);
        return NoContent(); // Devuelve 204 si la edición fue exitosa
    }

    // Eliminar una mascota
    [HttpDelete("mascotas/{id}")]
    public IActionResult EliminarMascota(int id)
    {
        _helper.EliminarMascota(id);
        return NoContent(); // Devuelve 204 si la eliminación fue exitosa
    }

    [HttpGet("persona/{id}/mis-datos")]
    public IActionResult ObtenerDatosDePersona(int id)
    {
        var datos = _helper.ObtenerDatosDePersona(id);

        if (datos == null)
        {
            return NotFound(new { message = "Persona no encontrada" });
        }

        return Ok(datos);
    }


}