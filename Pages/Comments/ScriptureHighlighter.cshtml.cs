using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CodeMechanic.Extensions;
using CodeMechanic.Advanced.Extensions;
using Neo4j.Driver;
using CodeMechanic.RazorHAT;
using TPOT_Links.Models;
using CodeMechanic.Embeds;


namespace TPOT_Links.Pages.Comments;

public class ScriptureHighlighterModel : HighSpeedPageModel
{
    // 'static' allows us to holds the value between Crud calls.  We can do it with many different data types, too.
    private static int count = 0;
    public int Count => count; // making a non-static, public Count variable exposes the cahced value to our view.  Not many people use this, sadly.  It's great for reducing the number of calls to the server for data.

    public ScriptureHighlighterModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver) 
    : base(embeddedResourceQuery, driver)
    {
    }

    public void OnGet(
      string sort_direction="..."
      , int max_lines = 3)  // sample url parameters
    {
        // reset on refresh.  
        // Great for times when we want to just send url params in, without cached values.
        count = 0;
    }
    
    public async Task<IActionResult> OnPostExtractedScriptures(bool dev = false)
    {
      var failure_message = Content(
            $"""
            <div class='alert alert-error'>
              <p class='text-xl text-white text-sh'>An Error Occurred... But fret not! Our team of intelligent lab mice are on the
                job!</p>
            </div>
            """);

      string [] all_sample_text = sample_comments.Concat(sample_teachings).ToArray();

      (List<Scripture> prefixed
      , List<Scripture> postfixed
      , List<Scripture> full
      ) = new ScriptureParser(all_sample_text);

      var extracted_scriptures_list = 
        prefixed.Dump("prefixed")
        // .Concat(postfixed.Dump("postfixed"))
        .Concat(full.Dump("full"))
         // joins N array of strings into a single array
        .Select(scripture=> $"""
              <div class="card w-96 bg-base-100 shadow-xl">
                <div class="card-body">
                  <h2 class="card-title">{scripture.Name}</h2>
                  <p>{scripture.Text}</p>
                </div>
              </div>
        """)  // each scripture is now templated as a daisyui card.
        .FlattenText();

      var regex_button = $"""
        <a href="https://regex101.com/r/HiM1uO/1">
          <button class="btn btn-primary">See the Regex</button>
        </a>
      """;

      var result = Content($"""
        <div class="flex flex-col items-center justify-center">
          <div class="flex flex-center hero min-h-screen bg-base-200">
            <div class="hero-content flex-col lg:flex-row-reverse">
              <div class="flex flex-col items-center justify-center gap-2">
                <h1 class="text-5xl font-bold">Scripture Parsing</h1>
                <div x-show='false' class="flex flex-row items-center justify-center gap-2">
                  <div class="bg-secondary text-white-content mockup-code w-3/4">
                    <pre x-show="debug">
                        <template x-for="pattern in patterns">
                          <pre data-prefix="$">
                            <code x-text="pattern">
                            </code>
                          </pre> 
                      </template>
                    </pre>
                  </div>
                </div>

                <div class="text-gold flex flex-col gap-4 items-center justify-center">

                  <div class='grid md:grid-cols-2 gap-2'>
                    {extracted_scriptures_list}
                  </div>
                </div>

                <pre>Devmode? {dev}</pre>

                <h2 class="text-4xl flex flex-col">
                  Comments Discovered
                  {regex_button}
                </h2>

                <div class="w-72 flex flex-col items-center justify-center gap-2">
                  <template x-for="comment in comment_highlights">
                    <h2 x-text="comment"></h2>
                  </template>
                </div>
              </div>
            </div>
          </div>
        </div>
      """);
       
       return result;
    }


    private static string[] sample_comments = new string [] {"Yes                        Victor Hafichuk . You speak The Truth!\n2 Corinthians 6:16 comes to mind! You must leave, separate yourselves from them, have nothing to do with what is unclean!", 
    
    "Embrace the Truth with your whole being. Romans 12:1,2 and Matthew 5:48. Flee all forms of religion, which exercises itself in every area of life - churchdom, government, medicine, entertainment, fashion, commerce, agriculture, food-production, processing, adulteration, and distribution.\n\nHebrews 4:12 - divide the good from the evil. Heed the Word of God. When we allow any pollution and corruption, we invite suffering, loss, and death. When we receive the Truth, we invite victory, security, healing, prosperity, peace, life, and joy. That's how it is. \n\nWhich do we choose and stand with - the prince of this world or the Prince of peace? The destroyer with all his machinations or the Savior in all His Glory? The one who seeks to deceive, steal, kill, and destroy us in all his devious, fleshly, and worldly ways, or the One Who laid His Life down for us so that we might, from the beginning to the end, have His Favor with Everlasting Life, Wisdom, and Power?\n\nIt doesn't take two brains to know the answer. In fact, we don't want two brains - it's called \"double-mindedness.\" One brain will do and only One, the Mind of Christ, our Creator and Lover. Amen?" };

    private static string [] sample_teachings = new string [] {@"
      Spiritual faith is a gift from above, from the unseen realm. It is not something one produces or performs. It has nothing to do with willpower or concentration or intellectual belief. It has nothing to do with doctrine or church, denominational, or religious affiliation.

Faith is something coming to, and not from, a person. It is a happening, an impartation, an influence, a blessing. No man can make or produce faith, and he cannot add or subtract from its influence.

Faith is a connection, a contact with the unseen realm, originating from without the person experiencing the faith. One does not come upon or into faith; rather, faith comes upon that one and immerses, includes, or involves him in the unseen realm.

Spiritual faith is the knowledge of God and His ways. It is a peek at, or the passive, unintentional observation of, or being partially enveloped by, or being brought in tune with, the heavenly dimension, the spiritual realm.

True faith connects with the Creator Himself and learns His Lordship over all things of all worlds, not only in the future, but in the present.

Faith has nothing to do with physical, emotional, or mental feelings. It is higher than intuition, instinct, gut feeling, or the sixth sense. Faith is independent of sensing in any way, even as a mist may settle on an area without the land’s doing or invitation. Faith is something that happens to one; it is never something one does or even indirectly provokes.

When one goes by, or acts on, or exercises, faith, it simply means that one reacts positively, receptively to what he or she is already freely receiving. There is no effort with faith. Faith is the inspiration, the fuel, with its effect on the person, as, for example, when one may act under hypnosis. The subject does not try to do things so much as yields to what is happening to him, responding to an external influence made internal.

Faith is an internalizing of the Kingdom of God, whether momentarily or constantly.

Faith has nothing to do with reason and intellectual comprehension. Faith works by revelation and inspiration, much as younger children conduct themselves. They may not understand from externals, but they are able to pick up or know things internally, beyond explanation or ability to understand intellectually.

Faith is not something one does, but what happens to one.

The more one grows in age and experience in the world, the less will be seen the trust, innocence, rest, and freedom that is obvious in youth. The older one grows, the more one sees reasoning kick in, and judging after the outward appearance. The senses and carnal knowledge are at enmity with faith, because they deal in terms of the outward, that which is understandable by the human mind and observable by the carnal senses. Faith works completely independently and beyond these.

Watch the one who has faith. There is an invisible shining, an independence of accomplishable knowledge and physical presence of anything. Faith is given, never acquired or earned or even cultivated when received. If one believes (or “faiths”) and acts on that faith or belief, more will be given that one. If one focuses primarily on the outward and on that which is present in time and space, one loses touch with the invisible realm; one loses that connection that can only exist through faith.

Without faith it is impossible to please or see God or to recognize those who are His or that which He does. God hides Himself, and only the eye of faith can see Him; only the heart responding to faith can comprehend the things of God.

Men may reason and impress other men intellectually, even great multitudes, by theories, doctrines, convictions, and letter-perfect truths, but without faith, to the saints they are only chattering know-it-alls. They may be entertaining, clever, educated, knowledgeable, impressive, expressive, persuasive, logical, rational, eloquent, and able to educate and convince their hearers intellectually and emotionally, but they will not bear witness to faith or to that which only faith can impart or reveal.

Yet faith is what saves. Faith is what man needs; he cannot live by bread alone. He must live by the provision of the Heavens. Roots absorb necessary moisture and nutrients from the soil, without which the plant may die, but unless that plant grows greenery above the roots and is exposed to the air, sunshine, and all the elements present only above ground, that plant will likely perish. We must have the food of earth, yes, as God the Creator of all things in Heaven and earth has provided, but woe to the man who has no faith.

Faith is foolishness to the carnal man. Those who are subject to their carnal senses scorn the things of the spiritual realm; how much more do they scorn the Invisible God, Who is Spirit and the Father of spirits?

Faith is the umbilical cord of spiritual life. The fetus does not arrange for the cord; neither does the fetus maintain or exercise any control over the cord. The cord is simply there, furnishing life and sustenance, and the fetus, in its innocence and helplessness, receives and is fed. It is provided for. Faith provides. Does the fetus suck or hold its breath to feed, or wish or intend or will to feed? No, it only receives. So it is with faith. God gives; one trusts without thought, receives, values, and makes use of what God gives, though what is seen and received by faith contradicts what is seen in the visible realm.

Faith is placing more value in the unseen than in what is seen. To place more value in the seen than the unseen is death and hell, but to place more value in the unseen is life, peace, and victory.

By faith, nothing is impossible. The dimension of this world and its natural laws are entirely subject to the Kingdom of Heaven. The acceptance, appreciation, embracing, and exercise of faith render this dimension subject to the Kingdom, where God lives and reigns. Therefore, faith pleases God.

“But without faith it is impossible to please Him, for he who comes to God must believe that He is and that He is a rewarder of those who diligently seek Him” (Hebrews 11:6 MKJV).

All believers, be they prophets, apostles, men, women, or little children, are sons of faith. The first apostles appreciated and coveted faith. By it, they healed the sick and raised the dead without medicine and cast out devils (spirits of the spiritual realm) without psychology or intellectual power or persuasion. They did the impossible.

By faith, they followed and served Jesus (which is impossible without faith). By faith, Peter knew the unknown and declared that Jesus was the Christ, the Son of the Living God. He did not learn and confess this by reasoning or indoctrination; it was given to him.

Jesus said to him: “You are blessed, Simon, son of Jonah, for flesh and blood did not reveal it to you, but My Father in Heaven” (Matthew 16:17 MKJV).

The disciples wanted more of that precious, exciting, living, invincible, supernatural, powerful, invigorating, illuminating, world-revolutionizing gift of God called faith. They knew they could not add to it qualitatively or quantitatively, but they knew its Source and came asking:

Luke 17:5-10 MKJV
(5) And the apostles said to the Lord, Give us more faith.
(6) And the Lord said, If you had faith as a grain of mustard seed, you might say to this sycamine tree, Be rooted up and be planted in the sea! And it would obey you.
(7) But which of you who has a servant plowing or feeding will say to him immediately after he has come from the field, Come, recline?
(8) Will he not say to him, Prepare something so that I may eat, and gird yourself and serve me until I eat and drink. And afterward you shall eat and drink.
(9) Does he thank that servant because he did the things that were commanded him? I think not.
(10) So likewise you, when you shall have done all the things commanded you, say, We are unprofitable servants, for we have done what we ought to do.

In essence, the Lord told them they already had available all they needed. He also taught them that faith was recognizing and walking in dependence of the virtue, power, and goodwill of God, with full recognition that they earned, deserved, and could do nothing worthy to merit faith or God’s recognition and grace, even if they fruitfully exercised faith. There was to be no expectation of pay or gratitude, because it was not their faith or their virtue in any way:

“So likewise you, when you shall have done all the things commanded you, say, We are unprofitable servants, for we have done what we ought to do.”

Those who walk in faith are doing the will of God – it is Him at work.

Faith is the absence of self-trust and dependence; it is trust in God, which comes from God. That is faith. Only God can make it happen. Faith is a gift of God.

Victor Hafichuk
    "};

}


