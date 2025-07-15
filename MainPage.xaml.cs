using DrawUITest.Pages;

namespace DrawUITest;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}


	private async void xamlDemoCodeBehind_Clicked(object sender, EventArgs e)
	{
		var page = new TestPage1CB();
		await Navigation.PushAsync(page);
	}

	private async void xamlDemoBinding_Clicked(object sender, EventArgs e)
	{
		var page = new TestPage1Bind();
		await Navigation.PushAsync(page);
	}

	private async void xamlDemoCollectionView_Clicked(object sender, EventArgs e)
	{
		var page = new TestPageCollectionView();
		await Navigation.PushAsync(page);
	}
	private async void xamlDemoCollectionView2_Clicked(object sender, EventArgs e)
	{
		var page = new TestPageCollectionView2();
		await Navigation.PushAsync(page);
	}
	
	private async void xamlDemoDrawAll_Clicked(object sender, EventArgs e)
	{
		var page = new TestPage1DrawAll();
		await Navigation.PushAsync(page);
	}
}
