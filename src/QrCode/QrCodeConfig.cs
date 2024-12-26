using System.Text.Json.Serialization;

namespace BlazorQrCodeScanner;

/// <summary>
///  class for configuring Html5Qrcode  instance.
/// </summary>
public class QrCodeConfig
{

    /// <summary>
    /// Set zoom level on supported devices
    /// </summary>
    [JsonPropertyName("supportedScanTypes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Html5QrcodeScanType[]? SupportedScanTypes { get; init; } = null;


    /// <summary>
    /// Set zoom level on supported devices
    /// </summary>
    [JsonPropertyName("defaultZoomValueIfSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DefaultZoomValueIfSupported { get; init; } = null;
    
    /// <summary>
    /// Array of formats to support of type <c>BarcodeType</c>.
    /// 
    /// All invalid values will be ignored. If null or undefined, all supported
    /// formats will be used for scanning. Unless you want to limit the scan to
    /// only certain formats or want to improve performance, you should not set
    /// this value.
    /// </summary>
    [JsonPropertyName("formatsToSupport")]
    [JsonIgnore(Condition =JsonIgnoreCondition.WhenWritingNull)]
    public BarcodeType[]? FormatsToSupport { get; init; } = null;


    /// <summary>
    /// <c>BarcodeDetector</c> is being implemented by browsers at the moment.
    /// It has very limited browser support but as it becomes available, it could
    /// enable a faster native code scanning experience.
    /// 
    /// Set this flag to true to enable using <c>BarcodeDetector</c> if
    /// supported. This is true by default.
    /// 
    /// Documentation:
    ///  - https://developer.mozilla.org/en-US/docs/Web/API/BarcodeDetector
    ///  - https://web.dev/shape-detection/#barcodedetector
    /// </summary>
    [JsonPropertyName("useBarCodeDetectorIfSupported")]
    public bool UseBarCodeDetectorIfSupported { get; init; } = true;

    /// <summary>
    /// Configuration for experimental features.
    /// Everything is false by default.
    /// </summary>
    [JsonPropertyName("experimentalFeatures")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ExperimentalFeaturesConfig? ExperimentalFeatures {get;init;}

    /// <summary>
    /// If true, all logs will be printed to the console. False by default.
    /// </summary>
    [JsonPropertyName("verbose")]
    public bool Verbose { get;init;} = false;


    /// <summary>
    /// Optional. Expected framerate of QR code scanning. For example, <c>{ fps: 2 }</c> means the
    /// scanning would be done every 500 ms.
    /// </summary>
    [JsonPropertyName("fps")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Fps { get;init;} = null;

    /// <summary>
    /// Optional. Edge size, dimension, or calculator function for the QR scanning
    /// box. The value or computed value should be smaller than the width and
    /// height of the full region.
    /// 
    /// This would make the scanner look like this:
    ///          ----------------------
    ///          |********************|
    ///          |******,,,,,,,,,*****|      <--- shaded region
    ///          |******|       |*****|      <--- non-shaded region would be
    ///          |******|       |*****|          used for QR code scanning.
    ///          |******|_______|*****|
    ///          |********************|
    ///          |********************|
    ///          ----------------------
    /// 
    /// An instance of <c>QrDimensions</c> can be passed to construct a non-square
    /// rendering of the scanner box. You can also pass in a function of type
    /// <c>QrDimensionFunction</c> that takes in the width and height of the
    /// video stream and returns a QR box size of type <c>QrDimensions</c>.
    /// 
    /// If this value is not set, no shaded QR box will be rendered and the
    /// scanner will scan the entire area of the video stream.
    /// </summary>
    [JsonIgnore]
    public IQrBox? QrBox { get; set; } = null;

    /// <summary>
    /// Optional. Desired aspect ratio for the video feed. Ideal aspect ratios
    /// are 4:3 or 16:9. Passing an incorrect aspect ratio could lead to the video feed
    /// not showing up.
    /// </summary>
    [JsonPropertyName("aspectRatio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AspectRatio { get; init; } = null;

    /// <summary>
    /// Optional. If <c>true</c>, flipped QR Codes won't be scanned. Only use this
    /// if you are sure the camera cannot provide a mirrored feed and you are facing
    /// performance constraints.
    /// </summary>
    [JsonPropertyName("disableFlip")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DisableFlip { get; init; } = null;


    /// <summary>
    /// Optional. <c>@beta</c> (this config is not well supported yet).
    /// 
    /// Important: When passed, this will override other parameters like
    /// 'cameraIdOrConfig' or configurations like 'aspectRatio'.
    /// 'videoConstraints' should be of type <c>MediaTrackConstraints</c> as
    /// defined in
    /// https://developer.mozilla.org/en-US/docs/Web/API/MediaTrackConstraints
    /// and is used to specify a variety of video or camera controls like:
    /// aspectRatio, facingMode, frameRate, etc.
    /// </summary>
    [JsonPropertyName("videoConstraints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? VideoConstraints { get; init; } = null;
}

public interface IQrBox;
public class QrBoxNumber : IQrBox
{
    public double Ratio { get; init; } 

    public QrBoxNumber(double ratio)
    {
        Ratio = ratio;
    }
}
public class QrBoxSize : IQrBox
{
    public double Width { get; set; }
    public double Height { get; set; }

    public QrBoxSize(double width, double height)
    {
        Width = width; Height = height;
    }
}

/// <summary>
/// Call A JS function to evaluate size , function will take viewPortWidth and ViewPortHeight as Parameter,  <b>this function must be exposed on window object</b>
/// </summary>
public class QrBoxFunction:IQrBox
{
    [JsonPropertyName("jSFunctionName")]
    public string JSFunctionName { get; init; }

    /// <summary>
    /// Takes JS Window function name that will be used for Qrbox calculation. <b>this function must be exposed on window object</b>
    /// </summary>
    /// <param name="jsFunctionNameOnWindowToCall"> <b>this function must be exposed on js window object</b></param>
    public QrBoxFunction(string jsFunctionNameOnWindowToCall)
    {
        this.JSFunctionName = jsFunctionNameOnWindowToCall;
    }
}

/// <summary>
/// Types of scans supported by the library.
/// </summary>
public enum Html5QrcodeScanType
{
    /// <summary>
    /// Camera based scanner.
    /// </summary>
    SCAN_TYPE_CAMERA = 0,

    /// <summary>
    /// File based scanner.
    /// </summary>
    SCAN_TYPE_FILE = 1
}
