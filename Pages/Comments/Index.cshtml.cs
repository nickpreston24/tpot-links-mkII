using Microsoft.AspNetCore.Mvc;
using RestSharp;
using CodeMechanic.Embeds;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4j.Driver;

namespace TPOT_Links.Pages.Comments
{
    public class IndexModel : PageModel
    {
        private readonly IEmbeddedResourceQuery embeddedResourceQuery;
        private readonly IDriver driver;
        private readonly IAirtableRepo airtable_repo;
        private static readonly RestClient restClient = new RestClient("https://jsonplaceholder.typicode.com/");

        public IndexModel(
            IEmbeddedResourceQuery embeddedResourceQuery
            , IDriver driver = null
            , IAirtableRepo airtableRepo = null
        )
        {
            this.embeddedResourceQuery = embeddedResourceQuery;
            this.driver = driver;
            this.airtable_repo = airtableRepo;
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