using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iService3.Models
{
    public partial class Service
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;
        public ImageSource Photo { get; set; }

        public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();
    }
}
