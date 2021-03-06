//
// MainWindow.cs
// 
// Author:
//   Ruben Vermeersch <ruben@savanne.be>
// 
// Copyright (c) 2010 Ruben Vermeersch <ruben@savanne.be>
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
using Hyena;
using Hyena.Jobs;
using Tripod.Base;
using Tripod.Jobs;

namespace FlashUnit
{
    public class MainWindow : Window
    {

        #region Layout components

        VBox primary_vbox;

        #endregion

        public MainWindow () : base("Flash Unit")
        {
            HeightRequest = 480;
            WidthRequest = 640;
        }

        private bool interface_constructed;

        protected override void OnShown ()
        {
            if (interface_constructed) {
                base.OnShown ();
                return;
            }
            
            interface_constructed = true;
            
            BuildLayout ();
            base.OnShown ();
        }

        #region Interface Construction

        void BuildLayout ()
        {
            primary_vbox = new VBox ();
            
            var label = new Label ("Super duper test UI!");
            label.Show ();
            primary_vbox.Add (label);
            
            var button_box = new HButtonBox ();
            button_box.Show ();
            primary_vbox.Add (button_box);
            
            var folder_button = new FileChooserButton ("Select import folder", FileChooserAction.SelectFolder);
            folder_button.FileSet += delegate {
                folder = folder_button.Uri;
                Log.Information ("Selected " + folder);
            };
            folder_button.Show ();
            button_box.Add (folder_button);
            
            var import_button = new Button { Label = "Start Import" };
            import_button.Activated += StartImport;
            import_button.Clicked += StartImport;
            import_button.Show ();
            button_box.Add (import_button);

            primary_vbox.Show ();
            Add (primary_vbox);
        }

        #endregion

        string folder;

        void StartImport (object sender, EventArgs args)
        {
            
        }
    }
}

