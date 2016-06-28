using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMMS.Model
{
    public class Group : INotifyPropertyChanged
    {
        public int id;
        private string name;
        private bool login;
        private bool editgoods;
        private bool querygoods;
        private bool restockgoods;
        private bool salegoods;
        private bool editpersonnel;
        private bool editgroup;
        private bool editlog;
        private bool querylog;

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return GetHashCode() == obj.GetHashCode();
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return id;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void INotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string findValueByName(string n)
        {
            switch (n)
            {
                case "ID":
                    return id.ToString();
                case "NAME":
                    return name;
                case "LOGIN":
                    return login ? "1" : "0";
                case "EDITGOODS":
                    return editgoods ? "1" : "0";
                case "QUERYGOODS":
                    return querygoods ? "1" : "0";
                case "RESTOCKGOODS":
                    return restockgoods ? "1" : "0";
                case "SALEGOODS":
                    return salegoods ? "1" : "0";
                case "EDITPERSONNEL":
                    return editpersonnel ? "1" : "0";
                case "EDITGROUP":
                    return editgroup ? "1" : "0";
                case "EDITLOG":
                    return editlog ? "1" : "0";
                case "QUERYLOG":
                    return querylog ? "1" : "0";
            }
            return "";
        }

        public Group(int id, string name, bool login, bool editgoods, bool querygoods, bool restockgoods, bool salegoods, bool editpersonnel, bool editgroup, bool editlog, bool querylog)
        {
            this.ID = id;
            this.NAME = name;
            this.LOGIN = login;
            this.EDITGOODS = editgoods;
            this.QUERYGOODS = querygoods;
            this.RESTOCKGOODS = restockgoods;
            this.SALEGOODS = salegoods;
            this.EDITPERSONNEL = editpersonnel;
            this.EDITGROUP = editgroup;
            this.EDITLOG = editlog;
            this.QUERYLOG = querylog;
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

        public string NAME
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                INotifyPropertyChanged("NAME");
            }
        }

        public bool LOGIN
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
                INotifyPropertyChanged("LOGIN");
            }
        }

        public bool EDITGOODS
        {
            get
            {
                return editgoods;
            }

            set
            {
                editgoods = value;
                INotifyPropertyChanged("EDITGOODS");
            }
        }

        public bool QUERYGOODS
        {
            get
            {
                return querygoods;
            }

            set
            {
                querygoods = value;
                INotifyPropertyChanged("QUERYGOODS");
            }
        }

        public bool RESTOCKGOODS
        {
            get
            {
                return restockgoods;
            }

            set
            {
                restockgoods = value;
                INotifyPropertyChanged("RESTOCKGOODS");
            }
        }

        public bool SALEGOODS
        {
            get
            {
                return salegoods;
            }

            set
            {
                salegoods = value;
                INotifyPropertyChanged("SALEGOODS");
            }
        }

        public bool EDITPERSONNEL
        {
            get
            {
                return editpersonnel;
            }

            set
            {
                editpersonnel = value;
                INotifyPropertyChanged("EDITPERSONNEL");
            }
        }

        public bool EDITGROUP
        {
            get
            {
                return editgroup;
            }

            set
            {
                editgroup = value;
                INotifyPropertyChanged("EDITGROUP");
            }
        }

        public bool EDITLOG
        {
            get
            {
                return editlog;
            }

            set
            {
                editlog = value;
                INotifyPropertyChanged("EDITLOG");
            }
        }

        public bool QUERYLOG
        {
            get
            {
                return querylog;
            }

            set
            {
                querylog = value;
                INotifyPropertyChanged("QUERYLOG");
            }
        }
    }
}
