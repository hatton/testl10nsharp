﻿using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace L10NSharp.UI
{
    public partial class HowToDistributeDialog : Form
    {
        private readonly string _targetTmxFilePath;

        public HowToDistributeDialog(string emailForSubmissions, string targetTmxFilePath)
        {
            _targetTmxFilePath = targetTmxFilePath;
            InitializeComponent();

#if __MonoCS__
			// In Mono, Label.AutoSize=true sets Size to PreferredSize (which is always
			// one line high) even if the Size has already been explicitly set.  In Windows,
			// Label.AutoSize=false makes the labels disappear.  So we need to turn off
			// AutoSize here and set the multiline labels explicitly to their largest
			// possible sizes for this fixed-size dialog.  (That allows all the available
			// space for localizations that may need more space.)
			label1.AutoSize = label2.AutoSize = label4.AutoSize = false;
			label4.Size = new System.Drawing.Size(300, 142);	// top message
			label2.Size = new System.Drawing.Size(300, 56);		// middle message
			label1.Size = new System.Drawing.Size(300, 112);	// bottom message
#endif
            _emailLabel.Text=emailForSubmissions;
        }

        private void OnShowTMXFile(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var path = _targetTmxFilePath;
#if __MonoCS__
            MessageBox.Show(
                "Sorry, this function isn't implemented for the Linux version yet. The file you want is at " +
                _targetTmxFilePath);
#else
            path = path.Replace("/", "\\"); //forward slashes kill the selection attempt and it opens in My Documents.

            if (!File.Exists(path))
            {
                MessageBox.Show("Sorry, the TMX file hasn't been saved yet, so we can't show it to you yet.");
                return;
            }
            Process.Start("explorer.exe", "/select, \"" + path + "\"");
  #endif

        }

        private void _emailLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:" + _emailLabel.Text);
        }

    }
}
