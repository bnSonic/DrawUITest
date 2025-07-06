using System;
using DrawnUi.Draw;
using DrawUITest.ViewModels;

namespace DrawUITest.Components;

public class MyTapSkiaLayout : SkiaLayout
{
    public override ISkiaGestureListener ProcessGestures(SkiaGesturesParameters args, GestureEventProcessingInfo apply)
    {
        if (args.Type == AppoMobi.Maui.Gestures.TouchActionResult.Tapped)
        {
            if (BindingContext is MyGroup group)
            {
                group.GroupTappedCommand.Execute(group);
            }
        }
        return base.ProcessGestures(args, apply);
    }
}
