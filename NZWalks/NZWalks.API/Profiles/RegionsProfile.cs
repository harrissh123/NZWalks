﻿using AutoMapper;
using System.ComponentModel.Design;

namespace NZWalks.API.Profiles
{
    public class RegionsProfile: Profile
    {
        public RegionsProfile()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
            //  .ForMember(dest => dest.Id, options => options.MapFrom(src => src.RegionId));  Example of Mapping srce and dest\
            .ReverseMap();
        }
    }
}
