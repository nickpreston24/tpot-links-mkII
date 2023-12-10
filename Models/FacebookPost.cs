using CodeMechanic.Advanced.Regex;
using CodeMechanic.Diagnostics;
using CodeMechanic.Extensions;

namespace tpot_links_seeder;

public class FacebookPost
{
    public static FacebookPost CreateInstance(string url = "")
    {
        // url.Dump();
        FacebookPostPattern.Dump("PATTERN");
        var instance = url.Extract<FacebookPost>(FacebookPostPattern)
            .SingleOrDefault()
            .Dump("fb post");
        return instance;
    }

    // https://regex101.com/r/vV12pT/1
    public const string FacebookPostPattern =
        @"^https?:\/\/www\.facebook\.com\/(?<Name>\w+)(\/posts\/)(?<post_id>[\w\d]+)\?(?<comment_ids>.*)";

    public string OutputPath => ToString();

    public string Extension { get; set; } = ".png";
    public string comment_ids { get; set; } = string.Empty;
    public string post_id { get; set; } = string.Empty;
    public string Name { get; set; }

    // https://www.facebook.com/officialbenshapiro/posts/pfbid0235H6HMsAdpGqULNzW4okjNxc5M31Fr6oof51GusMhMEtHq5tGMGoYdamG1JtHgbwl?comment_id=187601347521345&reply_comment_id=153781794119258&notif_id=1683167179141365&notif_t=comment_mention&ref=notif

    public override string ToString()
    {
        return $"{Name}_{post_id}.{Extension ?? ".png"}";
    }
}