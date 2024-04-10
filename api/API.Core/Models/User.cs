using API.Core.Constants;

namespace API.Core.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string CustomInstruction { get; set; }
    public DateTime CreatedAt { get; set; }
    public int RoleId { get; set; }

    public Role Role { get; set; }
    public List<Chat> Chats { get; set; }
    public List<Feedback> Feedbacks { get; set; }
    public List<Document> Documents { get; set; }

    public DateTime? LastChatDate => Chats.OrderByDescending(x => x.CreatedAt).FirstOrDefault()?.CreatedAt ?? null;
    public DateTime? LastUploadedDate => Documents.OrderByDescending(x => x.UploadedAt).FirstOrDefault()?.UploadedAt ?? null;
    public double TotalUploadCost => Documents.Sum(x => x.Cost);
    public double TotalChatCost => Chats.SelectMany(x => x.Messages).Sum(x => x.Cost ?? 0);
    public int NumberOfChats => Chats.Count;
    public int NumberOfFeedbacks => Feedbacks.Count;
    public int NumberOfDocuments => Documents.Count;
    public bool IsAdmin => Role?.Name == Roles.Admin;
}
