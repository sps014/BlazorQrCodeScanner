using Android.Webkit;
using Microsoft.AspNetCore.Components.WebView.Maui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorQrCodeScanner.Maui.Platforms.Android.Handlers;

internal class CurrentWebChromeClient: WebChromeClient
{
    public override void OnPermissionRequest(PermissionRequest request)
    {
        try
        {
            request.Grant(request.GetResources());
            base.OnPermissionRequest(request);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}
public class MauiBlazorWebViewHandler : BlazorWebViewHandler
{
    protected override global::Android.Webkit.WebView CreatePlatformView()
    {
        var view = base.CreatePlatformView();

        view.SetWebChromeClient(new CurrentWebChromeClient());

        return view;
    }
}