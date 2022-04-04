using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using yakov.TI.Lab3;

namespace yakov.TI.VM
{
    public enum KeyMode
    {
        None,
        CreateNew,
        UsePublicKey
    }

    public struct ErrorFields
    {
        public ErrorFields()
        {
            (keyP, keyQ, keyN, keyB) = (true, true, true, true);
        }

        public bool keyP, keyQ, keyN, keyB;
    }

    public class Lab3Context : INotifyPropertyChanged
    {
        private ErrorFields _errorFields = new();

        private void ProcessErrors()
        {
            if ((_keyToEnter == KeyMode.CreateNew && !_errorFields.keyP && !_errorFields.keyQ) ||
                ((_keyToEnter == KeyMode.None || _keyToEnter == KeyMode.None) && !_errorFields.keyN))
                IsFirstKeysSet = true;
            else
                IsFirstKeysSet = false;
        }

        public CryptMode Mode { get; set; }

        public bool IsEncryption
        {
            get { return Mode == CryptMode.Encryption; }
            set 
            { 
                if (value)
                {
                    Mode = CryptMode.Encryption;
                    IsPublicCrypt = true;
                    ProcessErrors();
                }
            }
        }

        public bool IsDecryption
        {
            get { return Mode == CryptMode.Decryption; }
            set 
            {
                if (value)
                {
                    Mode = CryptMode.Decryption;
                    IsPublicCrypt = false;
                    ProcessErrors();
                }
            }
        }

        #region Keys activity

        private KeyMode _keyToEnter = KeyMode.None;

        public bool IsPublicCrypt
        {
            get
            {
                if (_keyToEnter == KeyMode.None)
                    return true;

                return _keyToEnter == KeyMode.UsePublicKey;
            }
            set
            {
                _keyToEnter = value ? KeyMode.UsePublicKey : KeyMode.CreateNew;
                OnPropertyChanged("IsPublicCrypt");
            }
        }

        private BigInteger? _privateKeyP;
        public string PrivateKeyP
        {
            get
            {
                return _privateKeyP.ToString();
            }
            set
            {
                try
                {
                    if (value.Length == 0)
                        throw new ArgumentException("Key can't be empty.");

                    if (!BigInteger.TryParse(value, out BigInteger res))
                        throw new ArgumentException("Key must contain only digits.");

                    _privateKeyP = res;

                    if (!RabinHelpMath.IsNumberPrime((BigInteger)_privateKeyP))
                        throw new ArgumentException("Key isn't prime.");

                    if (_privateKeyP % 4 != 3)
                        throw new ArgumentException("Key % 4 must be 3");

                    _errorFields.keyP = false;

                    if (!IsDecryption && !_errorFields.keyP && !_errorFields.keyQ)
                        IsPublicCrypt = false;
                }
                catch(Exception ex)
                {
                    _errorFields.keyP = true;
                    if (!IsDecryption)
                        IsPublicCrypt = true;
                    throw ex;
                }
                finally
                {
                    ProcessErrors();
                    OnPropertyChanged("PrivateKeyP");
                }
            }
        }

        private BigInteger? _privateKeyQ;
        public string PrivateKeyQ
        {
            get
            {
                return _privateKeyQ.ToString();
            }
            set
            {
                try
                {
                    if (value.Length == 0)
                        throw new ArgumentException("Key can't be empty.");

                    if (!BigInteger.TryParse(value, out BigInteger res))
                        throw new ArgumentException("Key must contain only digits.");

                    _privateKeyQ = res;

                    if (!RabinHelpMath.IsNumberPrime((BigInteger)_privateKeyQ))
                        throw new ArgumentException("Key isn't prime.");

                    if (_privateKeyQ % 4 != 3)
                        throw new ArgumentException("Key % 4 must be 3");

                    _errorFields.keyQ = false;

                    if (!IsDecryption && !_errorFields.keyP && !_errorFields.keyQ)
                        IsPublicCrypt = false;
                }
                catch (Exception ex)
                {
                    _errorFields.keyQ = true;
                    if (!IsDecryption)
                        IsPublicCrypt = true;
                    throw ex;
                }
                finally
                {
                    ProcessErrors();
                    OnPropertyChanged("PrivateKeyQ");
                }
            }
        }

        private BigInteger? _publicKeyN;
        public string PublicKeyN
        {
            get
            {
                return _publicKeyN.ToString();
            }
            set
            {
                try
                {
                    if (value.Length == 0)
                        throw new ArgumentException("Key can't be empty.");

                    if (!BigInteger.TryParse(value, out BigInteger res))
                        throw new ArgumentException("Key must contain only digits.");

                    _publicKeyN = res;
                    _errorFields.keyN = false;
                }
                catch (Exception ex)
                {
                    _errorFields.keyN = true;
                    throw ex;
                }
                finally
                {
                    ProcessErrors();
                    OnPropertyChanged("PrivateKeyN");
                }
            }
        }

        private bool _isFirstKeysSet = false;
        public bool IsFirstKeysSet
        {
            get => _isFirstKeysSet;
            set
            {
                _isFirstKeysSet = value;
                OnPropertyChanged("IsFirstKeysSet");
            }
        }

        private BigInteger? _publicKeyB;
        public string PublicKeyB
        {
            get
            {
                return _publicKeyB.ToString();
            }
            set
            {
                try
                {
                    if (value.Length == 0)
                        throw new ArgumentException("Key can't be empty.");

                    if (!BigInteger.TryParse(value, out BigInteger res))
                        throw new ArgumentException("Key must contain only digits.");

                    if ((_keyToEnter == KeyMode.CreateNew && _privateKeyP * _privateKeyQ <= res) ||
                        ((_keyToEnter == KeyMode.UsePublicKey || _keyToEnter == KeyMode.None) && _publicKeyN <= res))
                        throw new ArgumentException("Key must be less, than n (p*q)");

                    _publicKeyB = res;
                    _errorFields.keyB = false;
                }
                catch (Exception ex)
                {
                    _errorFields.keyB = true;
                    throw ex;
                }
                finally
                {
                    OnPropertyChanged("PublicKeyB");
                }
            }
        }
        #endregion

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
                    if (!IsFirstKeysSet || _errorFields.keyB || _fileInputBytes.Length == 0)
                        return;

                    byte[] resultBytes = default;
                    switch (Mode)
                    {
                        case CryptMode.Encryption:
                            if (_keyToEnter == KeyMode.CreateNew)
                            {
                                resultBytes = RabinCrypt.Encrypt((BigInteger)_privateKeyQ, (BigInteger)_privateKeyP, out BigInteger publicN, (BigInteger)_publicKeyB, _fileInputBytes);
                                PublicKeyN = publicN.ToString();
                            }
                            else
                            {
                                resultBytes = RabinCrypt.Encrypt((BigInteger)_publicKeyN, (BigInteger)_publicKeyB, _fileInputBytes);
                            }
                            break;

                        case CryptMode.Decryption:
                            resultBytes = RabinCrypt.Decrypt((BigInteger)_privateKeyQ, (BigInteger)_privateKeyP, (BigInteger)_publicKeyB, _fileInputBytes);
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
