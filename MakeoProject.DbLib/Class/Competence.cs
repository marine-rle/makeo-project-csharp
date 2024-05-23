using System;
using System.Collections.Generic;

namespace MakeoProject.DbLib.Class;

public partial class Competence
{
    public ulong Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<FreelanceCompetence> FreelanceCompetences { get; set; } = new List<FreelanceCompetence>();

    public virtual ICollection<ProjetCompetence> ProjetCompetences { get; set; } = new List<ProjetCompetence>();
}
