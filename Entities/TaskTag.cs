using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

public class TaskTag
{
    public Guid TaskId { get; set; }
    public TaskEntity Task { get; set; }

    public Guid TagId { get; set; }

    public TagEntity Tag { get; set; }
}
