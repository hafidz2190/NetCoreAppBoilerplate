namespace NetCoreApp.Database.Model
{
  public class User : BaseModel
  {
    public string Username { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
  }
}
