using Microsoft.AspNetCore.Identity;

namespace TaskManager.Models
{
  public class ToDo
  {    
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsDone { get; set; }

    public virtual IdentityUser User { get; set;}    
  }
}
