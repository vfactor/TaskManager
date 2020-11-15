using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
  public class UserManager
  {    
    public int Id { get; set; }

    public virtual IdentityUser User { get; set; }    
    public virtual IdentityUser Manager { get; set; }
  }
}
