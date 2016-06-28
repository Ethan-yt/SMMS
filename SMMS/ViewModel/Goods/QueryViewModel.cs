using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using SMMS.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SMMS.ViewModel.Goods
{
    public class QueryViewModel : ViewModelBase
    {
        private readonly IModernNavigationService _modernNavigationService;

        public QueryViewModel(IModernNavigationService modernNavigationService)
        {
            _modernNavigationService = modernNavigationService;
            SelectedItem = new Category() { ID = 0, Name = "(不限)" };
            NavigatedToCommand.Execute(null);

        }


        /// <summary>
        /// 
        /// </summary>
        public ICommand QueryCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string where = "";
                    try
                    {
                        if (!string.IsNullOrEmpty(goodsname))
                            where += "GNAME LIKE '%" + goodsname + "%'";
                        if (!string.IsNullOrEmpty(gid))
                        {
                            if (where != "")
                                where += " AND ";
                            where += "GID = '" + gid + "'";;
                        }
                        if (!string.IsNullOrEmpty(code))
                        {
                            if (where != "")
                                where += " AND ";
                            where += "CODE = '" + code + "'"; ;
                        }
                        if (!string.IsNullOrEmpty(lowerPrice))
                        {
                            if (where != "")
                                where += " AND ";
                            where += "PRICE >=" + float.Parse(lowerPrice);
                        }
                        if (!string.IsNullOrEmpty(upperPrice))
                        {
                            if (where != "")
                                where += " AND ";
                            where += "PRICE <=" + float.Parse(upperPrice);
                        }
                        if (!string.IsNullOrEmpty(lowerNum))
                        {
                            if (where != "")
                                where += " AND ";
                            where += "NUM >=" + float.Parse(lowerNum);
                        }
                        if (!string.IsNullOrEmpty(upperNum))
                        {
                            if (where != "")
                                where += " AND ";
                            where += "NUM <=" + float.Parse(upperNum);
                        }

                        if (SelectedItem.Name != "(不限)")
                        {
                            if (where != "")
                                where += " AND ";
                            where += "CATEGORY = '" + SelectedItem.Name + "'";
                        }
                        var d = new Dictionary<string, object>();
                        d["GoodsWhere"] = where;
                        _modernNavigationService.Parameter = d;
                        Messenger.Default.Send(new object[] { "NavigateToDisplay" });
                    }
                    catch (System.Exception)
                    {
                        ModernDialog.ShowMessage("查询失败，请检查关键字是否符合格式。", "失败", System.Windows.MessageBoxButton.OK);
                    }
                });
            }
        }
        public RelayCommand NavigatedToCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    itemsSource = new ObservableCollection<Category>() { new Category() { ID = 0, Name = "(不限)" }};
                    string[] categorys = DBHelper.getCategorys();
                    for (int i = 0; i < categorys.Length; i++)
                        itemsSource.Add(new Category() { ID = i + 1, Name = categorys[i] });

                    foreach (var i in itemsSource)
                        if (i.Name == SelectedItem.Name)
                            SelectedItem = i;
                    RaisePropertyChanged("ItemsSource");

                });
            }
        }

        public string gid { get; set; }
        public string code { get; set; }

        public string goodsname { get; set; }

        public string lowerPrice { get; set; }

        public string upperPrice { get; set; }

        public string lowerNum { get; set; }

        public string upperNum { get; set; }

        public class Category
        {
            public int ID { set; get; }
            public string Name { set; get; }
        }



        private Category selectedItem;
        public Category SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }

        private ObservableCollection<Category> itemsSource;
        public ObservableCollection<Category> ItemsSource
        {
            get { return itemsSource; }
            set
            {
                itemsSource = value;
                RaisePropertyChanged("ItemsSource");
            }
        }

    }
}
