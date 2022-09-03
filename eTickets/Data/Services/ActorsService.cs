using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace eTickets.Data.Services;

public class ActorsService : IActorsService
{
    private readonly AppDbContext _context;

    public ActorsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Actor actor)
    {
        await _context.Actors.AddAsync(actor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var result = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id);
        _context.Actors.Remove(result);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Actor>> GetAllAsync()
    {
        var result = await _context.Actors.ToListAsync();
        return result;
    }

    public async Task<Actor> GetByIdAsync(int id)
    {
        var result = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id);
        //var result = _context.Actors.FirstOrDefault(a => a.Id == id);
        return result;
    }

    public async Task<Actor> UpdateAsync(int id, Actor actor)
    {
        _context.Actors.Update(actor);
        await _context.SaveChangesAsync();
        return actor;
    }
}
