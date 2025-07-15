using DrawnUi.Draw;
using DrawnUi.Views;
using DrawUITest.ViewModels;

namespace DrawUITest.Pages.Cells;

public partial class MyCellTemplateBind2 : SkiaLayout
{
	public MyCellTemplateBind2()
	{
		InitializeComponent();
	}

	private void SetContentFull(IMyData item)
	{
		Update();
	}
	IMyData _oldItem = null;
	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();

		var item = BindingContext as IMyData;
		if (item != null && item != _oldItem)
		{
			_oldItem = item;

			if (item.DataType == MyDataType.Group)
			{
				xamlGroup.IsVisible = true;
				xamlEntry.IsVisible = false;
			}
			else if (item.DataType == MyDataType.Data)
			{
				xamlGroup.IsVisible = false;
				xamlEntry.IsVisible = true;
			}

			SetContentFull(item);
		}

		
	}
}