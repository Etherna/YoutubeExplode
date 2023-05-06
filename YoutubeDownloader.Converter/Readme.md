# Attention, Etherna's YoutubeDownloader is a fork!

This is a fork of the original [YoutubeExplode](https://github.com/Tyrrrz/YoutubeExplode) project.  
We kept all the good that original project has, with some useful feature addition.  
Currently we decided to make only little edits, where really necessary. We will try to keep updated with mergings from original repository.

Anyway, we also removed any hate speech from the original project.  
We do not share hatred towards anyone, and we think that mutual respect is the basis of every civil community.

We started our versioning from the same as the original project, but it may differ in the future.  
We are addopting SemVer.

## Issue reports

If you've discovered a bug with our implementation, or have an idea for a new feature, please report it to our issue manager based on Jira https://etherna.atlassian.net/projects/YEF.  
Instead, if also original project is affected, please report it to the original [YoutubeExplode](https://github.com/Tyrrrz/YoutubeExplode) GitHub's page.

Detailed reports with stack traces, actual and expected behaviours are welcome.

## Questions? Problems?

For questions or problems please write an email to [info@etherna.io](mailto:info@etherna.io).

---

# YoutubeExplode.Converter

[![Version](https://img.shields.io/nuget/v/YoutubeExplode.svg)](https://nuget.org/packages/YoutubeExplode.Converter)
[![Downloads](https://img.shields.io/nuget/dt/YoutubeExplode.svg)](https://nuget.org/packages/YoutubeExplode.Converter)

**YoutubeExplode.Converter** is an extension package for **YoutubeExplode** that provides the capability to download YouTube videos by muxing separate streams into a single file.
This package relies on [FFmpeg](https://ffmpeg.org) under the hood.

## Install

- 📦 [NuGet](https://nuget.org/packages/YoutubeExplode.Converter): `dotnet add package YoutubeExplode.Converter`

> **Warning**:
> This package requires the [FFmpeg CLI](https://ffmpeg.org) to work, which can be downloaded [here](https://ffbinaries.com/downloads).
> Ensure that it's located in your application's probe directory or on the system's `PATH`, or provide a custom location yourself using one of the available method overloads.

## Usage

**YoutubeExplode.Converter** exposes its functionality by enhancing **YoutubeExplode**'s clients with additional extension methods.
To use them, simply add the corresponding namespace and follow the examples below.

### Downloading videos

You can download a video directly to a file through one of the extension methods provided on `VideoClient`.
For example, to download a video in the specified format using the highest quality streams, simply call `DownloadAsync(...)` with the video ID and the destination path:

```csharp
using YoutubeExplode;
using YoutubeExplode.Converter;

var youtube = new YoutubeClient();

var videoUrl = "https://youtube.com/watch?v=u_yIGGhubZs";
await youtube.Videos.DownloadAsync(videoUrl, "video.mp4");
```

Under the hood, this resolves the video's media streams, downloads the best candidates based on format, bitrate, framerate, and quality, and muxes them together into a single file.

> **Note**:
> If the specified output format is a known audio-only container (e.g. `mp3` or `ogg`) then only the audio stream is downloaded.

> **Warning**:
> Stream muxing is a resource-intensive process, especially when transcoding is involved.
> To avoid transcoding, consider specifying either `mp4` or `webm` for the output format, as these are the containers that YouTube uses for most of its streams. 

### Customizing the conversion process

To configure various aspects of the conversion process, use the following overload of `DownloadAsync(...)`:

```csharp
using YoutubeExplode;
using YoutubeExplode.Converter;

var youtube = new YoutubeClient();
var videoUrl = "https://youtube.com/watch?v=u_yIGGhubZs";

await youtube.Videos.DownloadAsync(videoUrl, "video.mp4", o => o
    .SetContainer("webm") // override format
    .SetPreset(ConversionPreset.UltraFast) // change preset
    .SetFFmpegPath("path/to/ffmpeg") // custom FFmpeg location
);
```

### Manually selecting streams

If you need precise control over which streams are used for the muxing process, you can also provide them yourself instead of relying on the automatic resolution:

```csharp
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Converter;

var youtube = new YoutubeClient();

// Get stream manifest
var videoUrl = "https://youtube.com/watch?v=u_yIGGhubZs";
var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);

// Select best audio stream (highest bitrate)
var audioStreamInfo = streamManifest
    .GetAudioStreams()
    .Where(s => s.Container == Container.Mp4)
    .GetWithHighestBitrate();

// Select best video stream (1080p60 in this example)
var videoStreamInfo = streamManifest
    .GetVideoStreams()
    .Where(s => s.Container == Container.Mp4)
    .First(s => s.VideoQuality.Label == "1080p60");

// Download and mux streams into a single file
var streamInfos = new IStreamInfo[] { audioStreamInfo, videoStreamInfo };
await youtube.Videos.DownloadAsync(streamInfos, new ConversionRequestBuilder("video.mp4").Build());
```

> **Warning**:
> Stream muxing is a resource-intensive process, especially when transcoding is involved.
> To avoid transcoding, consider prioritizing streams that are already encoded in the desired format (e.g. `mp4` or `webm`).