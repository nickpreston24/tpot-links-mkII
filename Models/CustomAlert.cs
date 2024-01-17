using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TPOT_Links.Models;

public class CustomAlert
{
    private readonly IActionResult? render;
    private readonly PageModel parentPageModel;
    private readonly Exception? exception;

    public CustomAlert()
    {
    }


    public CustomAlert(PageModel parentPageModel
        , Exception? exception = null
        , string message = ""
        , string partial_name = "_Alert" // defaults to the global _Alert
    )
    {
        this.parentPageModel = parentPageModel;
        this.exception = exception;
        this.Message = !string.IsNullOrWhiteSpace(message) ? message : exception.ToString();
        if (null != exception)
        {
            this.AlertType = AlertType.Error;
            this.render = parentPageModel.Partial(partial_name, this);
        }
    }

    public string Message { get; set; } = string.Empty;
    public AlertType AlertType { get; set; } = AlertType.Success;

    public IActionResult Render() => render != null ? render : parentPageModel.Content(Message);
}

public static class CustomAlertExtensions
{
    public static IActionResult AsCustomAlert(this Exception ex, PageModel pm)
    {
        return new CustomAlert(pm, ex).Render();
    }
}
//
// public class CustomAlertRender : PartialViewResult
// {
//     public static explicit operator CustomAlertRender(CustomAlert customAlert)
//     {
//         return customAlert.Render();
//     }
// }