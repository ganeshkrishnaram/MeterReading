using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ensek.MeterReading.Api.Data.Model
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public ICollection<MeterReading> MeterReading { get; set; }
        
    }
}