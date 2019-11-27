using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;

using MahApps.Metro.Controls;

namespace ScreenLocker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool _dismissEvent;
        public MainWindow()
        {
            InitializeComponent();
            var t = new Thread(MainThread);
            t.Start();
        }

        private void MainThread()
        {
            var r = new Random();
            var i = 0;
            while (true)
            {
                Thread.Sleep(r.Next(1000, 15000));

                Dispatcher?.Invoke(() => { DispatcherThread(i); });
                i++;
            }
        }

        private void DispatcherThread(int i)
        {
            var notification = (i % 10) switch
            {
                1 => new Notification("Remember to lock the computer",  "If you are seing this, maybe you should remember to lock your computer next time."),
                2 => new Notification("Security First !",               "Always lock your computer ! Someone might do it for you !"),
                3 => new Notification("Lock your computer !",           "You wouldn't want someone to put some virus on it, would you ?"),
                4 => new Notification("Do you lock your computer ?",    "Locking your computer greatly reduces the risks of having someone else using it."),
                5 => new Notification("It's easy !",                    "Press Windows + L to lock your coputer easily !"),
                6 => new Notification("Stay away from hackers",         "They cannot acces your data if you lock your computer"),
                7 => new Notification("ScreenLocker is here",           "Don't fret, I'm here to help !"),
                8 => new Notification("Don't let anyone near",          "Everyone want your data. Protect it !"),
                9 => new Notification("Safety should be your priority", "A safe computer is a locked computer"),
                _ => new Notification("Are you still here ?",           "If you're not seing this message, you're probably not in front of your computer. You should lock it to prevent people from accessing your data.")
            };
            if (i % 15 == 0 && i != 0)
            {
                notification = new Notification("Are you still here ?", "The session will lock up in 10 seconds if you don't dismiss this notification.") { Margin = new Thickness(5) };
                var flyout = new Flyout
                {
                    IsOpen                = false,
                    Position              = Position.Bottom,
                    CloseButtonVisibility = Visibility.Collapsed,
                    Content               = notification,
                    IsAutoCloseEnabled    = true,
                    AutoCloseInterval     = 10000
                };
                flyout.MouseLeftButtonUp += Flyout_MouseLeftButtonUp;
                flyout.MouseLeftButtonUp += Notification_Dismissed;
                notifPanel.Children.Add(flyout);
                flyout.IsOpen = true;

                var dismissThread = new Thread(notificationObj =>
                {
                    var notif    = notificationObj as Notification;
                    var timeLeft = TimeSpan.FromSeconds(10);
                    _dismissEvent = false;
                    while (!_dismissEvent && timeLeft.TotalSeconds > 0)
                    {
                        Thread.Sleep(1000);
                        timeLeft = timeLeft.Subtract(TimeSpan.FromMilliseconds(1000));
                        Dispatcher?.Invoke(() =>
                        {
                            if (notif != null) notif.lblContent.Text = $"The session will lock up in {timeLeft.TotalSeconds} seconds if you don't dismiss this notification.";
                        });
                    }
                    if (!_dismissEvent) { Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation"); }
                });
                dismissThread.Start(notification);
            }
            else
            {
                notification.Margin = new Thickness(5);
                var flyout = new Flyout
                {
                    IsOpen                = false,
                    Position              = Position.Bottom,
                    CloseButtonVisibility = Visibility.Collapsed,
                    Content               = notification,
                    IsAutoCloseEnabled    = true,
                    AutoCloseInterval     = 7000
                };
                notifPanel.Children.Add(flyout);
                flyout.MouseLeftButtonUp += Flyout_MouseLeftButtonUp;
                flyout.IsOpen            =  true;
            }
        }

        private static void Flyout_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        { ((Flyout)sender).IsOpen = false; }

        private void Notification_Dismissed(object sender, MouseButtonEventArgs e)
        {
            _dismissEvent = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            var window = (Window)sender;
            window.Topmost = true;
        }
    }
}
