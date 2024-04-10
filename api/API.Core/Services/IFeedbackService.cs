using API.Core.DTOs;
using API.Core.Models;

namespace API.Core.Services;

public interface IFeedbackService
{
    List<Feedback> GetFeedbacks(bool isArchived);
    void ArchiveFeedback(int feedbackId);
    void DeleteFeedback(int feedbackId);
    Feedback CreateFeedback(FeedbackDto feedbackDto);
}
