using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        #region Keys
        private void UpdateKeysInfo()
        {
            OnPropertyChanged("KeyP");
            OnPropertyChanged("KeyQ");
            OnPropertyChanged("KeyH");
            OnPropertyChanged("KeyX");
            OnPropertyChanged("KeyK");
            OnPropertyChanged("PublicKeyY");
        }

        #region Sign keys validation logic.
        private enum KeysValue
        {
            y = 1,
            h = 2,
            q = 4,
            p = 8,
            k = 16,
            x = 32
        }
        
        // Keys set as 1 - x, 2 - k, 3 - p, 4 - q, 5 - h, 6 - y.
        // 111110 : keys p,q,h,x,k must be set.
        private const byte NEEDED_KEYS_TO_SIGN = 0b111110;
        // 001111 : keys p,q,h,y must be set. 
        private const byte NEEDED_KEYS_CHECK_SIGN = 0b001111;

        private byte _validatedKeys = 0;
        public byte ValidatedKeys
        {
            get => _validatedKeys;
            set
            {
                _validatedKeys = value;
                OnPropertyChanged("IsSignPossible");
                OnPropertyChanged("IsCheckSignPossible");
            }
        }

        public bool IsSignPossible
        {
            get => ValidatedKeys == NEEDED_KEYS_TO_SIGN;
        }

        public bool IsCheckSignPossible
        {
            get => ValidatedKeys == NEEDED_KEYS_CHECK_SIGN;
        }

        private void ModifyValidatedKeys(bool isValid, byte keyValue)
        {
            if (isValid)
                ValidatedKeys |= keyValue;
            else
                ValidatedKeys &= (byte)~keyValue;
        }
        #endregion

        public string KeyP
        {
            get => _paramsEDS.p != 0 ? _paramsEDS.p.ToString() : null;
            set
            {
                _paramsEDS.p = BigInteger.Parse(value);
                ModifyValidatedKeys(DSAParamsValidator.Validate(_paramsEDS, "p") ?? false, (byte)KeysValue.p);

                UpdateKeysInfo();
            }
        }

        public string KeyQ
        {
            get => _paramsEDS.q != 0 ? _paramsEDS.q.ToString() : null;
            set
            {
                _paramsEDS.q = BigInteger.Parse(value);
                ModifyValidatedKeys(DSAParamsValidator.Validate(_paramsEDS, "q") ?? false, (byte)KeysValue.q);

                UpdateKeysInfo();
            }
        }

        public string KeyH
        {
            get => _paramsEDS.h != 0 ? _paramsEDS.h.ToString() : null;
            set
            {
                _paramsEDS.h = BigInteger.Parse(value);
                ModifyValidatedKeys(DSAParamsValidator.Validate(_paramsEDS, "h") ?? false, (byte)KeysValue.h);

                UpdateKeysInfo();
            }
        }

        public string KeyX
        {
            get => _paramsEDS.x != 0 ? _paramsEDS.x.ToString() : null;
            set
            {
                _paramsEDS.x = BigInteger.Parse(value);
                ModifyValidatedKeys(DSAParamsValidator.Validate(_paramsEDS, "x") ?? false, (byte)KeysValue.x);

                UpdateKeysInfo();
            }
        }

        public string KeyK
        {
            get => _paramsEDS.k != 0 ? _paramsEDS.k.ToString() : null;
            set
            {
                _paramsEDS.k = BigInteger.Parse(value);
                ModifyValidatedKeys(DSAParamsValidator.Validate(_paramsEDS, "k") ?? false, (byte)KeysValue.k);

                UpdateKeysInfo();
            }
        }

        public string PublicKeyY
        {
            get => _paramsEDS.y != 0 ? _paramsEDS.y.ToString() : null;
            set
            {
                _paramsEDS.y = BigInteger.Parse(value);
                ModifyValidatedKeys(DSAParamsValidator.Validate(_paramsEDS, "y") ?? false, (byte)KeysValue.y);

                UpdateKeysInfo();
            }
        }
        #endregion

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
                CheckSign();
                ////TODO: Auto-checking sign.
                OnPropertyChanged("CurrentTextFile");
            }
        }

        private BigInteger _textHash;
        public string TextHash 
        {
            get
            {
                return _textHash.ToString();
            }
            set
            {
                _textHash = BigInteger.Parse(value ?? "");
                OnPropertyChanged("TextHash");
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
                    CurrentTextFile = Encoding.ASCII.GetString(_currentBytes);
                    UpdateKeysInfo();
                }));
            }
        }

        private void CheckSign()
        {
            if (DSA.IsSignCorrect(_currentBytes, _paramsEDS, out BigInteger textHash))
            {
                Debug.WriteLine("true");
            }
            else
                Debug.WriteLine("false");
            TextHash = textHash.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
