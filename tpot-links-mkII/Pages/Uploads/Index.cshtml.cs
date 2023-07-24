using Microsoft.AspNetCore.Mvc.RazorPages;

/**
   To Try FROM:
    
    - [ ] https://www.aspsnippets.com/Articles/ASPNet-Core-Razor-Pages-Display-Word-files-from-Database-in-View.aspx

    - [ ] https://www.aspsnippets.com/Articles/Convert-Office-Word-Document-to-HTML-and-display-it-in-browser-in-ASP.Net.aspx

    - [ ] https://github.com/dotnet/Open-XML-SDK

    - [ ] https://www.e-iceblue.com/Tutorials/Spire.Doc/Spire.Doc-Program-Guide/Word-to-HTML-Convert-Word-to-HTML-with-C-VB.NET.html

    - [ ] https://www.syncfusion.com/forums/177377/convert-word-document-to-html-string

    - [ ] https://docs.devexpress.com/OfficeFileAPI/116818/word-processing-document-api/examples/document-conversion/how-to-convert-a-docx-document-to-html-format

    SERVER SIDE:

    - [ ] https://github.com/OfficeDev/Open-Xml-PowerTools/blob/vNext/OpenXmlPowerToolsExamples/HtmlConverter01/HtmlConverter01.cs

    GOOD UI SIDE SUGGESTIONS:

    - [ ] https://khalidabuhakmeh.com/multiple-file-drag-and-drop-upload-with-aspnet-core

*/
namespace TPOT_Links.Pages.Uploads
{
    public class IndexModel : PageModel
    {

        // public List<FileModel> Files { get; set; }
        // private IConfiguration Configuration;
    
        // public IndexModel(IConfiguration _configuration)
        // {
        //     Configuration = _configuration;
        // }
    
        // public void OnGet()
        // {
        //     this.Files = this.GetFiles();
        // }
    
        // public IActionResult OnPostUploadFile(IFormFile postedFile)
        // {
        //     string fileName = Path.GetFileName(postedFile.FileName);
        //     string contentType = postedFile.ContentType;
        //     using (MemoryStream ms = new MemoryStream())
        //     {
        //         postedFile.CopyTo(ms);
        //         string constr = this.Configuration.GetConnectionString("MyConn");
        //         using (SqlConnection con = new SqlConnection(constr))
        //         {
        //             string query = "INSERT INTO tblFiles VALUES (@Name, @ContentType, @Data)";
        //             using (SqlCommand cmd = new SqlCommand(query))
        //             {
        //                 cmd.Connection = con;
        //                 cmd.Parameters.AddWithValue("@Name", fileName);
        //                 cmd.Parameters.AddWithValue("@ContentType", contentType);
        //                 cmd.Parameters.AddWithValue("@Data", ms.ToArray());
        //                 con.Open();
        //                 cmd.ExecuteNonQuery();
        //                 con.Close();
        //             }
        //         }
        //     }
    
        //     return RedirectToPage("Index");
        // }
    
        // public JsonResult OnPostGetWordDocument(int fileId)
        // {
        //     byte[] bytes;
        //     string fileName, contentType;
        //     string constr = this.Configuration.GetConnectionString("MyConn");
        //     using (SqlConnection con = new SqlConnection(constr))
        //     {
        //         using (SqlCommand cmd = new SqlCommand())
        //         {
        //             cmd.CommandText = "SELECT Name, Data, ContentType FROM tblFiles WHERE Id=@Id";
        //             cmd.Parameters.AddWithValue("@Id", fileId);
        //             cmd.Connection = con;
        //             con.Open();
        //             using (SqlDataReader sdr = cmd.ExecuteReader())
        //             {
        //                 sdr.Read();
        //                 bytes = (byte[])sdr["Data"];
        //                 contentType = sdr["ContentType"].ToString();
        //                 fileName = sdr["Name"].ToString();
        //             }
        //             con.Close();
        //         }
        //     }
    
        //     return new JsonResult(new { FileName = fileName, ContentType = contentType, Data = bytes });
        // }
    
        // private List<FileModel> GetFiles()
        // {
        //     List<FileModel> files = new List<FileModel>();
        //     string constr = this.Configuration.GetConnectionString("MyConn");
        //     using (SqlConnection con = new SqlConnection(constr))
        //     {
        //         using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM tblFiles"))
        //         {
        //             cmd.Connection = con;
        //             con.Open();
        //             using (SqlDataReader sdr = cmd.ExecuteReader())
        //             {
        //                 while (sdr.Read())
        //                 {
        //                     files.Add(new FileModel
        //                     {
        //                         Id = Convert.ToInt32(sdr["Id"]),
        //                         Name = sdr["Name"].ToString()
        //                     });
        //                 }
        //             }
        //             con.Close();
        //         }
        //     }
        //     return files;
        // }
    }
}
