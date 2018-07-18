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
    public sealed partial class TablePage : Page
    {
        public TablePage()
        {
            this.InitializeComponent();
            DrawLess();


        }

        private void DrawMore()
        {
            RootTable.Children.Clear();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBlock tb = new TextBlock
                    {
                        Margin = new Thickness(2),
                        Text = (j + 1) + " × " + (i + 1) + " = " + (j + 1) * (i + 1),
                        FontSize = 20
                    };

                    Border border = new Border
                    {
                        BorderThickness = new Thickness(0, 2, 2, 0),
                        BorderBrush = new SolidColorBrush(Colors.Black)
                    };

                    if (i == 8)
                    {
                        border.BorderThickness = new Thickness(0, 2, 2, 2);
                    }
                    else if (j == 0)
                    {
                        border.BorderThickness = new Thickness(2, 2, 2, 0);
                    }
                    if (i == 8 && j == 0)
                    {
                        border.BorderThickness = new Thickness(2, 2, 2, 2);
                    }
                    border.Child = tb;

                    Grid.SetRow(border, i + 1);
                    Grid.SetColumn(border, j + 1);
                    RootTable.Children.Add(border);
                }
            }
        }

        private void DrawLess()
        {
            RootTable.Children.Clear();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    TextBlock tb = new TextBlock
                    {
                        Margin = new Thickness(2),
                        Text = (j + 1) + " × " + (i + 1) + " = " + (j + 1) * (i + 1),
                        FontSize = 20
                    };

                    Border border = new Border
                    {
                        BorderThickness = new Thickness(0, 2, 2, 0),
                        BorderBrush = new SolidColorBrush(Colors.Black)
                    };

                    if (i == 8)
                    {
                        border.BorderThickness = new Thickness(0, 2, 2, 2);
                    }
                    else if (j == 0)
                    {
                        border.BorderThickness = new Thickness(2, 2, 2, 0);
                    }
                    if (i == 8 && j == 0)
                    {
                        border.BorderThickness = new Thickness(2, 2, 2, 2);
                    }
                    border.Child = tb;

                    Grid.SetRow(border, i + 1);
                    Grid.SetColumn(border, j + 1);
                    RootTable.Children.Add(border);
                }
            }
        }

        private void MoreSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (MoreSwitch.IsOn)
            {
                DrawMore();
            }
            else
            {
                DrawLess();
            }
        }

        private void BackButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
