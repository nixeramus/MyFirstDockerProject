using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using myWebApp.Models;

namespace myWebApp.Reciever
{
    public class StudentRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public StudentRepository(string apiUrl)
        {
            _apiUrl = apiUrl;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(apiUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            var response = await _httpClient.GetAsync("api/students");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var students = JsonSerializer.Deserialize<List<Student>>(responseBody);
            return students;
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/students/{id}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var student = JsonSerializer.Deserialize<Student>(responseBody);
            return student;
        }

        public async Task<bool> CreateStudentAsync(Student student)
        {
            var json = JsonSerializer.Serialize(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/students", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateStudentAsync(int id, Student student)
        {
            var json = JsonSerializer.Serialize(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/students/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/students/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
