using iService3.Models;
using iService3.Services;
using iService3.Tools;

namespace iService3.Views;

public partial class ScheduleAppointment : ContentPage
{
    private readonly Car _car;
    private readonly AppointmentService _appointmentService;
    private readonly SecureStorageToolKit _secureStorageToolKit;
    private readonly List<string> _services = new();

    private readonly TimeRange range1 = new TimeRange(TimeSpan.FromHours(8));
    private readonly TimeRange range2 = new TimeRange(TimeSpan.FromHours(10.5));
    private readonly TimeRange range3 = new TimeRange(TimeSpan.FromHours(13));
    private readonly TimeRange range4 = new TimeRange(TimeSpan.FromHours(15.5));

    private Dictionary<TimeRange, string> _scheduleList;

    public ScheduleAppointment(Car car)
    {
        _secureStorageToolKit = new SecureStorageToolKit();
        _appointmentService = new AppointmentService();
        _car = car;
        InitializeComponent();
        UpdateSchedule();
        LoadService();
    }

    private async void UpdateSchedule()
    {

        SetIntervals();

        var services = await _appointmentService.GetServices();

        var selectedServiceIndex = Picker.SelectedIndex;
        var selectedService = 0;

        if (selectedServiceIndex >= 0)
        {
            selectedService = services.ElementAt(selectedServiceIndex).ServiceId;
        }

        var blockedAppointmentDates = await _appointmentService.GetAppointmentsDatesByServiceId(selectedService);
        blockedAppointmentDates = blockedAppointmentDates.Where(a => a.Date == datePicker.Date).ToList();


        foreach (var timeRange in from blockedDate in blockedAppointmentDates from timeRange in _scheduleList.Keys.ToList() where timeRange.startTime.Hours == blockedDate.Hour select timeRange)
        {
            _scheduleList.Remove(timeRange);
        }
        IntervalPicker.ItemsSource = _scheduleList.Values.ToList();
    }

    private async void LoadService()
    {
        try
        {
            IntervalPicker.ItemsSource = _scheduleList.Values.ToList();
            var services = await _appointmentService.GetServices();
            foreach (var item in services)
            {
                _services.Add(item.ServiceName);
            }
            Picker.ItemsSource = _services;
        }
        catch (Exception e)
        {
            await DisplayAlert("Error", e.Message, "OK");
        }
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        var userID = int.Parse(await _secureStorageToolKit.GetUserID());
        var services = await _appointmentService.GetServices();

        var selectedServiceIndex = Picker.SelectedIndex;

        if (selectedServiceIndex < 0)
        {
            await DisplayAlert("Error", "Please select a service", "OK");
            return;
        }

        var selectedService = services.ElementAt(selectedServiceIndex).ServiceId;


        var selectedRange = new TimeRange();


        if (IntervalPicker.SelectedItem is string selectedValue)
        {
            selectedRange = _scheduleList.FirstOrDefault(x => x.Value == selectedValue).Key;
        }

        var date = new DateTime(datePicker.Date.Year, datePicker.Date.Month, datePicker.Date.Day, selectedRange.startTime.Hours, selectedRange.startTime.Minutes, 0);

        if (date < DateTime.Now)
        {
            await DisplayAlert("Error", "You cannot schedule an appointment in the past", "OK");
            return;
        }

        if (IntervalPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Error", "Please select an interval", "OK");
            return;
        }

        if (string.IsNullOrEmpty(typeEntry.Text))
        {
            await DisplayAlert("Error", "Please enter the appointment type", "OK");
            return;
        }

        var appointment = new Appointment
        {
            AppointmentDate = date,
            AppointmentType = typeEntry.Text,
            AppointmentNotes = notesEditor.Text,
            ServiceId = selectedService,
            CarId = _car.CarId,
            UserId = userID
        };

        if (appointment.AppointmentNotes.Length > 100)
        {
            await DisplayAlert("Error", "Appointment notes exceed the maximum length", "OK");
            return;
        }

        await _appointmentService.ScheduleAppointment(appointment);
        await Navigation.PopModalAsync();
    }

    private async void CancelBtn_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void DatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
    {
        UpdateSchedule();
    }

    private void SetIntervals()
    {
        _scheduleList = new Dictionary<TimeRange, string>
        {
            { range1, "8:00 - 10:30" },
            { range2, "10:30 - 13:00" },
            { range3, "13:00 - 15:30" },
            { range4, "15:30 - 18:00" }
        };
    }

    private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateSchedule();
    }
}