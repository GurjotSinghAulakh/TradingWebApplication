using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TastyTrading.Models;
using System.Collections;

namespace TastyTrading.DAL
{
    public sealed class TradingDb : DbContext
    {
        [ExcludeFromCodeCoverage]
        public TradingDb(DbContextOptions<TradingDb> options) : base(options)
        {
            // Creating the database
            Database.EnsureCreated();
        }

        // Creating tables in the database
        public DbSet<User> Users { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // krever at vi importerer pakken Microsoft.EntityFrameworkCore.Proxies
            // og legge til "virtual" på de attributtene som ønskes å lastes automatisk (lazyLoading)
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}