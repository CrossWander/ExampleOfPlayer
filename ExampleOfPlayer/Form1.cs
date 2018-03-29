using ExampleOfPlayer.Audio_lib;
using ExampleOfPlayer.Audio_lib.lib;
using ExampleOfPlayer.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExampleOfPlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (_basicplayer == null)
            {
                switch (Properties.Settings.Default.AudioLibrary)
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
            }

            AudioPlayerFunction.Load();
        //    foreach (var _audio in AudioPlayerFunction.Playlist)
        //        MessageBox.Show(_audio.Artist + " - " + _audio.Title); //пусто
              //  PlayList.Items.Add(_audio.Artist + " - " + _audio.Title);

        }

        private static BasicPlayerFunction _basicplayer;
        private static FadeOutFunction _fade = new FadeOutFunction();
        //private static AudioPlayerFunction _AudioService = new AudioPlayerFunction();
        public List<string> filepath = new List<string>();
        private Audio _audio = new Audio();

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog { Multiselect = true, Title = "Выберите музыку" };
            open.Filter = "Audio files (*.wav; *.mp3)| *.wav; *.mp3;";
            if (open.ShowDialog() != DialogResult.OK) return;

            if (open.FileName == String.Empty)
                return;
            foreach (string fname in open.FileNames)
            {
                // PlayList.Items.Add(System.IO.Path.GetFileNameWithoutExtension(fname));
                filepath.Add(fname);
                _audio.AudioTags(fname);
                PlayList.Items.Add(_audio.Artist + " - " + _audio.Title);
                //  _AudioService.AudioPath(fname);
            }

            AudioPlayerFunction.AudioPath = filepath;

            /*      if (PlayList.Items.Count<0)    //эту проверку нужно будет вызывать...реализовать в классе, чтобы работало
                      //воспроизведение сразу после переноса песен в лейлист мышкой, а не про browse
                      PlayList.SetSelected(0, true);*/
        }

        private async void Playbutton_Click(object sender, EventArgs e)
        {
            int i;
            // сделать проверки - если список не пуст, то если не выбран элемент(селектиндекс равен -1) то проиграть 1й
            // элемент в списке, иначе проиграть выбранный, если играет песня - ничего не делать
            // AudioPlayerFunction - должен принимать индекс выбранной песни в списке
            if (PlayList.Items.Count != 0)
            {
                if (PlayList.SelectedIndex != -1)
                {
                    i = PlayList.SelectedIndex;
                    _basicplayer.Source = filepath[i];
                    AudioPlayerFunction.CurrentAudio = AudioPlayerFunction._playlist[PlayList.SelectedIndex];
                }
                else
                {                    
                    i = PlayList.SelectedIndex = 0;
                    AudioPlayerFunction.CurrentAudio = AudioPlayerFunction._playlist[PlayList.SelectedIndex];
                    _basicplayer.Source = filepath[i];
                }

                _basicplayer.Initialize();
                _basicplayer.Volume = 100;
                _basicplayer.Play();

                try
                {
                    _audio.AudioTags(filepath[i]);
                    Song.Text = "Song: " + _audio.Title;
                    Album.Text = "Album: " + _audio.Album;
                    Genre.Text = "Genre: " + _audio.Genre;
                    Composer.Text = "Composer: " + _audio.Composer;
                    // richTextBox1.Text = _audio.Lyrics;
                    if (Image.FromStream(new MemoryStream((byte[])_audio.Picture.Data.Data)).GetThumbnailImage(100, 100, null, IntPtr.Zero) == null)
                        return;
                    else
                        AudioCover.Image = Image.FromStream(new MemoryStream((byte[])_audio.Picture.Data.Data)).GetThumbnailImage(100, 100, null, IntPtr.Zero);
                }
                catch (Exception ex) { }//                MessageBox.Show(ex.ToString()); }


                // await _fade.FadeIn(100, _basicplayer);
                // await _fade.FadeIn2(2000, _basicplayer);
                label1.Text = _basicplayer.Position.Hours.ToString("00") + ":" +
                    _basicplayer.Position.Minutes.ToString("00") + ":" +
                    _basicplayer.Position.Seconds.ToString("00");
                label2.Text = _basicplayer.Duration.Hours.ToString("00") + ":" +
                    _basicplayer.Duration.Minutes.ToString("00") + ":" +
                    _basicplayer.Duration.Seconds.ToString("00");
                TimePosition.Maximum = (int)_basicplayer.Duration.TotalSeconds;
                TimePosition.Value = (int)_basicplayer.Position.TotalSeconds;
                Tickstimer.Enabled = true;
            }
            else return;
        }

        private void Pausebutton_Click(object sender, EventArgs e)
        {
            if (PlayList.Items.Count != 0)               
                _basicplayer.Pause();
        }


        private async void Stopbutton_Click(object sender, EventArgs e)
        {
            if (PlayList.Items.Count != 0)
            {
                await _fade.FadeOut(760, _basicplayer);
                //  await _fade.FadeOut2(50, _basicplayer);
                //  _basicplayer.Stop();
                label1.Text = "00:00:00";
                label2.Text = "00:00:00";
                TimePosition.Value = 0;
                Tickstimer.Enabled = false;
            }
        }
        private void Previous_Click(object sender, EventArgs e)
        {
            if (PlayList.Items.Count != 0)
            { }
        }

        private void Next_Click(object sender, EventArgs e)
        {
            if (PlayList.Items.Count != 0)
            { }
        }

        private void PlayList_DoubleClick(object sender, EventArgs e)
        {
            if (PlayList.SelectedIndex != -1)
            {
                _basicplayer.Dispose();
                int i = PlayList.SelectedIndex;
                _basicplayer.Source = filepath[i];
                _basicplayer.Initialize();
                _basicplayer.Volume = 100;                
                _basicplayer.Play();

                try
                {
                    _audio.AudioTags(filepath[i]);
                    Song.Text = "Song: " + _audio.Title;
                    Album.Text = "Album: " + _audio.Album;
                    Genre.Text = "Genre: " + _audio.Genre;
                    Composer.Text = "Composer: " + _audio.Composer;
                    // richTextBox1.Text = _audio.Lyrics;
                    if (Image.FromStream(new MemoryStream((byte[])_audio.Picture.Data.Data)).GetThumbnailImage(100, 100, null, IntPtr.Zero) == null)
                        AudioCover.Image = null;
                    //return;
                    else
                        AudioCover.Image = Image.FromStream(new MemoryStream((byte[])_audio.Picture.Data.Data)).GetThumbnailImage(100, 100, null, IntPtr.Zero);
                }
                catch (Exception ex) { /* MessageBox.Show(ex.ToString());*/ }


                //костылек - потом убрать
                label1.Text = _basicplayer.Position.Hours.ToString("00") + ":" +
                    _basicplayer.Position.Minutes.ToString("00") + ":" +
                    _basicplayer.Position.Seconds.ToString("00");
                label2.Text = _basicplayer.Duration.Hours.ToString("00") + ":" +
                    _basicplayer.Duration.Minutes.ToString("00") + ":" +
                    _basicplayer.Duration.Seconds.ToString("00");
                TimePosition.Maximum = (int)_basicplayer.Duration.TotalSeconds;
                TimePosition.Value = (int)_basicplayer.Position.TotalSeconds;
                Tickstimer.Enabled = true;
            }
            else return;
        }

        private void PlayList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ?
                DragDropEffects.All : DragDropEffects.None;
        }

        private void PlayList_DragDrop(object sender, DragEventArgs e)
        {
            var dropped = ((string[])e.Data.GetData(DataFormats.FileDrop));
            var files = dropped.ToList();

            if (!files.Any())
                return;

            foreach (string drop in dropped)
                if (Directory.Exists(drop))
                {
                    files.AddRange(Directory.GetFiles(drop, "*.mp3", SearchOption.AllDirectories));
                    files.AddRange(Directory.GetFiles(drop, "*.wav", SearchOption.AllDirectories));
                }

            foreach (string file in files)
            {
                if ((file.ToLower().EndsWith(".mp3")) || (file.ToLower().EndsWith(".wav")))
                {
                    PlayList.Items.Add(Path.GetFileNameWithoutExtension(file));
                    filepath.Add(file);
                }
            }

        }

        private void Settings_Click(object sender, EventArgs e)
        {
            var f = new SettingsForm();
            f.ShowDialog(this);
        }

        private void RepeatBut_Click(object sender, EventArgs e)
        {
            switch (RepeatBut.CheckState)
            {
                case CheckState.Checked:
                    AudioPlayerFunction.Repeat = true;
                    break;
                case CheckState.Unchecked:
                    AudioPlayerFunction.Repeat = false;
                    break;
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           // _AudioService.Repeat = false;
           // _basicplayer.Dispose();
        }

        private void Tickstimer_Tick(object sender, EventArgs e)
        {
            label1.Text = _basicplayer.Position.Hours.ToString("00") + ":" +
                _basicplayer.Position.Minutes.ToString("00") + ":" +
                _basicplayer.Position.Seconds.ToString("00");

            TimePosition.Value = (int)_basicplayer.Position.TotalSeconds;
        }

        private void VolumeValue_Scroll(object sender, EventArgs e)
        {
            _basicplayer.Volume = VolumeValue.Value;
            label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold);
            label3.ForeColor = Color.Black;
            if (_basicplayer.Volume == 0)
            {
                label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Strikeout | FontStyle.Bold);
                label3.ForeColor = Color.Red;
            }
        }

        private void TimePosition_Scroll(object sender, EventArgs e)
        {
            label1.Text = TimeSpan.FromSeconds(TimePosition.Value).Hours.ToString("00") + ":" +
                TimeSpan.FromSeconds(TimePosition.Value).Minutes.ToString("00") + ":" +
                TimeSpan.FromSeconds(TimePosition.Value).Seconds.ToString("00");
        }

        private void TimePosition_MouseDown(object sender, MouseEventArgs e)
        {
            Tickstimer.Enabled = false;
        }

        private void TimePosition_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                       /*MaxVal - MinVal = TotalVal  //максимум timeposition.maximum - timeposition.minimum
                         SlotEndPix - SlotStartPix = TotalPix   //ширина в пикселях трекбара
                         MouseLeftPix - SlotStartPix = MouseSlotPix   // e.X - timeposition.min
                         Fraction = MouseSlotPix / TotalPix  // e.X / timeposition.width
                         MouseNum = Fraction * TotalVal  // 1*timeposition.max                
                          */

                int position = Convert.ToInt32(((double)e.X / (double)TimePosition.Width) * (TimePosition.Maximum - TimePosition.Minimum));
                if (position > TimePosition.Maximum - 1) position = TimePosition.Maximum;
                if (position < 0) position = TimePosition.Minimum;
                TimePosition.Value = position;
                _basicplayer.Position = TimeSpan.FromSeconds(TimePosition.Value);
                label1.Text = _basicplayer.Position.Hours.ToString("00") + ":" +
                    _basicplayer.Position.Minutes.ToString("00") + ":" +
                    _basicplayer.Position.Seconds.ToString("00");
                Tickstimer.Enabled = true;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            AudioPlayerFunction.Save();
        }
    }
}
