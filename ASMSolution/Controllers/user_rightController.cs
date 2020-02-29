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
    public class user_rightController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ModalFrm()
        {
            return PartialView();
        }

        public JsonResult UserRightList()
        {
            var _qry = from _ur in db.ms_user_rights
                       where (_ur.deleted_date == null && _ur.fl_active == true)
                       join ut in db.ms_user_type on _ur.user_type_id equals ut.user_type_id
                       where (ut.deleted_date == null && ut.fl_active == true)
                       join jl in db.ms_job_level on _ur.job_level_id equals jl.job_level_id
                       where (jl.deleted_date == null && jl.fl_active == true)
                       select new
                       {
                           user_type_id = ut.user_type_id,
                           user_type_name = ut.user_type_name,
                           job_level_id = jl.job_level_id,
                           job_level_name = jl.job_level_name
                       };

            return Json(_qry, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            //ms_user_rights _usr = new ms_user_rights();
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var _qry = (from _ur in db.ms_user_rights
                        where (_ur.deleted_date == null && _ur.fl_active == true)
                        join ut in db.ms_user_type on _ur.user_type_id equals ut.user_type_id
                        where (ut.deleted_date == null && ut.fl_active == true)
                        join jl in db.ms_job_level on _ur.job_level_id equals jl.job_level_id
                        where (jl.deleted_date == null && jl.fl_active == true)
                        select new
                        {
                            user_type_id = ut.user_type_id,
                            user_type_name = ut.user_type_name,
                            job_level_id = jl.job_level_id,
                            job_level_name = jl.job_level_name
                        }).ToList();

            var _List = _qry.GroupBy(x => new { x.user_type_id, x.user_type_name, x.job_level_id, x.job_level_name })
                .Select(g => new
                {
                    g.Key.user_type_id,
                    g.Key.user_type_name,
                    g.Key.job_level_id,
                    g.Key.job_level_name
                });

            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "user_type_name":
                        _List = _List.Where(t => t.user_type_name.Contains(searchString));
                        break;
                    case "job_level_name":
                        _List = _List.Where(t => t.job_level_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = _List.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default soring
            if (sord.ToUpper() == "DESC")
            {
                _List = _List.OrderByDescending(t => t.user_type_id);
                _List = _List.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                _List = _List.OrderBy(t => t.user_type_id);
                _List = _List.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = _List
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult form()
        {
            return RedirectToAction("form/0/0", "user_right");
        }


        // GET: user_rights/Create
        //public ActionResult Create()
        //{
        //    ViewBag.job_level_id = new SelectList(db.ms_job_level, "job_level_id", "job_level_code");
        //    ViewBag.user_type_id = new SelectList(db.ms_user_type, "user_type_id", "user_type_code");
        //    return View();
        //}

        [HttpGet]
        [Route("user_right/form/{job_level}/{user_type}")]
        public ActionResult PopulateForm(int? job_level = 0, int? user_type = 0)
        {
            job_level = (job_level == null || job_level <= 0) ? 0 : job_level;
            user_type = (user_type == null || user_type <= 0) ? 0 : user_type;

            user_rightViewModel _model = null;
            //check apakah update mode or new mode
            if (job_level > 0 && user_type > 0)
            {
                List<ms_user_rights> _list = db.ms_user_rights
                    .Where(m => m.fl_active == true
                    && m.deleted_date == null
                    && m.user_type_id == user_type
                    && m.job_level_id == job_level)
                    .ToList<ms_user_rights>();

                if (_list.Count == 0) //----this is new  karena gak ketemu 
                {
                    //new 

                    _model = new user_rightViewModel();
                    _model.FormMode = EnumFormModeKey.Form_New;
                    _model.job_level_list = db.ms_job_level.Where(r=>r.fl_active == true && r.deleted_date == null).ToList<ms_job_level>();
                    _model.user_type_list = db.ms_user_type.Where(r => r.fl_active == true && r.deleted_date == null).ToList<ms_user_type>();
                    _model.ms_menus = db.ms_menu.Where(m => m.fl_active == true && m.deleted_date == null).ToList<ms_menu>();
                    foreach (var m in _model.ms_menus)
                    {
                        _model.checkbox_menu_id.Add(new SelectedMenu_CheckBoxes()
                        {
                            Checked = false,
                            menu_id = m.menu_id,
                            ms_menu = m,
                            Text = m.menu_name,
                            Value = m.menu_id.ToString()
                        });
                    }
                }
                else //sudah ada record = edit  mode
                {
                    _model = new user_rightViewModel();
                    _model.FormMode = EnumFormModeKey.Form_Edit;
                    _model.job_level_id = (int)job_level;
                    _model.user_type_id = (int)user_type;
                    _model.job_level_list = db.ms_job_level.Where(r => r.fl_active == true && r.deleted_date == null).ToList<ms_job_level>();
                    _model.user_type_list = db.ms_user_type.Where(r => r.fl_active == true && r.deleted_date == null).ToList<ms_user_type>();
                    _model.ms_menus = db.ms_menu.Where(m => m.fl_active == true && m.deleted_date == null).ToList<ms_menu>();
                    int _i = 0;
                    foreach (var m in _model.ms_menus)
                    {
                        Boolean _boolChecked = false;
                        ms_user_rights[] _res = (from t1 in _list
                                                 where t1.job_level_id == (int)job_level
                                                 && t1.user_type_id == (int)user_type
                                                 && t1.menu_id == (int)m.menu_id
                                                 select t1).ToArray();
                        _boolChecked = (_res.Length > 0);
                        if (_boolChecked)
                        {
                            _i++;
                        }
                        _model.checkbox_menu_id.Add(new SelectedMenu_CheckBoxes()
                        {
                            Checked = _boolChecked,
                            menu_id = m.menu_id,
                            ms_menu = m,
                            Text = m.menu_name,
                            Value = m.menu_id.ToString()
                        });
                    }
                }

            }
            else
            {
                //new 
                _model = new user_rightViewModel();
                _model.FormMode = EnumFormModeKey.Form_New;
                _model.job_level_list = db.ms_job_level.Where(r => r.fl_active == true && r.deleted_date == null).ToList<ms_job_level>();
                _model.user_type_list = db.ms_user_type.Where(r => r.fl_active == true && r.deleted_date == null).ToList<ms_user_type>();
                _model.ms_menus = db.ms_menu.Where(m => m.fl_active == true && m.deleted_date == null).ToList<ms_menu>();
                foreach (var m in _model.ms_menus)
                {
                    _model.checkbox_menu_id.Add(new SelectedMenu_CheckBoxes()
                    {
                        Checked = false,
                        menu_id = m.menu_id,
                        ms_menu = m,
                        Text = m.menu_name,
                        Value = m.menu_id.ToString()
                    });
                }
            }

            //List<ms_job_level> ds_job = db.ms_job_level.Where(m => m.fl_active == true && m.deleted_date == null).ToList();
            //ms_job_level jl_opt = new ms_job_level
            //{
            //    job_level_id = (int)job_level,
            //    job_level_code = "Select",
            //    job_level_name = "-- Select Job Level --"
            //};
            //ds_job.Add(jl_opt);
            //var job_level_ddl = new SelectList(ds_job, "job_level_id", "job_level_name", jl_opt);
            //ViewBag.job_level_id = job_level_ddl.OrderBy(c=>c.Value);

            //List<ms_user_type> ds_user_type = db.ms_user_type.Where(m => m.fl_active == true && m.deleted_date == null).ToList();
            //ms_user_type ut_opt = new ms_user_type
            //{
            //    user_type_id = (int)user_type,
            //    user_type_code = "Select",
            //    user_type_name = "-- Select User Right --"
            //};
            //ds_user_type.Add(ut_opt);
            //SelectList user_type_ddl = new SelectList(ds_user_type, "user_type_id", "user_type_name");            
            //ViewBag.user_type_id = user_type_ddl; 

            return View("Create", _model);
        }




        [HttpPost, ActionName("FormSave")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(user_rightViewModel user_right)
        {
            string _ids = Request.Form["menu_id"];
            if (string.IsNullOrWhiteSpace(_ids))
                user_right.selected_menu_id_str = "0,0";
            else
                user_right.selected_menu_id_str = _ids;

            if (ModelState.IsValid)
            {
                ms_user_rights[] _objs = (from m in db.ms_user_rights
                                          where m.job_level_id == user_right.job_level_id
                                         && m.user_type_id == user_right.user_type_id
                                          select m).ToArray();
                if (_objs.Length > 0)
                {
                    IEnumerable<ms_user_rights> _ur = db.ms_user_rights.RemoveRange(_objs);
                    db.SaveChanges();
                }
                ms_user_type user_type_db = db.ms_user_type.Single(u => u.user_type_id == user_right.user_type_id);
                if (user_type_db == null)
                {
                    throw new Exception("Unknown User Right");
                }
                ms_job_level job_level_db = db.ms_job_level.Single(u => u.job_level_id == user_right.job_level_id);
                if (job_level_db == null)
                {
                    throw new Exception("Unknown Job Level");
                }

                string[] arr_menu_id = user_right.selected_menu_id_str.Split(new char[1] { ',' });
                bool bool_menu = false;
                int menu_counter = 0;
                foreach (var menu_id_string in arr_menu_id)
                {
                    int menu_id_int = Convert.ToInt32(menu_id_string);
                    ms_menu menu_db = db.ms_menu.Single(m => m.menu_id == menu_id_int);
                    bool_menu = (menu_db != null);
                    if (bool_menu)
                    {
                        menu_counter++;
                        ms_user_rights rights = new ms_user_rights()
                        {
                            user_type_id = user_right.user_type_id,
                            job_level_id = user_right.job_level_id,
                            menu_id = menu_id_int,

                            fl_active = true,

                            created_date = DateTime.Now,
                            created_by = UserProfile.UserId,

                            updated_date = DateTime.Now,
                            updated_by = UserProfile.UserId,

                            deleted_date = null,
                            deleted_by = null,
                            org_id = UserProfile.OrgId
                        };
                        db.ms_user_rights.Add(rights);
                    }
                }
                if (menu_counter > 0)
                    db.SaveChanges();

                return RedirectToAction("Index");
            }

            List<ms_job_level> ds_job = db.ms_job_level.Where(m => m.fl_active == true && m.deleted_date == null).ToList();
            ms_job_level jl_opt = new ms_job_level
            {
                job_level_id = 0,
                job_level_code = "Select",
                job_level_name = "-- Select Job Level --"
            };
            ds_job.Add(jl_opt);
            var job_level_ddl = new SelectList(ds_job, "job_level_id", "job_level_name", jl_opt);
            ViewBag.job_level_id = job_level_ddl;

            List<ms_user_type> ds_user_type = db.ms_user_type.Where(m => m.fl_active == true && m.deleted_date == null).ToList();
            ms_user_type ut_opt = new ms_user_type
            {
                user_type_id = 0,
                user_type_code = "Select",
                user_type_name = "-- Select User Right --"
            };
            ds_user_type.Add(ut_opt);
            var user_type_ddl = new SelectList(ds_user_type, "user_type_id", "user_type_name", ut_opt);
            ViewBag.user_type_id = user_type_ddl;

            //ViewBag.job_level_id = new SelectList(db.ms_job_level, "job_level_id", "job_level_name", user_right.job_level_id);
            //ViewBag.user_type_id = new SelectList(db.ms_user_type, "user_type_id", "user_type_name", user_right.user_type_id);

            return View("Create", user_right);
        }


    }

    //public static MvcHtmlString DropDownList(this HtmlHelper html, string name, SelectList values, bool canEdit)
    //{
    //    if (canEdit)
    //    {
    //        return html.DropDownList(name, values);
    //    }
    //    return html.DropDownList(name, values, new { disabled = "disabled" });
    //}

}
