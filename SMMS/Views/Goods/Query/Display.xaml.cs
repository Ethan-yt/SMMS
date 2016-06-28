using FirstFloor.ModernUI.Windows;
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
using FirstFloor.ModernUI.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;

namespace SMMS.Views.Goods.Query
{
    /// <summary>
    /// Display.xaml 的交互逻辑
    /// </summary>
    public partial class Display
    {
        public Display()
        {
            InitializeComponent();
        }

        private void ModernUserControl_NavigatingFrom(object sender, FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            Messenger.Default.Send(new object[] { "NavigatingFromQueryPage", e});
        }
    }
}
