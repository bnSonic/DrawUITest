using DrawUITest.Pages.Cells;
using DrawUITest.ViewModels;

namespace DrawUITest.Pages;

public partial class TestPage1DrawAll : ContentPage
{
	MyDataListPaged _myDataList;

	public TestPage1DrawAll()
	{
		InitializeComponent();

		_myDataList = new MyDataListPaged(MyItemRefreshType.HoldItemsChangeProperties);

		BindingContext = _myDataList;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		//-- load data 
		_myDataList.LoadData();
		_myDataList.PropertyChanged += myData_PropertyChanged;
		_myDataList.Items.CollectionChanged += Items_CollectionChanged;

		DrawAllRows(_myDataList);
	}

	private void myData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(MyDataList.Items))
		{
			DrawAllRows(_myDataList);
		}
	}	
	private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
	{
		if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
		{
			DrawAllRows(_myDataList);
		}
		else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
		{
			foreach (var item in e.NewItems)
			{
				var cell = new MyCellTemplate() { BindingContext = item };
				xamlLayoutMain.Add(cell);
			}
		}
		else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
		{
			foreach (var item in e.OldItems)
			{
				var cell = xamlLayoutMain.Children.FirstOrDefault(c => c.BindingContext == item);
				if (cell != null)
					xamlLayoutMain.Remove(cell);
			}
		}
	}
	
	private void DrawAllRows(MyDataListPaged data)
	{
		xamlLayoutMain.Children.Clear();
		foreach (var item in data.Items)
		{
			var cell = new MyCellTemplate() { BindingContext = item };
			xamlLayoutMain.Add(cell);
		}
	}
}

