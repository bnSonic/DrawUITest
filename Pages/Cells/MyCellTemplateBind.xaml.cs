using DrawnUi.Draw;
using DrawUITest.ViewModels;

namespace DrawUITest.Pages.Cells;

public partial class MyCellTemplateBind : SkiaLayout
{
	public MyCellTemplateBind()
	{
		InitializeComponent();
	}

	public void xamlGroup_Tapped(object sender, SkiaGesturesParameters skiaGesturesParameters)
	{
		if (BindingContext is MyGroup group)
		{
			group.GroupTappedCommand.Execute(group);
		}
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