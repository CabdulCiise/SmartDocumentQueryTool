using API.Core.DTOs;
using API.Core.Models;
using API.Core.Services;
using API.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Web.Controllers;

public class FeedbackController(IFeedbackService feedbackService) : BaseApiController
{
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpGet]
    public List<Feedback> GetFeedbacks()
    {
        return feedbackService.GetFeedbacks(isArchived: false);
    }

    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpGet("Archived")]
    public List<Feedback> GetArchivedFeedbacks()
    {
        return feedbackService.GetFeedbacks(isArchived: true);
    }

    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPut("{feedbackId}/Archive")]
    public void ArchiveFeedback(int feedbackId)
    {
        feedbackService.ArchiveFeedback(feedbackId);
    }

    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpDelete("{feedbackId}")]
    public void DeleteFeedback(int feedbackId)
    {
        feedbackService.DeleteFeedback(feedbackId);
    }

    [HttpPost]
    public Feedback CreateFeedback(FeedbackDto feedback)
    {
        return feedbackService.CreateFeedback(feedback);
    }
}
