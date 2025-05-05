using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Timers;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();

            StartMemoryMonitor();

            Routing.RegisterRoute("Home", typeof(MainPage));
            Routing.RegisterRoute("Sync", typeof(Sync));
            Routing.RegisterRoute("VisitList", typeof(VisitList));
            Routing.RegisterRoute("Admin", typeof(AdminPage));
            Routing.RegisterRoute("SyncBeta", typeof(SyncBeta));
            Routing.RegisterRoute("Visit", typeof(Visit));
            Routing.RegisterRoute("VisitResume", typeof(VisitResume));
            Routing.RegisterRoute("AdminPageInfo", typeof(AdminPageInfo));
            Routing.RegisterRoute("AdminPageQuery", typeof(AdminPageQuery));
            Routing.RegisterRoute("AdminPageLogs", typeof(AdminPageLogs));
            Routing.RegisterRoute("AdminPageExplorer", typeof(AdminPageExplorer));
            Routing.RegisterRoute("AdminPageExplorerCache", typeof(AdminPageExplorerCache));

            lblAppVersion.Text = Util.AppCurrentVersion.Version();
            lblAppApiAddress.Text = Util.AppCurrentVersion.API_Address();
            lblAppEnviorment.Text = Util.AppCurrentVersion.Enviorment();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await CheckLoginStatus();
        }

        private async void BtnLogout_Clicked(object? sender, EventArgs e)
        {
            try
            {
                SecureStorage.Remove("AccessToken");
                SecureStorage.Remove("UserName");

                lblUserName.Text = "Visitante";

                DisableControls();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao fazer logout: {ex.Message}", "OK");
            }
        }

        async void btnLogin_Clicked(System.Object sender, System.EventArgs e)
        {
            DisableControls();

            try
            {
                #if ANDROID
                var currentActivity = Platform.CurrentActivity;
                #else
                var currentActivity = App.Current.MainPage;
                #endif

                var result = await MauiProgram.PCA.AcquireTokenInteractive(new[] { "User.Read" })
                    .WithParentActivityOrWindow(currentActivity)
                    .ExecuteAsync();

                var userInfoJson = await GetUserDetailsFromGraphAsync(result.AccessToken);

                var claim_cwid = result.ClaimsPrincipal.FindAll(w => w.Type.Contains("cwid")).FirstOrDefault();

                var claim_name = result.ClaimsPrincipal.FindAll(w => w.Type == "name").FirstOrDefault();

                if (claim_cwid == null)
                {
                    await DisplayAlert("Erro", $"Falha ao obter o seu CWID", "OK");
                    return;
                }

                if (claim_name == null)
                {
                    await DisplayAlert("Erro", $"Falha ao obter o seu NAME", "OK");
                    return;
                }

                lblUserName.Text = $"Olá, {claim_name.Value}!";
                //lblUserName.Text = $"CWID: {claim_cwid.Value}, Bem-vindo, {claim_name.Value}, E-MAIL: {result.Account.Username}!";

                await SecureStorage.SetAsync("AccessToken", result.AccessToken);
                await SecureStorage.SetAsync("UserName", claim_name.Value);
                await SecureStorage.SetAsync("CWID", claim_cwid.Value);

                EnableControls();

                Console.WriteLine(userInfoJson);
            }
            catch (MsalException ex)
            {
                await DisplayAlert("Erro", $"Falha ao autenticar: {ex.Message}", "OK");
            }
        }

        private async Task<string> GetUserDetailsFromGraphAsync(string accessToken)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync("https://graph.microsoft.com/v1.0/me");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private async Task CheckLoginStatus()
        {
            try
            {
                var accessToken = await SecureStorage.GetAsync("AccessToken");
                var userName = await SecureStorage.GetAsync("UserName");

                if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(userName))
                {
                    bool isValidToken = await ValidateTokenAsync(accessToken);

                    if (isValidToken)
                    {
                        EnableControls();
                        lblUserName.Text = $"Olá, {userName}!";
                    }
                    else
                    {
                        await DisplayAlert("Sessão expirada", "Por favor, faça login novamente.", "OK");
                        await ExecuteLogin(); 
                    }
                }
                else
                {
                    lblUserName.Text = "Visitante";
                    DisableControls();
                    await ExecuteLogin(); 
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao verificar status de login: {ex.Message}", "OK");
            }
        }

        private async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync("https://graph.microsoft.com/v1.0/me");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private async Task ExecuteLogin()
        {
            try
            {
                btnLogin_Clicked(null, null);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao realizar login: {ex.Message}", "OK");
            }
        }

        private async void DisableControls()
        {
            await SecureStorage.SetAsync("AccessToken", string.Empty);
            await SecureStorage.SetAsync("UserName", string.Empty);
            await SecureStorage.SetAsync("CWID", string.Empty);

            //lblUserName.IsVisible = false;
            btnLogout.Text = "Entrar";
            menuAdmin.IsVisible = false;
            menuSync.IsVisible = false;
            menuVisit.IsVisible = false;

            btnLogout.IsVisible = false;
            btnLogin.IsVisible = true;

            MessagingCenter.Send(this, "LoginStatusChanged", false);
        }

        private void EnableControls()
        {
            //lblUserName.IsVisible = true;
            btnLogout.Text = "Sair";
            menuAdmin.IsVisible = true;
            menuSync.IsVisible = true;
            menuVisit.IsVisible = true;

            btnLogout.IsVisible = true;
            btnLogin.IsVisible = false;

            MessagingCenter.Send(this, "LoginStatusChanged", true);
        }

        //private async Task CheckLoginStatus()
        //{
        //    try
        //    {
        //        // Tente obter o token armazenado
        //        var accessToken = await SecureStorage.GetAsync("AccessToken");
        //        var userName = await SecureStorage.GetAsync("UserName");

        //        if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(userName))
        //        {
        //            lblUserName.Text = $"Bem-vindo de volta, {userName}!";
        //            // Aqui você pode redirecionar para a página principal do aplicativo
        //        }
        //        else
        //        {
        //            // Se não houver token, exiba a tela de login
        //            // (Por exemplo, você pode chamar OnLoginClicked aqui ou redirecionar para a página de login)
        //            lblUserName.Text = "nao logado";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Lidar com exceções (ex.: falha ao acessar o armazenamento seguro)
        //        await DisplayAlert("Erro", $"Falha ao verificar status de login: {ex.Message}", "OK");
        //    }
        //}

        private async void StartMemoryMonitor()
        {
            while (true)
            {
                try
                {
                    long memoryUsed = GC.GetTotalMemory(false);
                    double memoryInMB = memoryUsed / (1024.0 * 1024.0);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        //MemoryUsageLabel.Text = $"Consumo de Memória: {memoryInMB:F2} MB {GetIcon(memoryInMB)}";
                        Util.MemoryRAM.CurrentValue = $"{memoryInMB}";
                    });
                }
                catch (Exception ex)
                {
                    Util.MemoryRAM.CurrentValue = $"Erro ao obter uso de memória: {ex.Message}";
                }

                await Task.Delay(1000);
            }
        }

        private string GetIcon(double value)
        {
            string icon = string.Empty;
            if (value < 100)
            {
                icon = "✅";
            }
            else if (value >= 100 && value < 300)
            {
                icon = "⚠️";
            }
            else if (value >= 300 && value < 600)
            {
                icon = "❗";
            }
            else if (value >= 600 && value < 1000)
            {
                icon = "🚨";
            }
            else if (value >= 1000)
            {
                icon = "🔥";
            }
            return icon;
        }
    }
}
