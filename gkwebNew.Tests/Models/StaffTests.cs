using gkweb.api.types.models;

namespace gkwebNew.Tests.Models;

public class StaffTests
{
    [Fact]
    public void Staff_DefaultConstructor_SetsDefaults()
    {
        var staff = new Staff();

        Assert.Equal(0, staff.Id);
        Assert.Null(staff.Title);
        Assert.Null(staff.Summary);
        Assert.Null(staff.ImageUrl);
    }

    [Fact]
    public void Staff_SetProperties_ReturnsExpectedValues()
    {
        var staff = new Staff
        {
            Id = 42,
            Title = "Senior Developer",
            Summary = "Experienced .NET engineer",
            ImageUrl = "/images/dev.png"
        };

        Assert.Equal(42, staff.Id);
        Assert.Equal("Senior Developer", staff.Title);
        Assert.Equal("Experienced .NET engineer", staff.Summary);
        Assert.Equal("/images/dev.png", staff.ImageUrl);
    }

    [Fact]
    public void Staff_NullableProperties_AcceptNull()
    {
        var staff = new Staff
        {
            Id = 1,
            Title = "Test",
            Summary = "Test",
            ImageUrl = "test.png"
        };

        staff.Title = null;
        staff.Summary = null;
        staff.ImageUrl = null;

        Assert.Null(staff.Title);
        Assert.Null(staff.Summary);
        Assert.Null(staff.ImageUrl);
    }
}
