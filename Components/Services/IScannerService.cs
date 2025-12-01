using System.Threading.Tasks;

namespace PDAApp.Components.Services;

public interface IScannerService
{
    Task<string?> ScanBarcodeAsync();
}