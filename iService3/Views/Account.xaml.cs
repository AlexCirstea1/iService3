using iService3.Models;
using iService3.Services;
using iService3.Tools;
using Newtonsoft.Json;

namespace iService3.Views;

public partial class Account : ContentPage
{
    private readonly UserService _userService;
    private readonly SecureStorageToolKit _secureStorageToolKit;
    private readonly CarService _carService;
    public Account()
    {
        _carService = new CarService();
        _userService = new UserService();
        _secureStorageToolKit = new SecureStorageToolKit();
        InitializeComponent();
        Load();
    }
    private async void Load()
    {
        try
        {
            usernameLabel.Text = await _secureStorageToolKit.GetUsername();
            string userJson = Preferences.Get("userData", "");
            User userData = JsonConvert.DeserializeObject<User>(userJson);
            NewsletterSwitch.IsToggled = (bool)userData.NewsletterSub;
            //avatar.Source = ImageStorageProfile.ProfilePicture;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        //avatar.Source = ImageStorageProfile.ProfilePicture;
    }
    public static void GoToHomePage()
    {
        var mainPage = new MainPage();
        Application.Current.MainPage = mainPage;
    }

    private async void LogOutButton_OnClickedButton_Clicked(object sender, EventArgs e)
    {
        Preferences.Default.Set("IsLoggedIn", false);
        SecureStorage.Default.Remove("UserID");
        SecureStorage.Default.Remove("Username");
        SecureStorage.Default.Remove("Token");
        await _userService.Logout();
        GoToHomePage();
    }

    private async void OnAvatarTapped(object sender, TappedEventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select Profile Picture",
            FileTypes = FilePickerFileType.Images
        });

        if (result == null)
        {
            return;
        }

        var stream = await result.OpenReadAsync();
        ImageStorageProfile.ProfilePicture = ImageSource.FromStream(() => stream);
        //avatar.Source = ImageStorageProfile.ProfilePicture;
        //DisplayAlert("Profile Picture Clicked", "clicked!", "Ok", "Cancel");
    }

    private async void NewsletterSwitch_OnToggled(object sender, ToggledEventArgs e)
    {
        bool isToggledOn = e.Value;
        var userid = Int32.Parse(await _secureStorageToolKit.GetUserID());
        var success = await _userService.UpdateNewsletterSub(userid, isToggledOn);
    }

    private async void Button_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ResetPassword(Int32.Parse(await _secureStorageToolKit.GetUserID())));
    }

    private async void FavCarButton(object sender, EventArgs e)
    {
        var userId = Int32.Parse(await _secureStorageToolKit.GetUserID());
        var userCars = await _carService.GetCarsByUserId(userId);

        var carSelection = await DisplayActionSheet("Select a car", "Cancel", null, userCars.Select(car => $"{car.Manufacturer} {car.Model}").ToArray());

        if (carSelection != null && carSelection != "Cancel")
        {
            var selectedCar = userCars.FirstOrDefault(car => $"{car.Manufacturer} {car.Model}" == carSelection);

            if (selectedCar != null)
            {
                await _userService.SetFavouriteCar(userId, selectedCar.CarId);
                LoadImage();
            }
        }
    }

    private async void LoadImage()
    {
        var userId = int.Parse(await _secureStorageToolKit.GetUserID());
        var favoriteCarId = await _userService.GetFavoriteCarId(userId);
        try
        {
            byte[] imageBytes = await _carService.GetImage(favoriteCarId);

            if (imageBytes != null)
            {
                var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                ImageStorage.CarImage = imageSource;
            }
            else
            {
                ImageStorage.RemoveCarImage();
                Console.WriteLine("Image not found or error occurred.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading the image: {ex.Message}");
        }
    }
}