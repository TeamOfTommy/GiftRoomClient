using API;
using Client.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private BitmapImage checkedBack = new BitmapImage(new Uri("/Resources/login-checked.png", UriKind.Relative));
        private BitmapImage uncheckedBack = new BitmapImage(new Uri("/Resources/login-unchecked.png", UriKind.Relative));
        public MainWindow()
        {
            InitializeComponent();
            this.CheckBox_AutoLogin.Source = uncheckedBack;
            this.CheckBox_RememberPwd.Source = uncheckedBack;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            GiftRoomApi.Init("http://112.74.49.61:8090/GiftRoom", "");
        }


        private void CheckBox_AutoLogin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var backImg = this.CheckBox_AutoLogin.Source.ToString();
            if(backImg.Contains("unchecked"))
            {
                this.CheckBox_AutoLogin.Source = checkedBack;
            }
            else
            {
                this.CheckBox_AutoLogin.Source = uncheckedBack;
            }

        }

        private void CheckBox_RememberPwd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        { 
            var backImg = this.CheckBox_RememberPwd.Source.ToString();
            if (backImg.Contains("unchecked"))
            {
                this.CheckBox_RememberPwd.Source = checkedBack;
            }
            else
            {
                this.CheckBox_RememberPwd.Source = uncheckedBack;
            }
        }

        private void btn_login_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            { 
                var userName = this.TextBox_UserName.Text;
                var pwd = this.TextBox_Pwd.Password;

                var autoLogin = !this.CheckBox_AutoLogin.Source.ToString().Contains("unchecked");
                var rememberPwd = !this.CheckBox_RememberPwd.Source.ToString().Contains("unchecked");

                var signinResult = GiftRoomApi.Create<IAccountInterface>().CheckPassword(userName, pwd);

                if (!"0".Equals(signinResult.code))
                {
                    MessageBox.Show(signinResult.message);
                    return;
                }

                MessageBox.Show("登录成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btn_signup_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var signUpWindow = new Signup();
            signUpWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;


            signUpWindow.Show();
            this.Close();
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userName = this.TextBox_UserName.Text;
                var pwd = this.TextBox_Pwd.Password;

                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(pwd))
                {
                    return;
                }

                var autoLogin = !this.CheckBox_AutoLogin.Source.ToString().Contains("unchecked");
                var rememberPwd = !this.CheckBox_RememberPwd.Source.ToString().Contains("unchecked");

                var signinResult = GiftRoomApi.Create<IAccountInterface>().CheckPassword(userName, pwd);

                if (!"0".Equals(signinResult.code))
                {
                    MessageBox.Show(signinResult.message);
                    return;
                }

                if (signinResult.data == null)
                {
                    MessageBox.Show("用户名或密码错误");
                    return;
                }

                MessageBox.Show("登录成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); 
            }

        }

         
    }
}
