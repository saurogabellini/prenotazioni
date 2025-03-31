using MedicoPrenotazioni.Models;
using MedicoPrenotazioni.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicoPrenotazioni.Services
{
    public interface ISlotDisponibilitaService
    {
        Task<List<SlotDisponibilita>> GetAllSlotDisponibilitaAsync();
        Task<List<SlotDisponibilita>> GetSlotDisponibilitaByOperatoreAsync(int operatoreId);
        Task<List<SlotDisponibilita>> GetSlotDisponibilitaByServizioAsync(int servizioId);
        Task<List<SlotDisponibilita>> GetSlotDisponibilitaByOperatoreEServizioAsync(int operatoreId, int servizioId);
        Task<SlotDisponibilita?> GetSlotDisponibilitaByIdAsync(int id);
        Task<SlotDisponibilita> CreateSlotDisponibilitaAsync(SlotDisponibilita slotDisponibilita);
        Task<SlotDisponibilita?> UpdateSlotDisponibilitaAsync(SlotDisponibilita slotDisponibilita);
        Task<bool> DeleteSlotDisponibilitaAsync(int id);
    }

    public class SlotDisponibilitaService : ISlotDisponibilitaService
    {
        private readonly ApplicationDbContext _context;

        public SlotDisponibilitaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SlotDisponibilita>> GetAllSlotDisponibilitaAsync()
        {
            return await _context.SlotDisponibilita
                .Include(s => s.Operatore)
                .Include(s => s.Servizio)
                .ToListAsync();
        }

        public async Task<List<SlotDisponibilita>> GetSlotDisponibilitaByOperatoreAsync(int operatoreId)
        {
            return await _context.SlotDisponibilita
                .Include(s => s.Operatore)
                .Include(s => s.Servizio)
                .Where(s => s.OperatoreId == operatoreId)
                .ToListAsync();
        }

        public async Task<List<SlotDisponibilita>> GetSlotDisponibilitaByServizioAsync(int servizioId)
        {
            return await _context.SlotDisponibilita
                .Include(s => s.Operatore)
                .Include(s => s.Servizio)
                .Where(s => s.ServizioId == servizioId)
                .ToListAsync();
        }

        public async Task<List<SlotDisponibilita>> GetSlotDisponibilitaByOperatoreEServizioAsync(int operatoreId, int servizioId)
        {
            return await _context.SlotDisponibilita
                .Include(s => s.Operatore)
                .Include(s => s.Servizio)
                .Where(s => s.OperatoreId == operatoreId && s.ServizioId == servizioId)
                .ToListAsync();
        }

        public async Task<SlotDisponibilita?> GetSlotDisponibilitaByIdAsync(int id)
        {
            return await _context.SlotDisponibilita
                .Include(s => s.Operatore)
                .Include(s => s.Servizio)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<SlotDisponibilita> CreateSlotDisponibilitaAsync(SlotDisponibilita slotDisponibilita)
        {
            // Verifica che l'operatore e il servizio esistano
            var operatore = await _context.Operatori.FindAsync(slotDisponibilita.OperatoreId);
            var servizio = await _context.Servizi.FindAsync(slotDisponibilita.ServizioId);

            if (operatore == null || servizio == null)
            {
                throw new InvalidOperationException("Operatore o servizio non valido");
            }

            _context.SlotDisponibilita.Add(slotDisponibilita);
            await _context.SaveChangesAsync();
            return slotDisponibilita;
        }

        public async Task<SlotDisponibilita?> UpdateSlotDisponibilitaAsync(SlotDisponibilita slotDisponibilita)
        {
            // Verifica che l'operatore e il servizio esistano
            var operatore = await _context.Operatori.FindAsync(slotDisponibilita.OperatoreId);
            var servizio = await _context.Servizi.FindAsync(slotDisponibilita.ServizioId);

            if (operatore == null || servizio == null)
            {
                return null;
            }

            var existingSlot = await _context.SlotDisponibilita.FindAsync(slotDisponibilita.Id);
            if (existingSlot == null)
            {
                return null;
            }

            _context.Entry(existingSlot).CurrentValues.SetValues(slotDisponibilita);
            await _context.SaveChangesAsync();
            return slotDisponibilita;
        }

        public async Task<bool> DeleteSlotDisponibilitaAsync(int id)
        {
            var slot = await _context.SlotDisponibilita.FindAsync(id);
            if (slot == null)
            {
                return false;
            }

            _context.SlotDisponibilita.Remove(slot);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
