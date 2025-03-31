using MedicoPrenotazioni.Models;
using MedicoPrenotazioni.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicoPrenotazioni.Services
{
    public interface IPrenotazioneService
    {
        Task<List<Prenotazione>> GetAllPrenotazioniAsync();
        Task<List<Prenotazione>> GetPrenotazioniByOperatoreAsync(int operatoreId);
        Task<List<Prenotazione>> GetPrenotazioniByServizioAsync(int servizioId);
        Task<List<Prenotazione>> GetPrenotazioniByDataAsync(DateTime data);
        Task<List<Prenotazione>> GetPrenotazioniByClienteAsync(string cognomeCliente);
        Task<Prenotazione?> GetPrenotazioneByIdAsync(int id);
        Task<Prenotazione?> CreatePrenotazioneAsync(Prenotazione prenotazione);
        Task<Prenotazione?> UpdatePrenotazioneAsync(Prenotazione prenotazione);
        Task<bool> DeletePrenotazioneAsync(int id);
        Task<List<DateTime>> GetDateDisponibiliAsync(int servizioId, int? operatoreId = null);
        Task<List<TimeSpan>> GetOrariDisponibiliAsync(int servizioId, int operatoreId, DateTime data);
        Task<bool> VerificaDisponibilitaAsync(int servizioId, int operatoreId, DateTime data, TimeSpan ora);
    }

    public class PrenotazioneService : IPrenotazioneService
    {
        private readonly ApplicationDbContext _context;

        public PrenotazioneService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Prenotazione>> GetAllPrenotazioniAsync()
        {
            return await _context.Prenotazioni
                .Include(p => p.Operatore)
                .Include(p => p.Servizio)
                .ToListAsync();
        }

        public async Task<List<Prenotazione>> GetPrenotazioniByOperatoreAsync(int operatoreId)
        {
            return await _context.Prenotazioni
                .Include(p => p.Operatore)
                .Include(p => p.Servizio)
                .Where(p => p.OperatoreId == operatoreId)
                .ToListAsync();
        }

        public async Task<List<Prenotazione>> GetPrenotazioniByServizioAsync(int servizioId)
        {
            return await _context.Prenotazioni
                .Include(p => p.Operatore)
                .Include(p => p.Servizio)
                .Where(p => p.ServizioId == servizioId)
                .ToListAsync();
        }

        public async Task<List<Prenotazione>> GetPrenotazioniByDataAsync(DateTime data)
        {
            return await _context.Prenotazioni
                .Include(p => p.Operatore)
                .Include(p => p.Servizio)
                .Where(p => p.Data.Date == data.Date)
                .ToListAsync();
        }

        public async Task<List<Prenotazione>> GetPrenotazioniByClienteAsync(string cognomeCliente)
        {
            return await _context.Prenotazioni
                .Include(p => p.Operatore)
                .Include(p => p.Servizio)
                .Where(p => p.CognomeCliente.Contains(cognomeCliente, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<Prenotazione?> GetPrenotazioneByIdAsync(int id)
        {
            return await _context.Prenotazioni
                .Include(p => p.Operatore)
                .Include(p => p.Servizio)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Prenotazione?> CreatePrenotazioneAsync(Prenotazione prenotazione)
        {
            // Verifica che l'operatore e il servizio esistano usando Include per caricare le relazioni
            var operatore = await _context.Operatori
                .Include(o => o.SlotDisponibilita)
                .FirstOrDefaultAsync(o => o.Id == prenotazione.OperatoreId);
            
            var servizio = await _context.Servizi
                .Include(s => s.SlotDisponibilita)
                .FirstOrDefaultAsync(s => s.Id == prenotazione.ServizioId);

            if (operatore == null || servizio == null)
            {
                return null;
            }

            // Verifica disponibilità
            bool disponibile = await VerificaDisponibilitaAsync(
                prenotazione.ServizioId,
                prenotazione.OperatoreId,
                prenotazione.Data,
                prenotazione.OraInizio);

            if (!disponibile)
            {
                return null;
            }

            try
            {
                prenotazione.DataCreazione = DateTime.Now;
                prenotazione.Operatore = operatore;    // Assegna esplicitamente le relazioni
                prenotazione.Servizio = servizio;      // Assegna esplicitamente le relazioni
                
                _context.Prenotazioni.Add(prenotazione);
                await _context.SaveChangesAsync();
                return prenotazione;
            }
            catch (Exception)
            {
                // Opzionale: gestione degli errori
                _context.Entry(prenotazione).State = EntityState.Detached;
                throw;
            }
        }

        public async Task<Prenotazione?> UpdatePrenotazioneAsync(Prenotazione prenotazione)
        {
            // Verifica che l'operatore e il servizio esistano
            var operatore = await _context.Operatori.FindAsync(prenotazione.OperatoreId);
            var servizio = await _context.Servizi.FindAsync(prenotazione.ServizioId);

            if (operatore == null || servizio == null)
            {
                return null;
            }

            var existingPrenotazione = await _context.Prenotazioni.FindAsync(prenotazione.Id);
            if (existingPrenotazione == null)
            {
                return null;
            }

            prenotazione.DataModifica = DateTime.Now;
            _context.Entry(existingPrenotazione).CurrentValues.SetValues(prenotazione);
            await _context.SaveChangesAsync();
            return prenotazione;
        }

        public async Task<bool> DeletePrenotazioneAsync(int id)
        {
            var prenotazione = await _context.Prenotazioni.FindAsync(id);
            if (prenotazione == null)
            {
                return false;
            }

            _context.Prenotazioni.Remove(prenotazione);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DateTime>> GetDateDisponibiliAsync(int servizioId, int? operatoreId = null)
        {
            // In una implementazione reale, questo metodo dovrebbe verificare gli slot di disponibilità
            // e le prenotazioni esistenti per determinare le date disponibili
            var dateDisponibili = new List<DateTime>();
            var oggi = DateTime.Today;

            // Ottieni gli slot di disponibilità per il servizio e l'operatore (se specificato)
            var query = _context.SlotDisponibilita
                .Where(s => s.ServizioId == servizioId && s.Attivo);

            if (operatoreId.HasValue)
            {
                query = query.Where(s => s.OperatoreId == operatoreId.Value);
            }

            var slots = await query.ToListAsync();

            // Se non ci sono slot disponibili, restituisci una lista vuota
            if (!slots.Any())
            {
                return dateDisponibili;
            }

            // Altrimenti, restituisci i prossimi 14 giorni in cui ci sono slot disponibili
            for (int i = 1; i <= 14; i++)
            {
                var data = oggi.AddDays(i);
                var giorno = (GiornoSettimana)((int)data.DayOfWeek == 0 ? 7 : (int)data.DayOfWeek);

                // Verifica se c'è almeno uno slot disponibile per questo giorno
                if (slots.Any(s => s.Giorno == giorno))
                {
                    dateDisponibili.Add(data);
                }
            }

            return dateDisponibili;
        }

        public async Task<List<TimeSpan>> GetOrariDisponibiliAsync(int servizioId, int operatoreId, DateTime data)
        {
            var orariDisponibili = new List<TimeSpan>();
            var giorno = (GiornoSettimana)((int)data.DayOfWeek == 0 ? 7 : (int)data.DayOfWeek);

            // Ottieni gli slot di disponibilità per l'operatore e il servizio nel giorno specificato
            var slots = await _context.SlotDisponibilita
                .Where(s => s.OperatoreId == operatoreId && 
                       s.ServizioId == servizioId && 
                       s.Giorno == giorno && 
                       s.Attivo)
                .ToListAsync();

            var slotGiorno = slots.FirstOrDefault();

            if (slotGiorno != null)
            {
                // Calcola gli orari disponibili in base allo slot
                var oraCorrente = slotGiorno.OraInizio;
                while (oraCorrente.Add(TimeSpan.FromMinutes(slotGiorno.DurataMinuti)) <= slotGiorno.OraFine)
                {
                    // Verifica se l'orario è già prenotato
                    var prenotazioniEsistenti = await _context.Prenotazioni
                        .AnyAsync(p => 
                            p.Data.Date == data.Date && 
                            p.OperatoreId == operatoreId && 
                            p.OraInizio == oraCorrente &&
                            p.Stato != StatoPrenotazione.Annullata);

                    if (!prenotazioniEsistenti)
                    {
                        orariDisponibili.Add(oraCorrente);
                    }

                    oraCorrente = oraCorrente.Add(TimeSpan.FromMinutes(slotGiorno.DurataMinuti));
                }
            }

            return orariDisponibili;
        }

        public async Task<bool> VerificaDisponibilitaAsync(int servizioId, int operatoreId, DateTime data, TimeSpan ora)
        {
            // Verifica se l'orario richiesto è disponibile
            var orariDisponibili = await GetOrariDisponibiliAsync(servizioId, operatoreId, data);
            return orariDisponibili.Contains(ora);
        }
    }
}
