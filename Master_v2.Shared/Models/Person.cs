using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Master_v2.Shared.Models
{
    public partial class Person
    {
        public Person()
        {
            Store = new HashSet<Store>();
        }
        [Required(ErrorMessage ="Id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Id")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Id")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Id")]
        [StringLength(11)]
        public string Oib { get; set; }
        [Required(ErrorMessage = "Id")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Id")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Id")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Id")]
        public bool IsActive { get; set; }

        public ICollection<Store> Store { get; set; }
    }
}
