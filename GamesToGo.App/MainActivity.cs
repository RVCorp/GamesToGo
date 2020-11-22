using Android.App;
using Android.Content.PM;
using Android.Views;
using osu.Framework;
using osu.Framework.Android;

namespace GamesToGo.App
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, SupportsPictureInPicture = false, HardwareAccelerated = false)]
    public class MainActivity : AndroidGameActivity
    {

        protected override Game CreateGame()
        {
            UIVisibilityFlags = SystemUiFlags.Visible;
            return new GamesToGoGame();
        }
    }
}
