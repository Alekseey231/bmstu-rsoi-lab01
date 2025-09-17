using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PersonService.Dto.Http;

/// <summary>
/// Модель запроса на создание сущности Person.
/// </summary>
public class PersonRequest
{
    /// <summary>
    /// Имя.
    /// </summary>
    [Required]
    [DataMember(Name = "name")]
    public string Name { get; set; }
    
    /// <summary>
    /// Возраст.
    /// </summary>
    [DataMember(Name = "age")]
    public int? Age { get; set; }
    
    /// <summary>
    /// Адрес.
    /// </summary>
    [DataMember(Name = "address")]
    public string? Address { get; set; }
    
    /// <summary>
    /// Место работы.
    /// </summary>
    [DataMember(Name = "work")]
    public string? Work { get; set; }

    public PersonRequest(string name, int? age, string? address, string? work)
    {
        Name = name;
        Age = age;
        Address = address;
        Work = work;
    }
}