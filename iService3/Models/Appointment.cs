using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iService3.Models
{
    public partial class Appointment
    {
        public int AppointmentId { get; set; }

        public int CarId { get; set; }

        public int UserId { get; set; }
        public string ServiceName { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string AppointmentType { get; set; } = null!;

        public string? AppointmentNotes { get; set; }

        public int ServiceId { get; set; }
        public bool isConfirmed { get; set; }
        public string Status { get; set; } = null!;
        public virtual Car Car { get; set; } = null!;

        public virtual Service Service { get; set; } = null!;

        public virtual User User { get; set; } = null!;
        public string CarName { get; set; }
    }
}