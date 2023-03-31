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

namespace KASUMI
{
    /// <summary>
    /// Логика взаимодействия для algorithm.xaml
    /// </summary>
    public partial class algorithm : Window
    {
        public algorithm()
        {
            InitializeComponent();
            buttonBack.Visibility = Visibility.Hidden;
            canvas3.Visibility = Visibility.Hidden;
            image2.Visibility = Visibility.Hidden;
            image3.Visibility = Visibility.Hidden;
            image4.Visibility = Visibility.Hidden;
            canvas3.Visibility = Visibility.Hidden;
            canvas4.Visibility = Visibility.Hidden;
            canvas5.Visibility = Visibility.Hidden;
            label5.Visibility = Visibility.Hidden;
            label6.Visibility = Visibility.Hidden;
            label7.Visibility = Visibility.Hidden;
            label8.Visibility = Visibility.Hidden;
            label9.Visibility = Visibility.Hidden;
            label10.Visibility = Visibility.Hidden;
        }
        private void FL_Click(object sender, RoutedEventArgs e)
        {
            label1.Content = "Операция FL";
            label2.Visibility = Visibility.Hidden;
            label3.Visibility = Visibility.Hidden;
            label4.Visibility = Visibility.Hidden;
            label9.Visibility = Visibility.Visible;
            label10.Visibility = Visibility.Visible;
            image1.Visibility = Visibility.Hidden;
            image4.Visibility = Visibility.Visible;
            canvas1.Visibility = Visibility.Hidden;
            canvas2.Visibility = Visibility.Hidden;
            canvas4.Visibility = Visibility.Visible;
            canvas5.Visibility = Visibility.Visible;
            buttonFL.Visibility = Visibility.Hidden;
            buttonFO.Visibility = Visibility.Hidden;
            buttonFI.Visibility = Visibility.Hidden;
            buttonBack.Visibility = Visibility.Visible;
        }
        private void FO_Click(object sender, RoutedEventArgs e)
        {
            label1.Content = "Операция FO";
            label2.Visibility = Visibility.Hidden;
            label3.Visibility = Visibility.Hidden;
            label4.Visibility = Visibility.Hidden;
            label5.Visibility = Visibility.Visible;
            label6.Visibility = Visibility.Visible;
            image1.Visibility = Visibility.Hidden;
            image2.Visibility = Visibility.Visible;
            canvas1.Visibility = Visibility.Hidden;
            canvas2.Visibility = Visibility.Hidden;
            canvas3.Visibility = Visibility.Visible;
            buttonFL.Visibility = Visibility.Hidden;
            buttonFO.Visibility = Visibility.Hidden;
            buttonFI.Visibility = Visibility.Hidden;
            buttonBack.Visibility = Visibility.Visible;
        }
        private void FI_Click(object sender, RoutedEventArgs e)
        {
            label1.Content = "Операция FI";
            label2.Visibility = Visibility.Hidden;
            label3.Visibility = Visibility.Hidden;
            label4.Visibility = Visibility.Hidden;
            label7.Visibility = Visibility.Visible;
            label8.Visibility = Visibility.Visible;
            image1.Visibility = Visibility.Hidden;
            image3.Visibility = Visibility.Visible;
            canvas1.Visibility = Visibility.Hidden;
            canvas2.Visibility = Visibility.Hidden;
            canvas3.Visibility = Visibility.Visible;
            buttonFL.Visibility = Visibility.Hidden;
            buttonFO.Visibility = Visibility.Hidden;
            buttonFI.Visibility = Visibility.Hidden;
            buttonBack.Visibility = Visibility.Visible;
        }
        private void back_Click(object swnder, RoutedEventArgs e)
        {
            label1.Content = "Алгоритм KASUMI";
            label2.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Visible;
            label4.Visibility = Visibility.Visible;
            label5.Visibility = Visibility.Hidden;
            label6.Visibility = Visibility.Hidden;
            label7.Visibility = Visibility.Hidden;
            label8.Visibility = Visibility.Hidden;
            label9.Visibility = Visibility.Hidden;
            label10.Visibility = Visibility.Hidden;
            image1.Visibility = Visibility.Visible;
            image2.Visibility = Visibility.Hidden;
            image3.Visibility = Visibility.Hidden;
            image4.Visibility = Visibility.Hidden;
            canvas1.Visibility = Visibility.Visible;
            canvas2.Visibility = Visibility.Visible;
            canvas3.Visibility = Visibility.Hidden;
            canvas4.Visibility = Visibility.Hidden;
            canvas5.Visibility = Visibility.Hidden;
            buttonFL.Visibility = Visibility.Visible;
            buttonFO.Visibility = Visibility.Visible;
            buttonFI.Visibility = Visibility.Visible;
            buttonBack.Visibility = Visibility.Hidden;
        }
    }
}
