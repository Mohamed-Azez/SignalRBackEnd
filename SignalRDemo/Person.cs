using System;
using System.Collections.Generic;

namespace SignalRDemo;

public partial class Person
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
