namespace CodingTracker.Data.Repos;

internal class EntryTypeRepo
{
    private readonly DbContext _context;

    public EntryTypeRepo(DbContext context)
    {
        _context = context;

        _context.InitializeDatabase();
        _context.SeedData();
    }

    public void Create(EntryType entryType)
    {
        string sql = @"
            INSERT INTO EntryTypes (Name)
            VALUES (@Name);";

        _context.Connection?.Execute(sql, new { entryType.Name });
    }

    public EntryType? GetById(int id)
    {
        string sql = @"
            SELECT * FROM EntryTypes 
            WHERE Id = @Id;";

        return _context.Connection?.QueryFirstOrDefault<EntryType>(sql, new { Id = id });
    }

    public IEnumerable<EntryType> GetAll()
    {
        string sql = "SELECT * FROM EntryTypes;";
        return _context.Connection?.Query<EntryType>(sql) ?? Enumerable.Empty<EntryType>();
    }

    public void Update(EntryType entryType)
    {
        string sql = @"
            UPDATE EntryTypes 
            SET Name = @Name
            WHERE Id = @Id;";

        _context.Connection?.Execute(sql, new
        {
            entryType.Id,
            entryType.Name
        });
    }

    public void Delete(int id)
    {
        string sql = "DELETE FROM EntryTypes WHERE Id = @Id;";
        _context.Connection?.Execute(sql, new { Id = id });
    }
}
