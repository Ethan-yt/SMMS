using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMMS.Model
{
    public class Goods:INotifyPropertyChanged
    {
        private int gid;
        private string gname;
        private float price;
        private string category;
        private string unit;
        private int num;
        private string code;

        public event PropertyChangedEventHandler PropertyChanged;
        private void INotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Goods(int gid, string gname, float price, string category,string unit,int num,string code)
        {
            this.gid = gid;
            this.gname = gname;
            this.price = price;
            this.category = category;
            this.unit = unit;
            this.num = num;
            this.code = code;
        }
        
        public string findValueByName(string name)
        {
            switch(name)
            {
                case "GID":
                    return gid.ToString();
                case "GNAME":
                    return gname;
                case "PRICE":
                    return price.ToString();
                case "CATEGORY":
                    return category;
                case "UNIT":
                    return unit;
                case "NUM":
                    return num.ToString();
                case "CODE":
                    return code;
            }
            return "";
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
            }
        }

        public string GNAME
        {
            get
            {
                return gname;
            }

            set
            {
                gname = value;
                INotifyPropertyChanged("GNAME");
            }
        }

        public float PRICE
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
                INotifyPropertyChanged("PRICE");
            }
        }

        public string CATEGORY
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
                INotifyPropertyChanged("CATEGORY");
            }
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

        public string UNIT
        {
            get
            {
                return unit;
            }

            set
            {
                unit = value;
                INotifyPropertyChanged("UNIT");
            }
        }

        public int NUM
        {
            get
            {
                return num;
            }

            set
            {
                num = value;
                INotifyPropertyChanged("NUM");
            }
        }

        public string CODE
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                INotifyPropertyChanged("CODE");
            }
        }
    }
}
