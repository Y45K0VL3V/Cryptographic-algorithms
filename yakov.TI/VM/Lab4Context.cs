using System;
using System.Collections.Generic;
using System.ComponentModel;
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

                OnPropertyChanged("Key P");
            }
        }

        public BigInteger KeyQ
        {
            get => _paramsEDS.q;
            set
            {

                OnPropertyChanged("Key Q");
            }
        }

        public BigInteger KeyH
        {
            get => _paramsEDS.h;
            set
            {

                OnPropertyChanged("Key H");
            }
        }

        public BigInteger KeyX
        {
            get => _paramsEDS.x;
            set
            {

                OnPropertyChanged("Key X");
            }
        }

        public BigInteger KeyK
        {
            get => _paramsEDS.k;
            set
            {

                OnPropertyChanged("Key K");
            }
        }

        public BigInteger PublicKeyY
        {
            get => _paramsEDS.y;
            set
            {

                OnPropertyChanged("Key Y");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
