using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw12.Models;

public partial class BedAssignment
{
    [Key]
    public int Id { get; set; }

    [StringLength(11)]
    [Unicode(false)]
    public string PatientPesel { get; set; } = null!;

    public int BedId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime From { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? To { get; set; }

    [ForeignKey("BedId")]
    [InverseProperty("BedAssignments")]
    public virtual Bed Bed { get; set; } = null!;

    [ForeignKey("PatientPesel")]
    [InverseProperty("BedAssignments")]
    public virtual Patient PatientPeselNavigation { get; set; } = null!;
}
