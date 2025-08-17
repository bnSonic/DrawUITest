namespace DrawUITest.Pages;

public partial class MainPageFlyout : FlyoutPage
{
	public MainPageFlyout()
	{
		InitializeComponent();
		Flyout = new MainMenuPage();
        IsGestureEnabled = false;
        FlyoutLayoutBehavior = FlyoutLayoutBehavior.Popover;
        Detail = new NavigationPage(new MainPage());
	}
}