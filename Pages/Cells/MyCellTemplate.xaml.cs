using System.ComponentModel;
using AppoMobi.Specials;
using DrawnUi.Controls;
using DrawnUi.Draw;
using DrawUITest.Components;
using DrawUITest.ViewModels;

namespace DrawUITest.Pages.Cells;

public class AppCell : SkiaDynamicDrawnCell
{
}

public partial class MyCellTemplate : AppCell
{
    public MyCellTemplate()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Dynamic changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void ContextPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.ContextPropertyChanged(sender, e);

        if (BindingContext is MyGroup group)
        {
            if (e.PropertyName == nameof(MyGroup.IsExpanded))
            {
                SetupGroupState(group, true);
            }
        }
    }

    /// <summary>
    /// BindingContext changed
    /// </summary>
    /// <param name="ctx"></param>
    protected override void SetContent(object ctx)
    {
        base.SetContent(ctx);

        var item = ctx as IMyData;
        if (item != null)
        {
            ItemId.Text = $"{this.ContextIndex}";

            if (item.DataType == MyDataType.Group)
            {
                SetGroupContent(item as MyGroup);
            }
            else if (item.DataType == MyDataType.Data)
            {
                SetItemContent(item as MyData);
            }
        }
    }

    private void SetGroupContent(MyGroup group)
    {
        xamlGroup.IsVisible = true;
        xamlEntry.IsVisible = false;

        xamlGroupTitle.Text = group.Title;

        SetupGroupState(group, false);
    }

    void SetupGroupState(MyGroup group, bool needScroll)
    {
        xamlGroupExpandIndicator.SvgString = group.IsExpanded
            ? SvgCache.GetSvgString("arrow_down")
            : SvgCache.GetSvgString("arrow_right");

        xamlGroup.BackgroundColor = group.IsExpanded
            ? xamlGroup.BackgroundColor.WithAlpha(1)
            : xamlGroup.BackgroundColor.WithAlpha(0.85f);

        var count = group.Children.Count;
        if (Parent is SkiaLayout layout)
        {
            layout.ReportChildVisibilityChanged(this.ContextIndex + 1, count, group.IsExpanded);

            if (needScroll)
            {
                if (layout.Parent is SkiaScroll scroll)
                {
                    if (group.IsExpanded)
                    {
                        //scroll to top to take full screen
                        //Tasks.StartDelayed(TimeSpan.FromMilliseconds(50),
                        //    () => { scroll.ScrollToIndex(this.ContextIndex, true, RelativePositionType.Start, true); });

                        //we will know where is our data index only when it really opens
                        //in next frame, so we wait a bit
                        Tasks.StartDelayed(TimeSpan.FromMilliseconds(50),
                            () => 
                            {
                                var firstDataCellIndex = this.ContextIndex + 1;
                                if (layout.LastVisibleIndex <= firstDataCellIndex)
                                {
                                    //show data first row if not visible  
                                    scroll.ScrollToIndex(firstDataCellIndex, true, RelativePositionType.End, true);
                                }
                            });
                    }
                    else
                    {
                        //scroll group header to center when closing  
                        Tasks.StartDelayed(TimeSpan.FromMilliseconds(50),
                            () => { scroll.ScrollToIndex(this.ContextIndex, true, RelativePositionType.Center, true); });
                    }
                }
            }
        }
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
}