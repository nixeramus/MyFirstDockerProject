using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myWebApp.Models;
using myWebApp.Reciever;
using Microsoft.Extensions.Configuration;

namespace myWebApp.Pages;

public class IndexModel : PageModel
    {

         public List<Student> Students { get; private set; } 
        private readonly ILogger<IndexModel> _logger;
        

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
          
        }

        public async Task<IActionResult> OnGetAsync()
        {

            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            string apiUrl = config["ApiSettings:ApiUrl"];
          
            var studentRepository = new StudentRepository(apiUrl);

            // Ваш код обработки страницы

            List<Student> students = await studentRepository.GetStudentsAsync();
            // Дальнейший код обработки полученных студентов
            Students= students;
            return Page();
        }


        
    }