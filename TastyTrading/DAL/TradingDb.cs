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
            // Requires us to import the Microsoft.EntityFrameworkCore.Proxies package
            // And add "virtual" to the attributes that are wanted to be loaded automatically (lazyLoading)
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}