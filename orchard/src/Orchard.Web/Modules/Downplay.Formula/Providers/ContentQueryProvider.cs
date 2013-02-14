using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Formula.Services;
using Orchard.ContentManagement;

namespace Downplay.Formula.Providers {
    public class ContentQueryProvider : IQueryCacheProvider {



        public void Describe(QueryDescribe describe) {
         /*   describe.For<KeywordQuery>(q=>{
                q.Unique(k => k.Text);

            });*/
        }
    }
}
