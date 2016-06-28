using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SMMS.Services;
using System.ComponentModel;
using System.Windows.Controls;
using System;

namespace SMMS.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class UserDataViewModel : ViewModelBase
    {
        private readonly IModernNavigationService _modernNavigationService;

        public UserDataViewModel(IModernNavigationService modernNavigationService)
        {
            NavigatedToCommand.Execute(null);
            _modernNavigationService = modernNavigationService;
        }


        public RelayCommand NavigatedToCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    UID = DBHelper.currentUser.UID.ToString();
                    UNAME = DBHelper.currentUser.UNAME;
                    TEL = DBHelper.currentUser.TEL;
                    ADDRESS = DBHelper.currentUser.ADDRESS;
                    REMARK = DBHelper.currentUser.REMARK;
                });
            }
        }
        public RelayCommand<object[]> EditCommand
        {
            get
            {
                return new RelayCommand<object[]>((password) =>
                {
                    if (UNAME != DBHelper.currentUser.UNAME)
                        DBHelper.updateUser(DBHelper.currentUser.UID, "UNAME", UNAME);
                    if (TEL != DBHelper.currentUser.TEL)
                        DBHelper.updateUser(DBHelper.currentUser.UID, "TEL", TEL);
                    if (ADDRESS != DBHelper.currentUser.ADDRESS)
                        DBHelper.updateUser(DBHelper.currentUser.UID, "ADDRESS", ADDRESS);
                    if (REMARK != DBHelper.currentUser.REMARK)
                        DBHelper.updateUser(DBHelper.currentUser.UID, "REMARK", REMARK);

                    if(((PasswordBox)password[0]).Password != "")
                    {
                        if(((PasswordBox)password[0]).Password == ((PasswordBox)password[1]).Password)
                        {
                            DBHelper.updateUser(DBHelper.currentUser.UID, "PASSWORD", DBHelper.MD5(((PasswordBox)password[0]).Password));
                        }
                        else
                        {
                            ModernDialog.ShowMessage("两次输入的密码不一致", "错误", System.Windows.MessageBoxButton.OK);
                            return;
                        }
                    }
                    ModernDialog.ShowMessage("修改成功！", "成功", System.Windows.MessageBoxButton.OK);
                    ((PasswordBox)password[0]).Password = ((PasswordBox)password[1]).Password = "";
                    DBHelper.currentUser = DBHelper.getUser("UID = " + UID)[0];
                    NavigatedToCommand.Execute(null);
                });


            }
        }
        private string remark;
        public string REMARK
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
                RaisePropertyChanged("REMARK");
            }
        }
        private string address;
        public string ADDRESS
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
                RaisePropertyChanged("ADDRESS");
            }
        }
        private string tel;
        public string TEL
        {
            get
            {
                return tel;
            }

            set
            {
                tel = value;
                RaisePropertyChanged("TEL");
            }
        }
        private string uname;
        public string UNAME
        {
            get
            {
                return uname;
            }

            set
            {
                uname = value;
                RaisePropertyChanged("UNAME");
            }
        }

        private string uid;
        public string UID
        {
            get
            {
                return uid;
            }

            set
            {
                uid = value;
                RaisePropertyChanged("UID");
            }
        }

    }
}