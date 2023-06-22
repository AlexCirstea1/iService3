using iService3.Models;
using iService3.Services;
using iService3.Tools;

namespace iService3.Views;

public partial class CarFormVIN : ContentPage
{
    private readonly CarService _carService;
    private readonly SecureStorageToolKit _secureStorageToolKit;
    public CarFormVIN()
	{
        _secureStorageToolKit = new SecureStorageToolKit();
        _carService = new CarService();
		InitializeComponent();
	}

    private async void SaveBtn_OnClicked(object sender, EventArgs e)
    {
        try
        {
            var vin = VinEntry.Text;

            if (string.IsNullOrWhiteSpace(vin))
            {
                await DisplayAlert("Error", "VIN number cannot be empty", "OK");
                return;
            }

            if (vin.Length != 17)
            {
                await DisplayAlert("Error", "VIN number should be 17 characters long", "OK");
                return;
            }

            var decodeResponse =  await CarMdApiService.DecodeVin(vin);


            AddCarToUser(decodeResponse);

            await Navigation.PopAsync();
            //await DisplayAlert("Success", "VIN number saved successfully", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Ok");
        }

        
    }

    private async void AddCarToUser(VinDecodeResponse dataResponse)
    {
        string transformedManufacturer= char.ToUpper(dataResponse.Data.Make[0]) + dataResponse.Data.Make.Substring(1).ToLower();
        string transformedModel= char.ToUpper(dataResponse.Data.Model[0]) + dataResponse.Data.Model.Substring(1).ToLower();

        var car = new Car()
        {
            UserId = Int32.Parse(await _secureStorageToolKit.GetUserID()),
            Manufacturer = transformedManufacturer,
            Model = transformedModel,
            Year = int.Parse(dataResponse.Data.Year.ToString())
        };
        await _carService.AddCar(car);
    }
}