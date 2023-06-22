using iService3.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using iService3.Tools;

namespace iService3.Services
{
    public class CarService
    {
        private HttpClient _httpClient;
        private HttpConnectionServer _connection;
        public CarService()
        {
            _connection = new HttpConnectionServer();
            _httpClient = _connection.GetHttpClient();
        }

        public async Task<List<Car>> GetCars()
        {
            //HttpResponseMessage response = await _httpClient.GetAsync("api/Car/GetCars");
            //response.EnsureSuccessStatusCode();

            //List<Car> cars = await response.Content.ReadAsAsync<List<Car>>();
            //return cars;

            var json = await _httpClient.GetStringAsync("api/Car/GetCars");
            var cars = JsonConvert.DeserializeObject<List<Car>>(json);
            return cars;
        }

        public async Task<Car> GetCarById(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Car/GetCarById/{id}");
            response.EnsureSuccessStatusCode();

            Car car = await response.Content.ReadAsAsync<Car>();
            return car;
        }

        public async Task<List<Car>> GetCarsByUserId(int userId)
        {

            var json = await _httpClient.GetStringAsync($"api/Car/GetCarsByUserId/{userId}");
            var cars = JsonConvert.DeserializeObject<List<Car>>(json);
            return cars;
        }

        public async Task<List<Car>> GetCarsByUserToken(string token)
        {
            var json = await _httpClient.GetStringAsync($"api/Car/GetCarsByUserToken/{token}");
            var cars = JsonConvert.DeserializeObject<List<Car>>(json);
            return cars;
        }
        public async Task<bool> AddCar(Car car)
        {
            var data = new
            {
                UserId = car.UserId,
                Manufacturer = car.Manufacturer,
                Model = car.Model,
                Year = car.Year
            };

            var jsonContent = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/Car/InsertCar", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task UpdateCar(int id, Car car)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/Car/UpdateCar/{id}", car);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCar(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Car/DeleteCar/{id}");
            response.EnsureSuccessStatusCode();
        }







        public async Task<string> GetCarImageUrl(string carName)
        {

            var content = await _httpClient.GetStringAsync($"api/CarExtra/GetCarLogo/{carName}");
            if(content == null )
                return null;
            return content.Trim('"');

        }
        public async Task<List<string>> GetCarManufacturers()
        {
            var response = await _httpClient.GetAsync("api/CarExtra/GetCarManufacturers");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var manufacturers = JsonConvert.DeserializeObject<List<string>>(json);
            return manufacturers;
        }

        public async Task<byte[]> GetImage(int CarId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/CarExtra/GetCarImageById/{CarId}");

                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    return imageBytes;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the image: {ex.Message}");
                return null;
            }
        }


        public async Task<List<string>> GetCarModels(string manufacturer)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/CarExtra/GetCarModels/{manufacturer}");

                if (response.IsSuccessStatusCode)
                {
                    var models = await response.Content.ReadFromJsonAsync<List<string>>();
                    return models;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving car models: {ex.Message}");
                return null;
            }
        }


    }
}
