using System.Reflection.Emit;
using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI;

public partial class AdminPageExplorer : ContentPage
{
    private AdminExplorerViewModel ViewModel => BindingContext as AdminExplorerViewModel;

    public AdminPageExplorer()
	{
		InitializeComponent();
        BindingContext = new AdminExplorerViewModel();
    }

    async void ListView_ItemTapped(System.Object sender, Microsoft.Maui.Controls.ItemTappedEventArgs e)
    {
        if (e.Item == null)
            return;

        string selectedItem = e.Item.ToString();

        // Verifica se é imagem
        if (selectedItem.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
        {
            string imagePath = Path.Combine(ViewModel.CurrentPath, selectedItem.Replace("📄 ", ""));

            //Navigation.PushAsync(new AdminPageImageViewer(imagePath));

            //await Shell.Current.GoToAsync("AdminPageImageViewer", true, new Dictionary<string, object>
            //{
            //    { "ImagePath", imagePath }
            //});

            //await Shell.Current.GoToAsync("AdminPageImageViewer");

            //await Navigation.PushModalAsync(new AdminPageImageViewer(imagePath));

            var popup = new AdminPageImageViewer(imagePath);
            await Navigation.PushModalAsync(popup);

        }
        else
        {
            ViewModel.Navigate(selectedItem);
        }
    }
}
