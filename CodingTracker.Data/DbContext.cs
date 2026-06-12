namespace CodingTracker.Data;

internal class DbContext : IDisposable
{
    const string ConnectionString = "Data Source=CodeTracker.db";
    public SqliteConnection? Connection { get; set; }

    public DbContext()
    {
        SqlMapper.AddTypeHandler(new TimeSpanHandler());
        Connection = new SqliteConnection(ConnectionString);
        Connection.Open();
    }

    public void SeedData()
    {
        // Check if EntryTypes already have data
        var entryTypeCount = Connection?.ExecuteScalar<int>("SELECT COUNT(*) FROM EntryTypes;");
        if (entryTypeCount == 0)
        {
            string insertEntryType = @"
                INSERT INTO EntryTypes (Name)
                VALUES (@Name);";
            Connection?.Execute(insertEntryType, new
            {
                Name = "Testing"
            });
            Connection?.Execute(insertEntryType, new
            {
                Name = "Data"
            });
            Connection?.Execute(insertEntryType, new
            {
                Name = "UI"
            });
        }

        // Check if Projects already have data
        var projectCount = Connection?.ExecuteScalar<int>("SELECT COUNT(*) FROM Projects;");
        if (projectCount == 0)
        {
            string insertProject = @"
                INSERT INTO Projects (Name)
                VALUES (@Name);";
            Connection?.Execute(insertProject, new
            {
                Name = "Coding Tracker"
            });
        }

        // Check if CodeEntries already have data
        var codeEntryCount = Connection?.ExecuteScalar<int>("SELECT COUNT(*) FROM CodeEntries;");
        if (codeEntryCount == 0)
        {
            string insertCodeEntry = @"
                INSERT INTO CodeEntries (Date, Duration, Description, EntryTypeId, ProjectId)
                VALUES (@Date, @Duration, @Description, @EntryTypeId, @ProjectId);";
            Connection?.Execute(insertCodeEntry, new
            {
                Date = DateTime.Parse("2024-02-02").ToString("yyyy-MM-dd"),
                Duration = TimeSpan.FromMinutes(25).ToString(),
                Description = "Added Repos to the test project.",
                EntryTypeId = 1, 
                ProjectId = 1    
            });
            Connection?.Execute(insertCodeEntry, new
            {
                Date = DateTime.Parse("2024-01-01").ToString("yyyy-MM-dd"),
                Duration = TimeSpan.FromMinutes(45).ToString(),
                Description = "Build basic Data Models with Repos.",
                EntryTypeId = 2, 
                ProjectId = 1    
            });
        }
    }

    public void InitializeDatabase()
    {
        string createCodeEntries = @"
            CREATE TABLE IF NOT EXISTS CodeEntries (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                Date        TEXT NOT NULL,
                Duration    TEXT NOT NULL,
                Description TEXT NULL,
                EntryTypeId INTEGER NOT NULL,
                ProjectId   INTEGER NOT NULL,
                FOREIGN KEY (EntryTypeId) REFERENCES EntryTypes(Id),
                FOREIGN KEY (ProjectId) REFERENCES Projects(Id)
            );";
        Connection?.Execute(createCodeEntries);

        string createEntryTypes = @"
            CREATE TABLE IF NOT EXISTS EntryTypes ( 
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                Name        TEXT NOT NULL
            );";
        Connection?.Execute(createEntryTypes);

        string createProjects = @"
            CREATE TABLE IF NOT EXISTS Projects (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                Name        TEXT NOT NULL
            );";
        Connection?.Execute(createProjects);
    }

    public void Dispose()
    {
        Connection?.Close();
        Connection?.Dispose();
    }
}