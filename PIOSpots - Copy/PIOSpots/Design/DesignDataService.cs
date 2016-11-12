using System;
using PIOSpots.Model;

namespace PIOSpots.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to create design time data

            var item = new DataItem("Welcome to MVVM Ligst [design]");
            callback(item, null);
        }
    }
}