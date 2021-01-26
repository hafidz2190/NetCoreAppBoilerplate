using System.Threading.Tasks;

namespace NetCoreApp.Business
{
  public interface IBroadcasterService
  {
    Task Send( string method, object arg );
  }
}
