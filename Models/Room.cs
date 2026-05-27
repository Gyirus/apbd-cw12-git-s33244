using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw12.Models;

public partial class Room
{
    [Key]
    [StringLength(4)]
    [Unicode(false)]
    public string Id { get; set; } = null!;

    public int WardId { get; set; }

    public bool HasTv { get; set; }

    [InverseProperty("Room")]
    public virtual ICollection<Bed> Beds { get; set; } = new List<Bed>();

    [ForeignKey("WardId")]
    [InverseProperty("Rooms")]
    public virtual Ward Ward { get; set; } = null!;
}
