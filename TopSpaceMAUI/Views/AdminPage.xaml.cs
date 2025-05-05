using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI;

public partial class AdminPage : ContentPage
{
    

    public AdminPage()
	{
		InitializeComponent();
        
	}

    async void btnQuery_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("AdminPageQuery");
    }

    async void btnLogs_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("AdminPageLogs");
    }

    async void btnInfo_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("AdminPageInfo");
    }

    async void btnExplorer_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("AdminPageExplorer");
    }

    async void btnExplorerCache_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("AdminPageExplorerCache");
    }

    async void btnExcluirVisitas_Clicked(System.Object sender, System.EventArgs e)
    {
        bool confirmar = await Application.Current.MainPage.DisplayAlert(
        "Confirmação",
        "Deseja realmente excluir todas as visitas e apagar todas as fotos ?",
        "Sim", "Não");

        if (!confirmar)
            return;

        var myConnection = DAL.Database.GetNewConnection();

        myConnection.Execute("DELETE FROM VISITCOUNT");
        myConnection.Execute("DELETE FROM VISITDATABRAND");
        myConnection.Execute("DELETE FROM VISITDATASKU");
        myConnection.Execute("DELETE FROM VISIT");

        var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        // Deleta todos os arquivos, exceto os .db
        foreach (var file in Directory.GetFiles(rootPath))
        {
            if (!file.EndsWith(".db", StringComparison.OrdinalIgnoreCase))
            {
                File.Delete(file);
            }
        }

        // Deleta todos os subdiretórios
        foreach (var dir in Directory.GetDirectories(rootPath))
        {
            Directory.Delete(dir, recursive: true);
        }

        await DisplayAlert("Sucesso", "Operacao realizada com sucesso.", "OK");
    }

    void btnClearMemory_Clicked(System.Object sender, System.EventArgs e)
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
}
