using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorQrCodeScanner;

public class QrCodeScanResult
{
    [JsonPropertyName("decodedText")]
    public string DecodedText { get; set; }

    [JsonPropertyName("result")]
    public ResultData? Result { get; set; }
    
    public string? ImageUrl { get; set; }
}

public class ResultData
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("format")]
    public FormatData? Format { get; set; }

    [JsonPropertyName("debugData")]
    public DebugData? DebugData { get; set; }
}

public class FormatData
{
    [JsonPropertyName("format")]
    public int Format { get; set; }

    [JsonPropertyName("formatName")]
    public string FormatName { get; set; }
}

public class DebugData
{
    [JsonPropertyName("decoderName")]
    public string? DecoderName { get; set; }
}