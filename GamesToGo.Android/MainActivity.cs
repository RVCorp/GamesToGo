using Android.App;
using Android.Content.PM;
using Android.Views;
using GamesToGo.Game;
using osu.Framework.Android;

namespace GamesToGo.Android
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, SupportsPictureInPicture = false, HardwareAccelerated = false)]
    public class MainActivity : AndroidGameActivity
    {
        protected override osu.Framework.Game CreateGame()
        {
            UIVisibilityFlags = SystemUiFlags.Visible;
            return new GamesToGoGame();
        }
    }
}
