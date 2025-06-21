using System;
using System.Collections.Generic;

namespace ExtaTask.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime Date { get; set; }

    public int MaxParticipants { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<Speaker> Speakers { get; set; } = new List<Speaker>();
}
