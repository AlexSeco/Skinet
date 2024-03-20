using System.Reflection;
using AutoMapper;

namespace SkinetAPI.Helpers;



public class UrlResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, string>
{
    private readonly string _propertyName;
    

    public string url = "https://localhost:5001/Content/";

    public UrlResolver(string propertyName)
    {
        _propertyName = propertyName;
    }

    public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context)
    {
        PropertyInfo propertyInfo = typeof(TSource).GetProperty(_propertyName);
        if (propertyInfo != null)
        {
            var propertyValue = propertyInfo.GetValue(source)?.ToString();
            if (!string.IsNullOrEmpty(propertyValue))
            {
                return url + propertyValue;
            }
        }
        return null;
    }
}