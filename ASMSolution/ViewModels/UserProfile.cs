using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ASM_UI.Models
{
    public class USER_PROFILE
    {
        #region diisi dari FormLogin di depan
        [Key]
        public string SESSION_ID { get; set; }

        public DateTime SESSION_CREATED { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int? company_id { get; set; }

        public int OrgId { get { return (int)this.company_id; } }

        public int? asset_reg_location_id { get; set; }

        #endregion
        public ms_asset_register_location register_location { get; set; }

        public string UserFullName { get; set; }

        public int employee_id { get; set; }

        public ms_employee ms_employee { get; set; }

        public string CompanyName { get; set; }


        public int? department_id { get; set; }

        public ms_department ms_department { get; set; }


        public bool? fl_approver { get; set; }

        public int? range_id { get; set; }

        public ms_approval_range ms_approval_range { get; set; }


        public int? job_level_id { get; set; }

        public ms_job_level ms_job_level { get; set; }


        public int? user_type_id { get; set; }

        public ms_user_type ms_user_type { get; set; }


        public int? location_id { get; set; }

        public ms_asset_location ms_asset_location { get; set; }
        public string location_name { get; set; }

    }
}