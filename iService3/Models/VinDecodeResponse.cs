using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iService3.Models
{
    public class VinDecodeResponse
    {
        public VinDecodeMessage Message { get; set; }
        public VinDecodeData Data { get; set; }
    }

    public class VinDecodeMessage
    {

    }

    public class VinDecodeData
    {
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Engine { get; set; }
        public string Trim { get; set; }
        public string Transmission { get; set; }
    }
}
