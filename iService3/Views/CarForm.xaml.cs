using iService3.Models;
using iService3.Services;
using iService3.Tools;

namespace iService3.Views;

public partial class CarForm : ContentPage
{
    private readonly CarService _carService = new();
    private List<string> _carManufacturerList = new();
    private readonly SecureStorageToolKit _secureStorageToolKit;
    public CarForm()
	{
        _secureStorageToolKit = new SecureStorageToolKit();
        InitializeComponent();
        LoadCarManufacturer();
    }

    private async void LoadCarManufacturer()
    {
        try
        {
            _carManufacturerList = await _carService.GetCarManufacturers();
            _carManufacturerList = _carManufacturerList.OrderBy(q=>q).ToList();
            manufacturerPicker.ItemsSource = _carManufacturerList;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void addCarBtn_Clicked(object sender, EventArgs e)
    {
		var car = new Car()
		{
            UserId = int.Parse(await _secureStorageToolKit.GetUserID()),
            Manufacturer = manufacturerPicker.SelectedItem.ToString(),
			Model = modelPicker.SelectedItem?.ToString(),
			Year = int.Parse(Year.Text)
		};
        await _carService.AddCar(car);
        await Navigation.PopAsync();
    }

    private void ManufacturerPicker_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedManufacturer = manufacturerPicker.SelectedItem?.ToString();

        if (!string.IsNullOrEmpty(selectedManufacturer))
        {
            LoadCarModels(selectedManufacturer);
        }
    }

    private async void LoadCarModels(string manufacturer)
    {
        try
        {
            var carModels = await _carService.GetCarModels(manufacturer);
            modelPicker.ItemsSource = carModels;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

}