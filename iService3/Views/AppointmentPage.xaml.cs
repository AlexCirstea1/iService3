using iService3.Services;
using iService3.Tools;

namespace iService3.Views;

public partial class AppointmentPage : ContentPage
{
    private readonly SecureStorageToolKit _storageToolKit;
    private readonly AppointmentService _appointmentService;

    public AppointmentPage()
	{
        _storageToolKit = new SecureStorageToolKit();
        _appointmentService = new AppointmentService();
		InitializeComponent();
        Load();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Load();
    }

    public async void Load()
    {
        try
        {
            var userID = int.Parse(await _storageToolKit.GetUserID());
            var appointments = await _appointmentService.GetAppointmentsByUserId(userID);
            appointments = appointments.Where(a => a.AppointmentDate.Date > DateTime.Now).OrderBy(a => a.AppointmentDate).ToList();
            foreach (var item in appointments)
            {
                item.ServiceName = item.Service.ServiceName;
                item.CarName = item.Car.Manufacturer;
            }
            appointmentsListView.ItemsSource = appointments;

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error loading appointments: {ex.Message}", "OK");
        }
    }
}