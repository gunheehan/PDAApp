
namespace PDAApp.Services;

public class ScannerService
{
    private TaskCompletionSource<string>? _scanTaskSource;

    public Task<string> ScanAsync()
    {
        _scanTaskSource = new TaskCompletionSource<string>();

#if ANDROID
        LaunchNativeScanActivity();
#else
        _scanTaskSource.TrySetCanceled();
#endif

        return _scanTaskSource.Task;
    }

    public void CancelScan()
    {
        _scanTaskSource?.TrySetCanceled();
        _scanTaskSource = null;
    }

#if ANDROID
    private void LaunchNativeScanActivity()
    {
        var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
        if (activity != null)
        {
            var intent = new global::Android.Content.Intent(activity, typeof(Platforms.Android.ScanActivity));
            activity.StartActivityForResult(intent, 1001);
        }
        else
        {
            _scanTaskSource?.TrySetCanceled();
        }
    }

    public void HandleActivityResult(int requestCode, global::Android.App.Result resultCode, global::Android.Content.Intent? data)
    {
        if (requestCode == 1001)
        {
            if (resultCode == global::Android.App.Result.Ok && data != null)
            {
                var barcode = data.GetStringExtra("barcode");
                var format = data.GetStringExtra("format");
                
                System.Diagnostics.Debug.WriteLine($"바코드 인식: {barcode} (형식: {format})");
                
                _scanTaskSource?.TrySetResult(barcode ?? "");
            }
            else
            {
                _scanTaskSource?.TrySetCanceled();
            }
        }
    }
#endif
}