using ZXing.Net.Maui;

namespace PDAApp;

public partial class NativeScanPage : ContentPage
{
    public NativeScanPage()
    {
        InitializeComponent();
    }

    private async void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (e.Results?.Length > 0)
        {
            var barcode = e.Results[0].Value;
            
            // 결과를 전달하고 돌아가기
            await Shell.Current.GoToAsync("..", new Dictionary<string, object>
            {
                ["ScanResult"] = barcode
            });
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}