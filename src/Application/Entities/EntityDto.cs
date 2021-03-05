using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Entities
{
    public class EntityDto : IMapFrom<Entity>
    {
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
        public double Factor { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Entity, EntityDto>();
        }
    }
}