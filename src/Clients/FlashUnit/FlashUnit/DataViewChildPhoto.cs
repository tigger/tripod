// 
// DataViewChildPhoto.cs
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

using Hyena.Data.Gui;
using Hyena.Data;

namespace FlashUnit
{

    public class DataViewChildPhoto : DataViewChild
    {

        public DataViewChildPhoto ()
        {
            //Padding = new Thickness (5);
            //ImageSize = 90;
            //ImageSpacing = 2;
            //TextSpacing = 2;
        }

        public override void Render (CellContext context)
        {
            if (BoundObject == null) {
                return;
            }

            if (!(BoundObject is string)) {
                throw new InvalidCastException ("StringColumn can only bind to string objects");
            }

            string value = (string)BoundObject;

            var layout = context.Layout;
            layout.Ellipsize = Pango.EllipsizeMode.End;



            layout.FontDescription.Weight = Pango.Weight.Bold;
            layout.SetText (value);

           // PangoCairoHelper.ShowLayout (context.Context, layout);
        }

        public override void Arrange ()
        {
            //throw new System.NotImplementedException ();
        }

        public override Hyena.Gui.Canvas.Size Measure (Hyena.Gui.Canvas.Size available)
        {
            //throw new System.NotImplementedException ();
            return new Hyena.Gui.Canvas.Size (50.0, 50.0);
        }



    }
}
