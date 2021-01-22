using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace NetCoreApp.Database.Model
{
  public class BaseModel
  {
    public BaseModel()
    {
      Version = 1;
      CreatedTime = DateTime.Now;
      LastModifiedTime = DateTime.Now;
      IsDeleted = false;
    }

    [DataMember] [Key] [DatabaseGenerated( DatabaseGeneratedOption.Identity )] public string Id { get; set; }
    [DataMember] public int Version { get; set; }
    [DataMember] public DateTime CreatedTime { get; set; }
    [DataMember] public DateTime LastModifiedTime { get; set; }
    [DataMember] public bool IsDeleted { get; set; }
  }
}
