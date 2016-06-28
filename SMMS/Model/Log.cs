using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMMS.Model
{
    public class Log:INotifyPropertyChanged
    {
        private int id;
        private int uid;
        private string uname;
        private string command;
        private string time;


        public Log(int id, int uid, string uname, string command, string time)
        {
            this.id = id;
            this.uid = uid;
            this.uname = uname;
            this.command = command;
            this.time = time;
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                INotifyPropertyChanged("ISSELECTED");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void INotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public int ID
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                INotifyPropertyChanged("ID");
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

        public string COMMAND
        {
            get
            {
                return command;
            }

            set
            {
                command = value;
                INotifyPropertyChanged("COMMAND");
            }
        }

        public string TIME
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
                INotifyPropertyChanged("TIME");
            }
        }
    }
}
