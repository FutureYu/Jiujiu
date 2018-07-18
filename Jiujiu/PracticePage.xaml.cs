using System;
using System.Collections.Generic;
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
    /// 

    public sealed partial class PracticePage : Page
    {
        TotalData totalData = new TotalData();
        int result = 0;
        int correctNumber = 0;
        int totalNumber = 0;
        bool isOneTimeTrue = true; // 用来记录该题是否一次作对
        AchievementData achievementData = new AchievementData();

        private void JudgeAchievement()
        {

            if (achievementData.A10000q == false)
            {
                if (totalData.TotalQuestionCount + totalNumber >= 10000)
                {
                    // toast
                    HomePage.MakeToast("完成10000道题");
                    achievementData.A10000q = true;
                }
            }
            if (achievementData.A1000q == false)
            {
                if (totalData.TotalQuestionCount + totalNumber >= 1000)
                {
                    // toast
                    HomePage.MakeToast("完成1000道题");
                    achievementData.A1000q = true;
                }
            }
            if (achievementData.A100q == false)
            {
                if (totalData.TotalQuestionCount + totalNumber >= 100)
                {
                    // toast
                    HomePage.MakeToast("完成100道题");
                    achievementData.A100q = true;
                }
            }

        }


        public PracticePage()
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

        private void RefreashData()
        {
            TotalBlock.Text = "总题数：" + totalNumber;
            CorrectBlock.Text = "正确数：" + correctNumber;
            PercentageBlock.Text = "正确率：" + ((int)(((double)correctNumber / totalNumber) * 10000) / 100.00) + "%";
        }

        private void JudgeResult()
        {
            if (this.result.ToString() == ResultBlock.Text)
            {
                Correct();
                JudgeAchievement();
            }
            else if (ResultBlock.Text.Length == result.ToString().Length)
            {
                Wrong();
            }

        }

        private void Correct()
        {
            StatusBorder.Background = new SolidColorBrush(Colors.Green);
            StatusBlock.Text = QuestionBlock.Text + ResultBlock.Text + "    结果正确！";
            ResultBlock.Text = "";
            if (isOneTimeTrue)
            {
                correctNumber++;
            }
            RefreashData();
            CreateQuestion();
        }

        private void Wrong()
        {
            StatusBorder.Background = new SolidColorBrush(Colors.Red);
            StatusBlock.Text = QuestionBlock.Text + ResultBlock.Text + "    结果错误！";
            ResultBlock.Text = "";
            isOneTimeTrue = false;
            RefreashData();
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
            this.isOneTimeTrue = true;
            TipBlock.Text = "答案：" + result;
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }


        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            totalData.TotalCorrectCount += correctNumber;
            totalData.TotalQuestionCount += totalNumber - 1;
            await totalData.WriteTotalDataAsync();
            await achievementData.WriteAchievementDataAsync();
        }
    }
}
