using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASM_UI.Models;

namespace ASM_UI.Controllers
{
    [Authorize]
    public class currencyController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: currency
        public ActionResult Index()
        {
            return View();
        }

        //GET : /country/list
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            //ms_currency AssetRegLoc = new ms_currency();
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var CurrencyList = from cur in db.ms_currency
                                  where (cur.deleted_date == null)
                                  join u in db.ms_user on cur.updated_by equals u.user_id
                                  into t_joined
                                  from row_join in t_joined.DefaultIfEmpty()
                                  from usr in db.ms_user.Where(rec_updated_by => (rec_updated_by == null) ? false : rec_updated_by.user_id == row_join.user_id).DefaultIfEmpty()
                                  select new
                                  {
                                      cur.currency_id,
                                      cur.currency_code,
                                      cur.currency_name,
                                      cur.fl_active,
                                      rec_isactive = (cur.fl_active == true) ? "Yes" : "No",
                                      cur.created_by,
                                      cur.created_date,
                                      updated_by = (usr == null) ? string.Empty : usr.user_name,
                                      updated_date = cur.updated_date,
                                      cur.deleted_by,
                                      cur.deleted_date
                                  };
            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "currency_code":
                        CurrencyList = CurrencyList.Where(t => t.currency_code.Contains(searchString));
                        break;
                    case "currency_name":
                        CurrencyList = CurrencyList.Where(t => t.currency_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = CurrencyList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                CurrencyList = CurrencyList.OrderByDescending(t => t.currency_name);
                CurrencyList = CurrencyList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                CurrencyList = CurrencyList.OrderBy(t => t.currency_name);
                CurrencyList = CurrencyList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = CurrencyList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public JsonResult CrudCurrency()
        {
            if (UserProfile.UserId != 0)
            {
                    if (Request.Form["oper"] == "add")
                {
                    //prepare for insert data
                    ms_currency cur = new ms_currency();
                    cur.currency_code = Request.Form["currency_code"];
                    cur.currency_name = Request.Form["currency_name"];
                    cur.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                    cur.created_by = UserProfile.UserId;
                    cur.created_date = DateTime.Now;
                    cur.updated_by = UserProfile.UserId;
                    cur.updated_date = DateTime.Now;
                    cur.org_id = UserProfile.OrgId;
                    cur.deleted_by = null;
                    cur.deleted_date = null;

                    db.Entry(cur).State = EntityState.Added;

                    db.SaveChanges();
                    return Json("Insert", JsonRequestBehavior.AllowGet);
                }
                else if (Request.Form["oper"] == "edit")
                {
                    if (IsNumeric(Request.Form["currency_id"].ToString()))
                    {
                        //prepare for update data
                        int id = Convert.ToInt32(Request.Form["currency_id"]);
                        ms_currency cur = db.ms_currency.Find(id);
                        cur.currency_code = Request.Form["currency_code"];
                        cur.currency_name = Request.Form["currency_name"];
                        cur.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                        cur.updated_by = UserProfile.UserId;
                        cur.updated_date = DateTime.Now;

                        db.Entry(cur).State = EntityState.Modified;
                        db.SaveChanges();

                        return Json("Update", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //prepare for insert data
                        ms_currency cur = new ms_currency();
                        cur.currency_code = Request.Form["currency_code"];
                        cur.currency_name = Request.Form["currency_name"];
                        cur.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                        cur.created_by = UserProfile.UserId;
                        cur.created_date = DateTime.Now;
                        cur.updated_by = UserProfile.UserId;
                        cur.updated_date = DateTime.Now;
                        cur.org_id = UserProfile.OrgId;
                        cur.deleted_by = null;
                        cur.deleted_date = null;

                        db.Entry(cur).State = EntityState.Added;
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
                            ms_currency cur = db.ms_currency.Find(id);

                            cur.fl_active = false;
                            cur.deleted_by = UserProfile.UserId;
                            cur.deleted_date = DateTime.Now;
                            db.Entry(cur).State = EntityState.Modified;
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

        public JsonResult GetCurrency()
        {

            var CountryList = db.ms_currency.Where(t => t.deleted_date == null).Select(
                t => new
                {
                    t.currency_id,
                    t.currency_name,
                }).ToList();
            return Json(CountryList, JsonRequestBehavior.AllowGet);
        }
    }
}
