using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASM_UI.Models;

namespace ASM_UI.Controllers
{
    [Authorize]
    public class courierController : BaseController
    {

        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: currency
        public ActionResult Index()
        {
            return View();
        }

        //GET : /courier/list
        [HttpGet]
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            ms_courier courier = new ms_courier();
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var CourierList = (from cou in db.ms_courier
                               join cty in db.ms_country
                               on cou.country_id equals cty.country_id
                               where (cou.deleted_date == null)
                               select new
                               {
                                   cou.courier_id,
                                   cou.courier_code,
                                   cou.courier_name,
                                   cou.courier_address,
                                   cou.country_id,
                                   cty.country_name,
                                   cou.courier_phone,
                                   cou.courier_mail,
                                   cou.courier_description,
                                   cou.fl_active,
                                   rec_isactive = (cou.fl_active == true) ? "Yes" : "No",
                                   cou.created_by,
                                   cou.created_date,
                                   cou.updated_by,
                                   cou.updated_date,
                                   cou.deleted_by,
                                   cou.deleted_date,
                               });
            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "courier_code":
                        CourierList = CourierList.Where(cou => cou.courier_code.Contains(searchString));
                        break;
                    case "courier_name":
                        CourierList = CourierList.Where(cou => cou.courier_name.Contains(searchString));
                        break;
                    case "courier_address":
                        CourierList = CourierList.Where(cou => cou.courier_address.Contains(searchString));
                        break;
                    case "country_name":
                        CourierList = CourierList.Where(cty => cty.country_name.Contains(searchString));
                        break;
                    case "courier_phone":
                        CourierList = CourierList.Where(cou => cou.courier_phone.Contains(searchString));
                        break;
                    case "courier_mail":
                        CourierList = CourierList.Where(cou => cou.courier_mail.Contains(searchString));
                        break;
                    case "courier_description":
                        CourierList = CourierList.Where(cou => cou.courier_description.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = CourierList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                CourierList = CourierList.OrderByDescending(t => t.country_name);
                CourierList = CourierList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                CourierList = CourierList.OrderBy(t => t.country_name);
                CourierList = CourierList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = CourierList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public JsonResult CrudCourier()
        {
            if (UserProfile.UserId != 0)
            {
                if (Request.Form["oper"] == "add")
                {
                    //prepare for insert data
                    ms_courier ms_cou = new ms_courier();
                    ms_cou.courier_code = Request.Form["courier_code"];
                    ms_cou.courier_name = Request.Form["courier_name"];
                    ms_cou.courier_address = Request.Form["courier_address"];
                    ms_cou.country_id = Convert.ToInt32(Request.Form["country_name"]);
                    ms_cou.courier_phone = Request.Form["courier_phone"];
                    ms_cou.courier_mail = Request.Form["courier_mail"];
                    ms_cou.courier_description = Request.Form["courier_description"];
                    ms_cou.created_by = 1;
                    ms_cou.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                    ms_cou.created_date = DateTime.Now;
                    ms_cou.updated_by = 1;
                    ms_cou.updated_date = DateTime.Now;
                    ms_cou.org_id = 0;
                    db.ms_courier.Add(ms_cou);
                    db.SaveChanges();
                    return Json("Insert", JsonRequestBehavior.AllowGet);
                }
                else if (Request.Form["oper"] == "edit")
                {
                    if (IsNumeric(Request.Form["courier_id"].ToString()))
                    {
                        //prepare for update data
                        int id = Convert.ToInt32(Request.Form["courier_id"]);
                        ms_courier cou = db.ms_courier.Find(id);
                        cou.courier_code = Request.Form["courier_code"];
                        cou.courier_name = Request.Form["courier_name"];
                        cou.courier_address = Request.Form["courier_address"];
                        cou.country_id = Convert.ToInt32(Request.Form["country_name"]);
                        cou.courier_phone = Request.Form["courier_phone"];
                        cou.courier_mail = Request.Form["courier_mail"];
                        cou.courier_description = Request.Form["courier_description"];
                        cou.updated_by = 1;
                        cou.updated_date = DateTime.Now;
                        db.SaveChanges();
                        return Json("Update", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //prepare for insert data
                        ms_courier ms_cou = new ms_courier();
                        ms_cou.courier_code = Request.Form["courier_code"];
                        ms_cou.courier_name = Request.Form["courier_name"];
                        ms_cou.courier_address = Request.Form["courier_address"];
                        ms_cou.country_id = Convert.ToInt32(Request.Form["country_name"]);
                        ms_cou.courier_phone = Request.Form["courier_phone"];
                        ms_cou.courier_mail = Request.Form["courier_mail"];
                        ms_cou.courier_description = Request.Form["courier_description"];
                        ms_cou.created_by = 1;
                        ms_cou.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                        ms_cou.created_date = DateTime.Now;
                        ms_cou.updated_by = 1;
                        ms_cou.updated_date = DateTime.Now;
                        ms_cou.org_id = 0;
                        db.ms_courier.Add(ms_cou);
                        db.SaveChanges();
                        return Json("Insert", JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    if (Request.Form["oper"] == "del")
                    {
                        //for delete process
                        string ids = Request.Form["id"];
                        string[] values = ids.Split(',');
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = values[i].Trim();
                            //prepare for soft delete data
                            int id = Convert.ToInt32(values[i]);
                            ms_courier cou = db.ms_courier.Find(id);
                            cou.deleted_by = 1;
                            cou.deleted_date = DateTime.Now;
                            db.SaveChanges();
                        }
                        return Json("Delete", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json("Session", JsonRequestBehavior.AllowGet);
            }
        }
       
    }
}
