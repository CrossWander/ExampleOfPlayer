using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExampleOfPlayer.Audio_lib.lib
{
    public class WinMediaPlayer : BasicPlayerFunction
    {
        bool Stops=false;
        private string _command;
        private string _source;
        private double _volume;
        private TimeSpan _duration;


        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, 
            StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume")]   //звук
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);



        /*   [DllImport("coredll.dll", SetLastError = true)]
           internal static extern int waveOutSetVolume(IntPtr device, int volume);

           [DllImport("coredll.dll", SetLastError = true)]
           internal static extern int waveOutGetVolume(IntPtr device, ref int volume);*/

        /*    internal static class NativeMethods
            {
                [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
                internal static extern int mciSendString([MarshalAs(UnmanagedType.LPWStr)] string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

                [DllImport("winmm.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
                internal static extern bool PlaySound(byte[] soundName, IntPtr hmod, int soundFlags);

                [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume")]   //звук
                public static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);
            }*/


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
                StringBuilder str = new StringBuilder(128);             
                mciSendString("status MediaFile mode", str, 128, IntPtr.Zero);
                if (str.Length == 7 && str.ToString().Substring(0, 7) == "stopped")
                    return TimeSpan.Zero;

                mciSendString("status MediaFile position", str, 128, IntPtr.Zero);
                var currentPos = TimeSpan.FromMilliseconds(Convert.ToInt32(str.ToString()));

                return TimeSpan.FromSeconds(Math.Min(Duration.TotalSeconds, currentPos.TotalSeconds));
            }
            set
            {
                string Pcommand;

                StringBuilder str = new StringBuilder(128);
                mciSendString("status MediaFile mode", str, 128, IntPtr.Zero);
                if (str.Length == 7 && str.ToString().Substring(0, 7) == "playing")
                {
                    Pcommand = "play MediaFile from " + value;
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
                else
                {
                    Pcommand = "seek MediaFile to " + value;
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                    mciSendString("play MediaFile", null, 0, IntPtr.Zero);
                }
            }
        }

        public override TimeSpan Duration
        {
            get
            {
                StringBuilder str = new StringBuilder(128);
                //if ((Err = mciSendString(Pcommand, str, 128, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                mciSendString("status MediaFile length", str, 128, IntPtr.Zero);
                return _duration = TimeSpan.FromMilliseconds(Convert.ToInt32(str.ToString()));
            }
        }
/// <summary>
/// /
/// </summary>
        public override double Volume
        {
            get
            {
              /*  // By the default set the volume to 0
                uint CurrentVolume = 0;
                // At this point, CurrVol gets assigned the volume
                waveOutGetVolume(IntPtr.Zero, out CurrentVolume);
                // Calculate the volume
                ushort CalcVol = (ushort)(CurrentVolume & 0x0000ffff);
                // Get the volume on a scale of 1 to 100 (to fit the trackbar)
                _volume = CalcVol / (ushort.MaxValue / 100);*/
                

                return _volume;
            }

            set
            {
                if (_volume == value)
                    return;

                /*  _volume = value;
                  uint v = ((uint)(_volume*100)) & 0xffff;
                  uint vAll = v | (v << 16);
                  WaveOutSetVolume(IntPtr.Zero, vAll);*/

                _volume = value*10;

                mciSendString(string.Concat("setaudio MediaFile volume to ", _volume), null, 0, IntPtr.Zero);
               // masterVolumn = value;

                /*    // Calculate the volume that's being set
                    int NewVolume = ((ushort.MaxValue / 100) * (int)_volume);
                    // Set the same volume for both the left and the right channels
                    uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
                    // Set the volume
                    waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);*/
                //  return res;*/
            }
        }

        public override void Initialize()
        {
            const string FORMAT = @"open ""{0}"" type mpegvideo alias MediaFile";
            _command = String.Format(FORMAT,_source);
            mciSendString(_command, null, 0, IntPtr.Zero);
        }

        public override void Play()
        {
            if (Stops == false)
            {
                _command = "play MediaFile";
                mciSendString(_command, null, 0, IntPtr.Zero);
            }
            else { Stops = false; mciSendString(string.Format("play MediaFile from 0"), null, 0, IntPtr.Zero); }
        }
        public override void Pause()
        {
            _command = "pause MediaFile";
            mciSendString(_command, null, 0, IntPtr.Zero);
        }
        public override void Stop()
        {
            _command = "stop MediaFile";
            mciSendString(_command, null, 0, IntPtr.Zero);
            Stops = true;            
        }

        public override void Dispose()
        {
            _command = "close MediaFile";
            mciSendString(_command, null, 0, IntPtr.Zero);
        }

    }
}
