using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TastyTrading.Models;

namespace TastyTrading.DAL
{
    public sealed class TradingDB : DbContext
    {
        [ExcludeFromCodeCoverage]
        public TradingDB(DbContextOptions<TradingDB> options) : base(options)
        {
            // Creating the database
            Database.EnsureCreated();
        }

        // Creating tables in the database
        public DbSet<Person> Persons { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        //public DbSet<Buy> Buys { get; set; }
        //public DbSet<Sell> Sells { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // krever at vi importerer pakken Microsoft.EntityFrameworkCore.Proxies
            // og legge til "virtual" på de attributtene som ønskes å lastes automatisk (lazyLoading)
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}