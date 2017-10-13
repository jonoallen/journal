using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Journal.Properties;

namespace Journal.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            Entries = new ObservableCollection<JournalEntry>();
        }

        private RelayCommand newEntry;
        public RelayCommand NewEntry => newEntry ?? (newEntry = new RelayCommand(() =>
        {
            var entry = new JournalEntry(DateTime.Today);
            setDefaultItems(entry);
            Entries.Insert(0, entry);
            SelectedEntry = entry;
        }));

        private void setDefaultItems(JournalEntry entry)
        {
            var mostRecent = Entries.OrderByDescending(e => e.Date).FirstOrDefault();
            if (mostRecent == null)
                return;

            foreach (var item in mostRecent.Items)
            {
                entry.Items.Add(new Item { Title = item.Title });
            }
        }

        private JournalEntry selectedEntry;

        public JournalEntry SelectedEntry
        {
            get { return selectedEntry; }
            set { selectedEntry = value; RaisePropertyChanged(nameof(SelectedEntry)); }
        }

        public ObservableCollection<JournalEntry> Entries { get; private set; }

        private RelayCommand loaded;
        public RelayCommand Loaded => loaded ?? (loaded = new RelayCommand(() =>
        {
            var path = Settings.Default.journalPath;

            if(!File.Exists(path))
            {
                throw new Exception($"Unable to locate {path}...check the settings file.");
            }
            if (File.Exists(path) && !Entries.Any())
            {
                string text = File.ReadAllText(path);
                var entries = JsonConvert.DeserializeObject<IEnumerable<JournalEntry>>(text);
                Entries = new ObservableCollection<JournalEntry>(entries);
                RaisePropertyChanged(nameof(Entries));
            }
        }));

        private RelayCommand closing;
        public RelayCommand Closing => closing ?? (closing = new RelayCommand(() =>
        {
            var json = JsonConvert.SerializeObject(Entries.OrderByDescending(e => e.Date), Formatting.Indented);
            File.WriteAllText(Settings.Default.journalPath, json);
        }));

    }
}