using gkweb.api.types.models;

namespace gkwebNew.Tests.Models;

public class ContactSubmissionTests
{
    [Fact]
    public void ContactSubmission_DefaultConstructor_SetsDefaults()
    {
        var submission = new ContactSubmission();

        Assert.Equal(0, submission.Id);
        Assert.Equal(string.Empty, submission.Name);
        Assert.Equal(string.Empty, submission.Email);
        Assert.Null(submission.Phone);
        Assert.Equal(string.Empty, submission.Message);
        Assert.Null(submission.ViewedAt);
    }

    [Fact]
    public void ContactSubmission_SubmittedAt_DefaultsToUtcNow()
    {
        var before = DateTime.UtcNow;
        var submission = new ContactSubmission();
        var after = DateTime.UtcNow;

        Assert.InRange(submission.SubmittedAt, before, after);
    }

    [Fact]
    public void ContactSubmission_SetProperties_ReturnsExpectedValues()
    {
        var viewedAt = DateTime.UtcNow;
        var submission = new ContactSubmission
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            Phone = "555-1234",
            Message = "Hello",
            ViewedAt = viewedAt
        };

        Assert.Equal(1, submission.Id);
        Assert.Equal("Test User", submission.Name);
        Assert.Equal("test@example.com", submission.Email);
        Assert.Equal("555-1234", submission.Phone);
        Assert.Equal("Hello", submission.Message);
        Assert.Equal(viewedAt, submission.ViewedAt);
    }

    [Fact]
    public void ContactSubmission_Phone_AcceptsNull()
    {
        var submission = new ContactSubmission { Phone = "555-1234" };
        submission.Phone = null;
        Assert.Null(submission.Phone);
    }

    [Fact]
    public void ContactSubmission_ViewedAt_AcceptsNull()
    {
        var submission = new ContactSubmission { ViewedAt = DateTime.UtcNow };
        submission.ViewedAt = null;
        Assert.Null(submission.ViewedAt);
    }
}
