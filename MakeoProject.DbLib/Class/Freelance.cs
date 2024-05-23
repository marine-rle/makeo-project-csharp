using System;
using System.Collections.Generic;

namespace MakeoProject.DbLib.Class;

public partial class Freelance
{
    public ulong Id { get; set; }

    public string Name { get; set; } = null!;
        
    public string Surname { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<FreelanceCompetence> FreelanceCompetences { get; set; } = new List<FreelanceCompetence>();

    public virtual ICollection<FreelanceProject> FreelanceProjects { get; set; } = new List<FreelanceProject>();

    public string Skills => string.Join(", ", FreelanceCompetences.Select(pc => pc.IdCompetencesNavigation.Name));
 
}
