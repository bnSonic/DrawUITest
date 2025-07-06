using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DrawUITest.ViewModels;

public class BulkObservableCollection<T> : ObservableCollection<T>
{
    private bool _suppressNotification = false;

    public BulkObservableCollection() : base()
    {

    }
    public BulkObservableCollection(IEnumerable<T> collection) : base(new List<T>(collection ?? throw new ArgumentNullException(nameof(collection))))
    {

    }

    public void AddRange(IEnumerable<T> items, bool clearBeforeAdding = false)
    {
        _suppressNotification = true;
        try
        {
            if (clearBeforeAdding)
                Clear();

            if (items == null)
                return;

            foreach (var item in items)
                Add(item);
        }
        finally
        {
            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }


    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (!_suppressNotification)
            base.OnCollectionChanged(e);
    }
}
