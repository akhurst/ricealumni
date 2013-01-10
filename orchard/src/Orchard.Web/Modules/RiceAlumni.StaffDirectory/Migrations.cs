using System.Collections.Generic;
using System.Linq;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Models;
using Orchard.Core.Contents.Extensions;
using Orchard.Core.Navigation.Models;
using Orchard.Core.Navigation.Services;
using Orchard.Core.Title.Models;
using Orchard.Data.Migration;
using Orchard.Projections.Models;
using Orchard.Projections.Services;
using Orchard.Security;

namespace RiceAlumni.StaffDirectory 
{
	public class Migrations : DataMigrationImpl
	{
		private readonly IContentManager _contentManager;
		private readonly IQueryService _queryService;
		private readonly IMenuService _menuService;
		private readonly IMembershipService _membershipService;

		public Migrations(IMenuService menuService, IContentManager contentManager, IQueryService queryService, IMembershipService membershipService)
		{
			_menuService = menuService;
			_contentManager = contentManager;
			_queryService = queryService;
			_membershipService = membershipService;
		}

		public int Create()
		{
			SchemaBuilder.CreateTable("StaffProfilePartRecord", table => table
				.ContentPartRecord()
				.Column<string>("Title")
				.Column<string>("Email")
				.Column<string>("Phone")
				);

			ContentDefinitionManager.AlterTypeDefinition("StaffGroup", builder => builder
				.Creatable()
				.WithPart("StaffGroupPart")
				//.WithPart("CommonPart", partBuilder => partBuilder.WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false"))
				.WithPart("CommonPart")
				.WithPart("TitlePart")
				.WithPart("BodyPart", partBuilder => partBuilder.WithSetting("BodyTypePartSettings.Flavor", "text"))
				.WithPart("ContainablePart")
				.WithPart("ContainerPart")
				.WithPart("AutoroutePart", cfg => cfg
						.WithSetting("AutorouteSettings.AllowCustomPattern", "false")
						.WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
						.WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Staff Group', Pattern: 'staff/{Content.Slug}', Description: 'staff/group-name'}]")
						.WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
				);

			ContentDefinitionManager.AlterPartDefinition("ImageField", part => part
				.WithField("Image", field => field
					.OfType("ImageField")
					.WithSetting("ImageFieldSettings.Width", "500")
					.WithSetting("ImageFieldSettings.Height", "500")
					.WithSetting("ImageFieldSettings.ResizeAction", "Crop")
					)
				);

			ContentDefinitionManager.AlterTypeDefinition("StaffProfile", builder => builder
				.Draftable()
				.Creatable()
				.WithPart("StaffProfilePart")
				.WithPart("CommonPart")
				.WithPart("TitlePart")
				.WithPart("BodyPart")
				.WithPart("ImageField")
				.WithPart("MenuPart")
				.WithPart("AdminMenuPart")
				.WithPart("ContainablePart")
				.WithPart("AutoroutePart", cfg => cfg
						.WithSetting("AutorouteSettings.AllowCustomPattern", "false")
						.WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
						.WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Group and Name', Pattern: '{Content.Container.Path}/{Content.Slug}', Description: 'staff-group/name'}]")
						.WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
				);

			var staffProfileQuery = CreateStaffProfileQuery();
			CreateStaffProfileProjection(staffProfileQuery);

			return 1;
		}

		private void CreateStaffProfileProjection(QueryPart staffProfileQuery)
		{
			var projection = _contentManager.Create<ProjectionPart>("ProjectionPage",
			                                                        t => { t.Record.QueryPartRecord = staffProfileQuery.Record; });
			projection.As<TitlePart>().Record.Title = "Staff Directory";
			projection.As<MenuPart>().Record.MenuText = "Staff Directory";
			projection.As<MenuPart>().Record.MenuPosition = "1";
			var mainMenu = _menuService.GetMenu("Main Menu");
			if(mainMenu != null)
				projection.As<MenuPart>().Record.MenuId = _menuService.GetMenu("Main Menu").Id;
			projection.As<AutoroutePart>().Record.DisplayAlias = "staff-directory";
			projection.As<CommonPart>().Record.OwnerId = _membershipService.GetUser("admin").Id;

			//publish the staff directory projection
			_contentManager.Publish(projection.ContentItem);
		}

		private QueryPart CreateStaffProfileQuery()
		{
			var staffProfileQuery = _queryService.CreateQuery("Staff Directory");

			// filter the query to only include staff profiles
			FilterStaffProfileQuery(staffProfileQuery);

			// sort the query
			SortStaffProfileQuery(staffProfileQuery);

			// customize the display of the query
			LayoutStaffProfileQuery(staffProfileQuery);

			// publish the query
			_contentManager.Publish(staffProfileQuery.ContentItem);
			return staffProfileQuery;
		}

		private void LayoutStaffProfileQuery(QueryPart staffProfileQuery)
		{
			var layoutDictionary = new Dictionary<string, string>();
			layoutDictionary.Add("QueryId", staffProfileQuery.Id.ToString());
			layoutDictionary.Add("Category", "Html");
			layoutDictionary.Add("Type", "List");
			layoutDictionary.Add("Description", "List");
			layoutDictionary.Add("Display", "0");
			layoutDictionary.Add("DisplayType", "Summary");

			LayoutRecord layout = staffProfileQuery.Layouts.FirstOrDefault();
			if (layout == null)
			{
				layout = new LayoutRecord()
				{
					Category = "Html",
					Type = "List",
					Description = "List",
					Display = 0,
					DisplayType = "Summary",
				};
				staffProfileQuery.Layouts.Add(layout);
			}
			layout.State = FormParametersHelper.ToString(layoutDictionary);
		}

		private static void SortStaffProfileQuery(QueryPart staffProfileQuery)
		{
			var sortDictionary = new Dictionary<string, string>();
			sortDictionary.Add("Description", "Staff Group");
			sortDictionary.Add("Sort", "false");

			//sort by name
			var profileNameCriteria = staffProfileQuery.SortCriteria.FirstOrDefault();
			if (profileNameCriteria == null)
			{
				profileNameCriteria = new SortCriterionRecord
				                      {
					                      Category = "TitlePartRecord",
					                      Type = "Title",
					                      Description = "Profile Name",
					                      Position = 1
				                      };
			}
			profileNameCriteria.State = FormParametersHelper.ToString(sortDictionary);
			staffProfileQuery.SortCriteria.Add(profileNameCriteria);
		}

		private static void FilterStaffProfileQuery(QueryPart staffProfileQuery)
		{
			var filterDictionary = new Dictionary<string, string>();
			filterDictionary.Add("ContentTypes", "StaffProfile");
			filterDictionary.Add("Description", "Staff Profiles");
			var state = FormParametersHelper.ToString(filterDictionary);
			staffProfileQuery.FilterGroups[0].Filters.Add(new FilterRecord
			                                              {
				                                              Category = "Content",
				                                              Description = "Staff Profiles",
				                                              Position = 0,
				                                              State = state,
				                                              Type = "ContentTypes"
			                                              });
		}
	}
}