using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using yakov.TI.Lab2.Crypt;
using yakov.TI.Lab2.KeyGenerators;

namespace yakov.TI.VM
{
    public class Lab2Context: INotifyPropertyChanged
    {
        public Lab2Context()
        {
            _generator = new(27, "x^27+x^8+x^7+x^1+1");
        }

        private LFSR _generator;

        private string _startState;
        public string StartState
        {
            get
            {
                return _startState ?? "";
            }
            set
            {
                _startState = "";
                var temp = LFSR.ParseInput(value);

                if (temp.Length < 1)
                    throw new ArgumentException("Not enough chars");

                if (temp.Length > _generator.RegisterLength)
                    throw new ArgumentException("Register length exceeded");

                _generator.SetRegisterState(value);
                _startState = value;
                OnPropertyChanged("StartState");
            }
        }

        #region Binary crypt info.
        private string _usedKeyBinary;
        public string UsedKeyBinary
        {
            get
            {
                return _usedKeyBinary;
            }
            set
            {
                _usedKeyBinary = value;
                OnPropertyChanged("UsedKeyBinary");
            }
        }

        private string _inputTextBinary;
        public string InputTextBinary
        {
            get
            {
                return _inputTextBinary;
            }
            set
            {
                _inputTextBinary = value;
                OnPropertyChanged("InputTextBinary");
            }
        }

        private string _outputTextBinary;
        public string OutputTextBinary
        {
            get
            {
                return _outputTextBinary;
            }
            set
            {
                _outputTextBinary = value;
                OnPropertyChanged("OutputTextBinary");
            }
        }
        #endregion

        private void ClearFields()
        {
            UsedKeyBinary = "";
            InputTextBinary = "";
            OutputTextBinary = "";
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
                    temp.Append(Convert.ToString(currByte, 2).PadLeft(8, '0') + " ");
                }

                ClearFields();
                InputTextBinary = temp.ToString();

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
                    if (LFSR.ParseInput(StartState).Length >= 1)
                    {
                        byte[] inputBytesCopy = new byte[_fileInputBytes.Length];
                        _fileInputBytes.CopyTo(inputBytesCopy, 0);
                        OutputTextBinary = StreamCrypt.CryptBinary(_generator,ref inputBytesCopy, out string keyBinary);
                        UsedKeyBinary = keyBinary;

                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            File.WriteAllBytes(saveFileDialog.FileName, inputBytesCopy);
                        }
                        _generator.SetRegisterState(StartState);
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
