using System.IO;
using System.Web.Mvc;

namespace RiceAlumni.Core.Controllers
{
	public class LegacyContentController : Controller
	{
		public ActionResult RenderLegacyContent(string path)
		{
			if (path == "index.html")
				return Redirect("~/");

			var splitPath = path.Split('/');
			var finalPath = path;

			if(splitPath.Length > 0 && !splitPath[splitPath.Length-1].Contains("."))
			{
				finalPath = finalPath + "/index.html";
			}
			return Redirect("~/legacycontent/"+finalPath);
		}
	}
}