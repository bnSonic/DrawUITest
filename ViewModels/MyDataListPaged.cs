using System;
using System.Collections.ObjectModel;

namespace DrawUITest.ViewModels;


public class MyDataListPaged : BaseViewModel
{
    MyItemRefreshType _refreshType;
    public MyDataListPaged(MyItemRefreshType refreshType)
    {
        _refreshType = refreshType;
    }
    
    /// <summary>
    /// this one holds all the Data loaded from database for a view
    /// </summary>
    public List<IMyData> Data = new();

    /// <summary>
    /// this one holds a subset of Data for display in the CollectionView
    /// For example: if you do a search/range-filter or if you open/close groups
    /// the Items Collection is refilled
    /// </summary>
    public BulkObservableCollection<IMyData> Items { get; set; } = new();

    private Command _loadMoreCommand;
    public Command LoadMoreCommand => _loadMoreCommand ??= new Command(() =>
    {
        LoadMore(5);
    });

    private List<string> _text1List = new()
    {
        "The Text 1 for ",
        "Other Text 1 for ",
        "This is some rather long Text 1 because here are more letters that are combined to words and not all fits into one line. Oh it's for the Id: "
    };
    private List<string> _text2List = new()
    {
        string.Empty,
        string.Empty,
        "Some short Text 2 for ",
        string.Empty,
        "different Text 2 for ",
        string.Empty,
        string.Empty,
        "Even the Text 2 can be longer sometimes. this can happen with having a long Text1 and long Text3 or just this one or any combination. everything is possible we have to deal with it. Its for "
    };
    private List<string> _text3List = new()
    {
        string.Empty,
        string.Empty,
        string.Empty,
        "short Text 3 for ",
        string.Empty,
        "Yep, Text 3 can rather long too! this CollectionView is about to display things a user has entered and/or selected and sometimes more Text is neccessary to see all relevant informations at a glance. this ones for "
    };

    /// <summary>
    /// something to load "all" data from database 
    /// </summary>
    public void LoadData()
    {
        Data.Clear();

        int counter = 0;
        //DateTime dt = new DateTime(2025, 06, 26);
        int groupNr = 0;

        var newData = new List<IMyData>();
        while (counter < 100)
        {
            if (counter % 10 == 0)
            {
                //dt = dt.AddDays(1);
                groupNr++;
            }

            var entry = new MyData(
                counter,
                groupNr.ToString(), //dt.ToString("dd.MM.yyyy"),
                counter + ": " + _text1List[counter % _text1List.Count],
                _text2List[counter % _text2List.Count],
                _text3List[counter % _text3List.Count]
            );

            entry.IsVisible = false; //default 
            newData.Add(entry);

            counter++;
        }

        Data = SortAndGroup(newData);

        //RefreshItems(20);
        LoadMore(15);
    }

    /// <summary>
    /// Refills the Items by selecting from Data
    /// </summary>
    public void RefreshItems(int howMany)
    {
        var newItems = new List<IMyData>();

        int counter = 0;
        foreach (var i in Data)
        {
            newItems.Add(i);
            counter++;

            if (counter > howMany)
                break;
        }
        // foreach (var i in Data)
        // {
        //     if (i.IsVisible)
        //         newItems.Add(i);
        // }

        if (_refreshType == MyItemRefreshType.HoldItemsChangeProperties)
        {
            Items = new BulkObservableCollection<IMyData>(newItems);
            OnPropertyChanged(nameof(Items));
        }

    }

    private void LoadMore(int howMany)
    {
        var newItems = new List<IMyData>();

        int counter = 0;
        int startIndex = Items.Count;
        for (int i = startIndex; i < Data.Count && counter < howMany; i++)
        {
            var iData = Data[i];
            newItems.Add(iData);
            counter++;
        }

        if (_refreshType == MyItemRefreshType.HoldItemsChangeProperties)
        {
            Items.AddRange(newItems, clearBeforeAdding: false);
        }
    }

    /// <summary>
    /// sort the Data and insert group entries where needed
    /// </summary>
    public List<IMyData> SortAndGroup(List<IMyData> data)
    {
        var sortedData = data
            .Where(i => i.DataType == MyDataType.Data)
            .OrderBy(i => i.GroupKey)
            .ToList();

        var groupedData = new List<IMyData>();

        string lastGroupKey = string.Empty;
        MyGroup lastGroup = null;
        foreach (var entry in sortedData)
        {
            if (entry.GroupKey != lastGroupKey)
            {
                lastGroup = new MyGroup(entry.GroupKey, entry.GroupKey);
                lastGroup.GroupTappedCommand = this.GroupTappedCommand;
                lastGroup.IsVisible = true; //default
                lastGroup.IsExpanded = true; //default collapsed
                groupedData.Add(lastGroup);

                lastGroupKey = entry.GroupKey;
            }
            entry.IsVisible = true;
            groupedData.Add(entry);
            entry.Parent = lastGroup;
            lastGroup.Children.Add(entry);
        }

        return groupedData;
    }

    private void SetExpandedForGroup(MyGroup group, bool expanded)
    {
        group.IsExpanded = expanded;

        //-- set visibility for children
        group.Children.ForEach(i => i.IsVisible = expanded);

        if (_refreshType == MyItemRefreshType.HoldItemsChangeProperties)
        {
            return;
        }
        
        //-- refresh Items
        //RefreshItems();

        //-- when expanded: scroll to the first child
        // if (expanded)
        // {
        //     if (MyCollectionView != null)
        //     {
        //         var firstChild = group.Children.First();
        //         MyCollectionView.CollectionView.ScrollTo(item: firstChild, position: ScrollToPosition.MakeVisible);
        //     }
        //     else if (MyListView != null)
        //     {
        //         var firstChild = group.Children.First();
        //         MyListView.ListView.ScrollTo(item: firstChild, position: ScrollToPosition.MakeVisible, animated: true);
        //     }
        // }
    }

    private Command _groupTappedCommand;
    public Command GroupTappedCommand => _groupTappedCommand ??= new Command((data) =>
    {
        if (data is MyGroup group)
        {
            SetExpandedForGroup(group, !group.IsExpanded);
        }
    });
}