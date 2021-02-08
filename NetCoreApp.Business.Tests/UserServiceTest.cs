using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NetCoreApp.Database;
using NetCoreApp.Database.Model;
using System.Collections.Generic;
using Xunit;

namespace NetCoreApp.Business.Tests
{
  public class UserServiceTest
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public UserServiceTest()
    {
      var dbContextOptionsBuilder = new DbContextOptionsBuilder<DatabaseContext>()
        .UseInMemoryDatabase( "test" )
        .ConfigureWarnings( e => e.Ignore( InMemoryEventId.TransactionIgnoredWarning ) );

      var dbContext = new DatabaseContext( dbContextOptionsBuilder.Options );
      _unitOfWork = new UnitOfWork<DatabaseContext>( dbContext );

      _userService = new UserService( _unitOfWork, dbContext, null );

      ClearData();
      SeedData();
    }

    private void ClearData()
    {
      var userRepo = _unitOfWork.GetRepository<User>();
      var users = userRepo.GetPagedList();

      userRepo.Delete( users.Items );
    }

    private void SeedData()
    {
      var userRepo = _unitOfWork.GetRepository<User>();

      var user1 = new User { Id = "1", Username = "11", Name = "111", Email = "1111" };
      var user2 = new User { Id = "2", Username = "22", Name = "222", Email = "2222" };
      var user3 = new User { Id = "3", Username = "33", Name = "333", Email = "3333" };

      userRepo.Insert( user1 );
      userRepo.Insert( user2 );
      userRepo.Insert( user3 );

      _unitOfWork.SaveChanges();
    }

    [Fact]
    public void Create()
    {
      var user = new User { Username = "9", Name = "99", Email = "999" };

      var response = _userService.Create( user );
      var createdUser = response.Payload;

      Assert.True(
        createdUser.Username == user.Username &&
        createdUser.Name == user.Name &&
        createdUser.Email == user.Email
      );
    }

    [Fact]
    public void Read()
    {
      var response = _userService.Read( "1" );
      var user = response.Payload;

      Assert.True( user.Username == "11" );
    }

    [Fact]
    public void ReadAll()
    {
      var response = _userService.Read();
      var createdUsers = response.Payload;

      Assert.True( createdUsers.Count == 3 );
    }

    [Fact]
    public void Update()
    {
      var readResponse = _userService.Read( "2" );
      var existingUser = readResponse.Payload;

      var propertyMap = new Dictionary<string, object>
      {
        ["Username"] = "77",
        ["Name"] = "777",
        ["Email"] = "7777"
      };

      var updateResponse = _userService.Update( existingUser.Id, propertyMap );
      var updatedUser = updateResponse.Payload;

      Assert.True(
        updatedUser.Username == "77" &&
        updatedUser.Name == "777" &&
        updatedUser.Email == "7777"
      );
    }

    [Fact]
    public void Delete()
    {
      _userService.Delete( "2" );

      var response = _userService.Read( "2" );
      var user = response.Payload;

      Assert.True( user == null );
    }
  }
}
