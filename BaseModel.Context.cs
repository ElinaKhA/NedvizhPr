﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NedvizhPr
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class nedvizhdbEntities : DbContext
    {
        private static nedvizhdbEntities _context;
        public nedvizhdbEntities()
            : base("name=nedvizhdbEntities")
        {
        }
        public static nedvizhdbEntities GetContext()
        {
            if (_context == null)
                _context = new nedvizhdbEntities();
            return _context;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Deal> Deals { get; set; }
        public virtual DbSet<Demand> Demands { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Realty> Realties { get; set; }
        public virtual DbSet<Supply> Supplies { get; set; }
    }
}
