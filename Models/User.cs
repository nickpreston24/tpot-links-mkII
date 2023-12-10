using TPOT_Links.Models;

namespace TPOT_Links;

public class User
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string first_name { get; set; } = string.Empty;
    public string last_name { get; set; } = string.Empty;

    public int Age { get; set; } = -1;

    public bool IsOnline { get; set; } = false;
    public string FullName => $"{last_name}, {first_name}";

    public AvatarDetails Avatar = new AvatarDetails();
}