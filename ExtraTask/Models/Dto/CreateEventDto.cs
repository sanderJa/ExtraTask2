using System.ComponentModel.DataAnnotations;

namespace ExtaTask.Models.Dto;

public class CreateEventDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(300)]
    public string Description { get; set; } = string.Empty;
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public int MaxParticipants { get; set; }
}