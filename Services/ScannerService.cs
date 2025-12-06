using ZXing.Net.Maui;
using CameraViewHandler = PDAApp.Handlers.CameraViewHandler;

namespace PDAApp.Services;

public class ScannerService
{
    private TaskCompletionSource<string>? scanTaskSource;

    public ScannerService()
    {
        CameraViewHandler.BarcodeDetected += OnBarcodeDetected;
    }

    public Task<string> ScanAsync()
    {
        scanTaskSource = new TaskCompletionSource<string>();
        CameraViewHandler.ShowCamera();
        return scanTaskSource.Task;
    }

    public void CancelScan()
    {
        CameraViewHandler.HideCamera();
        scanTaskSource?.TrySetCanceled();
        scanTaskSource = null;
    }

    private void OnBarcodeDetected(object? sender, BarcodeDetectionEventArgs e)
    {
        if (e.Results?.Length > 0)
        {
            var barcode = e.Results[0].Value;
            CameraViewHandler.HideCamera();
            scanTaskSource?.TrySetResult(barcode);
        }
    }
}