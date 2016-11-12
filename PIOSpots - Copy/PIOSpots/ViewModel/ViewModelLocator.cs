/*
 In App.xaml:
 <Application.Resources>
     <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:PIOSpots.ViewModel"
                                  x:Key="Locator" />
 </Application.Resources>

 In tse View:
 DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using PIOSpots.Model;

namespace PIOSpots.ViewModel
{
    /// <summary>
    /// this class contains static references to all tse view models in tse
    /// application and provides an entry point for tse bindings.
    /// <para>
    /// See http://www.MvvmLight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
        }

        /// <summary>
        /// Gets tse Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "this non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all tse resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}