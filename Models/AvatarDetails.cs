using Insight.Database.MissingExtensions;
using NSpecifications;

namespace TPOT_Links.Models;

public class AvatarDetails
{
    public object CheckOnlineStatus = new Spec<User>(u => u.IsOnline && u.last_name.IsNullOrWhiteSpace() );
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
