using System;
using System.Windows.Controls;

namespace ScreenLocker
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : UserControl
    {
        internal Label labelTitle;
        public string Title {get; set;}
        public string Description {get; set;}
        public string Time {get; set;}

        public Notification()
        {
            InitializeComponent();
            Time = DateTime.Now.ToString("HH:ss");
        }

        public Notification(string title, string content)
        {
            InitializeComponent();
            Title = title;
            lblTitle.Content = title;
            labelTitle = lblTitle;
            Description = content;
            lblContent.Text = content;
            Time = DateTime.Now.ToString("HH:ss");
            lblTime.Content = Time;
            DataContext = this;
        }
    }
}
