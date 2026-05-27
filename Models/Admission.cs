using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw12.Models;

public partial class Admission
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AdmissionDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DischargeDate { get; set; }

    [StringLength(11)]
    [Unicode(false)]
    public string PatientPesel { get; set; } = null!;

    public int WardId { get; set; }

    [ForeignKey("PatientPesel")]
    [InverseProperty("Admissions")]
    public virtual Patient PatientPeselNavigation { get; set; } = null!;

    [ForeignKey("WardId")]
    [InverseProperty("Admissions")]
    public virtual Ward Ward { get; set; } = null!;
}
