using System;
using System.Collections.Generic;

namespace MakeoProject.DbLib.Class;

public partial class Session
{
    public string Id { get; set; } = null!;

    public ulong? UserId { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public string Payload { get; set; } = null!;

    public int LastActivity { get; set; }
}
