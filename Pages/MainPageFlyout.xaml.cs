namespace DrawUITest.Pages;

public partial class MainPageFlyout : FlyoutPage
{
	public MainPageFlyout()
	{
		InitializeComponent();
		Flyout = new MainMenuPage();
		Detail = new NavigationPage(new MainPage());
		IsGestureEnabled = false;
	}
}