using System.Buffers.Text;
using System.Text.Json.Serialization;
using BlazorQrCodeScanner.MediaTrack;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace BlazorQrCodeScanner;

public partial class QrCodeScanner:ComponentBase,IAsyncDisposable
{
    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string Id { get; set; } = "reader";

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public string Width { get; set; } = "600px";

    [Parameter]
    public string Height { get; set; } = "600px";

    [Parameter]
    public string VideoBackground { get; set; } = "black";


    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private IJSObjectReference? qrCodeJSObjectReference;

    private QrDotnetRuntimeContext qrDotnetRuntimeContext;


    /// <summary>
    /// When instance is created for scanner
    /// </summary>
    [Parameter]
    public EventCallback OnCreated { get; set; }

    /// <summary>
    /// Called when scanner and camera is started for scanning
    /// </summary>
    [Parameter]
    public EventCallback OnScannerStarted { get; set; }


    /// <summary>
    /// Called when Camera starting fails
    /// </summary>
    [Parameter]
    public EventCallback<string?> OnScannerStartFailed { get; set; }

    /// <summary>
    /// Get called when qr scanning is successful
    /// </summary>
    [Parameter]
    public EventCallback<QrCodeScanResult> OnQrSuccess { get; set; }

    /// <summary>
    /// Get called when QrScan results in error
    /// </summary>
    [Parameter]
    public EventCallback<string?> OnQrScanFailed { get; set; }

    private bool onStartedCamera = false;
    protected override async Task OnInitializedAsync()
    {
        await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                    "/_content/BlazorQrCodeScanner/html5-qrcode.min.js");

        await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                    "/_content/BlazorQrCodeScanner/qrcodeScanner.js");

        qrDotnetRuntimeContext = new QrDotnetRuntimeContext();

        qrDotnetRuntimeContext.OnScannerStarted += ScannerStarted;
        qrDotnetRuntimeContext.OnScannerStartFailed += ScannerStartFailed;
        qrDotnetRuntimeContext.OnQrScanFailed += QrScanFailed;
        qrDotnetRuntimeContext.OnQrSuccess += QrSuccess;


        await JSRuntime.InvokeVoidAsync("createScanner", Id);
        await OnCreated.InvokeAsync();
    }

    private void QrScanFailed(object? sender, string? e)
    {
        OnQrScanFailed.InvokeAsync(e);
    }

    private void QrSuccess(object? sender, QrCodeScanResult e)
    {
        OnQrSuccess.InvokeAsync(e);
    }
    private async void ScannerStarted(object? sender, EventArgs e)
    {
        onStartedCamera = true;
        await OnScannerStarted.InvokeAsync();
        await SetWidthHeightBackgroundOfVideo();
    }

    private void ScannerStartFailed(object? sender, string? e)
    {
        OnScannerStartFailed.InvokeAsync(e);
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        foreach (var parameter in parameters)
        {
            if (onStartedCamera && (parameter.Name == nameof(Width) || parameter.Name == nameof(Height) ||
                parameter.Name == nameof(VideoBackground)))
            {
                 SetWidthHeightBackgroundOfVideo().ConfigureAwait(false);
            }
        }
        await base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Start Scanning with cameraId, raises OnScanningStartedEvent
    /// </summary>
    /// <param name="cameraId">CameraId to use</param>

    public ValueTask StartAsync(string cameraId, QrCodeConfig qrCodeConfig)
    {
        return StartInternalAsync(cameraId, qrCodeConfig);
    }

    /// <summary>
    /// Start Scanning with MediaTrackConstraints
    /// </summary>
    /// <param name="mediaTrackConstraintsConfig">The MediaTrackConstraints dictionary is used to describe a set of capabilities and the value or values each can take on. See https://developer.mozilla.org/en-US/docs/Web/API/MediaTrackConstraints</param>
    public ValueTask StartAsync(MediaTrackConstraintSet mediaTrackConstraintsConfig, QrCodeConfig qrCodeConfig)
    {
        return StartInternalAsync(mediaTrackConstraintsConfig, qrCodeConfig);
    }

    /// <summary>
    /// Applies video constraints on the running video track from the camera.
    /// 
    /// <b>Important:
    /// - Must be called only if the camera-based scanning is in progress.
    /// - Changing aspectRatio while the scanner is running is not yet supported.</b>
    /// </summary>
    public ValueTask ApplyVideoConstraintsAsync(MediaTrackConstraintSet videoConstraintsSet)
    {
        return JSRuntime.InvokeVoidAsync("applyVideoConstraintsScanner",Id, videoConstraintsSet);
    }

    /// <summary>
    /// Gets state of the camera scan.
    /// </summary>
    /// <returns>Enum current state</returns>
    public async ValueTask<Html5QrcodeScannerState> GetStateAsync()
    {
        var r = await JSRuntime.InvokeAsync<int>("getStateScanner", Id);
        return (Html5QrcodeScannerState)r;
    }

    /// <summary>
    /// Pauses the ongoing scan.
    /// error if method is called when scanner is not in scanning state.
    /// </summary>
    /// <param name="pauseVideo"></param>
    public ValueTask PauseAsync(bool pauseVideo= false)
    {
        return JSRuntime.InvokeVoidAsync("pauseScanner", Id,pauseVideo);
    }

    /// <summary>
    /// Resume video and scanning , throws error if scanner not in pause state
    /// </summary>
    /// <returns></returns>
    public ValueTask ResumeAsync()
    {
        return JSRuntime.InvokeVoidAsync("resumeScanner", Id);
    }

    /// <summary>
    /// stop video and scanning , throws error if scanner  in already in stopped state
    /// </summary>
    /// <returns></returns>
    public ValueTask StopAsync()
    {
        return JSRuntime.InvokeVoidAsync("stopScanner", Id);
    }

    /// <summary>
    /// Get the camera devices list
    /// </summary>
    /// <returns></returns>
    public ValueTask<CameraDevice[]> GetCamerasAsync()
    {
        return JSRuntime.InvokeAsync<CameraDevice[]>("getCamerasScanner");
    }

    /// <summary>
    /// Returns the object containing the current values of each constrainable property of the running video track.
    /// </summary>
    /// <returns></returns>
    public ValueTask<MediaTrackSettings> GetRunningTrackSettingsAsync()
    {
        return JSRuntime.InvokeAsync<MediaTrackSettings>("getRunningTrackSettingsScanner",Id);
    }

    /// <summary>
    /// Returns the capabilities of the running video track.
    /// </summary>
    /// <returns></returns>
    public ValueTask<MediaTrackCapabilities> GetRunningTrackCapabilitiesAsync()
    {
        return JSRuntime.InvokeAsync<MediaTrackCapabilities>("getRunningTrackCapabilitiesScanner", Id);
    }

    /// <summary>
    /// Returns the capabilities of the running video track.
    /// </summary>
    /// <returns></returns>
    public ValueTask<CameraCapabilities> GetRunningTrackCameraCapabilitiesAsync()
    {
        return JSRuntime.InvokeAsync<CameraCapabilities>("getRunningTrackCameraCapabilitiesScanner", Id);
    }
    
    /// <summary>
    /// Clears the existing canvas.
    /// 
    /// Note: In case of an ongoing webcam-based scan, it needs to be explicitly closed before calling this method, 
    /// else it will throw an exception.
    /// </summary>
    public ValueTask ClearAsync()
    {
        return JSRuntime.InvokeVoidAsync("clearScanner", Id);
    }

    /// <summary>
    /// Scans an Image File for QR Code.
    /// This feature is mutually exclusive to camera-based scanning, you should call stop() if the camera-based scanning was ongoing.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public ValueTask ScanFileAsync(byte[] array,string contentType, bool showImage = true)
    {
        return JSRuntime.InvokeVoidAsync("scanFileScanner", Id,array,contentType,
            qrDotnetRuntimeContext.QrDotNetObjectReference,showImage);
    }
    /// <summary>
    /// Scans an Image File for QR Code.
    /// This feature is mutually exclusive to camera-based scanning, you should call stop() if the camera-based scanning was ongoing.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public ValueTask ScanFileV2Async(byte[] array, string contentType,bool showImage = true)
    {
        return JSRuntime.InvokeVoidAsync("scanFileV2Scanner", Id, array, contentType, 
            qrDotnetRuntimeContext.QrDotNetObjectReference, showImage);
    }

    private ValueTask StartInternalAsync<T>(T mediaTrackConstraintsConfig, QrCodeConfig qrCodeConfig)
    {
        var qrBoxType = GetTypeOfQrBox(qrCodeConfig);

        return JSRuntime!.InvokeVoidAsync("startScanner", Id, mediaTrackConstraintsConfig, qrCodeConfig,
                    qrCodeConfig.QrBox, qrBoxType,
                    qrDotnetRuntimeContext.QrDotNetObjectReference);
    }


    private int GetTypeOfQrBox(QrCodeConfig qrCodeConfig)
    {
        if (qrCodeConfig.QrBox == null)
            return 0;
        else if (qrCodeConfig.QrBox is QrBoxNumber)
            return 1;
        else if (qrCodeConfig.QrBox is QrBoxSize)
            return 2;
        else if (qrCodeConfig.QrBox is QrBoxFunction)
            return 3;

        throw new NotImplementedException();
    }

    private ValueTask SetWidthHeightBackgroundOfVideo()
    {
        return JSRuntime.InvokeVoidAsync("setWidthHeightOfVideo", Id, Width, Height, VideoBackground);
    }

    public async ValueTask DisposeAsync()
    {
        if (qrCodeJSObjectReference != null)
        {
            await JSRuntime.InvokeVoidAsync("disposeScanner", Id);
            await qrCodeJSObjectReference.DisposeAsync();
        }

        qrDotnetRuntimeContext.OnScannerStarted -= ScannerStarted;
        qrDotnetRuntimeContext.Dispose();
    }
}

public enum Html5QrcodeScannerState
{
    /// <summary>
    /// Invalid internal state, do not set to this state.
    /// </summary>
    UNKNOWN = 0,

    /// <summary>
    /// Indicates the scanning is not running or user is using file-based scanning.
    /// </summary>
    NOT_STARTED = 1,

    /// <summary>
    /// Camera scan is running.
    /// </summary>
    SCANNING,

    /// <summary>
    /// Camera scan is paused but camera is running.
    /// </summary>
    PAUSED
}

public class CameraDevice
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; }
}