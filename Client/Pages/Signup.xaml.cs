using API;
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
using System.Windows.Shapes;

namespace Client.Pages
{
    /// <summary>
    /// Signup.xaml 的交互逻辑
    /// </summary>
    public partial class Signup : Window
    {
        private string msgId = "";

        public Signup()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            GiftRoomApi.Init("http://112.74.49.61:8090/GiftRoom", "");
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if(textBox!=null)
            {

                if("        密码     ".Contains(textBox.Text))
                {
                    this.Password_pwd.Visibility = Visibility.Visible;
                    //this.Password_pwd.Focus();
                    this.Text_pwd.Visibility = Visibility.Hidden; 
                }
                if(textBox.Foreground.ToString().Contains("#FF716666") || textBox.Foreground.ToString().Contains("#FF62BDFF"))
                {
                    textBox.Text = "";
                    textBox.Foreground= Brushes.Black;
                }
                
            }
        }

        private void btn_GetVerificationCode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            try
            {

                if (this.btn_GetVerificationCode.Content.ToString().Contains("验证码"))
                {
                    var phone = this.Text_phone.Text;
                    if (string.IsNullOrWhiteSpace(phone))
                    {
                        return;
                    }

                    var sendResult = GiftRoomApi.Create<IAccountInterface>().SendVerificationCode(phone);
                    if (!"0".Equals(sendResult.code))
                    {
                        MessageBox.Show(sendResult.message);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(sendResult.data.msgId))
                    {
                        MessageBox.Show("发送失败");
                        return;
                    }
                    this.msgId = sendResult.data.msgId;

                    Task.Factory.StartNew(() =>
                    {
                        for (int i = 60; i >= 0; i--)
                        {
                            this.btn_GetVerificationCode.Dispatcher.Invoke(() =>
                            {
                                this.btn_GetVerificationCode.Content = i;
                            });
                            Thread.Sleep(1000);
                        }
                        this.btn_GetVerificationCode.Dispatcher.Invoke(() =>
                        {
                            this.btn_GetVerificationCode.Content = "获取验证码";
                        });
                    });

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {



            try
            {

                if (string.IsNullOrWhiteSpace(msgId))
                {
                    MessageBox.Show("请先输入电话后获取验证码");
                    return;
                }
                var userName = this.Text_UserName.Text;
                var pwd = this.Password_pwd.Password;
                var phone = this.Text_phone.Text;
                var code = this.Text_code.Text;

                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(pwd) ||
                    string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(code))
                {
                    return;
                }

                var signupResult = GiftRoomApi.Create<IAccountInterface>().Signup(userName, phone, pwd, code, msgId);
                if (!"0".Equals(signupResult.code))
                {
                    MessageBox.Show(signupResult.message);
                    return;
                }
                MessageBox.Show("注册成功，确认后登录");

                new MainWindow().Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
