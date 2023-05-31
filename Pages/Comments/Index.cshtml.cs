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

        public IndexModel(IEmbeddedResourceQuery embeddedResourceQuery, IDriver driver = null,
            IAirtableRepo repo = null) : base(embeddedResourceQuery, driver, repo)
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