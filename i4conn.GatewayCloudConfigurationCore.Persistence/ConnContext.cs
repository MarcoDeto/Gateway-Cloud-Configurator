using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;

#nullable disable

namespace i4conn.GatewayCloudConfigurationCore.Persistence
{
    public partial class ConnContext : DbContext
    {
        public ConnContext()
        {
        }

        public ConnContext(DbContextOptions<ConnContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Confute> Confutes { get; set; }
        public virtual DbSet<Ts400EntitaTipologium> Ts400EntitaTipologia { get; set; }
        public virtual DbSet<Ts400Entitum> Ts400Entita { get; set; }
        public virtual DbSet<Ts400Gateway> Ts400Gateways { get; set; }
        public virtual DbSet<Ts400Interfacce> Ts400Interfacces { get; set; }
        public virtual DbSet<Ts400InterfacceCanaliAnagr> Ts400InterfacceCanaliAnagrs { get; set; }
        public virtual DbSet<Ts400InterfacceCanaliAssociati> Ts400InterfacceCanaliAssociatis { get; set; }
        public virtual DbSet<Ts400InterfacceCanaliValori> Ts400InterfacceCanaliValoris { get; set; }
        public virtual DbSet<Ts400InterfacceCanaliVariabili> Ts400InterfacceCanaliVariabilis { get; set; }
        public virtual DbSet<Ts400InterfacceGruppi> Ts400InterfacceGruppis { get; set; }
        public virtual DbSet<Ts400ParamEntAnagr> Ts400ParamEntAnagrs { get; set; }
        public virtual DbSet<Ts400ParamEntValori> Ts400ParamEntValoris { get; set; }
        public virtual DbSet<Ts400TipiCanale> Ts400TipiCanales { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=192.168.1.52\\SQL2017;Initial Catalog=TS_TES_I4CONN;Persist Security Info=True;User ID=sa;Password=Tesar2017");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Confute>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("CONFUTE");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.Adw)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ADW")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.FlgProfilo).HasColumnName("FLG_PROFILO");

                entity.Property(e => e.Livello)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("LIVELLO")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.Oggetto)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OGGETTO")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.Utente)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("UTENTE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Ts400EntitaTipologium>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("TS400_ENTITA_TIPOLOGIA");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.AllowEditChannel).HasColumnName("ALLOW_EDIT_CHANNEL");

                entity.Property(e => e.AllowEditVariable).HasColumnName("ALLOW_EDIT_VARIABLE");

                entity.Property(e => e.DesTipo)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DES_TIPO")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Entita)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ENTITA")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdTipo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_TIPO")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.Par01)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("PAR01")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Par02)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("PAR02")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Par03)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("PAR03")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Ts400Entitum>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("TS400_ENTITA");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.DesEntita)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DES_ENTITA")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Entita)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ENTITA")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Ts400Gateway>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("TS400_GATEWAY");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.DesGateway)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DES_GATEWAY")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DestIp)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("DEST_IP")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.DestIpSec)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("DEST_IP_SEC")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.DestPorta)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEST_PORTA")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.DestPortaSec)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEST_PORTA_SEC")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.DestPwd)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEST_PWD")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.DestPwdSec)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEST_PWD_SEC")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.DestUser)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEST_USER")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.DestUserSec)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEST_USER_SEC")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.FirmwareRoot)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FIRMWARE_ROOT")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdGateway)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ID_GATEWAY")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.Loc)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("LOC")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.NomeGateway)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NOME_GATEWAY")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.Serialnr)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("SERIALNR")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.VerDevice)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("VER_DEVICE")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.VerRouter)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("VER_ROUTER")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.VerRules)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("VER_RULES")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.VerSupervisor)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("VER_SUPERVISOR")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.VerWebapp)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("VER_WEBAPP")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Ts400Interfacce>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("TS400_INTERFACCE");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.Coordinatore)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("COORDINATORE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.DesGruppoInterfacce)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DES_GRUPPO_INTERFACCE")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DesInterfaccia)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DES_INTERFACCIA")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdDispositivo).HasColumnName("ID_DISPOSITIVO");

                entity.Property(e => e.IdGruppoInterfacce)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_GRUPPO_INTERFACCE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.IdInterfaccia)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ID_INTERFACCIA")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.InterfacciaContapezzi)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("INTERFACCIA_CONTAPEZZI")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.IpTerminale)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("IP_TERMINALE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.NumCanaliI).HasColumnName("NUM_CANALI_I");

                entity.Property(e => e.NumCanaliU).HasColumnName("NUM_CANALI_U");

                entity.Property(e => e.PortaTerminale)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PORTA_TERMINALE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.Router)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("ROUTER")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.UltimaInterrogazione)
                    .HasColumnType("datetime")
                    .HasColumnName("ULTIMA_INTERROGAZIONE");
            });

            modelBuilder.Entity<Ts400InterfacceCanaliAnagr>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("TS400_INTERFACCE_CANALI_ANAGR");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.Descrizione)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIZIONE")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Direzione)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("DIREZIONE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.IdCanale)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ID_CANALE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.TipoCanale)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_CANALE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.TipologiaInterfaccia)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPOLOGIA_INTERFACCIA")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Ts400InterfacceCanaliAssociati>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("TS400_INTERFACCE_CANALI_ASSOCIATI");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.Direzione)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("DIREZIONE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.FlgAbilitaOriginale).HasColumnName("FLG_ABILITA_ORIGINALE");

                entity.Property(e => e.IdCanale)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ID_CANALE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.IdCanaleVirtuale)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ID_CANALE_VIRTUALE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.IdInterfaccia)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ID_INTERFACCIA")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Ts400InterfacceCanaliValori>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("TS400_INTERFACCE_CANALI_VALORI");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.Descrizione)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIZIONE")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Destination)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DESTINATION")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.Direzione)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("DIREZIONE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.FlgVirtual).HasColumnName("FLG_VIRTUAL");

                entity.Property(e => e.IdCanale)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ID_CANALE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.IdCanaleOrigine)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ID_CANALE_ORIGINE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.IdInterfaccia)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ID_INTERFACCIA")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.IdRegola)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_REGOLA")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.TipoCanale)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_CANALE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Ts400InterfacceCanaliVariabili>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("TS400_INTERFACCE_CANALI_VARIABILI");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.Chiave)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("CHIAVE")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Direzione)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("DIREZIONE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.Gruppo)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("GRUPPO")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.IdCanale)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ID_CANALE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.IdInterfaccia)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ID_INTERFACCIA")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NOME")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Ts400InterfacceGruppi>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("TS400_INTERFACCE_GRUPPI");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.DesGruppoInterfacce)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DES_GRUPPO_INTERFACCE")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdGateway)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ID_GATEWAY")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.IdGruppoInterfacce)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_GRUPPO_INTERFACCE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Ts400ParamEntAnagr>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("TS400_PARAM_ENT_ANAGR");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.Entita)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ENTITA")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ParamNome)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PARAM_NOME")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ParamValoreDefault)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PARAM_VALORE_DEFAULT")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tipologia)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPOLOGIA")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Ts400ParamEntValori>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("TS400_PARAM_ENT_VALORI");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.Entita)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ENTITA")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdEntita)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID_ENTITA")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ParamNome)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PARAM_NOME")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ParamValore)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PARAM_VALORE")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Ts400TipiCanale>(entity =>
            {
                entity.HasKey(e => e.Recno);

                entity.ToTable("TS400_TIPI_CANALE");

                entity.Property(e => e.Recno).HasColumnName("__RECNO__");

                entity.Property(e => e.DesCanale)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("DES_CANALE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.FlgStandard).HasColumnName("FLG_STANDARD");

                entity.Property(e => e.TipoCanale)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_CANALE")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.Topic)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TOPIC")
                    .HasDefaultValueSql("('')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
