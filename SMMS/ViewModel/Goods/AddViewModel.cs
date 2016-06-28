using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SMMS.Services;
using System.ComponentModel;

namespace SMMS.ViewModel.Goods
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AddViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly IModernNavigationService _modernNavigationService;

        public AddViewModel(IModernNavigationService modernNavigationService)
        {
            _modernNavigationService = modernNavigationService;
        }



        public RelayCommand AddCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        int GID = DBHelper.addGoods(GNAME, PRICE, CATEGORY, UNIT, CODE);
                        ModernDialog.ShowMessage("进货成功！货号：" + GID.ToString() + "  商品名：" + GNAME, "成功", System.Windows.MessageBoxButton.OK);
                        CODE = UNIT = CATEGORY = PRICE = GNAME = "";
                    }
                    catch
                    {
                        ModernDialog.ShowMessage("进货失败！请检查商品信息是否正确。", "失败", System.Windows.MessageBoxButton.OK);
                    }
                });


            }
        }



        private string code;
        public string CODE
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                RaisePropertyChanged("CODE");
            }
        }

        private string unit;
        public string UNIT
        {
            get
            {
                return unit;
            }

            set
            {
                unit = value;
                RaisePropertyChanged("UNIT");
            }
        }


        private string category;
        public string CATEGORY
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
                RaisePropertyChanged("CATEGORY");
            }
        }

        private string price;
        public string PRICE
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
                RaisePropertyChanged("PRICE");
            }
        }

        private string gname;
        public string GNAME
        {
            get
            {
                return gname;
            }

            set
            {
                gname = value;
                RaisePropertyChanged("GNAME");
            }
        }
        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "GNAME")
                {
                    return string.IsNullOrEmpty(this.GNAME) ? "必填" : null;
                }
                if (columnName == "PRICE")
                {
                    return string.IsNullOrEmpty(this.PRICE) ? "必填" : null;
                }
                return null;
            }
        }
    }
}