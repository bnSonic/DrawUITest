using System.Collections.ObjectModel;

namespace DrawUITest.Pages;

public partial class TestPage2 : ContentPage
{
	DataList _data;
	public TestPage2()
	{
		InitializeComponent();
		_data = new DataList();
		BindingContext = _data;
	}
}

public class DataList
{
	public ObservableCollection<CellData> Items { get; set; } = new();
	public DataList()
	{
		for (int i = 0; i < 14; i++)
		{
			//Items.Add(new CellData { Text = $"Item {i}" });
			Items.Add(new CellData { Text = $"Item" });
		}
	}
}
public class CellData
{
	public string Text { get; set; }
}