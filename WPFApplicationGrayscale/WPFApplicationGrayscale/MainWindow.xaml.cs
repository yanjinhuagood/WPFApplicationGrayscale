using GrayscaleEffectShared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using WPFDevelopers.Controls;
using WPFDevelopers.Helpers;
using MessageBox = WPFDevelopers.Controls.MessageBox;

namespace WPFApplicationGrayscale
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static readonly DependencyProperty UserCollectionProperty =
           DependencyProperty.Register("UserCollection", typeof(ObservableCollection<UserModel>), typeof(MainWindow),
               new PropertyMetadata(null));

        public static readonly DependencyProperty AllSelectedProperty =
            DependencyProperty.Register("AllSelected", typeof(bool), typeof(MainWindow),
                new PropertyMetadata(AllSelectedChangedCallback));

        public static readonly DependencyProperty ThemesCollectionProperty =
            DependencyProperty.Register("ThemesCollection", typeof(ObservableCollection<ThemeModel>), typeof(MainWindow),
                new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainView_Loaded;
        }
        public ObservableCollection<ThemeModel> ThemesCollection
        {
            get => (ObservableCollection<ThemeModel>)GetValue(ThemesCollectionProperty);
            set => SetValue(ThemesCollectionProperty, value);
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Theme == ThemeType.Dark)
                tbLightDark.IsChecked = true;
            else
                tbLightDark.IsChecked = false;

            myPasswordBox.Password = "WPFDevelopers.Minimal";
            var time = DateTime.Now;
            UserCollection = new ObservableCollection<UserModel>();
            for (var i = 0; i < 4; i++)
            {
                UserCollection.Add(new UserModel
                {
                    Date = time,
                    Name = "WPFDevelopers",
                    Address = "No. 189, Grove St, Los Angeles",
                    Children = new List<UserModel>
                    {
                        new UserModel { Name = "WPFDevelopers.Minimal1.1" },
                        new UserModel { Name = "WPFDevelopers.Minimal1.2" },
                        new UserModel { Name = "WPFDevelopers.Minimal1.3" },
                        new UserModel { Name = "WPFDevelopers.Minimal1.4" },
                        new UserModel { Name = "WPFDevelopers.Minimal1.5" },
                        new UserModel { Name = "WPFDevelopers.Minimal1.6" }
                    }
                });
                time = time.AddDays(2);
            }

            if (ThemesCollection != null)
            {
                var model = ThemesCollection.FirstOrDefault(x => x.Color == "#B31B1B");
                if (model != null) return;
                ThemesCollection.Add(new ThemeModel
                {
                    Color = "#B31B1B",
                    ResourcePath =
                        "pack://application:,,,/WPFApplicationGrayscale;component/Light.Carmine.xaml"
                });
            }

        }

        private void btnInformation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("文件删除成功。", "消息", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnWarning_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("当前文件不存在！", "警告", MessageBoxImage.Warning);
        }

        private void btnError_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("当前文件不存在。", "错误", MessageBoxImage.Error);
        }

        private void btnQuestion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("当前文件不存在,是否继续?", "询问", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        private void GithubHyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void GiteeHyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void QQHyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var uri = new Uri(@"https://qm.qq.com/cgi-bin/qm/qr?k=f2zl3nvoetItho8kGfe1eys0jDkqvvcL&jump_from=webapi");
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Loading_Click(object sender, RoutedEventArgs e)
        {
            var task = new Task(() => { Thread.Sleep(5000); });
            task.ContinueWith(previousTask => { Loading.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());
            Loading.Show();
            task.Start();
        }
        private void LoadingOff_Click(object sender, RoutedEventArgs e)
        {
            var task = new Task(() => { Thread.Sleep(5000); });
            task.ContinueWith(previousTask => { Loading.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());
            Loading.Show(true);
            task.Start();
        }

        /// <summary>
        /// 此处演示关闭loading停止任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadingOffTask_Click(object sender, RoutedEventArgs e)
        {
            var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var task = new Task(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    //这里做自己的事情
                    if (tokenSource.IsCancellationRequested)
                        return;
                    Thread.Sleep(1000);
                }
            }, cancellationToken);
            task.ContinueWith(previousTask =>
            {
                if (tokenSource.IsCancellationRequested)
                    return;
                Loading.Close();
            }, TaskScheduler.FromCurrentSynchronizationContext());
            Loading.Show(true);
            Loading.LoadingQuitEvent += delegate
            {
                tokenSource.Cancel();
            };
            task.Start();
        }
        private void BtnLoading_Click(object sender, RoutedEventArgs e)
        {
            var task = new Task(() => { Thread.Sleep(5000); });
            task.ContinueWith(previousTask => { Loading.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());
            Loading.Show(btnLoading, 18.0d, System.Windows.Media.Brushes.White);
            task.Start();
        }
        private void LightDark_Checked(object sender, RoutedEventArgs e)
        {
            var lightDark = sender as ToggleButton;
            if (lightDark == null) return;
            var theme = lightDark.IsChecked.Value ? ThemeType.Dark : ThemeType.Light;
            if (App.Theme == theme) return;
            App.Theme = theme;
            ControlsHelper.ToggleLightAndDark(lightDark.IsChecked == true);
        }

        #region DataSource

        public ObservableCollection<UserModel> UserCollection
        {
            get => (ObservableCollection<UserModel>)GetValue(UserCollectionProperty);
            set => SetValue(UserCollectionProperty, value);
        }


        public bool AllSelected
        {
            get => (bool)GetValue(AllSelectedProperty);
            set => SetValue(AllSelectedProperty, value);
        }


        private static void AllSelectedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as MainWindow;
            var isChecked = (bool)e.NewValue;
            if ((bool)e.NewValue)
                view.UserCollection.ToList().ForEach(y => y.IsChecked = isChecked);
            else
                view.UserCollection.ToList().ForEach(y => y.IsChecked = isChecked);
        }




        #endregion

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbGrayscale_Checked(object sender, RoutedEventArgs e)
        {
            Create(0);
        }
        void Create(double to)
        {
            var sineEase = new SineEase() { EasingMode = EasingMode.EaseOut };
            var doubleAnimation = new DoubleAnimation
            {
                To = to,
                Duration = TimeSpan.FromMilliseconds(1000),
                EasingFunction = sineEase
            };
            grayscaleEffect.BeginAnimation(GrayscaleEffect.FactorProperty, doubleAnimation);
        }
        private void tbGrayscale_Unchecked(object sender, RoutedEventArgs e)
        {
            Create(1);
        }
    }
}
