using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Models
{
    public class RubricsModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasScore { get; set; }
        public int OrderId { get; set; }
        public List<BandScoreDescription> BandScoreDescriptions { get; set; }
    }
    public class BandScoreDescription
    {
        public int Id { get; set; }
        public int BandScore { get; set; }
        public string Description { get; set; }
    }
    public class RubricsQuery
    {
        public int Id { get; set; }
        public int CriteriaId { get; set; }
        public string CriteriaDescription { get; set; }
        public bool HasScore { get; set; }
        public string Name { get; set; }
        public int BandScore { get; set; }
        public string Description { get; set; }
        public int OrderId { get; set; }
    }
}
