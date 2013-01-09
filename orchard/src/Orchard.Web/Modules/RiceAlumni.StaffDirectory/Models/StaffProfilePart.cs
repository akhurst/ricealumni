using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Common.Models;
using Orchard.Core.Containers.Models;
using Orchard.Core.Title.Models;

namespace RiceAlumni.StaffDirectory.Models
{
	public class StaffProfilePart : ContentPart<StaffProfilePartRecord>
	{
		public string Name
		{
			get { return this.As<TitlePart>().Title; }
			set { this.As<TitlePart>().Title = value; }
		}

		public string Text
		{
			get { return this.As<BodyPart>().Text; }
			set { this.As<BodyPart>().Text = value; }
		}

		public string StaffGroupName
		{
			get
			{
				var container = this.As<ICommonPart>().Container;
				
				if(container != null)
				{
					return container.As<TitlePart>().Title;
				}
				else
				{
					return null;
				}
			}
		}

		public int StaffGroupWeight
		{
			get
			{
				var container = this.As<ICommonPart>().Container;

				if (container != null)
				{
					return container.As<ContainablePart>().Weight;
				}
				else
				{
					return 0;
				}
			}
		}

		public StaffGroupPart StaffGroupPart
		{
			get { return this.As<ICommonPart>().Container.As<StaffGroupPart>(); }
			set { this.As<ICommonPart>().Container = value; }
		}

		public virtual string Title { get { return Record.Title; } set { Record.Title = value; } }
		public virtual string Email { get { return Record.Email; } set { Record.Email = value; } }
		public virtual string Phone { get { return Record.Phone; } set { Record.Phone = value; } }
	}
}