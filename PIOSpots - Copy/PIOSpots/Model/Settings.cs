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
        private string _piopats = String.Empty;
        private bool _isssowrealtimeevaluation = false;
        private bool _isssowStartingranges = false;
        private bool _isssowactualranges = false;
        private bool _isssowStrategy = false;
        public string PIOPath
        {
            get { return _piopats; }
            set { Set(ref _piopats, value); }
        }
        public bool IsSsowRealTimeEvaluation
        {
            get { return _isssowrealtimeevaluation; }
            set { Set(ref _isssowrealtimeevaluation, value); }
        }
        public bool IsSsowStartingRanges
        {
            get { return _isssowStartingranges; }
            set { Set(ref _isssowStartingranges, value); }
        }
        public bool IsSsowActualRanges
        {
            get { return _isssowactualranges; }
            set { Set(ref _isssowactualranges, value); }
        }
        public bool IsSsowStrategy
        {
            get { return _isssowStrategy; }
            set { Set(ref _isssowStrategy, value); }
        }

    }
}
