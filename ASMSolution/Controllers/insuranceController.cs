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
    public class insuranceController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: insurance
        public ActionResult Index()
        {
            //var ms_insurance = db.ms_insurance.Include(m => m.ms_insurancentry);
            //return View(ms_insurance.ToList());
            return View();
        }

        public ActionResult Modal()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            //ms_insurance courier = new ms_insurance();
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var InsuranceList = (
                               from ins in db.ms_insurance
                               join cty in db.ms_country
                               on ins.country_id equals cty.country_id
                               where (ins.deleted_date == null)
                               join u in db.ms_user on ins.updated_by equals u.user_id
                               into t_joined
                               from row_join in t_joined.DefaultIfEmpty()
                               from usr in db.ms_user.Where(rec_updated_by => (rec_updated_by == null) ? false : rec_updated_by.user_id == row_join.user_id).DefaultIfEmpty()
                               select new
                               {
                                   ins.insurance_id,
                                   ins.insurance_code,
                                   ins.insurance_name,
                                   ins.insurance_address,
                                   ins.country_id,
                                   cty.country_name,
                                   ins.insurance_phone,
                                   ins.insurance_mail,
                                   ins.insurance_cp_name,
                                   ins.insurance_cp_phone,
                                   ins.insurance_cp_mail,
                                   ins.insurance_description,
                                   ins.fl_active,
                                   rec_isactive = (ins.fl_active == true) ? "Yes" : "No",
                                   ins.created_by,
                                   ins.created_date,
                                   updated_by = (usr == null) ? string.Empty : usr.user_name,
                                   ins.updated_date,
                                   ins.deleted_by,
                                   ins.deleted_date,
                               });
            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "insurance_code":
                        InsuranceList = InsuranceList.Where(ins => ins.insurance_code.Contains(searchString));
                        break;
                    case "insurance_name":
                        InsuranceList = InsuranceList.Where(ins => ins.insurance_name.Contains(searchString));
                        break;
                    case "insurance_address":
                        InsuranceList = InsuranceList.Where(ins => ins.insurance_address.Contains(searchString));
                        break;
                    case "insntry_name":
                        InsuranceList = InsuranceList.Where(cty => cty.country_name.Contains(searchString));
                        break;
                    case "insurance_phone":
                        InsuranceList = InsuranceList.Where(ins => ins.insurance_phone.Contains(searchString));
                        break;
                    case "insurance_mail":
                        InsuranceList = InsuranceList.Where(ins => ins.insurance_mail.Contains(searchString));
                        break;
                    case "insurance_cp_name":
                        InsuranceList = InsuranceList.Where(ins => ins.insurance_cp_name.Contains(searchString));
                        break;
                    case "insurance_cp_phone":
                        InsuranceList = InsuranceList.Where(ins => ins.insurance_cp_phone.Contains(searchString));
                        break;
                    case "insurance_cp_mail":
                        InsuranceList = InsuranceList.Where(ins => ins.insurance_cp_mail.Contains(searchString));
                        break;
                    case "insurance_description":
                        InsuranceList = InsuranceList.Where(ins => ins.insurance_description.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = InsuranceList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                InsuranceList = InsuranceList.OrderByDescending(t => t.country_name);
                InsuranceList = InsuranceList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                InsuranceList = InsuranceList.OrderBy(t => t.country_name);
                InsuranceList = InsuranceList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = InsuranceList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public ActionResult SaveData(string NewName)
        {
            int idC;
            int.TryParse(Request.Form["insuranceID"], out idC);

            ms_insurance ms_insurance = new ms_insurance();
            ms_insurance.insurance_id = idC;
            ms_insurance.insurance_code = Request.Form["insuranceCode"];
            ms_insurance.insurance_name = Request.Form["insuranceName"];
            ms_insurance.insurance_address = Request.Form["insuranceAddress"];
            ms_insurance.country_id = Convert.ToInt32(Request.Form["Country"]);
            ms_insurance.insurance_phone = Request.Form["insurancePhone"];
            ms_insurance.insurance_mail = Request.Form["insuranceMail"];
            ms_insurance.insurance_cp_name = Request.Form["insuranceCPName"];
            ms_insurance.insurance_cp_phone = Request.Form["insuranceCPPhone"];
            ms_insurance.insurance_cp_mail = Request.Form["insuranceCPMail"];
            ms_insurance.insurance_description = Request.Form["insuranceDescription"];
            ms_insurance.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
            ms_insurance.deleted_by = null;
            ms_insurance.deleted_date = null;

            if (idC == 0)
            {
                ms_insurance.created_by = UserProfile.UserId;
                ms_insurance.created_date = DateTime.Now;
                ms_insurance.org_id = UserProfile.OrgId;
                db.Entry(ms_insurance).State = EntityState.Added;
                db.SaveChanges();
                return Json("Insert", JsonRequestBehavior.AllowGet);
            }
            else
            {
                ms_insurance.updated_by = UserProfile.UserId;
                ms_insurance.updated_date = DateTime.Now;
                db.Entry(ms_insurance).State = EntityState.Modified;
                db.SaveChanges();
                return Json("Update", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult CrudInsurance()
        {
            if (Request.Form["oper"] == "add")
            {
                //prepare for insert data
                ms_insurance ms_insurance = new ms_insurance();
                ms_insurance.insurance_code = Request.Form["insurance_code"];
                ms_insurance.insurance_name = Request.Form["insurance_name"];
                ms_insurance.insurance_address = Request.Form["insurance_address"];
                ms_insurance.country_id = Convert.ToInt32(Request.Form["country_id"]);
                ms_insurance.insurance_phone = Request.Form["insurance_phone"];
                ms_insurance.insurance_mail = Request.Form["insurance_mail"];
                ms_insurance.insurance_cp_name = Request.Form["insurance_cp_name"];
                ms_insurance.insurance_cp_phone = Request.Form["insurance_cp_phone"];
                ms_insurance.insurance_cp_mail = Request.Form["insurance_cp_mail"];
                ms_insurance.insurance_description = Request.Form["insurance_description"];
                ms_insurance.created_by = 1;
                ms_insurance.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                ms_insurance.created_date = DateTime.Now;
                ms_insurance.updated_by = 1;
                ms_insurance.updated_date = DateTime.Now;
                ms_insurance.org_id = 0;
                db.ms_insurance.Add(ms_insurance);
                db.SaveChanges();
                return Json("Insert Insurance Data Success!", JsonRequestBehavior.AllowGet);
            }
            else if (Request.Form["oper"] == "edit")
            {
                if (IsNumeric(Request.Form["insurance_id"].ToString()))
                {
                    //prepare for update data
                    int id = Convert.ToInt32(Request.Form["insurance_id"]);
                    ms_insurance ms_insurance = db.ms_insurance.Find(id);
                    ms_insurance.insurance_code = Request.Form["insurance_code"];
                    ms_insurance.insurance_name = Request.Form["insurance_name"];
                    ms_insurance.insurance_address = Request.Form["insurance_address"];
                    ms_insurance.country_id = Convert.ToInt32(Request.Form["country_id"]);
                    ms_insurance.insurance_phone = Request.Form["insurance_phone"];
                    ms_insurance.insurance_mail = Request.Form["insurance_mail"];
                    ms_insurance.insurance_cp_name = Request.Form["insurance_cp_name"];
                    ms_insurance.insurance_cp_phone = Request.Form["insurance_cp_phone"];
                    ms_insurance.insurance_cp_mail = Request.Form["insurance_cp_mail"];
                    ms_insurance.insurance_description = Request.Form["insurance_description"];
                    ms_insurance.updated_by = 1;
                    ms_insurance.updated_date = DateTime.Now;
                    db.SaveChanges();
                    return Json("Update Insurance Data Success!", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //prepare for insert data
                    ms_insurance ms_insurance = new ms_insurance();
                    ms_insurance.insurance_code = Request.Form["insurance_code"];
                    ms_insurance.insurance_name = Request.Form["insurance_name"];
                    ms_insurance.insurance_address = Request.Form["insurance_address"];
                    ms_insurance.country_id = Convert.ToInt32(Request.Form["country_id"]);
                    ms_insurance.insurance_phone = Request.Form["insurance_phone"];
                    ms_insurance.insurance_mail = Request.Form["insurance_mail"];
                    ms_insurance.insurance_cp_name = Request.Form["insurance_cp_name"];
                    ms_insurance.insurance_cp_phone = Request.Form["insurance_cp_phone"];
                    ms_insurance.insurance_cp_mail = Request.Form["insurance_cp_mail"];
                    ms_insurance.insurance_description = Request.Form["insurance_description"];
                    ms_insurance.created_by = 1;
                    ms_insurance.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                    ms_insurance.created_date = DateTime.Now;
                    ms_insurance.updated_by = 1;
                    ms_insurance.updated_date = DateTime.Now;
                    ms_insurance.org_id = 0;
                    db.ms_insurance.Add(ms_insurance);
                    db.SaveChanges();
                    return Json("Insert Insurance Data Success!", JsonRequestBehavior.AllowGet);
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
                        ms_insurance cou = db.ms_insurance.Find(id);
                        cou.deleted_by = 1;
                        cou.deleted_date = DateTime.Now;
                        db.SaveChanges();
                    }

                }
                return Json("Deleted Success!");
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

        public JsonResult GetInsuranceByID(string id)
        {
            int idC;
            int.TryParse(id, out idC);
            var InsuranceList = (
                              from ins in db.ms_insurance
                              join cty in db.ms_country
                              on ins.country_id equals cty.country_id
                              where (ins.insurance_id == idC)
                              select new
                              {
                                  ins.insurance_id,
                                  ins.insurance_code,
                                  ins.insurance_name,
                                  ins.insurance_address,
                                  ins.country_id,
                                  cty.country_name,
                                  ins.insurance_phone,
                                  ins.insurance_mail,
                                  ins.insurance_cp_name,
                                  ins.insurance_cp_phone,
                                  ins.insurance_cp_mail,
                                  ins.insurance_description,
                                  ins.fl_active,
                                  rec_isactive = (ins.fl_active == true) ? "Yes" : "No"
                              });
            return Json(InsuranceList.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
