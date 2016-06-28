using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using SMMS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using System.Windows.Input;
namespace SMMS.ViewModel
{
    public class LogViewModel : ViewModelBase
    {
        private readonly IModernNavigationService _modernNavigationService;

        public LogViewModel(IModernNavigationService modernNavigationService)
        {
            _modernNavigationService = modernNavigationService;
            selectedItem = new KeyWordType { ID = 0, Name = "用户名" };
            comboItemsSource = new ObservableCollection<KeyWordType>() {
            selectedItem,
            new KeyWordType { ID = 1, Name = "命令" },
            new KeyWordType { ID = 2, Name = "时间" },
            };
            GridVisible = false;
            date = DateTime.Now;
            EditVisible = false;
            EditEnable = false;

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

        private DateTime date;

        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
                RaisePropertyChanged("Date");
            }
        }
        public RelayCommand QueryCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string where = "";
                    where = "TIME LIKE '%" + date.ToString("yyyy-MM-dd") + "%' ";
                    if (!string.IsNullOrEmpty(KeyWord))
                    {
                        if (selectedItem.Name == "用户名")
                            where += "AND LOG.UID IN (SELECT USER.UID FROM USER WHERE UNAME LIKE '%" + KeyWord + "%')";
                        else if (selectedItem.Name == "命令")
                            where += "AND `COMMAND` LIKE '%" + KeyWord + "%'";
                        else if (selectedItem.Name == "时间")
                            where += "AND TIME LIKE '%" + KeyWord + "%'";
                    }
                    try
                    {
                        ItemSource = DBHelper.getLog(where as string);
                        foreach (Model.Log log in ItemSource)
                            log.PropertyChanged += Log_PropertyChanged;
                        LabelVisible = ItemSource.Count == 0;
                        GridVisible = !LabelVisible;
                        selectedLog = new List<Model.Log>();
                        IsSelectedAll = false;
                        EnableDelete = false;
                        EditVisible = !LabelVisible && DBHelper.currentUser.Group.EDITLOG;
                        EditEnable = !EditVisible;
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

        private void Log_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var log = sender as Model.Log;
            if (e.PropertyName == "ISSELECTED")
            {
                if (log.IsSelected)
                    selectedLog.Add(log);
                else
                    selectedLog.Remove(log);
                EnableDelete = selectedLog.Count != 0;
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

        private List<Model.Log> selectedLog;

        private bool enableDelete;

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
                        selectedLog = new List<Model.Log>(ItemSource.ToArray());
                    else
                        selectedLog.Clear();

                });
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var t = DBHelper.beginTransaction();

                    foreach (var log in selectedLog)
                    {
                        try
                        {
                            DBHelper.deleteLog(log.ID);
                        }
                        catch
                        {
                            ModernDialog.ShowMessage("删除失败", "失败", System.Windows.MessageBoxButton.OK);
                            t.Rollback();
                            return;
                        }
                    }
                    t.Commit();
                    QueryCommand.Execute(null);
                });


            }
        }

        private List<SMMS.Model.Log> itemSource;
        public List<Model.Log> ItemSource
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
