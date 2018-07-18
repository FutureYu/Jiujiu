using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class GamePage : Page
    {
        int result = 0;
        int correctNumber = 0;
        int totalNumber = 0; // 总题数 
        int usedTime = 0;
        int score = 0;
        bool isNeedWriteAchi = false;
        GameData gameData = new GameData();
        TotalData totalData = new TotalData();
        Stopwatch stopwatch = new Stopwatch();
        AchievementData achievementData = new AchievementData();


        public GamePage()
        {
            this.InitializeComponent();
            CreateQuestion();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await GetInfoAsync();
        }

        private async System.Threading.Tasks.Task GetInfoAsync()
        {
            await totalData.ReadTotalDataAsync();
            await achievementData.ReadAchievementDataAsync();
        }

        private void JudgeTotalAchievement()
        {

            if (achievementData.A10000q == false)
            {
                if (totalData.TotalQuestionCount + totalNumber >= 10000)
                {
                    // toast
                    HomePage.MakeToast("完成10000道题");
                    achievementData.A10000q = true;
                    isNeedWriteAchi = true;
                }
            }
            if (achievementData.A1000q == false)
            {
                if (totalData.TotalQuestionCount + totalNumber >= 1000)
                {
                    // toast
                    HomePage.MakeToast("完成1000道题");
                    achievementData.A1000q = true;
                    isNeedWriteAchi = true;
                }
            }
            if (achievementData.A100q == false)
            {
                if (totalData.TotalQuestionCount + totalNumber >= 100)
                {
                    // toast
                    HomePage.MakeToast("完成100道题");
                    achievementData.A100q = true;
                    isNeedWriteAchi = true;
                }
            }
        }

        private void JudgeGameAchievement()
        {

            if (achievementData.A30s == false)
            {
                if (usedTime <= 30)
                {
                    // toast
                    HomePage.MakeToast("30秒内比赛全对");
                    achievementData.A30s = true;
                }
            }
            if (achievementData.A40s == false)
            {
                if (usedTime <= 40)
                {
                    // toast
                    HomePage.MakeToast("40秒内比赛全对");
                    achievementData.A40s = true;
                }
            }
            if (achievementData.A50s == false)
            {
                if (usedTime <= 50)
                {
                    // toast
                    HomePage.MakeToast("50秒内比赛全对");
                    achievementData.A50s = true;
                }
            }
        }

        private async void ShowData()
        {
            usedTime = (int)stopwatch.Elapsed.TotalSeconds;

            score = (int)((correctNumber * 3.4) - ((usedTime - 30) > 0 ? (usedTime - 30) : 0) * 1.0);
            score = score > 100 ? 100 : score;
            score = score < 0 ? 0 : score;
            ContentDialog ShowDataDialog = new ContentDialog
            {
                Title = "比赛结束",
                Content = String.Format("您本次共答对{0}道题，正确率为{1}%，用时为{2}秒，最终得分是{3}分。继续努力呦~", correctNumber, ((int)(((double)correctNumber / totalNumber) * 10000) / 100.00), usedTime, score),
                CloseButtonText = "好的",
            };

            ContentDialogResult result = await ShowDataDialog.ShowAsync();
            if (result == ContentDialogResult.None)
            {
                // 保存此次数据与总数据，返回主界面
                gameData.Score = score;
                gameData.Time = usedTime;
                gameData.CorrectNumber = correctNumber;
                if (correctNumber == 30)
                {
                    JudgeGameAchievement();
                }
                await gameData.AppendGameDataAsync();
                Frame.GoBack();
            }
        }

        private void JudgeResult()
        {
            if (this.result.ToString() == ResultBlock.Text)
            {
                correctNumber++;
                if (totalNumber == 30)
                {
                    stopwatch.Stop();
                    ShowData();
                    return;
                }
                JudgeTotalAchievement();
                CreateQuestion();
                ResultBlock.Text = "";

            }
            else if (ResultBlock.Text.Length == result.ToString().Length)
            {
                if (totalNumber == 30)
                {
                    stopwatch.Stop();
                    ShowData();
                    return;
                }
                JudgeTotalAchievement();
                CreateQuestion();
                ResultBlock.Text = "";
            }

        }



        private void CreateQuestion()
        {
            Random r = new Random();
            int firstNumber = 0;
            int secondNumber = 0;
            firstNumber = r.Next(1, 10);
            secondNumber = r.Next(1, 10);
            QuestionBlock.Text = firstNumber + " × " + secondNumber + " = ";
            this.result = firstNumber * secondNumber;
            this.totalNumber++;
            StatusBlock.Text = "( " + totalNumber + " / 30 )";
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultBlock.Text = "";
        }

        private void OneBtn_Click(object sender, RoutedEventArgs e)
        {

            ResultBlock.Text += "1";
            JudgeResult();
        }

        private void TwoBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultBlock.Text += "2";
            JudgeResult();
        }

        private void ThreeBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultBlock.Text += "3";
            JudgeResult();
        }

        private void FourBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultBlock.Text += "4";
            JudgeResult();
        }

        private void FiveBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultBlock.Text += "5";
            JudgeResult();
        }

        private void SixBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultBlock.Text += "6";
            JudgeResult();
        }

        private void SevenBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultBlock.Text += "7";
            JudgeResult();
        }

        private void EightBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultBlock.Text += "8";
            JudgeResult();
        }

        private void NineBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultBlock.Text += "9";
            JudgeResult();
        }

        private void ZeroBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ResultBlock.Text != "")
            {
                ResultBlock.Text += "0";
            }
            JudgeResult();
        }


        private void BackspaceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (0 <= ResultBlock.Text.Length - 1)
            {
                ResultBlock.Text = ResultBlock.Text.Substring(0, ResultBlock.Text.Length - 1);
            }
        }

        private void SkipBtn_Click(object sender, RoutedEventArgs e)
        {
            if (totalNumber == 30)
            {
                stopwatch.Stop();
                ShowData();
                return;
            }
            CreateQuestion();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            ReadyGrid.Visibility = Visibility.Collapsed;
            GameGrid.Visibility = Visibility.Visible;
            gameData.Date = DateTime.Now;
            stopwatch.Start();
        }

        private async void BackButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (totalNumber < 30 && totalNumber > 1)
            {
                stopwatch.Stop();
                ContentDialog ShowDataDialog = new ContentDialog
                {
                    Title = "结束比赛",
                    Content = "比赛尚未完成，是否要退出，此时退出将不会保存记录",
                    CloseButtonText = "继续比赛",
                    PrimaryButtonText = "退出比赛"
                };
                ContentDialogResult result = await ShowDataDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    // 退出比赛，只存储总数据
                    Frame.GoBack();
                }
                else
                {
                    // 继续比赛
                    stopwatch.Start();
                }
            }
            else
            {
                // 返回主界面
                Frame.GoBack();
            }
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)

        {
            totalData.AverageScore = (int)((totalData.AverageScore * totalData.TotalGameCount + score) * 1.00 / ++totalData.TotalGameCount);
            totalData.TotalCorrectCount += correctNumber;
            totalData.TotalGameTime += usedTime;
            totalData.TotalQuestionCount += totalNumber;
            await totalData.WriteTotalDataAsync();
            if (isNeedWriteAchi)
            {
                await achievementData.WriteAchievementDataAsync();
            }
        }

    }
}
