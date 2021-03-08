using System;
using System.Collections.Generic;
using System.Text;

namespace ABTRS.Model
{
    public class Seat
    {
        public int show_id { get; set; }

        public int row { get; set; }

        public int seat { get; set; }

        public char hor_path { get; set; }

        public char ver_path { get; set; }

        public string first_name { get; set; }

        public string last_name_prefix { get; set; }

        public string last_name { get; set; }

        public string e_mail { get; set; }

        public string status { get; set; }

        public string date_time_reservation { get; set; }

        public long order_id { get; set; }
    }
}
