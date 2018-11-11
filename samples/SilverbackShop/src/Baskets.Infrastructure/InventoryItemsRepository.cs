﻿using System.Linq;
using System.Threading.Tasks;
using Common.Domain;
using Microsoft.EntityFrameworkCore;
using SilverbackShop.Baskets.Domain.Model;
using SilverbackShop.Baskets.Domain.Repositories;
using SilverbackShop.Common.Infrastructure;

namespace SilverbackShop.Baskets.Infrastructure
{
    public class InventoryItemsRepository : Repository<InventoryItem>, IInventoryItemsRepository
    {
        public InventoryItemsRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<InventoryItem> FindInventoryItemAsync(string sku)
            => DbSet.FirstOrDefaultAsync(i => i.SKU == sku);

        public Task<int> GetStockQuantityAsync(string sku)
            => DbSet
                .Where(i => i.SKU == sku)
                .Select(i => i.StockQuantity)
                .FirstOrDefaultAsync();
    }
}