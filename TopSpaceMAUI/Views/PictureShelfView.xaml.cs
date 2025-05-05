using TopSpaceMAUI.Util;
using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI.Views;

public partial class PictureShelfView : ContentPage
{
    public static Type BaseType() => typeof(Page);
    private PictureShelfViewModel pictureShelfViewModel;

    private int activeRulerIndex = 0;

    private Func<PictureShelfViewModel, PictureShelfViewModel> callback;

    public PictureShelfView(PictureShelfViewModel pictureShelfViewModel)
	{
		InitializeComponent();
        this.pictureShelfViewModel = pictureShelfViewModel;
        BindingContext = this.pictureShelfViewModel;
        imgPhoto.Source = pictureShelfViewModel.Source;
    }

    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        if (this.callback != null)
        {
            this.callback(null);
        }
        await Shell.Current.GoToAsync("//VisitList/Visit");
    }

    private async void btnNewPicture_Clicked(object sender, EventArgs e)
    {
        Camera.TakePicture((result) =>
        {
            ImageSource source;
            if (result is FileResult)
            {
                source = ImageSource.FromStream(() => result.OpenReadAsync().GetAwaiter().GetResult());
            }
            else
            {
                source = ImageSource.FromFile(@"C:\repos\PH-TOP-SPACE-MAUI\TopSpaceMAUI\Resources\Images\background.png");
            }

            pictureShelfViewModel.Source = source;
            imgPhoto.Source = pictureShelfViewModel.Source;

            return true;
        });
    }

    private void btnDescartar_Clicked(object sender, EventArgs e)
    {
        IList<IView> rulers = layDisplay.Children.Where(x => x is UserResizableRuler).ToArray();
        foreach (var item in rulers)
        {
            pictureShelfViewModel.Rulers.Remove((UserResizableRuler)item);
            layDisplay.Children.Remove(item);
        }
        spanActual.Text = "0";
        //btnSalvar.IsEnabled = false;
        Ruler_ScoreUpdated(this, 0);
    }

    //private async void btnSalvar_Clicked(object sender, EventArgs e)
    //{
    //    var screenShot = await layDisplay.CaptureAsync();
    //    using (var memoryStream = new MemoryStream())
    //    {
    //        await screenShot.CopyToAsync(memoryStream, ScreenshotFormat.Png);
    //        pictureShelfViewModel.PhotoStream = memoryStream.ToArray();
    //    }

    //    if (this.callback != null)
    //    {
    //        this.callback(pictureShelfViewModel);
    //    }
    //    await Shell.Current.GoToAsync("//VisitList/Visit");
    //}

    private async void btnSalvar_Clicked(object sender, EventArgs e)
    {
        try
        {
            var screenShot = await layDisplay.CaptureAsync();
            if (screenShot == null)
            {
                await DisplayAlert("Erro", "Falha ao capturar a tela.", "OK");
                return;
            }

            using var screenStream = await screenShot.OpenReadAsync();
            using var fileStream = File.Create(pictureShelfViewModel.PhotoPath);

            await screenStream.CopyToAsync(fileStream); // Corrigido o fluxo de cópia

            if (this.callback != null)
            {
                this.callback(pictureShelfViewModel);
            }

            await Shell.Current.GoToAsync("//VisitList/Visit");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }


    private void btnNewRuler_Clicked(object sender, EventArgs e)
    {
        UserResizableRuler ruler = new UserResizableRuler();
        ruler.Activated += Ruler_Activated;
        ruler.ScoreUpdated += (Ruler_ScoreUpdated);
        layDisplay.Add(ruler);
        layDisplay.SetLayoutBounds(ruler, new Rect(100, 100, 200, 40));

        if (pictureShelfViewModel.Rulers.Count > 0)
        {
            pictureShelfViewModel.Rulers[activeRulerIndex].ToggleActive(false);
        }
        pictureShelfViewModel.Rulers.Add(ruler);
        activeRulerIndex = pictureShelfViewModel.Rulers.Count - 1;
        ruler.ToggleActive(true);
    }

    private void Ruler_Activated(object? sender, UserResizableRuler e)
    {
        int idx = pictureShelfViewModel.Rulers.IndexOf(sender as UserResizableRuler);
        UserResizableRuler ruler = pictureShelfViewModel.Rulers[activeRulerIndex];
        ruler.ToggleActive(false);

        activeRulerIndex = idx;
        ruler = pictureShelfViewModel.Rulers[activeRulerIndex];
        ruler.ToggleActive(true);
    }

    private void Ruler_ScoreUpdated(object? sender, double e)
    {
        double score = 0;
        double totalArea = 0;
        double usedArea = 0;
        foreach (UserResizableRuler ruler in pictureShelfViewModel.Rulers)
        {
            totalArea += (ruler.Width - 2 * UserResizableRuler.RulerBorder);
            usedArea += ruler.UsedWidth;
        }

        if (totalArea > 0)
        {
            score = Double.Round(usedArea / totalArea * 100, 0);
            if (score < 0) score = 0;
        }


        pictureShelfViewModel.Score = (decimal)score;
        //XUIAlertView.ShowTranslated("Score", pictureShelfViewModel.Score.ToString());
        spanActual.Text = pictureShelfViewModel.Score.ToString("0");
        //btnSalvar.IsEnabled = pictureShelfViewModel.IsOk;
        if (pictureShelfViewModel.IsOk)
        {
            btnLeft.Source = ImageSource.FromFile("ok.png");
            btnRight.Source = ImageSource.FromFile("ok.png");
            btnLeft.BackgroundColor = Color.FromHex("#32ba7c");
            btnRight.BackgroundColor = Color.FromHex("#32ba7c");
        } else
        {
            btnLeft.Source = ImageSource.FromFile("nok.png");
            btnRight.Source = ImageSource.FromFile("nok.png");
            btnLeft.BackgroundColor = Color.FromHex("#e04f5f");
            btnRight.BackgroundColor = Color.FromHex("#e04f5f");
        }
    }

    private async void btnNewArea_Clicked(object sender, EventArgs e)
    {
        if (pictureShelfViewModel.Rulers.Count > 0)
        {
            UserResizableRuler ruler = pictureShelfViewModel.Rulers[activeRulerIndex];
            if (ruler != null && ruler.UsedWidth < 10)
            {
                ruler.UsedOffset = 1;
                ruler.UsedWidth = 50;
                ruler.Invalidate();
            }
        }
    }

    private void btnDeletar_Clicked(object sender, EventArgs e)
    {
        if (pictureShelfViewModel.Rulers.Count > 0)
        {
            UserResizableRuler ruler = pictureShelfViewModel.Rulers[activeRulerIndex];
            layDisplay.Children.Remove(ruler);
            pictureShelfViewModel.Rulers.RemoveAt(activeRulerIndex);
            activeRulerIndex = pictureShelfViewModel.Rulers.Count - 1;
            Ruler_ScoreUpdated(ruler, 0);
        }
    }

    internal async Task<PictureShelfViewModel> OpenAsModal(Func<PictureShelfViewModel, PictureShelfViewModel> callback)
    {
        this.callback = callback;
        await Shell.Current.Navigation.PushModalAsync(this);

        return pictureShelfViewModel;
    }
}
