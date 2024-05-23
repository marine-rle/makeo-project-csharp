using System;
using System.Collections.Generic;

namespace MakeoProject.DbLib.Class;

public partial class FreelanceCompetence
{
    public ulong Id { get; set; }

    public ulong FreelanceId { get; set; }

    public ulong IdCompetences { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Freelance Freelance { get; set; } = null!;

    public virtual Competence IdCompetencesNavigation { get; set; } = null!;
}
