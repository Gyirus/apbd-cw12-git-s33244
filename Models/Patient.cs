using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw12.Models;

public partial class Patient
{
    [Key]
    [StringLength(11)]
    [Unicode(false)]
    public string Pesel { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    public int Age { get; set; }

    public bool Sex { get; set; }

    [InverseProperty("PatientPeselNavigation")]
    public virtual ICollection<Admission> Admissions { get; set; } = new List<Admission>();

    [InverseProperty("PatientPeselNavigation")]
    public virtual ICollection<BedAssignment> BedAssignments { get; set; } = new List<BedAssignment>();
}
