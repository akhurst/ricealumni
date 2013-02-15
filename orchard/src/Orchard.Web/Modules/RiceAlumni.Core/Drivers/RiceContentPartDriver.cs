using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace RiceAlumni.Core.Drivers
{
	public static class RiceContentPartDriverExtensions
	{
		public static void DoImport<TContent>(this ContentPartDriver<TContent> driver, TContent part, ImportContentContext context)
			where TContent : ContentPart,new()
		{
			var type = part.GetType();
			var properties = type.GetProperties(BindingFlags);

			foreach (var property in properties)
			{
				dynamic value = context.Attribute(part.PartDefinition.Name, property.Name);
				if (value != null && property.CanRead && property.CanWrite)
				{
					//					if(property.PropertyType ==typeof(DateTime))
					//					{
					//						DateTime dateTime;
					//						if(DateTime.TryParse(value, out dateTime)) property.SetValue(part, dateTime,null);
					//					}
					//					else if(property.PropertyType == typeof(int))
					//					{
					//						property.SetValue(part, Convert.ToInt32(value), null);
					//					}
					//					else if(property.PropertyType == typeof(double))
					//					{
					//						property.SetValue(part,Convert.ToDouble(value),null);
					//					}
					//					else
					{
						property.SetValue(part, value, null);
					}
				}
			}
		}

		public static void DoExport<TContent>(this ContentPartDriver<TContent> driver, TContent part, ExportContentContext context)
			where TContent : ContentPart, new()
		{
			var type = part.GetType();
			var properties = type.GetProperties(BindingFlags);

			foreach (var property in properties)
			{
				if(property.CanRead && property.CanWrite)
					context.Element(part.PartDefinition.Name).SetAttributeValue(property.Name, property.GetValue(part, null));
			}
		}

		private static BindingFlags BindingFlags
		{
			get { return BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly; }			
		}
	}
}