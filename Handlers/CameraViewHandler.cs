using Microsoft.Maui.Handlers;
using ZXing.Net.Maui.Controls;
using ZXing.Net.Maui;
using Microsoft.AspNetCore.Components.WebView.Maui;

namespace PDAApp.Handlers;

public class CameraViewHandler
{
    private static CameraBarcodeReaderView? _currentCamera;
    private static Grid? _parentGrid;
    private static BlazorWebView? _blazorWebView;
    public static event EventHandler<BarcodeDetectionEventArgs>? BarcodeDetected;

    public static void CreateCameraView(Grid parentGrid, BlazorWebView blazorWebView)
    {
#if ANDROID || IOS
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_currentCamera != null)
                return;

            _parentGrid = parentGrid;
            _blazorWebView = blazorWebView;

            _currentCamera = new CameraBarcodeReaderView
            {
                IsDetecting = false,
                IsVisible = false,
                Options = new BarcodeReaderOptions
                {
                    Formats = BarcodeFormats.All,
                    AutoRotate = true,
                    Multiple = false
                }
            };

            _currentCamera.BarcodesDetected += (s, e) =>
            {
                BarcodeDetected?.Invoke(s, e);
            };

            // 카메라를 BlazorWebView 뒤에 삽입
            parentGrid.Children.Insert(0, _currentCamera);
        });
#endif
    }

    public static void ShowCamera()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_currentCamera != null && _blazorWebView != null)
            {
                // 카메라 표시
                _currentCamera.IsVisible = true;
                _currentCamera.IsDetecting = true;
                
                // BlazorWebView의 배경을 투명하게
                _blazorWebView.BackgroundColor = Colors.Transparent;
            }
        });
    }

    public static void HideCamera()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_currentCamera != null && _blazorWebView != null)
            {
                _currentCamera.IsVisible = false;
                _currentCamera.IsDetecting = false;
                
                // BlazorWebView 배경 복원
                _blazorWebView.BackgroundColor = null;
            }
        });
    }
}