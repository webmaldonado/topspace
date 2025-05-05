using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI;

public partial class AdminPageExplorerCache : ContentPage
{
    private AdminExplorerCacheViewModel ViewModel => BindingContext as AdminExplorerCacheViewModel;

    public AdminPageExplorerCache()
	{
		InitializeComponent();
        BindingContext = new AdminExplorerCacheViewModel();
    }

    void ListView_ItemTapped(System.Object sender, Microsoft.Maui.Controls.ItemTappedEventArgs e)
    {
        if (e == null || e.Item == null)
            return;

        string selectedItem = e.Item.ToString();
        ViewModel.Navigate(selectedItem);
    }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        var path = Microsoft.Maui.Storage.FileSystem.CacheDirectory;
        var file_names = Directory.GetFiles(path);
        foreach (var name in file_names)
        {
            File.Delete(name);
        }

        BindingContext = new AdminExplorerCacheViewModel();
        ViewModel.LoadItems();
    }
}
