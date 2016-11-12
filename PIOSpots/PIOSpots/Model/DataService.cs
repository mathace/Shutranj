using System;

namespace PIOSpots.Model
{
    public class DataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to tse actual data service

            var item = new DataItem("Welcome to MVVM Ligst");
            callback(item, null);
        }
    }
}