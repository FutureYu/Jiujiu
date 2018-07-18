using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Jiujiu
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        public static void MakeToast(string str)
        {
            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                {
                       new AdaptiveText()
                       {
                         Text = "九九乘法训练——达成成就！"
                       },

                       new AdaptiveText()
                       {
                            Text = String.Format("恭喜！您已达成{0}的成就！",str)
                       },


                },

                    AppLogoOverride = new ToastGenericAppLogo()
                    {
                        Source = "/Assets/logo2.png",
                        HintCrop = ToastGenericAppLogoCrop.Circle
                    }
                }
            };

            // In a real app, these would be initialized with actual data
            int conversationId = 384928;

            // Construct the actions for the toast (inputs and buttons)
            ToastActionsCustom actions = new ToastActionsCustom()
            {

            };

            // Now we can construct the final toast content
            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
                Actions = actions,

                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    { "action", "viewConversation" },
                    { "conversationId", conversationId.ToString() }

                }.ToString()
            };

            // And create the toast notification
            var toast = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        TotalData totalData = new TotalData();
        AchievementData achievementData = new AchievementData();
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await LoginAsync();
        }

        private async System.Threading.Tasks.Task JudgeAchievementAsync(bool isTodayContinuous)
        {

            await achievementData.ReadAchievementDataAsync();
            if (achievementData.A30d == false)
            {
                if (totalData.ContinuousCount + (isTodayContinuous ? 1 : 0) >= 30)
                {
                    // toast
                    MakeToast("连续登录30天");
                    achievementData.A30d = true;
                }
            }
            if (achievementData.A14d == false)
            {
                if (totalData.ContinuousCount + (isTodayContinuous ? 1 : 0) >= 14)
                {
                    // toast
                    MakeToast("连续登录14天");
                    achievementData.A14d = true;
                }
            }
            if (achievementData.A7d == false)
            {
                if (totalData.ContinuousCount + (isTodayContinuous ? 1 : 0) >= 7)
                {
                    // toast
                    MakeToast("连续登录7天");
                    achievementData.A7d = true;
                }
            }

        }


        private async System.Threading.Tasks.Task LoginAsync()
        {
            bool isTodayContinuous = false;
            bool isTotalDataChanged = false;
            await totalData.ReadTotalDataAsync();
            if (totalData.ThisLoginDate.Date != totalData.LastLoginDate.Date)
            {
                totalData.LastLoginDate = totalData.ThisLoginDate;
                totalData.ThisLoginDate = DateTime.Now;
                isTotalDataChanged = true;
            }

            if (totalData.LastLoginDate.AddDays(1).Date == totalData.ThisLoginDate.Date)
            {
                totalData.TotalLoginCount++;
                totalData.ContinuousCount++;
                isTodayContinuous = true;
                isTotalDataChanged = true;
            }

            await JudgeAchievementAsync(isTodayContinuous);
            await achievementData.WriteAchievementDataAsync();
            if (isTotalDataChanged)
            {
                await totalData.WriteTotalDataAsync();
            }
        }


        private void PracticeBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PracticePage));
        }

        private void TableBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TablePage));
        }

        private void GameBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage));
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void PersonBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PersonPage));
        }
    }
}
