using MedicoPrenotazioni.Models;
using MedicoPrenotazioni.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicoPrenotazioni.Services
{
    public interface IServizioService
    {
        Task<List<Servizio>> GetServiziAsync();
        Task<Servizio?> GetServizioByIdAsync(int id);
        Task<Servizio> CreateServizioAsync(Servizio servizio);
        Task<Servizio?> UpdateServizioAsync(Servizio servizio);
        Task<bool> DeleteServizioAsync(int id);
    }

    public class ServizioService : IServizioService
    {
        private readonly ApplicationDbContext _context;

        public ServizioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Servizio>> GetServiziAsync()
        {
            return await _context.Servizi.ToListAsync();
        }

        public async Task<Servizio?> GetServizioByIdAsync(int id)
        {
            return await _context.Servizi.FindAsync(id);
        }

        public async Task<Servizio> CreateServizioAsync(Servizio servizio)
        {
            _context.Servizi.Add(servizio);
            await _context.SaveChangesAsync();
            return servizio;
        }

        public async Task<Servizio?> UpdateServizioAsync(Servizio servizio)
        {
            var existingServizio = await _context.Servizi.FindAsync(servizio.Id);
            if (existingServizio == null)
            {
                return null;
            }

            _context.Entry(existingServizio).CurrentValues.SetValues(servizio);
            await _context.SaveChangesAsync();
            return servizio;
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
