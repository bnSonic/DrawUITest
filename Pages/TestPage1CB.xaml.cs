using DrawnUi.Draw;
using DrawUITest.Pages.Cells;
using DrawUITest.ViewModels;

namespace DrawUITest.Pages;

public partial class TestPage1CB : ContentPage
{
	MyDataList _myDataList;

	public TestPage1CB()
	{
		InitializeComponent();

		_myDataList = new MyDataList();

		BindingContext = _myDataList;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		//-- load data 
		_myDataList.LoadData();
	}

}

