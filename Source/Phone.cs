using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBinding
{
    public class Phone : INotifyPropertyChanged, ICloneable
    {
        private string _name;
        private string _image;
        private string _price;
        private string _manufacturer;
        public string Name 
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Image"));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Image"));
            }
        }
        public string Manufacturer
        {
            get
            {
                return _manufacturer;
            }
            set
            {
                _manufacturer = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Manufacturer"));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Manufacturer"));
            }
        }
        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Price"));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
