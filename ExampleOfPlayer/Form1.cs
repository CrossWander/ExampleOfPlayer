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
       //&     AudioPlayerFunction.Load();

        }

        private static BasicPlayerFunction _basicplayer;
        private static FadeOutFunction _fade = new FadeOutFunction();
        public List<string> filepath = new List<string>();
        private Audio _audio = new Audio();

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog { Multiselect = true, Title = "Выберите музыку" };
            open.Filter = "Audio files | *.wav; *.mp3; *.flac; *.ac3; *.aac; *.wma";    //*.opus; *.m4a;";
            if (open.ShowDialog() != DialogResult.OK) return;

            if (open.FileName == String.Empty)
                return;
            foreach (string fname in open.FileNames)
            {
                filepath.Add(fname);
                _audio.AudioTags(fname);
                PlayList.Items.Add(_audio.Artist + " - " + _audio.Title);
            }

            AudioPlayerFunction.AudioPath = filepath;
        }

        private async void Playbutton_Click(object sender, EventArgs e)
        {
            if (PlayList.Items.Count != 0)
            {
                if (PlayList.SelectedIndex != -1)
                {
                    AudioPlayerFunction.CurrentAudio = AudioPlayerFunction._playlist[PlayList.SelectedIndex];
                }
                else
                {
                    PlayList.SelectedIndex = 0;
                    AudioPlayerFunction.CurrentAudio = AudioPlayerFunction._playlist[PlayList.SelectedIndex];
                }
            }
            AudioPlayerFunction.Play(AudioPlayerFunction.CurrentAudio);
            AudioPlayerFunction.Volume = VolumeValue.Value;


            try
            {
                Song.MaximumSize = new Size(300, 50);
                Song.Text = "Song: " + AudioPlayerFunction.CurrentAudio.Title;
                Album.Text = "Album: " + AudioPlayerFunction.CurrentAudio.Album;
                Genre.Text = "Genre: " + AudioPlayerFunction.CurrentAudio.Genre;
                Composer.Text = "Composer: " + AudioPlayerFunction.CurrentAudio.Composer;
                // richTextBox1.Text = _audio.Lyrics;
                if (Image.FromStream(new MemoryStream((byte[])AudioPlayerFunction.CurrentAudio.Picture.Data.Data)).GetThumbnailImage(100, 100, null, IntPtr.Zero) == null)
                    return;
                else
                    AudioCover.Image = Image.FromStream(new MemoryStream((byte[])AudioPlayerFunction.CurrentAudio.Picture.Data.Data)).GetThumbnailImage(100, 100, null, IntPtr.Zero);
            }
            catch (Exception ex) { } // MessageBox.Show(ex.ToString()); }


            // await _fade.FadeIn(100, _basicplayer);
            // await _fade.FadeIn2(2000, _basicplayer);
            label1.Text = AudioPlayerFunction.CurrentAudioPosition.Hours.ToString("00") + ":" +
                AudioPlayerFunction.CurrentAudioPosition.Minutes.ToString("00") + ":" +
                AudioPlayerFunction.CurrentAudioPosition.Seconds.ToString("00");
            label2.Text = AudioPlayerFunction.CurrentAudioDuration.Hours.ToString("0") + ":" +
                AudioPlayerFunction.CurrentAudioDuration.Minutes.ToString("00") + ":" +
                AudioPlayerFunction.CurrentAudioDuration.Seconds.ToString("00");
            TimePosition.Maximum = (int)AudioPlayerFunction.CurrentAudioDuration.TotalSeconds;
            TimePosition.Value = (int)AudioPlayerFunction.CurrentAudioPosition.TotalSeconds;
            Tickstimer.Enabled = true;
        }

        private void Pausebutton_Click(object sender, EventArgs e)
        {
            AudioPlayerFunction.Pause();
        }


        private void Stopbutton_Click(object sender, EventArgs e)
        {
            if (PlayList.Items.Count != 0)
            {
                //await _fade.FadeOut(760, _basicplayer);
                //  await _fade.FadeOut2(50, _basicplayer);
                AudioPlayerFunction.Stop();
                label1.Text = "00:00:00";
                label2.Text = "00:00:00";
                TimePosition.Value = 0;
                Tickstimer.Enabled = false;
            }
        }
        private void Previous_Click(object sender, EventArgs e)
        {
            AudioPlayerFunction.Previous();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            AudioPlayerFunction.SwitchNext();
        }

        private void PlayList_DoubleClick(object sender, EventArgs e)
        {

            AudioPlayerFunction.CurrentAudio = AudioPlayerFunction._playlist[PlayList.SelectedIndex];
            AudioPlayerFunction.Volume = VolumeValue.Value;
            AudioPlayerFunction.Play(AudioPlayerFunction.CurrentAudio);


            try
            {
                Song.Text = "Song: " + AudioPlayerFunction.CurrentAudio.Title;
                Album.Text = "Album: " + AudioPlayerFunction.CurrentAudio.Album;
                Genre.Text = "Genre: " + AudioPlayerFunction.CurrentAudio.Genre;
                Composer.Text = "Composer: " + AudioPlayerFunction.CurrentAudio.Composer;
                // richTextBox1.Text = _audio.Lyrics;
                if (Image.FromStream(new MemoryStream((byte[])AudioPlayerFunction.CurrentAudio.Picture.Data.Data)).GetThumbnailImage(100, 100, null, IntPtr.Zero) == null)
                    return;
                else
                    AudioCover.Image = Image.FromStream(new MemoryStream((byte[])AudioPlayerFunction.CurrentAudio.Picture.Data.Data)).GetThumbnailImage(100, 100, null, IntPtr.Zero);
            }
            catch (Exception ex) { } //MessageBox.Show(ex.ToString()); }


            // await _fade.FadeIn(100, _basicplayer);
            // await _fade.FadeIn2(2000, _basicplayer);
            label1.Text = AudioPlayerFunction.CurrentAudioPosition.Hours.ToString("00") + ":" +
                AudioPlayerFunction.CurrentAudioPosition.Minutes.ToString("00") + ":" +
                AudioPlayerFunction.CurrentAudioPosition.Seconds.ToString("00");
            label2.Text = AudioPlayerFunction.CurrentAudioDuration.Hours.ToString("0") + ":" +
                AudioPlayerFunction.CurrentAudioDuration.Minutes.ToString("00") + ":" +
                AudioPlayerFunction.CurrentAudioDuration.Seconds.ToString("00");
            TimePosition.Maximum = (int)AudioPlayerFunction.CurrentAudioDuration.TotalSeconds;
            TimePosition.Value = (int)AudioPlayerFunction.CurrentAudioPosition.TotalSeconds;
            Tickstimer.Enabled = true;
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
                   // *.wav; *.mp3; *.flac; *.ac3; *.aac; *.wma"; добавить
                }

            foreach (string file in files)
            {
                if ((file.ToLower().EndsWith(".mp3")) || (file.ToLower().EndsWith(".wav")))
                {
                    filepath.Add(file);
                    _audio.AudioTags(file);
                    PlayList.Items.Add(_audio.Artist + " - " + _audio.Title);
                }
            }
            AudioPlayerFunction.AudioPath = filepath;
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
            AudioPlayerFunction.Dispose();
        }

        private void Tickstimer_Tick(object sender, EventArgs e)
        {
            label1.Text = AudioPlayerFunction.CurrentAudioPosition.Hours.ToString("00") + ":" +
                AudioPlayerFunction.CurrentAudioPosition.Minutes.ToString("00") + ":" +
                AudioPlayerFunction.CurrentAudioPosition.Seconds.ToString("00");

            TimePosition.Value = (int)AudioPlayerFunction.CurrentAudioPosition.TotalSeconds;
        }

        private void VolumeValue_Scroll(object sender, EventArgs e)
        {
            AudioPlayerFunction.Volume = VolumeValue.Value;
            label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold);
            label3.ForeColor = Color.Black;
            if (AudioPlayerFunction.Volume == 0)
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
                AudioPlayerFunction.CurrentAudioPosition = TimeSpan.FromSeconds(TimePosition.Value);
                label1.Text = AudioPlayerFunction.CurrentAudioPosition.Hours.ToString("00") + ":" +
                    AudioPlayerFunction.CurrentAudioPosition.Minutes.ToString("00") + ":" +
                    AudioPlayerFunction.CurrentAudioPosition.Seconds.ToString("00");
                Tickstimer.Enabled = true;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            AudioPlayerFunction.Save();
        }
    }
}
