namespace ExampleOfPlayer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_basicplayer != null) _basicplayer.Dispose();
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Browse = new System.Windows.Forms.Button();
            this.Playbutton = new System.Windows.Forms.Button();
            this.Stopbutton = new System.Windows.Forms.Button();
            this.PlayList = new System.Windows.Forms.ListBox();
            this.Settings = new System.Windows.Forms.Button();
            this.Pausebutton = new System.Windows.Forms.Button();
            this.TimePosition = new System.Windows.Forms.TrackBar();
            this.RepeatBut = new System.Windows.Forms.CheckBox();
            this.VolumeValue = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Previous = new System.Windows.Forms.Button();
            this.Next = new System.Windows.Forms.Button();
            this.Tickstimer = new System.Windows.Forms.Timer(this.components);
            this.AudioCover = new System.Windows.Forms.PictureBox();
            this.Song = new System.Windows.Forms.Label();
            this.Album = new System.Windows.Forms.Label();
            this.Genre = new System.Windows.Forms.Label();
            this.Composer = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TimePosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AudioCover)).BeginInit();
            this.SuspendLayout();
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(569, 229);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(86, 23);
            this.Browse.TabIndex = 0;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Playbutton
            // 
            this.Playbutton.Location = new System.Drawing.Point(12, 340);
            this.Playbutton.Name = "Playbutton";
            this.Playbutton.Size = new System.Drawing.Size(82, 56);
            this.Playbutton.TabIndex = 1;
            this.Playbutton.Text = "Play";
            this.Playbutton.UseVisualStyleBackColor = true;
            this.Playbutton.Click += new System.EventHandler(this.Playbutton_Click);
            // 
            // Stopbutton
            // 
            this.Stopbutton.Location = new System.Drawing.Point(156, 353);
            this.Stopbutton.Name = "Stopbutton";
            this.Stopbutton.Size = new System.Drawing.Size(50, 31);
            this.Stopbutton.TabIndex = 2;
            this.Stopbutton.Text = "Stop";
            this.Stopbutton.UseVisualStyleBackColor = true;
            this.Stopbutton.Click += new System.EventHandler(this.Stopbutton_Click);
            // 
            // PlayList
            // 
            this.PlayList.AllowDrop = true;
            this.PlayList.FormattingEnabled = true;
            this.PlayList.Location = new System.Drawing.Point(322, 11);
            this.PlayList.Name = "PlayList";
            this.PlayList.Size = new System.Drawing.Size(333, 212);
            this.PlayList.TabIndex = 3;
            this.PlayList.DragDrop += new System.Windows.Forms.DragEventHandler(this.PlayList_DragDrop);
            this.PlayList.DragEnter += new System.Windows.Forms.DragEventHandler(this.PlayList_DragEnter);
            this.PlayList.DoubleClick += new System.EventHandler(this.PlayList_DoubleClick);
            // 
            // Settings
            // 
            this.Settings.Location = new System.Drawing.Point(322, 229);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(75, 23);
            this.Settings.TabIndex = 4;
            this.Settings.Text = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            // 
            // Pausebutton
            // 
            this.Pausebutton.Location = new System.Drawing.Point(100, 353);
            this.Pausebutton.Name = "Pausebutton";
            this.Pausebutton.Size = new System.Drawing.Size(50, 31);
            this.Pausebutton.TabIndex = 5;
            this.Pausebutton.Text = "Pause";
            this.Pausebutton.UseVisualStyleBackColor = true;
            this.Pausebutton.Click += new System.EventHandler(this.Pausebutton_Click);
            // 
            // TimePosition
            // 
            this.TimePosition.Location = new System.Drawing.Point(64, 289);
            this.TimePosition.Name = "TimePosition";
            this.TimePosition.Size = new System.Drawing.Size(460, 45);
            this.TimePosition.TabIndex = 6;
            this.TimePosition.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TimePosition.Scroll += new System.EventHandler(this.TimePosition_Scroll);
            this.TimePosition.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TimePosition_MouseDown);
            this.TimePosition.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TimePosition_MouseUp);
            // 
            // RepeatBut
            // 
            this.RepeatBut.AutoSize = true;
            this.RepeatBut.Location = new System.Drawing.Point(704, 340);
            this.RepeatBut.Name = "RepeatBut";
            this.RepeatBut.Size = new System.Drawing.Size(61, 17);
            this.RepeatBut.TabIndex = 7;
            this.RepeatBut.Text = "Repeat";
            this.RepeatBut.UseVisualStyleBackColor = true;
            this.RepeatBut.Click += new System.EventHandler(this.RepeatBut_Click);
            // 
            // VolumeValue
            // 
            this.VolumeValue.Location = new System.Drawing.Point(661, 289);
            this.VolumeValue.Maximum = 100;
            this.VolumeValue.Name = "VolumeValue";
            this.VolumeValue.Size = new System.Drawing.Size(104, 45);
            this.VolumeValue.TabIndex = 8;
            this.VolumeValue.TickFrequency = 0;
            this.VolumeValue.TickStyle = System.Windows.Forms.TickStyle.None;
            this.VolumeValue.Value = 100;
            this.VolumeValue.Scroll += new System.EventHandler(this.VolumeValue_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(7, 291);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "00:00:00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(529, 291);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "00:00:00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(595, 289);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "Volume";
            // 
            // Previous
            // 
            this.Previous.Location = new System.Drawing.Point(223, 353);
            this.Previous.Name = "Previous";
            this.Previous.Size = new System.Drawing.Size(50, 31);
            this.Previous.TabIndex = 13;
            this.Previous.Text = "<<";
            this.Previous.UseVisualStyleBackColor = true;
            this.Previous.Click += new System.EventHandler(this.Previous_Click);
            // 
            // Next
            // 
            this.Next.Location = new System.Drawing.Point(279, 353);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(50, 31);
            this.Next.TabIndex = 14;
            this.Next.Text = ">>";
            this.Next.UseVisualStyleBackColor = true;
            this.Next.Click += new System.EventHandler(this.Next_Click);
            // 
            // Tickstimer
            // 
            this.Tickstimer.Interval = 500;
            this.Tickstimer.Tick += new System.EventHandler(this.Tickstimer_Tick);
            // 
            // AudioCover
            // 
            this.AudioCover.Location = new System.Drawing.Point(12, 12);
            this.AudioCover.Name = "AudioCover";
            this.AudioCover.Size = new System.Drawing.Size(165, 163);
            this.AudioCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.AudioCover.TabIndex = 15;
            this.AudioCover.TabStop = false;
            // 
            // Song
            // 
            this.Song.AutoSize = true;
            this.Song.Location = new System.Drawing.Point(9, 188);
            this.Song.Name = "Song";
            this.Song.Size = new System.Drawing.Size(32, 13);
            this.Song.TabIndex = 16;
            this.Song.Text = "Song";
            // 
            // Album
            // 
            this.Album.AutoSize = true;
            this.Album.Location = new System.Drawing.Point(9, 210);
            this.Album.Name = "Album";
            this.Album.Size = new System.Drawing.Size(36, 13);
            this.Album.TabIndex = 17;
            this.Album.Text = "Album";
            // 
            // Genre
            // 
            this.Genre.AutoSize = true;
            this.Genre.Location = new System.Drawing.Point(9, 258);
            this.Genre.Name = "Genre";
            this.Genre.Size = new System.Drawing.Size(36, 13);
            this.Genre.TabIndex = 18;
            this.Genre.Text = "Genre";
            // 
            // Composer
            // 
            this.Composer.AutoSize = true;
            this.Composer.Location = new System.Drawing.Point(9, 235);
            this.Composer.Name = "Composer";
            this.Composer.Size = new System.Drawing.Size(54, 13);
            this.Composer.TabIndex = 19;
            this.Composer.Text = "Composer";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(438, 229);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(86, 23);
            this.SaveButton.TabIndex = 20;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 408);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.Composer);
            this.Controls.Add(this.Genre);
            this.Controls.Add(this.Album);
            this.Controls.Add(this.Song);
            this.Controls.Add(this.AudioCover);
            this.Controls.Add(this.Next);
            this.Controls.Add(this.Previous);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.VolumeValue);
            this.Controls.Add(this.RepeatBut);
            this.Controls.Add(this.TimePosition);
            this.Controls.Add(this.Pausebutton);
            this.Controls.Add(this.Settings);
            this.Controls.Add(this.PlayList);
            this.Controls.Add(this.Stopbutton);
            this.Controls.Add(this.Playbutton);
            this.Controls.Add(this.Browse);
            this.Name = "Form1";
            this.Text = "Example of Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.TimePosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AudioCover)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Button Playbutton;
        private System.Windows.Forms.Button Stopbutton;
        private System.Windows.Forms.ListBox PlayList;
        private System.Windows.Forms.Button Settings;
        private System.Windows.Forms.Button Pausebutton;
        private System.Windows.Forms.TrackBar TimePosition;
        private System.Windows.Forms.CheckBox RepeatBut;
        private System.Windows.Forms.TrackBar VolumeValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Previous;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.Timer Tickstimer;
        private System.Windows.Forms.PictureBox AudioCover;
        private System.Windows.Forms.Label Song;
        private System.Windows.Forms.Label Album;
        private System.Windows.Forms.Label Genre;
        private System.Windows.Forms.Label Composer;
        private System.Windows.Forms.Button SaveButton;
    }
}

