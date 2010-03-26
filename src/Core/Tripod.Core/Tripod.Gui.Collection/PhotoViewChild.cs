// 
// PhotoViewChild.cs
// 
// Author:
//   Mike Gemuende <mike@gemuende.de>
// 
// Copyright (c) 2010 Mike Gemuende
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

using Tripod.Collection;

using Hyena.Gui;
using Hyena.Data.Gui;
using Hyena.Gui.Theming;
using Hyena.Gui.Canvas;

using Gtk;
using Cairo;

namespace Tripod.Gui.Collection
{


    public class PhotoViewChild : DataViewChild
    {

#region Public Layout Properties

        public double ThumbnailWidth { get; set; }
        public double ThumbnailHeight { get; set; }
        public double CaptionSpacing { get; set; }

#endregion


#region Private Layout Values

        private bool valid;
        private Rect inner_allocation;
        private Rect thumbnail_allocation;
        private Rect caption_allocation;
        private string caption;

#endregion


#region Constructors

        public PhotoViewChild ()
        {
            Padding = new Thickness (5);
            ThumbnailWidth = 200;
            ThumbnailHeight = 150;
            CaptionSpacing = 5;
        }

#endregion


#region DataViewChild Implementation

        public override void Render (CellContext context)
        {
            if (inner_allocation.IsEmpty || ! valid)
                return;

            context.Context.Translate (inner_allocation.X, inner_allocation.Y);

            var text_color = context.Theme.Colors.GetWidgetColor (GtkColorClass.Text, context.State);

            var layout = context.Layout;
            layout.Ellipsize = Pango.EllipsizeMode.End;
            layout.Alignment = Pango.Alignment.Center;
            layout.Width = (int)(caption_allocation.Width * Pango.Scale.PangoScale);

            layout.FontDescription.Weight = Pango.Weight.Bold;
            layout.SetText (caption);

            context.Context.Color = text_color;
            context.Context.MoveTo (caption_allocation.X, caption_allocation.Y);
            PangoCairoHelper.ShowLayout (context.Context, layout);
        }

        public override void Arrange ()
        {

            if (BoundObject == null) {
                valid = false;
                return;
            }

            Photo photo = BoundObject as Photo;

            if (photo == null)
                throw new InvalidCastException ("PhotoViewChild can only bind Photo objects");

            valid = true;

            if (photo.Uri != null) {
                caption = photo.Uri.AbsolutePath;
            } else {
                caption = " NULL ";
            }

            double width = ThumbnailWidth;
            double height = ThumbnailHeight;

            inner_allocation = new Rect () {
                X = Padding.Left,
                Y = Padding.Top,
                Width = Allocation.Width - Padding.X,
                Height = Allocation.Height - Padding.Y
            };

            thumbnail_allocation = new Rect () {
                Width = Math.Min (inner_allocation.Width, width),
                Height = Math.Min (inner_allocation.Height, height)
            };

            thumbnail_allocation.X = Math.Round ((inner_allocation.Width - width) / 2.0);

            caption_allocation.Y = thumbnail_allocation.Height + CaptionSpacing;
            caption_allocation.Width = inner_allocation.Width;
        }

        public override Size Measure (Size available)
        {
            var widget = ParentLayout.View;

            var fd = widget.PangoContext.FontDescription;

            fd.Weight = Pango.Weight.Bold;
            caption_allocation.Height = fd.MeasureTextHeight (widget.PangoContext);

            double width = ThumbnailWidth + Padding.X;
            double height = ThumbnailHeight + CaptionSpacing + caption_allocation.Height + Padding.Y;

            return new Size (Math.Round (width), Math.Round (height));
        }

#endregion

//        private Rect inner_allocation;
//        private Rect image_allocation;
//        private Rect first_line_allocation;
//        private Rect second_line_allocation;
//
//        public double ImageSize { get; set; }
//        public double ImageSpacing { get; set; }
//        public double TextSpacing { get; set; }

//        private bool IsGridLayout {
//            // FIXME: Cache this after implementing virtual notification
//            // on ColumnCell that ViewLayout has changed ...
//            get { return ParentLayout is DataViewLayoutGrid; }
//        }

//        public override void Arrange ()
//        {
//            if (!HandleBoundObject (out lines)) {
//                return;
//            }
//
//            inner_allocation = new Rect () {
//                X = Padding.Left,
//                Y = Padding.Top,
//                Width = Allocation.Width - Padding.X,
//                Height = Allocation.Height - Padding.Y
//            };
//
//            double width = ImageSize;
//            double height = ImageSize;
//
//            image_allocation = new Rect () {
//                Width = Math.Min (inner_allocation.Width, width),
//                Height = Math.Min (inner_allocation.Height, height)
//            };
//
//            if (IsGridLayout) {
//                image_allocation.X = Math.Round ((inner_allocation.Width - width) / 2.0);
//            } else {
//                image_allocation.Y = Math.Round ((inner_allocation.Height - height) / 2.0);
//            }
//
//            if (IsGridLayout) {
//                first_line_allocation.Y = image_allocation.Height + ImageSpacing;
//                first_line_allocation.Width = second_line_allocation.Width = inner_allocation.Width;
//            } else {
//                first_line_allocation.X = second_line_allocation.X = image_allocation.Width + ImageSpacing;
//                first_line_allocation.Width = second_line_allocation.Width =
//                    inner_allocation.Width - image_allocation.Width - ImageSpacing;
//            }
//
//            second_line_allocation.Y = first_line_allocation.Bottom + TextSpacing;
//        }

//        public override void Render (CellContext context)
//        {
//            if (inner_allocation.IsEmpty) {
//                return;
//            }
//
//            context.Context.Translate (inner_allocation.X, inner_allocation.Y);
//

            // Render the overlay
//            if (IsGridLayout && prelight_opacity > 0) {
//                var a = prelight_opacity;
//                var cr = context.Context;
//                var grad = new RadialGradient (5, 5, (image_allocation.Width + image_allocation.Height) / 2.0, 5, 5, 0);
//                grad.AddColorStop (0, new Color (0, 0, 0, 0.65 * a));
//                grad.AddColorStop (1, new Color (0, 0, 0, 0.15 * a));
//                cr.Pattern = grad;
//                cr.Rectangle ((Cairo.Rectangle)image_allocation);
//                cr.Fill ();
//                grad.Destroy ();
//
//                /*cr.Save ();
//                cr.LineWidth = 2;
//                cr.Antialias = Cairo.Antialias.Default;
//
//                // math prep for rendering multiple controls...
//                double max_controls = 3;
//                double spacing = 4;
//                double radius = (width - ((max_controls + 1) * spacing)) / max_controls / 2;
//
//                // render first control
//                cr.Arc (width / 2, height - radius - 2 * spacing, radius, 0, 2 * Math.PI);
//
//                cr.Color = new Color (0, 0, 0, 0.4);
//                cr.FillPreserve ();
//                cr.Color = new Color (1, 1, 1, 0.8);
//                cr.Stroke ();
//
//                cr.Restore ();*/
//            }

//            if (lines == null || lines.Length < 2) {
//                return;
//            }
//
//            var text_color = context.Theme.Colors.GetWidgetColor (GtkColorClass.Text, context.State);
//
//            var layout = context.Layout;
//            layout.Ellipsize = Pango.EllipsizeMode.End;
//            layout.Width = (int)(first_line_allocation.Width * Pango.Scale.PangoScale);
//
//            int normal_size = layout.FontDescription.Size;
//            int small_size = (int)(normal_size * Pango.Scale.Small);
//
//            if (!String.IsNullOrEmpty (lines[0])) {
//                layout.FontDescription.Weight = Pango.Weight.Bold;
//                layout.FontDescription.Size = normal_size;
//                layout.SetText (lines[0]);
//
//                context.Context.Color = text_color;
//                context.Context.MoveTo (first_line_allocation.X, first_line_allocation.Y);
//                PangoCairoHelper.ShowLayout (context.Context, layout);
//            }
//
//            if (!String.IsNullOrEmpty (lines[1])) {
//                layout.FontDescription.Weight = Pango.Weight.Normal;
//                layout.FontDescription.Size = small_size;
//                layout.SetText (lines[1]);
//
//                text_color.A = 0.75;
//                context.Context.Color = text_color;
//                context.Context.MoveTo (second_line_allocation.X, second_line_allocation.Y);
//                PangoCairoHelper.ShowLayout (context.Context, layout);
//            }
//
//            layout.FontDescription.Size = normal_size;
//        }

//        public override Size Measure (Size available)
//        {
//            var widget = ParentLayout.View;
//
//            var fd = widget.PangoContext.FontDescription;
//            int normal_size = fd.Size;
//
//            fd.Weight = Pango.Weight.Bold;
//            first_line_allocation.Height = fd.MeasureTextHeight (widget.PangoContext);
//
//            fd.Weight = Pango.Weight.Normal;
//            fd.Size = (int)(fd.Size * Pango.Scale.Small);
//            fd.Style = Pango.Style.Italic;
//            second_line_allocation.Height = fd.MeasureTextHeight (widget.PangoContext);
//
//            fd.Size = normal_size;
//
//            double width, height;
//            double text_height = first_line_allocation.Height + second_line_allocation.Height;
//
//            if (IsGridLayout) {
//                width = ImageSize + Padding.X;
//                height = ImageSize + ImageSpacing + TextSpacing + text_height + Padding.Y;
//            } else {
//                double list_text_height = text_height + TextSpacing;
//                width = ImageSize + ImageSpacing + Padding.Y;
//                height = (list_text_height < ImageSize ? ImageSize : list_text_height) + Padding.X;
//            }
//
//            return new Size (Math.Round (width), Math.Round (height));
//        }

//        protected void InvalidateImage ()
//        {
//            var damage = thu;
//            damage.Offset (inner_allocation);
//            Invalidate (damage);
//        }

//        protected virtual bool HandleBoundObject (out string [] lines)
//        {
//            lines = null;
//
//            var album = BoundObject as Photo;
//            if (album == null) {
//                if (BoundObject == null) {
//                    return false;
//                };
//
//                throw new InvalidCastException ("ColumnCellAlbum can only bind to Photo objects");
//            }
//
//            lines = new [] { album.Url };
//
//            return true;
//        }

        public override void CursorEnterEvent ()
        {
        }

        public override void CursorLeaveEvent ()
        {
        }

    }
}
