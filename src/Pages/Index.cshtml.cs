using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myWebApp.Models;

namespace myWebApp.Pages;

public class IndexModel : PageModel
    {
        public string StudentName { get; private set; } = "PageModel in C#";
        private readonly ILogger<IndexModel> _logger;
        

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
          
        }

        public void OnGet()
        {
            //var s =_context.Students?.Where(d=>d.ID==1).FirstOrDefault();
            var s=new Student();
            this.StudentName = $"{s?.FirstMidName} {s?.LastName}";
        }
    }