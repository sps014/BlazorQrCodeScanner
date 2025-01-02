# BlazorQrCodeScanner
 
The `QrCodeScanner` component is a Blazor component for scanning QR codes using a webcam or an image file in Blazor WASM, Server and on MAUI Hybrid.
It is build on Top of [html5-qrcode](https://github.com/mebjas/html5-qrcode). It is fully customizable and can use NativeBarcode API on supported devices on Android.


| Framework      | Compatibility |
|----------------|:-------------:|
| Blazor WASM    |       ✔️      |
| Blazor Server  |       ✔️      |
| MAUI Blazor    |       ✔️      |


## Install

1. Via [Nuget Package Manager](https://www.nuget.org/packages/BlazorQrCodeScanner) <br>
2. Via Command Line
```
     dotnet add package BlazorQrCodeScanner
```

import this in head of wwwroot/index.html
```html
    <script src="https://unpkg.com/html5-qrcode" type="text/javascript"></script>
```

### MAUI Android Setup
* For MAUI Android camera permissions will be required to be added in Android Mainfest file and create a ChromeWebClient in Platforms/Android/Handlers folder
  
```xml
    <uses-permission android:name="android.permission.CAMERA" />
 ```

Platforms/Android/Handlers/CurrentWebChromeClient.cs

```cs
﻿using Android.Webkit;
using Microsoft.AspNetCore.Components.WebView.Maui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazor.Platforms.Android.Handlers
{
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

}
```

Now register this client for android in MauiPrograms
```cs
.ConfigureMauiHandlers(handlers =>
{
#if ANDROID
     handlers.AddHandler<BlazorWebView, MauiBlazorWebViewHandler>();
#endif
};
```


## Samples
```razor
@page "/"
@using BlazorQrCodeScanner
@using BlazorQrCodeScanner.MediaTrack

<QrCodeScanner @ref="qrScanner" Style="display: flex; align-items: center; justify-content: center; overflow: hidden !important;" Width="100%" Height="100vh" OnCreated="OnScannerCreated" OnScannerStarted="OnScanStarted" OnQrSuccess="OnQrCodeScanned">
    @if (isScannerStarted)
    {
        <div id="highlightFrame" style="position: fixed; z-index: 2; width: 100vw; height: 150px; background-color: transparent; outline: 2px solid yellow;">
        </div>
    }
</QrCodeScanner>

<img src="@scannedImage" width="100%" />

@code {
    private QrCodeScanner? qrScanner;
    private bool isScannerStarted;
    private string scannedImage;

    /// <summary>
    /// Initializes the component and requests camera permissions.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        //Do native permission check on MAUI only
        if (await Permissions.CheckStatusAsync<Permissions.Camera>() != PermissionStatus.Granted)
        {
            await Permissions.RequestAsync<Permissions.Camera>();
        }
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Called when the QR code scanner instance is created.
    /// </summary>
    private void OnScannerCreated()
    {
        qrScanner?.StartAsync(new MediaTrackConstraintSet
        {
            FacingMode = VideoFacingMode.Environment
        }, new QrCodeConfig
        {
            FormatsToSupport = new[] { BarcodeType.QR_CODE },
            QrBox = new QrBoxFunction("calculateBoundingBox"), // or pass QrBoxSize width and height or aspectratio as QrBoxNumber
            ExperimentalFeatures = new ExperimentalFeaturesConfig
            {
                UseBarCodeDetectorIfSupported = true
            },
            DefaultZoomValueIfSupported = 2,
            Fps = 10
        });
    }

    /// <summary>
    /// Called when a QR code is successfully scanned.
    /// </summary>
    /// <param name="result">The result of the QR code scan.</param>
    private void OnQrCodeScanned(QrCodeScanResult result)
    {
        Console.WriteLine(result.DecodedText);
        scannedImage = result.ImageUrl;
        StateHasChanged();
    }

    /// <summary>
    /// Called when the scanner starts.
    /// </summary>
    private async void OnScanStarted()
    {
        await Task.Delay(500); // let camera element in dom get created
        isScannerStarted = true;
        StateHasChanged();
    }
}
```

```js
// in app.js imported we have viewPort calculation function
window.calculateBoundingBox= function (viewfinderWidth, viewfinderHeight) {
    let minEdgePercentage = 0.999; 
    let minEdgeSize = Math.min(viewfinderWidth, viewfinderHeight);
    let qrboxSize = Math.floor(minEdgeSize * minEdgePercentage);
    return {
        width: qrboxSize,
        height: 150,
    };
};

```

## Documentation

## Parameters

- **Class** (`string?`): CSS class to apply to the component.
- **Id** (`string`): The ID of the scanner element. Default is "reader".
- **Style** (`string?`): Inline styles to apply to the component.
- **Width** (`string`): Width of the video element. Default is "600px".
- **Height** (`string`): Height of the video element. Default is "600px".
- **VideoBackground** (`string`): Background color of the video element. Default is "black".
- **ChildContent** (`RenderFragment?`): Child content to render inside the component. (Customize Crosshair as per your need)

## Events

- **OnCreated** (`EventCallback`): Invoked when the scanner instance is created.
- **OnScannerStarted** (`EventCallback`): Invoked when the scanner and camera start scanning.
- **OnScannerStartFailed** (`EventCallback<string?>`): Invoked when the camera fails to start.
- **OnQrSuccess** (`EventCallback<QrCodeScanResult>`): Invoked when a QR code is successfully scanned.
- **OnQrScanFailed** (`EventCallback<string?>`): Invoked when QR code scanning fails.

## Public Methods

### StartAsync

```csharp
public ValueTask StartAsync(string cameraId, QrCodeConfig qrCodeConfig)
```

Starts scanning with the specified camera ID and configuration.

### StartAsync

```csharp
public ValueTask StartAsync(MediaTrackConstraintSet mediaTrackConstraintsConfig, QrCodeConfig qrCodeConfig)
```

Starts scanning with the specified media track constraints and configuration.

### ApplyVideoConstraintsAsync

```csharp
public ValueTask ApplyVideoConstraintsAsync(MediaTrackConstraintSet videoConstraintsSet)
```

Applies video constraints to the running video track.

### GetStateAsync

```csharp
public async ValueTask<Html5QrcodeScannerState> GetStateAsync()
```

Gets the current state of the camera scan.

### PauseAsync

```csharp
public ValueTask PauseAsync(bool pauseVideo = false)
```

Pauses the ongoing scan.

### ResumeAsync

```csharp
public ValueTask ResumeAsync()
```

Resumes video and scanning.

### StopAsync

```csharp
public ValueTask StopAsync()
```

Stops video and scanning.

### GetCamerasAsync

```csharp
public ValueTask<CameraDevice[]> GetCamerasAsync()
```

Gets the list of available camera devices.

### GetRunningTrackSettingsAsync

```csharp
public ValueTask<MediaTrackSettings> GetRunningTrackSettingsAsync()
```

Gets the current settings of the running video track.

### GetRunningTrackCapabilitiesAsync

```csharp
public ValueTask<MediaTrackCapabilities> GetRunningTrackCapabilitiesAsync()
```

Gets the capabilities of the running video track.

### GetRunningTrackCameraCapabilitiesAsync

```csharp
public ValueTask<CameraCapabilities> GetRunningTrackCameraCapabilitiesAsync()
```

Gets the camera capabilities of the running video track.

### ClearAsync

```csharp
public ValueTask ClearAsync()
```

Clears the existing canvas.

### ScanFileAsync

```csharp
public ValueTask ScanFileAsync(byte[] array, string contentType, bool showImage = true)
```

Scans an image file for a QR code. (will fail if webcam scanning is running)

### ScanFileV2Async

```csharp
public ValueTask ScanFileV2Async(byte[] array, string contentType, bool showImage = true)
```

Scans an image file for a QR code (version 2).  (will fail if webcam scanning is running)

## Disposal

### DisposeAsync

```csharp
public async ValueTask DisposeAsync()
```


