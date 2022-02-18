using Beiming.app.login;
using Beiming.app.setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Beiming.app
{
    /// <summary>
    /// Mian.xaml 的交互逻辑
    /// </summary>
    public partial class Mian : Window
    {
        public Mian()
        {
            InitializeComponent();
        }

        private void Out_Login_Click(object sender, RoutedEventArgs e)
        {
            LogoPanel logo = new LogoPanel();
            logo.Show();
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            SettingPanel setting = new SettingPanel();
            setting.Show();
        }

        private void Pwd_Chage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pwd_Chage_Click");
        }

    }
}
