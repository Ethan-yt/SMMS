using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using SMMS.Model;
using SMMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SMMS.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IModernNavigationService _modernNavigationService;

        public LoginViewModel(IModernNavigationService modernNavigationService)
        {
            _modernNavigationService = modernNavigationService;
            if (Properties.Settings.Default.Username != "")
                UsernameText = Properties.Settings.Default.Username;


        }
        private bool checkedRemember;
        public bool CheckedRemember
        {
            get
            {
                return checkedRemember;
            }

            set
            {
                checkedRemember = value;
                RaisePropertyChanged("CheckedRemember");
            }
        }
        private string usernameText;

        /// <summary>
        /// Gets or sets the resources command.
        /// </summary>
        /// <value>The resources command.</value>
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand<object[]>((user) =>
                {
                    User u = DBHelper.login((string)user[0], ((PasswordBox)user[1]).Password);
                    if (u != null)
                    {
                        if (u.Group.LOGIN)
                        {
                            if(CheckedRemember)
                                Properties.Settings.Default.Username = (string)user[0];
                            else
                                Properties.Settings.Default.Username = "";
                            Properties.Settings.Default.Save();

                            DBHelper.currentUser = u;
                            Messenger.Default.Send(new object[] { "Login" });
                            _modernNavigationService.NavigateTo(ViewModelLocator.MainPageKey);
                            
                        }
                        else
                        {
                            ModernDialog.ShowMessage("您已经被管理员禁止登陆", "错误", System.Windows.MessageBoxButton.OK);
                        }
                    }
                    else
                    {

                        ModernDialog.ShowMessage("用户名或密码错误，请重新输入", "错误", System.Windows.MessageBoxButton.OK);
                    }

                });
            }
        }

        public string UsernameText
        {
            get
            {
                return usernameText;
            }

            set
            {
                usernameText = value;
                RaisePropertyChanged("UsernameText");
            }
        }
    }
}
