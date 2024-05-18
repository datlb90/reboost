using System;
namespace Reboost.DataAccess.Entities
{
    public class Plans : BaseEntity
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public int Price { get; set; }
    }
}

