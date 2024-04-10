﻿namespace API.Core.Models;

public class Feedback
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsArchived { get; set; }
    public int UserId { get; set; }

    public User? User { get; set; }
}
