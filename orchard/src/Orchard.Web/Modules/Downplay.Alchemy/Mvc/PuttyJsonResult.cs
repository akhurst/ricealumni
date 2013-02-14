using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Downplay.Alchemy.Mvc {
    public class StuffJsonResult : ActionResult {

        public dynamic Data { get; set; }

        public Encoding ContentEncoding {
            get;
            set;
        }

        public string ContentType {
            get;
            set;
        }

        public JsonRequestBehavior JsonRequestBehavior {
            get;
            set;
        }

        public StuffJsonResult(dynamic stuff) {

            Data = stuff;
            JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet;

        }

        public override void ExecuteResult(ControllerContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase)) {
                throw new InvalidOperationException("GET not allowed");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType)) {
                response.ContentType = ContentType;
            }
            else {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null) {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null) {
                response.Write(Data.Json());
            }
        }
    }
}
