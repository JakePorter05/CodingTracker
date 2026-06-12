namespace CodingTracker.Data.Models;

internal class EntryType
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<CodeEntry>? CodeEntries { get; set; } = null;
}