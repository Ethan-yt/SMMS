using GalaSoft.MvvmLight;
using SMMS.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMMS.Model;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Data.SQLite;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using SMMS.Controls;

namespace SMMS.ViewModel.Goods
{
    public class DisplayViewModel : ViewModelBase
    {

        private readonly IModernNavigationService _modernNavigationService;

        public DisplayViewModel(IModernNavigationService modernNavigationService)
        {
            _modernNavigationService = modernNavigationService;
            NavigatedToCommand.Execute(null);

            Messenger.Default.Register<object[]>(this, p =>
            {
                if (p[0] as string == "NavigatingFromQueryPage")
                {
                    NavigatingFromCommand.Execute(p[1] as FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs);
                }
            });
            Messenger.Default.Register<object[]>(this, p =>
            {
                if (p[0] as string == "NavigatedToQueryPage")
                {
                    NavigatedToCommand.Execute(null);
                }
            });
        }

        private List<SMMS.Model.Goods> itemSource;

        private SQLiteTransaction t;

        private bool enableUndo;

        private bool enableDelete;

        public RelayCommand NavigatedToCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    t = DBHelper.beginTransaction();
                    var p = _modernNavigationService.Parameter as Dictionary<string, object>;
                    string where = null;
                    try
                    {
                        where = p["GoodsWhere"] as string;
                        ItemSource = DBHelper.getGoods(where);
                    }
                    catch
                    {
                        ItemSource = DBHelper.getGoods();
                    }

                    EnableAll = !string.IsNullOrEmpty(where);

                    foreach (Model.Goods goods in ItemSource)
                        goods.PropertyChanged += Goods_PropertyChanged;
                    EnableUndo = false;
                    LabelVisible = ItemSource.Count == 0;

                    EditVisible = !LabelVisible && DBHelper.currentUser.Group.EDITGOODS;
                    EditEnable = !EditVisible;
                    selectedGoods = new List<Model.Goods>();
                    EnableDelete = false;


                });
            }
        }

        public RelayCommand<FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs> NavigatingFromCommand
        {
            get
            {
                return new RelayCommand<FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs>((e) =>
                {
                    if (enableUndo)
                    {
                        var ret = ModernDialog.ShowMessage("您的修改还未保存，是否保存？", "警告", System.Windows.MessageBoxButton.YesNoCancel);
                        switch (ret)
                        {
                            case System.Windows.MessageBoxResult.Yes:
                                t.Commit();
                                break;
                            case System.Windows.MessageBoxResult.No:
                                t.Rollback();
                                break;
                            case System.Windows.MessageBoxResult.Cancel:
                                e.Cancel = true;
                                return;

                        }
                    }
                    if(t.Connection != null)
                        t.Commit();
                });
                }
        
        }

        private void Goods_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var goods = sender as Model.Goods;
            if (e.PropertyName == "ISSELECTED")
            {
                if (goods.IsSelected)
                    selectedGoods.Add(goods);
                else
                    selectedGoods.Remove(goods);
                EnableDelete = selectedGoods.Count != 0;
            }
            else
            {
                try
                {
                    DBHelper.updateGoods(goods.GID, e.PropertyName, goods.findValueByName(e.PropertyName));
                    EnableUndo = true;
                }
                catch
                {
                    ModernDialog.ShowMessage("数据格式不正确", "错误", System.Windows.MessageBoxButton.OK);
                }

            }



        }

        private List<Model.Goods> selectedGoods;

        public ICommand UndoCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EnableUndo = false;
                    t.Rollback();
                    NavigatedToCommand.Execute(null);
                });
            }
        }
        public ICommand AllCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    (_modernNavigationService.Parameter as Dictionary<string, object>)["GoodsWhere"] = null;
                    NavigatedToCommand.Execute(null);
                });
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    t.Commit();
                    NavigatedToCommand.Execute(null);
                    ModernDialog.ShowMessage("已成功保存所有改动", "成功", System.Windows.MessageBoxButton.OK);

                });
            }
        }

        public ICommand DeleteCommand
        {
            get
            {

                return new RelayCommand(() =>
                {
                    if (ModernDialog.ShowMessage("确定要删除选中的货物吗？", "警告", System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.OK)
                    {
                        foreach (var goods in selectedGoods)
                        {
                            try
                            {
                                DBHelper.deleteGoods(goods.GID);
                            }
                            catch
                            {
                                ModernDialog.ShowMessage("删除货物失败，货号：" + goods.GID, "错误", System.Windows.MessageBoxButton.OK);
                            }
                                
                        }
                        t.Commit();
                        NavigatedToCommand.Execute(null);
                    }


                });
            }
        }


        private bool editEnable;
        public bool EditEnable
        {
            get
            {
                return editEnable;
            }

            set
            {
                editEnable = value;
                RaisePropertyChanged("EditEnable");
            }
        }
        private bool editVisible;
        public bool EditVisible
        {
            get
            {
                return editVisible;
            }

            set
            {
                editVisible = value;
                RaisePropertyChanged("EditVisible");
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
                RaisePropertyChanged("GridVisible");
            }
        }
        public bool GridVisible
        {
            get
            {
                return !labelVisible;
            }
        }

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

        public bool EnableUndo
        {
            get
            {
                return enableUndo;
            }

            set
            {
                enableUndo = value;
                RaisePropertyChanged("EnableUndo");
            }
        }
        private bool enableAll;

        public bool EnableAll
        {
            get
            {
                return enableAll;
            }

            set
            {
                enableAll = value;
                RaisePropertyChanged("EnableAll");
            }
        }
        public bool EnableDelete
        {
            get
            {
                return enableDelete;
            }

            set
            {
                enableDelete = value;
                RaisePropertyChanged("EnableDelete");
            }
        }

    }

}