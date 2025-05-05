using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI.Components;

public partial class VisitLegend : ContentView
{
    public VisitLegendViewModel myVM { get; set; } = new();

    public VisitLegend()
	{

		InitializeComponent();

		myVM.Description = Visit.POS.UnitVariation;

		BindingContext = myVM;
	}
}
