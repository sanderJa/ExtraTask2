using System.ComponentModel.DataAnnotations;

namespace ExtaTask.Models.Dto;

public class SpeakerCreateDto
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
}