using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Resources;

namespace RiceAlumni.Events
{
	public class ResourceManifest : IResourceManifestProvider
	{
		public void BuildManifests(ResourceManifestBuilder builder)
		{
			var manifest = builder.Add();
			manifest.DefineScript("knockout").SetUrl("knockout-2.2.1.js");
		}
	}
}