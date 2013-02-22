using System.Web.Mvc;

namespace RiceAlumni.MainSite.Controllers
{
	public class LegacyContentController : Controller
	{
		public ActionResult RenderLegacyContent(string path)
		{
			if (path == "index.html" || path == "legacycontent/index.html")
				return Redirect("~/");

			var splitPath = path.TrimEnd('/').Split('/');
			var finalPath = path;

			if(splitPath.Length > 0 && !splitPath[splitPath.Length-1].Contains("."))
			{
				finalPath = finalPath + "/index.html";
			}
			return Redirect("~/legacycontent/"+finalPath);
		}
	}
}
