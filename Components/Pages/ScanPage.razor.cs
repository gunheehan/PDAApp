using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PDAApp.Components.Pages;

public partial class ScanPage : ComponentBase
{
    private bool isScanning = false;
    private string? scannedBarcode;

    private async Task StartScan()
    {
        isScanning = true;
        StateHasChanged();

        try
        {
            scannedBarcode = await Scanner.ScanAsync();
        }
        catch (TaskCanceledException)
        {
// 사용자가 취소함
        }
        finally
        {
            isScanning = false;
            StateHasChanged();
        }
    }

    private void ClearResult()
    {
        scannedBarcode = null;
        StateHasChanged();
    }
}