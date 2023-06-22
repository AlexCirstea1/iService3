using iService3.Services;
using iService3.Tools;
using Launcher = Microsoft.Maui.ApplicationModel.Launcher;

namespace iService3.Views;

public partial class ServicesPage : ContentPage
{
    private readonly AppointmentService _apointmentService;
    public ServicesPage()
    {
        _apointmentService = new();
		InitializeComponent();
        Load();
	}
    public async void Load()
    {
        try
        {
            var services = await _apointmentService.GetServices();

            foreach (var service in services)
            {
                byte[] imageBytes = await _apointmentService.GetImage(service.ServiceId);
                service.Photo = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }

            serviceListView.ItemsSource = services;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error loading services: {ex.Message}", "OK");
        }
    }


    private void Button_OnClicked(object sender, EventArgs e)
    {
        var service = (sender as Button)?.BindingContext as Models.Service;
        CallPhoneNumber(service.PhoneNumber);
    }
    public void CallPhoneNumber(string phoneNumber)
    {
        if (PhoneDialer.Default.IsSupported)
        {
            PhoneDialer.Default.Open(phoneNumber);
        }
        else
        {
            DisplayAlert("Not Supported", "Dialing is not supported", "Ok");
        }
    }

    public async void SendEmail(string email)
    {
        if (Email.Default.IsComposeSupported)
        {
            var message = new EmailMessage
            {
                To = new List<string> { email }
            };

            await Email.Default.ComposeAsync(message);
        }
    }

    private void Button2_OnClicked(object sender, EventArgs e)
    {
        var service = (sender as Button)?.BindingContext as Models.Service;
        SendEmail(service.Email);
    }

    private async void Button3_OnClicked(object sender, EventArgs e)
    {
        var service = (sender as Button)?.BindingContext as Models.Service;
        OpenMaps(service.Address);
    }
    private async void OpenMaps(string location)
    {
        var uri = new Uri($"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(location)}");
        await Launcher.TryOpenAsync(uri);
    }
}