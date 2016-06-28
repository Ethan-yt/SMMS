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
    public class UserViewModel : ViewModelBase
    {
        private readonly IModernNavigationService _modernNavigationService;

        public UserViewModel(IModernNavigationService modernNavigationService)
        {
            _modernNavigationService = modernNavigationService;
            Messenger.Default.Register<object[]>(this, p =>
            {
                if (p[0] as string == "NavigatingFromUser")
                {
                    NavigatingFromCommand.Execute(p[1] as FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs);
                }
            });
            Messenger.Default.Register<object[]>(this, p =>
            {
                if (p[0] as string == "NavigatedToUser")
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
                    SelectedItem = new KeyWordType { ID = 0, Name = "用户ID" };
                    ComboItemsSource = new ObservableCollection<KeyWordType>() { selectedItem, new KeyWordType { ID = 1, Name = "用户名" }, new KeyWordType { ID = 2, Name = "用户组ID" }, new KeyWordType { ID = 2, Name = "备注" }, };

                    GridVisible = false;


                });
            }
        }
        private ObservableCollection<Group> groups;
        public ObservableCollection<Group> Groups
        {
            get { return groups; }
            set
            {
                groups = value;
                RaisePropertyChanged("Groups");
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
                        if (selectedItem.Name == "用户ID")
                            where = "UID = " + KeyWord;
                        else if (selectedItem.Name == "用户名")
                            where = "UNAME LIKE '%" + KeyWord + "%'";
                        else if (selectedItem.Name == "用户组ID")
                            where = "GID = " + KeyWord;
                        else if (selectedItem.Name == "备注")
                            where = "REMARK LIKE '%" + KeyWord + "%'";
                    }

                    try
                    {
                        ItemSource = DBHelper.getUser(where as string);
                        foreach (var user in ItemSource)
                            user.PropertyChanged += Goods_PropertyChanged;

                        LabelVisible = ItemSource.Count == 0;
                        GridVisible = !LabelVisible;

                        Groups = new ObservableCollection<Model.Group>(DBHelper.getGroup(null,false));
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

        public RelayCommand<int> DeleteCommand
        {
            get
            {
                return new RelayCommand<int>((uid) =>
                {
                    if (ModernDialog.ShowMessage("确定要删除这个账号吗？", "警告", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                    {

                        DBHelper.deleteUser(uid);
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
                        DBHelper.addUser();
                        t.Commit();
                        NavigatedToCommand.Execute(null);
                        keyWord = "";
                        QueryCommand.Execute(null);
                    }
                    catch
                    {
                        ModernDialog.ShowMessage("请修改新用户的信息", "错误", System.Windows.MessageBoxButton.OK);
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

        private void Goods_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var user = sender as Model.User;
            try
            {
                string value = user.findValueByName(e.PropertyName);
                if (e.PropertyName == "PASSWORD")
                    DBHelper.updateUser(user.UID, e.PropertyName, DBHelper.MD5(value));
                else if (e.PropertyName == "GROUP")
                    DBHelper.updateUser(user.UID, "GID", user.Group.ID.ToString());
                else
                    DBHelper.updateUser(user.UID, e.PropertyName, value);
                EnableUndo = true;
            }
            catch
            {
                ModernDialog.ShowMessage("数据格式不正确", "错误", System.Windows.MessageBoxButton.OK);
                
            }

        }


        private List<Model.User> itemSource;
        public List<Model.User> ItemSource
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