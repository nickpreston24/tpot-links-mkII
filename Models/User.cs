namespace TPOT_Links.Models;

public class User
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; } = -1;

    public AvatarDetails Avatar = new AvatarDetails();

    public bool IsOnline { get; set; } = true;
}

public class AvatarDetails
{
    public string Image { get; set; } = "/img/my_fb_avatar.jpg";

    public bool avatar_exists => !string.IsNullOrWhiteSpace(Image);

    public string avatar_status => avatar_exists ? " online " : " placeholder offline ";

    public string avatar_full_css => $"{avatar_status} avatar";


    public void Deconstruct(out bool exists, out string status, out string css_class, out string image)
    {
        status = avatar_status;
        exists = avatar_exists;
        css_class = avatar_full_css;
        image = Image;
    }
}