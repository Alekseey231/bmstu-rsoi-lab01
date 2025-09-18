using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PersonService.Core.Exceptions;
using PersonService.Core.Interfaces;
using PersonService.Dto.Http;
using PersonService.Dto.Http.Converters;
using PersonService.Dto.Http.Models;
using PersonService.Server.Extensions;
using Swashbuckle.AspNetCore.Annotations;
using ValidationException = FluentValidation.ValidationException;

namespace PersonService.Server.Controllers;

[ApiController]
[Route("/api/v1/persons")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly IValidator<PersonRequest> _personRequestValidator;
    private readonly ILogger<PersonController> _logger;

    public PersonController(IPersonService personService, 
        IValidator<PersonRequest> personRequestValidator,
        ILogger<PersonController> logger)
    {
        _personService = personService;
        _personRequestValidator = personRequestValidator;
        _logger = logger;
    }

    /// <summary>
    /// Метод для создания сущности Person.
    /// </summary>
    /// <remarks>Метод для создания сущности Person.</remarks>
    /// <param name="personRequest">Запрос на создание.</param>
    /// <response code="201">Сущность Person успешно создана.</response>
    /// <response code="400">Одно или несколько полей модели невалидны.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpPost]
    [SwaggerOperation("Метод для создания сущности Person.", "Метод для создания сущности Person.")]
    [SwaggerResponse(statusCode: 201, type: typeof(Person), description: "Сущность Person успешно создана.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Одно или несколько полей модели невалидны.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> CreatePersonAsync([Required] [FromBody] PersonRequest personRequest)
    {
        try
        {
            await _personRequestValidator.ValidateAndThrowAsync(personRequest);

            var person = await _personService.CreatePersonAsync(personRequest.Name,
                personRequest.Age,
                personRequest.Address,
                personRequest.Work);

            var dtoPerson = PersonConverter.Convert(person);

            return Created($"/api/v1/persons/{person.Id}", dtoPerson);
        }
        catch (ValidationException e)
        {
            _logger.LogWarning(e, "One or more field is invalid. {@Model}", personRequest);

            return StatusCode(400, e.ToValidationErrorResponse());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request! {@Request}", personRequest);
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }

    /// <summary>
    /// Метод для обновления сущности Person.
    /// </summary>
    /// <remarks>Метод для обновления сущности Person.</remarks>
    /// <param name="personRequest">Запрос на обновление.</param>
    /// <param name="personId">Идентификатор сущности.</param>
    /// <response code="200">Сущность Person успешно обновлена.</response>
    /// <response code="400">Одно или несколько полей модели невалидны.</response>
    /// <response code="404">Сущность с указанным идентификатором не существует.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpPatch("{personId:int}")]
    [SwaggerOperation("Метод для обновления сущности Person.", "Метод для обновления сущности Person.")]
    [SwaggerResponse(statusCode: 200, type: typeof(Person), description: "Сущность Person успешно обновлена.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Одно или несколько полей модели невалидны.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Сущность с указанным идентификатором не существует.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> UpdatePersonAsync([Required][FromRoute] int personId,
        [Required][FromBody] PersonRequest personRequest)
    {
        try
        {
            await _personRequestValidator.ValidateAndThrowAsync(personRequest);
            
            var person = await _personService.UpdatePersonAsync(personId,
                personRequest.Name,
                personRequest.Age,
                personRequest.Address,
                personRequest.Work);
            
            var dtoPerson = PersonConverter.Convert(person);
            
            return Ok(dtoPerson);
        }
        catch (ValidationException e)
        {
            _logger.LogWarning(e, "One or more field is invalid. {@Model}", personRequest);

            return StatusCode(400, e.ToValidationErrorResponse());
        }
        catch (PersonNotFoundException e)
        {
            _logger.LogWarning(e, "Person with id {Id} is not found", personId);

            return StatusCode(404, new ErrorResponse($"Сущность Person с идентификатором {personId} не найдена."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request! {@Request}", personRequest);
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }

    /// <summary>
    /// Метод для удаления сущности Person.
    /// </summary>
    /// <remarks>Метод для удаления сущности Person.</remarks>
    /// <param name="personId">Идентификатор сущности.</param>
    /// <response code="204">Сущность Person успешно удалена.</response>
    /// <response code="404">Сущность с указанным идентификатором не существует.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpDelete("{personId:int}")]
    [SwaggerOperation("Метод для удаления сущности Person.", "Метод для удаления сущности Person.")]
    [SwaggerResponse(statusCode: 204, description: "Сущность Person успешно удалена.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Сущность с указанным идентификатором не существует.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> DeletePersonAsync([Required][FromRoute] int personId)
    {
        try
        {
            await _personService.DeletePersonAsync(personId);

            return StatusCode(204);
        }
        catch (PersonNotFoundException e)
        {
            _logger.LogWarning(e, "Person with id {Id} is not found", personId);

            return StatusCode(404, new ErrorResponse($"Сущность Person с идентификатором {personId} не найдена."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request!");
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }

    /// <summary>
    /// Метод для получения коллекции сущностей Person.
    /// </summary>
    /// <remarks>Метод для получения коллекции сущностей Person.</remarks>
    /// <response code="200">Коллекция успешно получена.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpGet]
    [SwaggerOperation("Метод для получения коллекции сущностей Person.", "Метод для получения коллекции сущностей Person.")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<Person>), description: "Коллекция успешно получена.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetPersonsAsync()
    {
        try
        {
            var people = await _personService.GetPeopleAsync();
            
            var dtoPeople = people.ConvertAll(PersonConverter.Convert);
            
            return Ok(dtoPeople);
        }       
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request!");
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    /// <summary>
    /// Метод для получения сущности Person по <paramref name="personId"/>
    /// </summary>
    /// <remarks>Метод для получения сущности Person.</remarks>
    /// <param name="personId">Идентификатор сущности.</param>
    /// <response code="200">Сущность успешно получена.</response>
    /// <response code="404">Сущность с указанным идентификатором не найдена.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpGet("{personId:int}")]
    [SwaggerOperation("Метод для получения сущности Person.", "Метод для получения сущности Person.")]
    [SwaggerResponse(statusCode: 200, type: typeof(Person), description: "Коллекция успешно получена.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Сущность с указанным идентификатором не найдена.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetPersonByIdAsync(int personId)
    {
        try
        {
            var person = await _personService.GetPersonByIdAsync(personId);
            
            var dtoPerson = PersonConverter.Convert(person);
            
            return Ok(dtoPerson);
        }       
        catch (PersonNotFoundException e)
        {
            _logger.LogWarning(e, "Person with id {Id} is not found", personId);

            return StatusCode(404, new ErrorResponse($"Сущность Person с идентификатором {personId} не найдена."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request!");
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
}