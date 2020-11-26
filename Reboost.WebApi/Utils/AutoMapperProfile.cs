using AutoMapper;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reboost.WebApi.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RaterRequestModel, Raters>();
            CreateMap<Raters, RaterResponseModel>();
        }
    }
}
