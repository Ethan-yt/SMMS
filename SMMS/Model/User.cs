using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SMMS.Model
{
    public class User: ObservableObject
    {
        private int uid;
        private string uname;
        private string password;
        private int gid;
        private string tel;
        private string address;
        private string remark;
        private Group group;


        string[] pname = { "UID", "UNAME", "PASSWORD", "GID", "TEL", "ADDRESS", "REMARK" };

        public string findValueByName(string name)
        {
            switch (name)
            {
                case "UID":
                    return uid.ToString();
                case "UNAME":
                    return uname;
                case "PASSWORD":
                    return password;
                case "GID":
                    return gid.ToString();
                case "TEL":
                    return tel;
                case "ADDRESS":
                    return address;
                case "REMARK":
                    return remark;
            }
            return "";
        }
        public int UID
        {
            get
            {
                return uid;
            }

            set
            {
                uid = value;
                INotifyPropertyChanged("UID");
            }
        }


        public Group Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
                INotifyPropertyChanged("GROUP");
                RaisePropertyChanged("GROUP");
            }
        }
        public string UNAME
        {
            get
            {
                return uname;
            }

            set
            {
                uname = value;
                INotifyPropertyChanged("UNAME");
            }
        }

        public string PASSWORD
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
                INotifyPropertyChanged("PASSWORD");
            }
        }

        public int GID
        {
            get
            {
                return gid;
            }

            set
            {
                gid = value;
                INotifyPropertyChanged("GID");
            }
        }

        public string TEL
        {
            get
            {
                return tel;
            }

            set
            {
                tel = value;
                INotifyPropertyChanged("TEL");
            }
        }

        public string ADDRESS
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
                INotifyPropertyChanged("ADDRESS");
            }
        }

        public string REMARK
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
                INotifyPropertyChanged("REMARK");
            }
        }

        public User(int uid, string uname, string password, int gid, string tel, string address, string remark)
        {
            this.uid = uid;
            this.uname = uname;
            this.password = password;
            this.gid = gid;
            this.tel = tel;
            this.address = address;
            this.remark = remark;
            this.group = DBHelper.getGroup("ID = " + gid,false)[0];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void INotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
}
