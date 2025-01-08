using PaschoalottoDemo.Models;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace PaschoalottoDemo
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> opcoes) : base(opcoes)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }

    }
}
