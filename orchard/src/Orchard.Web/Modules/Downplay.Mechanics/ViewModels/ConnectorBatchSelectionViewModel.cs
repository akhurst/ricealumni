using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Downplay.Mechanics.ViewModels
{
    public class ConnectorBatchSelectionViewModel
    {

        public ConnectorBatchSelectionViewModel()
        {
            Selected = false;
        }

        public bool Selected { get; set; }

    }
}