﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.WindowsAPI.Dialogs;
using Microsoft.WindowsAPI.Shell;

using GmailNotifierPlus.Localization;
using GmailNotifierPlus.Utilities;

using Shellscape;

namespace GmailNotifierPlus.Forms {

	public partial class FirstRun : Shellscape.UI.Form {

		private Bitmap _background;

		public FirstRun() {
			InitializeComponent();

			this.Icon = Resources.Icons.Window;
			this.Text = Resources.Strings.WindowTitle;

			_ButtonOK.Click += _ButtonOk_Click;
			_ButtonOK.Text = Localization.Locale.Current.About.FirstRun;

			_background = Utilities.ResourceHelper.GetImage("intro.png");

			this.ClientSize = new Size(_background.Width, _background.Height + (this.Padding.Bottom + this.Padding.Top) + _ButtonOK.Height);
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);

			this.TopMost = false;
			this.Focus();
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			e.Graphics.DrawImage(_background, 0, 0, _background.Width, _background.Height);
		}

		private void _ButtonOk_Click(object sender, EventArgs e) {

			Program.MainForm.Jumplist_ShowPreferences(new String[] { "firstrun" });

			Close();
		}

	}
}
