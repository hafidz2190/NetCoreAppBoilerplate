using System.Collections.Generic;
using NetCoreApp.Business.DTO;
using NetCoreApp.Database.Model;

namespace NetCoreApp.Business
{
  public interface IUserService
  {
    ServiceResponse<User> Create( User user );
    ServiceResponse<User> Read( string id );
    ServiceResponse<List<User>> Read();
    ServiceResponse<User> Update( string id, Dictionary<string, object> propertyMap );
    ServiceResponse<bool> Delete( string id );
  }
}
