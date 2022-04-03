using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using yakov.TI.LabPages;

namespace yakov.TI.VM
{
    public enum CryptMode
    {
        Encryption,
        Decryption
    }

    public class MainContext: INotifyPropertyChanged
    {
        private Lab1Page _lab1page = new();
        private Lab2Page _lab2Page = new();
        private Lab3Page _lab3Page = new();
        private LabNoContentPage _lab4Page = new();

        private Page _currentPage;
        public Page CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public bool IsLab1Selected
        {
            get
            {
                return CurrentPage == _lab1page;
            }
            set 
            {
                CurrentPage = value ? _lab1page : CurrentPage;
                OnPropertyChanged("IsLab1Selected");
            }
        }

        public bool IsLab2Selected
        {
            get
            {
                return CurrentPage == _lab2Page;
            }
            set
            {
                CurrentPage = value ? _lab2Page : CurrentPage;
                OnPropertyChanged("IsLab2Selected");
            }
        }

        public bool IsLab3Selected
        {
            get
            {
                return CurrentPage == _lab3Page;
            }
            set
            {
                CurrentPage = value ? _lab3Page : CurrentPage;
                OnPropertyChanged("IsLab3Selected");
            }
        }

        public bool IsLab4Selected
        {
            get
            {
                return CurrentPage == _lab4Page;
            }
            set
            {
                CurrentPage = value ? _lab4Page : CurrentPage;
                OnPropertyChanged("IsLab4Selected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
