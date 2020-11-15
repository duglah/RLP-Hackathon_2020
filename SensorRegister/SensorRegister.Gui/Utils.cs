using System;
using Terminal.Gui;

namespace SensorRegister.Gui.Utils
{
    public class ViewBuilder
    {
        private readonly View _parent;
        private View _last;
        private int _marginHorizontal;
        private int _marginVertical;

        public View View
        {
            get => _parent;
        }

        ViewBuilder(View parent, int marginHorizontal = 0, int marginVertical = 0)
        {
            _parent = parent;
            _marginHorizontal = marginHorizontal;
            _marginVertical = marginVertical;
        }

        public static ViewBuilder Create(Dim width = null, Dim height = null)
        {
            return new ViewBuilder(new View()
            {
                Width = width ?? Dim.Fill(),
                Height = height ?? Dim.Fill(),
            });
        }

        public View Build(View parent)
        {
            parent.Add(_parent);
            return _parent;
        }

        public ViewBuilder SetHorizontalMargin(int margin)
        {
            _marginHorizontal = margin;
            return this;
        }

        public ViewBuilder SetVerticalMargin(int margin)
        {
            _marginVertical = margin;
            return this;
        }

        public ViewBuilder Add(View view, Pos x = null, Pos y = null, Dim width = null, Dim height = null)
        {
            view.X = x ?? (_last != null ? Pos.X(_last) : 0) + _marginHorizontal;
            view.Y = y ?? (_last != null ? Pos.Y(_last) : 0) + _marginVertical;

            view.Width = width ?? view.Width;
            view.Height = height ?? view.Height ?? 1;

            _last = view;
            _parent.Add(view);

            return this;
        }

        public ViewBuilder Add(Func<ViewBuilder, ViewBuilder> modify)
        {
            return modify(this);
        }

        public ViewBuilder Below(View view, Pos xOffset = null, Dim width = null, Dim height = null)
        {
            return Add(view, (xOffset ?? 0) + _marginHorizontal, Pos.Bottom(_last), width, height);
        }

        public ViewBuilder BelowIt(View view, Pos xOffset = null, Dim width = null, Dim height = null)
        {
            return Add(view, Pos.Left(_last) + (xOffset ?? 0), Pos.Bottom(_last), width, height);
        }

        // public ViewBuilder AboveIt(View view, Pos xOffset = null, Dim width = null, Dim height = null)
        // {
        //     return Add(view, Pos.Left(_last)+ (xOffset ?? 0), Pos.Top(_last) - view.Frame.Height, width, height);
        // }


        public ViewBuilder ToTheRight(View view, Pos yOffset = null, Dim width = null, Dim height = null)
        {
            return Add(view, Pos.Right(_last), Pos.Y(_last) + (yOffset ?? 0), width, height);
        }

        // public ViewBuilder ToTheLeft(View view, Pos yOffset = null, Dim width = null, Dim height = null)
        // {
        //     return Add(view, Pos.Left(_last) - view.Frame.Width, Pos.Y(_last) + (yOffset ?? 0), width, height);
        // }
    }
}