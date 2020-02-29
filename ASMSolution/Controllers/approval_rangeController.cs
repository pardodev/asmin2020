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
    public class approval_rangeController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();
        // GET: approval_range
        public ActionResult Index()
        {
            ViewBag.PageId = 1;
            return View();
            //_db.ms_approval_range.ToList()
        }

        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var ApprovalRange = from apv in db.ms_approval_range
                                where (apv.deleted_date == null)
                                join u in db.ms_user on apv.updated_by equals u.user_id
                                into t_joined
                                from row_join in t_joined.DefaultIfEmpty()
                                from usr in db.ms_user.Where(rec_updated_by => (rec_updated_by == null) ? false : rec_updated_by.user_id == row_join.user_id).DefaultIfEmpty()
                                select new
                                {
                                    apv.range_id,
                                    apv.range_type,
                                    apv.range_code,
                                    apv.range_min,
                                    apv.range_max,
                                    apv.fl_active,
                                    rec_isactive = (apv.fl_active == true) ? "Yes" : "No",
                                    apv.created_by,
                                    apv.created_date,
                                    updated_by = (usr == null) ? string.Empty : usr.user_name,
                                    updated_date = apv.updated_date
                                    //apv.deleted_by,
                                    //apv.deleted_date
                                };
            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "range_code":
                        ApprovalRange = ApprovalRange.Where(t => t.range_code.Contains(searchString));
                        break;
                    case "range_type":
                        ApprovalRange = ApprovalRange.Where(t => t.range_type.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = ApprovalRange.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                ApprovalRange = ApprovalRange.OrderByDescending(t => t.range_type);
                ApprovalRange = ApprovalRange.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                ApprovalRange = ApprovalRange.OrderBy(t => t.range_type);
                ApprovalRange = ApprovalRange.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = ApprovalRange
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public JsonResult CrudApproval()
        {
            decimal RangeMin = 0, RangeMax = 0;

            if (Request.Form["oper"] == "add")
            {
                if (Request.Form["range_min"] != null)
                    Decimal.TryParse(Request.Form["range_min"].Replace(",", ""), out RangeMin);

                if (Request.Form["range_max"] != null)
                    Decimal.TryParse(Request.Form["range_max"].Replace(",", ""), out RangeMax);

                //prepare for insert data
                ms_approval_range ms_approval_range = new ms_approval_range();
                ms_approval_range.range_code = Request.Form["range_code"];
                ms_approval_range.range_type = Request.Form["range_type"];
                ms_approval_range.range_min = RangeMin;
                ms_approval_range.range_max = RangeMax;
                ms_approval_range.fl_active = Request.Form["rec_isactive"].ToLower() == "yes" ? true : false;

                ms_approval_range.created_by = UserProfile.UserId;
                ms_approval_range.created_date = DateTime.Now;
                ms_approval_range.updated_by = UserProfile.UserId;
                ms_approval_range.updated_date = DateTime.Now;
                ms_approval_range.org_id = UserProfile.OrgId;
                ms_approval_range.deleted_by = null;
                ms_approval_range.deleted_date = null;

                db.Entry(ms_approval_range).State = EntityState.Added;
                db.SaveChanges();
                
                return Json("Insert Approval Range Data Success!", JsonRequestBehavior.AllowGet);

            }
            else if (Request.Form["oper"] == "edit")
            {
                
                if (Request.Form["range_min"] != null)
                    Decimal.TryParse(Request.Form["range_min"].Replace(",", ""), out RangeMin);

                if (Request.Form["range_max"] != null)
                    Decimal.TryParse(Request.Form["range_max"].Replace(",", ""), out RangeMax);

                if (IsNumeric(Request.Form["range_id"].ToString()))
                {
                    //prepare for update data
                    int id = Convert.ToInt32(Request.Form["range_id"]);
                    ms_approval_range ms_approval_range = db.ms_approval_range.Find(id);
                    ms_approval_range.range_code = Request.Form["range_code"];
                    ms_approval_range.range_type = Request.Form["range_type"];
                    ms_approval_range.range_min = RangeMin;
                    ms_approval_range.range_max = RangeMax;
                    ms_approval_range.fl_active = Request.Form["rec_isactive"].ToLower() == "yes" ? true : false;

                    ms_approval_range.updated_by = UserProfile.UserId;
                    ms_approval_range.updated_date = DateTime.Now;

                    db.Entry(ms_approval_range).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json("Update Asset Register PIC Data Success!", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //prepare for insert data
                    ms_approval_range ms_approval_range = new ms_approval_range();
                    ms_approval_range.range_code = Request.Form["range_code"];
                    ms_approval_range.range_type = Request.Form["range_type"];
                    ms_approval_range.range_min = RangeMin;
                    ms_approval_range.range_max = RangeMax;
                    ms_approval_range.fl_active = Request.Form["rec_isactive"].ToLower() == "yes" ? true : false;

                    ms_approval_range.created_by = UserProfile.UserId;
                    ms_approval_range.created_date = DateTime.Now;
                    ms_approval_range.updated_by = UserProfile.UserId;
                    ms_approval_range.updated_date = DateTime.Now;
                    ms_approval_range.org_id = UserProfile.OrgId;
                    ms_approval_range.deleted_by = null;
                    ms_approval_range.deleted_date = null;

                    db.Entry(ms_approval_range).State = EntityState.Added;
                    db.SaveChanges();
                    return Json("Insert Approval Range Data Success!", JsonRequestBehavior.AllowGet);
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
                        ms_approval_range ms_approval_range = db.ms_approval_range.Find(id);

                        ms_approval_range.fl_active = false;
                        ms_approval_range.deleted_by = UserProfile.UserId;
                        ms_approval_range.deleted_date = DateTime.Now;

                        db.Entry(ms_approval_range).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
                return Json("Deleted Success!");
            }
        }
    }
}
