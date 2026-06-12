namespace CodingTracker.Data.Models;

internal class CodeEntry
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Duration { get; set; }
    public string? Description { get; set; } = null;

    public int EntryTypeId { get; set; }
    public EntryType? EntryType { get; set; }

    public int ProjectId { get; set; }
    public Project? Project { get; set; }
}