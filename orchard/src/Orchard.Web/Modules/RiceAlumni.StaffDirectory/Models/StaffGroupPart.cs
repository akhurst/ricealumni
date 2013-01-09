using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Containers.Models;
using Orchard.Core.Title.Models;

namespace RiceAlumni.StaffDirectory.Models
{
	public class StaffGroupPart : ContentPart
	{
		public string Name
		{
			get { return this.As<TitlePart>().Title; }
			set { this.As<TitlePart>().Title = value; }
		}

		public string Description
		{
			get { return this.As<BodyPart>().Text; }
			set { this.As<BodyPart>().Text = value; }
		}

		public int Weight
		{
			get { return this.As<ContainablePart>().Weight; }
			set { this.As<ContainablePart>().Weight = value; }
		}
	}
}