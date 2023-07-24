using CodeMechanic.Airtable;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using CodeMechanic.Embeds;
using CodeMechanic.RazorPages;
using Neo4j.Driver;

namespace TPOT_Links.Pages.Comments
{
    public class IndexModel : HighSpeedPageModel
    {
        private static readonly RestClient restClient = new RestClient("https://jsonplaceholder.typicode.com/");

        public IndexModel(IEmbeddedResourceQuery embeddedResourceQuery
            , IAirtableRepo airtable
            , INeo4JRepo neorepo
        )
            : base(embeddedResourceQuery, airtable, neorepo)
        {
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetSearch(string keyword = "faith")
        {
            var url = $"https://tpot-api.vercel.app/api/pages?keyword={keyword}";

            var request = new RestRequest("todos");
            var comments = await restClient.GetAsync<List<Comment>>(request);

            // comments.Dump();

            return Content($"{comments.Count}".Tag("p"));
        }
    }
}