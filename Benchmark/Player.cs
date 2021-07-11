using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using deathmatch.APIs;
using deathmatch.Models_and_Factories;
using System;
using System.Threading;

namespace deathmatch
{
    public class Player : AltV.Net.Elements.Entities.Player
    {
        public ushort Handle { get; set; }
        public Account Account { get; set; }
        public bool Logged { get; set; }
        public byte LoginTries { get; set; }
        public bool Animal { get; set; }
        public int Color { get; set; }
        public bool Frozen { get; set; }
        public int Money { get; set; }
        public bool VInvincible { get; set; }
        public bool Invincible { get; set; }
        public bool Transparent { get; set; }
        public Position? Waypoint { get; set; }

        public Gang Gang { get; set; }
        public Gang GangInvite { get; set; }
        public Timer GangInviteTimer { get; set; }

        public byte Job { get; set; }

        public bool Healed { get; set; }
        public bool HealedOthers { get;  set; }
        public bool Armoured { get; set; }
        public bool ArmouredOthers { get;  set; }
        public bool AFK { get; set; }
        public Player PMTarget { get; set; }
        public DateTime NextPoke { get; set; }

        public Timer PunishmentTimer { get; set; }

        public Player(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            Handle = PlayerHandler.GetFreeSlot(); // basically, an ID system, lol
            Account = AccountExtensions.LoadAccount(this);
            Money = 0;
            Animal = false;
            Color = Utils.Random.Next(1, 85);
            SetSyncedMetaData("color", Color);
            Frozen = false;
            Invincible = false;
            Transparent = false;
            VInvincible = false;
            Waypoint = null;
            
            Gang = null;
            GangInvite = null;
            GangInviteTimer = null;

            Job = 0;

            Healed = false;
            HealedOthers = false;
            Armoured = false;
            ArmouredOthers = false;
            AFK = false;
            PMTarget = null;
            NextPoke = DateTime.Now;

            if (Account != null && Account.Punishment.Type != PunishmentType.NONE)
            {
                PunishmentTimer = new Timer((Account.Punishment.Type == PunishmentType.JAIL? Administration.Commands.Jail.PunishmentTimer_Elapsed : Administration.Commands.Mute.PunishmentTimer_Elapsed), this, 1000, 1000);
            } 
            else PunishmentTimer = null; 

            Emit("setLocalPlayerInvincible", Invincible);
            if(Vehicle != null) Emit("setLocalPlayerVehicleInvincible", VInvincible, Vehicle);

            this.SetPlayerMoney(800);

            PlayerHandler.AddPlayer(Handle, this);
        }
    }

    public class PlayerFactory : IEntityFactory<IPlayer>
    {
        public IPlayer Create(IntPtr entityPointer, ushort id)
        {
            return new Player(entityPointer, id);
        }
    }
}
