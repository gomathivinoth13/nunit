using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.CustomerWebService.Core.ExtensionMethods
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNullMembers<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            expression.ForAllMembers(opt => opt.Condition(src => src != null));
            return expression;
        }
    }
}
