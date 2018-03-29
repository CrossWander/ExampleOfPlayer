using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;

namespace ExampleOfPlayer.Audio_lib.lib
{
    class CSCoreMediaPlayer : BasicPlayerFunction
    {
        private ISoundOut _soundOut;
        private IWaveSource _waveSource;
        private string _source;
        private double _volume;
        public event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped;


        public PlaybackState PlaybackState
        {
            get
            {
                if (_soundOut != null)
                    return _soundOut.PlaybackState;
                return PlaybackState.Stopped;
            }
        }

        public override string Source
        {
            get { return _source; }
            set { _source = value; }
        }
/// <summary>
/// /////
/// </summary>
        public override TimeSpan Position
        {
            get
            {
                if (_waveSource != null)
                    return _waveSource.GetPosition();
                return TimeSpan.Zero;
            }
            set
            {
                if (_waveSource != null)
                    _waveSource.SetPosition(value);
            }
        }
        public override TimeSpan Duration
        {
            get
            {
                if (_waveSource != null)
                    return _waveSource.GetLength();
                return TimeSpan.Zero;
            }
        }
/// <summary>
/// ////////
/// </summary>
        public override double Volume
        {
            get
            {
               if (_soundOut != null)
                    return Math.Min(100, Math.Max((int)(_soundOut.Volume * 100), 0));
                return 100;
               // return _volume;
            }

            set
            {
                if (_volume == value)
                    return;

                _volume = value;

                if (_soundOut != null)
                {
                    _soundOut.Volume = (float)Math.Min(1.0f, Math.Max(_volume / 100f, 0f));
                }
            }
        }

        public override void Initialize()
        {
            CleanupPlayback();

            _waveSource = CodecFactory.Instance.GetCodec(_source)
                    .ToSampleSource()
                    .ToMono()
                    .ToWaveSource();
            _soundOut = new WasapiOut() { Latency = 100 };
            _soundOut.Initialize(_waveSource);

            if (PlaybackStopped != null) _soundOut.Stopped += PlaybackStopped;
        }

        public override void Play()
        {
            if (_soundOut != null)
                _soundOut.Play();
        }

        public override void Pause()
        {
            if (_soundOut != null)
                _soundOut.Pause();
        }

        public override void Stop()
        {
            if (_soundOut != null)
            {
                _soundOut.Stop();
                _waveSource.Position = 0;
            }
        }

        private void CleanupPlayback()
        {
            if (_soundOut != null)
            {
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_waveSource != null)
            {
                _waveSource.Dispose();
                _waveSource = null;
            }
        }

        public override void Dispose()
        {
            CleanupPlayback();
        }

    }
}
