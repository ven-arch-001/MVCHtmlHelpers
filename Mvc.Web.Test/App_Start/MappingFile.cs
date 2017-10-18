using AutoMapper;
using AutoMapper.Configuration;
using Mvc.Web.Test.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Web.Test
{

    public static class AutoMapperServiceConfig
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
 (this IMappingExpression<TSource, TDestination> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }

        public static IMapper Mapper;
        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingFile());
            });
            Mapper = config.CreateMapper();
           
        }
    }


    public class MappingFile : AutoMapper.Profile
    {      

        public MappingFile()
        {
            IMappingExpression<Models.EF.User, ViewModel.UserVM> 
                mappingExpression;
            mappingExpression = CreateMap<Models.EF.User, ViewModel.UserVM>();
            mappingExpression.IgnoreAllNonExisting();
            mappingExpression.ForMember(d => d.Gender, o => o.Ignore());
            mappingExpression.ForMember(d => d.State, o => o.Ignore());


            CreateMap<ViewModel.UserVM, Models.EF.User>()
                  .IgnoreAllNonExisting()                  
                .ForMember(d => d.Gender, o => o.Ignore())
                .ForMember(d => d.State, o => o.Ignore());

            CreateMap<Gender, SelectListItem>()
              .IgnoreAllNonExisting()
            .ForMember(d => d.Text, o => o.MapFrom(t => t.Text))
            .ForMember(d => d.Value, o => o.MapFrom(t => t.GenderId));


            CreateMap<State, SelectListItem>()
             .IgnoreAllNonExisting()
           .ForMember(d => d.Text, o => o.MapFrom(t => t.Text))
           .ForMember(d => d.Value, o => o.MapFrom(t => t.StateId));
        }
    }

    
}