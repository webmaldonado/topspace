using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI;

public partial class SyncBeta : ContentPage
{
	public SyncBetaViewModel vm = new();

	public SyncBeta()
	{
		InitializeComponent();
        BindingContext = vm;
    }
}
