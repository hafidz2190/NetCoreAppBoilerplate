using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NetCoreApp.Business;
using NetCoreApp.Hubs;

namespace NetCoreApp
{
  public class BroadcasterService : IBroadcasterService
  {
    public readonly IHubContext<MainHub> _mainHubContext;

    public BroadcasterService( IHubContext<MainHub> mainHubContext )
    {
      _mainHubContext = mainHubContext;
    }

    public Task Send( string method, object arg )
    {
      return _mainHubContext.Clients.All.SendAsync( method, arg );
    }
  }
}
