using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yakov.TI.Lab3;

namespace yakov.TI.VM
{
    public class Lab3Context : INotifyPropertyChanged
    {
        public CryptMode Mode { get; set; }

        public bool IsEncryption
        {
            get { return Mode == CryptMode.Encryption; }
            set { Mode = value ? CryptMode.Encryption : Mode; }
        }

        public bool IsDecryption
        {
            get { return Mode == CryptMode.Decryption; }
            set { Mode = value ? CryptMode.Decryption : Mode; }
        }

        private string _privateKeyP;
        public string PrivateKeyP
        {
            get
            {
                return _privateKeyP;
            }
            set
            {
                _privateKeyP = value;
                OnPropertyChanged("PrivateKeyP");
            }
        }

        private string _privateKeyQ;
        public string PrivateKeyQ
        {
            get
            {
                return _privateKeyQ;
            }
            set
            {
                _privateKeyQ = value;
                OnPropertyChanged("PrivateKeyQ");
            }
        }

        private string _publicKeyN;
        public string PublicKeyN
        {
            get
            {
                return _publicKeyN;
            }
            set
            {
                _publicKeyN = value;
                OnPropertyChanged("PublicKeyN");
            }
        }

        private string _publicKeyB;
        public string PublicKeyB
        {
            get
            {
                return _publicKeyB;
            }
            set
            {
                _publicKeyB = value;
                OnPropertyChanged("PublicKeyB");
            }
        }

        #region Crypt info.
        private string _sourceFileDataDec;
        public string SourceFileDataDec
        {
            get
            {
                return _sourceFileDataDec;
            }
            set
            {
                _sourceFileDataDec = value;
                OnPropertyChanged("SourceFileDataDec");
            }
        }

        private string _destFileDataDec;
        public string DestFileDataDec
        {
            get
            {
                return _destFileDataDec;
            }
            set
            {
                _destFileDataDec = value;
                OnPropertyChanged("DestFileDataDec");
            }
        }
        #endregion

        private void ClearFields()
        {
            SourceFileDataDec = "";
            DestFileDataDec = "";
        }

        #region Work with files.
        private byte[] _fileInputBytes;

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
                _fileInputBytes = File.ReadAllBytes(value);
                foreach (byte currByte in _fileInputBytes)
                {
                    temp.Append(currByte.ToString() + " ");
                }

                ClearFields();
                SourceFileDataDec = temp.ToString();

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
                    byte[] resultBytes;
                    switch (Mode)
                    {
                        case CryptMode.Encryption:
                            resultBytes = RabinCrypt.Encrypt();
                            break;

                        case CryptMode.Decryption:
                            resultBytes = RabinCrypt.Decrypt();
                            break;
                    }

                    

                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        File.WriteAllBytes(saveFileDialog.FileName, resultBytes);
                    }
                }));
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
