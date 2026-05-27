using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw12.Models;

public partial class Bed
{
    [Key]
    public int Id { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string RoomId { get; set; } = null!;

    public int BedTypeId { get; set; }

    [InverseProperty("Bed")]
    public virtual ICollection<BedAssignment> BedAssignments { get; set; } = new List<BedAssignment>();

    [ForeignKey("BedTypeId")]
    [InverseProperty("Beds")]
    public virtual BedType BedType { get; set; } = null!;

    [ForeignKey("RoomId")]
    [InverseProperty("Beds")]
    public virtual Room Room { get; set; } = null!;
}
