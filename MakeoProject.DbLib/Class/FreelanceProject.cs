using System;
using System.Collections.Generic;

namespace MakeoProject.DbLib.Class;

public partial class FreelanceProject
{
    public ulong Id { get; set; }

    public ulong FreelanceId { get; set; }

    public ulong ProjectId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Freelance Freelance { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
