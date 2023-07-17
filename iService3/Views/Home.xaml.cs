using iService3.Services;
using iService3.Models;
using iService3.Tools;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;

namespace iService3.Views
{
    public partial class Home : ContentPage
    {
        private readonly SecureStorageToolKit _storageToolKit;
        private readonly CarService _carService;
        private readonly UserService _userService;
        private readonly AppointmentService _appointmentService;
        private Image _carImage;

        public Home()
        {
            _appointmentService = new AppointmentService();
            _userService = new UserService();
            _carService = new CarService();
            _storageToolKit = new SecureStorageToolKit();
            InitializeComponent();
            _carImage = new Image();
            //LoadImage();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            GreetingMessage();
            await LoadInformation();
            LoadImage();
        }

        private async Task LoadInformation()
        {
            var userId = int.Parse(await _storageToolKit.GetUserID());
            var appointments = await _appointmentService.GetAppointmentsByUserId(userId);
            var favoriteCarId = await _userService.GetFavoriteCarId(userId);

            var carAppointments = appointments.Where(c => c.CarId == favoriteCarId);
            var nextAppointment = carAppointments.OrderBy(c => c.AppointmentDate).FirstOrDefault(c => c.AppointmentDate.Date > DateTime.Now);

            if (nextAppointment != null)
            {
                ManufacturerLabel.Text = nextAppointment.Car.Manufacturer;
                ModelLabel.Text = nextAppointment.Car.Model;
                NextAppointmentLabel.Text = nextAppointment.AppointmentDate.ToString("dd/MM/yyyy HH:mm");
                ServiceLabel.Text = nextAppointment.Service.ServiceName;
            }
            else
            {
                ManufacturerLabel.Text = "No appointments found";
                ModelLabel.Text = string.Empty;
                NextAppointmentLabel.Text = string.Empty;
                ServiceLabel.Text = string.Empty;
            }
        }


        private async void GreetingMessage()
        {
            var auxString = await _storageToolKit.GetUsername();
            usernameLabel.Text = auxString.Substring(0, auxString.IndexOf(' '));
            switch (DateTime.Now.Hour)
            {
                case < 12:
                    GreetingLabel.Text = "Good Morning,";
                    break;
                case < 18:
                    GreetingLabel.Text = "Good Afternoon,";
                    break;
                default:
                    GreetingLabel.Text = "Good Evening,";
                    break;
            }
        }

        private async void ImageButton_Pressed(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new About());
        }


        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ServicesPage());
        }

        private async void LoadImage()
        {
            try
            {
                CarImage.Source = ImageStorage.CarImage;
                //avatar.Source = ImageStorageProfile.ProfilePicture;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error loading image: {ex.Message}", "OK");
            }
        }

        private async void ServiceNowButton(object sender, EventArgs e)
        {
            var userId = int.Parse(await _storageToolKit.GetUserID());
            var favoriteCarId = await _userService.GetFavoriteCarId(userId);
            var car = await _carService.GetCarById(favoriteCarId);
            var appointmentPage = new ScheduleAppointment(car);
            await Navigation.PushModalAsync(appointmentPage);
        }
    }
}
