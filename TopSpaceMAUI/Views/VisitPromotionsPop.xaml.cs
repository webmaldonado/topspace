using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI;

public partial class VisitPromotionsPop : ContentPage
{
	public List<VisitPromotionViewModel> myListPromotions { get; set; }

	public VisitPromotionsPop(string pos_code)
	{
		InitializeComponent();

        DAL.Promotion PromotionDAL = new();
        DAL.Brand BrandDAL = new();
        DAL.SKU SkuDAL = new();

        myListPromotions = (from p in PromotionDAL.GetAll()
                             join b in BrandDAL.GetAll() on p.BrandID equals b.BrandID
                             join s in SkuDAL.GetAll() on p.SKUID equals s.SKUID
                             select new VisitPromotionViewModel() {
                                 Title = p.Title,
                                 Description = p.Description,
                                 BrandName = b.Name,
                                 SKUName = s.Name
                             }).ToList();

        myPromotions.ItemsSource = myListPromotions;

    }

    private void OnPreviousClicked(object sender, EventArgs e)
    {
        if (myPromotions.Position > 0)
        {
            myPromotions.Position -= 1;
        }
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
        if (myPromotions.Position < myListPromotions.Count -1)
        {
            myPromotions.Position += 1;
        }
    }

    async void btnClose_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("//VisitList/Visit");
    }
}
