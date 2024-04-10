using API.Core.DTOs;
using API.Core.Models;
using API.Core.Services;
using API.Data.Contexts;
using AutoMapper;

namespace API.Infrastructure.Services;

public class FeedbackService(ApiContext apiContext, IMapper mapper) : IFeedbackService
{
    public List<Feedback> GetFeedbacks(bool isArchived)
    {
        return mapper.Map<List<Feedback>>(apiContext.Feedbacks
            .Where(x => x.IsArchived == isArchived)
            .OrderByDescending(x => x.CreatedAt)
            .ToList());
    }

    public void ArchiveFeedback(int feedbackId)
    {
        var entity = apiContext.Feedbacks.FirstOrDefault(x => x.Id == feedbackId);

        if (entity != null)
        {
            entity.IsArchived = true;
            apiContext.SaveChanges();
        }
    }

    public void DeleteFeedback(int feedbackId)
    {
        var entity = apiContext.Feedbacks.FirstOrDefault(x => x.Id == feedbackId);

        if (entity != null)
        {
            apiContext.Feedbacks.Remove(entity);
            apiContext.SaveChanges();
        }
    }

    public Feedback CreateFeedback(FeedbackDto feedbackDto)
    {
        var entity = new Data.Entities.Feedback
        {
            IsArchived = false,
            Message = feedbackDto.Message,
            UserId = feedbackDto.UserId,
            CreatedAt = DateTime.Now,
        };

        apiContext.Feedbacks.Add(entity);
        apiContext.SaveChanges();

        return mapper.Map<Feedback>(entity);
    }
}
