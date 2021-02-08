using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetCoreApp.Business.DTO;
using NetCoreApp.Database;
using NetCoreApp.Database.Model;

namespace NetCoreApp.Business
{
  public class UserService : IUserService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _databaseContext;
    private readonly IBroadcasterService _broadcasterService;

    public UserService( IUnitOfWork unitOfWork, DatabaseContext databaseContext, IBroadcasterService broadcasterService )
    {
      _unitOfWork = unitOfWork;
      _databaseContext = databaseContext;
      _broadcasterService = broadcasterService;
    }

    public ServiceResponse<User> Create( User user )
    {
      User createdUser = null;

      using( var transaction = _databaseContext.Database.BeginTransaction() )
      {
        try
        {
          var userRepo = _unitOfWork.GetRepository<User>();

          userRepo.Insert( user );
          _unitOfWork.SaveChanges();
          transaction.Commit();

          createdUser = ServiceHelper.GetFirstOrDefault( userRepo, e => !e.IsDeleted, null, source => source.OrderByDescending( e => e.CreatedTime ) );

          OnUserDataUpdated( true );
        }
        catch( ServiceResponseException e )
        {
          transaction.Rollback();

          return new ServiceResponse<User>( e.ServiceResponse.ServiceResponseCode, null, null );
        }
        catch( Exception e )
        {
          transaction.Rollback();

          return new ServiceResponse<User>( ServiceResponseCode.UserServiceGenericError, null, e );
        }
      }

      return new ServiceResponse<User>( ServiceResponseCode.Ok, createdUser );
    }

    public ServiceResponse<User> Read( string id )
    {
      User user = null;

      try
      {
        var userRepo = _unitOfWork.GetRepository<User>();

        user = ServiceHelper.GetFirstOrDefault( userRepo, e => e.Id == id && !e.IsDeleted );
      }
      catch( ServiceResponseException e )
      {
        return new ServiceResponse<User>( e.ServiceResponse.ServiceResponseCode, null, null );
      }
      catch( Exception e )
      {
        return new ServiceResponse<User>( ServiceResponseCode.UserServiceGenericError, null, e );
      }

      return new ServiceResponse<User>( ServiceResponseCode.Ok, user );
    }

    public ServiceResponse<List<User>> Read()
    {
      List<User> users = new List<User>();

      try
      {
        var userRepo = _unitOfWork.GetRepository<User>();

        users = ServiceHelper.GetList( userRepo, e => !e.IsDeleted ).ToList();
      }
      catch( ServiceResponseException e )
      {
        return new ServiceResponse<List<User>>( e.ServiceResponse.ServiceResponseCode, null, null );
      }
      catch( Exception e )
      {
        return new ServiceResponse<List<User>>( ServiceResponseCode.UserServiceGenericError, null, e );
      }

      return new ServiceResponse<List<User>>( ServiceResponseCode.Ok, users );
    }

    public ServiceResponse<User> Update( string id, Dictionary<string, object> propertyMap )
    {
      User updatedUser = null;

      using( var transaction = _databaseContext.Database.BeginTransaction() )
      {
        try
        {
          var userRepo = _unitOfWork.GetRepository<User>();

          var existingUser = ServiceHelper.GetFirstOrDefault( userRepo, e => e.Id == id && !e.IsDeleted, null, null );

          if( existingUser == null )
          {
            throw new ServiceResponseException( ServiceResponseCode.UserServiceGenericError );
          }

          var user = ServiceHelper.UnboxProperty( existingUser, propertyMap, null );

          user.Version = user.Version + 1;
          user.LastModifiedTime = DateTime.Now;

          userRepo.Update( user );
          _unitOfWork.SaveChanges();
          transaction.Commit();

          updatedUser = ServiceHelper.GetFirstOrDefault( userRepo, e => e.Id == id && !e.IsDeleted, null, null );

          OnUserDataUpdated( true );
        }
        catch( ServiceResponseException e )
        {
          transaction.Rollback();

          return new ServiceResponse<User>( e.ServiceResponse.ServiceResponseCode, null, null );
        }
        catch( Exception e )
        {
          transaction.Rollback();

          return new ServiceResponse<User>( ServiceResponseCode.UserServiceGenericError, null, e );
        }
      }

      return new ServiceResponse<User>( ServiceResponseCode.Ok, updatedUser );
    }

    public ServiceResponse<bool> Delete( string id )
    {
      using( var transaction = _databaseContext.Database.BeginTransaction() )
      {
        try
        {
          var userRepo = _unitOfWork.GetRepository<User>();

          var existingUser = ServiceHelper.GetFirstOrDefault( userRepo, e => e.Id == id && !e.IsDeleted, null, null );

          if( existingUser == null )
          {
            throw new ServiceResponseException( ServiceResponseCode.UserServiceGenericError );
          }

          existingUser.Version = existingUser.Version + 1;
          existingUser.LastModifiedTime = DateTime.Now;
          existingUser.IsDeleted = true;

          userRepo.Update( existingUser );
          _unitOfWork.SaveChanges();
          transaction.Commit();

          OnUserDataUpdated( true );
        }
        catch( ServiceResponseException e )
        {
          transaction.Rollback();

          return new ServiceResponse<bool>( e.ServiceResponse.ServiceResponseCode, false, null );
        }
        catch( Exception e )
        {
          transaction.Rollback();

          return new ServiceResponse<bool>( ServiceResponseCode.UserServiceGenericError, false, e );
        }
      }

      return new ServiceResponse<bool>( ServiceResponseCode.Ok, true );
    }

    public ServiceResponse<bool> Delete( List<string> ids )
    {
      using( var transaction = _databaseContext.Database.BeginTransaction() )
      {
        try
        {
          var userRepo = _unitOfWork.GetRepository<User>();

          var existingUsers = ServiceHelper.GetList( userRepo, e => ids.Contains( e.Id ) && !e.IsDeleted, null, null );

          if( existingUsers.Count == 0 )
          {
            return new ServiceResponse<bool>( ServiceResponseCode.Ok, true );
          }

          foreach( var existingUser in existingUsers )
          {
            existingUser.Version = existingUser.Version + 1;
            existingUser.LastModifiedTime = DateTime.Now;
            existingUser.IsDeleted = true;

            userRepo.Update( existingUser );
          }

          _unitOfWork.SaveChanges();
          transaction.Commit();

          OnUserDataUpdated( true );
        }
        catch( ServiceResponseException e )
        {
          transaction.Rollback();

          return new ServiceResponse<bool>( e.ServiceResponse.ServiceResponseCode, false, null );
        }
        catch( Exception e )
        {
          transaction.Rollback();

          return new ServiceResponse<bool>( ServiceResponseCode.UserServiceGenericError, false, e );
        }
      }

      return new ServiceResponse<bool>( ServiceResponseCode.Ok, true );
    }

    private void OnUserDataUpdated(object arg)
    {
      _broadcasterService?.Send( "OnUserDataUpdated", arg );
    }
  }
}
