using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw12.Models;

public partial class BedType
{
    [Key]
    public int Id { get; set; }

    [StringLength(300)]
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    [InverseProperty("BedType")]
    public virtual ICollection<Bed> Beds { get; set; } = new List<Bed>();
}
