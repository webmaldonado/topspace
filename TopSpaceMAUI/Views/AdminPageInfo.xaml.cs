namespace TopSpaceMAUI;

public partial class AdminPageInfo : ContentPage
{
	public AdminPageInfo()
	{
		InitializeComponent();

        txtPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        txtAPI_Address.Text = Config.URL_API_BASE;

        swiVisitDebug.IsToggled = Preferences.Get("VisitLogDebug", false);
        lblVisitDebug.Text = GetBooleanLegend(swiVisitDebug.IsToggled);
    }

    async void btnCopyPath_Clicked(System.Object sender, System.EventArgs e)
    {
        await Clipboard.SetTextAsync(txtPath.Text);
        await DisplayAlert("Sucesso", "Texto copiado para a área de transferência", "OK");
    }

    async void btnCopyApiAddress_Clicked(System.Object sender, System.EventArgs e)
    {
        await Clipboard.SetTextAsync(txtAPI_Address.Text);
        await DisplayAlert("Sucesso", "Texto copiado para a área de transferência", "OK");
    }

    void swiVisitDebug_Toggled(System.Object sender, Microsoft.Maui.Controls.ToggledEventArgs e)
    {
        bool isSwitchedOn = e.Value;
        Preferences.Set("VisitLogDebug", isSwitchedOn);

        lblVisitDebug.Text = GetBooleanLegend(isSwitchedOn);
    }

    private string GetBooleanLegend(bool p_checked)
    {
        var ret = "DESLIGADO";
        ret = (p_checked ? "LIGADO" : "DESLIGADO");
        return ret;
    }
}
