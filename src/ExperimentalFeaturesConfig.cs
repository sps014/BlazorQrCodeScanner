using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorQrCodeScanner;

/// <summary>
/// Configuration for enabling or disabling experimental features in the library.
/// 
/// These features will eventually upgrade to fully supported features in the
/// library.
/// </summary>
public class ExperimentalFeaturesConfig
{
    /// <summary>
    /// <c>BarcodeDetector</c> is being implemented by browsers at the moment.
    /// It has very limited browser support but as it becomes available, it could
    /// enable a faster native code scanning experience.
    /// 
    /// Set this flag to true to enable using <c>BarcodeDetector</c> if
    /// supported. This is false by default.
    /// 
    /// <c>Deprecated:</c> This configuration has graduated to
    /// <c>Html5QrcodeCameraScanConfig</c>. You can set it there directly. All
    /// documentation and future improvements will be added to that one. This
    /// config will still work for backwards compatibility.
    /// 
    /// Documentation:
    ///  - https://developer.mozilla.org/en-US/docs/Web/API/BarcodeDetector
    ///  - https://web.dev/shape-detection/#barcodedetector
    /// </summary>
    [JsonPropertyName("useBarCodeDetectorIfSupported")]
    public bool UseBarCodeDetectorIfSupported { get; init; } = false;
}
