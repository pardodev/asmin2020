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
    public class countryController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: country
        public ActionResult Index()
        {
            return View();
        }

        //GET : /country/list
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            ms_country country = new ms_country();
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var CountryList = from cou in db.ms_country
                              where (cou.deleted_date == null)
                              join u in db.ms_user on cou.updated_by equals u.user_id
                              into t_joined
                              from row_join in t_joined.DefaultIfEmpty()
                              from usr in db.ms_user.Where(rec_updated_by => (rec_updated_by == null) ? false : rec_updated_by.user_id == row_join.user_id).DefaultIfEmpty()
                              select new
                              {
                                  cou.country_id,
                                  cou.country_code,
                                  cou.country_name,
                                  cou.fl_active,
                                  rec_isactive = (cou.fl_active == true) ? "Yes" : "No",
                                  cou.created_by,
                                  cou.created_date,
                                  updated_by = (usr == null) ? string.Empty : usr.user_name,
                                  updated_date = cou.updated_date,
                                  cou.deleted_by,
                                  cou.deleted_date
                              };
            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "country_code":
                        CountryList = CountryList.Where(t => t.country_code.Contains(searchString));
                        break;
                    case "country_name":
                        CountryList = CountryList.Where(t => t.country_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = CountryList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default soring
            if (sord.ToUpper() == "DESC")
            {
                CountryList = CountryList.OrderByDescending(t => t.country_name);
                CountryList = CountryList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                CountryList = CountryList.OrderBy(t => t.country_name);
                CountryList = CountryList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = CountryList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public JsonResult CrudCountry()
        {
            if (UserProfile.UserId != 0)
            {
                if (Request.Form["oper"] == "add")
                {
                    //prepare for insert data
                    ms_country ms_cnty = new ms_country();
                    ms_cnty.country_code = Request.Form["country_code"];
                    ms_cnty.country_name = Request.Form["country_name"];
                    ms_cnty.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                    ms_cnty.created_by = UserProfile.UserId;
                    ms_cnty.created_date = DateTime.Now;
                    ms_cnty.updated_by = UserProfile.UserId;
                    ms_cnty.updated_date = DateTime.Now;
                    ms_cnty.org_id = UserProfile.OrgId;
                    ms_cnty.deleted_by = null;
                    ms_cnty.deleted_date = null;

                    db.Entry(ms_cnty).State = EntityState.Added;
                    db.SaveChanges();
                    return Json("Insert", JsonRequestBehavior.AllowGet);
                }
                else if (Request.Form["oper"] == "edit")
                {
                    if (IsNumeric(Request.Form["country_id"].ToString()))
                    {
                        //prepare for update data
                        int id = Convert.ToInt32(Request.Form["country_id"]);
                        ms_country cnty = db.ms_country.Find(id);
                        cnty.country_code = Request.Form["country_code"];
                        cnty.country_name = Request.Form["country_name"];
                        cnty.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                        cnty.updated_by = UserProfile.UserId;
                        cnty.updated_date = DateTime.Now;

                        db.Entry(cnty).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json("Update", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //prepare for insert data
                        ms_country ms_cnty = new ms_country();
                        ms_cnty.country_code = Request.Form["country_code"];
                        ms_cnty.country_name = Request.Form["country_name"];
                        ms_cnty.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                        ms_cnty.created_by = UserProfile.UserId;
                        ms_cnty.created_date = DateTime.Now;
                        ms_cnty.updated_by = UserProfile.UserId;
                        ms_cnty.updated_date = DateTime.Now;
                        ms_cnty.org_id = UserProfile.OrgId;
                        ms_cnty.deleted_by = null;
                        ms_cnty.deleted_date = null;

                        db.Entry(ms_cnty).State = EntityState.Added;
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
                            ms_country cnty = db.ms_country.Find(id);

                            cnty.fl_active = false;
                            cnty.deleted_by = UserProfile.UserId;
                            cnty.deleted_date = DateTime.Now;

                            db.Entry(cnty).State = EntityState.Modified;
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

        public JsonResult GetCountry()
        {

            var CountryList = db.ms_country.Where(t => t.deleted_date == null).Select(
                t => new
                {
                    t.country_id,
                    t.country_name,
                }).ToList();
            return Json(CountryList, JsonRequestBehavior.AllowGet);
        }
        

    }
}
