using Microsoft.AspNetCore.Html;

namespace TPOT_Links;

public class AlpineModalOptions
{
    // private string showModal;
    // private string showModalOnInit;

    public HtmlString EnableModal()
    {
        // show_modal = false.AsJS();
        string script = @"<script>console.log('hello from alpine modal return script!')<script>";
        // return Content();
        return script.AsHTMLString();
    }

    public bool show_modal
    {
        get;
        set;
        // set => showModal = value.AsJS();
        // get => showModal.AsJS();
    }

    public bool show_modal_on_init
    {
        get;
        set;
        // set => showModalOnInit = value.AsJS();
        // get => showModalOnInit.AsJS();
    }
}