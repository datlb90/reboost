using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Models
{
    public class GetSubscriptionsModel
    {
        public string MonthSub { get; set; }
        public bool IsMonthExpired { get; set; }
        public string YearSub { get; set; }
        public bool IsYearExpired { get; set; }
    }
}
