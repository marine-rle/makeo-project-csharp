using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MakeoProject.DbLib.Class;

public partial class MakeoProjectContext : DbContext
{
    public MakeoProjectContext()
    {
    }

    public MakeoProjectContext(DbContextOptions<MakeoProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cache> Caches { get; set; }

    public virtual DbSet<CacheLock> CacheLocks { get; set; }

    public virtual DbSet<Competence> Competences { get; set; }

    public virtual DbSet<FailedJob> FailedJobs { get; set; }

    public virtual DbSet<Freelance> Freelances { get; set; }

    public virtual DbSet<FreelanceCompetence> FreelanceCompetences { get; set; }

    public virtual DbSet<FreelanceProject> FreelanceProjects { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobBatch> JobBatches { get; set; }

    public virtual DbSet<Migration> Migrations { get; set; }

    public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjetCompetence> ProjetCompetences { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Statut> Statuts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=192.168.56.56;port=3306;database=makeo_project;user=homestead;password=secret");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cache>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PRIMARY");

            entity.ToTable("cache");

            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Value)
                .HasColumnType("mediumtext")
                .HasColumnName("value");
        });

        modelBuilder.Entity<CacheLock>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PRIMARY");

            entity.ToTable("cache_locks");

            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Owner)
                .HasMaxLength(255)
                .HasColumnName("owner");
        });

        modelBuilder.Entity<Competence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("competences");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<FailedJob>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("failed_jobs");

            entity.HasIndex(e => e.Uuid, "failed_jobs_uuid_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Connection)
                .HasColumnType("text")
                .HasColumnName("connection");
            entity.Property(e => e.Exception).HasColumnName("exception");
            entity.Property(e => e.FailedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("failed_at");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.Queue)
                .HasColumnType("text")
                .HasColumnName("queue");
            entity.Property(e => e.Uuid).HasColumnName("uuid");
        });

        modelBuilder.Entity<Freelance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("freelances");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<FreelanceCompetence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("freelance_competences");

            entity.HasIndex(e => e.FreelanceId, "freelance_competences_freelance_id_foreign");

            entity.HasIndex(e => e.IdCompetences, "freelance_competences_id_competences_foreign");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.FreelanceId).HasColumnName("freelance_id");
            entity.Property(e => e.IdCompetences).HasColumnName("id_competences");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Freelance).WithMany(p => p.FreelanceCompetences)
                .HasForeignKey(d => d.FreelanceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("freelance_competences_freelance_id_foreign");

            entity.HasOne(d => d.IdCompetencesNavigation).WithMany(p => p.FreelanceCompetences)
                .HasForeignKey(d => d.IdCompetences)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("freelance_competences_id_competences_foreign");
        });

        modelBuilder.Entity<FreelanceProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("freelance_projects");

            entity.HasIndex(e => e.FreelanceId, "freelance_projects_freelance_id_foreign");

            entity.HasIndex(e => e.ProjectId, "freelance_projects_project_id_foreign");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.FreelanceId).HasColumnName("freelance_id");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Freelance).WithMany(p => p.FreelanceProjects)
                .HasForeignKey(d => d.FreelanceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("freelance_projects_freelance_id_foreign");

            entity.HasOne(d => d.Project).WithMany(p => p.FreelanceProjects)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("freelance_projects_project_id_foreign");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("jobs");

            entity.HasIndex(e => e.Queue, "jobs_queue_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attempts).HasColumnName("attempts");
            entity.Property(e => e.AvailableAt).HasColumnName("available_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.Queue).HasColumnName("queue");
            entity.Property(e => e.ReservedAt).HasColumnName("reserved_at");
        });

        modelBuilder.Entity<JobBatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("job_batches");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CancelledAt).HasColumnName("cancelled_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.FailedJobIds).HasColumnName("failed_job_ids");
            entity.Property(e => e.FailedJobs).HasColumnName("failed_jobs");
            entity.Property(e => e.FinishedAt).HasColumnName("finished_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Options)
                .HasColumnType("mediumtext")
                .HasColumnName("options");
            entity.Property(e => e.PendingJobs).HasColumnName("pending_jobs");
            entity.Property(e => e.TotalJobs).HasColumnName("total_jobs");
        });

        modelBuilder.Entity<Migration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("migrations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Batch).HasColumnName("batch");
            entity.Property(e => e.Migration1)
                .HasMaxLength(255)
                .HasColumnName("migration");
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PRIMARY");

            entity.ToTable("password_reset_tokens");

            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("projects");

            entity.HasIndex(e => e.StatutId, "projects_statut_id_foreign");

            entity.HasIndex(e => e.UserId, "projects_user_id_foreign");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.DateDemande)
                .HasColumnType("date")
                .HasColumnName("date_demande");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.StatutId).HasColumnName("statut_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Statut).WithMany(p => p.Projects)
                .HasForeignKey(d => d.StatutId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_statut_id_foreign");

            entity.HasOne(d => d.User).WithMany(p => p.Projects)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("projects_user_id_foreign");
        });

        modelBuilder.Entity<ProjetCompetence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("projet_competences");

            entity.HasIndex(e => e.IdCompetences, "projet_competences_id_competences_foreign");

            entity.HasIndex(e => e.ProjectId, "projet_competences_project_id_foreign");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.IdCompetences).HasColumnName("id_competences");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdCompetencesNavigation).WithMany(p => p.ProjetCompetences)
                .HasForeignKey(d => d.IdCompetences)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projet_competences_id_competences_foreign");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjetCompetences)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("projet_competences_project_id_foreign");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sessions");

            entity.HasIndex(e => e.LastActivity, "sessions_last_activity_index");

            entity.HasIndex(e => e.UserId, "sessions_user_id_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("ip_address");
            entity.Property(e => e.LastActivity).HasColumnName("last_activity");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.UserAgent)
                .HasColumnType("text")
                .HasColumnName("user_agent");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Statut>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("statuts");

            entity.HasIndex(e => e.Name, "statuts_name_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.EmailVerifiedAt)
                .HasColumnType("timestamp")
                .HasColumnName("email_verified_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RememberToken)
                .HasMaxLength(100)
                .HasColumnName("remember_token");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
