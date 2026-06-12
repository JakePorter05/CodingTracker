namespace CodingTracker.Test.Repos;

public class ProjectTests : IDisposable
{
    private readonly DbContext _context;
    private readonly ProjectRepo _repo;
    private readonly string _testDbPath = "test_project.db";

    public ProjectTests()
    {
        if (File.Exists(_testDbPath))
            File.Delete(_testDbPath);

        _context = new DbContext();
        _context.InitializeDatabase();
        _context.SeedData();
        _repo = new ProjectRepo(_context);
    }

    [Fact]
    public void Create_ShouldAddNewProject()
    {
        // Arrange
        var project = new Project
        {
            Name = "New Test Project"
        };

        // Act
        _repo.Create(project);
        var allProjects = _repo.GetAll().ToList();

        // Assert
        Assert.Contains(allProjects, p => p.Name == "New Test Project");
    }

    [Fact]
    public void GetById_ShouldReturnCorrectProject()
    {
        // Arrange
        var project = new Project { Name = "Specific Project" };
        _repo.Create(project);
        var allProjects = _repo.GetAll().ToList();
        var createdId = allProjects.First(p => p.Name == "Specific Project").Id;

        // Act
        var result = _repo.GetById(createdId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Specific Project", result.Name);
    }

    [Fact]
    public void GetAll_ShouldReturnAllProjects()
    {
        // Act
        var projects = _repo.GetAll().ToList();

        // Assert
        Assert.NotNull(projects);
        Assert.True(projects.Count >= 1); // Should have at least seed data (Coding Tracker)
    }

    [Fact]
    public void Update_ShouldModifyExistingProject()
    {
        // Arrange
        var project = new Project { Name = "Original Project Name" };
        _repo.Create(project);
        var allProjects = _repo.GetAll().ToList();
        var createdProject = allProjects.First(p => p.Name == "Original Project Name");

        // Act
        createdProject.Name = "Updated Project Name";
        _repo.Update(createdProject);
        var updatedProject = _repo.GetById(createdProject.Id);

        // Assert
        Assert.NotNull(updatedProject);
        Assert.Equal("Updated Project Name", updatedProject.Name);
    }

    [Fact]
    public void Delete_ShouldRemoveProject()
    {
        // Arrange
        var project = new Project { Name = "Project To Delete" };
        _repo.Create(project);
        var allProjects = _repo.GetAll().ToList();
        var createdProject = allProjects.First(p => p.Name == "Project To Delete");
        var projectId = createdProject.Id;

        // Act
        _repo.Delete(projectId);
        var deletedProject = _repo.GetById(projectId);

        // Assert
        Assert.Null(deletedProject);
    }

    public void Dispose()
    {
        _context?.Dispose();
        if (File.Exists(_testDbPath))
            File.Delete(_testDbPath);
    }
}
