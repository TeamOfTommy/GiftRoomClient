using API;
using API.Interface.Account;
using Client.UserControls;
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

namespace Client.Pages
{
    /// <summary>
    /// RoomList.xaml 的交互逻辑
    /// </summary>
    public partial class RoomList : Window
    {
        public RoomList()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            GiftRoomApi.Init("http://112.74.49.61:8090/GiftRoom", "");
            InitRoomList("");
        }

        private void InitRoomList(string searchKey)
        {

            //var roomList = GiftRoomApi.Create<IRoomInterface>().GetAll();
            var roomList = new GetAllRoomResult
            {
                data=new List<GetAllRoomResult.Data>
                {
                    new GetAllRoomResult.Data
                    {
                        imgUrl="https://www.baidu.com/img/bd_logo1.png",
                        RoomTitle="世界第一房",
                        UserOnlineNum=1,
                    },
                }
            };
            for (int i = 0; i < 10; i++)
            {
                roomList.data.Add(new GetAllRoomResult.Data
                {
                    imgUrl = "https://www.baidu.com/img/bd_logo1.png",
                    RoomTitle = "世界第一房",
                    UserOnlineNum = i,
                });
            }

            foreach (var item in roomList.data.Where(r=>string.IsNullOrWhiteSpace(searchKey)||r.RoomTitle.Contains(searchKey)))
            {
                var roomItem = new RoomItem
                {

                };
                roomItem.RoomPic1.Source = new BitmapImage(new Uri(item.imgUrl));
                roomItem.RoomPic2.Source = new BitmapImage(new Uri(item.imgUrl));
                roomItem.RoomTitle1.Content = item.RoomTitle;
                roomItem.RoomTitle2.Content = item.RoomTitle;
                roomItem.RoomOnline1.Content = string.Format("{0}人在线",item.UserOnlineNum);
                roomItem.RoomOnline2.Content = string.Format("{0}人在线", item.UserOnlineNum);


                this.RoomPannel.Children.Add(roomItem);
            }

        }

        private void TextBox_searchKey_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { 
        }

        private void TextBox_searchKey_MouseDown(object sender, MouseButtonEventArgs e)
        { 
        }

        private void TextBox_searchKey_GotFocus(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(this.TextBox_searchKey.Text) && this.TextBox_searchKey.Text.Contains("关键字"))
            {
                this.TextBox_searchKey.Text = "";
            }
        }

        private void TextBox_searchKey_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                this.InitRoomList(this.TextBox_searchKey.Text);
            }
        }
    }
}
