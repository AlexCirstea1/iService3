using iService3.Services;
using Newtonsoft.Json;

namespace iService3.Views;

public partial class RegisterPage : ContentPage
{
    private readonly UserService _userService;
	public RegisterPage()
	{
        _userService = new UserService();
		InitializeComponent();
	}
    public static void GoToAppShell()
    {
        var appShell = new AppShell();
        Application.Current.MainPage = appShell;
    }

    private async void RegisterButton_OnClicked(object sender, EventArgs e)
    {

        var responseRegister = await _userService.Register(UsernameEntry.Text, EmailEntry.Text, PasswordEntry.Text);
        if (responseRegister)
        {
            var userData = await _userService.Login(UsernameEntry.Text, PasswordEntry.Text);
            string userJson = JsonConvert.SerializeObject(userData);
            Preferences.Set("userData", userJson);
            await SecureStorage.Default.SetAsync("UserID", userData.UserId);
            await SecureStorage.Default.SetAsync("Username", userData.Username);
            await SecureStorage.Default.SetAsync("Token", userData.Token);

            Navigation.PopModalAsync();
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            DisplayAlert("Error", "Error Registering", "Ok");
        }
        
    }
}