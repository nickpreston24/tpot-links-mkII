namespace TPOT_Links.Models;

public class User
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; } = -1;

    public string AvatarPic { get; set; } = string.Empty;
    
    public bool IsOnline { get; set; } = true;
}