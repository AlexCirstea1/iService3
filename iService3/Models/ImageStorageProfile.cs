using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iService3.Models
{
    internal class ImageStorageProfile
    {
        private static ImageSource _profilePicture;

        public static ImageSource ProfilePicture
        {
            get { return _profilePicture; }
            set { _profilePicture = value; }
        }
    }
}
