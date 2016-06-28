using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SMMS.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using System.Data.SQLite;
using SMMS.Model;

namespace SMMS.ViewModel.Personnel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class GroupViewModel : ViewModelBase
    {
        private readonly IModernNavigationService _modernNavigationService;

        public GroupViewModel(IModernNavigationService modernNavigationService)
        {
            _modernNavigationService = modernNavigationService;
            Messenger.Default.Register<object[]>(this, p =>
            {
                if (p[0] as string == "NavigatingFromGroup")
                {
                    NavigatingFromCommand.Execute(p[1] as FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs);
                }
            });
            Messenger.Default.Register<object[]>(this, p =>
            {
                if (p[0] as string == "NavigatedToGroup")
                {
                    NavigatedToCommand.Execute(null);
                }
            });
        }
        public RelayCommand NavigatedToCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    t = DBHelper.beginTransaction();

                    EnableUndo = false;
                    SelectedItem = new KeyWordType { ID = 0, Name = "用户组名" };
                    ComboItemsSource = new ObservableCollection<KeyWordType>() { selectedItem, new KeyWordType { ID = 1, Name = "用户组ID" },};

                    GridVisible = false;



                });
            }
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
                        if (selectedItem.Name == "用户组名")
                            where = "NAME LIKE '%" + KeyWord + "%'";
                        else if (selectedItem.Name == "用户组ID")
                            where = "ID = " + KeyWord;
                    }

                    try
                    {
                        ItemSource = DBHelper.getGroup(where as string);
                        foreach (var group in ItemSource)
                            group.PropertyChanged += Group_PropertyChanged;

                        LabelVisible = ItemSource.Count == 0;
                        GridVisible = !LabelVisible;

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

        private void Group_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var group = sender as Model.Group;
            try
            {
                string value = group.findValueByName(e.PropertyName);
                    DBHelper.updateGroup(group.id, e.PropertyName, value);
                EnableUndo = true;
            }
            catch
            {
                ModernDialog.ShowMessage("数据格式不正确", "错误", System.Windows.MessageBoxButton.OK);

            }
        }

        public RelayCommand<int> DeleteCommand
        {
            get
            {
                return new RelayCommand<int>((id) =>
                {
                    if (ModernDialog.ShowMessage("确定要删除这个用户组吗？", "警告", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                    {
                        DBHelper.deleteGroup(id);
                        t.Commit();
                        NavigatedToCommand.Execute(null);
                        QueryCommand.Execute(null);
                    }
                });
            }

        }

        public RelayCommand UndoCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    t.Rollback();
                    NavigatedToCommand.Execute(null);
                    QueryCommand.Execute(null);
                });
            }
        }
        public RelayCommand AddCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        DBHelper.addGroup();
                        t.Commit();
                        NavigatedToCommand.Execute(null);
                        keyWord = "";
                        QueryCommand.Execute(null);
                    }
                    catch
                    {
                        ModernDialog.ShowMessage("请修改新用户组的信息", "错误", System.Windows.MessageBoxButton.OK);
                    }


                });
            }
        }
        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    t.Commit();
                    NavigatedToCommand.Execute(null);
                    QueryCommand.Execute(null);
                    ModernDialog.ShowMessage("已成功保存所有改动", "成功", System.Windows.MessageBoxButton.OK);
                    DBHelper.currentUser.Group = DBHelper.getGroup("ID = " + DBHelper.currentUser.GID, false)[0];

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
                                DBHelper.currentUser.Group = DBHelper.getGroup("ID = " + DBHelper.currentUser.GID, false)[0];
                                break;
                            case System.Windows.MessageBoxResult.No:
                                t.Rollback();
                                break;
                            case System.Windows.MessageBoxResult.Cancel:
                                e.Cancel = true;
                                return;

                        }
                    }
                    else
                        t.Commit();
                });
            }

        }

        private bool enableUndo;

        private SQLiteTransaction t;

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


        private List<Model.Group> itemSource;
        public List<Model.Group> ItemSource
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

        public class KeyWordType
        {
            public int ID { set; get; }
            public string Name { set; get; }
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


    }
}