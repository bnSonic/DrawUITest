using DrawUITest.ViewModels;

namespace DrawUITest.Pages;

public partial class TestPageCollectionView : ContentPage
{
	MyDataList _myDataList;

	public TestPageCollectionView()
	{
		InitializeComponent();

		_myDataList = new MyDataList(MyItemRefreshType.ClearAndAddRange);
		BindingContext = _myDataList;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		//-- load data 
		_myDataList.LoadData();
	}
}