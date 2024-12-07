using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorQrCodeScanner.Types;

/// <summary>
/// A range for <see cref="double"/> values.
/// </summary>
/// <remarks><see href="https://www.w3.org/TR/mediacapture-streams/#dom-doublerange">See the API definition here</see>.</remarks>
public class DoubleRange
{
    /// <summary>
    /// The maximum valid value of this property.
    /// </summary>
    [JsonPropertyName("max")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double Max { get; set; }

    /// <summary>
    /// The minimum value of this Property.
    /// </summary>
    [JsonPropertyName("min")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double Min { get; set; }
}