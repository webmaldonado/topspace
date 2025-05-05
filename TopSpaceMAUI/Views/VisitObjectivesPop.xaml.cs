using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI;

public partial class VisitObjectivesPop : ContentPage
{
	public ObservableCollection<VisitObjectivesViewModel> myListVM { get; set; }

	public VisitObjectivesPop(string pos_code)
	{
		InitializeComponent();

		myListVM = new ObservableCollection<VisitObjectivesViewModel>();

        DAL.ObjectiveBrand ObjectiveBrandDAL = new();
        DAL.Brand BrandDAL = new();
        DAL.Metric MetricDAL = new();

        var objectives = from o in ObjectiveBrandDAL.GetAll()
                         join b in BrandDAL.GetAll() on o.BrandID equals b.BrandID
                         join m in MetricDAL.GetAll() on o.MetricID equals m.MetricID
                         select new VisitObjectivesViewModel() {
                             BrandName = b.Name,
                             Metric = m.Name,
                             Objective = o.Objective
                         };

        myObjectives.ItemsSource = objectives.ToList();
    }

    async void btnClose_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("//VisitList/Visit");

    }
}


