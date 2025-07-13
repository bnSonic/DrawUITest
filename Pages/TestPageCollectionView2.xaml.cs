using DrawUITest.ViewModels;

namespace DrawUITest.Pages;

public partial class TestPageCollectionView2 : ContentPage
{
	MyDataList _myDataList;

	public TestPageCollectionView2()
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

	private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		var view = sender as View;
		if (view == null)
			return;

		if (view.BindingContext is MyGroup group)
		{
			group.GroupTappedCommand?.Execute(group);
		}
    }
}