namespace CodingTracker.Data.Models;

internal class Project
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<CodeEntry>? CodeEntries { get; set; } = null;
}