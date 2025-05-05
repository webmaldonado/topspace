using Android.App;
using Android.Content;
using Microsoft.Identity.Client;

namespace TopSpaceMAUI.Platforms.Android
{
    [Activity(Exported = true)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
        DataHost = "auth",
        DataScheme = "msal{30263f9a-5f26-4ece-9e1e-77f69e722213}")]
    public class MsalActivity : BrowserTabActivity
    {

    }
}
