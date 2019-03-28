using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using ExampleOfPlayer.Audio_lib.lib;
using ExampleOfPlayer.Services;
using ExampleOfPlayer.Properties;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace ExampleOfPlayer.Audio_lib
{
    public static class AudioPlayerFunction
    {
        private static List<string> _audio = new List<string>();
        private static BasicPlayerFunction _basicplayer;
        private static readonly System.Timers.Timer _positionTimer;
        private static Audio _currentAudio;
        public static List<Audio> _playlist = new List<Audio>();
        private static int _playFailsCount;
        private static PlayerPlayState _state;
        private static CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private static FadeOutFunction _fade = new FadeOutFunction();

        /// <summary>
        /// выбор библиотеки аудио, которая будет использоваться
        /// </summary>
        private static BasicPlayerFunction MediaPlayer
        {
            get
            {
                if (_basicplayer == null)
                {
                    switch (Settings.Default.AudioLibrary)
                    {
                        case (int)MediaEngine.Wmp:
                            _basicplayer = new WinMediaPlayer();
                            break;
                        case (int)MediaEngine.NAudio:
                            _basicplayer = new NAudioMediaPlayer();
                            break;
                        case (int)MediaEngine.CSCore:
                            _basicplayer = new CSCoreMediaPlayer();
                            break;
                        case (int)MediaEngine.Bass:
                            _basicplayer = new BassMediaPlayer();
                            break;
                    }
                    /*   _basicplayer = Properties.Settings.Default.AudioLibrary == (int)MediaEngine.Wmp
                        ? (BasicPlayerFunction)new WinMediaPlayer()
                        : Properties.Settings.Default.AudioLibrary == (int)MediaEngine.NAudio 
                        ? new NAudioMediaPlayer()
                        : Properties.Settings.Default.AudioLibrary == (int)MediaEngine.CSCore 
                        ? new CSCoreMediaPlayer(): new BassMediaPlayer(); */

                    _basicplayer.Initialize();
                    _basicplayer.MediaEnded += MediaPlayerOnMediaEnded;
                    _basicplayer.MediaFailed += MediaPlayerOnMediaFailed;
                    _basicplayer.MediaOpened += MediaPlayerOnMediaOpened;
                    _basicplayer.Volume = Volume;
                }
                return _basicplayer;
            }
        }

        private static PlayerPlayState State
        {
            get { return _state; }
            set
            {
                if (_state == value)
                    return;

                _state = value;

                if (_state == PlayerPlayState.Playing)
                    _positionTimer.Start();
                else
                    _positionTimer.Stop();

              new MediaPlayerState() { NewState = value };
            }
        }

        public static bool IsPlaying
        {
            get
            {
                return State == PlayerPlayState.Playing;
            }
        }

        public static List<Audio> Playlist
        {
            get { return _playlist; }
            set { _playlist = value; }
        }

        public static Audio CurrentAudio
        {
            get
            {
                return _currentAudio;
            }
            set
            {
                var old = _currentAudio;
                _currentAudio = value;
            }
        }

        public static List<string> AudioPath
        {
            get { return _audio; }
            set
            {
                _audio = value;
                SetPlaylist(_audio);
            }
        }

        /// <summary>
        /// добавление песен в плейлист
        /// </summary>
        /// <param name="_audio"></param>
        public static void SetPlaylist(List<string> _audio)
        {
            try
            {
                foreach (var s in _audio)
                {
                   // Playlist.Add(new Audio(s));
                    _playlist.Add(new Audio(s));
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        public static bool Repeat
        {
            get { return Settings.Default.Repeat; }
            set
            {
                Settings.Default.Repeat = value;
            }
        }

        public static TimeSpan CurrentAudioPosition
        {
            get
            {
                if (MediaPlayer == null)
                    return TimeSpan.Zero;

                return MediaPlayer.Position;
            }
            set
            {
                if (MediaPlayer == null)
                    return;

                if (MediaPlayer.Position.TotalSeconds == value.TotalSeconds)
                    return;

                MediaPlayer.Position = value;
            }
        }

        public static TimeSpan CurrentAudioDuration
        {
            get
            {
                if (MediaPlayer != null)
                    return MediaPlayer.Duration;

                return TimeSpan.Zero;
            }
        }

        public static float Volume
        {
            get { return Settings.Default.Volume; }
            set
            {
                if (Settings.Default.Volume == value)
                    return;

                Settings.Default.Volume = value.Clamp(0f, 100f); //проверяет входит ли число в указ диапазон, если нет ставит макс или мин
                MediaPlayer.Volume = Settings.Default.Volume;
            }
        }


        static AudioPlayerFunction()
        {
            _positionTimer = new System.Timers.Timer();
            _positionTimer.Interval = 500;
            _positionTimer.AutoReset = true;
            _positionTimer.Enabled = true;
            _positionTimer.Elapsed += PositionTimerTick;

            if (IsPlaying)
               _positionTimer.Start();
        }

        /// <summary>
        /// проверка на окончание трека
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void PositionTimerTick(object sender, object e) 
        {
            try
            {
                if (MediaPlayer.Position > MediaPlayer.Duration)
                    MediaPlayerOnMediaEnded(MediaPlayer, EventArgs.Empty);
            }
            catch (Exception ex) { }
        }

        public static void Play(Audio track)
        {
            CancelAsync();
            PlayInternal(track, _cancellationToken.Token);
        }

        public static async void PlayInternal(Audio track, CancellationToken token)
        {
            //if (track.IsPlaying) return;
            if (State == PlayerPlayState.Paused)
            {
                CurrentAudio.IsPlaying = true;
                State = PlayerPlayState.Playing;
                MediaPlayer.Play();
                return;
            }
            if (CurrentAudio != null)
            {
                CurrentAudio.IsPlaying = false;
                Stop();
            }
            track.IsPlaying = true;

            CurrentAudio = track;

            if (track.Source == null)
            {
        
            }

            if (token.IsCancellationRequested)
                return;

            //track.IsPlaying = true;
            

            MediaPlayer.Source = track.Source;
            MediaPlayer.Initialize(); 
            MediaPlayer.Play();

            State = PlayerPlayState.Playing;

        }

        public static async void Play()
        {
            if (MediaPlayer.Source == null && CurrentAudio != null)
            {
                Play(CurrentAudio);
            }

            MediaPlayer.Play();
            State = PlayerPlayState.Playing;
        }

        /// <summary>
        /// следующая песня по нажатию кнопки
        /// </summary>
        public static void SwitchNext()
        {
            NextSong(true);         
        }

        /// <summary>
        /// следующая песня в автоматическом режиме, после окончания предыдущей
        /// </summary>
        /// <param name="invokedByUser"></param>
        private static void NextSong(bool invokedByUser = false)
        {
            if (Repeat && !invokedByUser)
            {
                Play(CurrentAudio);
                return;
            }

            if (_playlist != null && _playlist.Count > 0)
            {
                int currentIndex = -1;
                if (_currentAudio != null)
                {
                    currentIndex = _playlist.IndexOf(_currentAudio);
                    if (currentIndex == -1)
                    {
                        var current = _playlist.FirstOrDefault(a => a.Source == _currentAudio.Source);
                        if (current != null)
                            currentIndex = _playlist.IndexOf(current);
                    }
                }

                currentIndex++;

                if (currentIndex >= _playlist.Count)
                {
                    currentIndex = 0;
                }

                Play(_playlist[currentIndex]);
            }
        }

        /// <summary>
        /// предыдущая песня по нажатию кнопки
        /// </summary>
        public static void Previous()
        {
            if (CurrentAudioPosition.TotalSeconds > 3)
            {
                CurrentAudioPosition = TimeSpan.Zero;
                MediaPlayer.Play();
                return;
            }
            if (_playlist != null)
            {
                int currentIndex = -1;
                if (_currentAudio != null)
                {
                    currentIndex = _playlist.IndexOf(_currentAudio);
                    if (currentIndex == -1)
                    {
                        var current = _playlist.FirstOrDefault(a => a.Source == _currentAudio.Source);
                        if (current != null)
                            currentIndex = _playlist.IndexOf(current);
                    }
                }
                if (currentIndex > 0)
                {
                    currentIndex--;

                    if (currentIndex >= 0)
                        Play(_playlist[currentIndex]);
                }
                else
                {
                    currentIndex = _playlist.Count -1;

                    if (currentIndex < _playlist.Count)
                        Play(_playlist[currentIndex]);
                }
            }
            

        }

        public static void Pause()
        {
            MediaPlayer.Pause();
            State = PlayerPlayState.Paused;
        }

        public static async void Stop()
        {
            //await _fade.FadeOut(760, _basicplayer);
            MediaPlayer.Stop();
            CurrentAudioPosition = TimeSpan.Zero;                 
            State = PlayerPlayState.Stopped;
        }


        /// <summary>
        /// ззагрузка плейлиста
        /// </summary>
        /// <returns></returns>
        public static Task Load()
        {
            return Task.Run(() =>
            {
                try
                {
                    if (!File.Exists("currentPlaylist.js"))
                        return;

                    var json = File.ReadAllText("currentPlaylist.js");
                    if (string.IsNullOrEmpty(json))
                        return;

                    var o = JObject.Parse(json);
                    if (o["currentAudio"] != null)
                    {
                      //  var audio = JsonConvert.DeserializeObject<Audio>(o["currentAudio"].ToString(), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
                      //  CurrentAudio = audio;
                        // Application.Current.Dispatcher.Invoke(() => CurrentAudio = audio);
                    }

                    if (o["currentPlaylist"] != null)
                    {
                        var playlist = JsonConvert.DeserializeObject<List<object>>(o["currentPlaylist"].ToString(), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
                        if (playlist != null)
                            SetCurrentPlaylist(playlist.OfType<Audio>());
                           // Application.Current.Dispatcher.Invoke(() => SetCurrentPlaylist(playlist.OfType<Audio>()));
                    }
                }
                catch (Exception ex) { }
            });
        }

        public static void SetCurrentPlaylist(IEnumerable<Audio> playlist)
        {
            if (playlist == null)
            {
                Playlist.Clear();
            }
            else
                Playlist = new List<Audio>(playlist);
        //    foreach (var s in _playlist)
             //   PlayList.Items.Add(s.Artist + " - " + s.Title);
       //           MessageBox.Show(s.Source);
        }

        /// <summary>
        /// сохранить плейлист
        /// </summary>
        public static void Save()
        {
            try
            {
                var o = new
                {
                    currentAudio = CurrentAudio,
                    currentPlaylist = Playlist,
                };

                var json = JsonConvert.SerializeObject(o, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
                File.WriteAllText("currentPlaylist.js", json);
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// удалить плейлист
        /// </summary>
        public static void Clear()
        {
            try
            {
                if (File.Exists("currentPlaylist.js"))
                    File.Delete("currentPlaylist.js");
            }
            catch (Exception ex) { }
        }

        private static void MediaPlayerOnMediaOpened(object sender, EventArgs e)
        {
            _playFailsCount = 0;
            State = PlayerPlayState.Playing;
        }

        private static void MediaPlayerOnMediaFailed(object sender, Exception e)
        {
            throw new NotImplementedException();
        }

        private static void MediaPlayerOnMediaEnded(object sender, EventArgs e)
        {
            NextSong();
        }

        private static void CancelAsync()
        {
            _cancellationToken.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static void Dispose()
        {
            _positionTimer.Enabled = false;
            _positionTimer.Stop();
            _positionTimer.Dispose();
            MediaPlayer.Dispose();
        }
    }
}
