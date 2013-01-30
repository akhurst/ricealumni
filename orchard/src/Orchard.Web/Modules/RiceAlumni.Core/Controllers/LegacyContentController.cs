using System.IO;
using System.Web.Mvc;

namespace RiceAlumni.Core.Controllers
{
	public class LegacyContentController : Controller
	{
		public ActionResult RenderLegacyContent(string path)
		{
			if (path == "index.html")
				Redirect("~/");

			var filePath = LegacyContentConstraint.FindPath(HttpContext, path);
			var stream = new FileStream(filePath, FileMode.Open);
			return new FileStreamResult(stream, "text/html");
		}
	}
}