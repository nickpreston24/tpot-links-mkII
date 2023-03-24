using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using CodeMechanic.Extensions;

namespace TPOT_Links.Pages.Comments
{
    public class IndexModel : PageModel
    {
        private static readonly RestClient restClient = new RestClient("https://jsonplaceholder.typicode.com/");

        public void OnGet()
        {
        }        

        public async Task<IActionResult> OnGetSearch(string keyword = "faith") {
            var url = $"https://tpot-api.vercel.app/api/pages?keyword={keyword}";

            var request = new RestRequest("todos");
            var comments = await restClient.GetAsync<List<Comment>>(request);

            comments.Dump();

            return  Content( $"{comments.Count}".Tag("p"));
        }
    }
}
