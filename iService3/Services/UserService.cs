using iService3.Models;
using iService3.Tools;
using Microsoft.Maui.ApplicationModel.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iService3.Services
{
    internal class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpConnectionServer _connection;
        public UserService()
        {
            _connection = new HttpConnectionServer();
            _httpClient = _connection.GetHttpClient();
        }
        //public async Task<List<User>> GetAllUsers()
        //{
        //    var response = await _httpClient.GetAsync($"api/User/GetUsers");
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return null;
        //    }
        //    var content = await response.Content.ReadAsStringAsync();
        //    var userList = JsonSerializer.Deserialize<List<User>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //    return userList;
        //}

        public async Task<User> GetUserById(int id)
        {
            var response = await _httpClient.GetAsync($"api/User/GetUserById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(responseString);
                return user;
            }
            else
            {
                throw new Exception($"Error getting user with id {id}: {response.ReasonPhrase}");
            }
        }

        //public async Task<List<Appointment>> GetUserAppointments(int id)
        //{
        //    var response = await _httpClient.GetAsync($"api/GetUserAppointments/{id}");
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return null;
        //    }
        //    var content = await response.Content.ReadAsStringAsync();
        //    var appointmentList = JsonSerializer.Deserialize<List<Appointment>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //    return appointmentList;
        //}

        public async Task<bool> Register(string username, string email, string password)
        {
            var registerModel = new
            {
                Username = username,
                Email = email,
                Pass = password
            };

            var json = JsonConvert.SerializeObject(registerModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/User/Register", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<User> Login(string username, string password)
        {
            const string url = "api/User/LogIn";

            var requestData = new
            {
                username = username,
                pass = password,
            };

            var jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode) return null;
            var responseContent = await response.Content.ReadAsStringAsync();

            var userObj = JsonConvert.DeserializeObject<User>(responseContent);

            return userObj;
        }


        public async Task<bool> Logout()
        {
            var response = await _httpClient.PostAsync($"api/User/LogOut", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/DeleteUser/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateNewsletterSub(int userId, bool newsletterSub)
        {
            var data = new
            {
                userId = userId,
                newsletterSub = newsletterSub
            };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/User/SetNewsletterSub/", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetPassword(int userId, string oldPassword, string newPassword)
        {
            var data = new
            {
                userId = userId,
                oldPassword = oldPassword,
                newPassword = newPassword
            };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/User/ResetPassword", content);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> SetFavouriteCar(int userId, int carId)
        {
            try
            {
                var favouriteCarModel = new 
                {
                    UserId = userId,
                    CarId = carId
                };

                var json = JsonConvert.SerializeObject(favouriteCarModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("api/User/SetFavouriteCar", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<int> GetFavoriteCarId(int userId)
        {
            var response = await _httpClient.GetAsync($"api/User/GetFavoriteCarId/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return int.Parse(content);
            }
            else
            {
                throw new Exception($"Error getting favorite car ID for user with ID {userId}: {response.ReasonPhrase}");
            }

        }
    }
}