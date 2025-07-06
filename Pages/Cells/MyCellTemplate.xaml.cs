using DrawnUi.Draw;
using DrawUITest.ViewModels;

namespace DrawUITest.Pages.Cells;

public partial class MyCellTemplate : SkiaLayout
{
	public MyCellTemplate()
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

	private void SetGroupContent(MyGroup group)
	{
		xamlGroup.IsVisible = true;
		xamlEntry.IsVisible = false;

		xamlGroupTitle.Text = group.Title;
		xamlGroupExpandIndicator.SvgString = group.ExpandIndicatorSvg;
	}

	private void SetItemContent(MyData item)
	{
		xamlGroup.IsVisible = false;
		xamlEntry.IsVisible = true;

		xamlItemText1.IsVisible = !string.IsNullOrEmpty(item.Text1);
		xamlItemText2.IsVisible = !string.IsNullOrEmpty(item.Text2);
		xamlItemText3.IsVisible = !string.IsNullOrEmpty(item.Text3);
		xamlItemText1.Text = item.Text1;
		xamlItemText2.Text = item.Text2;
		xamlItemText3.Text = item.Text3;

		xamlLinePrev.IsVisible = item.HasPrev;
		xamlLineNext.IsVisible = item.HasNext;

		xamlItemIcon.SvgString = item.Icon;
	}

	private void SetContentFull(IMyData item)
	{
		if (item.DataType == MyDataType.Group)
		{
			SetGroupContent(item as MyGroup);
			xamlGroup.Update();
		}
		else if (item.DataType == MyDataType.Data)
		{
			SetItemContent(item as MyData);
			xamlEntry.Update();
		}

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

			// if (item.DataType == MyDataType.Group)
			// {
			// 	xamlGroup.IsVisible = true;
			// 	xamlEntry.IsVisible = false;
			// }
			// else if (item.DataType == MyDataType.Data)
			// {
			// 	xamlGroup.IsVisible = false;
			// 	xamlEntry.IsVisible = true;
			// }

			SetContentFull(item);
		}
	}
}