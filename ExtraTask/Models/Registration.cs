using System;
using System.Collections.Generic;

namespace ExtaTask.Models;

public partial class Registration
{
    public int Id { get; set; }

    public DateTime RegisteredAt { get; set; }

    public int ParticipantId { get; set; }

    public int EventId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Participant Participant { get; set; } = null!;
}
