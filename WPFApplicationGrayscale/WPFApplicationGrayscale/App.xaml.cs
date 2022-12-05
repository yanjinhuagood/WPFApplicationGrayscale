using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFDevelopers.Helpers;

namespace WPFApplicationGrayscale
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static double Wdith
        {
            get { return SystemParameters.WorkArea.Width / 1.5; }
        }
        public static double Height
        {
            get { return SystemParameters.WorkArea.Height / 1.5; }
        }
        public static ThemeType Theme { get; set; }
    }
}
