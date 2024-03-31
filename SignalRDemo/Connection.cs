using System;
using System.Collections.Generic;

namespace SignalRDemo;

public partial class Connection
{
    public Guid Id { get; set; }

    public Guid PersonId { get; set; }

    public string SignalRid { get; set; } = null!;

    public DateTime TimeStamp { get; set; }
}
