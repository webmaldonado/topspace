namespace TopSpaceMAUI;

public partial class AdminPageImageViewer : ContentPage
{
    public AdminPageImageViewer()
    {
        InitializeComponent();
    }

    public AdminPageImageViewer(string imagePath)
    {
        InitializeComponent();
        ImageView.Source = ImageSource.FromFile(imagePath);
    }

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();

    //    Shell.SetBackButtonBehavior(this, new BackButtonBehavior
    //    {
    //        IsVisible = false
    //    });
    //}

    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("//Admin/AdminPageExplorer");
    }
}
