namespace CodingTracker.Test.Repos;

public class CodeEntryTests : IDisposable
{
    private readonly DbContext _context;
    private readonly CodeEntryRepo _repo;
    private readonly string _testDbPath = "test_codeentry.db";

    public CodeEntryTests()
    {
        if (File.Exists(_testDbPath))
            File.Delete(_testDbPath);

        _context = new DbContext();
        _context.InitializeDatabase();
        _context.SeedData();
        _repo = new CodeEntryRepo(_context);
    }

    [Fact]
    public void Create_ShouldAddNewCodeEntry()
    {
        // Arrange
        var entry = new CodeEntry
        {
            Date = DateTime.Parse("2024-03-15"),
            Duration = TimeSpan.FromHours(3),
            Description = "Test Entry",
            EntryTypeId = 1,
            ProjectId = 1
        };

        // Act
        _repo.Create(entry);
        var allEntries = _repo.GetAll().ToList();

        // Assert
        Assert.True(allEntries.Count > 0);
        Assert.Contains(allEntries, e => e.Description == "Test Entry");
    }

    [Fact]
    public void GetById_ShouldReturnCorrectEntry()
    {
        // Arrange
        var entry = new CodeEntry
        {
            Date = DateTime.Parse("2024-03-15"),
            Duration = TimeSpan.FromHours(2),
            Description = "Get By Id Test",
            EntryTypeId = 1,
            ProjectId = 1
        };
        _repo.Create(entry);
        var allEntries = _repo.GetAll().ToList();
        var createdId = allEntries.First(e => e.Description == "Get By Id Test").Id;

        // Act
        var result = _repo.GetById(createdId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Get By Id Test", result.Description);
    }

    [Fact]
    public void GetAll_ShouldReturnAllEntries()
    {
        // Act
        var entries = _repo.GetAll().ToList();

        // Assert
        Assert.NotNull(entries);
        Assert.True(entries.Count >= 2); // Should have at least seed data
    }

    [Fact]
    public void Update_ShouldModifyExistingEntry()
    {
        // Arrange
        var entry = new CodeEntry
        {
            Date = DateTime.Parse("2024-03-15"),
            Duration = TimeSpan.FromHours(1),
            Description = "Original Description",
            EntryTypeId = 1,
            ProjectId = 1
        };
        _repo.Create(entry);
        var allEntries = _repo.GetAll().ToList();
        var createdEntry = allEntries.First(e => e.Description == "Original Description");

        // Act
        createdEntry.Description = "Updated Description";
        createdEntry.Duration = TimeSpan.FromHours(5);
        _repo.Update(createdEntry);
        var updatedEntry = _repo.GetById(createdEntry.Id);

        // Assert
        Assert.NotNull(updatedEntry);
        Assert.Equal("Updated Description", updatedEntry.Description);
        Assert.Equal(TimeSpan.FromHours(5).ToString(), updatedEntry.Duration.ToString());
    }

    [Fact]
    public void Delete_ShouldRemoveEntry()
    {
        // Arrange
        var entry = new CodeEntry
        {
            Date = DateTime.Parse("2024-03-15"),
            Duration = TimeSpan.FromHours(1),
            Description = "To Be Deleted",
            EntryTypeId = 1,
            ProjectId = 1
        };
        _repo.Create(entry);
        var allEntries = _repo.GetAll().ToList();
        var createdEntry = allEntries.First(e => e.Description == "To Be Deleted");
        var entryId = createdEntry.Id;

        // Act
        _repo.Delete(entryId);
        var deletedEntry = _repo.GetById(entryId);

        // Assert
        Assert.Null(deletedEntry);
    }

    public void Dispose()
    {
        _context?.Dispose();
        if (File.Exists(_testDbPath))
            File.Delete(_testDbPath);
    }
}
