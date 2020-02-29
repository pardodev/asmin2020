namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_disposal_process
    {
        [Key]
        public int disposal_id { get; set; }

        public int? request_id { get; set; }

        public bool? fl_pic_asset_comfirm { get; set; }

        public DateTime? pic_asset_confirm_date { get; set; }

        public int? pic_asset_employee_id { get; set; }

        public int? pic_asset_level_id { get; set; }

        public DateTime? pic_asset_received_date { get; set; }

        public int? pic_asset_received_employee_id { get; set; }

        public int? pic_asset_received_level_id { get; set; }

        public int? disposal_suggestion_id { get; set; }

        public int? disposal_sent_dept_id { get; set; }

        public DateTime? disposal_sent_date { get; set; }

        public int? user_asset_received_employee_id { get; set; }

        public int? user_asset_received_level_id { get; set; }

        public DateTime? user_asset_received_date { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

        public int? org_id { get; set; }
    }
}