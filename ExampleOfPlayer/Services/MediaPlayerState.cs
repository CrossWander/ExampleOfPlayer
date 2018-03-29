namespace ExampleOfPlayer.Services
{
    public enum PlayerPlayState
    {
        Opening,
        Playing,
        Paused,
        Stopped,
        Buffering
    }

    public class MediaPlayerState
    {
        public PlayerPlayState NewState { get; set; }
    }
}
