using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;

namespace ExampleOfPlayer.Audio_lib.lib
{
    public class BassMediaPlayer : BasicPlayerFunction
    {
        private static bool InitDefaultDevice;
        private double _volume;
        private string _source;
        public static int _stream;
        float vol;

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
                if (_stream == 0)
                    return TimeSpan.Zero;

                long pos = Bass.BASS_ChannelGetPosition(_stream);
                double position = Bass.BASS_ChannelBytes2Seconds(_stream, pos);
                return TimeSpan.FromSeconds(position);
            }
            set
            {
                if (_stream != 0)
                    Bass.BASS_ChannelSetPosition(_stream, value.TotalSeconds);
            }
        }

        public override TimeSpan Duration
        {
            get
            {
                if (_stream != 0)
                {
                    long TimeBytes = Bass.BASS_ChannelGetLength(_stream);
                    double duration = Bass.BASS_ChannelBytes2Seconds(_stream, TimeBytes);
                    return TimeSpan.FromSeconds(duration);
                }
                return TimeSpan.Zero;
            }
        }
        /// <summary>
        /// /
        /// </summary>


        /// <summary>
        /// не выставляется звук - проверить
        /// </summary>
        public override double Volume
        {
            get
            {
                return _volume;
            }

            set
            {
                if (_volume == value)
                    return;

                _volume = value;
                Bass.BASS_ChannelGetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, ref vol);
                if (_stream != 0)                
                    if (!Bass.BASS_ChannelSetAttribute(_stream,BASSAttribute.BASS_ATTRIB_VOL, (float)_volume/100F))
                        MessageBox.Show("Error= " + Bass.BASS_ErrorGetCode().ToString());
            }
        }

        public override void Initialize()
        {
            if (!InitDefaultDevice)
                InitDefaultDevice = Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            _stream = Bass.BASS_StreamCreateFile(_source, 0, 0, BASSFlag.BASS_DEFAULT);
        }

        public override void Pause()
        {
            if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
                Bass.BASS_ChannelPause(_stream);
        }

        public override void Play()
        {
            if ((Bass.BASS_ChannelIsActive(_stream) != BASSActive.BASS_ACTIVE_PAUSED) || (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_STOPPED))
            {
                Bass.BASS_StreamFree(_stream);

                _stream = Bass.BASS_StreamCreateFile(_source, 0, 0, BASSFlag.BASS_DEFAULT);
                //_stream = Bass.BASS_StreamCreateFile(_source, 0, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
                if (_stream != 0)
                {
                    //костыли
                    Bass.BASS_ChannelGetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, ref vol);
                    //костыли
                    Volume = vol * 100F;
                    //костыли
                    Bass.BASS_ChannelSetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, (float)_volume);
                    Bass.BASS_ChannelPlay(_stream, false); //2й параметр это луп (рестарт)                
                 //   MessageBox.Show(vol.ToString());
                }
                else MessageBox.Show("Error= " + Bass.BASS_ErrorGetCode().ToString());
            }
            else Bass.BASS_ChannelPlay(_stream, false);

     //       MessageBox.Show(Duration.ToString());
//
           // Position = TimeSpan.Parse("00:01:00");// 40000;
        }

        public override void Stop()
        {
            Bass.BASS_ChannelStop(_stream);
            Bass.BASS_StreamFree(_stream);
        }


        public override void Dispose()
        {
            Bass.BASS_Stop();
      /*      for (int i = 0; i < BassPluginsHandles.Count; i++)
                Bass.BASS_PluginFree(BassPluginsHandles[i]);*/
            Bass.BASS_Free();
            _stream = 0;
            InitDefaultDevice = false;
        }
    }
}
