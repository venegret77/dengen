﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MashZavod.Models.DbModels
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class database_murom_factory2Entities1 : DbContext
    {
        public database_murom_factory2Entities1()
            : base("name=database_murom_factory2Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<access> access { get; set; }
        public virtual DbSet<chat> chat { get; set; }
        public virtual DbSet<doc> doc { get; set; }
        public virtual DbSet<edit> edit { get; set; }
        public virtual DbSet<group_rank> group_rank { get; set; }
        public virtual DbSet<infoFileDoc> infoFileDoc { get; set; }
        public virtual DbSet<message> message { get; set; }
        public virtual DbSet<@object> @object { get; set; }
        public virtual DbSet<rank> rank { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<tom> tom { get; set; }
        public virtual DbSet<User_chat> User_chat { get; set; }
        public DbSet<users> users { get; set; }
    }
}
