using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using GamesToGo.Game;
using osu.Framework.Android;

namespace GamesToGo.Android
{
    [Activity(Label = "GamesToGo", Theme = "@android:style/Theme.NoTitleBar", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, SupportsPictureInPicture = false, HardwareAccelerated = false)]
    public class MainActivity : AndroidGameActivity
    {
        protected override osu.Framework.Game CreateGame()
        {
            return new GamesToGoGame();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            UIVisibilityFlags = SystemUiFlags.Visible;
        }
    }
}
