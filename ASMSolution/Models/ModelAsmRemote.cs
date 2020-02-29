namespace ASM_UI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelAsmRemote : DbContext
    {
        public ModelAsmRemote()
            : base("name=ModelAsmRemote")
        {
        }

        public virtual DbSet<ms_approval_range> ms_approval_range { get; set; }
        public virtual DbSet<ms_asmin_company> ms_asmin_company { get; set; }
        public virtual DbSet<ms_asset_category> ms_asset_category { get; set; }
        public virtual DbSet<ms_asset_location> ms_asset_location { get; set; }
        public virtual DbSet<ms_asset_register_location> ms_asset_register_location { get; set; }
        public virtual DbSet<ms_asset_register_pic> ms_asset_register_pic { get; set; }
        public virtual DbSet<ms_asset_status> ms_asset_status { get; set; }
        public virtual DbSet<ms_asset_type> ms_asset_type { get; set; }
        public virtual DbSet<ms_country> ms_country { get; set; }
        public virtual DbSet<ms_courier> ms_courier { get; set; }
        public virtual DbSet<ms_currency> ms_currency { get; set; }
        public virtual DbSet<ms_department> ms_department { get; set; }
        public virtual DbSet<ms_depreciation_type> ms_depreciation_type { get; set; }
        public virtual DbSet<ms_disposal_type> ms_disposal_type { get; set; }
        public virtual DbSet<ms_employee> ms_employee { get; set; }
        public virtual DbSet<ms_employee_detail> ms_employee_detail { get; set; }
        public virtual DbSet<ms_insurance> ms_insurance { get; set; }
        public virtual DbSet<ms_job_level> ms_job_level { get; set; }
        public virtual DbSet<ms_menu> ms_menu { get; set; }
        public virtual DbSet<ms_module> ms_module { get; set; }
        public virtual DbSet<ms_request_status> ms_request_status { get; set; }
        public virtual DbSet<ms_user> ms_user { get; set; }
        public virtual DbSet<ms_user_rights> ms_user_rights { get; set; }
        public virtual DbSet<ms_user_type> ms_user_type { get; set; }
        public virtual DbSet<ms_vendor> ms_vendor { get; set; }
        public virtual DbSet<sy_app_setting> sy_app_setting { get; set; }
        public virtual DbSet<sy_email_log> sy_email_log { get; set; }
        public virtual DbSet<sy_lookup> sy_lookup { get; set; }
        public virtual DbSet<sy_message_notification> sy_message_notification { get; set; }
        public virtual DbSet<sy_ref_approval_level> sy_ref_approval_level { get; set; }
        public virtual DbSet<tr_asset_image> tr_asset_image { get; set; }
        public virtual DbSet<tr_asset_insurance> tr_asset_insurance { get; set; }
        public virtual DbSet<tr_asset_license> tr_asset_license { get; set; }
        public virtual DbSet<tr_asset_registration> tr_asset_registration { get; set; }
        public virtual DbSet<tr_asset_taking> tr_asset_taking { get; set; }
        public virtual DbSet<tr_asset_warranty> tr_asset_warranty { get; set; }
        public virtual DbSet<tr_depreciation> tr_depreciation { get; set; }
        public virtual DbSet<tr_depreciation_detail> tr_depreciation_detail { get; set; }
        public virtual DbSet<tr_disposal_announcement> tr_disposal_announcement { get; set; }
        public virtual DbSet<tr_disposal_approval> tr_disposal_approval { get; set; }
        public virtual DbSet<tr_disposal_process> tr_disposal_process { get; set; }
        public virtual DbSet<tr_disposal_request> tr_disposal_request { get; set; }
        public virtual DbSet<tr_mutation_approval> tr_mutation_approval { get; set; }
        public virtual DbSet<tr_mutation_process> tr_mutation_process { get; set; }
        public virtual DbSet<tr_mutation_request> tr_mutation_request { get; set; }
        public virtual DbSet<tr_asset_taking_detail> tr_asset_taking_detail { get; set; }
        public virtual DbSet<tr_depreciation_change_log> tr_depreciation_change_log { get; set; }
        public virtual DbSet<tr_disposal_image> tr_disposal_image { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ms_approval_range>()
                .Property(e => e.range_type)
                .IsUnicode(false);

            modelBuilder.Entity<ms_approval_range>()
                .Property(e => e.range_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_approval_range>()
                .Property(e => e.range_min)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ms_approval_range>()
                .Property(e => e.range_max)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ms_asmin_company>()
                .Property(e => e.company_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asmin_company>()
                .Property(e => e.company_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_category>()
                .Property(e => e.category_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_category>()
                .Property(e => e.category_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_location>()
                .Property(e => e.location_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_location>()
                .Property(e => e.location_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_register_location>()
                .Property(e => e.asset_reg_location_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_register_location>()
                .Property(e => e.asset_reg_location_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_register_pic>()
                .Property(e => e.asset_reg_pic_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_register_pic>()
                .Property(e => e.asset_reg_pic_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_status>()
                .Property(e => e.asset_status_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_status>()
                .Property(e => e.asset_status_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_type>()
                .Property(e => e.asset_type_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_type>()
                .Property(e => e.asset_type_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_type>()
                .HasMany(e => e.tr_asset_registration)
                .WithOptional(e => e.ms_asset_type)
                .HasForeignKey(e => e.asset_type_id);

            modelBuilder.Entity<ms_country>()
                .Property(e => e.country_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_country>()
                .Property(e => e.country_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_courier>()
                .Property(e => e.courier_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_courier>()
                .Property(e => e.courier_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_courier>()
                .Property(e => e.courier_address)
                .IsUnicode(false);

            modelBuilder.Entity<ms_courier>()
                .Property(e => e.courier_phone)
                .IsUnicode(false);

            modelBuilder.Entity<ms_courier>()
                .Property(e => e.courier_mail)
                .IsUnicode(false);

            modelBuilder.Entity<ms_courier>()
                .Property(e => e.courier_description)
                .IsUnicode(false);

            modelBuilder.Entity<ms_currency>()
                .Property(e => e.currency_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_currency>()
                .Property(e => e.currency_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_department>()
                .Property(e => e.department_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_department>()
                .Property(e => e.department_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_department>()
                .Property(e => e.department_email)
                .IsUnicode(false);

            modelBuilder.Entity<ms_depreciation_type>()
                .Property(e => e.depreciation_type_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_depreciation_type>()
                .Property(e => e.depreciation_type_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_disposal_type>()
                .Property(e => e.disposal_type_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_disposal_type>()
                .Property(e => e.disposal_type_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_disposal_type>()
                .HasMany(e => e.tr_disposal_announcement)
                .WithOptional(e => e.ms_disposal_type)
                .HasForeignKey(e => e.approval_disposal_type_id);

            modelBuilder.Entity<ms_employee>()
                .Property(e => e.employee_nik)
                .IsUnicode(false);

            modelBuilder.Entity<ms_employee>()
                .Property(e => e.employee_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_employee>()
                .Property(e => e.employee_email)
                .IsUnicode(false);

            modelBuilder.Entity<ms_employee>()
                .Property(e => e.ip_address)
                .IsUnicode(false);

            modelBuilder.Entity<ms_employee>()
                .HasMany(e => e.ms_user)
                .WithRequired(e => e.ms_employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ms_insurance>()
                .Property(e => e.insurance_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_insurance>()
                .Property(e => e.insurance_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_insurance>()
                .Property(e => e.insurance_address)
                .IsUnicode(false);

            modelBuilder.Entity<ms_insurance>()
                .Property(e => e.insurance_phone)
                .IsUnicode(false);

            modelBuilder.Entity<ms_insurance>()
                .Property(e => e.insurance_mail)
                .IsUnicode(false);

            modelBuilder.Entity<ms_insurance>()
                .Property(e => e.insurance_cp_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_insurance>()
                .Property(e => e.insurance_cp_phone)
                .IsUnicode(false);

            modelBuilder.Entity<ms_insurance>()
                .Property(e => e.insurance_cp_mail)
                .IsUnicode(false);

            modelBuilder.Entity<ms_insurance>()
                .Property(e => e.insurance_description)
                .IsUnicode(false);

            modelBuilder.Entity<ms_job_level>()
                .Property(e => e.job_level_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_job_level>()
                .Property(e => e.job_level_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_job_level>()
                .HasMany(e => e.ms_user_rights)
                .WithRequired(e => e.ms_job_level)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ms_menu>()
                .Property(e => e.menu_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_menu>()
                .Property(e => e.menu_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_menu>()
                .Property(e => e.menu_url)
                .IsUnicode(false);

            modelBuilder.Entity<ms_module>()
                .Property(e => e.module_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_module>()
                .Property(e => e.module_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_request_status>()
                .Property(e => e.request_status_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_request_status>()
                .Property(e => e.request_status_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_user>()
                .Property(e => e.user_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_user>()
                .Property(e => e.user_password)
                .IsUnicode(false);

            modelBuilder.Entity<ms_user_type>()
                .Property(e => e.user_type_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_user_type>()
                .Property(e => e.user_type_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_user_type>()
                .HasMany(e => e.ms_user_rights)
                .WithRequired(e => e.ms_user_type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ms_vendor>()
                .Property(e => e.vendor_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_vendor>()
                .Property(e => e.vendor_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_vendor>()
                .Property(e => e.vendor_address)
                .IsUnicode(false);

            modelBuilder.Entity<ms_vendor>()
                .Property(e => e.vendor_phone)
                .IsUnicode(false);

            modelBuilder.Entity<ms_vendor>()
                .Property(e => e.vendor_mail)
                .IsUnicode(false);

            modelBuilder.Entity<ms_vendor>()
                .Property(e => e.vendor_cp_name)
                .IsUnicode(false);

            modelBuilder.Entity<ms_vendor>()
                .Property(e => e.vendor_cp_phone)
                .IsUnicode(false);

            modelBuilder.Entity<ms_vendor>()
                .Property(e => e.vendor_cp_mail)
                .IsUnicode(false);

            modelBuilder.Entity<ms_vendor>()
                .Property(e => e.vendor_description)
                .IsUnicode(false);

            modelBuilder.Entity<sy_app_setting>()
                .Property(e => e.app_key)
                .IsUnicode(false);

            modelBuilder.Entity<sy_app_setting>()
                .Property(e => e.app_value)
                .IsUnicode(false);

            modelBuilder.Entity<sy_app_setting>()
                .Property(e => e.app_desc)
                .IsUnicode(false);

            modelBuilder.Entity<sy_email_log>()
                .Property(e => e.elog_template)
                .IsUnicode(false);

            modelBuilder.Entity<sy_email_log>()
                .Property(e => e.elog_from)
                .IsUnicode(false);

            modelBuilder.Entity<sy_email_log>()
                .Property(e => e.elog_to)
                .IsUnicode(false);

            modelBuilder.Entity<sy_email_log>()
                .Property(e => e.elog_cc)
                .IsUnicode(false);

            modelBuilder.Entity<sy_email_log>()
                .Property(e => e.elog_bcc)
                .IsUnicode(false);

            modelBuilder.Entity<sy_email_log>()
                .Property(e => e.elog_subject)
                .IsUnicode(false);

            modelBuilder.Entity<sy_email_log>()
                .Property(e => e.elog_body)
                .IsUnicode(false);

            modelBuilder.Entity<sy_email_log>()
                .Property(e => e.elog_file_attachment)
                .IsFixedLength();

            modelBuilder.Entity<sy_email_log>()
                .Property(e => e.err_message)
                .IsUnicode(false);

            modelBuilder.Entity<sy_lookup>()
                .Property(e => e.lkp_code)
                .IsUnicode(false);

            modelBuilder.Entity<sy_lookup>()
                .Property(e => e.lkp_value)
                .IsUnicode(false);

            modelBuilder.Entity<sy_lookup>()
                .Property(e => e.lkp_text)
                .IsUnicode(false);

            modelBuilder.Entity<sy_message_notification>()
                .Property(e => e.notif_group)
                .IsUnicode(false);

            modelBuilder.Entity<sy_message_notification>()
                .Property(e => e.notify_user)
                .IsUnicode(false);

            modelBuilder.Entity<sy_message_notification>()
                .Property(e => e.notify_ip)
                .IsUnicode(false);

            modelBuilder.Entity<sy_message_notification>()
                .Property(e => e.notify_message)
                .IsUnicode(false);

            modelBuilder.Entity<sy_message_notification>()
                .Property(e => e.shown_ip)
                .IsUnicode(false);

            modelBuilder.Entity<sy_message_notification>()
                .Property(e => e.shown_host)
                .IsUnicode(false);

            modelBuilder.Entity<sy_message_notification>()
                .Property(e => e.err_message)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_image>()
                .Property(e => e.asset_img_address)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_insurance>()
                .Property(e => e.insurance_activa_number)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_insurance>()
                .Property(e => e.insurance_activa_name)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_insurance>()
                .Property(e => e.insurance_activa_description)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_license>()
                .Property(e => e.license_number)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_license>()
                .Property(e => e.license_name)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_license>()
                .Property(e => e.license_issued_by)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_license>()
                .Property(e => e.license_description)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_registration>()
                .Property(e => e.asset_number)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_registration>()
                .Property(e => e.asset_po_number)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_registration>()
                .Property(e => e.asset_do_number)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_registration>()
                .Property(e => e.asset_name)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_registration>()
                .Property(e => e.asset_merk)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_registration>()
                .Property(e => e.asset_serial_number)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_registration>()
                .Property(e => e.asset_description)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_registration>()
                .HasMany(e => e.tr_asset_registration1)
                .WithOptional(e => e.tr_asset_registration2)
                .HasForeignKey(e => e.asset_parent_id);

            modelBuilder.Entity<tr_asset_taking>()
                .Property(e => e.file_name)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_warranty>()
                .Property(e => e.warranty_number)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_warranty>()
                .Property(e => e.warranty_item_name)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_warranty>()
                .Property(e => e.warranty_description)
                .IsUnicode(false);

            modelBuilder.Entity<tr_depreciation>()
                .Property(e => e.usd_kurs)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tr_depreciation>()
                .Property(e => e.idr_kurs)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tr_depreciation>()
                .Property(e => e.fis_ddb_percentage)
                .HasPrecision(6, 2);

            modelBuilder.Entity<tr_depreciation>()
                .Property(e => e.mkt_ddb_percentage)
                .HasPrecision(6, 2);

            modelBuilder.Entity<tr_disposal_announcement>()
                .Property(e => e.announcement_description)
                .IsUnicode(false);

            modelBuilder.Entity<tr_disposal_announcement>()
                .Property(e => e.announcement_upload_address)
                .IsUnicode(false);

            modelBuilder.Entity<tr_disposal_announcement>()
                .Property(e => e.fin_announcement_upload_address)
                .IsUnicode(false);

            modelBuilder.Entity<tr_disposal_announcement>()
                .Property(e => e.remove_asset_description)
                .IsUnicode(false);

            modelBuilder.Entity<tr_disposal_approval>()
                .Property(e => e.approval_noted)
                .IsUnicode(false);

            modelBuilder.Entity<tr_disposal_request>()
                .Property(e => e.disposal_number)
                .IsUnicode(false);

            modelBuilder.Entity<tr_disposal_request>()
                .Property(e => e.request_description)
                .IsUnicode(false);

            modelBuilder.Entity<tr_disposal_request>()
                .HasMany(e => e.tr_disposal_announcement)
                .WithRequired(e => e.tr_disposal_request)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tr_disposal_request>()
                .HasMany(e => e.tr_disposal_approval)
                .WithRequired(e => e.tr_disposal_request)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tr_disposal_request>()
                .HasMany(e => e.tr_disposal_image)
                .WithRequired(e => e.tr_disposal_request)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tr_mutation_approval>()
                .Property(e => e.approval_noted)
                .IsUnicode(false);

            modelBuilder.Entity<tr_mutation_process>()
                .Property(e => e.courier_description)
                .IsUnicode(false);

            modelBuilder.Entity<tr_mutation_request>()
                .Property(e => e.request_code)
                .IsUnicode(false);

            modelBuilder.Entity<tr_asset_taking_detail>()
                .Property(e => e.asset_number)
                .IsUnicode(false);

            modelBuilder.Entity<tr_depreciation_change_log>()
                .Property(e => e.variant_mkt_cost)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tr_disposal_image>()
                .Property(e => e.asset_img_address)
                .IsUnicode(false);
        }
    }

}
