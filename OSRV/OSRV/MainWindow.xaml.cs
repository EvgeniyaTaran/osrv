using OSRV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace OSRV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Robot R1 = new Robot(Robot1);
            Robot R2 = new Robot(Robot2);
            R1.Move("R1", 10);
            R2.Move("R2", 15);
            R1.Rotate("R1", 90);
            R1.Move("R1", 2);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
