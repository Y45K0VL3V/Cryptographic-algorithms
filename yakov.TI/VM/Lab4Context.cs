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
        delegate void UpdatePropDelegate();
        private void UpdateKeysInfo(string senderKey)
        {
            Dictionary<string, UpdatePropDelegate> keyPropsUpdateMethods = new Dictionary<string, UpdatePropDelegate>()
            {
                ["KeyP"] = () => KeyP = KeyP,
                ["KeyQ"] = () => KeyQ = KeyQ,
                ["KeyH"] = () => KeyH = KeyH,
                ["KeyX"] = () => KeyX = KeyX,
                ["KeyK"] = () => KeyK = KeyK,
                ["PublicKeyY"] = () => PublicKeyY = PublicKeyY,
            };

            //DoKeyAfterSet("KeyP", "p", (byte)KeysValue.p);
            //DoKeyAfterSet("KeyQ", "q", (byte)KeysValue.q);
            //DoKeyAfterSet("KeyH", "h", (byte)KeysValue.h);
            //DoKeyAfterSet("KeyX", "x", (byte)KeysValue.x);
            //DoKeyAfterSet("KeyK", "k", (byte)KeysValue.k);
            //DoKeyAfterSet("PublicKeyY", "y", (byte)KeysValue.y);

            if (_isKeysChanged)
                foreach (string propName in keyPropsUpdateMethods.Keys)
                {
                    if (propName != senderKey)
                        keyPropsUpdateMethods[propName]?.Invoke();
                }
        }

        #region Sign keys validation logic.

        private bool _isKeysChanged = false;

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
            get => (ValidatedKeys & NEEDED_KEYS_TO_SIGN) == NEEDED_KEYS_TO_SIGN;
        }

        public bool IsCheckSignPossible
        {
            get => (ValidatedKeys & NEEDED_KEYS_CHECK_SIGN) == NEEDED_KEYS_CHECK_SIGN;
        }

        private void ModifyValidatedKeys(bool isValid, byte keyValue)
        {
            if (isValid)
                ValidatedKeys |= keyValue;
            else
                ValidatedKeys &= (byte)~keyValue;
        }
        #endregion

        private void DoKeyAfterSet(string propName, string keyName, byte keyValue)
        {
            bool isValid = false;
            try
            {
                isValid = DSAParamsValidator.Validate(_paramsEDS, keyName) ?? false;
            }
            catch
            {
                throw;
            }
            finally
            {
                ModifyValidatedKeys(isValid, keyValue);
                OnPropertyChanged(propName);
            }
        }

        public string KeyP
        {
            get => _paramsEDS.p != 0 ? _paramsEDS.p.ToString() : null;
            set
            {
                BigInteger newValue = BigInteger.Parse(String.IsNullOrEmpty(value) ? "0" : value);
                _isKeysChanged = newValue == _paramsEDS.p ? false : true;
                _paramsEDS.p = newValue;
                try
                {
                    DoKeyAfterSet("KeyP", "p", (byte)KeysValue.p);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    UpdateKeysInfo("KeyP");
                }
            }
        }

        public string KeyQ
        {
            get => _paramsEDS.q != 0 ? _paramsEDS.q.ToString() : null;
            set
            {
                BigInteger newValue = BigInteger.Parse(String.IsNullOrEmpty(value) ? "0" : value);
                _isKeysChanged = newValue == _paramsEDS.q ? false : true;
                _paramsEDS.q = newValue;
                try
                {
                    DoKeyAfterSet("KeyQ", "q", (byte)KeysValue.q);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    UpdateKeysInfo("KeyQ");
                }
            }
        }

        public string KeyH
        {
            get => _paramsEDS.h != 0 ? _paramsEDS.h.ToString() : null;
            set
            {
                BigInteger newValue = BigInteger.Parse(String.IsNullOrEmpty(value) ? "0" : value);
                _isKeysChanged = newValue == _paramsEDS.h ? false : true;
                _paramsEDS.h = newValue;                
                try
                {
                    DoKeyAfterSet("KeyH", "h", (byte)KeysValue.h);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    UpdateKeysInfo("KeyH");
                }
            }
        }

        public string KeyX
        {
            get => _paramsEDS.x != 0 ? _paramsEDS.x.ToString() : null;
            set
            {
                BigInteger newValue = BigInteger.Parse(String.IsNullOrEmpty(value) ? "0" : value);
                _isKeysChanged = newValue == _paramsEDS.x ? false : true;
                _paramsEDS.x = newValue;
                try
                {
                    DoKeyAfterSet("KeyX", "x", (byte)KeysValue.x);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    UpdateKeysInfo("KeyX");
                }
            }
        }

        public string KeyK
        {
            get => _paramsEDS.k != 0 ? _paramsEDS.k.ToString() : null;
            set
            {
                BigInteger newValue = BigInteger.Parse(String.IsNullOrEmpty(value) ? "0" : value);
                _isKeysChanged = newValue == _paramsEDS.k ? false : true;
                _paramsEDS.k = newValue;
                try
                {
                    DoKeyAfterSet("KeyK", "k", (byte)KeysValue.k);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    UpdateKeysInfo("KeyK");
                }
            }
        }

        public string PublicKeyY
        {
            get => _paramsEDS.y != 0 ? _paramsEDS.y.ToString() : null;
            set
            {
                BigInteger newValue = BigInteger.Parse(String.IsNullOrEmpty(value) ? "0" : value);
                _isKeysChanged = newValue == _paramsEDS.y ? false : true;
                _paramsEDS.y = newValue;
                try
                {
                    DoKeyAfterSet("PublicKeyY", "y", (byte)KeysValue.y);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                   UpdateKeysInfo("PublicKeyY"); 
                }
            }
        }
        #endregion

        private byte[] _currentBytes = new byte[] {0};

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

                if (IsCheckSignPossible)
                    CheckSign();
                else
                    SignValidityAnswer = null;

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

        private string _signValidityAnswer = null;
        public string SignValidityAnswer
        {
            get => _signValidityAnswer;
            set
            {
                _signValidityAnswer = value;
                OnPropertyChanged("SignValidityAnswer");
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
                    _isKeysChanged = true;
                    UpdateKeysInfo(null);
                }));
            }
        }

        private RelayCommand _doCheckSignFile;
        public RelayCommand DoCheckSignFile
        {
            get
            {
                return _doCheckSignFile ?? (_doCheckSignFile = new RelayCommand(obj =>
                {
                    CheckSign();
                }));
            }
        }

        private void CheckSign()
        {
            if (DSA.IsSignCorrect(_currentBytes, _paramsEDS, out BigInteger textHash))
            {
                SignValidityAnswer = "Sign valid.";
            }
            else
                SignValidityAnswer = "Sign invalid";

            TextHash = textHash.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
