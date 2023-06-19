using Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Globalization;
using System.Security.Policy;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Web.UI.WebControls;
using ListViewItem = System.Windows.Controls.ListViewItem;
using System.Collections.ObjectModel;

namespace DataBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindow()
        {
            InitializeComponent();
        }

        //BindingList<Phone> _listPhones = new BindingList<Phone>();
        ObservableCollection<Phone> _allItems = new ObservableCollection<Phone>();
        ObservableCollection<Phone> _subItems = new ObservableCollection<Phone>();

        private int _selectedIndex = -1;
        private int _totalItems = 0;
        private int _currentPage = 0;
        private int _displayingItems = 0;
        private int _totalPages = 0;
        private int _rowsPerPage = 10;
        private string _keyword = "";

        //public string Keyword { get; set; } = "";
        public string Keyword
        {
            get
            {
                return _keyword;
            }
            set
            {
                _keyword = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Keyword"));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Keyword"));
            }
        }

        //public int RowsPerPage { get; set; } = 10;
        public int RowsPerPage
        {
            get
            {
                return _rowsPerPage;
            }
            set
            {
                _rowsPerPage = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("RowsPerPage"));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowsPerPage"));
            }
        }

        //public int TotalPages { get; set; } = 0;
        public int TotalPages
        {
            get
            {
                return _totalPages;
            }
            set
            {
                _totalPages = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("TotalPages"));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalPages"));
            }
        }

        //public int CurrentPage { get; set; } = 0;
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("CurrentPage"));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentPage"));
            }
        }

        //public int DisplayingItems { get; set; } = 0;
        public int DisplayingItems
        {
            get
            {
                return _displayingItems;
            }
            set
            {
                _displayingItems = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DisplayingItems"));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayingItems"));
            }
        }

        //public int TotalItems { get; set; } = 0;
        public int TotalItems
        {
            get
            {
                return _totalItems;
            }
            set
            {
                _totalItems = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("TotalItems"));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalItems"));
            }
        }
        

        void UpdatePaging()
        {
            TotalItems = _allItems.Count;
            TotalPages = (TotalItems / RowsPerPage) +
                (TotalItems % RowsPerPage == 0 ? 0 : 1);

            _subItems = new ObservableCollection<Phone>(_allItems
                .Skip((CurrentPage - 1) * RowsPerPage)
                .Take(RowsPerPage).ToList());
            DisplayingItems = _subItems.Count();
            phonesComboBox.ItemsSource = _subItems;
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string fileName = "data.txt";
            string[] lines = File.ReadAllLines(fileName);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(new string[] { ";" }, StringSplitOptions.None);

                string name = tokens[0];
                string image = tokens[1];
                string manufacturer = tokens[2];
                string price = tokens[3];

                var phone = new Phone()
                {
                    Name = name,
                    Image = image,
                    Manufacturer = manufacturer,
                    Price = price
                };

                _allItems.Add(phone);

                phonesComboBox.ItemsSource = _allItems;

                phonesComboBox.SelectedIndex = -1;

                CurrentPage = 1;
                UpdatePaging();

                DataContext = this;
            }
        }

        private void BackstageTabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }       

        private void ScrollToBottom()
        {
            var selectedIndex = phonesComboBox.Items.Count - 1;

            phonesComboBox.SelectedIndex = selectedIndex;
            phonesComboBox.UpdateLayout();
            phonesComboBox.ScrollIntoView(phonesComboBox.SelectedItem);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddPhoneWindow();
            //bool isScroll = false;

            if (screen.ShowDialog() == true)
            {
                var phone = (Phone)screen.NewPhone; 
                _allItems.Add(phone);

                UpdatePaging();
                //isScroll = true;
            }
            else
            {
                // TODO...
            }

            //if (isScroll) ScrollToBottom();
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if(phonesComboBox.SelectedIndex >= 0 && phonesComboBox.SelectedIndex < _allItems.Count)
            {
                var phone = (Phone)phonesComboBox.SelectedItem;
                var result = MessageBox.Show($"Bạn thật sự muốn xóa điện thoại {phone.Name} ?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _allItems.Remove(phone);
                    MessageBox.Show("Xóa điện thoại thành công", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdatePaging();
                }
                else
                {
                    //Do nothing
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn điện thoại cần xóa !", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (phonesComboBox.SelectedIndex >= 0 && phonesComboBox.SelectedIndex < _allItems.Count)
            {
                var phone = (Phone)phonesComboBox.SelectedItem;
                var screen = new EditPhoneWindow(phone);
                if (screen.ShowDialog() == true && phonesComboBox.SelectedIndex != -1)
                {
                    var info = screen.EditedPhone;
                    phone.Name = info.Name;
                }
                else
                {
                    //Do nothing
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn điện thoại cần chỉnh sửa !", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                int index = phonesComboBox.SelectedIndex;

                var bitmap = new BitmapImage(new Uri(_subItems[index].Image, UriKind.Relative));
                imageSub.ImageSource = bitmap;

                string namesub = _subItems[index].Name;
                nameMain.Text = namesub;
                nameSub.Text = namesub;

                string respricesub = _subItems[index].Price;
                var cul = CultureInfo.GetCultureInfo("vi-VN");
                string pricesub = (string)respricesub;
                if(pricesub != "0")
                    pricesub = double.Parse((string)respricesub).ToString("#,# đ", cul.NumberFormat);
                priceSub.Text = pricesub;

                string manusub = _subItems[index].Manufacturer;
                manuSub.Text = manusub;

                _selectedIndex = index;
            }
        }

        private void deleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (phonesComboBox.SelectedIndex >= 0 && phonesComboBox.SelectedIndex < _allItems.Count)
            {
                var phone = (Phone)phonesComboBox.SelectedItem;
                var result = MessageBox.Show($"Bạn thật sự muốn xóa điện thoại {phone.Name} ?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _allItems.Remove(phone);
                    MessageBox.Show("Xóa điện thoại thành công", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdatePaging();
                }
                else
                {
                    //Do nothing
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn điện thoại cần xóa !", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void editMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (phonesComboBox.SelectedIndex >= 0 && phonesComboBox.SelectedIndex < _allItems.Count)
            {
                var phone = (Phone)phonesComboBox.SelectedItem;
                var screen = new EditPhoneWindow(phone);
                if (screen.ShowDialog() == true && phonesComboBox.SelectedIndex != -1)
                {
                    var info = screen.EditedPhone;
                    phone.Name = info.Name;
                }
                else
                {
                    //Do nothing
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn điện thoại cần chỉnh sửa !", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (phonesComboBox.SelectedIndex >= 0 && phonesComboBox.SelectedIndex < _allItems.Count)
            {
                var phone = (Phone)phonesComboBox.SelectedItem;
                var screen = new EditPhoneWindow(phone);
                if (screen.ShowDialog() == true && phonesComboBox.SelectedIndex != -1)
                {
                    var info = screen.EditedPhone;
                    phone.Name = info.Name;
                }
                else
                {
                    //Do nothing
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn điện thoại cần chỉnh sửa !", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedIndex--;
            if (_selectedIndex <= -1)
            {
                _selectedIndex = 0;
            }
            
            if (_selectedIndex >= 0 && _selectedIndex < _allItems.Count)
            {

                var bitmap = new BitmapImage(new Uri(_allItems[_selectedIndex].Image, UriKind.Relative));
                imageSub.ImageSource = bitmap;

                string namesub = _allItems[_selectedIndex].Name;
                nameMain.Text = namesub;
                nameSub.Text = namesub;

                string respricesub = _allItems[_selectedIndex].Price;
                var cul = CultureInfo.GetCultureInfo("vi-VN");
                var pricesub = double.Parse((string)respricesub).ToString("#,# đ", cul.NumberFormat);
                priceSub.Text = pricesub;

                string manusub = _allItems[_selectedIndex].Manufacturer;
                manuSub.Text = manusub;
            }
            else
            {
                //MessageBox.Show("Đã hết điện thoại !", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            phonesComboBox.SelectedIndex = _selectedIndex;
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedIndex++;
            if (_selectedIndex >= _allItems.Count)
            {
                _selectedIndex = _allItems.Count - 1;
            }
            if (_selectedIndex >= 0 && _selectedIndex < _allItems.Count)
            {
                
                var bitmap = new BitmapImage(new Uri(_allItems[_selectedIndex].Image, UriKind.Relative));
                imageSub.ImageSource = bitmap;

                string namesub = _allItems[_selectedIndex].Name;
                nameMain.Text = namesub;
                nameSub.Text = namesub;

                string respricesub = _allItems[_selectedIndex].Price;
                var cul = CultureInfo.GetCultureInfo("vi-VN");
                var pricesub = double.Parse((string)respricesub).ToString("#,# đ", cul.NumberFormat);
                priceSub.Text = pricesub;

                string manusub = _allItems[_selectedIndex].Manufacturer;
                manuSub.Text = manusub;
            }
            else
            {
                //MessageBox.Show("Đã hết điện thoại !", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            phonesComboBox.SelectedIndex = _selectedIndex;
        }

        private void keywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _subItems = new ObservableCollection<Phone>(_allItems.Where(
               phone => phone.Name.Contains(Keyword)
           ).ToList());

            phonesComboBox.ItemsSource = _subItems;
            
        }

        private void nextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                UpdatePaging();
            }
            else
            {
                // Do nothing
            }
        }

        private void backPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdatePaging();
            }
            else
            {
                // Do nothing
            }
        }
    }
}
