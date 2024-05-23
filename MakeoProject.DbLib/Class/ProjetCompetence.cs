using System;
using System.Collections.Generic;

namespace MakeoProject.DbLib.Class;

public partial class ProjetCompetence
{
    public ulong Id { get; set; }

    public ulong ProjectId { get; set; }

    public ulong IdCompetences { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Competence IdCompetencesNavigation { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
