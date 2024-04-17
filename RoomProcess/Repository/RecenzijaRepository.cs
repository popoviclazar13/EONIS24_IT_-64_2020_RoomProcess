﻿using Microsoft.EntityFrameworkCore;
using RoomProcess.Data;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.Entities;

namespace RoomProcess.Repository
{
    public class RecenzijaRepository : IRecenzijaRepository
    {

        private readonly DataContext _dataContext;
        public RecenzijaRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public bool CreateRecenzija(Recenzija recenzija)
        {
            _dataContext.Add(recenzija);
            _dataContext.SaveChanges();
            return Save();
        }

        public bool DeleteRecenzija(Recenzija recenzija)
        {
            _dataContext.Remove(recenzija);
            return Save();
        }

        public Recenzija GetRecenzijaById(int recenzijaId)
        {
            return _dataContext.Recenzija.Where(k => k.RecenzijaId == recenzijaId)
                            .Include(x => x.Korisnik)
                            .Include(x => x.Rezervacija)
                                   .FirstOrDefault();
        }

        public ICollection<Recenzija> GetRecenzijas()
        {
            return _dataContext.Recenzija.OrderBy(k => k.RecenzijaId)
                .Include(x => x.Korisnik)
                .Include(x => x.Rezervacija)
                                        .ToList();
        }

        public bool RecenzijaExist(int recenzijaId)
        {
            return _dataContext.Recenzija.Any(k => k.RecenzijaId == recenzijaId);
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateRecenzija(Recenzija recenzija)
        {
            _dataContext.Update(recenzija);
            return Save();
        }
    }
}