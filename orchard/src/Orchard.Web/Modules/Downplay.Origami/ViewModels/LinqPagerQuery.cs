using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Downplay.Origami.ViewModels {
    public abstract class LinqPagerQuery<TRecord> : IPagerQuery {

        protected int _startIndex;
        protected int _pageSize;

        protected IQueryable<TRecord> _Query;
        protected bool _IsDirty = true;
        public IQueryable<TRecord> Query {
            get { return _Query; }
            set {
                _IsDirty = true;
            }
        }
        protected IEnumerable<TRecord> _Paged;
        public IEnumerable<TRecord> Paged {
            get {
                if (_Query == null) return null;
                if (_IsDirty) {
                    _Paged = _Query.Skip(_startIndex).Take(_pageSize);
                }
                return _Paged;
            }
        }

        protected int _TotalCount;
        public int TotalCount {
            get {
                _TotalCount = Query.Count();
                return _TotalCount;
            }
        }

        public void SetPage(int startIndex, int pageSize) {
            _startIndex = startIndex;
            _pageSize = pageSize;
            _IsDirty = true;
        }

        protected void Refresh() 
        {
            _IsDirty = false;
            _TotalCount = _Query.Count();
        //    _Paged = 
        }
    }
}