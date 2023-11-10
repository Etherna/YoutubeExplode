using YoutubeExplode.Channels;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;

namespace YoutubeExplode
{
    /// <summary>
    /// Interface to client for interacting with YouTube.
    /// </summary>
    public interface IYoutubeClient
    {
        /// <summary>
        /// Operations related to YouTube channels.
        /// </summary>
        public ChannelClient Channels { get; }

        /// <summary>
        /// Operations related to YouTube playlists.
        /// </summary>
        public PlaylistClient Playlists { get; }

        /// <summary>
        /// Operations related to YouTube search.
        /// </summary>
        public SearchClient Search { get; }

        /// <summary>
        /// Operations related to YouTube videos.
        /// </summary>
        public VideoClient Videos { get; }
    }
}
