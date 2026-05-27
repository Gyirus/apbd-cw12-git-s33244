namespace apbd_cw12.DTOs;

public class BedAssignmentRequestDto
{
    public DateTime From { get; set; }
    public DateTime? To { get; set; }  
    public string BedType { get; set; }
    public string Ward { get; set; }
}