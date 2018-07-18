using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications; // Notifications library
using Microsoft.QueryStringDotNET; // QueryString.NET
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.ViewManagement;
using Windows.Storage;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Jiujiu
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 



    public sealed partial class MainPage : Page
    {
        bool isPaneOpenPre = false;
        bool isNeedChange = false;
        public static string oldUri; 

        public MainPage()
        {
            this.InitializeComponent();

            MainFrame.Navigate(typeof(HomePage));
            isPaneOpenPre = MainNav.IsPaneOpen;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            oldUri = await BackgroundData.ReadUriDataAsync();
            if (oldUri == null)
            {
                oldUri = "ms-appx:///Assets/Background/Background_Blue.png";
                await BackgroundData.WriteUriDataAsync(oldUri);

            }

            MainGrid.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(oldUri))
            };
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                MainFrame.Navigate(typeof(SettingsPage));
                MainNav.Header = "设置";
            }
            else
            {
                //选中项的内容
                switch (args.InvokedItem)
                {
                    case "主页":
                        MainFrame.Navigate(typeof(HomePage));
                        MainNav.Header = "主页";
                        if (isNeedChange)
                        {
                            MainNav.IsPaneOpen = isPaneOpenPre;
                        }
                        break;
                    case "练习模式":
                        MainFrame.Navigate(typeof(PracticePage));
                        MainNav.Header = "练习模式";
                        if (isNeedChange)
                        {
                            MainNav.IsPaneOpen = isPaneOpenPre;
                        }
                        break;
                    case "比赛模式":
                        MainFrame.Navigate(typeof(GamePage));
                        MainNav.Header = "比赛模式";
                        if (isNeedChange)
                        {
                            MainNav.IsPaneOpen = isPaneOpenPre;
                        }
                        break;
                    case "九九乘法表":
                        isNeedChange = true;
                        isPaneOpenPre = MainNav.IsPaneOpen;
                        MainFrame.Navigate(typeof(TablePage));
                        MainNav.Header = "九九乘法表";
                        MainNav.IsPaneOpen = false;
                        break;
                    case "个人中心":
                        MainFrame.Navigate(typeof(PersonPage));
                        MainNav.Header = "个人中心";
                        if (isNeedChange)
                        {
                            MainNav.IsPaneOpen = isPaneOpenPre;
                        }
                        break;

                }
            }
        }
    }
}
