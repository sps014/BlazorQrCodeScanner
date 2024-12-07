//copied from https://github.com/KristofferStrube/Blazor.MediaCaptureStreams/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorQrCodeScanner;


/// <summary>
/// Each member of a <see cref="MediaTrackConstraintSet"/> corresponds to a constrainable property and specifies a subset of the property's valid <see cref="MediaTrackCapabilities"/> values. Applying a <see cref="MediaTrackConstraintSet"/> instructs the User Agent to restrict the settings of the corresponding constrainable properties to the specified values or ranges of values.
/// </summary>
/// <remarks><see href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackconstraintset">See the API definition here</see>.</remarks>
public class MediaTrackConstraintSet
{
    /// <summary>
    /// The width, in pixels.
    /// </summary>
    [JsonPropertyName("width")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Width { get; set; }

    /// <summary>
    /// The height, in pixels.
    /// </summary>
    [JsonPropertyName("height")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Height { get; set; }

    /// <summary>
    /// The exact aspect ratio (width in pixels divided by height in pixels, represented as a double rounded to the tenth decimal place) or aspect ratio range.
    /// </summary>
    [JsonPropertyName("aspectRatio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? AspectRatio { get; set; }

    /// <summary>
    /// The frame rate (frames per second).
    /// </summary>
    [JsonPropertyName("frameRate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? FrameRate { get; set; }

    /// <summary>
    /// This is one of the members of <see cref="VideoFacingMode"/>. The members describe the directions that the camera can face, as seen from the user's perspective.
    /// </summary>
    /// <remarks>
    /// Note that <see cref="MediaStreamTrack.GetConstraintsAsync"/> may not return exactly the same string for strings not in this enum. This preserves the possibility of using a future version of WebIDL enum for this property.
    /// </remarks>
    [JsonPropertyName("facingMode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VideoFacingMode? FacingMode { get; set; }

    /// <summary>
    /// This is one of the members of <see cref="VideoResizeMode"/>. The members describe the means by which the resolution can be derived by the User Agent. In other words, whether the UA is allowed to use cropping and downscaling on the camera output.
    /// </summary>
    /// <remarks>
    /// Note that <see cref="MediaStreamTrack.GetConstraintsAsync"/> may not return exactly the same string for strings not in this enum. This preserves the possibility of using a future version of WebIDL enum for this property.
    /// </remarks>
    [JsonPropertyName("resizeMode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VideoResizeMode? ResizeMode { get; set; }

    /// <summary>
    /// The sample rate in samples per second for the audio data.
    /// </summary>
    [JsonPropertyName("sampleRate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? SampleRate { get; set; }

    /// <summary>
    /// The linear sample size in bits. As a constraint, it can only be satisfied for audio devices that produce linear samples.
    /// </summary>
    [JsonPropertyName("sampleSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? SampleSize { get; set; }

    /// <summary>
    /// When one or more audio streams is being played in the processes of various microphones, it is often desirable to attempt to remove all the sound being played from the input signals recorded by the microphones. This is referred to as echo cancellation. There are cases where it is not needed and it is desirable to turn it off so that no audio artifacts are introduced. This allows applications to control this behavior.
    /// </summary>
    [JsonPropertyName("echoCancellation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EchoCancellation { get; set; }

    /// <summary>
    /// Automatic gain control is often desirable on the input signal recorded by the microphone. There are cases where it is not needed and it is desirable to turn it off so that the audio is not altered. This allows applications to control this behavior.
    /// </summary>
    [JsonPropertyName("autoGainControl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AutoGainControl { get; set; }

    /// <summary>
    /// Noise suppression is often desirable on the input signal recorded by the microphone. There are cases where it is not needed and it is desirable to turn it off so that the audio is not altered. This allows applications to control this behavior.
    /// </summary>
    [JsonPropertyName("noiseSuppression")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? NoiseSuppression { get; set; }

    /// <summary>
    /// The latency or latency range, in seconds. The latency is the time between start of processing (for instance, when sound occurs in the real world) to the data being available to the next step in the process. Low latency is critical for some applications; high latency may be acceptable for other applications because it helps with power constraints. The number is expected to be the target latency of the configuration; the actual latency may show some variation from that.
    /// </summary>
    [JsonPropertyName("latency")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Latency { get; set; }

    /// <summary>
    /// The number of independent channels of sound that the audio data contains, i.e. the number of audio samples per sample frame.
    /// </summary>
    [JsonPropertyName("channelCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? ChannelCount { get; set; }

    /// <summary>
    /// The identifier of the device generating the content of the <see cref="MediaStreamTrack"/>.
    /// </summary>
    /// <remarks>
    /// This property can be used for initial media selection with <see cref="MediaDevices.GetUserMediaAsync(MediaStreamConstraints)"/>. However, it is not useful for subsequent media control with <see cref="MediaStreamTrack.ApplyContraintsAsync(MediaTrackConstraints?)"/>, since any attempt to set a different value will result in an unsatisfiable <see cref="MediaTrackConstraintSet"/>. If a string of length 0 is used as a deviceId value constraint with <see cref="MediaDevices.GetUserMediaAsync(MediaStreamConstraints)"/>, it may be interpreted as if the constraint is not specified.
    /// </remarks>
    [JsonPropertyName("deviceId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DeviceId { get; set; }

    /// <summary>
    /// The document-unique group identifier for the device generating the content of the <see cref="MediaStreamTrack"/>.
    /// </summary>
    /// <remarks>
    /// Note that the setting of this property is uniquely determined by the source that is attached to the <see cref="MediaStreamTrack"/>. Since this property is not stable between browsing sessions, its usefulness for initial media selection with <see cref="MediaDevices.GetUserMediaAsync(MediaStreamConstraints)"/> is limited. It is not useful for subsequent media control with <see cref="MediaStreamTrack.ApplyContraintsAsync(MediaTrackConstraints?)"/>, since any attempt to set a different value will result in an unsatisfiable <see cref="MediaTrackConstraintSet"/>.
    /// </remarks>
    [JsonPropertyName("groupId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? GroupId { get; set; }


}

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


[JsonConverter(typeof(VideoResizeModeConverter))]
public enum VideoResizeMode
{
    /// <summary>
    /// This resolution and frame rate is offered by the camera, its driver, or the OS.
    /// </summary>
    /// <remarks>
    /// The User Agent may report this value to disguise concurrent use, but only when the camera is in use in another navigable.
    /// </remarks>
    None,

    /// <summary>
    /// This resolution is downscaled and/or cropped from a higher camera resolution by the User Agent, or its frame rate is decimated by the User Agent. The media must not be upscaled, stretched or have fake data created that did not occur in the input source.
    /// </summary>
    CropAndScale
}

internal class VideoResizeModeConverter : JsonConverter<VideoResizeMode>
{
    public override VideoResizeMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return value switch
        {
            "none" => VideoResizeMode.None,
            "crop-and-scale" => VideoResizeMode.CropAndScale,
            _ => throw new ArgumentException($"The value '{value}' was not valid for enum VideoResizeMode")
        };
    }

    public override void Write(Utf8JsonWriter writer, VideoResizeMode value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            VideoResizeMode.None => "none",
            VideoResizeMode.CropAndScale => "crop-and-scale",
            _ => throw new ArgumentException("The enum value 'value' was not valid for enum VideoResizeMode")
        });
    }
}