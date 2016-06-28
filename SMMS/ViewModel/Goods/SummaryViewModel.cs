using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
using LiveCharts.Wpf;
using SMMS.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using System.Windows.Input;
namespace SMMS.ViewModel.Goods
{
    public class SummaryViewModel : ViewModelBase
    {
        private readonly IModernNavigationService _modernNavigationService;

        public SummaryViewModel(IModernNavigationService modernNavigationService)
        {
            _modernNavigationService = modernNavigationService;
            init1();
            init2();
            NavigatedToCommand.Execute(null);
            init4();
        }

        private void init1()
        {
            var goods = DBHelper.getGoods("NUM <= 50", false);
            
            List<string> names = new List<string>();

            SeriesCollection1 = new SeriesCollection();
            SeriesCollection1.Add(new ColumnSeries() { Title = "货物"});
            SeriesCollection1[0].Values = new ChartValues<int>();
            foreach (var g in goods)
            {
                SeriesCollection1[0].Values.Add(g.NUM);
                names.Add(g.GNAME);
            }
            Labels1 = names.ToArray();
        }
        private void init2()
        {

            List<string> names = new List<string>();

            seriesCollection2 = new SeriesCollection();
            seriesCollection2.Add(new LineSeries() { Title = "销售量" });
            seriesCollection2[0].Values = new ChartValues<int>();
            for (int i =1;i<=7;i++)
            {
                var log = DBHelper.getLog("date(TIME) = date('now','localtime','-"+i+" day') AND COMMAND LIKE '%销售 货号%'",false);
                int num = 0;
                foreach (var l in log)
                {
                    string str = l.COMMAND.Substring(l.COMMAND.IndexOf("数量") + 3);
                    str = str.Substring(0, str.IndexOf(" "));
                    num += int.Parse(str);
                }

                seriesCollection2[0].Values.Add(num);
                names.Add(i+"天前");
            }
            Labels2 = names.ToArray();

        }
        private void init3()
        {

            List<string> names = new List<string>();

            SeriesCollection3 = new SeriesCollection();
  
            var summary = DBHelper.getSummary();

            foreach (var s in summary)
            {
                SeriesCollection3.Add(new PieSeries
                {
                    Title = s[1] as string,
                    Values = new ChartValues<int> { s[0] },
                    DataLabels = true
                });

            }

        }
        private void init4()
        {

            List<string> names = new List<string>();

            seriesCollection4 = new SeriesCollection();
            seriesCollection4.Add(new LineSeries() { Title = "进货量" });
            seriesCollection4[0].Values = new ChartValues<int>();
            for (int i = 1; i <= 7; i++)
            {
                var log = DBHelper.getLog("date(TIME) = date('now','localtime','-" + i + " day') AND COMMAND LIKE '%进货 货号%'",false);
                int num = 0;
                foreach (var l in log)
                {
                    string str = l.COMMAND.Substring(l.COMMAND.IndexOf("数量") + 3);
                    str = str.Substring(0, str.IndexOf(" "));
                    num += int.Parse(str);
                }

                seriesCollection4[0].Values.Add(num);
                names.Add(i + "天前");
            }
            Labels4 = names.ToArray();

        }


        public RelayCommand NavigatedToCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    init3();

                });
            }
        }

        private SeriesCollection seriesCollection1;
        private string[] labels1;
        private SeriesCollection seriesCollection2;
        private string[] labels2;
        private SeriesCollection seriesCollection3;
        private SeriesCollection seriesCollection4;
        private string[] labels4;
        public SeriesCollection SeriesCollection1
        {
            get
            {
                return seriesCollection1;
            }

            set
            {
                seriesCollection1 = value;
                RaisePropertyChanged("SeriesCollection1");
            }
        }

        public string[] Labels1
        {
            get
            {
                return labels1;
            }

            set
            {
                labels1 = value;
                RaisePropertyChanged("Labels1");
            }
        }
        public SeriesCollection SeriesCollection2
        {
            get
            {
                return seriesCollection2;
            }

            set
            {
                seriesCollection2 = value;
                RaisePropertyChanged("SeriesCollection2");
            }
        }

        public string[] Labels2
        {
            get
            {
                return labels2;
            }

            set
            {
                labels2 = value;
                RaisePropertyChanged("Labels2");
            }
        }
        public SeriesCollection SeriesCollection3
        {
            get
            {
                return seriesCollection3;
            }

            set
            {
                seriesCollection3 = value;
                RaisePropertyChanged("SeriesCollection3");
            }
        }
        public SeriesCollection SeriesCollection4
        {
            get
            {
                return seriesCollection4;
            }

            set
            {
                seriesCollection4 = value;
                RaisePropertyChanged("SeriesCollection4");
            }
        }

        public string[] Labels4
        {
            get
            {
                return labels4;
            }

            set
            {
                labels4 = value;
                RaisePropertyChanged("Labels4");
            }
        }
    }

}
