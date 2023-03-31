using Microsoft.Win32;
using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace KASUMI
{
    /// <summary>
    /// Логика взаимодействия для message.xaml
    /// </summary>
    public partial class message : Window
    {
        public message()
        {
            InitializeComponent();
        }
        private void close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void showMessage(string text)
        {
            messageText.Content = text;
        }
    }
}
