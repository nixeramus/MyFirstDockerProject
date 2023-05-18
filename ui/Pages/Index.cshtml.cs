using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myWebApp.Models;
using myWebApp.Reciever;

namespace myWebApp.Pages;

public class IndexModel : PageModel
    {
        public string StudentName { get; private set; } = "PageModel in C#";

         public List<Student> Students { get; private set; } 
        private readonly ILogger<IndexModel> _logger;
        

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
          
        }

        public async Task<IActionResult> OnGetAsync()
        {
           // string apiUrl = "http://api:80"; // Замените на URL вашего API

            string apiUrl = " http://localhost:5001"; // Замените на URL вашего API
          
            var studentRepository = new StudentRepository(apiUrl);

            // Ваш код обработки страницы

            List<Student> students = await studentRepository.GetStudentsAsync();
            // Дальнейший код обработки полученных студентов
            Students= students;
            return Page();
        }


        
    }