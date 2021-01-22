using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace NetCoreApp.Business
{
  public static class ServiceHelper
  {
    public static T UnboxProperty<T>( T model, Dictionary<string, object> propertyMap, List<string> excludedKeys ) where T : class
    {
      List<string> readOnlyKeys = new List<string>
          {
            "Id",
            "Version",
            "CreatedTime",
            "LastModifiedTime",
            "IsDeleted"
          };

      readOnlyKeys.AddRange( excludedKeys );

      foreach( var kvp in propertyMap )
      {
        // https://stackoverflow.com/questions/9404523/set-property-value-using-property-name
        Type type = typeof( T );
        PropertyInfo propertyInfo = type.GetProperty( kvp.Key );

        if( readOnlyKeys.Contains( kvp.Key ) || propertyInfo == null )
        {
          continue;
        }

        propertyInfo.SetValue( model, kvp.Value );
      }

      return model;
    }

    public static IList<T> GetList<T>( IRepository<T> repo, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null ) where T : class
    {
      return repo.GetPagedList( filter, orderBy, include, 0, int.MaxValue, false ).Items;
    }

    public static T GetFirstOrDefault<T>( IRepository<T> repo, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null ) where T : class
    {
      return repo.GetFirstOrDefault( filter, orderBy, include, false );
    }
  }
}
