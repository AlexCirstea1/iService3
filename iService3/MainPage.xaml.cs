using iService3.Services;
using iService3.Tools;
using iService3.Views;
using Newtonsoft.Json;

namespace iService3;

public partial class MainPage : ContentPage
{
    private readonly UserService _userService;
    public MainPage()
    {
        _userService = new UserService();
        InitializeComponent();
    }

    public void GoToAppShell()
    {
        var appShell = new AppShell();
        Application.Current.MainPage = appShell;
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text.Trim();
        var password = PasswordEntry.Text.Trim();

        if (string.IsNullOrWhiteSpace(username))
        {
            await DisplayAlert("Error", "Please enter a username", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Please enter a password", "OK");
            return;
        }

        var userData = await _userService.Login(username, password);

        if (userData == null)
        {
            await DisplayAlert("Error", "Invalid username or password", "OK");
            return;
        }

        var userJson = JsonConvert.SerializeObject(userData);
        Preferences.Set("userData", userJson);
        Preferences.Set("IsLoggedIn", true);
        //_secureStorageToolKit.SaveUserID(userData.UserId.ToString());
        //_secureStorageToolKit.SaveUsername(userData.Username);
        //_secureStorageToolKit.SaveToken(userData.Token);

        await SecureStorage.Default.SetAsync("UserID", userData.UserId);
        await SecureStorage.Default.SetAsync("Username", userData.Username);
        await SecureStorage.Default.SetAsync("Token", userData.Token);

        GoToAppShell();
    }

    private async void Register(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new RegisterPage());
    }

    private void RegisterButton_OnClicked(object sender, EventArgs e)
    {
        
    }
}