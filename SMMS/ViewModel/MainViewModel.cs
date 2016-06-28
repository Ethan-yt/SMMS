using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using SMMS.Model;
using SMMS.Services;
using System.Collections.Generic;

namespace SMMS.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepsViewModel"/> class. 
        /// </summary>
        /// <param name="modernNavigationService">
        /// The modern Navigation Service.
        /// </param>
        /// 
        public MainViewModel(IModernNavigationService modernNavigationService)
        {
            _modernNavigationService = modernNavigationService;
            NavigatedToCommand.Execute(null);
        }
        public RelayCommand NavigatedToCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                    var user = DBHelper.currentUser;
                    string str = "你好！";
                    str += "[" + user.Group.NAME + "]";
                    str += user.UNAME + "。";
                    HelloStr = str;

                    MsgStr = "上次登录时间：" + DBHelper.getLastLoginTime(user.UNAME);
                });
            }
        }

        public string HelloStr
        {
            get
            {
                return helloStr;
            }

            set
            {
                helloStr = value;
                RaisePropertyChanged("HelloStr");
            }
        }

        public string MsgStr
        {
            get
            {
                return msgStr;
            }

            set
            {
                msgStr = value;
                RaisePropertyChanged("MsgStr");
            }
        }

        private string helloStr;
        private string msgStr;
    }
}