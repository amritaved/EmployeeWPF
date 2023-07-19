using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EmployeeWPF.Model;
using EmployeeWPF.Common;
using System.Text.Json;
using System.IO;

namespace EmployeeWPF
{
    public class RestAPI
    {

        private static HttpClient _httpClient;

        public static async Task<HttpResponseMessage> GetUsers(string url)
        {
            try
            {
                _httpClient = new HttpClient();
                url = (url == null) ? ConstantStrings.apiUrl : url;
                var response = await _httpClient.GetAsync(url);
                string content = await response.Content.ReadAsStringAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<HttpResponseMessage> CreateUser(Employee model)
        {
            try
            {
                var employee = JsonSerializer.Serialize(model);
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ConstantStrings.apiToken}");
                var requestContent = new StringContent(employee, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ConstantStrings.apiUrl, requestContent);
                var content = await response.Content.ReadAsStringAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<HttpResponseMessage> DeleteUser(Employee employee)
        {
            try
            {
                var uri = Path.Combine(ConstantStrings.apiUrl, employee.id.ToString());
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ConstantStrings.apiToken}");
                var response = await _httpClient.DeleteAsync(uri);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public static async Task<HttpResponseMessage> UpdateUser(Employee employee)
        {
            try
            {
                var company = JsonSerializer.Serialize(employee);
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ConstantStrings.apiToken}");
                var requestContent = new StringContent(company, Encoding.UTF8, "application/json");
                var uri = Path.Combine(ConstantStrings.apiUrl, employee.id.ToString());
                var response = await _httpClient.PutAsync(uri, requestContent);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
