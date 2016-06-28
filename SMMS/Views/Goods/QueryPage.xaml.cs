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

namespace SMMS.Views.Goods
{
    /// <summary>
    /// Query.xaml 的交互逻辑
    /// </summary>
    public partial class QueryPage
    {
        public QueryPage()
        {
            InitializeComponent();
            if (!(DBHelper.currentUser.Group.EDITGOODS || DBHelper.currentUser.Group.RESTOCKGOODS))
                Tab.Links.Remove(add);
            Messenger.Default.Register<object[]>(this, p =>
            {
                if (p[0] as string == "NavigateToDisplay")
                {
                    Tab.SelectedSource = new Uri("/Views/Goods/Query/Display.xaml", UriKind.RelativeOrAbsolute);
                }
            });
        }

        private void ModernUserControl_NavigatedTo(object sender, FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {

            Messenger.Default.Send(new object[] { "NavigatedToQueryPage" });
        }

        private void ModernUserControl_NavigatingFrom(object sender, FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            Messenger.Default.Send(new object[] { "NavigatingFromQueryPage",e });
        }

    }
    
}
