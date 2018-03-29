using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOfPlayer.Audio_lib.lib
{
    public class NAudioMediaPlayer : BasicPlayerFunction
    {
        private IWavePlayer _wavePlayer;
        private WaveStream _outputStream;
        private WaveChannel32 _volumeStream;
        private double _volume;
        private TimeSpan _duration;
        private string _source;



        public override string Source
        {
            get { return _source; }
            set { _source = value; }
        }
/// <summary>
/// /
/// </summary>
        public override TimeSpan Position
        {
            get
            {
                if (_outputStream == null)
                    return TimeSpan.Zero;

                var currentPos = _outputStream.CurrentTime;
                //if (Math.Round(currentPos.TotalSeconds, 1) >= Math.Round(_duration.TotalSeconds, 1))
                //    SwitchNext();

                return TimeSpan.FromSeconds(Math.Min(Duration.TotalSeconds, currentPos.TotalSeconds));
            }
            set
            {
                if (_outputStream != null)
                    _outputStream.CurrentTime = value;
            }
        }

        public override TimeSpan Duration
        {
            get { return _duration = _outputStream.TotalTime; }
        }
/// <summary>
/// /
/// </summary>
        public override double Volume
        {
            get
            {
                return _volume = _volumeStream.Volume;
            }
            set
            {
                if (_volume == value)
                    return;

                _volume = value;

                if (_volumeStream != null)
                    _volumeStream.Volume = (float)value/100F;
            }
        }

        public override void Initialize()
        {
            _wavePlayer = new WaveOutEvent();
        }

        public override void Pause()
        {
             _wavePlayer.Pause();
        }

        public override void Play()
        {
            if (_wavePlayer.PlaybackState == NAudio.Wave.PlaybackState.Paused) _wavePlayer.Play();
            if (_wavePlayer.PlaybackState != NAudio.Wave.PlaybackState.Playing)
            {
                _outputStream = new MediaFoundationReader(_source);
                _volumeStream = new WaveChannel32(_outputStream, (float)_volume, 0);
                _volumeStream.PadWithZeroes = false;
                _wavePlayer.Init(_volumeStream);
                _wavePlayer.PlaybackStopped += _wavePlayer_PlaybackStopped;
                _wavePlayer.Play();
            }
        }

        void _wavePlayer_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (_outputStream != null && _outputStream.CurrentTime.TotalSeconds > Duration.TotalSeconds / 2)
                SwitchNext();
        }

        public override void Stop()
        {
            _wavePlayer.Stop();
            _outputStream.CurrentTime = TimeSpan.Zero;
        }

        private void CleanupPlayback()
        {
            if (_wavePlayer != null)
            {
                _wavePlayer.Dispose();
                _wavePlayer = null;
            }
            if (_volumeStream != null)
            {
                _volumeStream.Dispose();
                _volumeStream = null;
            }
            if (_outputStream != null)
            {
                _outputStream.Dispose();
                _outputStream = null;
            }
        }

        public override void Dispose()
        {
            CleanupPlayback();
        }

        private void SwitchNext()
        {
            if (MediaEnded != null)
                MediaEnded(this, EventArgs.Empty);
        }

    }
}
