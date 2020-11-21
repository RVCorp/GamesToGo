using Android.App;
using Android.Content.PM;
using osu.Framework;
using osu.Framework.Android;

namespace GamesToGo.App
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, SupportsPictureInPicture = false, HardwareAccelerated = false)]
    public class MainActivity : AndroidGameActivity
    {
        protected override Game CreateGame()
        {
            return new GamesToGoGame();
        }
    }
}
