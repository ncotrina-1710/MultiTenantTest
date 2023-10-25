﻿using Microsoft.EntityFrameworkCore;
using MultiTenantTest.Dominio.Entidades;

namespace MultiTenantTest.Infraestructura.Productos
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products => Set<Product>();
    }
}
