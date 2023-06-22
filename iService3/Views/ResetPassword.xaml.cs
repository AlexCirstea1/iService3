using iService3.Services;

namespace iService3.Views;

public partial class ResetPassword : ContentPage
{
    private readonly UserService _userService;
    private readonly int _userID;
	public ResetPassword(int userId)
    {
        _userID = userId;
        _userService = new();
		InitializeComponent();
	}

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        var response = await _userService.ResetPassword(_userID, OldPassword.Text, NewPassword.Text);
        if (response)
        { 
            await Navigation.PopModalAsync();
        }
        else
        {
            DisplayAlert("Error", "Password do not match!", "Ok");
        }
       
    }

    private async void CancelBtn_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}