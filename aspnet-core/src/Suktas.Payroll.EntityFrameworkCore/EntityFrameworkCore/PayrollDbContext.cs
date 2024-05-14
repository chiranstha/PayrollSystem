using Suktas.Payroll.Master;
using Suktas.Payroll.Payroll;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Suktas.Payroll.Authorization.Delegation;
using Suktas.Payroll.Authorization.Roles;
using Suktas.Payroll.Authorization.Users;
using Suktas.Payroll.Chat;
using Suktas.Payroll.Editions;
using Suktas.Payroll.Friendships;
using Suktas.Payroll.MultiTenancy;
using Suktas.Payroll.MultiTenancy.Accounting;
using Suktas.Payroll.MultiTenancy.Payments;
using Suktas.Payroll.Storage;
using System.Linq;

namespace Suktas.Payroll.EntityFrameworkCore
{
    public class PayrollDbContext : AbpZeroDbContext<Tenant, Role, User, PayrollDbContext>
    {
        public virtual DbSet<GradeUpgrade> GradeUpgrades { get; set; }

        public virtual DbSet<FestivalBonusSetting> FestivalBonusSettings { get; set; }

        public virtual DbSet<PrincipalAllowanceSetting> Tbl_PrincipalAllowanceSettings { get; set; }

        public virtual DbSet<EmployeeSalary> EmployeeSalary { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<FinancialYear> FinancialYears { get; set; }

        public virtual DbSet<SchoolInfo> SchoolInfos { get; set; }

        public virtual DbSet<InternalGradeSetup> InternalGradeSetup { get; set; }

        public virtual DbSet<CategorySalary> CategorySalary { get; set; }

        public virtual DbSet<EmployeeLevel> EmployeeLevels { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public PayrollDbContext(DbContextOptions<PayrollDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                foreignKey.DeleteBehavior = DeleteBehavior.Cascade;

            modelBuilder.Entity<GradeUpgrade>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<FestivalBonusSetting>(f =>
                       {
                           f.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PrincipalAllowanceSetting>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<EmployeeSalary>(x =>
                       {
                           x.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<FinancialYear>(f =>
                       {
                           f.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SchoolInfo>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<InternalGradeSetup>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CategorySalary>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<EmployeeLevel>(x =>
                       {
                           x.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BinaryObject>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });
        }
    }
}