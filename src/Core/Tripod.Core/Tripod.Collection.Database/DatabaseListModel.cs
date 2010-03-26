//
// DatabaseListModel.cs
//
// Author:
//   Aaron Bockover <abockover@novell.com>
//   Gabriel Burt <gburt@novell.com>
//
// Copyright (C) 2007 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Text;
using System.Collections.Generic;

using Hyena;
using Hyena.Data;
using Hyena.Data.Sqlite;
using Hyena.Query;


namespace Tripod.Collection.Database
{
    public class DatabaseListModel<T, U> : BaseListModel<U>, ICacheableDatabaseModel
        where T : U, new () where U : ICacheableItem, new()
    {
        private readonly HyenaSqliteConnection connection;
        private readonly SqliteModelCache<T> cache;

        public DatabaseListModel (string name, string label, /*Banshee.Sources.DatabaseSource source,*/
                                        HyenaSqliteConnection connection, SqliteModelProvider<T> provider, string uuid)
            : base ()
        {
            this.connection = connection;
            cache = new SqliteModelCache<T> (connection, uuid, this, provider);
            cache.HasSelectAllItem = false;
        }


#region Implement BaseListModel

        public override void Clear()
        {
            //cache.Clear ();
        }

        public override void Reload()
        {
            lock (cache) {
                connection.BeginTransaction ();
                cache.SaveSelection ();
                cache.Reload ();
                cache.UpdateAggregates ();
                cache.RestoreSelection ();
                connection.CommitTransaction ();
            }
        }

        public override U this[int index] {
            get {
                lock (cache) {
                    return cache.GetValue (index);
                }
            }
        }

        public override int Count {
            get { return (int) cache.Count; }
        }

#endregion


#region Implement ICachableDatabaseModel

        public virtual string ReloadFragment { get { return null; } }
        public virtual string SelectAggregates { get { return null; } }
        public virtual string JoinTable { get { return null; } }
        public virtual string JoinFragment { get { return null; } }
        public virtual string JoinPrimaryKey { get { return null; } }
        public virtual string JoinColumn { get { return null; } }
        public virtual bool CachesJoinTableEntries { get { return false; } }
        public virtual bool CachesValues { get { return false; } }

#endregion


#region Implement ICachableDatabaseModel

        public virtual int FetchCount { get { return 40; } }

#endregion

    }
}