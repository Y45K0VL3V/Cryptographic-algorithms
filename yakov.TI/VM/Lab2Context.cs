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
                StringBuilder stateStr = new();
                foreach (Match match in Regex.Matches(value, "[01]+"))
                {
                    stateStr.Append(match.Value);
                }

                if (stateStr.Length < 1)
                    throw new ArgumentException("Not enough chars");

                if (stateStr.Length > _generator.RegisterLength)
                    throw new ArgumentException("Register length exceeded");

                _generator.SetRegisterState(Convert.ToInt64(stateStr.ToString(), 2));
                _startState = stateStr.ToString();
                OnPropertyChanged("StartState");
            }
        }

        #region Crypt info
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
        #endregion

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
            OutputText = "";
            InputText = "";
            InputTextBinary = "";
            OutputTextBinary = "";
        }

        private string CryptBinaryMethod(string input)
        {
            string result = null;
            if (StartState != "")
            {
                _generator.SetRegisterState(Convert.ToInt64(StartState, 2));
                var inputBytes = new List<byte>();
                foreach (string strByte in input.Split(' '))
                    inputBytes.Add(Convert.ToByte(strByte, 2));

                result = StreamCrypt.CryptBinary(_generator, inputBytes.ToArray(), out string keyBinary);
                UsedKeyBinary = keyBinary;
            }
            else
            {
                ClearFields();
            }

            return result ?? "";
        }

        private RelayCommand _doCryptCommand;
        public RelayCommand DoCryptCommand
        {
            get
            {
                return _doCryptCommand ?? (_doCryptCommand = new RelayCommand(obj =>
                {
                    if (StartState != "")
                    {
                        _generator.SetRegisterState(Convert.ToInt64(StartState, 2));
                        OutputText = StreamCrypt.Crypt(_generator, InputText, out string keyBinary, out string inputBinary, out string outputBinary);
                        UsedKeyBinary = keyBinary;
                        InputTextBinary = inputBinary;
                        OutputTextBinary = outputBinary;
                    }
                    else
                    {
                        ClearFields();
                    }
                }));
            }
        }

        private RelayCommand _doBinaryCryptCommand;
        public RelayCommand DoBinaryCryptCommand
        {
            get
            {
                return _doBinaryCryptCommand ?? (_doBinaryCryptCommand = new RelayCommand(obj =>
                {
                    OutputTextBinary = CryptBinaryMethod(InputTextBinary);
                }));
            }
        }

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
                    if (StartState != "")
                    {
                        _outputText = CryptBinaryMethod(_fileText);
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
