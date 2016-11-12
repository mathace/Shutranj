using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Watcher
    {
        public FileSystemWatcher fwatcher = new FileSystemWatcher();
        public List<string> watchedfiles = new List<string>();
        public Watcher(string path = @"C:\Users\Tom\Documents\PSHH\Kensix")
        {
            fwatcher.Path = path;
            fwatcher.Created += watcher_Created;
            fwatcher.Changed += watcher_Changed;
            fwatcher.EnableRaisingEvents = true;
        }

        void watcher_Created(object sender, FileSystemEventArgs e)
        {
            watchedfiles.Add(e.FullPath);
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (WatcherChangeTypes.Changed == e.ChangeType)
            {
                var filetext = File.ReadAllText(e.FullPath);
                var thisHH = filetext.Substring(filetext.LastIndexOf("PokerStars Hand"), filetext.Length - filetext.LastIndexOf("PokerStars Hand"));
                var h = new Hand(thisHH);
            }
        }
    }
}
