using System.ComponentModel;

namespace NetCoreApp.Business.DTO
{
  public enum ServiceResponseCode : int
  {
    [Description( "Ok" )]
    Ok = 0,
    [Description( "UserServiceGenericError" )]
    UserServiceGenericError = 100
  }
}
