using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Store;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Jiujiu
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        AchievementData achievementData = new AchievementData();
        StorageFile sourceFile = null;
        StorageFolder targetFolder = ApplicationData.Current.LocalFolder;
        int i = 0;
        string uri = null;
        bool isCustomed = false;

        public SettingsPage()
        {
            this.InitializeComponent();
            if (Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
            {
                this.FeedbackByHubStack.Visibility = Visibility.Visible;
                this.FeedbackByEmailStack.Visibility = Visibility.Collapsed;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void RateBtn_Click(object sender, RoutedEventArgs e)
        {
            BackgroundGridView.Visibility = Visibility.Collapsed;
            ConfirmBtn.Visibility = Visibility.Collapsed;
            CancelBtn.Visibility = Visibility.Collapsed;
            LogoImage.Height = 300;
            LogoImage.Width = 300;
            ChangePanel.Background = null;
            Grid mainGrid = (Grid)((NavigationView)((Frame)(this.Parent)).Parent).Parent;
            mainGrid.Background = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(MainPage.oldUri))
            };
            bool b = await ShowRatingReviewDialog();
            await achievementData.ReadAchievementDataAsync();
            if (achievementData.Arate == false && b)
            {
                achievementData.Arate = true;
                HomePage.MakeToast("为我们评分");
                await achievementData.WriteAchievementDataAsync();
            }
        }

        private async void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            BackgroundGridView.Visibility = Visibility.Collapsed;
            ConfirmBtn.Visibility = Visibility.Collapsed;
            CancelBtn.Visibility = Visibility.Collapsed;
            LogoImage.Height = 300;
            LogoImage.Width = 300;
            ChangePanel.Background = null;
            Grid mainGrid = (Grid)((NavigationView)((Frame)(this.Parent)).Parent).Parent;
            mainGrid.Background = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(MainPage.oldUri))
            };
            ContentDialog dialog = new ContentDialog
            {
                Title = "清空数据",
                Content = "确认清空所有数据？一旦清空，数据将不会恢复！",
                CloseButtonText = "取消",
                PrimaryButtonText = "清空"
            };

            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await Data.DeleteAllDataAsync();
                await new ContentDialog
                {
                    Title = "数据清空",
                    Content = "已清空所有数据！",
                    CloseButtonText = "好的"
                }.ShowAsync();
            }
        }

        public async Task<bool> ShowRatingReviewDialog()
        {
            StoreSendRequestResult result = await StoreRequestHelper.SendRequestAsync(
                StoreContext.GetDefault(), 16, String.Empty);

            if (result.ExtendedError == null)
            {
                JObject jsonObject = JObject.Parse(result.Response);
                if (jsonObject.SelectToken("status").ToString() == "success")
                {
                    // The customer rated or reviewed the app.
                    return true;
                }
            }

            // There was an error with the request, or the customer chose not to
            // rate or review the app.
            return false;
        }

        private async void FeedbackBtn_Click(object sender, RoutedEventArgs e)
        {
            BackgroundGridView.Visibility = Visibility.Collapsed;
            ConfirmBtn.Visibility = Visibility.Collapsed;
            CancelBtn.Visibility = Visibility.Collapsed;
            LogoImage.Height = 300;
            LogoImage.Width = 300;
            ChangePanel.Background = null;
            Grid mainGrid = (Grid)((NavigationView)((Frame)(this.Parent)).Parent).Parent;
            mainGrid.Background = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(MainPage.oldUri))
            };
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

        private void ChangeBackgroundBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BackgroundGridView.Visibility == Visibility.Visible)
            {
                // 要隐藏保存按钮
                BackgroundGridView.Visibility = Visibility.Collapsed;
                ConfirmBtn.Visibility = Visibility.Collapsed;
                CancelBtn.Visibility = Visibility.Collapsed;
                LogoImage.Height = 300;
                LogoImage.Width = 300;
                ChangePanel.Background = null;
                Grid mainGrid = (Grid)((NavigationView)((Frame)(this.Parent)).Parent).Parent;
                mainGrid.Background = new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri(MainPage.oldUri))
                };
            }
            else
            {
                // 要显示保存按钮
                BackgroundGridView.Visibility = Visibility.Visible;
                ConfirmBtn.Visibility = Visibility.Visible;
                CancelBtn.Visibility = Visibility.Visible;
                LogoImage.Height = 200;
                LogoImage.Width = 200;
                ChangePanel.Background = new SolidColorBrush(Windows.UI.Colors.Black);
                ChangePanel.Background.Opacity = 0.1;
            }
        }

        private async void BackgroundGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            string name = ((Image)e.ClickedItem).Name;
            
            switch (name)
            {
                case "green": uri = "ms-appx:///Assets/Background/Background_Green.png"; isCustomed = true; break;
                case "blue": uri = "ms-appx:///Assets/Background/Background_Blue.png"; isCustomed = true; break;
                case "yellow": uri = "ms-appx:///Assets/Background/Background_Yellow.png"; isCustomed = true; break;
                case "pink": uri = "ms-appx:///Assets/Background/Background_Pink.png"; isCustomed = true; break;
                default: uri = null; isCustomed = false; break;
            }


            if (uri == null)
            {
                var picker = new Windows.Storage.Pickers.FileOpenPicker();
                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");
                sourceFile = await picker.PickSingleFileAsync();
                sourceFile = await sourceFile.CopyAsync(targetFolder, String.Format("{0}.png",i++), NameCollisionOption.ReplaceExisting);
                uri = sourceFile.Path;
            }
            else
            {
                sourceFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));
            }
            Grid mainGrid = (Grid)((NavigationView)((Frame)(this.Parent)).Parent).Parent;
            mainGrid.Background = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(uri))
            };
            
          
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            BackgroundGridView.Visibility = Visibility.Collapsed;
            ConfirmBtn.Visibility = Visibility.Collapsed;
            CancelBtn.Visibility = Visibility.Collapsed;
            LogoImage.Height = 300;
            LogoImage.Width = 300;
            ChangePanel.Background = null;
            Grid mainGrid = (Grid)((NavigationView)((Frame)(this.Parent)).Parent).Parent;
            mainGrid.Background = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(MainPage.oldUri))
            };
        }

        private async void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            BackgroundGridView.Visibility = Visibility.Collapsed;
            ConfirmBtn.Visibility = Visibility.Collapsed;
            CancelBtn.Visibility = Visibility.Collapsed;
            LogoImage.Height = 300;
            LogoImage.Width = 300;
            ChangePanel.Background = null;
            if (isCustomed == true)
            {
                sourceFile = await sourceFile.CopyAsync(targetFolder, "currentCus.png", NameCollisionOption.ReplaceExisting);

            }
            await BackgroundData.WriteUriDataAsync(sourceFile.Path);
        }

        //    try
        //    {
        //        mainGrid.Background = null;
        //        ChangingRing.IsActive = true;
        //        await sourceFile.CopyAsync(targetFolder, "background_current.png", NameCollisionOption.ReplaceExisting);
        //        mainGrid.Background = new ImageBrush()
        //        {
        //            ImageSource = new BitmapImage(new Uri("ms-appdata:///local/background_current.png"))
        //        };
        //        ChangingRing.IsActive = false;

        //    }
        //    catch
        //    {
        //        ChangingRing.IsActive = false;
        //        ContentDialog ShowDialog = new ContentDialog
        //        {
        //            Title = "设置主题",
        //            Content = "设置主题失败，请稍后再试",
        //            CloseButtonText = "好的"
        //        };
        //        await ShowDialog.ShowAsync();
        //    }

       
    }
}
