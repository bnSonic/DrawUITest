using System;
using System.Diagnostics;
using DrawnUi.Draw;
using ShimSkiaSharp;

namespace DrawUITest.Components;

public class MySwipeView : SkiaLayout
{
    public MySwipeView() : base()
    {
        Tag = "MySwipeView";
        UseCache = SkiaCacheType.Operations;
        Type = LayoutType.Grid;
        HorizontalOptions = LayoutOptions.Fill;
        RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
    }

    public override ISkiaGestureListener ProcessGestures(SkiaGesturesParameters args, GestureEventProcessingInfo apply)
    {
        if (args.Type == AppoMobi.Maui.Gestures.TouchActionResult.Down)
        {

        }
        if (args.Type == AppoMobi.Maui.Gestures.TouchActionResult.Panning)
        {
            var startPos = args.Event.StartingLocation;
            var dist = args.Event.Distance;
            var pos = args.Event.Location;
            Trace.WriteLine($"+++ Distance Total X/Y: {dist.Total.X} / {dist.Total.Y} | {dist.Velocity}");
        }
        if (args.Type == AppoMobi.Maui.Gestures.TouchActionResult.Up)
        {
            if (args.Event.Distance.Total.X < -100)
                _ = SwipeLeftAsync(true);
            else if (args.Event.Distance.Total.X > 100)
                _ = SwipeRightAsync(true);
        }

        return base.ProcessGestures(args, apply);
    }

    //===== Bindable for ContextView =========================
    public static readonly BindableProperty ContextViewProperty = BindableProperty.Create(
           nameof(ContextView), typeof(SkiaLayout), typeof(MySwipeView), null,
           propertyChanged: OnContextViewPropertyChanged);

    static void OnContextViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var me = ((MySwipeView)bindable);
        if (me == null)
            return;

        var contextView = newValue as SkiaLayout;

        // Entferne alte ContextView, falls vorhanden
        if (oldValue is SkiaLayout oldContext && me.Children.Contains(oldContext))
            me.Children.Remove(oldContext);

        if (contextView != null)
        {
            // ContextView immer an Index 0 einfügen
            me.Children.Insert(0, contextView);
            Grid.SetColumn(contextView, 0);

            //Default invisible; Will be visible if revealed (see SwipeLeftAsync)
            //contextView.IsVisible = false; 
        }
    }

    public SkiaLayout ContextView
    {
        get => (SkiaLayout)GetValue(ContextViewProperty);
        set => SetValue(ContextViewProperty, value);
    }


    //===== Bindable for MainView =========================
    public static readonly BindableProperty MainViewProperty = BindableProperty.Create(
           nameof(MainView), typeof(SkiaLayout), typeof(MySwipeView), null,
           propertyChanged: OnMainViewPropertyChanged);

    static void OnMainViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var me = ((MySwipeView)bindable);
        if (me == null)
            return;

        var mainView = newValue as SkiaLayout;

        // Entferne alte MainView, falls vorhanden
        if (oldValue is SkiaLayout oldMain && me.Children.Contains(oldMain))
            me.Children.Remove(oldMain);

        if (mainView != null)
        {
            // MainView immer an Index 1 einfügen (über ContextView)
            // Falls ContextView noch nicht da ist, einfach anhängen
            if (me.Children.Count == 0)
                me.Children.Add(mainView);
            else if (me.Children.Count == 1)
                me.Children.Add(mainView);
            else
                me.Children.Insert(1, mainView);

            Grid.SetColumn(mainView, 0);
        }
    }

    public SkiaLayout MainView
    {
        get => (SkiaLayout)GetValue(MainViewProperty);
        set => SetValue(MainViewProperty, value);
    }

    private MyMainSkiaStack MyMainSkiaStack
    {
        get
        {
            var parent = this.Parent;
            while (parent != null)
            {
                if (parent is MyMainSkiaStack v)
                    return v;

                if (parent is SkiaLayout sl)
                    parent = sl.Parent;
                else
                    break;
            }
            return null;
        }
    }

    protected override async void OnBindingContextChanged()
    {
        try
        {
            if (_isRevealed)
            {
                await SwipeRightAsync(false);
            }
        }
        catch
        { }

        base.OnBindingContextChanged();
    }

    private bool _isRevealed = false;
    private Easing _easingOpen = Easing.CubicOut; //Easing.SpringOut;
    private Easing _easingClose = Easing.CubicOut;
    private uint _durationOpen = 200;
    private uint _durationClose = 200;

    //private Command _swipeLeftCommand;

    /// <summary>
    /// Parameter: true wenn animiert werden soll, sonst false
    /// </summary>
    //public Command SwipeLeftCommand => _swipeLeftCommand ??= new Command(async (animated) =>
    public async Task SwipeLeftAsync(bool animated)
    {
        if (_isRevealed)
            return;

        var myMainLayout = this.MyMainSkiaStack;

        MySwipeView view = myMainLayout != null
            ? myMainLayout.CurrentRevealedSwipeGrid as MySwipeView
            : null;

        //-- schon ein anderes Grid revealed? dann dieses schließen
        if (view != null)
        {
            _ = view.CloseMenu();
        }

        //-- Breite der Buttons
        //this.ContextView.IsVisible = true;
        /* setting IsVisible of the ContextView to true at this point doesn't work
           Width is still -1 then
           this.Update() / this.Invalidate() didn't help
           ContextView.Update() / ContextView.Invalidate didn't help
           Maybe there is on of the .Measure… function that might help? 
        */
        var x = this.ContextView != null ? this.ContextView.Width : 0;

        //-- CellView nach links verschieben um Buttons sichtbar zu machen
        if (this.MainView != null)
        {
            await this.MainView.TranslateTo(x * -1, 0, length: _durationOpen, easing: _easingOpen);
            _isRevealed = true;

            if (MyMainSkiaStack != null)
            {
                MyMainSkiaStack.CurrentRevealedSwipeGrid = this;
            }
        }
    }

    //private Command _swipeRightCommand;

    /// <summary>
    /// Parameter: true wenn animiert werden soll, sonst false
    /// </summary>
    //public Command SwipeRightCommand => _swipeRightCommand ??= new Command(async (animated) =>
    public async Task SwipeRightAsync(bool animated)
    {
        if (!_isRevealed)
            return;

        bool anim = (bool)animated;
        if (this.MainView != null)
        {
            if (anim)
            {
                //-- CellView nach links verschieben um Buttons sichtbar zu machen
                await this.MainView.TranslateTo(0, 0, length: _durationClose, easing: _easingClose);
            }
            else
            {
                this.MainView.TranslationX = 0;
            }
            //this.ContextView.IsVisible = false; 
        }
        _isRevealed = false;

        var myMainLayout = this.MyMainSkiaStack;

        if (myMainLayout != null)
        {
            myMainLayout.CurrentRevealedSwipeGrid = null;
        }
    }
    
    private async Task CloseMenu(bool animated = true)
    {
        if (!_isRevealed)
            return;

        if (this.MainView != null)
        {
            if (animated)
            {
                //-- CellView nach links verschieben um Buttons sichtbar zu machen
                await this.MainView.TranslateTo(0, 0, length:_durationClose, easing: _easingClose);
            }
            else
            {
                this.MainView.TranslationX = 0;
            }
        }
        _isRevealed = false;
    }
}
