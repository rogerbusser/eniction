using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Windows.UI.Xaml.Data;

namespace Eniction
{
    public class EnictionData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private double? _temp;
        private string _ritme;
        private string _label;
        private string _label2;
        private string _label3;
        private double? _confidence;

        public double? Temp
        {
            get { return _temp; }
            set
            {
                if (_temp != value)
                {
                    _temp = value;
                    RaisePropertyChanged("Temp");
                }
            }
        }

        public string Ritme
        {
            get { return _ritme; }
            set
            {
                if (_ritme != value)
                {
                    _ritme = value;
                    RaisePropertyChanged("Ritme");
                }
            }
        }
        public string Label
        {
            get { return _label; }
            set
            {
                if (_label != value)
                {
                    _label = value;
                    RaisePropertyChanged("Label");
                }
            }
        }
        public string Label2
        {
            get { return _label2; }
            set
            {
                if (_label2 != value)
                {
                    _label2 = value;
                    RaisePropertyChanged("Label2");
                }
            }
        }
        public string Label3
        {
            get { return _label3; }
            set
            {
                if (_label3 != value)
                {
                    _label3 = value;
                    RaisePropertyChanged("Label3");
                }
            }
        }
        public double? Confidence
        {
            get { return _confidence; }
            set
            {
                if (_confidence != value)
                {
                    _confidence = value;
                    RaisePropertyChanged("Confidence");
                }
            }
        }

        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        } 
    }

    public class TextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            double retVal= ((double)value - 45f) / 10f + 15f;
            return String.Format(parameter as string, retVal);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return null;
        }
    }
}
