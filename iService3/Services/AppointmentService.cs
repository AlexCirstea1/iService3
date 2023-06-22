using iService3.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iService3.Models;
using iService3.Views;
using Service = iService3.Models.Service;

namespace iService3.Services
{
    class AppointmentService
    {
        private HttpClient _httpClient;
        private HttpConnectionServer _connection;
        public AppointmentService()
        {
            _connection = new HttpConnectionServer();
            _httpClient = _connection.GetHttpClient();
        }

        public async Task<List<Appointment>> GetAppointmentsByUserId(int userId)
        {
            var json = await _httpClient.GetStringAsync($"api/Appointment/GetAppointmentsByUserId/{userId}");
            var appointments = JsonConvert.DeserializeObject<List<Appointment>>(json);
            return appointments;
        }

        public async Task<bool> ScheduleAppointment(Appointment appointment)
        {
            var requestData = new
            {
                carId = appointment.CarId,
                userId = appointment.UserId,
                appointmentDate = appointment.AppointmentDate,
                appointmentType = appointment.AppointmentType,
                appointmentNotes = appointment.AppointmentNotes,
                serviceId = appointment.ServiceId
            };

            var jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync("api/Appointment/ScheduleAppointment", content);

            return true;
        }

        public async Task<List<Service>> GetServices()
        {
            var json = await _httpClient.GetStringAsync("api/Service/GetServices");
            var services = JsonConvert.DeserializeObject<List<Service>>(json);
            return services;
        }

        public async Task<List<DateTime>> GetAppointmentsDates()
        {
            var response = await _httpClient.GetAsync("api/Appointment/Appointments/Dates");
            response.EnsureSuccessStatusCode();
            var appointmentDates = await response.Content.ReadAsAsync<List<DateTime>>();
            return appointmentDates;
        }

        public async Task<List<DateTime>> GetAppointmentsDatesByServiceId(int serviceId)
        {
            var response = await _httpClient.GetAsync($"api/Appointment/Appointments/Dates/{serviceId}");
            response.EnsureSuccessStatusCode();
            var appointmentDates = await response.Content.ReadAsAsync<List<DateTime>>();
            return appointmentDates;
        }

        public async Task<byte[]> GetImage(int serviceId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Service/GetServiceLogoById/{serviceId}");

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
    }
}
