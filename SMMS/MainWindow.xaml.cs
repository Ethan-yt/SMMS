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
using FirstFloor.ModernUI.Windows.Controls;
using System.Data.SQLite;
using GalaSoft.MvvmLight.Messaging;
using FirstFloor.ModernUI.Presentation;
using SMMS.Services;
using SMMS.ViewModel;
using GalaSoft.MvvmLight.Ioc;
namespace SMMS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DBHelper.initDB();
            SetupNavigation();
   
            //AppearanceManager.Current.AccentColor = Colors.Green;
            Messenger.Default.Register<object[]>(this, p =>
            {
                if (p[0] as string == "Login")
                {
                    setupMenu();
                }
            });
        }

        private void SetupNavigation()
        {
            var navigationService = new NavigationService();
            navigationService.Configure(ViewModelLocator.MainPageKey, new Uri("/Views/MainPage.xaml", UriKind.Relative));
            navigationService.Configure(ViewModelLocator.GoodsDisplayPageKey, new Uri("/Views/Goods/Query/Display.xaml", UriKind.Relative));
            SimpleIoc.Default.Register<IModernNavigationService>(() => navigationService);
        }

        private void setupMenu()
        {
            for(int i = 0;i< MenuLinkGroups.Count;i++)
            {
                if (MenuLinkGroups[i].GroupKey == "main")
                    MenuLinkGroups.RemoveAt(i--);

            }

            LinkGroup lkgp = new LinkGroup();
            lkgp.DisplayName = "首页";
            lkgp.GroupKey = "main";
            lkgp.Links.Add(new Link { DisplayName = "欢迎页", Source = new Uri("/Views/MainPage.xaml", UriKind.RelativeOrAbsolute) });
            lkgp.Links.Add(new Link { DisplayName = "修改资料", Source = new Uri("/Views/UserData.xaml", UriKind.RelativeOrAbsolute) });
            MenuLinkGroups.Add(lkgp);

            if (DBHelper.currentUser.Group.SALEGOODS || DBHelper.currentUser.Group.RESTOCKGOODS || DBHelper.currentUser.Group.QUERYGOODS || DBHelper.currentUser.Group.EDITGOODS)
            {
                lkgp = new LinkGroup();
                lkgp.DisplayName = "商品管理";
                lkgp.GroupKey = "main";
                lkgp.Links.Add(new Link { DisplayName = "库存概况", Source = new Uri("/Views/Goods/Summary.xaml", UriKind.RelativeOrAbsolute) });
                lkgp.Links.Add(new Link { DisplayName = DBHelper.currentUser.Group.EDITGOODS ? "商品编辑" : "商品查询", Source = new Uri("/Views/Goods/QueryPage.xaml", UriKind.RelativeOrAbsolute) });
                if (DBHelper.currentUser.Group.RESTOCKGOODS)
                    lkgp.Links.Add(new Link { DisplayName = "进货", Source = new Uri("/Views/Goods/Restock.xaml", UriKind.RelativeOrAbsolute) });
                if (DBHelper.currentUser.Group.SALEGOODS)
                    lkgp.Links.Add(new Link { DisplayName = "销售", Source = new Uri("/Views/Goods/Sale.xaml", UriKind.RelativeOrAbsolute) });
                MenuLinkGroups.Add(lkgp);
            }
            if (DBHelper.currentUser.Group.EDITPERSONNEL || DBHelper.currentUser.Group.EDITGROUP)
            {
                lkgp = new LinkGroup();
                lkgp.DisplayName = "人事管理";
                lkgp.GroupKey = "main";
                if (DBHelper.currentUser.Group.EDITPERSONNEL)
                    lkgp.Links.Add(new Link { DisplayName = "人事管理", Source = new Uri("/Views/Personnel/User.xaml", UriKind.RelativeOrAbsolute) });
                if (DBHelper.currentUser.Group.EDITGROUP)
                    lkgp.Links.Add(new Link { DisplayName = "职位管理", Source = new Uri("/Views/Personnel/Group.xaml", UriKind.RelativeOrAbsolute) });
                MenuLinkGroups.Add(lkgp);
            }
            if (DBHelper.currentUser.Group.EDITLOG || DBHelper.currentUser.Group.QUERYLOG)
            {
                lkgp = new LinkGroup();
                lkgp.DisplayName = "操作日志";
                lkgp.GroupKey = "main";
                lkgp.Links.Add(new Link { DisplayName = "日志", Source = new Uri("/Views/Log.xaml", UriKind.RelativeOrAbsolute) });
                MenuLinkGroups.Add(lkgp);
            }
        }
    }
}
