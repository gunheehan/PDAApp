using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using ZXing.Mobile;

namespace PDAApp.Platforms.Android;

[Activity(Label = "바코드 스캔",
          Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
          ScreenOrientation = ScreenOrientation.Portrait,
          ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
public class ScanActivity : Activity
{
    private MobileBarcodeScanner? _scanner;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // ZXing 초기화
        MobileBarcodeScanner.Initialize(Application);

        StartScanning();
    }

    private async void StartScanning()
    {
        _scanner = new MobileBarcodeScanner();

        // 스캔 옵션
        var options = new MobileBarcodeScanningOptions
        {
            PossibleFormats = new List<ZXing.BarcodeFormat>
            {
                ZXing.BarcodeFormat.EAN_13,
                ZXing.BarcodeFormat.EAN_8,
                ZXing.BarcodeFormat.UPC_A,
                ZXing.BarcodeFormat.CODE_128,
                ZXing.BarcodeFormat.QR_CODE
            },
            CameraResolutionSelector = new MobileBarcodeScanningOptions.CameraResolutionSelectorDelegate(SelectHighestResolution),
            UseNativeScanning = true,
            AutoRotate = false,
            TryHarder = true,
            TryInverted = true,
            PureBarcode = false,
            DelayBetweenAnalyzingFrames = 3000,
            InitialDelayBeforeAnalyzingFrames = 300
        };

        // 커스텀 오버레이
        _scanner.TopText = "바코드를 프레임 안에 맞춰주세요";
        _scanner.BottomText = "자동으로 인식됩니다";
        _scanner.FlashButtonText = "플래시";

        _scanner.UseCustomOverlay = false;

        // 스캔 시작 (카메라 화면이 자동으로 표시됨)
        var result = await _scanner.Scan(options);

        if (result != null && !string.IsNullOrEmpty(result.Text))
        {
            // 결과를 Intent로 반환
            var resultIntent = new Intent();
            resultIntent.PutExtra("barcode", result.Text);
            resultIntent.PutExtra("format", result.BarcodeFormat.ToString());
            SetResult(Result.Ok, resultIntent);
        }
        else
        {
            // 취소됨
            SetResult(Result.Canceled);
        }

        Finish();
    }
    
    private CameraResolution SelectHighestResolution(List<CameraResolution> availableResolutions)
    {
        // 가장 높은 해상도 선택
        return availableResolutions
            .OrderByDescending(r => r.Width * r.Height)
            .FirstOrDefault() ?? new CameraResolution { Width = 1920, Height = 1080 };
    }
    private CameraResolution SelectLowestResolutionMatchingDisplayAspectRatio(List<CameraResolution> availableResolutions)
    {
        CameraResolution? result = null;
        var aspectTolerance = 0.1;
        var displayOrientationHeight = Resources?.DisplayMetrics?.HeightPixels ?? 0;
        var displayOrientationWidth = Resources?.DisplayMetrics?.WidthPixels ?? 0;

        var targetRatio = (double)displayOrientationHeight / displayOrientationWidth;
        var targetHeight = displayOrientationHeight;
        var minDiff = double.MaxValue;

        foreach (var r in availableResolutions)
        {
            var ratio = (double)r.Width / r.Height;
            if (Math.Abs(ratio - targetRatio) < aspectTolerance)
            {
                var diff = Math.Abs(r.Width - targetHeight);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    result = r;
                }
            }
        }

        return result ?? availableResolutions.FirstOrDefault() ?? new CameraResolution { Width = 640, Height = 480 };
    }

    protected override void OnDestroy()
    {
        _scanner?.Cancel();
        base.OnDestroy();
    }
}