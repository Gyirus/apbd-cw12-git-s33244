using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw12.Models;

public partial class Ward
{
    [Key]
    public int Id { get; set; }

    [StringLength(300)]
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    [InverseProperty("Ward")]
    public virtual ICollection<Admission> Admissions { get; set; } = new List<Admission>();

    [InverseProperty("Ward")]
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
