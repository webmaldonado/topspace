using CommunityToolkit.Maui.Views;

namespace TopSpaceMAUI;

public partial class ImagePopup : Popup
{
    public ImagePopup(ImageSource imageSource)
    {
        InitializeComponent();
        FullImage.Source = imageSource;
    }

    private void ClosePopup(object sender, EventArgs e)
    {
        Close();
    }
}
