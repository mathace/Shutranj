using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOSpots.Model
{
    [Serializable]
    public class Settings : ViewModelBase
    {
        private string _piopath = String.Empty;
        private bool _isshowrealtimeevaluation = false;
        private bool _isshowStartingranges = false;
        private bool _isshowactualranges = false;
        private bool _isshowStrategy = false;
        public string PIOPath
        {
            get { return _piopath; }
            set { Set(ref _piopath, value); }
        }
        public bool IsShowRealTimeEvaluation
        {
            get { return _isshowrealtimeevaluation; }
            set { Set(ref _isshowrealtimeevaluation, value); }
        }
        public bool IsShowStartingRanges
        {
            get { return _isshowStartingranges; }
            set { Set(ref _isshowStartingranges, value); }
        }
        public bool IsShowActualRanges
        {
            get { return _isshowactualranges; }
            set { Set(ref _isshowactualranges, value); }
        }
        public bool IsShowStrategy
        {
            get { return _isshowStrategy; }
            set { Set(ref _isshowStrategy, value); }
        }

    }
}
