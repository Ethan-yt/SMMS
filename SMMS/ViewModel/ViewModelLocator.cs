/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:SMMS"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace SMMS.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public const string GoodsDisplayPageKey = "GoodsDisplayPage";
        public const string MainPageKey = "MainPage";
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<Goods.QueryViewModel>();
            SimpleIoc.Default.Register<Goods.DisplayViewModel>();
            SimpleIoc.Default.Register<Goods.RestockViewModel>();
            SimpleIoc.Default.Register<Goods.AddViewModel>();
            SimpleIoc.Default.Register<Goods.SaleViewModel>();
            SimpleIoc.Default.Register<Goods.SummaryViewModel>();
            SimpleIoc.Default.Register<Personnel.UserViewModel>();
            SimpleIoc.Default.Register<Personnel.GroupViewModel>();
            SimpleIoc.Default.Register<LogViewModel>();
            SimpleIoc.Default.Register<UserDataViewModel>();
        }
        public UserDataViewModel UserDataViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserDataViewModel>();
            }
        }
        public LogViewModel LogViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LogViewModel>();
            }
        }
        public Personnel.GroupViewModel PersonnelGroupViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Personnel.GroupViewModel>();
            }
        }
        public Personnel.UserViewModel PersonnelUserViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Personnel.UserViewModel>();
            }
        }
        public Goods.RestockViewModel GoodsRestockViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Goods.RestockViewModel>();
            }
        }
        public Goods.SaleViewModel GoodsSaleViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Goods.SaleViewModel>();
            }
        }
        public Goods.AddViewModel GoodsAddViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Goods.AddViewModel>();
            }
        }
        public Goods.SummaryViewModel GoodsSummaryViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Goods.SummaryViewModel>();
            }
        }
        public LoginViewModel LoginViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }
        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public Goods.QueryViewModel GoodsQueryViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Goods.QueryViewModel>();
            }
        }
        public Goods.DisplayViewModel GoodsDisplayViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Goods.DisplayViewModel>();
            }
        }
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}