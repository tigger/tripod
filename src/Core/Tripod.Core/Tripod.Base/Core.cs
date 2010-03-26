//
// Core.cs
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

using Gtk;
using System;
using Hyena.Jobs;
using Hyena.Data.Sqlite;

using Tripod.Collection.Database;


namespace Tripod.Base
{
    public class Core
    {
        static readonly Scheduler scheduler = new Scheduler ();
        public static Scheduler Scheduler {
            get { return scheduler; }
        }

        public static void Initialize (string name, ref string[] args)
        {
            Hyena.Log.Debugging = true;
            GLib.Log.SetLogHandler ("Gtk", GLib.LogLevelFlags.Critical, GLib.Log.PrintTraceLogFunction);
            
            Hyena.Log.Debug ("Initializing Core");
            
            Application.Init (name, ref args);

            InitializeDatabase ();
        }

        public static SqliteModelProvider<DatabasePhoto> PhotoProvider { get; private set; }
        public static HyenaSqliteConnection DatabaseConnection { get; private set; }

        private static void InitializeDatabase ()
        {
            DatabaseConnection = new HyenaSqliteConnection("test.db");
            PhotoProvider = new SqliteModelProvider<DatabasePhoto>(DatabaseConnection, "Photos");

            if (PhotoProvider.FetchAll ().GetEnumerator ().Current == null)
                InitializePhotos ();
        }

        private static void InitializePhotos ()
        {
            string [] paths = {
                "/photo1.jpg",
                "/photo2.jpg",
                "/photo3.jpg",
                "/photo4.jpg",
                "/photo5.jpg",
                "/photo6.jpg",
                "/photo7.jpg",
                "/photo8.jpg",
                "/photo9.jpg",
                "/photo10.jpg"
            };

            foreach (string path in paths) {
                var photo = new DatabasePhoto (new SafeUri (path));
                PhotoProvider.Save (photo);
            }
        }
    }
}