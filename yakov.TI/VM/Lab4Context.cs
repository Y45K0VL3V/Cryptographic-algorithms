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

        public BigInteger KeyP
        {
            get => _paramsEDS.p;
            set
            {
                _paramsEDS.p = value;
                OnPropertyChanged("Key P");
            }
        }

        public BigInteger KeyQ
        {
            get => _paramsEDS.q;
            set
            {
                _paramsEDS.q = value;
                OnPropertyChanged("Key Q");
            }
        }

        public BigInteger KeyH
        {
            get => _paramsEDS.h;
            set
            {
                _paramsEDS.h = value;
                OnPropertyChanged("Key H");
            }
        }

        public BigInteger KeyX
        {
            get => _paramsEDS.x;
            set
            {
                _paramsEDS.x = value;
                OnPropertyChanged("Key X");
            }
        }

        public BigInteger KeyK
        {
            get => _paramsEDS.k;
            set
            {
                _paramsEDS.k = value;
                OnPropertyChanged("Key K");
            }
        }

        public BigInteger PublicKeyY
        {
            get => _paramsEDS.y;
            set
            {
                _paramsEDS.y = value;
                OnPropertyChanged("Key Y");
            }
        }


        #region File interact.

        private byte[] _currentBytes;

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

                StringBuilder temp = new();
                _currentBytes = File.ReadAllBytes(value);
                foreach (byte currByte in _currentBytes)
                {
                    temp.Append(currByte.ToString() + " ");
                }

                //ClearFields();
                //SourceFileDataDec = temp.ToString();

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
                       // File.WriteAllBytes(saveFileDialog.FileName, resultBytes);
                    }
                }));
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
