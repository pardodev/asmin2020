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
    public class asmin_companyController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();
        //private ModelAsm _db = new ModelAsm();

        // GET: asmin_company
        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            //var AsminCompany = db.ms_asmin_company.Where(t => t.deleted_date == null).Select(
            //        t => new
            //        {
            //            t.company_id,
            //            t.company_code,
            //            t.company_name,
            //            t.fl_active,
            //            rec_isactive = (t.fl_active == true) ? "Yes" : "No",
            //            t.created_by,
            //            t.created_date,
            //            t.updated_by,
            //            t.updated_date,
            //            t.deleted_by,
            //            t.deleted_date,
            //        });

            var AsminCompany = from comp in db.ms_asmin_company
                               where (comp.deleted_date == null)
                               join u in db.ms_user on comp.updated_by equals u.user_id
                               into t_joined
                               from row_join in t_joined.DefaultIfEmpty()
                               from usr in db.ms_user.Where(rec_updated_by => (rec_updated_by == null) ? false : rec_updated_by.user_id == row_join.user_id).DefaultIfEmpty()
                               select new
                               {
                                   comp.company_id,
                                   comp.company_code,
                                   comp.company_name,
                                   comp.fl_active,
                                   rec_isactive = (comp.fl_active == true) ? "Yes" : "No",
                                   comp.created_by,
                                   comp.created_date,
                                   updated_by = (usr == null) ? string.Empty : usr.user_name,
                                   updated_date = comp.updated_date,
                                   comp.deleted_by,
                                   comp.deleted_date
                               };

            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "company_code":
                        AsminCompany = AsminCompany.Where(t => t.company_code.Contains(searchString));
                        break;
                    case "company_name":
                        AsminCompany = AsminCompany.Where(t => t.company_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = AsminCompany.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                AsminCompany = AsminCompany.OrderByDescending(t => t.company_name);
                AsminCompany = AsminCompany.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                AsminCompany = AsminCompany.OrderBy(t => t.company_name);
                AsminCompany = AsminCompany.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = AsminCompany
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public JsonResult CrudCompany()
        {
            if (UserProfile.UserId != 0)
            {
                if (Request.Form["oper"] == "add")
                {
                    //prepare for insert data
                    ms_asmin_company ms_asmin_company = new ms_asmin_company();
                    ms_asmin_company.company_code = Request.Form["company_code"];
                    ms_asmin_company.company_name = Request.Form["company_name"];
                    ms_asmin_company.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                    //ms_asmin_company.fl_active = true;

                    ms_asmin_company.created_by = UserProfile.UserId;
                    ms_asmin_company.created_date = DateTime.Now;
                    ms_asmin_company.updated_by = UserProfile.UserId;
                    ms_asmin_company.updated_date = DateTime.Now;
                    ms_asmin_company.org_id = UserProfile.OrgId;
                    ms_asmin_company.deleted_by = null;
                    ms_asmin_company.deleted_date = null;

                    //db.ms_asmin_company.Add(ms_asmin_company);
                    db.Entry(ms_asmin_company).State = EntityState.Added;
                    db.SaveChanges();
                    return Json("Insert", JsonRequestBehavior.AllowGet);
                }
                else if (Request.Form["oper"] == "edit")
                {
                    if (IsNumeric(Request.Form["company_id"].ToString()))
                    {
                        //prepare for update data
                        int id = Convert.ToInt32(Request.Form["company_id"]);
                        ms_asmin_company ms_asmin_company = db.ms_asmin_company.Find(id);
                        ms_asmin_company.company_code = Request.Form["company_code"];
                        ms_asmin_company.company_name = Request.Form["company_name"];
                        ms_asmin_company.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                        //ms_asmin_company.fl_active = true;

                        ms_asmin_company.updated_by = UserProfile.UserId;
                        ms_asmin_company.updated_date = DateTime.Now;

                        db.Entry(ms_asmin_company).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json("Update", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //prepare for insert data
                        ms_asmin_company ms_asmin_company = new ms_asmin_company();
                        ms_asmin_company.company_code = Request.Form["company_code"];
                        ms_asmin_company.company_name = Request.Form["company_name"];
                        ms_asmin_company.created_by = 1;
                        ms_asmin_company.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                        //ms_asmin_company.fl_active = true;

                        ms_asmin_company.created_by = UserProfile.UserId;
                        ms_asmin_company.created_date = DateTime.Now;
                        ms_asmin_company.updated_by = UserProfile.UserId;
                        ms_asmin_company.updated_date = DateTime.Now;
                        ms_asmin_company.org_id = UserProfile.OrgId;
                        ms_asmin_company.deleted_by = null;
                        ms_asmin_company.deleted_date = null;

                        //db.ms_asmin_company.Add(ms_asmin_company);
                        db.Entry(ms_asmin_company).State = EntityState.Added;
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
                            ms_asmin_company ms_asmin_company = db.ms_asmin_company.Find(id);

                            ms_asmin_company.fl_active = false;
                            ms_asmin_company.deleted_by = UserProfile.UserId;
                            ms_asmin_company.deleted_date = DateTime.Now;
                            db.Entry(ms_asmin_company).State = EntityState.Modified;
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
