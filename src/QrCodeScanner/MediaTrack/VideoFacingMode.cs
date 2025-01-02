using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorQrCodeScanner.MediaTrack;


[JsonConverter(typeof(VideoFacingModeConverter))]
public enum VideoFacingMode
{
    /// <summary>
    /// The source is facing toward the user (a self-view camera).
    /// </summary>
    User,

    /// <summary>
    /// The source is facing away from the user (viewing the environment).
    /// </summary>
    Environment,

    /// <summary>
    /// The source is facing to the left of the user.
    /// </summary>
    Left,

    /// <summary>
    /// The source is facing to the right of the user.
    /// </summary>
    Right
}


internal class VideoFacingModeConverter : JsonConverter<VideoFacingMode>
{
    public override VideoFacingMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return value switch
        {
            "user" => VideoFacingMode.User,
            "environment" => VideoFacingMode.Environment,
            "left" => VideoFacingMode.Left,
            "right" => VideoFacingMode.Right,
            _ => throw new ArgumentException($"The value '{value}' was not valid for enum VideoFacingMode")
        };
    }

    public override void Write(Utf8JsonWriter writer, VideoFacingMode value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().ToLower());
    }
}
