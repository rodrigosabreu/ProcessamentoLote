using Microsoft.EntityFrameworkCore;
using WorkerServicePostgres.Data;
using WorkerServicePostgres.Entities;

namespace WorkerServicePostgres.Repositories;

public class CarroRepository : IRepository<Carro>
{
    private readonly MyDbContext _context;

    public CarroRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Carro> GetByIdAsync(Guid id)
    {
        return await _context.Carros.FindAsync(id);
    }

    public async Task<IEnumerable<Carro>> GetAllAsync()
    {
        return await _context.Carros.ToListAsync();
    }

    public async Task AddAsync(Carro entity)
    {
        await _context.Carros.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Carro entity)
    {
        _context.Carros.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var carro = await _context.Carros.FindAsync(id);
        if (carro != null)
        {
            _context.Carros.Remove(carro);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddRangeAsync(IEnumerable<Carro> entities)
    {
        await _context.Carros.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<Guid> ids)
    {
        var carros = await _context.Carros.Where(c => ids.Contains(c.Id)).ToListAsync();
        if (carros.Any())
        {
            _context.Carros.RemoveRange(carros);
            await _context.SaveChangesAsync();
        }
    }
}