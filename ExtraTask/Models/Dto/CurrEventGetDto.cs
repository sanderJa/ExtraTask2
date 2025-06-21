namespace ExtaTask.Models.Dto;

public class CurrEventGetDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime Date { get; set; }

    public int MaxParticipants { get; set; }
    public string FullNameSpeaker { get; set; } = null!;
    public int CurrCountOfParticipants { get; set; }
    public int CountOfFreePlaces { get; set; }
}