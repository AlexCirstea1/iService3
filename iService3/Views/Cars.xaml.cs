using iService3.Models;
using iService3.Services;
using iService3.Tools;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace iService3.Views;

public partial class Cars : ContentPage
{
    private readonly CarService _carService = new CarService();
    private SecureStorageToolKit _secureStorageToolKit;
    private readonly CarMdApiService _carMdApiService;

    public Cars()
    {
        _secureStorageToolKit = new SecureStorageToolKit();
        _carMdApiService = new CarMdApiService();
        InitializeComponent();
        LoadCars();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadCars();
    }

    private async void LoadCars()
    {
        try
        {
            var userID = Int32.Parse(await _secureStorageToolKit.GetUserID());
            var cars = await _carService.GetCarsByUserId(userID);
            foreach (var car in cars)
            {
                car.ImageUrl = await _carService.GetCarImageUrl(car.Manufacturer);
            }
            carsListView.ItemsSource = cars;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error loading cars: {ex.Message}", "OK");
        }
    }

    private async void addBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new CarForm());
    }


    private async void ScheduleAppointmentBtn_Clicked(object sender, EventArgs e)
    {
        var car = (sender as Button)?.BindingContext as Car;
        var appointmentPage = new ScheduleAppointment(car);
        await Navigation.PushModalAsync(appointmentPage);
    }

    private async void DeleteBtn_Clicked(object sender, EventArgs e)
    {
        var car = (sender as Button)?.BindingContext as Car;
        if (car != null && await DisplayAlert("Confirmation", $"Are you sure you want to delete {car.Manufacturer} {car.Model}?", "Yes", "No"))
        {
            await _carService.DeleteCar(car.CarId);
            //LoadCars();
        }
    }

    private async void AddBtnVin_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new CarFormVIN());
    }
}