using iService3.Models;
using iService3.Services;
using iService3.Tools;
using Newtonsoft.Json;

namespace iService3;

public partial class App : Application
{
    private readonly SecureStorageToolKit _secureStorage;
    private readonly UserService _userService;

    public App()
    {
        _secureStorage = new SecureStorageToolKit();
        _userService = new UserService();
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
        }
    }

    private async Task<bool> GetStatus()
    {
        var token = await _secureStorage.GetToken();
        if (token.Length == 32)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}