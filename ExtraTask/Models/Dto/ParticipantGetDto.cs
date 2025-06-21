namespace ExtaTask.Models.Dto;

public class ParticipantGetDto
{
    
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; }= null!;
    public string Email { get; set; }= null!;
    public List<EventGetDto> Events { get; set; }= null!;
    
}