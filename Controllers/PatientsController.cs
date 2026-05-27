using apbd_cw12.Data;
using apbd_cw12.DTOs;
using apbd_cw12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly HospitalDbContext _context;

    public PatientsController(HospitalDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientResponseDto>>> GetPatients([FromQuery] string? search)
    {
        var query = _context.Patients
            .Include(p => p.Admissions)
                .ThenInclude(a => a.Ward)
            .Include(p => p.BedAssignments)
                .ThenInclude(ba => ba.Bed)
                    .ThenInclude(b => b.BedType)
            .Include(p => p.BedAssignments)
                .ThenInclude(ba => ba.Bed)
                    .ThenInclude(b => b.Room)
                        .ThenInclude(r => r.Ward)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.FirstName.Contains(search) || p.LastName.Contains(search));
        }

        var patients = await query.ToListAsync();

        var result = patients.Select(p => new PatientResponseDto
        {
            Pesel = p.Pesel,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Age = p.Age,
            Sex = p.Sex ? "Male" : "Female",
            Admissions = p.Admissions.Select(a => new AdmissionDto
            {
                Id = a.Id,
                AdmissionDate = a.AdmissionDate,
                DischargeDate = a.DischargeDate ?? DateTime.MinValue, 
                Ward = new WardDto
                {
                    Id = a.Ward.Id,
                    Name = a.Ward.Name,
                    Description = a.Ward.Description
                }
            }).ToList(),
            BedAssignments = p.BedAssignments.Select(bd => new BedAssignmentDto
            {
                Id = bd.Id,
                From = bd.From,
                To = bd.To,
                Bed = new BedDto
                {
                    Id = bd.Bed.Id,
                    BedType = new BedTypeDto
                    {
                        Id = bd.Bed.BedType.Id,
                        Name = bd.Bed.BedType.Name,
                        Description = bd.Bed.BedType.Description
                    },
                    Room = new RoomDto
                    {
                        Id = bd.Bed.Room.Id,
                        HasTv = bd.Bed.Room.HasTv,
                        Ward = new WardDto
                        {
                            Id = bd.Bed.Room.Ward.Id,
                            Name = bd.Bed.Room.Ward.Name,
                            Description = bd.Bed.Room.Ward.Description
                        }
                    }
                }
            }).ToList()
        }).ToList();

        return Ok(result);
    }

    [HttpPost("{pesel}/bedassignments")]
    public async Task<ActionResult> AssignBed(string pesel, [FromBody] BedAssignmentRequestDto request)
    {
        var patient = await _context.Patients.FindAsync(pesel);

        if (patient == null)
        {
            return NotFound($"Patient with PESEL '{pesel}' not found.");
        }

        if (request.To.HasValue && request.To.Value <= request.From)
            return BadRequest("'to' date must be greater than 'from' date.");

        var potentialBeds = await _context.Beds
            .Include(b => b.BedType)
            .Include(b => b.Room)
                .ThenInclude(r => r.Ward)
            .Where(b => b.BedType.Name == request.BedType && b.Room.Ward.Name == request.Ward)
            .ToListAsync(); 

        if (!potentialBeds.Any())
            return NotFound($"No beds of type '{request.BedType}' found in ward '{request.Ward}'.");

        Bed freeBed = null;
        foreach (var bed in potentialBeds)
        {
            await _context.Entry(bed).Collection(b => b.BedAssignments).LoadAsync();

            bool isFree = !bed.BedAssignments.Any(bd =>
            {
                return !((bd.To < request.From) || (request.To < bd.From));
            });

            if (isFree)
            {
                freeBed = bed;
                break;
            }
        }

        if (freeBed == null)
            return NotFound($"No free bed of type '{request.BedType}' in ward '{request.Ward}' for period " +
                            $"from {request.From} to {(request.To.HasValue ? request.To.Value.ToString() : "open-ended")}.");

        var assignment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = freeBed.Id,
            From = request.From,
            To = request.To
        };

        _context.BedAssignments.Add(assignment);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Bed assigned successfully", assignmentId = assignment.Id });    }
}