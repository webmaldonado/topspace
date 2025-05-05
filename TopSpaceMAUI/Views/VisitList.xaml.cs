using TopSpaceMAUI.ViewModel;
using TopSpaceMAUI.Util;
using Location = TopSpaceMAUI.Util.Location;
using TopSpaceMAUI.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui;
using Microsoft.IdentityModel.Tokens;

namespace TopSpaceMAUI;

public partial class VisitList : ContentPage
{
    public bool UserIsLogged { get; set; }

    public VisitList()
	{
		InitializeComponent();

        var userName = SecureStorage.GetAsync("UserName");
        UserIsLogged = userName?.Id == null ? false : true;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        LoadVisitsInProgress();
        LoadVisitsFinished();
    }

    private void LoadVisitsInProgress(string p_search = "")
	{
        var list_visit_view_model = new List<ListVisitViewModel>();
        List<Model.POS> lstPOSNotVisited = LoadPOSNotVisited(p_search);

        foreach (var item in lstPOSNotVisited)
        {
            var CurrentVisitHistory = LoadVisitHistory(item.POSCode);
            var PreviousVisitHistory = LoadVisitHistory(item.POSCode, -1);
            var PreviousScoreAVG = $"Anterior: {CalculateAVG(PreviousVisitHistory)}%";
            var CurrentScoreAVG = $"Atual: {CalculateAVG(CurrentVisitHistory)}%";

            list_visit_view_model.Add(new ListVisitViewModel() {
                Name = item.Name,
                POSCode = item.POSCode,
                TagBaseName = item.TagBaseName,
                Address = item.Address,
                City = item.City,
                VisitCount = item.VisitCount,
                Category = item.Category,
                CurrentScoreAVG = CurrentScoreAVG,
                PreviousScoreAVG = PreviousScoreAVG,
                VisitHistory = CurrentVisitHistory
            });
        }

        lstInProgress.ItemsSource = list_visit_view_model;
	}

    private float CalculateAVG(List<ListVisitHistoryViewModel> myList)
    {
        float myAVG = 0f;
        if (myList != null && myList.Count > 0)
            myAVG = (myList.Sum(f => f.Score)/myList.Count);
        return myAVG;
    }

    private void LoadVisitsFinished(string p_search = "")
    {
        var list_visit_view_model = new List<ListVisitViewModel>();
        List<Model.POS> lstPOSVisited = LoadPOSVisited(p_search);
        //List<Model.POS> lstPOSVisited = LoadPOSNotVisited();

        foreach (var item in lstPOSVisited)
        {
            var CurrentVisitHistory = LoadVisitHistory(item.POSCode);
            var PreviousVisitHistory = LoadVisitHistory(item.POSCode, -1);
            var PreviousScoreAVG = $"Anterior: {CalculateAVG(PreviousVisitHistory)}%";
            var CurrentScoreAVG = $"Atual: {CalculateAVG(CurrentVisitHistory)}%";

            list_visit_view_model.Add(new ListVisitViewModel()
            {
                Name = item.Name,
                POSCode = item.POSCode,
                TagBaseName = item.TagBaseName,
                Address = item.Address,
                City = item.City,
                VisitCount = item.VisitCount,
                Category = item.Category,
                CurrentScoreAVG = CurrentScoreAVG,
                PreviousScoreAVG = PreviousScoreAVG,
                VisitHistory = CurrentVisitHistory
            });
        }

        lstFinished.ItemsSource = list_visit_view_model;
    }

    private List<Model.POS> LoadPOSNotVisited(string p_search)
    {
        DAL.POS POSController = new DAL.POS();
        var myListPOSNotVisited = POSController.GetPOSs(false).Where(w => p_search == string.Empty || w.Name.ToLower().Contains(p_search.ToLower()) || w.Address.ToLower().Contains(p_search.ToLower())).ToList();

        // Caso a localização esteja habilitada, as farmácias exibidas são as mais próximos da localização atual
        //Location location = new Location();
        //if (location.position != null)
        //{
        //    myListPOSNotVisited = POSController.OrderNearestPOS(myListPOSNotVisited, location.position);
        //}

        return myListPOSNotVisited;
    }

    private List<Model.POS> LoadPOSVisited(string p_search)
    {
        DAL.POS POSController = new DAL.POS();
        var myListPOSVisited = POSController.GetPOSs(true).Where(w => w.Name.ToLower().Contains(p_search.ToLower())).ToList(); ;

        // Caso a localização esteja habilitada, as farmácias exibidas são as mais próximos da localização atual
        //Location location = new Location();
        //if (location.position != null)
        //{
        //    myListPOSVisited = POSController.OrderNearestPOS(myListPOSVisited, location.position);
        //}


        return myListPOSVisited;
    }

    private List<ListVisitHistoryViewModel> LoadVisitHistory(string pos_code, int month_ref = 0)
    {
        var list_visit_history = new List<ListVisitHistoryViewModel>();
        var dalVisits = new TopSpaceMAUI.DAL.Visit();
        var dalLastVisit = new TopSpaceMAUI.DAL.LastVisit();

        var current_month = DateTime.Now.AddMonths(month_ref).Month;
        //var current_month = (month_ref == 0 ? 3 : 2);
        var current_year = DateTime.Now.AddMonths(month_ref).Year;

        var visits = dalVisits.GetVisit()
            .FindAll(w => w.POSCode == pos_code
            && w.Status == Config.STATUS_VISIT_COMPLETED
            && DateTime.Parse(w.VisitDate).Month == current_month
            && DateTime.Parse(w.VisitDate).Year == current_year);

        var lastVisits = dalLastVisit.GetAll().
            FindAll(w => w.POSCode == pos_code
            && DateTime.Parse(w.VisitDate).Month == current_month
            && DateTime.Parse(w.VisitDate).Year == current_year);

        if (visits != null && visits.Count > 0)
        {
            foreach (var item in visits)
            {
                lastVisits.Add(new Model.LastVisit()
                {
                    POSCode = item.POSCode,
                    Score = item.Score,
                    //Score = new Random().Next(100),
                    VisitDate = item.VisitDate
                    //VisitDate = RandomDay().ToString()
                });
            }
        }

        lastVisits.Sort(new DateStringComparer());

        foreach (var item in lastVisits)
        {
            list_visit_history.Add(new ListVisitHistoryViewModel()
            {
                VisitDate = DateTime.Parse(item.VisitDate).ToString("dd/MM/yyyy"),
                Score = item.Score
            });
        }

        return list_visit_history;
    }

    private Random gen = new Random();
    DateTime RandomDay()
    {
        DateTime start = new DateTime(1995, 1, 1);
        int range = (DateTime.Today - start).Days;
        return start.AddDays(gen.Next(range));
    }

    async void btnCheckIn_Clicked(System.Object sender, System.EventArgs e)
    {
        if (!UserIsLogged)
        {
            return;
        }

        var pos_code = ((Button)sender).CommandParameter.ToString();
        var route = $"Visit?pos_code={pos_code}";
        await Shell.Current.GoToAsync(route);
    }

    async void txtSearch_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        await Task.Delay(1000);

        LoadVisitsInProgress(e.NewTextValue);
    }
}

class DateStringComparer : IComparer<Model.LastVisit>
{
    public string OrderType { get; set; }
    public DateStringComparer(string order_type = "desc")
    {
        this.OrderType = order_type;
    }
    public int Compare(Model.LastVisit x, Model.LastVisit y)
    {
        DateTime dateX = DateTime.Parse(x.VisitDate);
        DateTime dateY = DateTime.Parse(y.VisitDate);

        if (this.OrderType == "desc")
            return DateTime.Compare(dateY, dateX);
        else
            return DateTime.Compare(dateX, dateY);
    }
}
