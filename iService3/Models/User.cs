using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iService3.Models
{
    public partial class User
    {
        public string UserId { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? Pass { get; set; }

        public string? Token { get; set; }

        public bool? NewsletterSub { get; set; } = false;

        public int FavouriteCar { get; set; }

        public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

        public virtual ICollection<Car> Cars { get; } = new List<Car>();
    }
}
