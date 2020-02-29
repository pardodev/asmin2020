namespace CustomAuthenticationMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TCustomer")]
    public partial class TCustomer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cust_id { get; set; }

        [StringLength(50)]
        public string cust_name { get; set; }

        [StringLength(50)]
        public string cust_phone { get; set; }

        [StringLength(100)]
        public string cust_address { get; set; }

        [StringLength(50)]
        public string cust_email { get; set; }
    }
}
