using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace NetCoreApp.Business.DTO
{
  [DataContract]
  public class ServiceResponse
  {
    public ServiceResponse( ServiceResponseCode code )
    {
      ServiceResponseCode = code;
    }

    public ServiceResponse( ServiceResponseCode code, Exception e )
    {
      ServiceResponseCode = code;
      Exception = e;
    }

    [DataMember]
    public ServiceResponseCode ServiceResponseCode { get; protected set; }

    [DataMember]
    public int Code { get { return (int)ServiceResponseCode; } }

    [DataMember]
    public string Description { get { return GetEnumDescription( ServiceResponseCode ); } }

    [DataMember]
    public Exception Exception { get; protected set; }

    public override string ToString()
    {
      return $"Code: {Code}";
    }

    // https://stackoverflow.com/a/11959512
    private string GetEnumDescription( ServiceResponseCode value )
    {
      // Get the Description attribute value for the enum value
      FieldInfo fi = value.GetType().GetField( value.ToString() );
      DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes( typeof( DescriptionAttribute ), false );

      if( attributes.Length > 0 )
      {
        return attributes[0].Description;
      }

      return value.ToString();
    }
  }

  [DataContract]
  public class ServiceResponse<TPayloadType> : ServiceResponse
  {
    public ServiceResponse( ServiceResponseCode code, TPayloadType payload ) : base( code )
    {
      Payload = payload;
    }

    public ServiceResponse( ServiceResponseCode code ) : base( code )
    {
      Payload = default( TPayloadType );
    }

    public ServiceResponse( TPayloadType payload ) : base( ServiceResponseCode.Ok )
    {
      Payload = payload;
    }

    public ServiceResponse( ServiceResponseCode code, TPayloadType payload, Exception e ) : base( code, e )
    {
      Payload = payload;
    }

    public ServiceResponse( ServiceResponseCode code, Exception e ) : base( code, e )
    {
      Payload = default( TPayloadType );
    }

    [DataMember]
    public TPayloadType Payload { get; protected set; }

    public override string ToString()
    {
      return $"Code: {Code}; Description: {Description}; Payload: {Payload}";
    }
  }
}
