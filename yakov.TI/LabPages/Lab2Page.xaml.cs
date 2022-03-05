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
using System.Windows.Navigation;
using System.Windows.Shapes;
using yakov.TI.VM;

namespace yakov.TI.LabPages
{
    /// <summary>
    /// Interaction logic for Lab2Page.xaml
    /// </summary>
    public partial class Lab2Page : Page
    {
        public Lab2Page()
        {
            InitializeComponent();
            DataContext = new Lab2Context();
        }
    }
}
