using GalaSoft.MvvmLight.Messaging;
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

namespace SMMS.Views.Personnel
{
    /// <summary>
    /// Query.xaml 的交互逻辑
    /// </summary>
    public partial class Group
    {
        public Group()
        {
            InitializeComponent();
        }


        private void ModernUserControl_NavigatedTo(object sender, FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            Messenger.Default.Send(new object[] { "NavigatedToGroup" });
        }

        private void ModernUserControl_NavigatingFrom(object sender, FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            Messenger.Default.Send(new object[] { "NavigatingFromGroup", e });
        }
    }
}
