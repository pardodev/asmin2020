namespace ASM_UI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelAsmRemote : DbContext
    {
        public ModelAsmRemote()
            : base("name=AMSDB")
        {
        }

        public virtual DbSet<ms_approval_range> ms_approval_range { get; set; }
        public virtual DbSet<ms_approval_status> ms_approval_status { get; set; }
        public virtual DbSet<ms_asmin_company> ms_asmin_company { get; set; }
        public virtual DbSet<ms_asset_category> ms_asset_category { get; set; }
        public virtual DbSet<ms_asset_location> ms_asset_location { get; set; }
        public virtual DbSet<ms_asset_register_location> ms_asset_register_location { get; set; }
        public virtual DbSet<ms_asset_register_pic> ms_asset_register_pic { get; set; }
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
        public virtual DbSet<ms_user> ms_user { get; set; }
        public virtual DbSet<ms_user_rights> ms_user_rights { get; set; }
        public virtual DbSet<ms_user_type> ms_user_type { get; set; }
        public virtual DbSet<ms_vendor> ms_vendor { get; set; }
        public virtual DbSet<tr_asset_image> tr_asset_image { get; set; }
        public virtual DbSet<tr_asset_insurance> tr_asset_insurance { get; set; }
        public virtual DbSet<tr_asset_license> tr_asset_license { get; set; }
        public virtual DbSet<tr_asset_registration> tr_asset_registration { get; set; }
        public virtual DbSet<tr_asset_warranty> tr_asset_warranty { get; set; }
        public virtual DbSet<tr_depreciation> tr_depreciation { get; set; }
        public virtual DbSet<tr_depreciation_detail> tr_depreciation_detail { get; set; }
        public virtual DbSet<tr_disposal_approval> tr_disposal_approval { get; set; }
        public virtual DbSet<tr_disposal_bap> tr_disposal_bap { get; set; }
        public virtual DbSet<tr_disposal_process> tr_disposal_process { get; set; }
        public virtual DbSet<tr_disposal_request> tr_disposal_request { get; set; }
        public virtual DbSet<tr_mutation_approval> tr_mutation_approval { get; set; }
        public virtual DbSet<tr_mutation_process> tr_mutation_process { get; set; }
        public virtual DbSet<tr_mutation_request> tr_mutation_request { get; set; }
        public virtual DbSet<ms_country_x> ms_country_x { get; set; }

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

            modelBuilder.Entity<ms_approval_status>()
                .Property(e => e.approval_status_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_approval_status>()
                .Property(e => e.approval_status_name)
                .IsUnicode(false);

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

            modelBuilder.Entity<ms_asset_type>()
                .Property(e => e.asset_type_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_asset_type>()
                .Property(e => e.asset_type_name)
                .IsUnicode(false);

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
                .Property(e => e.Currency_kurs)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tr_depreciation>()
                .Property(e => e.fis_ddb_precentage)
                .HasPrecision(6, 2);

            modelBuilder.Entity<tr_depreciation>()
                .Property(e => e.mkt_ddb_percentage)
                .HasPrecision(6, 2);

            modelBuilder.Entity<tr_disposal_approval>()
                .Property(e => e.approval_noted)
                .IsUnicode(false);

            modelBuilder.Entity<tr_disposal_bap>()
                .Property(e => e.disposal_bap_description)
                .IsUnicode(false);

            modelBuilder.Entity<tr_disposal_bap>()
                .Property(e => e.disposal_upload_address)
                .IsUnicode(false);

            modelBuilder.Entity<tr_mutation_approval>()
                .Property(e => e.approval_noted)
                .IsUnicode(false);

            modelBuilder.Entity<tr_mutation_process>()
                .Property(e => e.courier_description)
                .IsUnicode(false);

            modelBuilder.Entity<ms_country_x>()
                .Property(e => e.country_code)
                .IsUnicode(false);

            modelBuilder.Entity<ms_country_x>()
                .Property(e => e.country_name)
                .IsUnicode(false);
        }
    }
}
