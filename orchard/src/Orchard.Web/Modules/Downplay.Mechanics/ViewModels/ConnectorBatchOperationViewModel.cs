using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Downplay.Mechanics.ViewModels
{
    public class ConnectorBatchOperationViewModel
    {

        public ConnectorBatchOperationViewModel()
        {
        }

        public string BatchCommand { get; set; }


        public IEnumerable<System.Web.Mvc.SelectListItem> BatchCommandList { get; set; }
    }
}