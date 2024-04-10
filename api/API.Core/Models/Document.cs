namespace API.Core.Models;

public class Document
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Cost { get; set; }
    public TimeSpan ProcessTime { get; set; }
    public DateTime UploadedAt { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }
}
