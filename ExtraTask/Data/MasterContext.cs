using System;
using System.Collections.Generic;
using ExtaTask.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtaTask.Data;

public partial class MasterContext : DbContext
{
    public MasterContext()
    {
    }

    public MasterContext(DbContextOptions<MasterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Participant> Participants { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<Speaker> Speakers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Event_pk");

            entity.ToTable("Event");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasMany(d => d.Speakers).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "SpeakerEvent",
                    r => r.HasOne<Speaker>().WithMany()
                        .HasForeignKey("SpeakerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SpeakerEvent_Speaker"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SpeakerEvent_Event"),
                    j =>
                    {
                        j.HasKey("EventId", "SpeakerId").HasName("SpeakerEvent_pk");
                        j.ToTable("SpeakerEvent");
                        j.IndexerProperty<int>("EventId").HasColumnName("Event_ID");
                        j.IndexerProperty<int>("SpeakerId").HasColumnName("Speaker_ID");
                    });
        });

        modelBuilder.Entity<Participant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Participant_pk");

            entity.ToTable("Participant");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Registration_pk");

            entity.ToTable("Registration");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventId).HasColumnName("Event_ID");
            entity.Property(e => e.ParticipantId).HasColumnName("Participant_ID");

            entity.HasOne(d => d.Event).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Registration_Event");

            entity.HasOne(d => d.Participant).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Registration_Participant");
        });

        modelBuilder.Entity<Speaker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Speaker_pk");

            entity.ToTable("Speaker");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
