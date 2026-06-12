namespace CodingTracker.Data.Repos;

internal class ProjectRepo
{
    private readonly DbContext _context;

    public ProjectRepo(DbContext context)
    {
        _context = context;

        _context.InitializeDatabase();
        _context.SeedData();
    }

    public void Create(Project project)
    {
        string sql = @"
            INSERT INTO Projects (Name)
            VALUES (@Name);";

        _context.Connection?.Execute(sql, new { project.Name });
    }

    public Project? GetById(int id)
    {
        string sql = @"
            SELECT * FROM Projects 
            WHERE Id = @Id;";

        return _context.Connection?.QueryFirstOrDefault<Project>(sql, new { Id = id });
    }

    public IEnumerable<Project> GetAll()
    {
        string sql = "SELECT * FROM Projects;";
        return _context.Connection?.Query<Project>(sql) ?? Enumerable.Empty<Project>();
    }

    public void Update(Project project)
    {
        string sql = @"
            UPDATE Projects 
            SET Name = @Name
            WHERE Id = @Id;";

        _context.Connection?.Execute(sql, new
        {
            project.Id,
            project.Name
        });
    }

    public void Delete(int id)
    {
        string sql = "DELETE FROM Projects WHERE Id = @Id;";
        _context.Connection?.Execute(sql, new { Id = id });
    }
}
