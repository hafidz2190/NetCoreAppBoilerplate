using System;
using System.Runtime.Serialization;
using NetCoreApp.Business.DTO;

namespace NetCoreApp.Business
{
  public class ServiceResponseException : Exception
  {
    public ServiceResponse ServiceResponse;

    public ServiceResponseException( ServiceResponseCode code )
    {
      ServiceResponse = new ServiceResponse( code );
    }

    public ServiceResponseException( string message ) : base( message )
    {
    }

    public ServiceResponseException( string message, Exception innerException ) : base( message, innerException )
    {
    }

    protected ServiceResponseException( SerializationInfo info, StreamingContext context ) : base( info, context )
    {
    }
  }

  public class ServiceResponseException<TPayloadType> : Exception
  {
    public ServiceResponse ServiceResponse;

    public ServiceResponseException( ServiceResponseCode code, TPayloadType payload )
    {
      ServiceResponse = new ServiceResponse<TPayloadType>( code, payload );
    }

    public ServiceResponseException( string message ) : base( message )
    {
    }

    public ServiceResponseException( string message, Exception innerException ) : base( message, innerException )
    {
    }

    protected ServiceResponseException( SerializationInfo info, StreamingContext context ) : base( info, context )
    {
    }
  }
}
