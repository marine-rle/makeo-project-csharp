using System;
using System.Collections.Generic;

namespace MakeoProject.DbLib.Class;

public partial class Project
{
    public string UserName => User?.Email;
    public string StatusName => Statut?.Name;

    public ulong Id { get; set; }

    public ulong UserId { get; set; }

    public DateTime DateDemande { get; set; }

    public ulong StatutId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<FreelanceProject> FreelanceProjects { get; set; } = new List<FreelanceProject>();

    public virtual ICollection<ProjetCompetence> ProjetCompetences { get; set; } = new List<ProjetCompetence>();

    public virtual Statut Statut { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public string Skills => string.Join(", ", ProjetCompetences.Select(pc => pc.IdCompetencesNavigation.Name));

}
