using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PersonService.Core.Exceptions;
using PersonService.Core.Interfaces;
using PersonService.Dto.Http;
using PersonService.Dto.Http.Converters;
using PersonService.Dto.Http.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace PersonService.Server.Controllers;

[ApiController]
[Route("/api/v1/persons")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly ILogger<PersonController> _logger;

    public PersonController(IPersonService personService, ILogger<PersonController> logger)
    {
        _personService = personService;
        _logger = logger;
    }

    /// <summary>
    /// Метод для создания сущности Person.
    /// </summary>
    /// <remarks>Метод для сущности Person.</remarks>
    /// <param name="personRequest">Запрос на создание.</param>
    /// <response code="200">Сущность Person успешно создана.</response>
    /// <response code="400">Одно или несколько полей модели невалидны.</response>
    /// <response code="409">Сущность с указанным идентификатором уже существует.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpPost]
    [SwaggerOperation("Метод для создания сущности Person.", "Метод для создания сущности Person.")]
    [SwaggerResponse(statusCode: 200, type: typeof(Person), description: "Сущность Person успешно создана.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Одно или несколько полей модели невалидны.")]
    [SwaggerResponse(statusCode: 409, type: typeof(ErrorResponse), description: "Сущность с указанным идентификатором уже существует.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> CreatePersonAsync([Required] [FromBody] PersonRequest personRequest)
    {
        try
        {
            var person = await _personService.CreatePersonAsync(personRequest.Name,
                personRequest.Age,
                personRequest.Address,
                personRequest.Work);

            var dtoPerson = PersonConverter.Convert(person);
            
            return Created($"/api/v1/persons/{person.Id}", dtoPerson);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request! {@Request}", personRequest);
            
            return StatusCode(404, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
}