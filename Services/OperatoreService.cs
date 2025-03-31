using MedicoPrenotazioni.Models;
using MedicoPrenotazioni.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicoPrenotazioni.Services
{
    public interface IOperatoreService
    {
        Task<List<Operatore>> GetOperatoriAsync();
        Task<Operatore?> GetOperatoreByIdAsync(int id);
        Task<Operatore> CreateOperatoreAsync(Operatore operatore);
        Task<Operatore?> UpdateOperatoreAsync(Operatore operatore);
        Task<bool> DeleteOperatoreAsync(int id);
    }

    public class OperatoreService : IOperatoreService
    {
        private readonly ApplicationDbContext _context;

        public OperatoreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Operatore>> GetOperatoriAsync()
        {
            return await _context.Operatori.ToListAsync();
        }

        public async Task<Operatore?> GetOperatoreByIdAsync(int id)
        {
            return await _context.Operatori.FindAsync(id);
        }

        public async Task<Operatore> CreateOperatoreAsync(Operatore operatore)
        {
            _context.Operatori.Add(operatore);
            await _context.SaveChangesAsync();
            return operatore;
        }

        public async Task<Operatore?> UpdateOperatoreAsync(Operatore operatore)
        {
            var existingOperatore = await _context.Operatori.FindAsync(operatore.Id);
            if (existingOperatore == null)
            {
                return null;
            }

            _context.Entry(existingOperatore).CurrentValues.SetValues(operatore);
            await _context.SaveChangesAsync();
            return operatore;
        }

        public async Task<bool> DeleteOperatoreAsync(int id)
        {
            var operatore = await _context.Operatori.FindAsync(id);
            if (operatore == null)
            {
                return false;
            }

            _context.Operatori.Remove(operatore);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
