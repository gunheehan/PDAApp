using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace PDAApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        // ScannerService에 결과 전달
        var scannerService = IPlatformApplication.Current?.Services.GetService<Services.ScannerService>();
        scannerService?.HandleActivityResult(requestCode, resultCode, data);
    }
}