namespace ExtaTask.Models.Dto;

public class EventGetDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime Date { get; set; }
    public int SpeakerId { get; set; }
    public string LastNameSpeaker { get; set; } = null!;
    
    
}