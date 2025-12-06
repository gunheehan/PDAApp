namespace PDAApp.Services;

public class NavigationService
{
    public async Task NavigateToNativeScanPage()
    {
        await Shell.Current.GoToAsync(nameof(NativeScanPage));
    }
}