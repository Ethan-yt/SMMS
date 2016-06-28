using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using SMMS.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using System.Windows.Input;
namespace SMMS.ViewModel.Goods
{
    public class RestockViewModel : ViewModelBase
    {
        private readonly IModernNavigationService _modernNavigationService;

        public RestockViewModel(IModernNavigationService modernNavigationService)
        {
            _modernNavigationService = modernNavigationService;
            selectedItem = new KeyWordType { ID = 0, Name = "商品名" };
            comboItemsSource = new ObservableCollection<KeyWordType>() {
            selectedItem,
            new KeyWordType { ID = 1, Name = "货号" },
            new KeyWordType { ID = 2, Name = "条形码" },
            };
            GridVisible = false;

        }

        public RelayCommand QueryCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string where = "";
                    if (!string.IsNullOrEmpty(KeyWord))
                    {
                        if (selectedItem.Name == "商品名")
                            where = "GNAME LIKE '%" + KeyWord + "%'";
                        else if (selectedItem.Name == "货号")
                            where = "GID = " + KeyWord;
                        else if (selectedItem.Name == "条形码")
                            where = "CODE = " + KeyWord;
                    }

                    try
                    {
                        ItemSource = DBHelper.getGoods(where as string);
                        foreach (Model.Goods goods in ItemSource)
                            goods.PropertyChanged += Goods_PropertyChanged;
                        LabelVisible = ItemSource.Count == 0;
                        GridVisible = !LabelVisible;
                        selectedGoods = new List<Model.Goods>();
                        IsSelectedAll = false;
                        EnableAdd = false;
                    }
                    catch
                    {
                        ModernDialog.ShowMessage("关键字格式不正确", "错误", System.Windows.MessageBoxButton.OK);
                        LabelVisible = false;
                        GridVisible = false;
                    }

                });
            }
        }


        private List<Model.Goods> selectedGoods;
        private void Goods_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var goods = sender as Model.Goods;
            if (e.PropertyName == "ISSELECTED")
            {
                if (goods.IsSelected)
                    selectedGoods.Add(goods);
                else
                    selectedGoods.Remove(goods);
                EnableAdd = selectedGoods.Count != 0;
                //RaisePropertyChanged("ItemSource");
                foreach (var g in itemSource)
                {
                    if (!g.IsSelected)
                    {
                        IsSelectedAll = false;
                        return;
                    }
                }
                IsSelectedAll = true;
            }
        }
        private bool enableAdd;

        public bool EnableAdd
        {
            get
            {
                return enableAdd;
            }

            set
            {
                enableAdd = value;
                RaisePropertyChanged("EnableAdd");
            }
        }

        private bool isSelectedAll;

        public bool IsSelectedAll
        {
            get
            {
                return isSelectedAll;
            }

            set
            {
                isSelectedAll = value;
                RaisePropertyChanged("IsSelectedAll");
            }
        }

        public RelayCommand<bool> SelectAllCommand
        {
            get
            {
                return new RelayCommand<bool>((p) =>
                {
                    ItemSource.ForEach((a) => a.IsSelected = p);
                    if (p)
                        selectedGoods = new List<Model.Goods>(ItemSource.ToArray());
                    else
                        selectedGoods.Clear();

                });
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var t = DBHelper.beginTransaction();

                    foreach (var goods in selectedGoods)
                    {
                        try
                        {
                            DBHelper.updateGoods(goods.GID, "NUM", (int.Parse(GoodsNum) + goods.NUM).ToString(),true,false, int.Parse(GoodsNum));
                        }
                        catch
                        {
                            ModernDialog.ShowMessage("进货失败！请检查数量设置是否正确。", "失败", System.Windows.MessageBoxButton.OK);
                            t.Rollback();
                            return;
                        }
                    }

                    ModernDialog.ShowMessage("进货成功！", "成功", System.Windows.MessageBoxButton.OK);
                    t.Commit();
                    QueryCommand.Execute(null);
                });


            }
        }

        private List<SMMS.Model.Goods> itemSource;
        public List<Model.Goods> ItemSource
        {
            get
            {
                return itemSource;
            }

            set
            {
                itemSource = value;
                RaisePropertyChanged("ItemSource");
            }
        }

        private string goodsNum;
        public string GoodsNum
        {
            get
            {
                return goodsNum;
            }

            set
            {
                goodsNum = value;
                RaisePropertyChanged("GoodsNum");
            }
        }
        private bool labelVisible;
        public bool LabelVisible
        {
            get
            {
                return labelVisible;
            }

            set
            {
                labelVisible = value;
                RaisePropertyChanged("LabelVisible");
            }
        }
        private bool gridVisible;
        public bool GridVisible
        {
            get
            {
                return gridVisible;
            }
            set
            {
                gridVisible = value;
                RaisePropertyChanged("GridVisible");
            }
        }




        public class KeyWordType
        {
            public int ID { set; get; }
            public string Name { set; get; }
        }




        private KeyWordType selectedItem;
        public KeyWordType SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }


        private ObservableCollection<KeyWordType> comboItemsSource;
        public ObservableCollection<KeyWordType> ComboItemsSource
        {
            get { return comboItemsSource; }
            set
            {
                comboItemsSource = value;
                RaisePropertyChanged("ComboItemsSource");
            }
        }

        private string keyWord;

        public string KeyWord
        {
            get
            {
                return keyWord;
            }

            set
            {
                keyWord = value;
                RaisePropertyChanged("KeyWord");
            }
        }
    }
}
