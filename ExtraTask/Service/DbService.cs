using ExtaTask.Data;
using ExtaTask.Exceptions;
using ExtaTask.Models;
using ExtaTask.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ExtaTask.Service;

public interface IDbService
{
    public Task<GetCreatedEventDto?> GetEventByIdAsync(int id);
    public Task<GetCreatedEventDto> CreateEventAsync(CreateEventDto dto);
    public Task AssignSpeakerToEventAsync(int speakerId,int eventId);
    public Task<CreateRegistrationDto?> GetRegistrationByIdAsync(int id);
    public Task<CreateRegistrationDto> CreateRegistrationAsync(int parId, int evId);
    public Task DeleteRegistrationAsync(int parId, int evId);
    public Task<ICollection<CurrEventGetDto>> GetCurrEventsAsync();
    public Task<ParticipantGetDto> GetParticipantAsync(int id);
    
    //Dodałem dodatkową funkcjonalność, ponieważ uznałem, że samo przekazywanie ID w metodach POST może być niewystarczające.
    public Task<int> CreateSpeakerAsync(SpeakerCreateDto dto);
    public Task<SpeakerGetDto?> GetSpeakerAsync(int id);
    public Task<int> CreateParticipantAsync(ParticipantCreateDto dto);
    public Task<IEnumerable<SpeakerGetDto>> GetAllSpeakersAsync();
    public Task<IEnumerable<ParticipantGetAllDto>> GetAllParticipantsAsync();


}
public class DbService(MasterContext data) : IDbService
{
    public async Task<GetCreatedEventDto?> GetEventByIdAsync(int id)
    {
       return await data.Events
           .Where(e => e.Id == id)
           .Select(e => new GetCreatedEventDto
           {
               Id = e.Id,
               Title = e.Title,
               Description = e.Description,
               Date = e.Date,
               MaxParticipants = e.MaxParticipants
           })
           .FirstOrDefaultAsync();
    }

    public async Task<GetCreatedEventDto> CreateEventAsync(CreateEventDto dto)
    {
        if (dto.Date <= DateTime.Now)
        {
            throw new InvalidOperationException("Event date cannot be in the past.");
        }

        var ev = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            Date = dto.Date,
            MaxParticipants = dto.MaxParticipants
        };

        await data.Events.AddAsync(ev);
        await data.SaveChangesAsync();

        return new GetCreatedEventDto
        {
            Id = ev.Id,
            Title = ev.Title,
            Description = ev.Description,
            Date = ev.Date,
            MaxParticipants = ev.MaxParticipants
        };
    }

    public async Task AssignSpeakerToEventAsync(int speakerId, int eventId)
    {
        if (!await data.Speakers.AnyAsync(s => s.Id == speakerId))
            throw new NotFoundException($"Speaker with id {speakerId} not found");

        var targetEvent = await data.Events.FindAsync(eventId);
        if (targetEvent is null)
            throw new NotFoundException($"Event with id {eventId} not found");
        
        var conflict = await data.Events
            .Where(e => e.Date == targetEvent.Date && e.Speakers.Any(s => s.Id == speakerId))
            .AnyAsync();

        if (conflict)
            throw new InvalidOperationException("Speaker is already assigned to another event at the same time.");
        
        var speaker = await data.Speakers.FindAsync(speakerId);
        speaker!.Events.Add(targetEvent);
        await data.SaveChangesAsync();

    }

    public async Task<CreateRegistrationDto?> GetRegistrationByIdAsync(int id)
    {
        return await data.Registrations.Where(r => r.Id == id).Select(r => new CreateRegistrationDto
        {
            Id = r.Id,
            EventId = r.EventId,
            ParticipantId = r.ParticipantId,
            RegisteredAt = r.RegisteredAt
        }).FirstOrDefaultAsync();
    }

    public async Task<CreateRegistrationDto> CreateRegistrationAsync(int parId, int evId)
    {
        if (!await data.Participants.AnyAsync(x => x.Id == parId))
        {
            throw new NotFoundException($"Participant with id: {parId} not found");
        }
        
        var eventEntity = await data.Events
            .Where(e => e.Id == evId)
            .Select(e => new { e.Id, e.MaxParticipants })
            .FirstOrDefaultAsync();

        if (eventEntity == null)
        {
            throw new NotFoundException($"Event with id: {evId} not found");
        }
        
        if (await data.Registrations.AnyAsync(x => x.ParticipantId == parId && x.EventId == evId))
        {
            throw new AlreadyExistsException($"Participant with id: {parId} already registered on event with id: {evId}");
        }
        
        var currentCount = await data.Registrations.CountAsync(r => r.EventId == evId);

        if (currentCount >= eventEntity.MaxParticipants)
        {
            throw new FullEventException($"Event with id: {evId} does not have any free places");
        }
        
        var reg = new Registration
        {
            ParticipantId = parId,
            EventId = evId,
            RegisteredAt = DateTime.Now
        };
        await data.Registrations.AddAsync(reg);
        await data.SaveChangesAsync();
        
        return new CreateRegistrationDto
        {
            Id = reg.Id,
            EventId = evId,
            ParticipantId = parId,
            RegisteredAt = reg.RegisteredAt
        };
    }

    public async Task DeleteRegistrationAsync(int parId, int evId)
    {
        var eventDate = await data.Events
            .Where(e => e.Id == evId)
            .Select(e => e.Date)
            .FirstOrDefaultAsync();

        if (eventDate == default)
        {
            throw new NotFoundException("Event not found");
        }

        if (!await data.Participants.AnyAsync(x => x.Id == parId))
        {
            throw new NotFoundException("Participant with id:" + parId + " not found");
        }

        var reg = await data.Registrations
            .FirstOrDefaultAsync(x => x.ParticipantId == parId && x.EventId == evId);
        if (reg is null)
        {
            throw new NotFoundException("Registration with ParticipantId:" + parId + " and EventId:" + evId +
                                        " not found");
        }

        if (eventDate <= DateTime.Now.AddHours(24))
        {
            throw new InvalidOperationException(
                "You cannot cancel your registration less than 24 hours before the event starts.");
        }

        data.Registrations.Remove(reg);
        await data.SaveChangesAsync();
    }

    public async Task<ICollection<CurrEventGetDto>> GetCurrEventsAsync()
    {
        return await data.Events
            .Where(e => e.Date > DateTime.Now)
            .Select(e => new CurrEventGetDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                MaxParticipants = e.MaxParticipants,
                FullNameSpeaker = e.Speakers
                    .Select(s => s.FirstName + " " + s.LastName)
                    .FirstOrDefault() ?? string.Empty,
                CurrCountOfParticipants = e.Registrations.Count(),
                CountOfFreePlaces = e.MaxParticipants - e.Registrations.Count()
            })
            .ToListAsync();
    }

    public async Task<ParticipantGetDto> GetParticipantAsync(int id)
    {
        var par = await data.Participants
            .Where(p => p.Id == id)
            .Select(p => new ParticipantGetDto
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                Events = p.Registrations
                    .Select(r => r.Event)
                    .Select(e => new EventGetDto
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Date = e.Date,
                        SpeakerId = e.Speakers.Select(s => s.Id).FirstOrDefault(),
                        LastNameSpeaker = e.Speakers.Select(s => s.LastName).FirstOrDefault()!
                    }).ToList()
            })
            .FirstOrDefaultAsync();

        if (par is null)
        {
            throw new NotFoundException("Participant with id: " + id + " not found");
        }

        return par;
    }
    
    //Dodatkowa funkcjonalność
    public async Task<int> CreateSpeakerAsync(SpeakerCreateDto dto)
    {
        var speaker = new Speaker
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        await data.Speakers.AddAsync(speaker);
        await data.SaveChangesAsync();

        return speaker.Id;
    }

    public async Task<SpeakerGetDto?> GetSpeakerAsync(int id)
    {
        return await data.Speakers
            .Where(s => s.Id == id)
            .Select(s => new SpeakerGetDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateParticipantAsync(ParticipantCreateDto dto)
    {
        var participant = new Participant
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        };

        await data.Participants.AddAsync(participant);
        await data.SaveChangesAsync();

        return participant.Id;
    }
    
    public async Task<IEnumerable<SpeakerGetDto>> GetAllSpeakersAsync()
    {
        return await data.Speakers
            .Select(s => new SpeakerGetDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName
            })
            .ToListAsync();
    }
    public async Task<IEnumerable<ParticipantGetAllDto>> GetAllParticipantsAsync()
    {
        return await data.Participants
            .Select(p => new ParticipantGetAllDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email
            })
            .ToListAsync();
    }


}