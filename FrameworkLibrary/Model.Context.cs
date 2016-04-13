﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FrameworkLibrary
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<EmailLog> EmailLogs { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Media> AllMedia { get; set; }
        public virtual DbSet<MediaDetail> MediaDetails { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<UserMediaDetail> UsersMediaDetails { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<MediaType> MediaTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MediaTypeRolePermission> MediaTypeRolesPermissions { get; set; }
        public virtual DbSet<MediaTypeRole> MediaTypeRoles { get; set; }
        public virtual DbSet<MasterPage> MasterPages { get; set; }
        public virtual DbSet<Settings> AllSettings { get; set; }
        public virtual DbSet<WebserviceRequest> WebserviceRequests { get; set; }
        public virtual DbSet<MediaTag> MediaTags { get; set; }
        public virtual DbSet<GlossaryTerm> GlossaryTerms { get; set; }
        public virtual DbSet<Field> Fields { get; set; }
        public virtual DbSet<FieldAssociation> FieldAssociations { get; set; }
        public virtual DbSet<FieldFile> FieldFiles { get; set; }
        public virtual DbSet<IPLocationTrackerEntry> IPLocationTrackerEntries { get; set; }
        public virtual DbSet<RoleMediaDetail> RolesMediaDetails { get; set; }
    }
}
