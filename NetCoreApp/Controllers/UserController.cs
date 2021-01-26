using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Business;
using NetCoreApp.Database.Model;

namespace NetCoreApp.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  public class UserController : ControllerBase
  {
    public readonly IUserService _userService;

    public UserController( IUserService userService )
    {
      _userService = userService;
    }

    [HttpPost( "create" )]
    public dynamic Create( User user )
    {
      return _userService.Create( user );
    }

    [HttpGet( "read/{id}" )]
    public dynamic Read( string id )
    {
      return _userService.Read( id );
    }

    [HttpGet( "read" )]
    public dynamic Read()
    {
      return _userService.Read();
    }

    [HttpPost( "update" )]
    public dynamic Update( UpdateUserArg arg )
    {
      return _userService.Update( arg.Id, arg.PropertyMap );
    }

    [HttpPost( "delete/{id}" )]
    public dynamic Delete( string id )
    {
      return _userService.Delete( id );
    }
  }

  public class UpdateUserArg
  {
    public string Id { get; set; }
    public Dictionary<string, object> PropertyMap { get; set; }
  }
}