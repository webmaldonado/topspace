using Microsoft.Maui.ApplicationModel.Communication;

namespace TopSpaceMAUI;

public partial class AdminPageLogs : ContentPage
{
    private readonly string logFilePath = TopSpaceMAUI.Util.LogUnhandledException.logFilePath;

    public AdminPageLogs()
	{
		InitializeComponent();
        LoadLogs();
    }

    private void LoadLogs()
    {
        if (File.Exists(logFilePath))
        {
            LogLabel.Text = File.ReadAllText(logFilePath);
            if (LogLabel.Text.Length == 0)
            {
                LogLabel.Text = "Nenhum log encontrado.";
            }
        }
        else
        {
            LogLabel.Text = "Arquivo de armazenamento dos logs nao foi encontrado.";
        }
    }

    async private void OnSendEmailClicked(object sender, EventArgs e)
    {
        try
        {
            var message = new EmailMessage
            {
                Subject = "Top Space - Logs de Erro",
                Body = LogLabel.Text,
                To = new List<string> { "ronald.maldonado1.ext@bayer.com" }
            };

            await Email.Default.ComposeAsync(message);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Não foi possível abrir o aplicativo de e-mail: " + ex.Message, "OK");
        }
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        if (File.Exists(logFilePath))
        {
            File.WriteAllText(logFilePath, string.Empty);
            LoadLogs();
        }
    }

    private async void BotaoCopiar_Clicked(System.Object sender, System.EventArgs e)
    {
        await Clipboard.SetTextAsync(LogLabel.Text);
        await DisplayAlert("Sucesso", "Texto copiado para a área de transferência", "OK");
    }
}
