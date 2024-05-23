using System;
using System.Collections.Generic;

namespace MakeoProject.DbLib.Class;

public partial class Statut
{
    public ulong Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
