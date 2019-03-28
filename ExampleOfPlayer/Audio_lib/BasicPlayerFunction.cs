using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOfPlayer.Audio_lib
{
    public enum MediaEngine
    {
        /// Windows Media Player engine
        Wmp,
        /// NAudio engine
        NAudio,
        ///CSCore engine
        CSCore,
        /// Bass engine
        Bass
    }

    public abstract class BasicPlayerFunction : IDisposable
    {
        //fields
        public abstract string Source { get; set; }
        public abstract double Volume { get; set; }
        public abstract TimeSpan Position { get; set; }
        public abstract TimeSpan Duration { get; }

        //events
        public EventHandler MediaOpened;
        public EventHandler MediaEnded;
        public EventHandler<Exception> MediaFailed;

      //  public abstract Task<bool> FadeOut(int sec, bool fading);
        public abstract void Initialize();
        public abstract void Play();
        public abstract void Pause();
        public abstract void Stop();
        public abstract void Dispose();

    }
}
