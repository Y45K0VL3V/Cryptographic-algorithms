using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yakov.TI.Lab1.Crypt;

namespace yakov.TI.VM
{
    public enum CryptMethod
    {
        ColumnarImproved,
        Decimation,
        Vigenere
    }

    public class Lab1Context: INotifyPropertyChanged
    {
        private string _cryptKey;
        public string CryptKey
        {
            get
            {
                return _cryptKey ?? "";
            }
            set
            {
                _cryptKey = "";
                OnPropertyChanged("CryptKey");
                CheckKeyValid(value.ToUpper());
                _cryptKey = value;
                OnPropertyChanged("CryptKey");
            }
        }

        private string _inputText;
        public string InputText
        {
            get
            {
                return _inputText ?? "";
            }
            set
            {
                _inputText = value;
                OnPropertyChanged("InputText");
            }
        }

        private string _outputText;
        public string OutputText
        {
            get
            {
                return _outputText;
            }
            set
            {
                _outputText = value;
                OnPropertyChanged("OutputText");
            }
        }

        private void CheckKeyValid(string key)
        {
            if (key.Length != 0)
            {
                switch (Method)
                {
                    case CryptMethod.ColumnarImproved:
                        if (ColumnarImprovedEncryption.GetValidChars(key).Length < 1)
                        {
                            throw new ArgumentException("No chars from alphabet.");
                        }
                        break;
                    case CryptMethod.Decimation:
                        if (DecimationEncryption.GetValidKey(key).Length < 1)
                        {
                            throw new ArgumentException("No digits.");
                        }
                        break;
                    case CryptMethod.Vigenere:
                        if (VigenereEncryption.GetValidChars(key).Length < 1)
                        {
                            throw new ArgumentException("No chars from alphabet.");
                        }
                        break;
                }
            }
            else
            {
                throw new ArgumentException("No any chars");
            }
        }

        #region Choose crypt method and mode.
        private CryptMethod _method;
        public CryptMethod Method 
        { 
            get
            {
                return _method;
            }
            set
            {
                _method = value;
                CryptKey = "";
            }
        }

        public bool IsColumnarImproved
        {
            get { return Method == CryptMethod.ColumnarImproved; }
            set { Method = value ? CryptMethod.ColumnarImproved : Method; }
        }

        public bool IsDecimation
        {
            get { return Method == CryptMethod.Decimation; }
            set { Method = value ? CryptMethod.Decimation : Method; }
        }

        public bool IsVigenere
        {
            get { return Method == CryptMethod.Vigenere; }
            set { Method = value ? CryptMethod.Vigenere : Method; }
        }

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
        #endregion

        #region Work with files.
        private string _fileText { get; set; }
        private string _fileOutput { get; set; }

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
                using (StreamReader sr = new StreamReader(value))
                {
                    _fileText = sr.ReadToEnd();
                }
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
                    if (CryptKey != "")
                    {
                        _outputText = DoCrypt(_fileText);
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            File.WriteAllText(saveFileDialog.FileName, _outputText);
                        }
                    }
                }));
            }
        }
        #endregion

        private string DoCrypt(string text)
        {
            if (Mode == CryptMode.Encryption)
            {
                switch (Method)
                {
                    case CryptMethod.ColumnarImproved:
                        return ColumnarImprovedEncryption.Encrypt(text, CryptKey);

                    case CryptMethod.Decimation:
                        return DecimationEncryption.Encrypt(text, CryptKey);

                    case CryptMethod.Vigenere:
                        return VigenereEncryption.Encrypt(text, CryptKey);
                }
            }
            else
            {
                switch (Method)
                {
                    case CryptMethod.ColumnarImproved:
                        return ColumnarImprovedEncryption.Decrypt(text, CryptKey);

                    case CryptMethod.Decimation:
                        return DecimationEncryption.Decrypt(text, CryptKey);

                    case CryptMethod.Vigenere:
                        return VigenereEncryption.Decrypt(text, CryptKey);
                }
            }

            return "";
        }

        private RelayCommand _doCryptCommand;
        public RelayCommand DoCryptCommand
        {
            get
            {
                return _doCryptCommand ?? (_doCryptCommand = new RelayCommand(obj =>
                {
                    if (CryptKey != "")
                        OutputText = DoCrypt(InputText);
                    else
                        OutputText = "";
                }));
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
