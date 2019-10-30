

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NHLStats.Core.Data;
using NHLStats.Core.Models;

namespace NHLStats.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly NHLStatsContext _db;

        public PlayerRepository(NHLStatsContext db)
        {
            _db = db;
        }

        public async Task<Player> Get(int id)
        {
            return await _db.Players.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Player> GetRandom()
        {
            return await _db.Players.OrderBy(o => Guid.NewGuid()).FirstOrDefaultAsync();
        }

        public async Task<List<Player>> All()
        {
            return await _db.Players.ToListAsync();

        }

        public async Task<Player> Add(Player player)
        {
            await _db.Players.AddAsync(player);
            await _db.SaveChangesAsync();
            return player;
        }

        public async Task<Player> GetByWeight(int weight)
        {
            return await _db.Players.FirstOrDefaultAsync(x => x.WeightLbs == weight);
        }

        public async Task<Player> Update(Player player)
        {
            var item = Get(player.Id);
            if (item == null)
                return null;

            var result = await Task.Run(() => _db.Players.Update(player));
            await _db.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            var player = new Player { Id = id};
            _db.Players.Remove(player);
            var result = await _db.SaveChangesAsync();
            return result > 0 ? true : false;

        }
    }
}
