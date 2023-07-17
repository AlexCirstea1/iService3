using iService3.Models;
using iService3.Services;
using iService3.Tools;
using Newtonsoft.Json;

namespace iService3;

public partial class App : Application
{
    private readonly SecureStorageToolKit _secureStorage;
    private readonly UserService _userService;
    private readonly CarService _carService;

    public App()
    {
        _secureStorage = new SecureStorageToolKit();
        _userService = new UserService();
        _carService = new CarService();
        InitializeComponent();
        if (Preferences.ContainsKey("IsLoggedIn") && Preferences.Get("IsLoggedIn", false))
        {
            UpdateUser();
            MainPage = new AppShell();
        }
        else
        {
            Preferences.Remove("userData");
            SecureStorage.Default.Remove("UserID");
            SecureStorage.Default.Remove("Username");
            SecureStorage.Default.Remove("Token");
            MainPage = new MainPage();
        }
    }

    private async void UpdateUser()
    {
        string userJson = Preferences.Get("userData", "");
        User userData = JsonConvert.DeserializeObject<User>(userJson);
        if (userData != null)
        {
            userData = await _userService.GetUserById(Int32.Parse(userData.UserId));
            userJson = JsonConvert.SerializeObject(userData);
            Preferences.Set("userData", userJson);
            LoadImage(int.Parse(userData.UserId));
        }
    }

    private async void LoadImage(int userId)
    {
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
                Console.WriteLine("Image not found or error occurred.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading the image: {ex.Message}");
        }
    }
}