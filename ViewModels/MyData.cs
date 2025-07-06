using System;
using DrawUITest.Components;

namespace DrawUITest.ViewModels;

public enum MyDataType { Group, Data}
public interface IMyData
{
    MyDataType DataType { get; }
    string GroupKey { get; }
    public bool IsVisible { get; set; }
    
    IMyData Parent { get; set; }
}

public class MyData : BaseViewModel, IMyData
{
    public MyDataType DataType => MyDataType.Data;
    public string GroupKey { get; private set; }

    public MyData()
    {

    }
    public MyData(int id, string group, string text1, string text2, string text3)
    {
        Id = id;
        GroupKey = group;
        _text1 = text1;
        _text2 = text2;
        _text3 = text3;

        DurationInHour = 0.1;
    }

    /// <summary>
    /// this is set in the MyDataList - there we need to know which group is tapped
    /// </summary>
    public Command TappedCommand { get; set; }

    private bool _isVisible = false; //default hidden
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            SetProperty(ref _isVisible, value);
        }
    }

    /// <summary>
    /// for the Demo App to show a unique number for each unique data-row
    /// </summary>
    public int Id { get; set; }

    public string StartText => "07:00"; //all the same for demo purposes

    public string Title => string.Empty;
    private string _text1;
    public string Text1
    {
        get => _text1;
        set { SetProperty(ref _text1, value); }
    }

    private string _text2;
    public string Text2
    {
        get => _text2;
        set { SetProperty(ref _text2, value); }
    }

    private string _text3;
    public string Text3
    {
        get => _text3;
        set { SetProperty(ref _text3, value); }
    }

    public bool IsNotRunning { get; set; } = true;
    public string Icon => SvgCache.GetSvgString("boards");
    
    public double DurationInHour { get; set; }

    public string StundenText
    {
        get
        {
            string zeit = string.Empty;

            // if (_aufgabeModel.IsRunning)
            // {
            //     TimeSpan duration = DateTime.Now - _aufgabeModel.Start;
            //     zeit = TimeControllerZeitabschnitte.GetTimeDurationAsText(duration, kompakt: true);
            // }
            // else
            // {
            //--- Aufgaben mit Ende-Datum
            // if (_aufgabeModel.Ende.HasValue)
            // {
            //TimeSpan duration = this.DurationInHour; TimeSpan.FromHours(_aufgabeModel.DurationInHour);
            //zeit = TimeControllerZeitabschnitte.GetTimeDurationAsText(duration, kompakt: true);
            //}
            //     else if (_aufgabeModel.HasPauschalTime)
            //     {
            //         //--- Pauschale 
            //         zeit = TimeControllerZeitabschnitte.GetTimeDurationAsText(_aufgabeModel.PauschalTime.Value.TimeOfDay, kompakt: true);                        
            //     }
            // }
            TimeSpan ts = TimeSpan.FromHours(DurationInHour);
            zeit = $"{ts.Hours}:{ts.Minutes}"; //simplified for this demo
            return zeit;
        }
    }

    public Color BackgroundColor => Colors.WhiteSmoke;
    public Color TextColor => Colors.Black;
    public FontAttributes FontAttributes => FontAttributes.None;

    public IMyData Parent { get; set; }

    public bool HasPrev
    {
        get
        {
            if (Parent == null)
                return false;

            // if (this.IsPauschal)
            //     return false;

            if (Parent is MyGroup group)
            {
                var idx = group.Children.IndexOf(this);
                if (idx <= 0)
                    return false;

                var p = group.Children[idx - 1];
                // if (p is VMTageszettelZeileDummy)
                //     return false;
                // else if (p is VMTageszettelZeile zeile && zeile.IsPauschal)
                //     return false;
                // else
                return true;
            }
            return false;
        }
    }
    public bool HasNext
    {
        get
        {
            if (Parent == null)
                return false;

            // if (this.IsPauschal)
            //     return false;

            if (Parent is MyGroup group)
            {
                // if (IsRunning)
                //     return false;

                var idx = group.Children.IndexOf(this);
                if (idx >= group.Children.Count-1)
                    return false;

                var p = group.Children[idx + 1];
                // if (p is VMTageszettelZeileDummy)
                //     return false;
                // else if (p is VMTageszettelZeile zeile && zeile.IsPauschal)
                //     return false;
                // else
                    return true;
            }
            return false;
        }
    }
}

public class MyGroup : BaseViewModel, IMyData
{
    public MyDataType DataType => MyDataType.Group;
    public List<IMyData> Children { get; private set; }
    public IMyData Parent { get; set; }
    public MyGroup()
    {
        Children = new List<IMyData>();
    }

    public MyGroup(string group, string title) 
    {
         Children = new List<IMyData>();
        _title = title;
        GroupKey = group;
    }

    private bool _isVisible = true; //default visible
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            SetProperty(ref _isVisible, value);
        }
    }

    public string Text1 => string.Empty;
    public string Text2 => string.Empty;
    public string Text3 => string.Empty;

    private string _title;
    public string Title
    {
        get => _title;
        set { SetProperty(ref _title, value); }
    }
    public string GroupKey { get; set; }

    public string ExpandIndicatorSvg
    {
        get => IsExpanded
            ? SvgCache.GetSvgString("arrow_down")
            : SvgCache.GetSvgString("arrow_right");
    }

    public string SvgResIcon
    {
        get
        {
            return SvgMA;
        }
    }
    private string SvgMA => SvgCache.GetSvgString("boards");

    private bool _isExpanded;
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (value != _isExpanded)
            {
                SetProperty(ref _isExpanded, value);
                OnPropertyChanged(nameof(ExpandIndicatorSvg));
            }
        }
    }

    public TimeSpan StundenRES
    {
        get
        {
            double stunden = 0.0;
            var aufgaben = Children.Where(i => i is MyData).ToList();
            aufgaben.ForEach(i =>
            {
                if (i is MyData zeile)
                {
                    stunden += zeile.DurationInHour;
                }
            });
            return TimeSpan.FromHours(stunden);
        }
    }
    public string StundenTextRES
    {
        get
        {
            var s = $"{StundenRES.Hours}:{StundenRES.Minutes}"; //simplified for this demo
            return s;
        }
    }

    /// <summary>
    /// this is set in the MyDataList - there we need to know which group is tapped
    /// </summary>
    public Command GroupTappedCommand { get; set; }
}
