using Microsoft.AspNetCore.Components.WebView.Maui;
using PDAApp.Handlers;

namespace PDAApp;

public partial class MainPage : ContentPage
{
    private BlazorWebView? _blazorWebView;

    public MainPage()
    {
        InitializeComponent();
        InitializeBlazorWebView();
        Loaded += OnPageLoaded;
    }

    private void InitializeBlazorWebView()
    {
        _blazorWebView = new BlazorWebView
        {
            HostPage = "wwwroot/index.html"
        };

        _blazorWebView.RootComponents.Add(new RootComponent
        {
            Selector = "#app",
            ComponentType = typeof(Components.Routes)
        });

        // BlazorWebView를 Grid에 추가
        MainGrid.Children.Add(_blazorWebView);
    }

    private void OnPageLoaded(object? sender, EventArgs e)
    {
        // 카메라 뷰를 BlazorWebView 뒤에 추가
        CameraViewHandler.CreateCameraView(MainGrid, _blazorWebView);
    }
}