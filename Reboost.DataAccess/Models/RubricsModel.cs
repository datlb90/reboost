using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Models
{
    public class RubricsModel
    {
        public string Name { get; set; }
        public List<BandScoreDescription> BandScoreDescriptions { get; set; }
    }
    public class BandScoreDescription
    {
        public int BandScore { get; set; }
        public string Description { get; set; }
    }
    public class RubricsQuery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BandScore { get; set; }
        public string Description { get; set; }
    }
}
