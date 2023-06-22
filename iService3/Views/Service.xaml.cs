namespace iService3.Views;

public partial class Service : ContentPage
{
    public Service()
    {
        InitializeComponent();
    }

    private async Task ImageButton_PressedAsync(object sender, EventArgs e)
    {
    }

    private async void ImageButton_Pressed(object sender, EventArgs e)
    {
        await DisplayAlert("Alert", "You have been alerted", "OK");
    }
}