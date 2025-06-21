﻿namespace ExtaTask.Models.Dto;

public class GetCreatedEventDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime Date { get; set; }

    public int MaxParticipants { get; set; }
}