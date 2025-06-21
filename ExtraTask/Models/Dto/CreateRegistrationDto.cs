namespace ExtaTask.Models.Dto;

public class CreateRegistrationDto
{
    public int Id { get; set; }

    public DateTime RegisteredAt { get; set; }

    public int ParticipantId { get; set; }

    public int EventId { get; set; }
}