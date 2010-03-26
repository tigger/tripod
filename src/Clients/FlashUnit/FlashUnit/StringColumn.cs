// 
// StringColumn.cs
// 
// Author:
//   mike <${AuthorEmail}>
// 
// Copyright (c) 2010 mike
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Gtk;
using Cairo;

using Hyena.Gui;
using Hyena.Data.Gui;
using Hyena.Gui.Theming;

namespace FlashUnit
{
    public class StringColumn : ColumnCell, ITextCell
    {
        private Pango.Weight font_weight = Pango.Weight.Normal;

        public StringColumn () : base (null, true)
        {
        }

        public override Gdk.Size Measure (Widget widget)
        {
            using (var layout = new Pango.Layout (widget.PangoContext)) {
                int lw, lh;
                layout.SetMarkup ("<b>W</b>\n<small><i>W</i></small>");
                layout.GetPixelSize (out lw, out lh);
                return new Gdk.Size (0, lh + 8);
            }
        }

        public override void Render (CellContext context, StateType state, double cellWidth, double cellHeight)
        {
            if (BoundObject == null) {
                return;
            }

            if (!(BoundObject is string)) {
                throw new InvalidCastException ("StringColumn can only bind to string objects");
            }

            string value = (string)BoundObject;

            context.Layout.Width = (int)((cellWidth - 8) * Pango.Scale.PangoScale);
            context.Layout.Ellipsize = Pango.EllipsizeMode.End;
            context.Layout.FontDescription.Weight = font_weight;
            context.Layout.SetMarkup (String.Format ("<b>{0}</b>", value));


            int text_width;
            int text_height;
            context.Layout.GetPixelSize (out text_width, out text_height);

            context.Context.MoveTo (4, ((int)cellHeight - text_height) / 2);
            Cairo.Color color = context.Theme.Colors.GetWidgetColor (
                context.TextAsForeground ? GtkColorClass.Foreground : GtkColorClass.Text, state);
            color.A = (!context.Opaque) ? 0.3 : 1.0;
            context.Context.Color = color;

            PangoCairoHelper.ShowLayout (context.Context, context.Layout);
        }

        public Pango.Weight FontWeight {
            get { return font_weight; }
            set { font_weight = value; }
        }
    }
}
