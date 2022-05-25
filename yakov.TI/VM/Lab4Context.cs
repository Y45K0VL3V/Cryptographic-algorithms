using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using yakov.TI.Lab4;

namespace yakov.TI.VM
{
    public class Lab4Context : INotifyPropertyChanged
    {

        private DSAParams _paramsEDS = new DSAParams();

        public string KeyP
        {
            get => _paramsEDS.p != 0 ? _paramsEDS.p.ToString() : null;
            set
            {
                _paramsEDS.p = BigInteger.Parse(value);
                OnPropertyChanged("Key P");
            }
        }

        public string KeyQ
        {
            get => _paramsEDS.q != 0 ? _paramsEDS.q.ToString() : null;
            set
            {
                _paramsEDS.q = BigInteger.Parse(value);
                OnPropertyChanged("Key Q");
            }
        }

        public string KeyH
        {
            get => _paramsEDS.h != 0 ? _paramsEDS.h.ToString() : null;
            set
            {
                _paramsEDS.h = BigInteger.Parse(value);
                OnPropertyChanged("Key H");
            }
        }

        public string KeyX
        {
            get => _paramsEDS.x != 0 ? _paramsEDS.x.ToString() : null;
            set
            {
                _paramsEDS.x = BigInteger.Parse(value);
                OnPropertyChanged("Key X");
            }
        }

        public string KeyK
        {
            get => _paramsEDS.k != 0 ? _paramsEDS.k.ToString() : null;
            set
            {
                _paramsEDS.k = BigInteger.Parse(value);
                OnPropertyChanged("Key K");
            }
        }

        public string PublicKeyY
        {
            get => _paramsEDS.y != 0 ? _paramsEDS.y.ToString() : null;
            set
            {
                _paramsEDS.y = BigInteger.Parse(value);
                OnPropertyChanged("Key Y");
            }
        }


        private byte[] _currentBytes;

        private string _currentTextFile;
        public string CurrentTextFile
        {
            get
            {
                return _currentTextFile;
            }
            set
            {
                _currentTextFile = value;
                ////TODO: Auto-checking sign.
                OnPropertyChanged("CurrentTextFile");
            }
        }

        #region File interact.

        private string _filePath;
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                _currentBytes = File.ReadAllBytes(value);
                CurrentTextFile = File.ReadAllText(value);

                //ClearFields();

                OnPropertyChanged("FilePath");
            }
        }

        private RelayCommand _getInputFile;
        public RelayCommand GetInputFile
        {
            get
            {
                return _getInputFile ?? (_getInputFile = new RelayCommand(obj =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == true)
                    {
                        FilePath = openFileDialog.FileName;
                    }
                }));
            }
        }

        private RelayCommand _saveProcessedFile;
        public RelayCommand SaveProcessedFile
        {
            get
            {
                return _saveProcessedFile ?? (_saveProcessedFile = new RelayCommand(obj =>
                {

                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (saveFileDialog.ShowDialog() == true)
                    {
                       File.WriteAllBytes(saveFileDialog.FileName, _currentBytes);
                    }
                }));
            }
        }

        #endregion

        private RelayCommand _doSignFile;
        public RelayCommand DoSignFile
        {
            get
            {
                return _doSignFile ?? (_doSignFile = new RelayCommand(obj =>
                {
                    _currentBytes = DSA.ToSign(_currentBytes, ref _paramsEDS);
                    CurrentTextFile = Encoding.UTF8.GetString(_currentBytes);
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
