#if ANDROID
using BlazorQrCodeScanner.Maui.Platforms.Android.Handlers;
#endif


using Microsoft.AspNetCore.Components.WebView.Maui;

namespace BlazorQrCodeScanner.Maui;

// All the code in this file is included in all platforms.
public static class MauiHost
{
    public static MauiAppBuilder ConfigureMauiQrCodeScanner(this MauiAppBuilder builder)
    {
        return builder.ConfigureMauiHandlers(handlers =>
        {
            #if ANDROID
                handlers.AddHandler<BlazorWebView, MauiBlazorWebViewHandler>();
            #endif
        });
    }
}