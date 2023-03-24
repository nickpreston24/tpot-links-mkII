using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using TPOT_Links;

public class ScrapingBeeRepo {

    protected string base_url = string.Empty;
    protected string api_key = string.Empty;

    public ScrapingBeeRepo(string api_key) {

        this.api_key = api_key;
        this.base_url = $"https://app.scrapingbee.com/api/v1/?api_key={api_key}";
        
        /// &url=https://www.facebook.com/stephanie.reinke.92/posts/pfbid02Q9rYwrWFvme7e7VRXU1waKewW8VzUDAPCgi6k7q83yFzDsYvYYDpNKfHnn2EBHXl";


        // https://www.facebook.com/stephanie.reinke.92/posts/pfbid02Q9rYwrWFvme7e7VRXU1waKewW8VzUDAPCgi6k7q83yFzDsYvYYDpNKfHnn2EBHXl

        // declare @username = ''
        // coalesce(@username ,.... '%');
        //string sql = "SELECT user_name, post_id, comment FROM Comments WHERE user_name like @username && post_id = @postid";

    }

    public async Task<List<Comment>?> GetAllcomments()
    {
        // var request = new RestRequest(this.base_url);
        // var comments = await restClient.GetAsync<List<Comment>>(request);
        // return comments;

        return Enumerable.Empty<Comment>().ToList();
    }
}

public class Comment 
{
    public string user_name { get; set; }= "stephanie.reinke.92";
    public string post_id { get; set; }= "pfbid02Q9rYwrWFvme7e7VRXU1waKewW8VzUDAPCgi6k7q83yFzDsYvYYDpNKfHnn2EBHXl";
}