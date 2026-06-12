namespace CodingTracker.Test.Repos;

public class EntryTypeTests : IDisposable
{
    private readonly DbContext _context;
    private readonly EntryTypeRepo _repo;
    private readonly string _testDbPath = "test_entrytype.db";

    public EntryTypeTests()
    {
        if (File.Exists(_testDbPath))
            File.Delete(_testDbPath);

        _context = new DbContext();
        _context.InitializeDatabase();
        _context.SeedData();
        _repo = new EntryTypeRepo(_context);
    }

    [Fact]
    public void Create_ShouldAddNewEntryType()
    {
        // Arrange
        var entryType = new EntryType
        {
            Name = "Documentation"
        };

        // Act
        _repo.Create(entryType);
        var allTypes = _repo.GetAll().ToList();

        // Assert
        Assert.Contains(allTypes, t => t.Name == "Documentation");
    }

    [Fact]
    public void GetById_ShouldReturnCorrectEntryType()
    {
        // Arrange
        var entryType = new EntryType { Name = "Refactoring" };
        _repo.Create(entryType);
        var allTypes = _repo.GetAll().ToList();
        var createdId = allTypes.First(t => t.Name == "Refactoring").Id;

        // Act
        var result = _repo.GetById(createdId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Refactoring", result.Name);
    }

    [Fact]
    public void GetAll_ShouldReturnAllEntryTypes()
    {
        // Act
        var types = _repo.GetAll().ToList();

        // Assert
        Assert.NotNull(types);
        Assert.True(types.Count >= 3); // Should have at least seed data (Testing, Data, UI)
    }

    [Fact]
    public void Update_ShouldModifyExistingEntryType()
    {
        // Arrange
        var entryType = new EntryType { Name = "Original Name" };
        _repo.Create(entryType);
        var allTypes = _repo.GetAll().ToList();
        var createdType = allTypes.First(t => t.Name == "Original Name");

        // Act
        createdType.Name = "Updated Name";
        _repo.Update(createdType);
        var updatedType = _repo.GetById(createdType.Id);

        // Assert
        Assert.NotNull(updatedType);
        Assert.Equal("Updated Name", updatedType.Name);
    }

    [Fact]
    public void Delete_ShouldRemoveEntryType()
    {
        // Arrange
        var entryType = new EntryType { Name = "To Be Deleted" };
        _repo.Create(entryType);
        var allTypes = _repo.GetAll().ToList();
        var createdType = allTypes.First(t => t.Name == "To Be Deleted");
        var typeId = createdType.Id;

        // Act
        _repo.Delete(typeId);
        var deletedType = _repo.GetById(typeId);

        // Assert
        Assert.Null(deletedType);
    }

    public void Dispose()
    {
        _context?.Dispose();
        if (File.Exists(_testDbPath))
            File.Delete(_testDbPath);
    }
}
