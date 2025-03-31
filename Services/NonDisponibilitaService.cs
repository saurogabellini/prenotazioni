using MedicoPrenotazioni.Models;
using MedicoPrenotazioni.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicoPrenotazioni.Services
{
    public interface INonDisponibileService
    {
        Task<List<NonDisponibile>> GetServiziAsync();
        Task<NonDisponibile?> GetServizioByIdAsync(int id);

        Task<List<NonDisponibile>> GetAllNonDisponibileAsync();
        Task<NonDisponibile> CreateServizioAsync(NonDisponibile nondisponibile);
        Task<NonDisponibile?> UpdateServizioAsync(NonDisponibile nondisponibile);
        Task<bool> DeleteServizioAsync(int id);
    }

    public class NonDisponibileService : INonDisponibileService
    {
        private readonly ApplicationDbContext _context;

        public NonDisponibileService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<NonDisponibile>> GetServiziAsync()
        {
            return await _context.NonDisponibile.ToListAsync();
        }

        public async Task<NonDisponibile?> GetServizioByIdAsync(int id)
        {
            return await _context.NonDisponibile.FindAsync(id);
        }

        public async Task<List<NonDisponibile>> GetAllNonDisponibileAsync()
        {
            return await _context.NonDisponibile
                .Include(s => s.Operatore)                
                .ToListAsync();
        }


        public async Task<NonDisponibile> CreateServizioAsync(NonDisponibile nondisponibile)
        {
            _context.NonDisponibile.Add(nondisponibile);
            await _context.SaveChangesAsync();
            return nondisponibile;
        }

        public async Task<NonDisponibile?> UpdateServizioAsync(NonDisponibile nondisponibile)
        {
            var existingServizio = await _context.NonDisponibile.FindAsync(nondisponibile.Id);
            if (existingServizio == null)
            {
                return null;
            }

            _context.Entry(existingServizio).CurrentValues.SetValues(nondisponibile);
            await _context.SaveChangesAsync();
            return nondisponibile;
        }

        public async Task<bool> DeleteServizioAsync(int id)
        {
            var servizio = await _context.Servizi.FindAsync(id);
            if (servizio == null)
            {
                return false;
            }

            _context.Servizi.Remove(servizio);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
