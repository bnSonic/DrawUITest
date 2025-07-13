using DrawUITest.Pages.Cells;
using DrawUITest.ViewModels;

namespace DrawUITest.Pages;

public partial class TestPage1Bind : ContentPage
{
	MyDataList _myDataList;

	public TestPage1Bind()
	{
		InitializeComponent();

		_myDataList = new MyDataList(MyItemRefreshType.ReplaceWithNewCollection);

		BindingContext = _myDataList;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		//-- load data 
		_myDataList.LoadData();
	}
}

