using API.Core.Models;
using API.Core.Services;
using API.Web.Controllers;
using Moq;

namespace API.Test.Integration.Controllers;

[TestFixture]
public class FeedbackControllerTests
{
    private FeedbackController feedbackController;
    private Mock<IFeedbackService> mockFeedbackService;

    public FeedbackControllerTests()
    {
        mockFeedbackService = new Mock<IFeedbackService>();
        feedbackController = new FeedbackController(mockFeedbackService.Object);
    }

    [Test]
    public void GetFeedbacks_CallGetFeedbacksInService()
    {
        var expectedFeedbacks = new List<Feedback>();
        mockFeedbackService.Setup(x => x.GetFeedbacks(false)).Returns(expectedFeedbacks);

        var result = feedbackController.GetFeedbacks();

        mockFeedbackService.Verify(x => x.GetFeedbacks(false), Times.Once);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<List<Feedback>>(result);
        Assert.That(result, Is.EqualTo(expectedFeedbacks));
    }

    [Test]
    public void GetArchivedFeedbacks_CallGetFeedbacksInService()
    {
        var expectedFeedbacks = new List<Feedback>();
        mockFeedbackService.Setup(x => x.GetFeedbacks(true)).Returns(expectedFeedbacks);

        var result = feedbackController.GetArchivedFeedbacks();

        mockFeedbackService.Verify(x => x.GetFeedbacks(true), Times.Once);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<List<Feedback>>(result);
        Assert.That(result, Is.EqualTo(expectedFeedbacks));
    }

    [Test]
    public void ArchiveFeedback_CallArchiveFeedbackInService()
    {
        int feedbackId = 123;
        feedbackController.ArchiveFeedback(feedbackId);

        mockFeedbackService.Verify(x => x.ArchiveFeedback(feedbackId), Times.Once);
    }

    [Test]
    public void DeleteFeedback_CallDeleteFeedbackInService()
    {
        int feedbackId = 123;
        feedbackController.DeleteFeedback(feedbackId);

        mockFeedbackService.Verify(x => x.DeleteFeedback(feedbackId), Times.Once);
    }
}