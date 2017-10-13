using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal
{
    public class JournalEntry : INotifyPropertyChanged
    {
        public JournalEntry()
        {
            Items = new ObservableCollection<Item>();
        }
        public JournalEntry(DateTime date) : this()
        {
            Date = date;
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Date))); }
        }

        public ObservableCollection<Item> Items { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Item
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
