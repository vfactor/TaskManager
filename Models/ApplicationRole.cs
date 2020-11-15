using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
  public class ApplicationRole
  {
    [Required(ErrorMessage = "Role name is required")]
    [StringLength(256)]
    public string Name { get; set; }    
  }
}
