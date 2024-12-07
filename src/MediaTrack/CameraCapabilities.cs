using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorQrCodeScanner.MediaTrack;


public class CameraCapabilities
{
    [JsonPropertyName("torch")]
    public TorchSettings? Torch { get; set; }

    [JsonPropertyName("zoom")]
    public ZoomSettings? Zoom { get; set; }
}

public class TorchSettings
{
    [JsonPropertyName("isSupported")]
    public bool? IsSupported { get; set; }

    [JsonPropertyName("value")]
    public bool? Value { get; set; }
}

public class ZoomSettings
{

    [JsonPropertyName("isSupported")]
    public bool IsSupported { get; set; }

    [JsonPropertyName("min")]
    public int Min { get; set; }

    [JsonPropertyName("max")]
    public int Max { get; set; }

    [JsonPropertyName("step")]
    public int Step { get; set; }
}