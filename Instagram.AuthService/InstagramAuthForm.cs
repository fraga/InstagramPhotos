using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Instagram.Photos.AuthService
{
    public partial class InstagramAuthForm : Form
    {
        public InstagramAuthForm()
        {
            InitializeComponent();
        }

        public InstagramAuthForm(string response)
        {
            InitializeComponent();
            CleanCookies();
            webBrowser1.Navigate(response);
        }

        public void CleanCookies()
        {
            var cookiesDir = Environment.GetFolderPath(Environment.SpecialFolder.Cookies, Environment.SpecialFolderOption.None);

            if (!Directory.Exists(cookiesDir)) return;

            try
            {
                new DirectoryInfo(cookiesDir).GetFiles().ToList().ForEach(f => f.Delete());
            }
            catch (IOException)
            {
            }
        }
    }
}
