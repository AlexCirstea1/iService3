using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iService3.Models
{
    internal class ImageStorage
    {
        private static ImageSource _carImage;
        

        public static ImageSource CarImage
        {
            get { return _carImage; }
            set { _carImage = value; }
        }


        public static void RemoveCarImage()
        {
            _carImage = null;
        }
    }
}
