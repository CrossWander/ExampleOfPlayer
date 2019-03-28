using System;
using TagLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExampleOfPlayer.Services
{
    public class Audio
    {
        //здесь заполняются теги файла в переменные, так же путь к файлу - текущему и следующему и прошлому

        private int _bitRate;
        private int _freq;
        private string _album;
        private string _title;
        private string _artist;
        private string _year;
        private string _genre;
        private TimeSpan _duration;
        private string _lyrics;
        private IPicture _picture;
        private string _composer;
        private bool _isPlaying;


        /// <summary>
        /// Id
        /// </summary>
     //   [PrimaryKey]
   //     [Unique]
   //     [NotNull]
        public string Id { get; set; }

        /// <summary>
        /// File path or url
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// file playing
        /// </summary>
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (_isPlaying == value)
                    return;

                _isPlaying = value;
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value)
                    return;

                _title = value;
            }
        }

        /// <summary>
        /// Album
        /// </summary>
        public string Album
        {
            get { return _album; }
            set
            {
                if (_album == value)
                    return;

                _album = value;
            }
        }

        /// <summary>
        /// Artist
        /// </summary>
        public string Artist
        {
            get { return _artist; }
            set
            {
                if (_artist == value)
                    return;

                _artist = value;
            }
        }

        /// <summary>
        /// Duration
        /// </summary>
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                if (_duration == value)
                    return;

                _duration = value;
            }
        }

        /// <summary>
        /// Genre - жанр
        /// </summary>
        public string Genre
        {
            get { return _genre; }
            set
            {
                if (_genre == value)
                    return;

                _genre = value;
            }
        }

        /// <summary>
        /// Composer
        /// </summary>
        public string Composer
        {
            get { return _composer; }
            set
            {
                if (_composer == value)
                    return;

                _composer = value;
            }
        }

        /// <summary>
        /// Lyrics - слова песни
        /// </summary>
        public string Lyrics
        {
            get { return _lyrics; }
            set
            {
                if (_lyrics == value)
                    return;

                _lyrics = value;
            }
        }

        /// <summary>
        /// Picture - картинка альбома
        /// </summary>
        public IPicture Picture
        {
            get { return _picture; }
            set
            {
                if (_picture == value)
                    return;

                _picture = value;
            }
        }

        public void AudioTags (string filepath)
        {
            using (var audioFile = TagLib.File.Create(filepath))
            {
                //  Lyrics = audioFile.Tag.Lyrics;

                Title = string.IsNullOrEmpty(audioFile.Tag.Title) ? Path.GetFileNameWithoutExtension(filepath) : audioFile.Tag.Title.Trim();
                Album = string.IsNullOrEmpty(audioFile.Tag.Album) ? "Unknown Album" : audioFile.Tag.Album.Trim();
                //TrackNumber = (int)tag.Tag.Track;
                Artist = !audioFile.Tag.Performers.Any() ? "Unknown Artist" : string.Join(" & ", audioFile.Tag.Performers);
                //Year = (int)tag.Tag.Year;
                //    Genre = audioFile.Tag.FirstGenre;
                Genre = audioFile.Tag.JoinedGenres;
                //Bitrate = tag.Properties.AudioBitrate;
                Duration = audioFile.Properties.Duration;
                Composer = audioFile.Tag.FirstComposer;
                Source = filepath;

                if (audioFile.Tag.Pictures.Length >= 1)
                    {
                        Picture = audioFile.Tag.Pictures.FirstOrDefault();  //трек имейдж
                    }
                }  

       //         var audio = new Audio();
       //     audio.Id = this.Id;
       //     audio.Title = this.Title;
       //     audio.Artist = this.Artist;
      //      audio.Duration = this.Duration;
           // audio.IsPlaying = this.IsPlaying;

          //  return audio;
        }

        public Audio(string filepath)
        {
            using (var audioFile = TagLib.File.Create(filepath))
            {
                //Title = audioFile.Tag.Title;
                //Artist = audioFile.Tag.FirstPerformer;
                //Album = audioFile.Tag.Album;
                //Duration = audioFile.Properties.Duration;
                //Genre = audioFile.Tag.FirstGenre;
                //Composer = audioFile.Tag.FirstComposer;
                //Source = filepath;
                ////  Lyrics = audioFile.Tag.Lyrics;

                //if (audioFile.Tag.Pictures.Length >= 1)
                //{
                //    Picture = audioFile.Tag.Pictures.FirstOrDefault();  //трек имейдж
                //}

                Title = string.IsNullOrEmpty(audioFile.Tag.Title) ? Path.GetFileNameWithoutExtension(filepath) : audioFile.Tag.Title.Trim();
                Album = string.IsNullOrEmpty(audioFile.Tag.Album) ? "Unknown Album" : audioFile.Tag.Album.Trim();
                //TrackNumber = (int)tag.Tag.Track;
                Artist = !audioFile.Tag.Performers.Any() ? "Unknown Artist" : string.Join(" & ", audioFile.Tag.Performers);
                //Year = (int)tag.Tag.Year;
                //    Genre = audioFile.Tag.FirstGenre;
                Genre = audioFile.Tag.JoinedGenres;
                //Bitrate = tag.Properties.AudioBitrate;
                Duration = audioFile.Properties.Duration;
                Composer = audioFile.Tag.FirstComposer;
                Source = filepath;

                if (audioFile.Tag.Pictures.Length >= 1)
                {
                    Picture = audioFile.Tag.Pictures.FirstOrDefault();  //трек имейдж
                }
            }
        }

        public Audio()
        {
        }

        private static bool IsNullOrLikeEmpty(string value)
        {
            return value == null || value.Trim().Length == 0;
        }

        private static bool IsNullOrLikeEmpty(string[] value)
        {
            if (value == null)
                return true;

            foreach (string s in value)
                if (!IsNullOrLikeEmpty(s))
                    return false;

            return true;
        }

    }
}
