using ClosedXML.Excel;
using Mvc.Controls.DataTable.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Web.Test.Controllers
{
    public class ExportController : Controller
    {
        public class DataTableExportParam
        {
            internal object dataTableParam;

            public string DataAction { get; internal set; }
            public string DataController { get; internal set; }
        }

        // GET: Excel
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Download(DataTableExportParam dataExportParam)
        {
            var dataJson = this.GetDataReflection(dataExportParam);
            //var dataObject = JsonConvert.DeserializeObject<IEnumerable<Object>>(dataJson.ca);
            var dataObject = new List<object>();
            using (XLWorkbook wb = new XLWorkbook())
            {
                
                var ws = wb.Worksheets.Add("Data");
                ws.Cell(1, 1).InsertData(dataObject);

                
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= Data.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("Index", "Export");
        }


        [HttpGet]
        public virtual ActionResult Download(string file)
        {
            string fullPath = Path.Combine(Server.MapPath("~/MyFiles"), file);
            return File(fullPath, "application/vnd.ms-excel", file);
        }

        private async Task GetDataAjax(DataTableExportParam dataExportParam)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage response = await client.PostAsJsonAsync(base.Url.Action(dataExportParam.DataAction, 
                    dataExportParam.DataController), dataExportParam.dataTableParam);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);
                }
            }
        }


        private JsonResult GetDataReflection(DataTableExportParam dataExportParam)
        {
            Type typeOfController;
            object instanceOfController;

            try
            {
                typeOfController = System.Reflection.Assembly.GetExecutingAssembly()
                    .GetType(name: dataExportParam.DataController, throwOnError: true, ignoreCase: true);
                instanceOfController = System.Reflection.Assembly.GetExecutingAssembly()
                    .CreateInstance(typeName: dataExportParam.DataController, ignoreCase: true);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot create data controller instance (Data Export)", ex);
            }

            JsonResult result = null;

            try
            {
                result = typeOfController.InvokeMember(dataExportParam.DataAction,
                    System.Reflection.BindingFlags.InvokeMethod, null, instanceOfController, null) as JsonResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while performing the data method invoke (Data Export)", ex);
            }
            return result;
        }
    }
}