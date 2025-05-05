using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Microsoft.Identity.Client;
using TopSpaceMAUI.Util;
using TopSpaceMAUI.ViewModel;
using TopSpaceMAUI.Views;

namespace TopSpaceMAUI
{
    public partial class MainPage : ContentPage
    {
        public bool UserIsLogged { get; set; }

        public MainPage()
        {
            InitializeComponent();

            InitializeAsync();

            MessagingCenter.Subscribe<AppShell, bool>(this, "LoginStatusChanged", (sender, isLoggedIn) =>
            {
                EnableAndDisableButtons(isLoggedIn);
            });
        }

        private async void InitializeAsync()
        {
            var userName = await SecureStorage.GetAsync("CWID");
            UserIsLogged = !string.IsNullOrEmpty(userName);

            EnableAndDisableButtons(UserIsLogged);

        }

        private void EnableAndDisableButtons(bool isLoggedIn)
        {
            btnAdmin.IsEnabled = isLoggedIn;
            btnMyPharmas.IsEnabled = isLoggedIn;
            btnSyncNow.IsEnabled = isLoggedIn;
        }

        private void ShowBanner()
        {
            var url = "banner_2.jpeg";
            var ImgLibDAL = new DAL.ImgLib();
            var image = ImgLibDAL.GetAll()?.Where(w => w.Tags.Equals("BANNERAPP")).FirstOrDefault();

            if (image != null)
            {
                url = image.URLDownload;
            }

            Banner.Source = url;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ShowBanner();

            InitializeAsync();

        }


        protected override bool OnBackButtonPressed() { return true; }

        async void btnSyncNow_Clicked(System.Object sender, System.EventArgs e)
        {
            var userName = await SecureStorage.GetAsync("CWID");

            if (UserIsLogged == false) return;
            await Shell.Current.GoToAsync("//Sync");
        }

        async void btnMyPharmas_Clicked(System.Object sender, System.EventArgs e)
        {
            if (UserIsLogged == false) return;
            await Shell.Current.GoToAsync("//VisitList");
        }

        async void btnAdmin_Clicked(System.Object sender, System.EventArgs e)
        {
            if (UserIsLogged == false) return;
            //await DisplayAlert("Top Space", "Modulo ADMIN ainda não disponível.", "OK");
            //return;
            await Shell.Current.GoToAsync("//Admin");
        }

    }

    public class AppVersion
    {
        public string appVersion { get; set; }
        public string appEnvironment { get; set; }
        public string appEndpoint { get; set; }

    }
}
