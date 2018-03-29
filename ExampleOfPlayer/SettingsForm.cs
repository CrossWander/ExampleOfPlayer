using System;
using System.Windows.Forms;

namespace ExampleOfPlayer
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.AudioLibraryChosing.Text = Properties.Settings.Default.DefaultAudioLibrary;
            this.AudioLibraryChosing.SelectedIndex = Properties.Settings.Default.AudioLibrary;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DefaultAudioLibrary = this.AudioLibraryChosing.Text;
            Properties.Settings.Default.AudioLibrary = this.AudioLibraryChosing.SelectedIndex;
            Properties.Settings.Default.Save();
            this.Close();
            Application.Restart();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
