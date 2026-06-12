namespace CodingTracker.Data.Repos;

internal class CodeEntryRepo
{
    private readonly DbContext _context;

    public CodeEntryRepo(DbContext context)
    {
        _context = context;

        _context.InitializeDatabase();
        _context.SeedData();
    }

    public void Create(CodeEntry entry)
    {
        string sql = @"
            INSERT INTO CodeEntries (Date, Duration, Description, EntryTypeId, ProjectId)
            VALUES (@Date, @Duration, @Description, @EntryTypeId, @ProjectId);";

        _context.Connection?.Execute(sql, new
        {
            Date = entry.Date.ToString("yyyy-MM-dd"),
            Duration = entry.Duration.ToString(),
            entry.Description,
            entry.EntryTypeId,
            entry.ProjectId
        });
    }

    public CodeEntry? GetById(int id)
    {
        string sql = @"
            SELECT * FROM CodeEntries 
            WHERE Id = @Id;";

        return _context.Connection?.QueryFirstOrDefault<CodeEntry>(sql, new { Id = id });
    }

    public IEnumerable<CodeEntry> GetAll()
    {
        string sql = "SELECT * FROM CodeEntries;";
        return _context.Connection?.Query<CodeEntry>(sql) ?? Enumerable.Empty<CodeEntry>();
    }

    public void Update(CodeEntry entry)
    {
        string sql = @"
            UPDATE CodeEntries 
            SET Date = @Date, 
                Duration = @Duration, 
                Description = @Description,
                EntryTypeId = @EntryTypeId,
                ProjectId = @ProjectId
            WHERE Id = @Id;";

        _context.Connection?.Execute(sql, new
        {
            entry.Id,
            Date = entry.Date.ToString("yyyy-MM-dd"),
            Duration = entry.Duration.ToString(),
            entry.Description,
            entry.EntryTypeId,
            entry.ProjectId
        });
    }

    public void Delete(int id)
    {
        string sql = "DELETE FROM CodeEntries WHERE Id = @Id;";
        _context.Connection?.Execute(sql, new { Id = id });
    }
}
