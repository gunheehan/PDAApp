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
        await SetPageBackground("transparent");
        StateHasChanged();

        await Task.Delay(100);

        try
        {
            scannedBarcode = await Scanner.ScanAsync();
        }
        catch (TaskCanceledException)
        {
        }
        finally
        {
            isScanning = false;
            await SetPageBackground("#f5f5f5");
            StateHasChanged();
        }
    }

    private async Task CancelScan()
    {
        Scanner.CancelScan();
        isScanning = false;
        await SetPageBackground("#f5f5f5");
        StateHasChanged();
    }

    private void ClearResult()
    {
        scannedBarcode = null;
        StateHasChanged();
    }

    private async Task SetPageBackground(string color)
    {
        try
        {
            await JS.InvokeVoidAsync("eval",
                $"document.querySelector('.scan-page').style.setProperty('--page-background', '{color}')");
        }
        catch
        {
// JS 호출 실패 시 무시
        }
    }
}