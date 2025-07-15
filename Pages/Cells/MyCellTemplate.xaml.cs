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
		xamlMyCellTemplate.IsVisible = item.IsVisible;
		if (!item.IsVisible)
		{
			return;
		}

		if (item.DataType == MyDataType.Group)
		{
			SetGroupContent(item as MyGroup);
			xamlGroup.Update();
			(item as MyGroup).PropertyChanged += item_PropertyChanged;
		}
		else if (item.DataType == MyDataType.Data)
		{
			SetItemContent(item as MyData);
			xamlEntry.Update();
			(item as MyData).PropertyChanged += item_PropertyChanged;
		}

		Update();
	}

	private void item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(IMyData.IsVisible))
		{
			xamlMyCellTemplate.IsVisible = ((IMyData)sender).IsVisible;
			if (!xamlMyCellTemplate.IsVisible)
				xamlMyCellTemplate.HeightRequest = 0;
			else
				xamlMyCellTemplate.HeightRequest = -1; 
			Update();
			
		}
	}

	IMyData _oldItem = null;
	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();

		var item = BindingContext as IMyData;
		if (item != null && item != _oldItem)
		{
			if (_oldItem != null)
			{
				// Unsubscribe from the old item's PropertyChanged event
				if (_oldItem.DataType == MyDataType.Group)
				{
					(_oldItem as MyGroup).PropertyChanged -= item_PropertyChanged;
				}
				else if (_oldItem.DataType == MyDataType.Data)
				{
					(_oldItem as MyData).PropertyChanged -= item_PropertyChanged;
				}
			}

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