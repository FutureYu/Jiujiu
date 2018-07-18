using Microsoft.Advertising.Shared.WinRT;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Jiujiu
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PersonPage : Page
    {
        TotalData totalData = new TotalData();
        AchievementData achievementData = new AchievementData();
        public PersonPage()
        {
            this.InitializeComponent();
            
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await GetAchievementRecordListAsync();
            await GetTotalRecordListAsync();
            await GetGameRecordListAsync();
        }

        public async System.Threading.Tasks.Task GetAchievementRecordListAsync()
        {
            await achievementData.ReadAchievementDataAsync();

            AchievementList.Items.Add(String.Format("完成100道题：\t\t\t{0}", achievementData.A100q == false ? "未完成" : "已完成"));
            AchievementList.Items.Add(String.Format("完成1000道题：\t\t\t{0}", achievementData.A1000q == false ? "未完成" : "已完成"));
            AchievementList.Items.Add(String.Format("完成10000道题：\t\t{0}", achievementData.A10000q == false ? "未完成" : "已完成"));
            AchievementList.Items.Add(String.Format("30秒内比赛全对：\t\t{0}", achievementData.A30s == false ? "未完成" : "已完成"));
            AchievementList.Items.Add(String.Format("40秒内比赛全对：\t\t{0}", achievementData.A40s == false ? "未完成" : "已完成"));
            AchievementList.Items.Add(String.Format("50秒内比赛全对：\t\t{0}", achievementData.A50s == false ? "未完成" : "已完成"));
            AchievementList.Items.Add(String.Format("连续登录7天：\t\t\t{0}", achievementData.A7d == false ? "未完成" : "已完成"));
            AchievementList.Items.Add(String.Format("连续登录14天：\t\t\t{0}", achievementData.A14d == false ? "未完成" : "已完成"));
            AchievementList.Items.Add(String.Format("连续登录30天：\t\t\t{0}", achievementData.A30d == false ? "未完成" : "已完成"));
            AchievementList.Items.Add(String.Format("为我们评分：\t\t\t{0}", achievementData.Arate == false ? "未完成" : "已完成"));

        }

        private async void ClearAchievementRecordBtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            ClearAchievementRecordBtn.IsEnabled = false;
            await achievementData.DeleteAchievementDataAsync();
            AchievementList.Items.Clear();
            await GetAchievementRecordListAsync();
        }



        // 总数据
        public async System.Threading.Tasks.Task GetTotalRecordListAsync()
        {
            await totalData.ReadTotalDataAsync();

            TotalList.Items.Add(String.Format("上次登录时间：\t{0}", totalData.LastLoginDate));
            TotalList.Items.Add(String.Format("总登录数：\t{0}", totalData.TotalLoginCount));
            TotalList.Items.Add(String.Format("连续登录天数：\t{0}", totalData.ContinuousCount));
            TotalList.Items.Add(String.Format("总比赛数：\t{0}", totalData.TotalGameCount));
            TotalList.Items.Add(String.Format("总题数：\t\t{0}", totalData.TotalQuestionCount));
            TotalList.Items.Add(String.Format("总正确数：\t{0}", totalData.TotalCorrectCount));
            TotalList.Items.Add(String.Format("平均分数：\t{0}", totalData.AverageScore));
            TotalList.Items.Add(String.Format("总游戏时间：\t{0}", totalData.TotalGameTime));
        }

        private async void ClearTotalRecordBtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            ClearTotalRecordBtn.IsEnabled = false;
            await totalData.DeleteTotalDataAsync();
            TotalList.Items.Clear();
            await GetTotalRecordListAsync();
        }


        // 比赛模式
        public async System.Threading.Tasks.Task GetGameRecordListAsync()
        {
            GameData[] gameDatas = await GameData.ReadGameDataAsync();
            if (gameDatas == null)
            {
                ClearGameRecordBtn.IsEnabled = false;
                GameList.Items.Add("还没有使用过比赛模式，快去试试吧~");
                return;
            }
            foreach (var gameData in gameDatas)
            {

                GameList.Items.Add(String.Format("时间：{0}\t\t正确数：{1} \t用时：{2}秒 \t得分：{3}", gameData.Date, gameData.CorrectNumber, gameData.Time, gameData.Score));
            }
            if (GameList.Items.Count == 0)
            {
                ClearGameRecordBtn.IsEnabled = false;
                GameList.Items.Add("还没有使用过比赛模式，快去试试吧~");
            }
        }

        private async void ClearGameRecordBtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            ClearGameRecordBtn.IsEnabled = false;
            await GameData.DeleteGameDataAsync();
            GameList.Items.Clear();
            GameList.Items.Add("还没有使用过比赛模式，快去试试吧~");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void OnErrorOccurred(object sender, AdErrorEventArgs e)
        {
             GameList.Items.Add("~");
        }

        private void OnAdRefreshed(object sender, RoutedEventArgs e)
        {
            // We increment the ad count so that the message changes at every refresh.

            GameList.Items.Add("~~");
        }
    }
}
