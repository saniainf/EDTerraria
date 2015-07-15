/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria
{
    public class Projectile : Entity
    {
        public static int maxAI = 2;
        public float scale = 1f;
        public int owner = 255;
        public float[] ai = new float[maxAI];
        public float[] localAI = new float[maxAI];
        public float stepSpeed = 1f;
        public int spriteDirection = 1;
        public int penetrate = 1;
        private int[] npcImmune = new int[200];
        public int maxPenetrate = 1;
        public Vector2[] oldPos = new Vector2[10];
        public float[] oldRot = new float[10];
        public int[] oldSpriteDirection = new int[10];
        public int[] playerImmune = new int[255];
        public string miscText = "";
        public bool arrow;
        public int numHits;
        public bool bobber;
        public bool netImportant;
        public bool noDropItem;
        public bool counterweight;
        public float rotation;
        public int type;
        public int alpha;
        public short glowMask;
        public float gfxOffY;
        public int aiStyle;
        public int timeLeft;
        public int soundDelay;
        public int damage;
        public bool hostile;
        public float knockBack;
        public bool friendly;
        private bool updatedNPCImmunity;
        public int identity;
        public float light;
        public bool netUpdate;
        public bool netUpdate2;
        public int netSpam;
        public bool minion;
        public float minionSlots;
        public int minionPos;
        public int restrikeDelay;
        public bool tileCollide;
        public int extraUpdates;
        public int numUpdates;
        public bool ignoreWater;
        public bool hide;
        public bool ownerHitCheck;
        public bool melee;
        public bool ranged;
        public bool thrown;
        public bool magic;
        public bool coldDamage;
        public bool noEnchantments;
        public bool trap;
        public bool npcProj;
        public int frameCounter;
        public int frame;
        public bool manualDirectionChange;

        public float Opacity
        {
            get {return (float)(1.0 - alpha / 255);}
            set {alpha = (int)MathHelper.Clamp((float)((1.0 - value) * 255), 0.0f, 255);}
        }

        public int MaxUpdates
        {
            get {return extraUpdates + 1;}
            set {extraUpdates = value - 1;}
        }

        public void SetDefaults(int Type)
        {
            counterweight = false;
            arrow = false;
            bobber = false;
            numHits = 0;
            netImportant = false;
            manualDirectionChange = false;
            int newSize = 10;
            if (Type >= 0)
                newSize = ProjectileID.Sets.TrailCacheLength[Type];
            if (newSize != oldPos.Length)
            {
                Array.Resize(ref oldPos, newSize);
                Array.Resize(ref oldRot, newSize);
                Array.Resize(ref oldSpriteDirection, newSize);
            }
            for (int index = 0; index < oldPos.Length; ++index)
            {
                oldPos[index].X = 0.0f;
                oldPos[index].Y = 0.0f;
                oldRot[index] = 0.0f;
                oldSpriteDirection[index] = 0;
            }
            for (int index = 0; index < maxAI; ++index)
            {
                ai[index] = 0.0f;
                localAI[index] = 0.0f;
            }
            for (int index = 0; index < 255; ++index)
                playerImmune[index] = 0;
            for (int index = 0; index < 200; ++index)
                npcImmune[index] = 0;
            noDropItem = false;
            minion = false;
            minionSlots = 0.0f;
            soundDelay = 0;
            spriteDirection = 1;
            melee = false;
            ranged = false;
            thrown = false;
            magic = false;
            ownerHitCheck = false;
            hide = false;
            lavaWet = false;
            wetCount = 0;
            wet = false;
            ignoreWater = false;
            hostile = false;
            netUpdate = false;
            netUpdate2 = false;
            netSpam = 0;
            numUpdates = 0;
            extraUpdates = 0;
            identity = 0;
            restrikeDelay = 0;
            light = 0.0f;
            penetrate = 1;
            tileCollide = true;
            position = Vector2.Zero;
            velocity = Vector2.Zero;
            aiStyle = 0;
            alpha = 0;
            glowMask = -1;
            type = Type;
            active = true;
            rotation = 0.0f;
            scale = 1f;
            owner = 255;
            timeLeft = 3600;
            name = "";
            friendly = false;
            damage = 0;
            knockBack = 0.0f;
            miscText = "";
            coldDamage = false;
            noEnchantments = false;
            trap = false;
            npcProj = false;
            frame = 0;
            frameCounter = 0;
            if (type == 1)
            {
                arrow = true;
                name = "Wooden Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
            }
            else if (type == 2)
            {
                arrow = true;
                name = "Fire Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                light = 1f;
                ranged = true;
            }
            else if (type == 3)
            {
                name = "Shuriken";
                width = 22;
                height = 22;
                aiStyle = 2;
                friendly = true;
                penetrate = 4;
                thrown = true;
            }
            else if (type == 4)
            {
                arrow = true;
                name = "Unholy Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                light = 0.35f;
                penetrate = 5;
                ranged = true;
            }
            else if (type == 5)
            {
                arrow = true;
                name = "Jester's Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                light = 0.4f;
                penetrate = -1;
                timeLeft = 120;
                alpha = 100;
                ignoreWater = true;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 6)
            {
                name = "Enchanted Boomerang";
                width = 22;
                height = 22;
                aiStyle = 3;
                friendly = true;
                penetrate = -1;
                melee = true;
                light = 0.4f;
            }
            else if (type == 7 || type == 8)
            {
                name = "Vilethorn";
                width = 28;
                height = 28;
                aiStyle = 4;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                alpha = 255;
                ignoreWater = true;
                magic = true;
            }
            else if (type == 9)
            {
                name = "Starfury";
                width = 24;
                height = 24;
                aiStyle = 5;
                friendly = true;
                penetrate = 2;
                alpha = 50;
                scale = 0.8f;
                tileCollide = false;
                melee = true;
            }
            else if (type == 10)
            {
                name = "Purification Powder";
                width = 64;
                height = 64;
                aiStyle = 6;
                friendly = true;
                tileCollide = false;
                penetrate = -1;
                alpha = 255;
                ignoreWater = true;
            }
            else if (type == 11)
            {
                name = "Vile Powder";
                width = 48;
                height = 48;
                aiStyle = 6;
                friendly = true;
                tileCollide = false;
                penetrate = -1;
                alpha = 255;
                ignoreWater = true;
            }
            else if (type == 12)
            {
                name = "Falling Star";
                width = 16;
                height = 16;
                aiStyle = 5;
                friendly = true;
                penetrate = -1;
                alpha = 50;
                light = 1f;
            }
            else if (type == 13)
            {
                netImportant = true;
                name = "Hook";
                width = 18;
                height = 18;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
            }
            else if (type == 14)
            {
                name = "Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 255;
                scale = 1.2f;
                timeLeft = 600;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 15)
            {
                name = "Ball of Fire";
                width = 16;
                height = 16;
                aiStyle = 8;
                friendly = true;
                light = 0.8f;
                alpha = 100;
                magic = true;
            }
            else if (type == 16)
            {
                name = "Magic Missile";
                width = 10;
                height = 10;
                aiStyle = 9;
                friendly = true;
                light = 0.8f;
                alpha = 100;
                magic = true;
            }
            else if (type == 17)
            {
                name = "Dirt Ball";
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                ignoreWater = true;
            }
            else if (type == 18)
            {
                netImportant = true;
                name = "Shadow Orb";
                width = 32;
                height = 32;
                aiStyle = 11;
                friendly = true;
                light = 0.9f;
                alpha = 150;
                tileCollide = false;
                penetrate = -1;
                timeLeft *= 5;
                ignoreWater = true;
                scale = 0.8f;
            }
            else if (type == 19)
            {
                name = "Flamarang";
                width = 22;
                height = 22;
                aiStyle = 3;
                friendly = true;
                penetrate = -1;
                light = 1f;
                melee = true;
            }
            else if (type == 20)
            {
                name = "Green Laser";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 3;
                light = 0.75f;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.4f;
                timeLeft = 600;
                magic = true;
            }
            else if (type == 21)
            {
                name = "Bone";
                width = 16;
                height = 16;
                aiStyle = 2;
                scale = 1.2f;
                friendly = true;
                thrown = true;
            }
            else if (type == 22)
            {
                name = "Water Stream";
                width = 18;
                height = 18;
                aiStyle = 12;
                friendly = true;
                alpha = 255;
                penetrate = -1;
                extraUpdates = 2;
                ignoreWater = true;
                magic = true;
            }
            else if (type == 23)
            {
                name = "Harpoon";
                width = 4;
                height = 4;
                aiStyle = 13;
                friendly = true;
                penetrate = -1;
                alpha = 255;
                ranged = true;
            }
            else if (type == 24)
            {
                name = "Spiky Ball";
                width = 14;
                height = 14;
                aiStyle = 14;
                friendly = true;
                penetrate = 6;
                thrown = true;
            }
            else if (type == 25)
            {
                name = "Ball 'O Hurt";
                width = 22;
                height = 22;
                aiStyle = 15;
                friendly = true;
                penetrate = -1;
                melee = true;
                scale = 0.8f;
            }
            else if (type == 26)
            {
                name = "Blue Moon";
                width = 22;
                height = 22;
                aiStyle = 15;
                friendly = true;
                penetrate = -1;
                melee = true;
                scale = 0.8f;
            }
            else if (type == 27)
            {
                name = "Water Bolt";
                width = 16;
                height = 16;
                aiStyle = 8;
                friendly = true;
                alpha = 255;
                timeLeft /= 2;
                penetrate = 10;
                magic = true;
            }
            else if (type == 28)
            {
                name = "Bomb";
                width = 22;
                height = 22;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
            }
            else if (type == 29)
            {
                name = "Dynamite";
                width = 10;
                height = 10;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
            }
            else if (type == 30)
            {
                name = "Grenade";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                thrown = true;
            }
            else if (type == 31)
            {
                name = "Sand Ball";
                knockBack = 6f;
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                hostile = true;
                penetrate = -1;
            }
            else if (type == 32)
            {
                name = "Ivy Whip";
                width = 18;
                height = 18;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
            }
            else if (type == 33)
            {
                name = "Thorn Chakram";
                width = 38;
                height = 38;
                aiStyle = 3;
                friendly = true;
                scale = 0.9f;
                penetrate = -1;
                melee = true;
            }
            else if (type == 34)
            {
                name = "Flamelash";
                width = 14;
                height = 14;
                aiStyle = 9;
                friendly = true;
                light = 0.8f;
                alpha = 100;
                penetrate = 1;
                magic = true;
            }
            else if (type == 35)
            {
                name = "Sunfury";
                width = 22;
                height = 22;
                aiStyle = 15;
                friendly = true;
                penetrate = -1;
                melee = true;
                scale = 0.8f;
            }
            else if (type == 36)
            {
                name = "Meteor Shot";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 2;
                light = 0.6f;
                alpha = 255;
                scale = 1.4f;
                timeLeft = 600;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 37)
            {
                name = "Sticky Bomb";
                width = 22;
                height = 22;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
            }
            else if (type == 38)
            {
                name = "Harpy Feather";
                width = 14;
                height = 14;
                aiStyle = 0;
                hostile = true;
                penetrate = -1;
                aiStyle = 1;
                tileCollide = true;
            }
            else if (type == 39)
            {
                name = "Mud Ball";
                knockBack = 6f;
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                hostile = true;
                penetrate = -1;
            }
            else if (type == 40)
            {
                name = "Ash Ball";
                knockBack = 6f;
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                hostile = true;
                penetrate = -1;
            }
            else if (type == 41)
            {
                arrow = true;
                name = "Hellfire Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                penetrate = -1;
                ranged = true;
                light = 0.3f;
            }
            else if (type == 42)
            {
                name = "Sand Ball";
                knockBack = 8f;
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                extraUpdates = 1;
            }
            else if (type == 43)
            {
                name = "Tombstone";
                knockBack = 12f;
                width = 24;
                height = 24;
                aiStyle = 17;
                penetrate = -1;
            }
            else if (type == 44)
            {
                name = "Demon Sickle";
                width = 48;
                height = 48;
                alpha = 100;
                light = 0.2f;
                aiStyle = 18;
                hostile = true;
                penetrate = -1;
                tileCollide = true;
                scale = 0.9f;
            }
            else if (type == 45)
            {
                name = "Demon Scythe";
                width = 48;
                height = 48;
                alpha = 100;
                light = 0.2f;
                aiStyle = 18;
                friendly = true;
                penetrate = 5;
                tileCollide = true;
                scale = 0.9f;
                magic = true;
            }
            else if (type == 46)
            {
                name = "Dark Lance";
                width = 20;
                height = 20;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.1f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 47)
            {
                name = "Trident";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.1f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 48)
            {
                name = "Throwing Knife";
                width = 12;
                height = 12;
                aiStyle = 2;
                friendly = true;
                penetrate = 2;
                thrown = true;
            }
            else if (type == 49)
            {
                name = "Spear";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.2f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 50)
            {
                netImportant = true;
                name = "Glowstick";
                width = 6;
                height = 6;
                aiStyle = 14;
                penetrate = -1;
                alpha = 75;
                light = 1f;
                timeLeft *= 5;
            }
            else if (type == 51)
            {
                name = "Seed";
                width = 8;
                height = 8;
                aiStyle = 1;
                friendly = true;
            }
            else if (type == 52)
            {
                name = "Wooden Boomerang";
                width = 22;
                height = 22;
                aiStyle = 3;
                friendly = true;
                penetrate = -1;
                melee = true;
            }
            else if (type == 53)
            {
                netImportant = true;
                name = "Sticky Glowstick";
                width = 6;
                height = 6;
                aiStyle = 14;
                penetrate = -1;
                alpha = 75;
                light = 1f;
                timeLeft *= 5;
                tileCollide = false;
            }
            else if (type == 54)
            {
                name = "Poisoned Knife";
                width = 12;
                height = 12;
                aiStyle = 2;
                friendly = true;
                penetrate = 2;
                thrown = true;
            }
            else if (type == 55)
            {
                name = "Stinger";
                width = 10;
                height = 10;
                aiStyle = 0;
                hostile = true;
                penetrate = -1;
                aiStyle = 1;
                tileCollide = true;
            }
            else if (type == 56)
            {
                name = "Ebonsand Ball";
                knockBack = 6f;
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                hostile = true;
                penetrate = -1;
            }
            else if (type == 57)
            {
                name = "Cobalt Chainsaw";
                width = 18;
                height = 18;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 58)
            {
                name = "Mythril Chainsaw";
                width = 18;
                height = 18;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 1.08f;
            }
            else if (type == 59)
            {
                name = "Cobalt Drill";
                width = 22;
                height = 22;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 0.9f;
            }
            else if (type == 60)
            {
                name = "Mythril Drill";
                width = 22;
                height = 22;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 0.9f;
            }
            else if (type == 61)
            {
                name = "Adamantite Chainsaw";
                width = 18;
                height = 18;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 1.16f;
            }
            else if (type == 62)
            {
                name = "Adamantite Drill";
                width = 22;
                height = 22;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 0.9f;
            }
            else if (type == 63)
            {
                name = "The Dao of Pow";
                width = 22;
                height = 22;
                aiStyle = 15;
                friendly = true;
                penetrate = -1;
                melee = true;
            }
            else if (type == 64)
            {
                name = "Mythril Halberd";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.25f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 65)
            {
                name = "Ebonsand Ball";
                knockBack = 6f;
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                penetrate = -1;
                extraUpdates = 1;
            }
            else if (type == 66)
            {
                name = "Adamantite Glaive";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.27f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 67)
            {
                name = "Pearl Sand Ball";
                knockBack = 6f;
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                hostile = true;
                penetrate = -1;
            }
            else if (type == 68)
            {
                name = "Pearl Sand Ball";
                knockBack = 6f;
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                penetrate = -1;
                extraUpdates = 1;
            }
            else if (type == 69)
            {
                name = "Holy Water";
                width = 14;
                height = 14;
                aiStyle = 2;
                friendly = true;
                penetrate = 1;
            }
            else if (type == 70)
            {
                name = "Unholy Water";
                width = 14;
                height = 14;
                aiStyle = 2;
                friendly = true;
                penetrate = 1;
            }
            else if (type == 621)
            {
                name = "Blood Water";
                width = 14;
                height = 14;
                aiStyle = 2;
                friendly = true;
                penetrate = 1;
            }
            else if (type == 71)
            {
                name = "Silt Ball";
                knockBack = 6f;
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                hostile = true;
                penetrate = -1;
            }
            else if (type == 72)
            {
                netImportant = true;
                name = "Blue Fairy";
                width = 18;
                height = 18;
                aiStyle = 11;
                friendly = true;
                light = 0.9f;
                tileCollide = false;
                penetrate = -1;
                timeLeft *= 5;
                ignoreWater = true;
                scale = 0.8f;
            }
            else if (type == 73 || type == 74)
            {
                netImportant = true;
                name = "Hook";
                width = 18;
                height = 18;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
                light = 0.4f;
            }
            else if (type == 75)
            {
                name = "Happy Bomb";
                width = 22;
                height = 22;
                aiStyle = 16;
                hostile = true;
                penetrate = -1;
            }
            else if (type == 76 || type == 77 || type == 78)
            {
                if (type == 76)
                {
                    width = 10;
                    height = 22;
                }
                else if (type == 77)
                {
                    width = 18;
                    height = 24;
                }
                else
                {
                    width = 22;
                    height = 24;
                }
                name = "Note";
                aiStyle = 21;
                friendly = true;
                ranged = true;
                alpha = 100;
                light = 0.3f;
                penetrate = -1;
                timeLeft = 180;
                magic = true;
            }
            else if (type == 79)
            {
                name = "Rainbow";
                width = 10;
                height = 10;
                aiStyle = 9;
                friendly = true;
                light = 0.8f;
                alpha = 255;
                magic = true;
            }
            else if (type == 80)
            {
                name = "Ice Block";
                width = 16;
                height = 16;
                aiStyle = 22;
                friendly = true;
                magic = true;
                tileCollide = false;
                light = 0.5f;
                coldDamage = true;
            }
            else if (type == 81)
            {
                name = "Wooden Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                hostile = true;
                ranged = true;
            }
            else if (type == 82)
            {
                name = "Flaming Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                hostile = true;
                ranged = true;
            }
            else if (type == 83)
            {
                name = "Eye Laser";
                width = 4;
                height = 4;
                aiStyle = 1;
                hostile = true;
                penetrate = 3;
                light = 0.75f;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.7f;
                timeLeft = 600;
                magic = true;
            }
            else if (type == 84)
            {
                name = "Pink Laser";
                width = 4;
                height = 4;
                aiStyle = 1;
                hostile = true;
                penetrate = 3;
                light = 0.75f;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.2f;
                timeLeft = 600;
                magic = true;
            }
            else if (type == 85)
            {
                name = "Flames";
                width = 6;
                height = 6;
                aiStyle = 23;
                friendly = true;
                alpha = 255;
                penetrate = 3;
                extraUpdates = 2;
                ranged = true;
            }
            else if (type == 86)
            {
                netImportant = true;
                name = "Pink Fairy";
                width = 18;
                height = 18;
                aiStyle = 11;
                friendly = true;
                light = 0.9f;
                tileCollide = false;
                penetrate = -1;
                timeLeft *= 5;
                ignoreWater = true;
                scale = 0.8f;
            }
            else if (type == 87)
            {
                netImportant = true;
                name = "Pink Fairy";
                width = 18;
                height = 18;
                aiStyle = 11;
                friendly = true;
                light = 0.9f;
                tileCollide = false;
                penetrate = -1;
                timeLeft *= 5;
                ignoreWater = true;
                scale = 0.8f;
            }
            else if (type == 88)
            {
                name = "Purple Laser";
                width = 6;
                height = 6;
                aiStyle = 1;
                friendly = true;
                penetrate = 3;
                light = 0.75f;
                alpha = 255;
                extraUpdates = 4;
                scale = 1.4f;
                timeLeft = 600;
                magic = true;
            }
            else if (type == 89)
            {
                name = "Crystal Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 255;
                scale = 1.2f;
                timeLeft = 600;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 90)
            {
                name = "Crystal Shard";
                width = 6;
                height = 6;
                aiStyle = 24;
                friendly = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 50;
                scale = 1.2f;
                timeLeft = 600;
                ranged = true;
                tileCollide = false;
            }
            else if (type == 91)
            {
                arrow = true;
                name = "Holy Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
            }
            else if (type == 92)
            {
                name = "Hallow Star";
                width = 24;
                height = 24;
                aiStyle = 5;
                friendly = true;
                penetrate = 2;
                alpha = 50;
                scale = 0.8f;
                tileCollide = false;
                magic = true;
            }
            else if (type == 93)
            {
                light = 0.15f;
                name = "Magic Dagger";
                width = 12;
                height = 12;
                aiStyle = 2;
                friendly = true;
                penetrate = 2;
                magic = true;
            }
            else if (type == 94)
            {
                ignoreWater = true;
                name = "Crystal Storm";
                width = 8;
                height = 8;
                aiStyle = 24;
                friendly = true;
                light = 0.5f;
                alpha = 50;
                scale = 1.2f;
                timeLeft = 600;
                magic = true;
                tileCollide = true;
                penetrate = 1;
            }
            else if (type == 95)
            {
                name = "Cursed Flame";
                width = 16;
                height = 16;
                aiStyle = 8;
                friendly = true;
                light = 0.8f;
                alpha = 100;
                magic = true;
                penetrate = 2;
            }
            else if (type == 96)
            {
                name = "Cursed Flame";
                width = 16;
                height = 16;
                aiStyle = 8;
                hostile = true;
                light = 0.8f;
                alpha = 100;
                magic = true;
                penetrate = -1;
                scale = 0.9f;
                scale = 1.3f;
            }
            else if (type == 97)
            {
                name = "Cobalt Naginata";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.1f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 98)
            {
                name = "Poison Dart";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                hostile = true;
                penetrate = -1;
                trap = true;
            }
            else if (type == 99)
            {
                name = "Boulder";
                width = 31;
                height = 31;
                aiStyle = 25;
                friendly = true;
                hostile = true;
                ranged = true;
                penetrate = -1;
                trap = true;
            }
            else if (type == 100)
            {
                name = "Death Laser";
                width = 4;
                height = 4;
                aiStyle = 1;
                hostile = true;
                penetrate = 3;
                light = 0.75f;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.8f;
                timeLeft = 2700;
                magic = true;
            }
            else if (type == 101)
            {
                name = "Eye Fire";
                width = 6;
                height = 6;
                aiStyle = 23;
                hostile = true;
                alpha = 255;
                penetrate = -1;
                extraUpdates = 3;
                magic = true;
            }
            else if (type == 102)
            {
                name = "Bomb";
                width = 22;
                height = 22;
                aiStyle = 16;
                hostile = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 103)
            {
                arrow = true;
                name = "Cursed Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                light = 1f;
                ranged = true;
            }
            else if (type == 104)
            {
                name = "Cursed Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 255;
                scale = 1.2f;
                timeLeft = 600;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 105)
            {
                name = "Gungnir";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.3f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 106)
            {
                name = "Light Disc";
                width = 32;
                height = 32;
                aiStyle = 3;
                friendly = true;
                penetrate = -1;
                melee = true;
                light = 0.4f;
            }
            else if (type == 107)
            {
                name = "Hamdrax";
                width = 22;
                height = 22;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 1.1f;
            }
            else if (type == 108)
            {
                name = "Explosives";
                width = 260;
                height = 260;
                aiStyle = 16;
                friendly = true;
                hostile = true;
                penetrate = -1;
                tileCollide = false;
                alpha = 255;
                timeLeft = 2;
                trap = true;
            }
            else if (type == 109)
            {
                name = "Snow Ball";
                knockBack = 6f;
                width = 10;
                height = 10;
                aiStyle = 10;
                hostile = true;
                scale = 0.9f;
                penetrate = -1;
                coldDamage = true;
                thrown = true;
            }
            else if (type == 110)
            {
                name = "Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                hostile = true;
                penetrate = -1;
                light = 0.5f;
                alpha = 255;
                scale = 1.2f;
                timeLeft = 600;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 111)
            {
                netImportant = true;
                name = "Bunny";
                width = 18;
                height = 18;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 112)
            {
                netImportant = true;
                name = "Penguin";
                width = 18;
                height = 18;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 113)
            {
                name = "Ice Boomerang";
                width = 22;
                height = 22;
                aiStyle = 3;
                friendly = true;
                penetrate = -1;
                melee = true;
                light = 0.4f;
                coldDamage = true;
            }
            else if (type == 114)
            {
                name = "Unholy Trident";
                width = 16;
                height = 16;
                aiStyle = 27;
                magic = true;
                penetrate = 3;
                light = 0.5f;
                alpha = 255;
                friendly = true;
            }
            else if (type == 115)
            {
                name = "Unholy Trident";
                width = 16;
                height = 16;
                aiStyle = 27;
                hostile = true;
                magic = true;
                penetrate = -1;
                light = 0.5f;
                alpha = 255;
            }
            else if (type == 116)
            {
                name = "Sword Beam";
                width = 16;
                height = 16;
                aiStyle = 27;
                melee = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 255;
                friendly = true;
            }
            else if (type == 117)
            {
                arrow = true;
                name = "Bone Arrow";
                extraUpdates = 2;
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
            }
            else if (type == 118)
            {
                name = "Ice Bolt";
                width = 10;
                height = 10;
                aiStyle = 28;
                alpha = 255;
                melee = true;
                penetrate = 1;
                friendly = true;
                coldDamage = true;
            }
            else if (type == 119)
            {
                name = "Frost Bolt";
                width = 14;
                height = 14;
                aiStyle = 28;
                alpha = 255;
                melee = true;
                penetrate = 2;
                friendly = true;
            }
            else if (type == 120)
            {
                arrow = true;
                name = "Frost Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
                coldDamage = true;
                extraUpdates = 1;
            }
            else if (type == 121)
            {
                name = "Amethyst Bolt";
                width = 10;
                height = 10;
                aiStyle = 29;
                alpha = 255;
                magic = true;
                penetrate = 1;
                friendly = true;
            }
            else if (type == 122)
            {
                name = "Topaz Bolt";
                width = 10;
                height = 10;
                aiStyle = 29;
                alpha = 255;
                magic = true;
                penetrate = 1;
                friendly = true;
            }
            else if (type == 123)
            {
                name = "Sapphire Bolt";
                width = 10;
                height = 10;
                aiStyle = 29;
                alpha = 255;
                magic = true;
                penetrate = 1;
                friendly = true;
            }
            else if (type == 124)
            {
                name = "Emerald Bolt";
                width = 10;
                height = 10;
                aiStyle = 29;
                alpha = 255;
                magic = true;
                penetrate = 2;
                friendly = true;
            }
            else if (type == 125)
            {
                name = "Ruby Bolt";
                width = 10;
                height = 10;
                aiStyle = 29;
                alpha = 255;
                magic = true;
                penetrate = 2;
                friendly = true;
            }
            else if (type == 126)
            {
                name = "Diamond Bolt";
                width = 10;
                height = 10;
                aiStyle = 29;
                alpha = 255;
                magic = true;
                penetrate = 2;
                friendly = true;
            }
            else if (type == 127)
            {
                netImportant = true;
                name = "Turtle";
                width = 22;
                height = 22;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 128)
            {
                name = "Frost Blast";
                width = 14;
                height = 14;
                aiStyle = 28;
                alpha = 255;
                penetrate = -1;
                friendly = false;
                hostile = true;
                coldDamage = true;
            }
            else if (type == 129)
            {
                name = "Rune Blast";
                width = 14;
                height = 14;
                aiStyle = 28;
                alpha = 255;
                penetrate = -1;
                friendly = false;
                hostile = true;
                tileCollide = false;
            }
            else if (type == 130)
            {
                name = "Mushroom Spear";
                width = 22;
                height = 22;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.2f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 131)
            {
                name = "Mushroom";
                width = 22;
                height = 22;
                aiStyle = 30;
                friendly = true;
                penetrate = 1;
                tileCollide = false;
                melee = true;
                light = 0.5f;
            }
            else if (type == 132)
            {
                name = "Terra Beam";
                width = 16;
                height = 16;
                aiStyle = 27;
                melee = true;
                penetrate = 3;
                light = 0.5f;
                alpha = 255;
                friendly = true;
            }
            else if (type == 133)
            {
                name = "Grenade";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 134)
            {
                name = "Rocket";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 135)
            {
                name = "Proximity Mine";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 136)
            {
                name = "Grenade";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 137)
            {
                name = "Rocket";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 138)
            {
                name = "Proximity Mine";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 139)
            {
                name = "Grenade";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 140)
            {
                name = "Rocket";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 141)
            {
                name = "Proximity Mine";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 142)
            {
                name = "Grenade";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 143)
            {
                name = "Rocket";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 144)
            {
                name = "Proximity Mine";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 145)
            {
                name = "Pure Spray";
                width = 6;
                height = 6;
                aiStyle = 31;
                friendly = true;
                alpha = 255;
                penetrate = -1;
                extraUpdates = 2;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type == 146)
            {
                name = "Hallow Spray";
                width = 6;
                height = 6;
                aiStyle = 31;
                friendly = true;
                alpha = 255;
                penetrate = -1;
                extraUpdates = 2;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type == 147)
            {
                name = "Corrupt Spray";
                width = 6;
                height = 6;
                aiStyle = 31;
                friendly = true;
                alpha = 255;
                penetrate = -1;
                extraUpdates = 2;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type == 148)
            {
                name = "Mushroom Spray";
                width = 6;
                height = 6;
                aiStyle = 31;
                friendly = true;
                alpha = 255;
                penetrate = -1;
                extraUpdates = 2;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type == 149)
            {
                name = "Crimson Spray";
                width = 6;
                height = 6;
                aiStyle = 31;
                friendly = true;
                alpha = 255;
                penetrate = -1;
                extraUpdates = 2;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type == 150 || type == 151 || type == 152)
            {
                name = "Nettle Burst";
                width = 28;
                height = 28;
                aiStyle = 4;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                alpha = 255;
                ignoreWater = true;
                magic = true;
            }
            else if (type == 153)
            {
                name = "The Rotted Fork";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.1f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 154)
            {
                name = "The Meatball";
                width = 22;
                height = 22;
                aiStyle = 15;
                friendly = true;
                penetrate = -1;
                melee = true;
                scale = 0.8f;
            }
            else if (type == 155)
            {
                netImportant = true;
                name = "Beach Ball";
                width = 44;
                height = 44;
                aiStyle = 32;
                friendly = true;
            }
            else if (type == 156)
            {
                name = "Light Beam";
                width = 16;
                height = 16;
                aiStyle = 27;
                melee = true;
                light = 0.5f;
                alpha = 255;
                friendly = true;
            }
            else if (type == 157)
            {
                name = "Night Beam";
                width = 32;
                height = 32;
                aiStyle = 27;
                melee = true;
                light = 0.5f;
                alpha = 255;
                friendly = true;
                scale = 1.2f;
            }
            else if (type == 158)
            {
                name = "Copper Coin";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                alpha = 255;
                timeLeft = 600;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 159)
            {
                name = "Silver Coin";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                alpha = 255;
                timeLeft = 600;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 160)
            {
                name = "Gold Coin";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                alpha = 255;
                timeLeft = 600;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 161)
            {
                name = "Platinum Coin";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                alpha = 255;
                timeLeft = 600;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 162)
            {
                name = "Cannonball";
                width = 16;
                height = 16;
                aiStyle = 2;
                friendly = true;
                penetrate = 4;
                alpha = 255;
            }
            else if (type == 163)
            {
                netImportant = true;
                name = "Flare";
                width = 6;
                height = 6;
                aiStyle = 33;
                friendly = true;
                penetrate = -1;
                alpha = 255;
                timeLeft = 36000;
            }
            else if (type == 164)
            {
                name = "Landmine";
                width = 128;
                height = 128;
                aiStyle = 16;
                friendly = true;
                hostile = true;
                penetrate = -1;
                tileCollide = false;
                alpha = 255;
                timeLeft = 2;
            }
            else if (type == 165)
            {
                netImportant = true;
                name = "Web";
                width = 12;
                height = 12;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
            }
            else if (type == 166)
            {
                name = "Snow Ball";
                width = 14;
                height = 14;
                aiStyle = 2;
                friendly = true;
                ranged = true;
                coldDamage = true;
            }
            else if (type == 167 || type == 168 || (type == 169 || type == 170))
            {
                name = "Rocket";
                width = 14;
                height = 14;
                aiStyle = 34;
                friendly = true;
                ranged = true;
                timeLeft = 45;
            }
            else if (type == 171 || type == 505 || type == 506)
            {
                name = "Rope Coil";
                width = 14;
                height = 14;
                aiStyle = 35;
                penetrate = -1;
                tileCollide = false;
                timeLeft = 400;
            }
            else if (type == 172)
            {
                arrow = true;
                name = "Frostburn Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                light = 1f;
                ranged = true;
                coldDamage = true;
            }
            else if (type == 173)
            {
                name = "Enchanted Beam";
                width = 16;
                height = 16;
                aiStyle = 27;
                melee = true;
                penetrate = 1;
                light = 0.2f;
                alpha = 255;
                friendly = true;
            }
            else if (type == 174)
            {
                name = "Ice Spike";
                alpha = 255;
                width = 6;
                height = 6;
                aiStyle = 1;
                hostile = true;
                penetrate = -1;
                coldDamage = true;
            }
            else if (type == 175)
            {
                name = "Baby Eater";
                width = 34;
                height = 34;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 176)
            {
                name = "Jungle Spike";
                alpha = 255;
                width = 6;
                height = 6;
                aiStyle = 1;
                hostile = true;
                penetrate = -1;
            }
            else if (type == 177)
            {
                name = "Icewater Spit";
                width = 10;
                height = 10;
                aiStyle = 28;
                alpha = 255;
                penetrate = -1;
                friendly = false;
                hostile = true;
                coldDamage = true;
            }
            else if (type == 178)
            {
                name = "Confetti";
                width = 10;
                height = 10;
                aiStyle = 1;
                alpha = 255;
                penetrate = -1;
                timeLeft = 2;
            }
            else if (type == 179)
            {
                name = "Slush Ball";
                knockBack = 6f;
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                hostile = true;
                penetrate = -1;
            }
            else if (type == 180)
            {
                name = "Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                hostile = true;
                penetrate = -1;
                light = 0.5f;
                alpha = 255;
                scale = 1.2f;
                timeLeft = 600;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 181)
            {
                name = "Bee";
                width = 8;
                height = 8;
                aiStyle = 36;
                friendly = true;
                penetrate = 3;
                alpha = 255;
                timeLeft = 600;
                extraUpdates = 3;
            }
            else if (type == 182)
            {
                light = 0.15f;
                name = "Possessed Hatchet";
                width = 30;
                height = 30;
                aiStyle = 3;
                friendly = true;
                penetrate = 10;
                melee = true;
                extraUpdates = 1;
            }
            else if (type == 183)
            {
                name = "Beenade";
                width = 14;
                height = 22;
                aiStyle = 14;
                penetrate = 1;
                ranged = true;
                timeLeft = 180;
                thrown = true;
                friendly = true;
            }
            else if (type == 184)
            {
                name = "Poison Dart";
                width = 6;
                height = 6;
                aiStyle = 1;
                friendly = true;
                hostile = true;
                penetrate = -1;
                trap = true;
            }
            else if (type == 185)
            {
                name = "Spiky Ball";
                width = 14;
                height = 14;
                aiStyle = 14;
                friendly = true;
                hostile = true;
                penetrate = -1;
                timeLeft = 900;
                trap = true;
            }
            else if (type == 186)
            {
                name = "Spear";
                width = 10;
                height = 14;
                aiStyle = 37;
                friendly = true;
                tileCollide = false;
                ignoreWater = true;
                hostile = true;
                penetrate = -1;
                timeLeft = 300;
                trap = true;
            }
            else if (type == 187)
            {
                name = "Flamethrower";
                width = 6;
                height = 6;
                aiStyle = 38;
                alpha = 255;
                tileCollide = false;
                ignoreWater = true;
                timeLeft = 60;
                trap = true;
            }
            else if (type == 188)
            {
                name = "Flames";
                width = 6;
                height = 6;
                aiStyle = 23;
                friendly = true;
                hostile = true;
                alpha = 255;
                penetrate = -1;
                extraUpdates = 2;
                trap = true;
            }
            else if (type == 189)
            {
                name = "Wasp";
                width = 8;
                height = 8;
                aiStyle = 36;
                friendly = true;
                penetrate = 4;
                alpha = 255;
                timeLeft = 600;
                magic = true;
                extraUpdates = 3;
            }
            else if (type == 190)
            {
                name = "Mechanical Piranha";
                width = 22;
                height = 22;
                aiStyle = 39;
                friendly = true;
                penetrate = -1;
                alpha = 255;
                ranged = true;
            }
            else if (type >= 191 && type <= 194)
            {
                netImportant = true;
                name = "Pygmy";
                width = 18;
                height = 18;
                aiStyle = 26;
                penetrate = -1;
                timeLeft *= 5;
                minion = true;
                minionSlots = 1f;
                if (type == 192)
                    scale = 1.025f;
                if (type == 193)
                    scale = 1.05f;
                if (type == 194)
                    scale = 1.075f;
            }
            else if (type == 195)
            {
                tileCollide = false;
                name = "Pygmy";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
            }
            else if (type == 196)
            {
                name = "Smoke Bomb";
                width = 16;
                height = 16;
                aiStyle = 14;
                penetrate = -1;
                scale = 0.8f;
            }
            else if (type == 197)
            {
                netImportant = true;
                name = "Baby Skeletron Head";
                width = 42;
                height = 42;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 198)
            {
                netImportant = true;
                name = "Baby Hornet";
                width = 26;
                height = 26;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 199)
            {
                netImportant = true;
                name = "Tiki Spirit";
                width = 28;
                height = 28;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 200)
            {
                netImportant = true;
                name = "Pet Lizard";
                width = 28;
                height = 28;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 201)
            {
                name = "Tombstone";
                knockBack = 12f;
                width = 24;
                height = 24;
                aiStyle = 17;
                penetrate = -1;
            }
            else if (type == 202)
            {
                name = "Tombstone";
                knockBack = 12f;
                width = 24;
                height = 24;
                aiStyle = 17;
                penetrate = -1;
            }
            else if (type == 203)
            {
                name = "Tombstone";
                knockBack = 12f;
                width = 24;
                height = 24;
                aiStyle = 17;
                penetrate = -1;
            }
            else if (type == 204)
            {
                name = "Tombstone";
                knockBack = 12f;
                width = 24;
                height = 24;
                aiStyle = 17;
                penetrate = -1;
            }
            else if (type == 205)
            {
                name = "Tombstone";
                knockBack = 12f;
                width = 24;
                height = 24;
                aiStyle = 17;
                penetrate = -1;
            }
            else if (type == 206)
            {
                name = "Leaf";
                width = 14;
                height = 14;
                aiStyle = 40;
                friendly = true;
                penetrate = 1;
                alpha = 255;
                timeLeft = 600;
                magic = true;
            }
            else if (type == 207)
            {
                name = "Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.2f;
                timeLeft = 600;
                ranged = true;
            }
            else if (type == 208)
            {
                netImportant = true;
                name = "Parrot";
                width = 18;
                height = 36;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 209)
            {
                name = "Truffle";
                width = 12;
                height = 32;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
                light = 0.5f;
            }
            else if (type == 210)
            {
                netImportant = true;
                name = "Sapling";
                width = 14;
                height = 30;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 211)
            {
                netImportant = true;
                name = "Wisp";
                width = 24;
                height = 24;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
                light = 1f;
                ignoreWater = true;
            }
            else if (type == 212)
            {
                name = "Palladium Pike";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.12f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 213)
            {
                name = "Palladium Drill";
                width = 22;
                height = 22;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 0.92f;
            }
            else if (type == 214)
            {
                name = "Palladium Chainsaw";
                width = 18;
                height = 18;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 215)
            {
                name = "Orichalcum Halberd";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.27f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 216)
            {
                name = "Orichalcum Drill";
                width = 22;
                height = 22;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 0.93f;
            }
            else if (type == 217)
            {
                name = "Orichalcum Chainsaw";
                width = 18;
                height = 18;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 1.12f;
            }
            else if (type == 218)
            {
                name = "Titanium Trident";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.28f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 219)
            {
                name = "Titanium Drill";
                width = 22;
                height = 22;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 0.95f;
            }
            else if (type == 220)
            {
                name = "Titanium Chainsaw";
                width = 18;
                height = 18;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 1.2f;
            }
            else if (type == 221)
            {
                name = "Flower Petal";
                width = 20;
                height = 20;
                aiStyle = 41;
                friendly = true;
                tileCollide = false;
                ignoreWater = true;
                timeLeft = 120;
                penetrate = -1;
                scale = (float)(1.0 + Main.rand.Next(30) * 0.00999999977648258);
                extraUpdates = 2;
            }
            else if (type == 222)
            {
                name = "Chlorophyte Partisan";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.3f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 223)
            {
                name = "Chlorophyte Drill";
                width = 22;
                height = 22;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 1f;
            }
            else if (type == 224)
            {
                name = "Chlorophyte Chainsaw";
                width = 18;
                height = 18;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 1.1f;
            }
            else if (type == 225)
            {
                arrow = true;
                penetrate = 2;
                name = "Chlorophyte Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
            }
            else if (type == 226)
            {
                netImportant = true;
                name = "Crystal Leaf";
                width = 22;
                height = 42;
                aiStyle = 42;
                friendly = true;
                tileCollide = false;
                penetrate = -1;
                timeLeft *= 5;
                light = 0.4f;
                ignoreWater = true;
            }
            else if (type == 227)
            {
                netImportant = true;
                tileCollide = false;
                light = 0.1f;
                name = "Crystal Leaf";
                width = 14;
                height = 14;
                aiStyle = 43;
                friendly = true;
                penetrate = 1;
                timeLeft = 180;
            }
            else if (type == 228)
            {
                tileCollide = false;
                name = "Spore Cloud";
                width = 30;
                height = 30;
                aiStyle = 44;
                friendly = true;
                scale = 1.1f;
                penetrate = -1;
            }
            else if (type == 229)
            {
                name = "Chlorophyte Orb";
                width = 30;
                height = 30;
                aiStyle = 44;
                friendly = true;
                penetrate = -1;
                light = 0.2f;
            }
            else if (type >= 230 && type <= 235)
            {
                netImportant = true;
                name = "Gem Hook";
                width = 18;
                height = 18;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
            }
            else if (type == 236)
            {
                netImportant = true;
                name = "Baby Dino";
                width = 34;
                height = 34;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 237)
            {
                netImportant = true;
                name = "Rain Cloud";
                width = 28;
                height = 28;
                aiStyle = 45;
                penetrate = -1;
            }
            else if (type == 238)
            {
                tileCollide = false;
                ignoreWater = true;
                name = "Rain Cloud";
                width = 54;
                height = 28;
                aiStyle = 45;
                penetrate = -1;
            }
            else if (type == 239)
            {
                ignoreWater = true;
                name = "Rain";
                width = 4;
                height = 40;
                aiStyle = 45;
                friendly = true;
                penetrate = -1;
                timeLeft = 300;
                scale = 1.1f;
                magic = true;
                extraUpdates = 1;
            }
            else if (type == 240)
            {
                name = "Cannonball";
                width = 16;
                height = 16;
                aiStyle = 2;
                hostile = true;
                penetrate = -1;
                alpha = 255;
            }
            else if (type == 241)
            {
                name = "Crimsand Ball";
                knockBack = 6f;
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                hostile = true;
                penetrate = -1;
            }
            else if (type == 242)
            {
                name = "Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 255;
                extraUpdates = 7;
                scale = 1.18f;
                timeLeft = 600;
                ranged = true;
                ignoreWater = true;
            }
            else if (type == 243)
            {
                name = "Blood Cloud";
                width = 28;
                height = 28;
                aiStyle = 45;
                penetrate = -1;
            }
            else if (type == 244)
            {
                tileCollide = false;
                ignoreWater = true;
                name = "Blood Cloud";
                width = 54;
                height = 28;
                aiStyle = 45;
                penetrate = -1;
            }
            else if (type == 245)
            {
                ignoreWater = true;
                name = "Blood Rain";
                width = 4;
                height = 40;
                aiStyle = 45;
                friendly = true;
                penetrate = 2;
                timeLeft = 300;
                scale = 1.1f;
                magic = true;
                extraUpdates = 1;
            }
            else if (type == 246)
            {
                name = "Stynger";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
                alpha = 255;
                extraUpdates = 1;
            }
            else if (type == 247)
            {
                name = "Flower Pow";
                width = 34;
                height = 34;
                aiStyle = 15;
                friendly = true;
                penetrate = -1;
                melee = true;
            }
            else if (type == 248)
            {
                name = "Flower Pow";
                width = 18;
                height = 18;
                aiStyle = 1;
                friendly = true;
                melee = true;
            }
            else if (type == 249)
            {
                name = "Stynger";
                width = 12;
                height = 12;
                aiStyle = 2;
                friendly = true;
                ranged = true;
            }
            else if (type == 250)
            {
                name = "Rainbow";
                width = 12;
                height = 12;
                aiStyle = 46;
                penetrate = -1;
                magic = true;
                alpha = 255;
                ignoreWater = true;
                scale = 1.25f;
            }
            else if (type == 251)
            {
                name = "Rainbow";
                width = 14;
                height = 14;
                aiStyle = 46;
                friendly = true;
                penetrate = -1;
                magic = true;
                alpha = 255;
                light = 0.3f;
                tileCollide = false;
                ignoreWater = true;
                scale = 1.25f;
            }
            else if (type == 252)
            {
                name = "Chlorophyte Jackhammer";
                width = 18;
                height = 18;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 1.1f;
            }
            else if (type == 253)
            {
                name = "Ball of Frost";
                width = 16;
                height = 16;
                aiStyle = 8;
                friendly = true;
                light = 0.8f;
                alpha = 100;
                magic = true;
            }
            else if (type == 254)
            {
                name = "Magnet Sphere";
                width = 38;
                height = 38;
                aiStyle = 47;
                magic = true;
                timeLeft = 660;
                light = 0.5f;
            }
            else if (type == 255)
            {
                name = "Magnet Sphere";
                width = 8;
                height = 8;
                aiStyle = 48;
                friendly = true;
                magic = true;
                extraUpdates = 100;
                timeLeft = 100;
            }
            else if (type == 256)
            {
                netImportant = true;
                tileCollide = false;
                name = "Skeletron Hand";
                width = 6;
                height = 6;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                scale = 1f;
                timeLeft *= 10;
            }
            else if (type == 257)
            {
                name = "Frost Beam";
                ignoreWater = true;
                width = 4;
                height = 4;
                aiStyle = 1;
                hostile = true;
                penetrate = -1;
                light = 0.75f;
                alpha = 255;
                scale = 1.2f;
                timeLeft = 600;
                magic = true;
                coldDamage = true;
                extraUpdates = 1;
            }
            else if (type == 258)
            {
                name = "Fireball";
                width = 16;
                height = 16;
                aiStyle = 8;
                hostile = true;
                penetrate = -1;
                alpha = 100;
                timeLeft = 300;
            }
            else if (type == 259)
            {
                name = "Eye Beam";
                ignoreWater = true;
                tileCollide = false;
                width = 8;
                height = 8;
                aiStyle = 1;
                hostile = true;
                penetrate = -1;
                light = 0.3f;
                scale = 1.1f;
                magic = true;
                extraUpdates = 1;
            }
            else if (type == 260)
            {
                name = "Heat Ray";
                width = 8;
                height = 8;
                aiStyle = 48;
                friendly = true;
                magic = true;
                extraUpdates = 100;
                timeLeft = 200;
                penetrate = -1;
            }
            else if (type == 261)
            {
                name = "Boulder";
                width = 32;
                height = 34;
                aiStyle = 14;
                friendly = true;
                penetrate = 6;
                magic = true;
                ignoreWater = true;
            }
            else if (type == 262)
            {
                name = "Golem Fist";
                width = 30;
                height = 30;
                aiStyle = 13;
                friendly = true;
                penetrate = -1;
                alpha = 255;
                melee = true;
                extraUpdates = 1;
            }
            else if (type == 263)
            {
                name = "Ice Sickle";
                width = 34;
                height = 34;
                alpha = 100;
                light = 0.5f;
                aiStyle = 18;
                friendly = true;
                penetrate = 5;
                tileCollide = true;
                scale = 1f;
                melee = true;
                timeLeft = 180;
                coldDamage = true;
            }
            else if (type == 264)
            {
                ignoreWater = true;
                name = "Rain";
                width = 4;
                height = 40;
                aiStyle = 45;
                hostile = true;
                penetrate = -1;
                timeLeft = 120;
                scale = 1.1f;
                extraUpdates = 1;
            }
            else if (type == 265)
            {
                name = "Poison Fang";
                width = 12;
                height = 12;
                aiStyle = 1;
                alpha = 255;
                friendly = true;
                magic = true;
                penetrate = 4;
            }
            else if (type == 266)
            {
                netImportant = true;
                alpha = 75;
                name = "Baby Slime";
                width = 24;
                height = 16;
                aiStyle = 26;
                penetrate = -1;
                timeLeft *= 5;
                minion = true;
                minionSlots = 1f;
            }
            else if (type == 267)
            {
                alpha = 255;
                name = "Poison Dart";
                width = 14;
                height = 14;
                aiStyle = 1;
                friendly = true;
                ranged = true;
            }
            else if (type == 268)
            {
                netImportant = true;
                name = "Eye Spring";
                width = 18;
                height = 32;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 269)
            {
                netImportant = true;
                name = "Baby Snowman";
                width = 20;
                height = 26;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 270)
            {
                name = "Skull";
                width = 26;
                height = 26;
                aiStyle = 1;
                alpha = 255;
                friendly = true;
                magic = true;
                penetrate = 3;
            }
            else if (type == 271)
            {
                name = "Boxing Glove";
                width = 20;
                height = 20;
                aiStyle = 13;
                friendly = true;
                penetrate = -1;
                alpha = 255;
                melee = true;
                scale = 1.2f;
            }
            else if (type == 272)
            {
                name = "Bananarang";
                width = 32;
                height = 32;
                aiStyle = 3;
                friendly = true;
                scale = 0.9f;
                penetrate = -1;
                melee = true;
            }
            else if (type == 273)
            {
                name = "Chain Knife";
                width = 26;
                height = 26;
                aiStyle = 13;
                friendly = true;
                penetrate = -1;
                alpha = 255;
                melee = true;
            }
            else if (type == 274)
            {
                name = "Death Sickle";
                width = 42;
                height = 42;
                alpha = 100;
                light = 0.5f;
                aiStyle = 18;
                friendly = true;
                penetrate = 5;
                tileCollide = false;
                scale = 1.1f;
                melee = true;
                timeLeft = 180;
            }
            else if (type == 275)
            {
                alpha = 255;
                name = "Seed";
                width = 14;
                height = 14;
                aiStyle = 1;
                hostile = true;
            }
            else if (type == 276)
            {
                alpha = 255;
                name = "Poison Seed";
                width = 14;
                height = 14;
                aiStyle = 1;
                hostile = true;
            }
            else if (type == 277)
            {
                alpha = 255;
                name = "Thorn Ball";
                width = 38;
                height = 38;
                aiStyle = 14;
                hostile = true;
            }
            else if (type == 278)
            {
                arrow = true;
                name = "Ichor Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                light = 1f;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 279)
            {
                name = "Ichor Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.25f;
                timeLeft = 600;
                ranged = true;
            }
            else if (type == 280)
            {
                name = "Golden Shower";
                width = 32;
                height = 32;
                aiStyle = 12;
                friendly = true;
                alpha = 255;
                penetrate = 5;
                extraUpdates = 2;
                ignoreWater = true;
                magic = true;
            }
            else if (type == 281)
            {
                name = "Explosive Bunny";
                width = 28;
                height = 28;
                aiStyle = 49;
                friendly = true;
                penetrate = 1;
                alpha = 255;
                timeLeft = 600;
            }
            else if (type == 282)
            {
                arrow = true;
                name = "Venom Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 283)
            {
                name = "Venom Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.25f;
                timeLeft = 600;
                ranged = true;
            }
            else if (type == 284)
            {
                name = "Party Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.3f;
                timeLeft = 600;
                ranged = true;
            }
            else if (type == 285)
            {
                name = "Nano Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.3f;
                timeLeft = 600;
                ranged = true;
            }
            else if (type == 286)
            {
                name = "Explosive Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.3f;
                timeLeft = 600;
                ranged = true;
            }
            else if (type == 287)
            {
                name = "Golden Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                light = 0.5f;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.3f;
                timeLeft = 600;
                ranged = true;
            }
            else if (type == 288)
            {
                name = "Golden Shower";
                width = 32;
                height = 32;
                aiStyle = 12;
                hostile = true;
                alpha = 255;
                penetrate = -1;
                extraUpdates = 2;
                ignoreWater = true;
                magic = true;
            }
            else if (type == 289)
            {
                name = "Confetti";
                width = 10;
                height = 10;
                aiStyle = 1;
                alpha = 255;
                penetrate = -1;
                timeLeft = 2;
            }
            else if (type == 290)
            {
                name = "Shadow Beam";
                width = 4;
                height = 4;
                aiStyle = 48;
                hostile = true;
                magic = true;
                extraUpdates = 100;
                timeLeft = 100;
                penetrate = -1;
            }
            else if (type == 291)
            {
                name = "Inferno";
                width = 12;
                height = 12;
                aiStyle = 50;
                hostile = true;
                alpha = 255;
                magic = true;
                tileCollide = false;
                penetrate = -1;
            }
            else if (type == 292)
            {
                name = "Inferno";
                width = 130;
                height = 130;
                aiStyle = 50;
                hostile = true;
                alpha = 255;
                magic = true;
                tileCollide = false;
                penetrate = -1;
            }
            else if (type == 293)
            {
                name = "Lost Soul";
                width = 12;
                height = 12;
                aiStyle = 51;
                hostile = true;
                alpha = 255;
                magic = true;
                tileCollide = false;
                penetrate = -1;
                extraUpdates = 1;
            }
            else if (type == 294)
            {
                name = "Shadow Beam";
                width = 4;
                height = 4;
                aiStyle = 48;
                friendly = true;
                magic = true;
                extraUpdates = 100;
                timeLeft = 300;
                penetrate = -1;
            }
            else if (type == 295)
            {
                name = "Inferno";
                width = 12;
                height = 12;
                aiStyle = 50;
                friendly = true;
                alpha = 255;
                magic = true;
                tileCollide = true;
            }
            else if (type == 296)
            {
                name = "Inferno";
                width = 150;
                height = 150;
                aiStyle = 50;
                friendly = true;
                alpha = 255;
                magic = true;
                tileCollide = false;
                penetrate = -1;
            }
            else if (type == 297)
            {
                name = "Lost Soul";
                width = 12;
                height = 12;
                aiStyle = 51;
                friendly = true;
                alpha = 255;
                magic = true;
                extraUpdates = 1;
            }
            else if (type == 298)
            {
                name = "Spirit Heal";
                width = 6;
                height = 6;
                aiStyle = 52;
                alpha = 255;
                magic = true;
                tileCollide = false;
                extraUpdates = 3;
            }
            else if (type == 299)
            {
                name = "Shadowflames";
                width = 6;
                height = 6;
                aiStyle = 1;
                hostile = true;
                alpha = 255;
                penetrate = -1;
                extraUpdates = 2;
                magic = true;
                ignoreWater = true;
                tileCollide = false;
            }
            else if (type == 300)
            {
                name = "Paladin's Hammer";
                width = 38;
                height = 38;
                aiStyle = 2;
                hostile = true;
                penetrate = -1;
                ignoreWater = true;
                tileCollide = false;
            }
            else if (type == 301)
            {
                name = "Paladin's Hammer";
                width = 38;
                height = 38;
                aiStyle = 3;
                friendly = true;
                penetrate = -1;
                melee = true;
                extraUpdates = 2;
            }
            else if (type == 302)
            {
                name = "Sniper Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                hostile = true;
                penetrate = -1;
                light = 0.3f;
                alpha = 255;
                extraUpdates = 7;
                scale = 1.18f;
                timeLeft = 300;
                ranged = true;
                ignoreWater = true;
            }
            else if (type == 303)
            {
                name = "Rocket";
                width = 14;
                height = 14;
                aiStyle = 16;
                hostile = true;
                penetrate = -1;
                ranged = true;
            }
            else if (type == 304)
            {
                name = "Vampire Knife";
                alpha = 255;
                width = 30;
                height = 30;
                aiStyle = 2;
                friendly = true;
                penetrate = 1;
                melee = true;
                light = 0.2f;
                ignoreWater = true;
                extraUpdates = 0;
            }
            else if (type == 305)
            {
                name = "Vampire Heal";
                width = 6;
                height = 6;
                aiStyle = 52;
                alpha = 255;
                tileCollide = false;
                extraUpdates = 10;
            }
            else if (type == 306)
            {
                name = "Eater's Bite";
                alpha = 255;
                width = 14;
                height = 14;
                aiStyle = 2;
                friendly = true;
                penetrate = 1;
                melee = true;
                ignoreWater = true;
                extraUpdates = 1;
            }
            else if (type == 307)
            {
                name = "Tiny Eater";
                width = 16;
                height = 16;
                aiStyle = 36;
                penetrate = 1;
                alpha = 255;
                timeLeft = 600;
                melee = true;
                extraUpdates = 3;
            }
            else if (type == 308)
            {
                name = "Frost Hydra";
                width = 80;
                height = 74;
                aiStyle = 53;
                timeLeft = 7200;
                light = 0.25f;
                ignoreWater = true;
                coldDamage = true;
            }
            else if (type == 309)
            {
                name = "Frost Blast";
                width = 14;
                height = 14;
                aiStyle = 28;
                alpha = 255;
                penetrate = 1;
                friendly = true;
                extraUpdates = 3;
                coldDamage = true;
            }
            else if (type == 310)
            {
                netImportant = true;
                name = "Blue Flare";
                width = 6;
                height = 6;
                aiStyle = 33;
                friendly = true;
                penetrate = -1;
                alpha = 255;
                timeLeft = 36000;
            }
            else if (type == 311)
            {
                name = "Candy Corn";
                width = 10;
                height = 12;
                aiStyle = 1;
                friendly = true;
                penetrate = 3;
                alpha = 255;
                timeLeft = 600;
                ranged = true;
            }
            else if (type == 312)
            {
                name = "Jack 'O Lantern";
                alpha = 255;
                width = 32;
                height = 32;
                aiStyle = 1;
                friendly = true;
                ranged = true;
                timeLeft = 300;
            }
            else if (type == 313)
            {
                netImportant = true;
                name = "Spider";
                width = 30;
                height = 30;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 314)
            {
                netImportant = true;
                name = "Squashling";
                width = 24;
                height = 40;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 315)
            {
                netImportant = true;
                name = "Bat Hook";
                width = 14;
                height = 14;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
            }
            else if (type == 316)
            {
                alpha = 255;
                name = "Bat";
                width = 16;
                height = 16;
                aiStyle = 36;
                friendly = true;
                penetrate = 1;
                timeLeft = 600;
                magic = true;
            }
            else if (type == 317)
            {
                netImportant = true;
                name = "Raven";
                width = 28;
                height = 28;
                aiStyle = 54;
                penetrate = 1;
                timeLeft *= 5;
                minion = true;
                minionSlots = 1f;
            }
            else if (type == 318)
            {
                name = "Rotten Egg";
                width = 12;
                height = 14;
                aiStyle = 2;
                friendly = true;
                thrown = true;
            }
            else if (type == 319)
            {
                netImportant = true;
                name = "Black Cat";
                width = 36;
                height = 30;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 320)
            {
                name = "Bloody Machete";
                width = 34;
                height = 34;
                aiStyle = 3;
                friendly = true;
                penetrate = -1;
                melee = true;
            }
            else if (type == 321)
            {
                name = "Flaming Jack";
                width = 30;
                height = 30;
                aiStyle = 55;
                friendly = true;
                melee = true;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type == 322)
            {
                netImportant = true;
                name = "Wood Hook";
                width = 14;
                height = 14;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
            }
            else if (type == 323)
            {
                penetrate = 10;
                name = "Stake";
                extraUpdates = 3;
                width = 14;
                height = 14;
                aiStyle = 1;
                alpha = 255;
                friendly = true;
                ranged = true;
                scale = 0.8f;
            }
            else if (type == 324)
            {
                netImportant = true;
                name = "Cursed Sapling";
                width = 26;
                height = 38;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 325)
            {
                alpha = 255;
                penetrate = -1;
                name = "Flaming Wood";
                width = 14;
                height = 14;
                aiStyle = 1;
                hostile = true;
                tileCollide = false;
            }
            else if (type >= 326 && type <= 328)
            {
                name = "Greek Fire";
                if (type == 326)
                {
                    width = 14;
                    height = 16;
                }
                else if (type == 327)
                {
                    width = 12;
                    height = 14;
                }
                else
                {
                    width = 6;
                    height = 12;
                }
                aiStyle = 14;
                hostile = true;
                penetrate = -1;
                timeLeft = 360;
            }
            else if (type == 329)
            {
                name = "Flaming Scythe";
                width = 80;
                height = 80;
                light = 0.25f;
                aiStyle = 56;
                hostile = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft = 420;
            }
            else if (type == 330)
            {
                name = "Star Anise";
                width = 22;
                height = 22;
                aiStyle = 2;
                friendly = true;
                penetrate = 6;
                thrown = true;
            }
            else if (type == 331)
            {
                netImportant = true;
                name = "Candy Cane Hook";
                width = 18;
                height = 18;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
            }
            else if (type == 332)
            {
                netImportant = true;
                name = "Christmas Hook";
                width = 18;
                height = 18;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
                light = 0.5f;
            }
            else if (type == 333)
            {
                name = "Fruitcake Chakram";
                width = 38;
                height = 38;
                aiStyle = 3;
                friendly = true;
                scale = 0.9f;
                penetrate = -1;
                melee = true;
            }
            else if (type == 334)
            {
                netImportant = true;
                name = "Puppy";
                width = 28;
                height = 28;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 335)
            {
                name = "Ornament";
                width = 22;
                height = 22;
                aiStyle = 2;
                friendly = true;
                penetrate = 1;
                melee = true;
            }
            else if (type == 336)
            {
                name = "Pine Needle";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                magic = true;
                scale = 0.8f;
                extraUpdates = 1;
            }
            else if (type == 337)
            {
                name = "Blizzard";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                magic = true;
                tileCollide = false;
                coldDamage = true;
                extraUpdates = 1;
            }
            else if (type == 338 || type == 339 || (type == 340 || type == 341))
            {
                name = "Rocket";
                width = 14;
                height = 14;
                aiStyle = 16;
                penetrate = -1;
                friendly = true;
                ranged = true;
                scale = 0.9f;
            }
            else if (type == 342)
            {
                name = "North Pole";
                width = 22;
                height = 2;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.1f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                coldDamage = true;
            }
            else if (type == 343)
            {
                alpha = 255;
                name = "North Pole";
                width = 10;
                height = 10;
                aiStyle = 57;
                friendly = true;
                melee = true;
                scale = 1.1f;
                penetrate = 3;
                coldDamage = true;
            }
            else if (type == 344)
            {
                name = "North Pole";
                width = 26;
                height = 26;
                aiStyle = 1;
                friendly = true;
                scale = 0.9f;
                alpha = 255;
                melee = true;
                coldDamage = true;
            }
            else if (type == 345)
            {
                name = "Pine Needle";
                width = 4;
                height = 4;
                aiStyle = 1;
                hostile = true;
                scale = 0.8f;
            }
            else if (type == 346)
            {
                name = "Ornament";
                width = 18;
                height = 18;
                aiStyle = 14;
                hostile = true;
                penetrate = -1;
                timeLeft = 300;
            }
            else if (type == 347)
            {
                name = "Ornament";
                width = 6;
                height = 6;
                aiStyle = 2;
                hostile = true;
                penetrate = -1;
            }
            else if (type == 348)
            {
                name = "Frost Wave";
                aiStyle = 1;
                width = 48;
                height = 48;
                hostile = true;
                penetrate = -1;
                tileCollide = false;
                coldDamage = true;
                extraUpdates = 1;
            }
            else if (type == 349)
            {
                name = "Frost Shard";
                aiStyle = 1;
                width = 12;
                height = 12;
                hostile = true;
                penetrate = -1;
                coldDamage = true;
            }
            else if (type == 350)
            {
                alpha = 255;
                penetrate = -1;
                name = "Missile";
                width = 14;
                height = 14;
                aiStyle = 1;
                hostile = true;
                tileCollide = false;
                timeLeft /= 2;
            }
            else if (type == 351)
            {
                alpha = 255;
                penetrate = -1;
                name = "Present";
                width = 24;
                height = 24;
                aiStyle = 58;
                hostile = true;
                tileCollide = false;
            }
            else if (type == 352)
            {
                name = "Spike";
                width = 30;
                height = 30;
                aiStyle = 14;
                hostile = true;
                penetrate = -1;
                timeLeft /= 3;
            }
            else if (type == 353)
            {
                netImportant = true;
                name = "Baby Grinch";
                width = 18;
                height = 28;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 354)
            {
                name = "Crimsand Ball";
                knockBack = 6f;
                width = 10;
                height = 10;
                aiStyle = 10;
                friendly = true;
                penetrate = -1;
                extraUpdates = 1;
            }
            else if (type == 355)
            {
                name = "Venom Fang";
                width = 12;
                height = 12;
                aiStyle = 1;
                alpha = 255;
                friendly = true;
                magic = true;
                penetrate = 7;
            }
            else if (type == 356)
            {
                name = "Spectre Wrath";
                width = 6;
                height = 6;
                aiStyle = 59;
                alpha = 255;
                magic = true;
                tileCollide = false;
                extraUpdates = 3;
            }
            else if (type == 357)
            {
                name = "Pulse Bolt";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 6;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.2f;
                timeLeft = 600;
                ranged = true;
            }
            else if (type == 358)
            {
                name = "Water Gun";
                width = 18;
                height = 18;
                aiStyle = 60;
                alpha = 255;
                penetrate = -1;
                extraUpdates = 2;
                ignoreWater = true;
            }
            else if (type == 359)
            {
                name = "Frost Bolt";
                width = 14;
                height = 14;
                aiStyle = 28;
                alpha = 255;
                magic = true;
                penetrate = 2;
                friendly = true;
                coldDamage = true;
            }
            else if (type >= 360 && type <= 366 || (type == 381 || type == 382))
            {
                name = "Bobber";
                width = 14;
                height = 14;
                aiStyle = 61;
                penetrate = -1;
                bobber = true;
            }
            else if (type == 367)
            {
                name = "Obsidian Swordfish";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                scale = 1.1f;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 368)
            {
                name = "Swordfish";
                width = 18;
                height = 18;
                aiStyle = 19;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 369)
            {
                name = "Sawtooth Shark";
                width = 22;
                height = 22;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 370)
            {
                name = "Love Potion";
                width = 14;
                height = 14;
                aiStyle = 2;
                friendly = true;
                penetrate = 1;
            }
            else if (type == 371)
            {
                name = "Foul Potion";
                width = 14;
                height = 14;
                aiStyle = 2;
                friendly = true;
                penetrate = 1;
            }
            else if (type == 372)
            {
                name = "Fish Hook";
                width = 18;
                height = 18;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
            }
            else if (type == 373)
            {
                netImportant = true;
                name = "Hornet";
                width = 24;
                height = 26;
                aiStyle = 62;
                penetrate = -1;
                timeLeft *= 5;
                minion = true;
                minionSlots = 1f;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type == 374)
            {
                name = "Hornet Stinger";
                width = 10;
                height = 10;
                aiStyle = 0;
                friendly = true;
                penetrate = 1;
                aiStyle = 1;
                tileCollide = true;
                scale *= 0.9f;
            }
            else if (type == 375)
            {
                netImportant = true;
                name = "Flying Imp";
                width = 34;
                height = 26;
                aiStyle = 62;
                penetrate = -1;
                timeLeft *= 5;
                minion = true;
                minionSlots = 1f;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type == 376)
            {
                name = "Imp Fireball";
                width = 12;
                height = 12;
                aiStyle = 0;
                friendly = true;
                penetrate = -1;
                aiStyle = 1;
                tileCollide = true;
                timeLeft = 100;
                alpha = 255;
                extraUpdates = 1;
            }
            else if (type == 377)
            {
                name = "Spider Turret";
                width = 66;
                height = 50;
                aiStyle = 53;
                timeLeft = 7200;
                ignoreWater = true;
            }
            else if (type == 378)
            {
                name = "Spider Egg";
                width = 16;
                height = 16;
                aiStyle = 14;
                friendly = true;
                penetrate = -1;
                timeLeft = 60;
                scale = 0.9f;
            }
            else if (type == 379)
            {
                name = "Baby Spider";
                width = 14;
                height = 10;
                aiStyle = 63;
                friendly = true;
                timeLeft = 300;
                penetrate = 1;
            }
            else if (type == 380)
            {
                netImportant = true;
                name = "Zephyr Fish";
                width = 26;
                height = 26;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 383)
            {
                name = "Anchor";
                width = 34;
                height = 34;
                aiStyle = 3;
                friendly = true;
                penetrate = -1;
                melee = true;
            }
            else if (type == 384)
            {
                name = "Sharknado";
                width = 150;
                height = 42;
                hostile = true;
                penetrate = -1;
                aiStyle = 64;
                tileCollide = false;
                ignoreWater = true;
                alpha = 255;
                timeLeft = 540;
            }
            else if (type == 385)
            {
                name = "Sharknado Bolt";
                width = 30;
                height = 30;
                hostile = true;
                penetrate = -1;
                aiStyle = 65;
                alpha = 255;
                timeLeft = 300;
            }
            else if (type == 386)
            {
                name = "Cthulunado";
                width = 150;
                height = 42;
                hostile = true;
                penetrate = -1;
                aiStyle = 64;
                tileCollide = false;
                ignoreWater = true;
                alpha = 255;
                timeLeft = 840;
            }
            else if (type == 387)
            {
                netImportant = true;
                name = "Retanimini";
                width = 40;
                height = 20;
                aiStyle = 66;
                penetrate = -1;
                timeLeft *= 5;
                minion = true;
                minionSlots = 0.5f;
                tileCollide = false;
                ignoreWater = true;
                friendly = true;
            }
            else if (type == 388)
            {
                netImportant = true;
                name = "Spazmamini";
                width = 40;
                height = 20;
                aiStyle = 66;
                penetrate = -1;
                timeLeft *= 5;
                minion = true;
                minionSlots = 0.5f;
                tileCollide = false;
                ignoreWater = true;
                friendly = true;
            }
            else if (type == 389)
            {
                name = "Mini Retina Laser";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                penetrate = 3;
                light = 0.75f;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.2f;
                timeLeft = 600;
            }
            else if (type == 390 || type == 391 || type == 392)
            {
                name = "Venom Spider";
                width = 18;
                height = 18;
                aiStyle = 26;
                penetrate = -1;
                netImportant = true;
                timeLeft *= 5;
                minion = true;
                minionSlots = 0.75f;
                if (type == 391)
                    name = "Jumper Spider";
                if (type == 392)
                    name = "Dangerous Spider";
            }
            else if (type == 393 || type == 394 || type == 395)
            {
                name = "One Eyed Pirate";
                width = 20;
                height = 30;
                aiStyle = 67;
                penetrate = -1;
                netImportant = true;
                timeLeft *= 5;
                minion = true;
                minionSlots = 1f;
                if (type == 394)
                    name = "Soulscourge Pirate";
                if (type == 395)
                    name = "Pirate Captain";
            }
            else if (type == 396)
            {
                name = "Slime Hook";
                width = 18;
                height = 18;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
                alpha = 100;
            }
            else if (type == 397)
            {
                name = "Sticky Grenade";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                thrown = true;
                tileCollide = false;
            }
            else if (type == 398)
            {
                netImportant = true;
                name = "Mini Minotaur";
                width = 18;
                height = 38;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 399)
            {
                name = "Molotov Cocktail";
                width = 14;
                height = 14;
                aiStyle = 68;
                friendly = true;
                penetrate = 1;
                alpha = 255;
                thrown = true;
                noEnchantments = true;
            }
            else if (type >= 400 && type <= 402)
            {
                name = "Molotov Fire";
                if (type == 400)
                {
                    width = 14;
                    height = 16;
                }
                else if (type == 401)
                {
                    width = 12;
                    height = 14;
                }
                else
                {
                    width = 6;
                    height = 12;
                }
                penetrate = 3;
                aiStyle = 14;
                friendly = true;
                timeLeft = 360;
                ranged = true;
                noEnchantments = true;
            }
            else if (type == 403)
            {
                netImportant = true;
                name = "Track Hook";
                width = 18;
                height = 18;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
            }
            else if (type == 404)
            {
                name = "Flairon";
                width = 26;
                height = 26;
                aiStyle = 69;
                friendly = true;
                penetrate = -1;
                alpha = 255;
                melee = true;
            }
            else if (type == 405)
            {
                name = "Flairon Bubble";
                width = 14;
                height = 14;
                aiStyle = 70;
                friendly = true;
                penetrate = 1;
                alpha = 255;
                timeLeft = 90;
                melee = true;
                noEnchantments = true;
            }
            else if (type == 406)
            {
                name = "Slime Gun";
                width = 14;
                height = 14;
                aiStyle = 60;
                alpha = 255;
                penetrate = -1;
                extraUpdates = 2;
                ignoreWater = true;
            }
            else if (type == 407)
            {
                netImportant = true;
                name = "Tempest";
                width = 28;
                height = 40;
                aiStyle = 62;
                penetrate = -1;
                timeLeft *= 5;
                minion = true;
                friendly = true;
                minionSlots = 1f;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type == 408)
            {
                name = "Mini Sharkron";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                alpha = 255;
                ignoreWater = true;
            }
            else if (type == 409)
            {
                name = "Typhoon";
                width = 30;
                height = 30;
                penetrate = -1;
                aiStyle = 71;
                alpha = 255;
                timeLeft = 360;
                friendly = true;
                tileCollide = true;
                extraUpdates = 2;
                magic = true;
                ignoreWater = true;
            }
            else if (type == 410)
            {
                name = "Bubble";
                width = 14;
                height = 14;
                aiStyle = 72;
                friendly = true;
                penetrate = 1;
                alpha = 255;
                timeLeft = 50;
                magic = true;
                ignoreWater = true;
            }
            else if (type >= 411 && type <= 414)
            {
                switch (type)
                {
                    case 411:
                        name = "Copper Coins";
                        break;
                    case 412:
                        name = "Silver Coins";
                        break;
                    case 413:
                        name = "Gold Coins";
                        break;
                    case 414:
                        name = "Platinum Coins";
                        break;
                }
                width = 10;
                height = 10;
                aiStyle = 10;
            }
            else if (type == 415 || type == 416 || (type == 417 || type == 418))
            {
                name = "Rocket";
                width = 14;
                height = 14;
                aiStyle = 34;
                friendly = true;
                ranged = true;
                timeLeft = 45;
            }
            else if (type >= 419 && type <= 422)
            {
                name = "Firework Fountain";
                width = 4;
                height = 4;
                aiStyle = 73;
                friendly = true;
            }
            else if (type == 423)
            {
                netImportant = true;
                name = "UFO";
                width = 28;
                height = 28;
                aiStyle = 62;
                penetrate = -1;
                timeLeft *= 5;
                minion = true;
                friendly = true;
                minionSlots = 1f;
                ignoreWater = true;
            }
            else if (type >= 424 && type <= 426)
            {
                name = "Meteor";
                width = 24;
                height = 24;
                aiStyle = 1;
                friendly = true;
                magic = true;
                tileCollide = false;
                extraUpdates = 2;
            }
            else if (type == 427)
            {
                name = "Vortex Chainsaw";
                width = 22;
                height = 56;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                glowMask = (short)2;
            }
            else if (type == 428)
            {
                name = "Vortex Drill";
                width = 26;
                height = 54;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                glowMask = (short)3;
            }
            else if (type == 429)
            {
                name = "Nebula Chainsaw";
                width = 18;
                height = 56;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                glowMask = (short)7;
            }
            else if (type == 430)
            {
                name = "Nebula Drill";
                width = 30;
                height = 54;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                glowMask = (short)8;
            }
            else if (type == 431)
            {
                name = "Solar Flare Chainsaw";
                width = 28;
                height = 64;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 432)
            {
                name = "Solar Flare Drill";
                width = 30;
                height = 54;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
            }
            else if (type == 610)
            {
                name = "Stardust Chainsaw";
                width = 28;
                height = 64;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                glowMask = (short)179;
            }
            else if (type == 609)
            {
                name = "Stardust Drill";
                width = 30;
                height = 54;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                glowMask = (short)180;
            }
            else if (type == 433)
            {
                name = "UFO Ray";
                width = 8;
                height = 8;
                aiStyle = 48;
                friendly = true;
                extraUpdates = 100;
                timeLeft = 100;
                ignoreWater = true;
            }
            else if (type == 434)
            {
                name = "Scutlix Laser";
                width = 1;
                height = 1;
                aiStyle = 74;
                friendly = true;
                extraUpdates = 100;
                penetrate = -1;
            }
            else if (type == 435)
            {
                name = "Electric Bolt";
                width = 10;
                height = 10;
                aiStyle = 1;
                hostile = true;
                ignoreWater = true;
            }
            else if (type == 436)
            {
                name = "Brain Scrambling Bolt";
                width = 10;
                height = 10;
                aiStyle = 1;
                hostile = true;
                ignoreWater = true;
            }
            else if (type == 437)
            {
                name = "Gigazapper Spearhead";
                width = 10;
                height = 10;
                aiStyle = 1;
                hostile = true;
                extraUpdates = 2;
                ignoreWater = true;
            }
            else if (type == 438)
            {
                name = "Laser Ray";
                width = 8;
                height = 8;
                aiStyle = 1;
                hostile = true;
                alpha = 255;
                extraUpdates = 3;
                ignoreWater = true;
            }
            else if (type == 439)
            {
                name = "Laser Machinegun";
                width = 22;
                height = 22;
                aiStyle = 75;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                magic = true;
                ignoreWater = true;
            }
            else if (type == 440)
            {
                name = "Laser";
                width = 5;
                height = 5;
                aiStyle = 1;
                friendly = true;
                alpha = 255;
                extraUpdates = 2;
                scale = 1f;
                timeLeft = 600;
                magic = true;
                ignoreWater = true;
            }
            else if (type == 441)
            {
                name = "Scutlix Crosshair";
                width = 1;
                height = 1;
                aiStyle = 76;
                ignoreWater = true;
                tileCollide = false;
            }
            else if (type == 442)
            {
                name = "Electrosphere Missile";
                width = 14;
                height = 14;
                aiStyle = 1;
                friendly = true;
                alpha = 255;
                scale = 1f;
                timeLeft = 600;
                ranged = true;
            }
            else if (type == 443)
            {
                name = "Electrosphere";
                width = 80;
                height = 80;
                aiStyle = 77;
                friendly = true;
                alpha = 255;
                scale = 1f;
                ranged = true;
                ignoreWater = true;
                tileCollide = false;
                penetrate = -1;
            }
            else if (type == 444)
            {
                name = "Xenopopper";
                width = 10;
                height = 10;
                aiStyle = 78;
                friendly = true;
                alpha = 255;
                scale = 1f;
                ranged = true;
                ignoreWater = true;
                extraUpdates = 1;
            }
            else if (type == 445)
            {
                name = "Laser Drill";
                width = 10;
                height = 10;
                aiStyle = 75;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                melee = true;
                ignoreWater = true;
                ownerHitCheck = true;
            }
            else if (type == 446)
            {
                netImportant = true;
                name = "Anti-Gravity Hook";
                width = 14;
                height = 14;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
                light = 0.7f;
            }
            else if (type == 447)
            {
                name = "Martian Deathray";
                width = 30;
                height = 30;
                aiStyle = 79;
                hostile = true;
                penetrate = -1;
                tileCollide = false;
                ignoreWater = true;
                timeLeft = 240;
            }
            else if (type == 448)
            {
                name = "Martian Rocket";
                width = 14;
                height = 14;
                aiStyle = 80;
                hostile = true;
                penetrate = -1;
                tileCollide = false;
            }
            else if (type == 449)
            {
                name = "Saucer Laser";
                width = 5;
                height = 5;
                aiStyle = 1;
                hostile = true;
                alpha = 255;
                extraUpdates = 1;
                scale = 1f;
                timeLeft = 600;
                ignoreWater = true;
            }
            else if (type == 450)
            {
                name = "Saucer Scrap";
                width = 14;
                height = 14;
                aiStyle = 14;
                hostile = true;
                penetrate = -1;
                timeLeft = 360;
            }
            else if (type == 451)
            {
                name = "Influx Waver";
                width = 16;
                height = 16;
                aiStyle = 81;
                melee = true;
                penetrate = 3;
                light = 0.2f;
                alpha = 255;
                friendly = true;
            }
            else if (type == 452)
            {
                name = "Phantasmal Eye";
                width = 14;
                height = 14;
                aiStyle = 82;
                hostile = true;
                penetrate = -1;
                alpha = 255;
                timeLeft = 600;
            }
            else if (type == 453)
            {
                name = "Drill Crosshair";
                width = 1;
                height = 1;
                aiStyle = 76;
                ignoreWater = true;
                tileCollide = false;
            }
            else if (type == 454)
            {
                name = "Phantasmal Sphere";
                width = 46;
                height = 46;
                aiStyle = 83;
                hostile = true;
                penetrate = -1;
                alpha = 255;
                timeLeft = 600;
                tileCollide = false;
            }
            else if (type == 455)
            {
                name = "Phantasmal Deathray";
                width = 36;
                height = 36;
                aiStyle = 84;
                hostile = true;
                penetrate = -1;
                alpha = 255;
                timeLeft = 600;
                tileCollide = false;
            }
            else if (type == 456)
            {
                name = "Moon Leech";
                width = 16;
                height = 16;
                aiStyle = 85;
                hostile = true;
                penetrate = -1;
                alpha = 255;
                timeLeft = 600;
                tileCollide = false;
            }
            else if (type == 459)
            {
                name = "Charged Blaster Orb";
                width = 22;
                height = 22;
                aiStyle = 1;
                friendly = true;
                magic = true;
                alpha = 255;
                scale = 1f;
                ignoreWater = true;
                extraUpdates = 1;
            }
            else if (type == 460)
            {
                name = "Charged Blaster Cannon";
                width = 14;
                height = 18;
                aiStyle = 75;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                magic = true;
                ignoreWater = true;
            }
            else if (type == 461)
            {
                name = "Charged Blaster Laser";
                width = 18;
                height = 18;
                aiStyle = 84;
                friendly = true;
                magic = true;
                penetrate = -1;
                alpha = 255;
                tileCollide = false;
                hide = true;
            }
            else if (type == 462)
            {
                name = "Phantasmal Bolt";
                width = 8;
                height = 8;
                aiStyle = 1;
                hostile = true;
                alpha = 255;
                extraUpdates = 3;
                ignoreWater = true;
                tileCollide = false;
            }
            else if (type == 463)
            {
                name = "Vicious Powder";
                width = 48;
                height = 48;
                aiStyle = 6;
                friendly = true;
                tileCollide = false;
                penetrate = -1;
                alpha = 255;
                ignoreWater = true;
            }
            else if (type == 464)
            {
                name = "Ice Mist";
                width = 60;
                height = 60;
                aiStyle = 86;
                hostile = true;
                tileCollide = false;
                penetrate = -1;
                alpha = 255;
                ignoreWater = true;
            }
            else if (type == 467)
            {
                name = "Fireball";
                width = 40;
                height = 40;
                aiStyle = 1;
                hostile = true;
                alpha = 255;
                ignoreWater = true;
                extraUpdates = 1;
            }
            else if (type == 468)
            {
                name = "Shadow Fireball";
                width = 40;
                height = 40;
                aiStyle = 1;
                hostile = true;
                alpha = 255;
                ignoreWater = true;
                extraUpdates = 1;
            }
            else if (type == 465)
            {
                name = "Lightning Orb";
                width = 80;
                height = 80;
                aiStyle = 88;
                hostile = true;
                alpha = 255;
                ignoreWater = true;
                tileCollide = false;
            }
            else if (type == 466)
            {
                name = "Lightning Orb Arc";
                width = 14;
                height = 14;
                aiStyle = 88;
                hostile = true;
                alpha = 255;
                ignoreWater = true;
                tileCollide = true;
                extraUpdates = 4;
                timeLeft = 120 * (extraUpdates + 1);
            }
            else if (type == 491)
            {
                name = "Flying Knife";
                width = 26;
                height = 26;
                aiStyle = 9;
                friendly = true;
                melee = true;
                penetrate = -1;
            }
            else if (type == 500)
            {
                name = "Crimson Heart";
                width = 20;
                height = 20;
                aiStyle = 67;
                penetrate = -1;
                netImportant = true;
                timeLeft *= 5;
                friendly = true;
                ignoreWater = true;
                scale = 0.8f;
            }
            else if (type == 499)
            {
                netImportant = true;
                name = "Baby Face Monster";
                width = 34;
                height = 34;
                aiStyle = 26;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 469)
            {
                alpha = 255;
                arrow = true;
                name = "Bee Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
            }
            else if (type == 470)
            {
                name = "Sticky Dynamite";
                width = 10;
                height = 10;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
            }
            else if (type == 471)
            {
                name = "Bone";
                width = 16;
                height = 16;
                aiStyle = 2;
                scale = 1.2f;
                hostile = true;
                ranged = true;
            }
            else if (type == 472)
            {
                name = "Web spit";
                width = 8;
                height = 8;
                aiStyle = 0;
                hostile = true;
                penetrate = -1;
                aiStyle = 1;
                tileCollide = true;
                timeLeft = 50;
            }
            else if (type == 474)
            {
                arrow = true;
                name = "Bone Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
            }
            else if (type == 473)
            {
                netImportant = true;
                name = "Spelunker Glowstick";
                width = 8;
                height = 8;
                aiStyle = 14;
                penetrate = -1;
                alpha = 75;
                light = 1f;
                timeLeft *= 2;
            }
            else if (type == 475)
            {
                name = "Vine Rope Coil";
                width = 14;
                height = 14;
                aiStyle = 35;
                penetrate = -1;
                tileCollide = false;
                timeLeft = 400;
            }
            else if (type == 476)
            {
                name = "Soul Drain";
                width = 200;
                height = 200;
                aiStyle = -1;
                friendly = true;
                tileCollide = false;
                penetrate = -1;
                alpha = 255;
                ignoreWater = true;
                timeLeft = 3;
            }
            else if (type == 477)
            {
                alpha = 255;
                name = "Crystal Dart";
                width = 14;
                height = 14;
                aiStyle = 1;
                friendly = true;
                penetrate = 7;
                extraUpdates = 1;
                ranged = true;
            }
            else if (type == 478)
            {
                alpha = 255;
                name = "Cursed Dart";
                width = 14;
                height = 14;
                aiStyle = 1;
                friendly = true;
                timeLeft = 300;
                ranged = true;
            }
            else if (type == 479)
            {
                alpha = 255;
                name = "Ichor Dart";
                width = 14;
                height = 14;
                aiStyle = 1;
                friendly = true;
                ranged = true;
            }
            else if (type == 480)
            {
                alpha = 255;
                name = "Cursed Flame";
                width = 12;
                height = 12;
                penetrate = 3;
                aiStyle = 14;
                friendly = true;
                timeLeft = 120;
                ranged = true;
                noEnchantments = true;
            }
            else if (type == 481)
            {
                name = "Chain Guillotine";
                width = 22;
                height = 22;
                aiStyle = 13;
                friendly = true;
                penetrate = -1;
                alpha = 255;
                melee = true;
                extraUpdates = 0;
            }
            else if (type == 482)
            {
                name = "Cursed Flames";
                width = 16;
                height = 200;
                aiStyle = 87;
                friendly = true;
                tileCollide = false;
                penetrate = 20;
                alpha = 255;
                ignoreWater = true;
                timeLeft = 2700;
            }
            else if (type == 483)
            {
                arrow = true;
                name = "Seedler";
                width = 14;
                height = 14;
                aiStyle = 14;
                friendly = true;
                ranged = true;
            }
            else if (type == 484)
            {
                arrow = true;
                name = "Seedler";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
                extraUpdates = 1;
            }
            else if (type == 485)
            {
                arrow = true;
                name = "Hellwing";
                width = 24;
                height = 24;
                aiStyle = 1;
                friendly = true;
                ranged = true;
                penetrate = -1;
            }
            else if (type >= 486 && type <= 489)
            {
                name = "Hook";
                if (type == 486)
                {
                    width = 12;
                    height = 12;
                }
                else if (type == 487)
                {
                    width = 22;
                    height = 22;
                }
                else if (type == 488)
                {
                    width = 12;
                    height = 12;
                    light = 0.3f;
                }
                else if (type == 489)
                {
                    width = 20;
                    height = 16;
                }
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
            }
            else if (type == 492)
            {
                netImportant = true;
                name = "Magic Lantern";
                width = 18;
                height = 32;
                aiStyle = 90;
                friendly = true;
                penetrate = -1;
                timeLeft *= 5;
            }
            else if (type == 490)
            {
                name = "Lightning Ritual";
                width = 14;
                height = 14;
                aiStyle = 89;
                hostile = true;
                alpha = 255;
                ignoreWater = true;
                tileCollide = false;
                timeLeft = 600;
                netImportant = true;
            }
            else if (type == 493 || type == 494)
            {
                name = "Crystal Vile Shard";
                width = 32;
                height = 32;
                aiStyle = 4;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                alpha = 255;
                ignoreWater = true;
                magic = true;
                light = 0.2f;
            }
            else if (type == 495)
            {
                arrow = true;
                name = "Shadowflame Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
                penetrate = 3;
            }
            else if (type == 496)
            {
                alpha = 255;
                name = "Shadowflame";
                width = 40;
                height = 40;
                aiStyle = 91;
                friendly = true;
                magic = true;
                MaxUpdates = 3;
                penetrate = 3;
            }
            else if (type == 497)
            {
                name = "Shadowflame Knife";
                width = 30;
                height = 30;
                aiStyle = 2;
                friendly = true;
                penetrate = 3;
                melee = true;
            }
            else if (type == 498)
            {
                name = "Nail";
                width = 6;
                height = 6;
                aiStyle = 1;
                hostile = true;
                penetrate = -1;
                timeLeft = 180;
            }
            else if (type == 501)
            {
                name = "Flask";
                width = 14;
                height = 14;
                aiStyle = 2;
                scale = 1.1f;
                hostile = true;
                ranged = true;
            }
            else if (type == 502)
            {
                name = "Meowmere";
                width = 16;
                height = 16;
                aiStyle = 8;
                friendly = true;
                melee = true;
                penetrate = 5;
            }
            else if (type == 503)
            {
                name = "Star Wrath";
                width = 24;
                height = 24;
                aiStyle = 5;
                friendly = true;
                penetrate = 2;
                alpha = 255;
                tileCollide = false;
                melee = true;
                extraUpdates = 1;
            }
            else if (type == 504)
            {
                name = "Spark";
                width = 10;
                height = 10;
                aiStyle = 2;
                friendly = true;
                magic = true;
                alpha = 255;
                penetrate = 2;
            }
            else if (type == 507)
            {
                name = "Javelin";
                width = 16;
                height = 16;
                aiStyle = 1;
                friendly = true;
                melee = true;
                penetrate = 3;
            }
            else if (type == 508)
            {
                name = "Javelin";
                width = 16;
                height = 16;
                aiStyle = 1;
                hostile = true;
                melee = true;
                penetrate = -1;
            }
            else if (type == 509)
            {
                name = "Butcher's Chainsaw";
                width = 22;
                height = 22;
                aiStyle = 20;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ownerHitCheck = true;
                melee = true;
                scale = 1.2f;
            }
            else if (type == 510)
            {
                name = "Toxic Flask";
                width = 18;
                height = 18;
                aiStyle = 2;
                friendly = true;
                penetrate = 1;
                magic = true;
            }
            else if (type == 511)
            {
                name = "Toxic Cloud";
                width = 32;
                height = 32;
                aiStyle = 92;
                friendly = true;
                penetrate = -1;
                scale = 1.1f;
                magic = true;
            }
            else if (type == 512)
            {
                name = "Toxic Cloud";
                width = 40;
                height = 38;
                aiStyle = 92;
                friendly = true;
                penetrate = -1;
                scale = 1.1f;
                magic = true;
            }
            else if (type == 513)
            {
                name = "Toxic Cloud";
                width = 30;
                height = 28;
                aiStyle = 92;
                friendly = true;
                penetrate = -1;
                scale = 1.1f;
                magic = true;
            }
            else if (type == 514)
            {
                name = "Nail";
                width = 10;
                height = 10;
                aiStyle = 93;
                friendly = true;
                penetrate = 3;
                alpha = 255;
                ranged = true;
            }
            else if (type == 515)
            {
                netImportant = true;
                name = "Bouncy Glowstick";
                width = 6;
                height = 6;
                aiStyle = 14;
                penetrate = -1;
                alpha = 75;
                light = 1f;
                timeLeft *= 5;
            }
            else if (type == 516)
            {
                name = "Bouncy Bomb";
                width = 22;
                height = 22;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
            }
            else if (type == 517)
            {
                name = "Bouncy Grenade";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                thrown = true;
            }
            else if (type == 518)
            {
                name = "Coin Portal";
                width = 32;
                height = 32;
                aiStyle = 94;
                friendly = true;
                alpha = 255;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type == 519)
            {
                name = "Bomb Fish";
                width = 24;
                height = 24;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
            }
            else if (type == 520)
            {
                name = "Frost Daggerfish";
                width = 22;
                height = 22;
                aiStyle = 2;
                friendly = true;
                penetrate = 3;
                thrown = true;
            }
            else if (type == 521)
            {
                name = "Crystal Charge";
                width = 14;
                height = 14;
                aiStyle = 29;
                alpha = 255;
                magic = true;
                penetrate = 1;
                friendly = true;
            }
            else if (type == 522)
            {
                name = "Crystal Charge";
                width = 8;
                height = 8;
                aiStyle = 29;
                alpha = 255;
                magic = true;
                penetrate = 1;
                friendly = true;
            }
            else if (type == 523)
            {
                name = "Toxic Bubble";
                width = 32;
                height = 32;
                aiStyle = 95;
                alpha = 255;
                ranged = true;
                penetrate = 1;
                friendly = true;
            }
            else if (type == 524)
            {
                name = "Ichor Splash";
                width = 10;
                height = 10;
                aiStyle = 96;
                friendly = true;
                alpha = 255;
                penetrate = -1;
                ignoreWater = true;
                melee = true;
                extraUpdates = 5;
            }
            else if (type == 525)
            {
                name = "Flying Piggy Bank";
                width = 30;
                height = 24;
                aiStyle = 97;
                tileCollide = false;
                timeLeft = 10800;
            }
            else if (type == 526)
            {
                name = "Energy";
                width = 8;
                height = 8;
                aiStyle = 98;
                tileCollide = false;
                timeLeft = 120;
                alpha = 255;
            }
            else if (type >= 527 && type <= 531)
            {
                name = "Tombstone";
                knockBack = 12f;
                width = 24;
                height = 24;
                aiStyle = 17;
                penetrate = -1;
            }
            else if (type == 532)
            {
                name = "XBone";
                width = 16;
                height = 16;
                aiStyle = 1;
                scale = 1f;
                friendly = true;
                thrown = true;
                penetrate = 3;
                extraUpdates = 1;
            }
            else if (type == 533)
            {
                netImportant = true;
                name = "Deadly Sphere";
                width = 20;
                height = 20;
                aiStyle = 66;
                penetrate = -1;
                timeLeft *= 5;
                minion = true;
                minionSlots = 1f;
                tileCollide = false;
                ignoreWater = true;
                friendly = true;
            }
            else if (type == 534)
            {
                extraUpdates = 0;
                name = "Yoyo";
                width = 16;
                height = 16;
                aiStyle = 99;
                friendly = true;
                penetrate = -1;
                melee = true;
                scale = 1f;
            }
            else if (type >= 541 && type <= 555)
            {
                extraUpdates = 0;
                name = "Yoyo";
                width = 16;
                height = 16;
                aiStyle = 99;
                friendly = true;
                penetrate = -1;
                melee = true;
                scale = 1f;
                if (type == 547)
                    scale = 1.1f;
                if (type == 554)
                    scale = 1.2f;
                if (type == 555)
                    scale = 1.15f;
                if (type == 551 || type == 550)
                    scale = 1.1f;
            }
            else if (type >= 562 && type <= 564)
            {
                extraUpdates = 0;
                name = "Yoyo";
                width = 16;
                height = 16;
                aiStyle = 99;
                friendly = true;
                penetrate = -1;
                melee = true;
                scale = 1f;
                if (type == 563)
                    scale = 1.05f;
                if (type == 564)
                    scale = 1.075f;
            }
            else if (type == 603)
            {
                extraUpdates = 0;
                name = "Terrarian";
                width = 16;
                height = 16;
                aiStyle = 99;
                friendly = true;
                penetrate = -1;
                melee = true;
                scale = 1.15f;
            }
            else if (type == 604)
            {
                extraUpdates = 0;
                name = "Terrarian";
                width = 16;
                height = 16;
                aiStyle = 115;
                friendly = true;
                penetrate = -1;
                melee = true;
                scale = 1.2f;
            }
            else if (type >= 556 && type <= 561)
            {
                extraUpdates = 0;
                name = "Counterweight";
                width = 10;
                height = 10;
                aiStyle = 99;
                friendly = true;
                penetrate = -1;
                melee = true;
                scale = 1f;
                counterweight = true;
            }
            else if (type == 535)
            {
                name = "Medusa Ray";
                width = 18;
                height = 18;
                aiStyle = 100;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                magic = true;
                ignoreWater = true;
            }
            else if (type == 536)
            {
                name = "Medusa Ray";
                width = 10;
                height = 10;
                aiStyle = 101;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                magic = true;
                ignoreWater = true;
            }
            else if (type == 537)
            {
                name = "Stardust Laser";
                width = 22;
                height = 22;
                aiStyle = 84;
                hostile = true;
                penetrate = -1;
                alpha = 255;
                timeLeft = 240;
                tileCollide = false;
            }
            else if (type == 538)
            {
                name = "Twinkle";
                width = 12;
                height = 12;
                aiStyle = 14;
                hostile = true;
                penetrate = -1;
                timeLeft = 120;
                extraUpdates = 1;
                alpha = 255;
            }
            else if (type == 539)
            {
                name = "Flow Invader";
                width = 18;
                height = 30;
                aiStyle = 102;
                hostile = true;
                penetrate = -1;
                timeLeft = 600;
            }
            else if (type == 540)
            {
                name = "Starmark";
                width = 20;
                height = 20;
                aiStyle = 103;
                hostile = true;
                penetrate = -1;
                timeLeft = 300;
                alpha = 255;
            }
            else if (type == 565)
            {
                name = "Brain of Confusion";
                width = 28;
                height = 28;
                aiStyle = 104;
                penetrate = -1;
                tileCollide = false;
                ignoreWater = true;
                alpha = 255;
                scale = 0.8f;
            }
            else if (type == 566)
            {
                name = "Bee";
                width = 16;
                height = 16;
                aiStyle = 36;
                friendly = true;
                penetrate = 4;
                alpha = 255;
                timeLeft = 660;
                extraUpdates = 3;
            }
            else if (type == 567 || type == 568)
            {
                name = "Spore";
                if (type == 567)
                {
                    width = 14;
                    height = 14;
                }
                else
                {
                    width = 16;
                    height = 16;
                }
                aiStyle = 105;
                friendly = true;
                penetrate = 1;
                alpha = 255;
                timeLeft = 3600;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type >= 569 && type <= 571)
            {
                name = "Spore";
                width = 32;
                height = 32;
                aiStyle = 106;
                friendly = true;
                penetrate = -1;
                alpha = 255;
                timeLeft = 3600;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type == 575)
            {
                name = "Nebula Sphere";
                width = 24;
                height = 24;
                aiStyle = 107;
                hostile = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft = 420;
                alpha = 255;
            }
            else if (type == 573)
            {
                name = "Nebula Piercer";
                width = 18;
                height = 30;
                aiStyle = 102;
                hostile = true;
                penetrate = -1;
                timeLeft = 600;
            }
            else if (type == 574)
            {
                name = "Nebula Eye";
                width = 18;
                height = 18;
                aiStyle = 102;
                hostile = true;
                timeLeft = 600;
                tileCollide = false;
            }
            else if (type == 572)
            {
                name = "Poison Spit";
                width = 10;
                height = 10;
                aiStyle = 1;
                alpha = 255;
                penetrate = -1;
                friendly = false;
                hostile = true;
            }
            else if (type == 576)
            {
                name = "Nebula Laser";
                width = 4;
                height = 4;
                aiStyle = 1;
                hostile = true;
                penetrate = -1;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.2f;
                timeLeft = 600;
            }
            else if (type == 577)
            {
                name = "Vortex Laser";
                width = 4;
                height = 4;
                aiStyle = 1;
                hostile = true;
                penetrate = -1;
                alpha = 255;
                extraUpdates = 2;
                scale = 1.2f;
                timeLeft = 600;
            }
            else if (type == 578 || type == 579)
            {
                name = "Vortex";
                width = 32;
                height = 32;
                aiStyle = 108;
                friendly = true;
                alpha = 255;
                tileCollide = false;
                ignoreWater = true;
                hostile = true;
                hide = true;
            }
            else if (type == 580)
            {
                name = "Vortex Lightning";
                width = 14;
                height = 14;
                aiStyle = 88;
                hostile = true;
                alpha = 255;
                ignoreWater = true;
                tileCollide = true;
                extraUpdates = 4;
                timeLeft = 600;
            }
            else if (type == 581)
            {
                name = "Alien Goop";
                width = 10;
                height = 10;
                aiStyle = 1;
                alpha = 255;
                penetrate = -1;
                friendly = false;
                hostile = true;
            }
            else if (type == 582)
            {
                name = "Mechanic's Wrench";
                width = 20;
                height = 20;
                aiStyle = 109;
                friendly = true;
                penetrate = -1;
                MaxUpdates = 2;
            }
            else if (type == 583)
            {
                name = "Syringe";
                width = 10;
                height = 10;
                aiStyle = 2;
                friendly = true;
                scale = 0.8f;
            }
            else if (type == 589)
            {
                name = "Christmas Ornament";
                width = 10;
                height = 10;
                aiStyle = 2;
                friendly = true;
            }
            else if (type == 584)
            {
                name = "Syringe";
                width = 10;
                height = 10;
                aiStyle = 110;
                friendly = true;
                scale = 0.8f;
                penetrate = 3;
            }
            else if (type == 585)
            {
                name = "Skull";
                width = 26;
                height = 26;
                aiStyle = 1;
                alpha = 255;
                friendly = true;
                penetrate = 3;
            }
            else if (type == 586)
            {
                name = "Dryad's ward";
                width = 26;
                height = 26;
                aiStyle = 111;
                alpha = 255;
                friendly = true;
                penetrate = -1;
            }
            else if (type == 587)
            {
                name = "Paintball";
                width = 10;
                height = 10;
                aiStyle = 1;
                alpha = 255;
                friendly = true;
            }
            else if (type == 588)
            {
                name = "Confetti Grenade";
                width = 14;
                height = 14;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
            }
            else if (type == 590)
            {
                name = "Truffle Spore";
                width = 14;
                height = 14;
                aiStyle = 112;
                friendly = true;
                penetrate = 1;
                alpha = 255;
                timeLeft = 900;
                tileCollide = false;
                ignoreWater = true;
            }
            else if (type == 591)
            {
                name = "Minecart Laser";
                width = 8;
                height = 8;
                aiStyle = 101;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ignoreWater = true;
            }
            else if (type == 592)
            {
                name = "Laser Ray";
                width = 8;
                height = 8;
                aiStyle = 1;
                hostile = true;
                alpha = 255;
                extraUpdates = 3;
                ignoreWater = true;
            }
            else if (type == 593)
            {
                name = "Prophecy's End";
                width = 16;
                height = 16;
                aiStyle = 1;
                hostile = true;
                alpha = 255;
                extraUpdates = 1;
                ignoreWater = true;
            }
            else if (type == 594)
            {
                name = "Blowup Smoke";
                width = 40;
                height = 40;
                aiStyle = 1;
                alpha = 255;
                extraUpdates = 2;
            }
            else if (type == 595)
            {
                name = "Arkhalis";
                width = 68;
                height = 64;
                aiStyle = 75;
                friendly = true;
                tileCollide = false;
                melee = true;
                penetrate = -1;
                ownerHitCheck = true;
            }
            else if (type == 596)
            {
                name = "Desert Spirit's Curse";
                width = 8;
                height = 8;
                aiStyle = 107;
                hostile = true;
                alpha = 255;
                ignoreWater = true;
                timeLeft = 180;
                tileCollide = false;
            }
            else if (type == 597)
            {
                name = "Ember Bolt";
                width = 10;
                height = 10;
                aiStyle = 29;
                alpha = 255;
                magic = true;
                penetrate = 2;
                friendly = true;
            }
            else if (type == 598)
            {
                name = "Bone Javelin";
                width = 16;
                height = 16;
                aiStyle = 113;
                friendly = true;
                melee = true;
                penetrate = -1;
                alpha = 255;
                hide = true;
            }
            else if (type == 599)
            {
                name = "Bone Dagger";
                width = 22;
                height = 22;
                aiStyle = 2;
                friendly = true;
                penetrate = 6;
                thrown = true;
            }
            else if (type == 600)
            {
                name = "Portal Gun";
                width = 14;
                height = 14;
                aiStyle = 75;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ignoreWater = true;
            }
            else if (type == 601)
            {
                name = "Portal Bolt";
                width = 10;
                height = 10;
                aiStyle = 1;
                alpha = 255;
                friendly = true;
                extraUpdates = 7;
            }
            else if (type == 602)
            {
                name = "Portal Gate";
                width = 10;
                height = 10;
                aiStyle = 114;
                alpha = 255;
                friendly = true;
                tileCollide = false;
            }
            else if (type == 605)
            {
                name = "Slime Spike";
                alpha = 255;
                width = 6;
                height = 6;
                aiStyle = 1;
                hostile = true;
                penetrate = -1;
            }
            else if (type == 606)
            {
                name = "Laser";
                width = 5;
                height = 5;
                aiStyle = 1;
                friendly = true;
                alpha = 255;
                extraUpdates = 2;
                scale = 1f;
                timeLeft = 600;
                ignoreWater = true;
            }
            else if (type == 607)
            {
                name = "Solar Flare";
                width = 10;
                height = 10;
                aiStyle = 116;
                friendly = true;
                alpha = 255;
                timeLeft = 600;
                ignoreWater = true;
                tileCollide = false;
                penetrate = -1;
            }
            else if (type == 608)
            {
                name = "Solar Radiance";
                width = 160;
                height = 160;
                aiStyle = 117;
                friendly = true;
                alpha = 255;
                timeLeft = 3;
                ignoreWater = true;
                tileCollide = false;
                penetrate = -1;
                hide = true;
            }
            else if (type == 611)
            {
                name = "Solar Eruption";
                width = 16;
                height = 16;
                aiStyle = 75;
                friendly = true;
                melee = true;
                penetrate = -1;
                alpha = 255;
                hide = true;
                tileCollide = false;
                ignoreWater = true;
                updatedNPCImmunity = true;
            }
            else if (type == 612)
            {
                name = "Solar Eruption";
                width = 8;
                height = 8;
                aiStyle = 117;
                friendly = true;
                alpha = 255;
                ignoreWater = true;
                timeLeft = 60;
                tileCollide = false;
                penetrate = -1;
                updatedNPCImmunity = true;
            }
            else if (type == 613)
            {
                netImportant = true;
                name = "Stardust Cell";
                width = 24;
                height = 24;
                aiStyle = 62;
                penetrate = -1;
                timeLeft *= 5;
                minion = true;
                friendly = true;
                minionSlots = 1f;
                ignoreWater = true;
            }
            else if (type == 614)
            {
                name = "Stardust Cell";
                width = 16;
                height = 16;
                aiStyle = 113;
                friendly = true;
                penetrate = -1;
                alpha = 255;
            }
            else if (type == 615)
            {
                name = "Vortex Beater";
                width = 22;
                height = 22;
                aiStyle = 75;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ranged = true;
                ignoreWater = true;
            }
            else if (type == 616)
            {
                name = "Vortex Rocket";
                width = 14;
                height = 14;
                aiStyle = 1;
                friendly = true;
                penetrate = 1;
                alpha = 255;
                ranged = true;
                extraUpdates = 2;
                timeLeft = 90 * MaxUpdates;
            }
            else if (type == 617)
            {
                name = "Nebula Arcanum";
                width = 32;
                height = 32;
                aiStyle = 118;
                friendly = true;
                alpha = 255;
                ignoreWater = true;
                hide = true;
                magic = true;
                penetrate = 3;
            }
            else if (type == 618)
            {
                name = "Nebula Arcanum";
                tileCollide = false;
                width = 18;
                height = 30;
                aiStyle = 119;
                penetrate = -1;
                timeLeft = 420;
                magic = true;
                friendly = true;
            }
            else if (type == 619)
            {
                name = "Nebula Arcanum";
                width = 14;
                height = 14;
                aiStyle = 29;
                alpha = 255;
                magic = true;
                penetrate = 1;
                friendly = true;
            }
            else if (type == 620)
            {
                name = "Nebula Arcanum";
                width = 8;
                height = 8;
                aiStyle = 29;
                alpha = 255;
                magic = true;
                penetrate = 1;
                friendly = true;
            }
            else if (type == 622)
            {
                name = "Blowup Smoke";
                width = 10;
                height = 10;
                aiStyle = 1;
                alpha = 255;
                extraUpdates = 2;
            }
            else if (type == 623)
            {
                netImportant = true;
                name = "Stardust Guardian";
                width = 50;
                height = 80;
                aiStyle = 120;
                penetrate = -1;
                timeLeft *= 5;
                minion = true;
                friendly = true;
                minionSlots = 0.0f;
                ignoreWater = true;
                tileCollide = false;
            }
            else if (type == 624)
            {
                name = "Starburst";
                width = 8;
                height = 8;
                aiStyle = 117;
                friendly = true;
                alpha = 255;
                ignoreWater = true;
                timeLeft = 60;
                tileCollide = false;
                penetrate = -1;
            }
            else if (type >= 625 && type <= 628)
            {
                if (type == 625 || type == 628)
                    netImportant = true;
                if (type == 626 || type == 627)
                    minionSlots = 0.5f;
                name = "Stardust Dragon";
                width = 24;
                height = 24;
                aiStyle = 121;
                penetrate = -1;
                timeLeft *= 5;
                minion = true;
                friendly = true;
                ignoreWater = true;
                tileCollide = false;
                alpha = 255;
                hide = true;
            }
            else if (type == 629)
            {
                name = "Released Energy";
                width = 8;
                height = 8;
                aiStyle = 122;
                hostile = true;
                alpha = 255;
                ignoreWater = true;
                timeLeft = 3600;
                tileCollide = false;
                penetrate = -1;
                extraUpdates = 2;
            }
            else if (type == 630)
            {
                name = "Phantasm";
                width = 22;
                height = 22;
                aiStyle = 75;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                hide = true;
                ranged = true;
                ignoreWater = true;
            }
            else if (type == 631)
            {
                arrow = true;
                name = "Phantasm";
                width = 10;
                height = 10;
                aiStyle = 122;
                friendly = true;
                ranged = true;
                tileCollide = false;
                alpha = 255;
                ignoreWater = true;
                extraUpdates = 1;
            }
            else if (type == 633)
            {
                name = "Last Prism";
                width = 14;
                height = 18;
                aiStyle = 75;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                magic = true;
                ignoreWater = true;
            }
            else if (type == 632)
            {
                name = "Last Prism";
                width = 18;
                height = 18;
                aiStyle = 84;
                friendly = true;
                magic = true;
                penetrate = -1;
                alpha = 255;
                tileCollide = false;
            }
            else if (type == 634)
            {
                name = "Nebula Blaze";
                width = 40;
                height = 40;
                aiStyle = 1;
                friendly = true;
                alpha = 255;
                ignoreWater = true;
                extraUpdates = 2;
                magic = true;
            }
            else if (type == 635)
            {
                name = "Nebula Blaze Ex";
                width = 40;
                height = 40;
                aiStyle = 1;
                friendly = true;
                alpha = 255;
                friendly = true;
                extraUpdates = 3;
                magic = true;
            }
            else if (type == 636)
            {
                name = "Daybreak";
                width = 16;
                height = 16;
                aiStyle = 113;
                friendly = true;
                melee = true;
                penetrate = -1;
                alpha = 255;
                hide = true;
                MaxUpdates = 2;
            }
            else if (type == 637)
            {
                name = "Bouncy Dynamite";
                width = 10;
                height = 10;
                aiStyle = 16;
                friendly = true;
                penetrate = -1;
            }
            else if (type == 638)
            {
                name = "Luminite Bullet";
                width = 4;
                height = 4;
                aiStyle = 1;
                friendly = true;
                alpha = 255;
                extraUpdates = 5;
                timeLeft = 600;
                ranged = true;
                ignoreWater = true;
                updatedNPCImmunity = true;
                penetrate = -1;
            }
            else if (type == 639)
            {
                arrow = true;
                name = "Luminite Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
                MaxUpdates = 2;
                timeLeft = MaxUpdates * 45;
                ignoreWater = true;
                updatedNPCImmunity = true;
                alpha = 255;
                penetrate = 4;
            }
            else if (type == 640)
            {
                name = "Luminite Arrow";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                ranged = true;
                MaxUpdates = 3;
                timeLeft = 90;
                ignoreWater = true;
                updatedNPCImmunity = true;
                alpha = 255;
                penetrate = 4;
            }
            else if (type == 642)
            {
                name = "Lunar Portal Laser";
                width = 18;
                height = 18;
                aiStyle = 84;
                friendly = true;
                penetrate = -1;
                alpha = 255;
                tileCollide = false;
                updatedNPCImmunity = true;
            }
            else if (type == 641)
            {
                name = "Lunar Portal";
                width = 32;
                height = 32;
                aiStyle = 123;
                timeLeft = 7200;
                ignoreWater = true;
                tileCollide = false;
                alpha = 255;
                hide = true;
            }
            else if (type == 643)
            {
                name = "Rainbow Crystal";
                width = 32;
                height = 32;
                aiStyle = 123;
                timeLeft = 7200;
                ignoreWater = true;
                tileCollide = false;
                alpha = 255;
            }
            else if (type == 644)
            {
                name = "Rainbow Explosion";
                width = 14;
                height = 14;
                aiStyle = 112;
                penetrate = 1;
                timeLeft = 900;
                tileCollide = false;
                ignoreWater = true;
                alpha = 255;
            }
            else if (type == 645)
            {
                name = "Lunar Flare";
                width = 10;
                height = 10;
                aiStyle = 1;
                friendly = true;
                magic = true;
                tileCollide = false;
                extraUpdates = 5;
                penetrate = -1;
                updatedNPCImmunity = true;
            }
            else if (type >= 646 && type <= 649)
            {
                name = "Lunar Hook";
                width = 18;
                height = 18;
                aiStyle = 7;
                friendly = true;
                penetrate = -1;
                tileCollide = false;
                timeLeft *= 10;
            }
            else if (type == 650)
            {
                name = "Suspicious Looking Tentacle";
                width = 20;
                height = 20;
                aiStyle = 124;
                penetrate = -1;
                netImportant = true;
                timeLeft *= 5;
                friendly = true;
                ignoreWater = true;
                tileCollide = false;
                manualDirectionChange = true;
            }
            else
                active = false;
            width = (int)(width * scale);
            height = (int)(height * scale);
            maxPenetrate = penetrate;
        }

        public static int GetNextSlot()
        {
            int num = 1000;
            for (int index = 0; index < 1000; ++index)
            {
                if (!Main.projectile[index].active)
                {
                    num = index;
                    break;
                }
            }
            return num;
        }

        public static int NewProjectile(float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner = 255, float ai0 = 0.0f, float ai1 = 0.0f)
        {
            int number = 1000;
            for (int index = 0; index < 1000; ++index)
            {
                if (!Main.projectile[index].active)
                {
                    number = index;
                    break;
                }
            }
            if (number == 1000)
                return number;
            Projectile projectile = Main.projectile[number];
            projectile.SetDefaults(Type);
            projectile.position.X = X - projectile.width * 0.5f;
            projectile.position.Y = Y - projectile.height * 0.5f;
            projectile.owner = Owner;
            projectile.velocity.X = SpeedX;
            projectile.velocity.Y = SpeedY;
            projectile.damage = Damage;
            projectile.knockBack = KnockBack;
            projectile.identity = number;
            projectile.gfxOffY = 0.0f;
            projectile.stepSpeed = 1f;
            projectile.wet = Collision.WetCollision(projectile.position, projectile.width, projectile.height);
            if (projectile.ignoreWater)
                projectile.wet = false;
            projectile.honeyWet = Collision.honey;
            if (projectile.aiStyle == 1)
            {
                while (projectile.velocity.X >= 16.0 || projectile.velocity.X <= -16.0 || (projectile.velocity.Y >= 16.0 || projectile.velocity.Y < -16.0))
                {
                    projectile.velocity.X *= 0.97f;
                    projectile.velocity.Y *= 0.97f;
                }
            }
            if (Owner == Main.myPlayer)
            {
                if (Type == 206)
                {
                    projectile.ai[0] = Main.rand.Next(-100, 101) * 0.0005f;
                    projectile.ai[1] = Main.rand.Next(-100, 101) * 0.0005f;
                }
                else if (Type == 335)
                    projectile.ai[1] = Main.rand.Next(4);
                else if (Type == 358)
                    projectile.ai[1] = Main.rand.Next(10, 31) * 0.1f;
                else if (Type == 406)
                {
                    projectile.ai[1] = Main.rand.Next(10, 21) * 0.1f;
                }
                else
                {
                    projectile.ai[0] = ai0;
                    projectile.ai[1] = ai1;
                }
            }
            if (Type == 434)
            {
                projectile.ai[0] = projectile.position.X;
                projectile.ai[1] = projectile.position.Y;
            }
            if (Main.netMode != 0 && Owner == Main.myPlayer)
                NetMessage.SendData(27, -1, -1, "", number, 0.0f, 0.0f, 0.0f, 0, 0, 0);
            if (Owner == Main.myPlayer)
            {
                if (Type == 28)
                    projectile.timeLeft = 180;
                if (Type == 516)
                    projectile.timeLeft = 180;
                if (Type == 519)
                    projectile.timeLeft = 180;
                if (Type == 29)
                    projectile.timeLeft = 300;
                if (Type == 470)
                    projectile.timeLeft = 300;
                if (Type == 637)
                    projectile.timeLeft = 300;
                if (Type == 30)
                    projectile.timeLeft = 180;
                if (Type == 517)
                    projectile.timeLeft = 180;
                if (Type == 37)
                    projectile.timeLeft = 180;
                if (Type == 75)
                    projectile.timeLeft = 180;
                if (Type == 133)
                    projectile.timeLeft = 180;
                if (Type == 136)
                    projectile.timeLeft = 180;
                if (Type == 139)
                    projectile.timeLeft = 180;
                if (Type == 142)
                    projectile.timeLeft = 180;
                if (Type == 397)
                    projectile.timeLeft = 180;
                if (Type == 419)
                    projectile.timeLeft = 600;
                if (Type == 420)
                    projectile.timeLeft = 600;
                if (Type == 421)
                    projectile.timeLeft = 600;
                if (Type == 422)
                    projectile.timeLeft = 600;
                if (Type == 588)
                    projectile.timeLeft = 180;
                if (Type == 443)
                    projectile.timeLeft = 300;
            }
            if (Type == 249)
                projectile.frame = Main.rand.Next(5);
            return number;
        }

        public void StatusNPC(int i)
        {
            if (melee && Main.player[owner].meleeEnchant > 0 && !noEnchantments)
            {
                int num = Main.player[owner].meleeEnchant;
                if (num == 1)
                    Main.npc[i].AddBuff(70, 60 * Main.rand.Next(5, 10), false);
                if (num == 2)
                    Main.npc[i].AddBuff(39, 60 * Main.rand.Next(3, 7), false);
                if (num == 3)
                    Main.npc[i].AddBuff(24, 60 * Main.rand.Next(3, 7), false);
                if (num == 5)
                    Main.npc[i].AddBuff(69, 60 * Main.rand.Next(10, 20), false);
                if (num == 6)
                    Main.npc[i].AddBuff(31, 60 * Main.rand.Next(1, 4), false);
                if (num == 8)
                    Main.npc[i].AddBuff(20, 60 * Main.rand.Next(5, 10), false);
                if (num == 4)
                    Main.npc[i].AddBuff(72, 120, false);
            }
            if (type == 195)
            {
                if (Main.rand.Next(3) == 0)
                    Main.npc[i].AddBuff(70, 60 * Main.rand.Next(10, 21), false);
                else
                    Main.npc[i].AddBuff(20, 60 * Main.rand.Next(10, 21), false);
            }
            if (type == 567 || type == 568)
                Main.npc[i].AddBuff(20, 60 * Main.rand.Next(5, 11), false);
            if (type == 598)
                Main.npc[i].AddBuff(169, 900, false);
            if (type == 636)
                Main.npc[i].AddBuff(189, 300, false);
            if (type == 611)
                Main.npc[i].AddBuff(189, 300, false);
            if (type == 612)
                Main.npc[i].AddBuff(189, 300, false);
            if (type == 614)
                Main.npc[i].AddBuff(183, 900, false);
            if (type == 585)
                Main.npc[i].AddBuff(153, 60 * Main.rand.Next(5, 11), false);
            if (type == 583)
                Main.npc[i].AddBuff(20, 60 * Main.rand.Next(3, 6), false);
            if (type == 524)
                Main.npc[i].AddBuff(69, 60 * Main.rand.Next(3, 8), false);
            if (type == 504 && Main.rand.Next(3) == 0)
            {
                if (Main.rand.Next(3) == 0)
                    Main.npc[i].AddBuff(24, Main.rand.Next(60, 180), false);
                else
                    Main.npc[i].AddBuff(24, Main.rand.Next(30, 120), false);
            }
            if (type == 545 && Main.rand.Next(3) == 0)
                Main.npc[i].AddBuff(24, Main.rand.Next(60, 240), false);
            if (type == 553)
                Main.npc[i].AddBuff(24, Main.rand.Next(180, 480), false);
            if (type == 552 && Main.rand.Next(3) != 0)
                Main.npc[i].AddBuff(44, Main.rand.Next(120, 320), false);
            if (type == 495)
                Main.npc[i].AddBuff(153, Main.rand.Next(120, 300), false);
            if (type == 497)
                Main.npc[i].AddBuff(153, Main.rand.Next(60, 180), false);
            if (type == 496)
                Main.npc[i].AddBuff(153, Main.rand.Next(240, 480), false);
            if (type == 476)
                Main.npc[i].AddBuff(151, 30, false);
            if (type == 523)
                Main.npc[i].AddBuff(20, 60 * Main.rand.Next(10, 30), false);
            if (type == 478 || type == 480)
                Main.npc[i].AddBuff(39, 60 * Main.rand.Next(3, 7), false);
            if (type == 479)
                Main.npc[i].AddBuff(69, 60 * Main.rand.Next(7, 15), false);
            if (type == 379)
                Main.npc[i].AddBuff(70, 60 * Main.rand.Next(4, 7), false);
            if (type >= 390 && type <= 392)
                Main.npc[i].AddBuff(70, 60 * Main.rand.Next(2, 5), false);
            if (type == 374)
                Main.npc[i].AddBuff(20, 60 * Main.rand.Next(4, 7), false);
            if (type == 376)
                Main.npc[i].AddBuff(24, 60 * Main.rand.Next(3, 7), false);
            if (type >= 399 && type <= 402)
                Main.npc[i].AddBuff(24, 60 * Main.rand.Next(3, 7), false);
            if (type == 295 || type == 296)
                Main.npc[i].AddBuff(24, 60 * Main.rand.Next(8, 16), false);
            if ((melee || ranged) && (Main.player[owner].frostBurn && !noEnchantments))
                Main.npc[i].AddBuff(44, 60 * Main.rand.Next(5, 15), false);
            if (melee && Main.player[owner].magmaStone && !noEnchantments)
            {
                if (Main.rand.Next(7) == 0)
                    Main.npc[i].AddBuff(24, 360, false);
                else if (Main.rand.Next(3) == 0)
                    Main.npc[i].AddBuff(24, 120, false);
                else
                    Main.npc[i].AddBuff(24, 60, false);
            }
            if (type == 287)
                Main.npc[i].AddBuff(72, 120, false);
            if (type == 285)
            {
                if (Main.rand.Next(3) == 0)
                    Main.npc[i].AddBuff(31, 180, false);
                else
                    Main.npc[i].AddBuff(31, 60, false);
            }
            if (type == 2 && Main.rand.Next(3) == 0)
                Main.npc[i].AddBuff(24, 180, false);
            if (type == 172)
            {
                if (Main.rand.Next(3) == 0)
                    Main.npc[i].AddBuff(44, 180, false);
            }
            else if (type == 15)
            {
                if (Main.rand.Next(2) == 0)
                    Main.npc[i].AddBuff(24, 300, false);
            }
            else if (type == 253)
            {
                if (Main.rand.Next(2) == 0)
                    Main.npc[i].AddBuff(44, 480, false);
            }
            else if (type == 19)
            {
                if (Main.rand.Next(5) == 0)
                    Main.npc[i].AddBuff(24, 180, false);
            }
            else if (type == 33)
            {
                if (Main.rand.Next(5) == 0)
                    Main.npc[i].AddBuff(20, 420, false);
            }
            else if (type == 34)
            {
                if (Main.rand.Next(2) == 0)
                    Main.npc[i].AddBuff(24, Main.rand.Next(240, 480), false);
            }
            else if (type == 35)
            {
                if (Main.rand.Next(4) == 0)
                    Main.npc[i].AddBuff(24, 180, false);
            }
            else if (type == 54)
            {
                if (Main.rand.Next(2) == 0)
                    Main.npc[i].AddBuff(20, 600, false);
            }
            else if (type == 267)
            {
                if (Main.rand.Next(3) == 0)
                    Main.npc[i].AddBuff(20, 3600, false);
                else
                    Main.npc[i].AddBuff(20, 1800, false);
            }
            else if (type == 63)
            {
                if (Main.rand.Next(5) != 0)
                    Main.npc[i].AddBuff(31, 60 * Main.rand.Next(2, 5), false);
            }
            else if (type == 85 || type == 188)
                Main.npc[i].AddBuff(24, 1200, false);
            else if (type == 95 || type == 103 || type == 104)
                Main.npc[i].AddBuff(39, 420, false);
            else if (type == 278 || type == 279 || type == 280)
                Main.npc[i].AddBuff(69, 600, false);
            else if (type == 282 || type == 283)
                Main.npc[i].AddBuff(70, 600, false);
            if (type == 163 || type == 310)
            {
                if (Main.rand.Next(3) == 0)
                    Main.npc[i].AddBuff(24, 600, false);
                else
                    Main.npc[i].AddBuff(24, 300, false);
            }
            else if (type == 98)
                Main.npc[i].AddBuff(20, 600, false);
            else if (type == 184)
                Main.npc[i].AddBuff(20, 900, false);
            else if (type == 265)
            {
                Main.npc[i].AddBuff(20, 1800, false);
            }
            else
            {
                if (type != 355)
                    return;
                Main.npc[i].AddBuff(70, 1800, false);
            }
        }

        public void StatusPvP(int i)
        {
            if (melee && Main.player[owner].meleeEnchant > 0 && !noEnchantments)
            {
                int num = Main.player[owner].meleeEnchant;
                if (num == 1)
                    Main.player[i].AddBuff(70, 60 * Main.rand.Next(5, 10), true);
                if (num == 2)
                    Main.player[i].AddBuff(39, 60 * Main.rand.Next(3, 7), true);
                if (num == 3)
                    Main.player[i].AddBuff(24, 60 * Main.rand.Next(3, 7), true);
                if (num == 5)
                    Main.player[i].AddBuff(69, 60 * Main.rand.Next(10, 20), true);
                if (num == 6)
                    Main.player[i].AddBuff(31, 60 * Main.rand.Next(1, 4), true);
                if (num == 8)
                    Main.player[i].AddBuff(20, 60 * Main.rand.Next(5, 10), true);
            }
            if (type == 295 || type == 296)
                Main.player[i].AddBuff(24, 60 * Main.rand.Next(8, 16), true);
            if (type == 478 || type == 480)
                Main.player[i].AddBuff(39, 60 * Main.rand.Next(3, 7), true);
            if ((melee || ranged) && (Main.player[owner].frostBurn && !noEnchantments))
                Main.player[i].AddBuff(44, 60 * Main.rand.Next(1, 8), false);
            if (melee && Main.player[owner].magmaStone && !noEnchantments)
            {
                if (Main.rand.Next(4) == 0)
                    Main.player[i].AddBuff(24, 360, true);
                else if (Main.rand.Next(2) == 0)
                    Main.player[i].AddBuff(24, 240, true);
                else
                    Main.player[i].AddBuff(24, 120, true);
            }
            if (type == 2 && Main.rand.Next(3) == 0)
                Main.player[i].AddBuff(24, 180, false);
            if (type == 172)
            {
                if (Main.rand.Next(3) == 0)
                    Main.player[i].AddBuff(44, 240, false);
            }
            else if (type == 15)
            {
                if (Main.rand.Next(2) == 0)
                    Main.player[i].AddBuff(24, 300, false);
            }
            else if (type == 253)
            {
                if (Main.rand.Next(2) == 0)
                    Main.player[i].AddBuff(44, 480, false);
            }
            else if (type == 19)
            {
                if (Main.rand.Next(5) == 0)
                    Main.player[i].AddBuff(24, 180, false);
            }
            else if (type == 33)
            {
                if (Main.rand.Next(5) == 0)
                    Main.player[i].AddBuff(20, 420, false);
            }
            else if (type == 34)
            {
                if (Main.rand.Next(2) == 0)
                    Main.player[i].AddBuff(24, 240, false);
            }
            else if (type == 35)
            {
                if (Main.rand.Next(4) == 0)
                    Main.player[i].AddBuff(24, 180, false);
            }
            else if (type == 54)
            {
                if (Main.rand.Next(2) == 0)
                    Main.player[i].AddBuff(20, 600, false);
            }
            else if (type == 267)
            {
                if (Main.rand.Next(3) == 0)
                    Main.player[i].AddBuff(20, 3600, true);
                else
                    Main.player[i].AddBuff(20, 1800, true);
            }
            else if (type == 63)
            {
                if (Main.rand.Next(3) != 0)
                    Main.player[i].AddBuff(31, 120, true);
            }
            else if (type == 85 || type == 188)
                Main.player[i].AddBuff(24, 1200, false);
            else if (type == 95 || type == 103 || type == 104)
                Main.player[i].AddBuff(39, 420, true);
            else if (type == 278 || type == 279 || type == 280)
                Main.player[i].AddBuff(69, 900, true);
            else if (type == 282 || type == 283)
                Main.player[i].AddBuff(70, 600, true);
            if (type == 163 || type == 310)
            {
                if (Main.rand.Next(3) == 0)
                    Main.player[i].AddBuff(24, 600, true);
                else
                    Main.player[i].AddBuff(24, 300, true);
            }
            else if (type == 265)
            {
                Main.player[i].AddBuff(20, 1200, true);
            }
            else
            {
                if (type != 355)
                    return;
                Main.player[i].AddBuff(70, 1800, true);
            }
        }

        public void ghostHurt(int dmg, Vector2 Position)
        {
            if (!magic)
                return;
            int Damage = damage / 2;
            if (dmg / 2 <= 1)
                return;
            int num1 = 1000;
            if (Main.player[Main.myPlayer].ghostDmg > num1)
                return;
            Main.player[Main.myPlayer].ghostDmg += Damage;
            int[] numArray = new int[200];
            int maxValue1 = 0;
            int maxValue2 = 0;
            for (int index = 0; index < 200; ++index)
            {
                if (Main.npc[index].CanBeChasedBy(this, false))
                {
                    float num2 = Math.Abs(Main.npc[index].position.X + (Main.npc[index].width / 2) - position.X + (width / 2)) + Math.Abs(Main.npc[index].position.Y + (Main.npc[index].height / 2) - position.Y + (height / 2));
                    if (num2 < 800.0)
                    {
                        if (Collision.CanHit(position, 1, 1, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height) && num2 > 50.0)
                        {
                            numArray[maxValue2] = index;
                            ++maxValue2;
                        }
                        else if (maxValue2 == 0)
                        {
                            numArray[maxValue1] = index;
                            ++maxValue1;
                        }
                    }
                }
            }
            if (maxValue1 == 0 && maxValue2 == 0)
                return;
            int ai0 = maxValue2 <= 0 ? numArray[Main.rand.Next(maxValue1)] : numArray[Main.rand.Next(maxValue2)];
            float num4 = 4f;
            float num5 = Main.rand.Next(-100, 101);
            float num6 = Main.rand.Next(-100, 101);
            float num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
            float num8 = num4 / num7;
            float SpeedX = num5 * num8;
            float SpeedY = num6 * num8;
            NewProjectile(Position.X, Position.Y, SpeedX, SpeedY, 356, Damage, 0.0f, owner, ai0, 0.0f);
        }

        public void ghostHeal(int dmg, Vector2 Position)
        {
            float num1 = 0.2f - numHits * 0.05f;
            if (num1 <= 0.0)
                return;
            float ai1 = dmg * num1;
            if ((int)ai1 <= 0 || Main.player[Main.myPlayer].lifeSteal <= 0.0)
                return;
            Main.player[Main.myPlayer].lifeSteal -= ai1;
            if (!magic)
                return;
            float num2 = 0.0f;
            int ai0 = owner;
            for (int index = 0; index < 255; ++index)
            {
                if (Main.player[index].active && !Main.player[index].dead && (!Main.player[owner].hostile && !Main.player[index].hostile || Main.player[owner].team == Main.player[index].team) && ((Math.Abs(Main.player[index].position.X + (Main.player[index].width / 2) - position.X + (width / 2)) + Math.Abs(Main.player[index].position.Y + (Main.player[index].height / 2) - position.Y + (height / 2))) < 1200.0 && (Main.player[index].statLifeMax2 - Main.player[index].statLife) > num2))
                {
                    num2 = (Main.player[index].statLifeMax2 - Main.player[index].statLife);
                    ai0 = index;
                }
            }
            NewProjectile(Position.X, Position.Y, 0.0f, 0.0f, 298, 0, 0.0f, owner, ai0, ai1);
        }

        public void vampireHeal(int dmg, Vector2 Position)
        {
            float ai1 = dmg * 0.075f;
            if ((int)ai1 == 0 || Main.player[Main.myPlayer].lifeSteal <= 0.0)
                return;
            Main.player[Main.myPlayer].lifeSteal -= ai1;
            int ai0 = owner;
            NewProjectile(Position.X, Position.Y, 0.0f, 0.0f, 305, 0, 0.0f, owner, ai0, ai1);
        }

        public void StatusPlayer(int i)
        {
            if (type == 472)
                Main.player[i].AddBuff(149, Main.rand.Next(30, 150), true);
            if (type == 467)
                Main.player[i].AddBuff(24, Main.rand.Next(30, 150), true);
            if (type == 581)
            {
                if (Main.expertMode)
                    Main.player[i].AddBuff(164, Main.rand.Next(300, 540), true);
                else if (Main.rand.Next(2) == 0)
                    Main.player[i].AddBuff(164, Main.rand.Next(360, 720), true);
            }
            if (type == 572 && Main.rand.Next(3) != 0)
                Main.player[i].AddBuff(20, Main.rand.Next(120, 240), true);
            if (type == 276)
            {
                if (Main.expertMode)
                    Main.player[i].AddBuff(20, Main.rand.Next(120, 540), true);
                else if (Main.rand.Next(2) == 0)
                    Main.player[i].AddBuff(20, Main.rand.Next(180, 420), true);
            }
            if (type == 436 && Main.rand.Next(5) >= 2)
                Main.player[i].AddBuff(31, 300, true);
            if (type == 435 && Main.rand.Next(3) != 0)
                Main.player[i].AddBuff(144, 300, true);
            if (type == 437)
                Main.player[i].AddBuff(144, 60 * Main.rand.Next(4, 9), true);
            if (type == 348)
            {
                if (Main.rand.Next(2) == 0)
                    Main.player[i].AddBuff(46, 600, true);
                else
                    Main.player[i].AddBuff(46, 300, true);
                if (Main.rand.Next(3) != 0)
                {
                    if (Main.rand.Next(16) == 0)
                        Main.player[i].AddBuff(47, 60, true);
                    else if (Main.rand.Next(12) == 0)
                        Main.player[i].AddBuff(47, 40, true);
                    else if (Main.rand.Next(8) == 0)
                        Main.player[i].AddBuff(47, 20, true);
                }
            }
            if (type == 349)
            {
                if (Main.rand.Next(3) == 0)
                    Main.player[i].AddBuff(46, 600, true);
                else if (Main.rand.Next(2) == 0)
                    Main.player[i].AddBuff(46, 300, true);
            }
            if (type >= 399 && type <= 402)
                Main.npc[i].AddBuff(24, 60 * Main.rand.Next(3, 7), false);
            if (type == 55)
            {
                if (Main.rand.Next(3) == 0)
                    Main.player[i].AddBuff(20, 600, true);
                else if (Main.expertMode)
                    Main.player[i].AddBuff(20, Main.rand.Next(60, 300), true);
            }
            if (type == 44 && Main.rand.Next(3) == 0)
                Main.player[i].AddBuff(22, 900, true);
            if (type == 293)
                Main.player[i].AddBuff(80, 60 * Main.rand.Next(2, 7), true);
            if (type == 82 && Main.rand.Next(3) == 0)
                Main.player[i].AddBuff(24, 420, true);
            if (type == 285)
            {
                if (Main.rand.Next(3) == 0)
                    Main.player[i].AddBuff(31, 180, true);
                else
                    Main.player[i].AddBuff(31, 60, true);
            }
            if (type == 96 || type == 101)
            {
                if (Main.rand.Next(6) == 0)
                    Main.player[i].AddBuff(39, 480, true);
                else if (Main.rand.Next(4) == 0)
                    Main.player[i].AddBuff(39, 300, true);
                else if (Main.rand.Next(2) == 0)
                    Main.player[i].AddBuff(39, 180, true);
            }
            else if (type == 288)
                Main.player[i].AddBuff(69, 900, true);
            else if (type == 253 && Main.rand.Next(2) == 0)
                Main.player[i].AddBuff(44, 600, true);
            if (type == 291 || type == 292)
                Main.player[i].AddBuff(24, 60 * Main.rand.Next(8, 16), true);
            if (type == 98)
                Main.player[i].AddBuff(20, 600, true);
            if (type == 184)
                Main.player[i].AddBuff(20, 900, true);
            if (type == 290)
                Main.player[i].AddBuff(32, 60 * Main.rand.Next(5, 16), true);
            if (type == 174)
            {
                Main.player[i].AddBuff(46, 1200, true);
                if (!Main.player[i].frozen && Main.rand.Next(20) == 0)
                    Main.player[i].AddBuff(47, 90, true);
                else if (!Main.player[i].frozen && Main.expertMode && Main.rand.Next(20) == 0)
                    Main.player[i].AddBuff(47, 60, true);
            }
            if (type == 257)
            {
                Main.player[i].AddBuff(46, 2700, true);
                if (!Main.player[i].frozen && Main.rand.Next(5) == 0)
                    Main.player[i].AddBuff(47, 60, true);
            }
            if (type == 177)
            {
                Main.player[i].AddBuff(46, 1500, true);
                if (!Main.player[i].frozen && Main.rand.Next(10) == 0)
                    Main.player[i].AddBuff(47, Main.rand.Next(30, 120), true);
            }
            if (type != 176)
                return;
            if (Main.rand.Next(4) == 0)
            {
                Main.player[i].AddBuff(20, 1200, true);
            }
            else
            {
                if (Main.rand.Next(2) != 0)
                    return;
                Main.player[i].AddBuff(20, 300, true);
            }
        }

        public void Damage()
        {
            if (type == 18 || type == 72 || (type == 86 || type == 87) || (aiStyle == 31 || aiStyle == 32 || (type == 226 || type == 378)) || (type == 613 || type == 650 || type == 434 && localAI[0] != 0.0 || (type == 439 || type == 444)) || (type == 451 && ((int)(ai[0] - 1.0) / penetrate == 0 || ai[1] < 5.0) && ai[0] != 0.0 || (type == 500 || type == 460 || (type == 633 || type == 600) || (type == 601 || type == 602 || type == 535))) || (type == 631 && localAI[1] == 0.0 || aiStyle == 93 && ai[0] != 0.0 && ai[0] != 2.0 || aiStyle == 10 && localAI[1] == -1.0) || Main.projPet[type] && type != 266 && (type != 407 && type != 317) && ((type != 388 || ai[0] != 2.0) && (type < 390 || type > 392)) && ((type < 393 || type > 395) && (type != 533 || ai[0] < 6.0 || ai[0] > 8.0)) && (type < 625 || type > 628))
                return;
            Rectangle myRect = new Rectangle((int)position.X, (int)position.Y, width, height);
            if (type == 85 || type == 101)
            {
                int num = 30;
                myRect.X -= num;
                myRect.Y -= num;
                myRect.Width += num * 2;
                myRect.Height += num * 2;
            }
            if (type == 188)
            {
                int num = 20;
                myRect.X -= num;
                myRect.Y -= num;
                myRect.Width += num * 2;
                myRect.Height += num * 2;
            }
            if (aiStyle == 29)
            {
                int num = 4;
                myRect.X -= num;
                myRect.Y -= num;
                myRect.Width += num * 2;
                myRect.Height += num * 2;
            }
            if (friendly && owner == Main.myPlayer && !npcProj)
            {
                if (aiStyle == 16 && type != 338 && (type != 339 && type != 340) && type != 341 && (timeLeft <= 1 || type == 108 || type == 164) || type == 286 && localAI[1] == -1.0)
                {
                    int index = Main.myPlayer;
                    if (Main.player[index].active && !Main.player[index].dead && !Main.player[index].immune && (!ownerHitCheck || Collision.CanHit(Main.player[owner].position, Main.player[owner].width, Main.player[owner].height, Main.player[index].position, Main.player[index].width, Main.player[index].height)))
                    {
                        Rectangle rectangle = new Rectangle((int)Main.player[index].position.X, (int)Main.player[index].position.Y, Main.player[index].width, Main.player[index].height);
                        if (myRect.Intersects(rectangle))
                        {
                            if (Main.player[index].position.X + (Main.player[index].width / 2) < position.X + (width / 2))
                                direction = -1;
                            else
                                direction = 1;
                            int Damage = Main.DamageVar(damage);
                            StatusPlayer(index);
                            Main.player[index].Hurt(Damage, direction, true, false, Lang.deathMsg(owner, -1, whoAmI, -1), false);
                            if (trap)
                            {
                                Main.player[index].trapDebuffSource = true;
                                if (Main.player[index].dead)
                                    AchievementsHelper.HandleSpecialEvent(Main.player[index], 4);
                            }
                            if (Main.netMode != 0)
                                NetMessage.SendData(26, -1, -1, Lang.deathMsg(owner, -1, whoAmI, -1), index, direction, Damage, 1f, 0, 0, 0);
                        }
                    }
                }
                if (aiStyle != 45 && aiStyle != 92 && (aiStyle != 105 && aiStyle != 106) && (type != 463 && type != 69 && (type != 70 && type != 621)) && (type != 10 && type != 11 && (type != 379 && type != 407) && (type != 476 && type != 623 && (type < 625 || type > 628))))
                {
                    int num1 = (int)(position.X / 16.0);
                    int num2 = (int)((position.X + width) / 16.0) + 1;
                    int num3 = (int)(position.Y / 16.0);
                    int num4 = (int)((position.Y + height) / 16.0) + 1;
                    if (num1 < 0)
                        num1 = 0;
                    if (num2 > Main.maxTilesX)
                        num2 = Main.maxTilesX;
                    if (num3 < 0)
                        num3 = 0;
                    if (num4 > Main.maxTilesY)
                        num4 = Main.maxTilesY;
                    AchievementsHelper.CurrentlyMining = true;
                    for (int i = num1; i < num2; ++i)
                    {
                        for (int j = num3; j < num4; ++j)
                        {
                            if (Main.tile[i, j] != null && Main.tileCut[Main.tile[i, j].type] && (Main.tile[i, j + 1] != null && Main.tile[i, j + 1].type != 78) && Main.tile[i, j + 1].type != 380)
                            {
                                WorldGen.KillTile(i, j, false, false, false);
                                if (Main.netMode != 0)
                                    NetMessage.SendData(17, -1, -1, "", 0, i, j, 0.0f, 0, 0, 0);
                            }
                        }
                    }
                    if (type == 461 || type == 632 || type == 642)
                        Utils.PlotTileLine(Center, Center + velocity * localAI[1], width * scale, new Utils.PerLinePoint(DelegateMethods.CutTiles));
                    else if (type == 611)
                        Utils.PlotTileLine(Center, Center + velocity, width * scale, new Utils.PerLinePoint(DelegateMethods.CutTiles));
                    AchievementsHelper.CurrentlyMining = false;
                }
            }
            if (owner == Main.myPlayer)
            {
                if (damage > 0)
                {
                    for (int index1 = 0; index1 < 200; ++index1)
                    {
                        bool flag1 = !updatedNPCImmunity || npcImmune[index1] == 0;
                        if (Main.npc[index1].active && !Main.npc[index1].dontTakeDamage && flag1 && (friendly && (!Main.npc[index1].friendly || type == 318 || Main.npc[index1].type == 22 && owner < 255 && Main.player[owner].killGuide || Main.npc[index1].type == 54 && owner < 255 && Main.player[owner].killClothier) || hostile && Main.npc[index1].friendly) && (owner < 0 || Main.npc[index1].immune[owner] == 0 || maxPenetrate == 1))
                        {
                            bool flag2 = false;
                            if (type == 11 && (Main.npc[index1].type == 47 || Main.npc[index1].type == 57))
                                flag2 = true;
                            else if (type == 31 && Main.npc[index1].type == 69)
                                flag2 = true;
                            else if (Main.npc[index1].trapImmune && trap)
                                flag2 = true;
                            else if (Main.npc[index1].immortal && npcProj)
                                flag2 = true;
                            if (!flag2 && (Main.npc[index1].noTileCollide || !ownerHitCheck || Collision.CanHit(Main.player[owner].position, Main.player[owner].width, Main.player[owner].height, Main.npc[index1].position, Main.npc[index1].width, Main.npc[index1].height)))
                            {
                                bool flag3;
                                if (Main.npc[index1].type == 414)
                                {
                                    Rectangle rect = Main.npc[index1].getRect();
                                    int num = 8;
                                    rect.X -= num;
                                    rect.Y -= num;
                                    rect.Width += num * 2;
                                    rect.Height += num * 2;
                                    flag3 = Colliding(myRect, rect);
                                }
                                else
                                    flag3 = Colliding(myRect, Main.npc[index1].getRect());
                                if (flag3)
                                {
                                    if (type == 604)
                                        Main.player[owner].Counterweight(Main.npc[index1].Center, damage, knockBack);
                                    if (Main.npc[index1].reflectingProjectiles && CanReflect())
                                    {
                                        Main.npc[index1].ReflectProjectile(whoAmI);
                                        return;
                                    }
                                    int Damage1 = Main.DamageVar(damage);
                                    if (type == 604)
                                    {
                                        friendly = false;
                                        ai[1] = 1000f;
                                    }
                                    if ((type == 400 || type == 401 || type == 402) && (Main.npc[index1].type >= 13 && Main.npc[index1].type <= 15))
                                    {
                                        Damage1 = (int)(Damage1 * 0.65);
                                        if (penetrate > 1)
                                            --penetrate;
                                    }
                                    if (type == 504)
                                        ai[0] += (float)((60.0 - ai[0]) / 2.0);
                                    if (aiStyle == 3 && type != 301)
                                    {
                                        if (ai[0] == 0.0)
                                        {
                                            velocity.X = -velocity.X;
                                            velocity.Y = -velocity.Y;
                                            netUpdate = true;
                                        }
                                        ai[0] = 1f;
                                    }
                                    else if (type == 582)
                                    {
                                        if (ai[0] != 0.0)
                                            direction *= -1;
                                    }
                                    else if (type == 612)
                                        direction = Main.player[owner].direction;
                                    else if (type == 624)
                                    {
                                        float num = 1f;
                                        if (Main.npc[index1].knockBackResist > 0.0)
                                            num = 1f / Main.npc[index1].knockBackResist;
                                        knockBack = 4f * num;
                                        if (Main.npc[index1].Center.X < Center.X)
                                            direction = 1;
                                        else
                                            direction = -1;
                                    }
                                    else if (aiStyle == 16)
                                    {
                                        if (timeLeft > 3)
                                            timeLeft = 3;
                                        if (Main.npc[index1].position.X + (Main.npc[index1].width / 2) < position.X + (width / 2))
                                            direction = -1;
                                        else
                                            direction = 1;
                                    }
                                    else if (aiStyle == 68)
                                    {
                                        if (timeLeft > 3)
                                            timeLeft = 3;
                                        if (Main.npc[index1].position.X + (Main.npc[index1].width / 2) < position.X + (width / 2))
                                            direction = -1;
                                        else
                                            direction = 1;
                                    }
                                    else if (aiStyle == 50)
                                    {
                                        if (Main.npc[index1].position.X + (Main.npc[index1].width / 2) < position.X + (width / 2))
                                            direction = -1;
                                        else
                                            direction = 1;
                                    }
                                    if (type == 509)
                                    {
                                        int num = Main.rand.Next(2, 6);
                                        for (int index2 = 0; index2 < num; ++index2)
                                        {
                                            Vector2 vector2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                                            vector2 += velocity * 3f;
                                            vector2.Normalize();
                                            vector2 *= Main.rand.Next(35, 81) * 0.1f;
                                            int Damage2 = (int)(damage * 0.5);
                                            NewProjectile(Center.X, Center.Y, vector2.X, vector2.Y, 504, Damage2, knockBack * 0.2f, owner, 0.0f, 0.0f);
                                        }
                                    }
                                    if (type == 598 || type == 636 || type == 614)
                                    {
                                        ai[0] = 1f;
                                        ai[1] = index1;
                                        velocity = (Main.npc[index1].Center - Center) * 0.75f;
                                        netUpdate = true;
                                    }
                                    if (type >= 511 && type <= 513)
                                        timeLeft = 0;
                                    if (type == 524)
                                    {
                                        netUpdate = true;
                                        ai[0] += 50f;
                                    }
                                    if (aiStyle == 39)
                                    {
                                        if (ai[1] == 0.0)
                                        {
                                            ai[1] = (index1 + 1);
                                            netUpdate = true;
                                        }
                                        if (Main.player[owner].position.X + (Main.player[owner].width / 2) < position.X + (width / 2))
                                            direction = 1;
                                        else
                                            direction = -1;
                                    }
                                    if (type == 41 && timeLeft > 1)
                                        timeLeft = 1;
                                    bool crit = false;
                                    if (!npcProj && !trap)
                                    {
                                        if (melee && Main.rand.Next(1, 101) <= Main.player[owner].meleeCrit)
                                            crit = true;
                                        if (ranged && Main.rand.Next(1, 101) <= Main.player[owner].rangedCrit)
                                            crit = true;
                                        if (magic && Main.rand.Next(1, 101) <= Main.player[owner].magicCrit)
                                            crit = true;
                                        if (thrown && Main.rand.Next(1, 101) <= Main.player[owner].thrownCrit)
                                            crit = true;
                                    }
                                    if (aiStyle == 99)
                                    {
                                        Main.player[owner].Counterweight(Main.npc[index1].Center, damage, knockBack);
                                        if (Main.npc[index1].Center.X < Main.player[owner].Center.X)
                                            direction = -1;
                                        else
                                            direction = 1;
                                        if (ai[0] >= 0.0)
                                        {
                                            Vector2 vector2_1 = Center - Main.npc[index1].Center;
                                            vector2_1.Normalize();
                                            float num = 16f;
                                            Projectile projectile1 = this;
                                            Vector2 vector2_2 = projectile1.velocity * -0.5f;
                                            projectile1.velocity = vector2_2;
                                            Projectile projectile2 = this;
                                            Vector2 vector2_3 = projectile2.velocity + vector2_1 * num;
                                            projectile2.velocity = vector2_3;
                                            netUpdate = true;
                                            localAI[0] += 20f;
                                            if (!Collision.CanHit(position, width, height, Main.player[owner].position, Main.player[owner].width, Main.player[owner].height))
                                            {
                                                localAI[0] += 40f;
                                                Damage1 = (int)(Damage1 * 0.75);
                                            }
                                        }
                                    }
                                    if (aiStyle == 93)
                                    {
                                        if (ai[0] == 0.0)
                                        {
                                            ai[1] = 0.0f;
                                            ai[0] = (-index1 - 1);
                                            velocity = Main.npc[index1].Center - Center;
                                        }
                                        Damage1 = ai[0] != 2.0 ? (int)(Damage1 * 0.15) : (int)(Damage1 * 1.35);
                                    }
                                    if (!npcProj)
                                    {
                                        int num = Item.NPCtoBanner(Main.npc[index1].BannerID());
                                        if (num >= 0)
                                            Main.player[Main.myPlayer].lastCreatureHit = num;
                                    }
                                    if (Main.netMode != 2)
                                    {
                                        int index2 = Item.NPCtoBanner(Main.npc[index1].BannerID());
                                        if (index2 > 0 && Main.player[owner].NPCBannerBuff[index2])
                                        {
                                            if (Main.expertMode)
                                                Damage1 *= 2;
                                            else
                                                Damage1 = (int)(Damage1 * 1.5);
                                        }
                                    }
                                    if (Main.expertMode)
                                    {
                                        if ((type == 30 || type == 28 || (type == 29 || type == 470) || (type == 517 || type == 588 || type == 637)) && (Main.npc[index1].type >= 13 && Main.npc[index1].type <= 15))
                                            Damage1 /= 5;
                                        if (type == 280 && (Main.npc[index1].type >= 134 && Main.npc[index1].type <= 136 || Main.npc[index1].type == 139))
                                            Damage1 = (int)(Damage1 * 0.75);
                                    }
                                    if (Main.netMode != 2 && Main.npc[index1].type == 439 && (type >= 0 && type <= 651) && ProjectileID.Sets.Homing[type])
                                        Damage1 = (int)(Damage1 * 0.75);
                                    if (type == 497 && penetrate != 1)
                                    {
                                        ai[0] = 25f;
                                        float num = velocity.Length();
                                        Vector2 vector2 = Main.npc[index1].Center - Center;
                                        vector2.Normalize();
                                        velocity = -(vector2 * num) * 0.9f;
                                        netUpdate = true;
                                    }
                                    if (type == 323 && (Main.npc[index1].type == 158 || Main.npc[index1].type == 159))
                                        Damage1 *= 10;
                                    if (type == 294)
                                        damage = (int)(damage * 0.8);
                                    if (type == 477 && penetrate > 1)
                                    {
                                        int[] numArray = new int[10];
                                        int maxValue = 0;
                                        int num1 = 700;
                                        int num2 = 20;
                                        for (int index2 = 0; index2 < 200; ++index2)
                                        {
                                            if (index2 != index1 && Main.npc[index2].CanBeChasedBy(this, false))
                                            {
                                                float num3 = (Center - Main.npc[index2].Center).Length();
                                                if (num3 > num2 && num3 < num1 && Collision.CanHitLine(Center, 1, 1, Main.npc[index2].Center, 1, 1))
                                                {
                                                    numArray[maxValue] = index2;
                                                    ++maxValue;
                                                    if (maxValue >= 9)
                                                        break;
                                                }
                                            }
                                        }
                                        if (maxValue > 0)
                                        {
                                            int index2 = Main.rand.Next(maxValue);
                                            Vector2 vector2 = Main.npc[numArray[index2]].Center - Center;
                                            float num3 = velocity.Length();
                                            vector2.Normalize();
                                            velocity = vector2 * num3;
                                            netUpdate = true;
                                        }
                                    }
                                    if (type == 261)
                                    {
                                        float num = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
                                        if (num < 1.0)
                                            num = 1f;
                                        Damage1 = (int)(Damage1 * num / 8.0);
                                    }
                                    StatusNPC(index1);
                                    if (type != 221 && type != 227 && type != 614)
                                        Main.player[owner].OnHit(Main.npc[index1].Center.X, Main.npc[index1].Center.Y, Main.npc[index1]);
                                    if (type == 317)
                                    {
                                        ai[1] = -1f;
                                        netUpdate = true;
                                    }
                                    if (!npcProj && !hostile && Main.player[owner].armorPenetration > 0)
                                        Damage1 += Main.npc[index1].checkArmorPenetration(Main.player[owner].armorPenetration);
                                    int dmg = npcProj ? (int)Main.npc[index1].StrikeNPCNoInteraction(Damage1, knockBack, direction, crit, false, false) : (int)Main.npc[index1].StrikeNPC(Damage1, knockBack, direction, crit, false, false);
                                    if (!npcProj && Main.player[owner].accDreamCatcher)
                                        Main.player[owner].addDPS(dmg);
                                    if (!npcProj && !Main.npc[index1].immortal)
                                    {
                                        if (type == 304 && dmg > 0 && (Main.npc[index1].lifeMax > 5 && !Main.player[owner].moonLeech))
                                            vampireHeal(dmg, new Vector2(Main.npc[index1].Center.X, Main.npc[index1].Center.Y));
                                        if (Main.npc[index1].value > 0.0 && Main.player[owner].coins && Main.rand.Next(5) == 0)
                                        {
                                            int Type = 71;
                                            if (Main.rand.Next(10) == 0)
                                                Type = 72;
                                            if (Main.rand.Next(100) == 0)
                                                Type = 73;
                                            int number = Item.NewItem((int)Main.npc[index1].position.X, (int)Main.npc[index1].position.Y, Main.npc[index1].width, Main.npc[index1].height, Type, 1, false, 0, false);
                                            Main.item[number].stack = Main.rand.Next(1, 11);
                                            Main.item[number].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
                                            Main.item[number].velocity.X = Main.rand.Next(10, 31) * 0.2f * direction;
                                            if (Main.netMode == 1)
                                                NetMessage.SendData(21, -1, -1, "", number, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                                        }
                                        if (dmg > 0 && Main.npc[index1].lifeMax > 5 && (friendly && !hostile) && aiStyle != 59)
                                        {
                                            if (Main.npc[index1].canGhostHeal)
                                            {
                                                if (Main.player[owner].ghostHeal && !Main.player[owner].moonLeech)
                                                    ghostHeal(dmg, new Vector2(Main.npc[index1].Center.X, Main.npc[index1].Center.Y));
                                                if (Main.player[owner].ghostHurt)
                                                    ghostHurt(dmg, new Vector2(Main.npc[index1].Center.X, Main.npc[index1].Center.Y));
                                                if (Main.player[owner].setNebula && Main.player[owner].nebulaCD == 0 && Main.rand.Next(3) == 0)
                                                {
                                                    Main.player[owner].nebulaCD = 30;
                                                    int Type = Utils.SelectRandom(Main.rand, 3453, 3454, 3455);
                                                    int number = Item.NewItem((int)Main.npc[index1].position.X, (int)Main.npc[index1].position.Y, Main.npc[index1].width, Main.npc[index1].height, Type, 1, false, 0, false);
                                                    Main.item[number].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
                                                    Main.item[number].velocity.X = Main.rand.Next(10, 31) * 0.2f * direction;
                                                    if (Main.netMode == 1)
                                                        NetMessage.SendData(21, -1, -1, "", number, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                                                }
                                            }
                                            if (melee && Main.player[owner].beetleOffense)
                                            {
                                                if (Main.player[owner].beetleOrbs == 0)
                                                    Main.player[owner].beetleCounter += (dmg * 3);
                                                else if (Main.player[owner].beetleOrbs == 1)
                                                    Main.player[owner].beetleCounter += (dmg * 2);
                                                else
                                                    Main.player[owner].beetleCounter += dmg;
                                                Main.player[owner].beetleCountdown = 0;
                                            }
                                            if (arrow && type != 631 && Main.player[owner].phantasmTime > 0)
                                            {
                                                Vector2 Source = Main.player[owner].position + Main.player[owner].Size * Utils.RandomVector2(Main.rand, 0.0f, 1f);
                                                Vector2 vector2 = Main.npc[index1].DirectionFrom(Source) * 6f;
                                                NewProjectile(Source.X, Source.Y, vector2.X, vector2.Y, 631, damage, 0.0f, owner, index1, 0.0f);
                                                NewProjectile(Source.X, Source.Y, vector2.X, vector2.Y, 631, damage, 0.0f, owner, index1, 15f);
                                                NewProjectile(Source.X, Source.Y, vector2.X, vector2.Y, 631, damage, 0.0f, owner, index1, 30f);
                                            }
                                        }
                                    }
                                    if (!npcProj && melee && Main.player[owner].meleeEnchant == 7)
                                        NewProjectile(Main.npc[index1].Center.X, Main.npc[index1].Center.Y, Main.npc[index1].velocity.X, Main.npc[index1].velocity.Y, 289, 0, 0.0f, owner, 0.0f, 0.0f);
                                    if (Main.netMode != 0)
                                    {
                                        if (crit)
                                            NetMessage.SendData(28, -1, -1, "", index1, Damage1, knockBack, direction, 1, 0, 0);
                                        else
                                            NetMessage.SendData(28, -1, -1, "", index1, Damage1, knockBack, direction, 0, 0, 0);
                                    }
                                    if (type >= 390 && type <= 392)
                                        localAI[1] = 20f;
                                    if (type == 434)
                                        numUpdates = 0;
                                    else if (type == 598 || type == 636 || type == 614)
                                    {
                                        damage = 0;
                                        int length = 6;
                                        if (type == 614)
                                            length = 10;
                                        if (type == 636)
                                            length = 8;
                                        Point[] pointArray = new Point[length];
                                        int num = 0;
                                        for (int x = 0; x < 1000; ++x)
                                        {
                                            if (x != whoAmI && Main.projectile[x].active && (Main.projectile[x].owner == Main.myPlayer && Main.projectile[x].type == type) && (Main.projectile[x].ai[0] == 1.0 && Main.projectile[x].ai[1] == index1))
                                            {
                                                pointArray[num++] = new Point(x, Main.projectile[x].timeLeft);
                                                if (num >= pointArray.Length)
                                                    break;
                                            }
                                        }
                                        if (num >= pointArray.Length)
                                        {
                                            int index2 = 0;
                                            for (int index3 = 1; index3 < pointArray.Length; ++index3)
                                            {
                                                if (pointArray[index3].Y < pointArray[index2].Y)
                                                    index2 = index3;
                                            }
                                            Main.projectile[pointArray[index2].X].Kill();
                                        }
                                    }
                                    else if (type == 632)
                                        Main.npc[index1].immune[owner] = 5;
                                    else if (type == 514)
                                        Main.npc[index1].immune[owner] = 1;
                                    else if (type == 617)
                                        Main.npc[index1].immune[owner] = 4;
                                    else if (type == 611)
                                    {
                                        if (localAI[1] <= 0.0)
                                            NewProjectile(Main.npc[index1].Center.X, Main.npc[index1].Center.Y, 0.0f, 0.0f, 612, damage, 10f, owner, 0.0f, (float)(0.850000023841858 + Utils.NextFloat(Main.rand) * 1.14999997615814));
                                        localAI[1] = 4f;
                                    }
                                    else if (type == 595)
                                        Main.npc[index1].immune[owner] = 5;
                                    else if (type >= 625 && type <= 628)
                                        Main.npc[index1].immune[owner] = 6;
                                    else if (type == 286)
                                        Main.npc[index1].immune[owner] = 5;
                                    else if (type == 514)
                                        Main.npc[index1].immune[owner] = 3;
                                    else if (type == 443)
                                        Main.npc[index1].immune[owner] = 8;
                                    else if (type >= 424 && type <= 426)
                                        Main.npc[index1].immune[owner] = 5;
                                    else if (type == 634 || type == 635)
                                        Main.npc[index1].immune[owner] = 5;
                                    else if (type == 246)
                                        Main.npc[index1].immune[owner] = 7;
                                    else if (type == 249)
                                        Main.npc[index1].immune[owner] = 7;
                                    else if (type == 190)
                                        Main.npc[index1].immune[owner] = 8;
                                    else if (type == 409)
                                        Main.npc[index1].immune[owner] = 6;
                                    else if (type == 407)
                                        Main.npc[index1].immune[owner] = 20;
                                    else if (type == 311)
                                        Main.npc[index1].immune[owner] = 7;
                                    else if (type == 582)
                                    {
                                        Main.npc[index1].immune[owner] = 7;
                                        if (ai[0] != 1.0)
                                        {
                                            ai[0] = 1f;
                                            netUpdate = true;
                                        }
                                    }
                                    else
                                    {
                                        if (type == 451)
                                        {
                                            if (ai[0] == 0.0)
                                                ai[0] += penetrate;
                                            else
                                                ai[0] -= (penetrate + 1);
                                            ai[1] = 0.0f;
                                            netUpdate = true;
                                            break;
                                        }
                                        if (penetrate != 1)
                                            Main.npc[index1].immune[owner] = 10;
                                    }
                                    if (penetrate > 0 && type != 317)
                                    {
                                        if (type == 357)
                                            damage = (int)(damage * 0.9);
                                        --penetrate;
                                        if (penetrate == 0)
                                            break;
                                    }
                                    if (aiStyle == 7)
                                    {
                                        ai[0] = 1f;
                                        damage = 0;
                                        netUpdate = true;
                                    }
                                    else if (aiStyle == 13)
                                    {
                                        ai[0] = 1f;
                                        netUpdate = true;
                                    }
                                    else if (aiStyle == 69)
                                    {
                                        ai[0] = 1f;
                                        netUpdate = true;
                                    }
                                    else if (type == 607)
                                    {
                                        ai[0] = 1f;
                                        netUpdate = true;
                                        friendly = false;
                                    }
                                    else if (type == 638 || type == 639 || type == 640)
                                    {
                                        npcImmune[index1] = -1;
                                        Main.npc[index1].immune[owner] = 0;
                                        damage = (int)(damage * 0.96);
                                    }
                                    else if (type == 642)
                                    {
                                        npcImmune[index1] = 10;
                                        Main.npc[index1].immune[owner] = 0;
                                    }
                                    else if (type == 611 || type == 612)
                                    {
                                        npcImmune[index1] = 6;
                                        Main.npc[index1].immune[owner] = 4;
                                    }
                                    else if (type == 645)
                                    {
                                        npcImmune[index1] = -1;
                                        Main.npc[index1].immune[owner] = 0;
                                        if (ai[1] != -1.0)
                                        {
                                            ai[0] = 0.0f;
                                            ai[1] = -1f;
                                            netUpdate = true;
                                        }
                                    }
                                    ++numHits;
                                }
                            }
                        }
                    }
                }
                if (damage > 0 && Main.player[Main.myPlayer].hostile)
                {
                    for (int index = 0; index < 255; ++index)
                    {
                        if (index != owner && Main.player[index].active && (!Main.player[index].dead && !Main.player[index].immune) && (Main.player[index].hostile && playerImmune[index] <= 0 && (Main.player[Main.myPlayer].team == 0 || Main.player[Main.myPlayer].team != Main.player[index].team)) && ((!ownerHitCheck || Collision.CanHit(Main.player[owner].position, Main.player[owner].width, Main.player[owner].height, Main.player[index].position, Main.player[index].width, Main.player[index].height)) && Colliding(myRect, Main.player[index].getRect())))
                        {
                            if (aiStyle == 3)
                            {
                                if (ai[0] == 0.0)
                                {
                                    velocity.X = -velocity.X;
                                    velocity.Y = -velocity.Y;
                                    netUpdate = true;
                                }
                                ai[0] = 1f;
                            }
                            else if (aiStyle == 16)
                            {
                                if (timeLeft > 3)
                                    timeLeft = 3;
                                if (Main.player[index].position.X + (Main.player[index].width / 2) < position.X + (width / 2))
                                    direction = -1;
                                else
                                    direction = 1;
                            }
                            else if (aiStyle == 68)
                            {
                                if (timeLeft > 3)
                                    timeLeft = 3;
                                if (Main.player[index].position.X + (Main.player[index].width / 2) < position.X + (width / 2))
                                    direction = -1;
                                else
                                    direction = 1;
                            }
                            if (type == 41 && timeLeft > 1)
                                timeLeft = 1;
                            bool Crit = false;
                            if (melee && Main.rand.Next(1, 101) <= Main.player[owner].meleeCrit)
                                Crit = true;
                            int Damage = Main.DamageVar(damage);
                            if (!Main.player[index].immune)
                                StatusPvP(index);
                            if (type != 221 && type != 227 && type != 614)
                                Main.player[owner].OnHit(Main.player[index].Center.X, Main.player[index].Center.Y, Main.player[index]);
                            int dmg = (int)Main.player[index].Hurt(Damage, direction, true, false, Lang.deathMsg(owner, -1, whoAmI, -1), Crit);
                            if (dmg > 0 && Main.player[owner].ghostHeal && (friendly && !hostile))
                                ghostHeal(dmg, new Vector2(Main.player[index].Center.X, Main.player[index].Center.Y));
                            if (type == 304 && dmg > 0)
                                vampireHeal(dmg, new Vector2(Main.player[index].Center.X, Main.player[index].Center.Y));
                            if (melee && Main.player[owner].meleeEnchant == 7)
                                NewProjectile(Main.player[index].Center.X, Main.player[index].Center.Y, Main.player[index].velocity.X, Main.player[index].velocity.Y, 289, 0, 0.0f, owner, 0.0f, 0.0f);
                            if (Main.netMode != 0)
                            {
                                if (Crit)
                                    NetMessage.SendData(26, -1, -1, Lang.deathMsg(owner, -1, whoAmI, -1), index, direction, Damage, 1f, 1, 0, 0);
                                else
                                    NetMessage.SendData(26, -1, -1, Lang.deathMsg(owner, -1, whoAmI, -1), index, direction, Damage, 1f, 0, 0, 0);
                            }
                            playerImmune[index] = 40;
                            if (penetrate > 0)
                            {
                                --penetrate;
                                if (penetrate == 0)
                                    break;
                            }
                            if (aiStyle == 7)
                            {
                                ai[0] = 1f;
                                damage = 0;
                                netUpdate = true;
                            }
                            else if (aiStyle == 13)
                            {
                                ai[0] = 1f;
                                netUpdate = true;
                            }
                            else if (aiStyle == 69)
                            {
                                ai[0] = 1f;
                                netUpdate = true;
                            }
                        }
                    }
                }
            }
            if (type == 10 && Main.netMode != 1)
            {
                for (int index = 0; index < 200; ++index)
                {
                    if (Main.npc[index].active && Main.npc[index].type == 534)
                    {
                        Rectangle rectangle = new Rectangle((int)Main.npc[index].position.X, (int)Main.npc[index].position.Y, Main.npc[index].width, Main.npc[index].height);
                        if (myRect.Intersects(rectangle))
                            Main.npc[index].Transform(441);
                    }
                }
            }
            if (type == 11 && Main.netMode != 1)
            {
                for (int index = 0; index < 200; ++index)
                {
                    if (Main.npc[index].active)
                    {
                        if (Main.npc[index].type == 46 || Main.npc[index].type == 303)
                        {
                            Rectangle rectangle = new Rectangle((int)Main.npc[index].position.X, (int)Main.npc[index].position.Y, Main.npc[index].width, Main.npc[index].height);
                            if (myRect.Intersects(rectangle))
                                Main.npc[index].Transform(47);
                        }
                        else if (Main.npc[index].type == 55)
                        {
                            Rectangle rectangle = new Rectangle((int)Main.npc[index].position.X, (int)Main.npc[index].position.Y, Main.npc[index].width, Main.npc[index].height);
                            if (myRect.Intersects(rectangle))
                                Main.npc[index].Transform(57);
                        }
                        else if (Main.npc[index].type == 148 || Main.npc[index].type == 149)
                        {
                            Rectangle rectangle = new Rectangle((int)Main.npc[index].position.X, (int)Main.npc[index].position.Y, Main.npc[index].width, Main.npc[index].height);
                            if (myRect.Intersects(rectangle))
                                Main.npc[index].Transform(168);
                        }
                    }
                }
            }
            if (type == 463 && Main.netMode != 1)
            {
                for (int index = 0; index < 200; ++index)
                {
                    if (Main.npc[index].active)
                    {
                        if (Main.npc[index].type == 46 || Main.npc[index].type == 303)
                        {
                            Rectangle rectangle = new Rectangle((int)Main.npc[index].position.X, (int)Main.npc[index].position.Y, Main.npc[index].width, Main.npc[index].height);
                            if (myRect.Intersects(rectangle))
                                Main.npc[index].Transform(464);
                        }
                        else if (Main.npc[index].type == 55)
                        {
                            Rectangle rectangle = new Rectangle((int)Main.npc[index].position.X, (int)Main.npc[index].position.Y, Main.npc[index].width, Main.npc[index].height);
                            if (myRect.Intersects(rectangle))
                                Main.npc[index].Transform(465);
                        }
                        else if (Main.npc[index].type == 148 || Main.npc[index].type == 149)
                        {
                            Rectangle rectangle = new Rectangle((int)Main.npc[index].position.X, (int)Main.npc[index].position.Y, Main.npc[index].width, Main.npc[index].height);
                            if (myRect.Intersects(rectangle))
                                Main.npc[index].Transform(470);
                        }
                    }
                }
            }
            if (Main.netMode == 2 || !hostile || (Main.myPlayer >= 255 || damage <= 0))
                return;
            int i1 = Main.myPlayer;
            if (!Main.player[i1].active || Main.player[i1].dead || (Main.player[i1].immune || !Colliding(myRect, Main.player[i1].getRect())))
                return;
            int num5 = direction;
            int hitDirection = Main.player[i1].position.X + (Main.player[i1].width / 2) >= position.X + (width / 2) ? 1 : -1;
            int num6 = Main.DamageVar(damage);
            if (!Main.player[i1].immune)
                StatusPlayer(i1);
            if (Main.player[i1].resistCold && coldDamage)
                num6 = (int)(num6 * 0.699999988079071);
            if (Main.expertMode)
                num6 = (int)(num6 * Main.expertDamage);
            Main.player[i1].Hurt(num6 * 2, hitDirection, false, false, Lang.deathMsg(-1, -1, whoAmI, -1), false);
            if (trap)
            {
                Main.player[i1].trapDebuffSource = true;
                if (Main.player[i1].dead)
                    AchievementsHelper.HandleSpecialEvent(Main.player[i1], 4);
            }
            if (type == 435)
                --penetrate;
            if (type == 436)
                --penetrate;
            if (type != 437)
                return;
            --penetrate;
        }

        public bool Colliding(Rectangle myRect, Rectangle targetRect)
        {
            if (type == 598 && targetRect.Width > 8 && targetRect.Height > 8)
                targetRect.Inflate(-targetRect.Width / 8, -targetRect.Height / 8);
            else if (type == 614 && targetRect.Width > 8 && targetRect.Height > 8)
                targetRect.Inflate(-targetRect.Width / 8, -targetRect.Height / 8);
            else if (type == 636 && targetRect.Width > 8 && targetRect.Height > 8)
                targetRect.Inflate(-targetRect.Width / 8, -targetRect.Height / 8);
            else if (type == 607)
            {
                myRect.X += (int)velocity.X;
                myRect.Y += (int)velocity.Y;
            }
            if (myRect.Intersects(targetRect))
                return true;
            if (type == 461)
            {
                float collisionPoint = 0.0f;
                if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetRect), Utils.Size(targetRect), Center, Center + velocity * localAI[1], 22f * scale, ref collisionPoint))
                    return true;
            }
            else if (type == 642)
            {
                float collisionPoint = 0.0f;
                if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetRect), Utils.Size(targetRect), Center, Center + velocity * localAI[1], 30f * scale, ref collisionPoint))
                    return true;
            }
            else if (type == 632)
            {
                float collisionPoint = 0.0f;
                if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetRect), Utils.Size(targetRect), Center, Center + velocity * localAI[1], 22f * scale, ref collisionPoint))
                    return true;
            }
            else if (type == 455)
            {
                float collisionPoint = 0.0f;
                if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetRect), Utils.Size(targetRect), Center, Center + velocity * localAI[1], 36f * scale, ref collisionPoint))
                    return true;
            }
            else if (type == 611)
            {
                float collisionPoint = 0.0f;
                if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetRect), Utils.Size(targetRect), Center, Center + velocity, 16f * scale, ref collisionPoint))
                    return true;
            }
            else if (type == 537)
            {
                float collisionPoint = 0.0f;
                if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetRect), Utils.Size(targetRect), Center, Center + velocity * localAI[1], 22f * scale, ref collisionPoint))
                    return true;
            }
            else if (type == 466 || type == 580)
            {
                for (int index = 0; index < oldPos.Length; ++index)
                {
                    myRect.X = (int)oldPos[index].X;
                    myRect.Y = (int)oldPos[index].Y;
                    if (myRect.Intersects(targetRect))
                        return true;
                }
            }
            else if (type == 464 && ai[1] != 1.0)
            {
                Vector2 spinningpoint = Utils.RotatedBy(new Vector2(0.0f, -720f), Utils.ToRotation(velocity), new Vector2()) * (float)(ai[0] % 45.0 / 45.0);
                for (int index = 0; index < 6; ++index)
                {
                    float num = (float)(index * 6.28318548202515 / 6.0);
                    if (Utils.CenteredRectangle(Center + Utils.RotatedBy(spinningpoint, num, new Vector2()), new Vector2(30f, 30f)).Intersects(targetRect))
                        return true;
                }
            }
            return false;
        }

        public void ProjLight()
        {
            if (light <= 0.0)
                return;
            float R = light;
            float G = light;
            float B = light;
            if (type == 446)
            {
                R *= 0.0f;
                B *= 0.8f;
            }
            else if (type == 493 || type == 494)
                G *= 0.3f;
            else if (type == 332)
            {
                B *= 0.1f;
                G *= 0.6f;
            }
            else if (type == 259)
                B *= 0.1f;
            else if (type == 329)
            {
                B *= 0.1f;
                G *= 0.9f;
            }
            else if (type == 2 || type == 82)
            {
                G *= 0.75f;
                B *= 0.55f;
            }
            else if (type == 172)
            {
                G *= 0.55f;
                R *= 0.35f;
            }
            else if (type == 308)
            {
                G *= 0.7f;
                R *= 0.1f;
            }
            else if (type == 304)
            {
                G *= 0.2f;
                B *= 0.1f;
            }
            else if (type == 263)
            {
                G *= 0.7f;
                R *= 0.1f;
            }
            else if (type == 274)
            {
                G *= 0.1f;
                R *= 0.7f;
            }
            else if (type == 254)
                R *= 0.1f;
            else if (type == 94)
            {
                R *= 0.5f;
                G *= 0.0f;
            }
            else if (type == 95 || type == 96 || (type == 103 || type == 104))
            {
                R *= 0.35f;
                G *= 1f;
                B *= 0.0f;
            }
            else if (type == 4)
            {
                G *= 0.1f;
                R *= 0.5f;
            }
            else if (type == 257)
            {
                G *= 0.9f;
                R *= 0.1f;
            }
            else if (type == 9)
            {
                G *= 0.1f;
                B *= 0.6f;
            }
            else if (type == 488)
            {
                R = 0.3f;
                B = 0.25f;
                G = 0.0f;
            }
            else if (type == 92)
            {
                G *= 0.6f;
                R *= 0.8f;
            }
            else if (type == 93)
            {
                G *= 1f;
                R *= 1f;
                B *= 0.01f;
            }
            else if (type == 12)
            {
                R *= 0.9f;
                G *= 0.8f;
                B *= 0.1f;
            }
            else if (type == 14 || type == 110 || (type == 180 || type == 242) || type == 302)
            {
                G *= 0.7f;
                B *= 0.1f;
            }
            else if (type == 15)
            {
                G *= 0.4f;
                B *= 0.1f;
                R = 1f;
            }
            else if (type == 16)
            {
                R *= 0.1f;
                G *= 0.4f;
                B = 1f;
            }
            else if (type == 18)
            {
                G *= 0.1f;
                R *= 0.6f;
            }
            else if (type == 19)
            {
                G *= 0.5f;
                B *= 0.1f;
            }
            else if (type == 20)
            {
                R *= 0.1f;
                B *= 0.3f;
            }
            else if (type == 22)
            {
                R = 0.0f;
                G = 0.0f;
            }
            else if (type == 27)
            {
                R *= 0.0f;
                G *= 0.3f;
                B = 1f;
            }
            else if (type == 34)
            {
                G *= 0.1f;
                B *= 0.1f;
            }
            else if (type == 36)
            {
                R = 0.8f;
                G *= 0.2f;
                B *= 0.6f;
            }
            else if (type == 41)
            {
                G *= 0.8f;
                B *= 0.6f;
            }
            else if (type == 44 || type == 45)
            {
                B = 1f;
                R *= 0.6f;
                G *= 0.1f;
            }
            else if (type == 50)
            {
                R *= 0.7f;
                B *= 0.8f;
            }
            else if (type == 515)
            {
                G *= 0.6f;
                B *= 0.85f;
            }
            else if (type == 53)
            {
                R *= 0.7f;
                G *= 0.8f;
            }
            else if (type == 473)
            {
                R *= 1.05f;
                G *= 0.95f;
                B *= 0.55f;
            }
            else if (type == 72)
            {
                R *= 0.45f;
                G *= 0.75f;
                B = 1f;
            }
            else if (type == 86)
            {
                R *= 1f;
                G *= 0.45f;
                B = 0.75f;
            }
            else if (type == 87)
            {
                R *= 0.45f;
                G = 1f;
                B *= 0.75f;
            }
            else if (type == 73)
            {
                R *= 0.4f;
                G *= 0.6f;
                B *= 1f;
            }
            else if (type == 74)
            {
                R *= 1f;
                G *= 0.4f;
                B *= 0.6f;
            }
            else if (type == 284)
            {
                R *= 1f;
                G *= 0.1f;
                B *= 0.8f;
            }
            else if (type == 285)
            {
                R *= 0.1f;
                G *= 0.5f;
                B *= 1f;
            }
            else if (type == 286)
            {
                R *= 1f;
                G *= 0.5f;
                B *= 0.1f;
            }
            else if (type == 287)
            {
                R *= 0.9f;
                G *= 1f;
                B *= 0.4f;
            }
            else if (type == 283)
            {
                R *= 0.8f;
                G *= 0.1f;
            }
            else if (type == 76 || type == 77 || type == 78)
            {
                R *= 1f;
                G *= 0.3f;
                B *= 0.6f;
            }
            else if (type == 79)
            {
                R = Main.DiscoR / 255;
                G = Main.DiscoG / 255;
                B = Main.DiscoB / 255;
            }
            else if (type == 80)
            {
                R *= 0.0f;
                G *= 0.8f;
                B *= 1f;
            }
            else if (type == 83 || type == 88)
            {
                R *= 0.7f;
                G *= 0.0f;
                B *= 1f;
            }
            else if (type == 100)
            {
                R *= 1f;
                G *= 0.5f;
                B *= 0.0f;
            }
            else if (type == 84 || type == 389)
            {
                R *= 0.8f;
                G *= 0.0f;
                B *= 0.5f;
            }
            else if (type == 89 || type == 90)
            {
                G *= 0.2f;
                B *= 1f;
                R *= 0.05f;
            }
            else if (type == 106)
            {
                R *= 0.0f;
                G *= 0.5f;
                B *= 1f;
            }
            else if (type == 113)
            {
                R *= 0.25f;
                G *= 0.75f;
                B *= 1f;
            }
            else if (type == 114 || type == 115)
            {
                R *= 0.5f;
                G *= 0.05f;
                B *= 1f;
            }
            else if (type == 116)
                B *= 0.25f;
            else if (type == 131)
            {
                R *= 0.1f;
                G *= 0.4f;
            }
            else if (type == 132 || type == 157)
            {
                R *= 0.2f;
                B *= 0.6f;
            }
            else if (type == 156)
            {
                R *= 1f;
                B *= 0.6f;
                G = 0.0f;
            }
            else if (type == 173)
            {
                R *= 0.3f;
                B *= 1f;
                G = 0.4f;
            }
            else if (type == 207)
            {
                R *= 0.4f;
                B *= 0.4f;
            }
            else if (type == 253)
            {
                R = 0.0f;
                G *= 0.4f;
            }
            else if (type == 211)
            {
                R *= 0.5f;
                G *= 0.9f;
                B *= 1f;
                light = localAI[0] != 0.0 ? 1f : 1.5f;
            }
            else if (type == 209)
            {
                float num1 = (float)((255 - alpha) / 255);
                float num2 = R * 0.3f;
                float num3 = G * 0.4f;
                B = B * 1.75f * num1;
                R = num2 * num1;
                G = num3 * num1;
            }
            else if (type == 226 || type == 227 | type == 229)
            {
                R *= 0.25f;
                G *= 1f;
                B *= 0.5f;
            }
            else if (type == 251)
            {
                float num1 = Main.DiscoR / 255;
                float num2 = Main.DiscoG / 255;
                float num3 = Main.DiscoB / 255;
                float num4 = (float)((num1 + 1.0) / 2.0);
                float num5 = (float)((num2 + 1.0) / 2.0);
                float num6 = (float)((num3 + 1.0) / 2.0);
                R = num4 * light;
                G = num5 * light;
                B = num6 * light;
            }
            else if (type == 278 || type == 279)
            {
                R *= 1f;
                G *= 1f;
                B *= 0.0f;
            }
            Lighting.AddLight((int)((position.X + (width / 2)) / 16.0), (int)((position.Y + (height / 2)) / 16.0), R, G, B);
        }

        public Rectangle getRect()
        {
            return new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public void Update(int i)
        {
            if (!active)
                return;
            numUpdates = extraUpdates;
            while (numUpdates >= 0)
            {
                --numUpdates;
                if (type == 640 && ai[1] > 0.0)
                {
                    --ai[1];
                }
                else
                {
                    if (position.X <= Main.leftWorld || position.X + width >= Main.rightWorld || (position.Y <= Main.topWorld || position.Y + height >= Main.bottomWorld))
                    {
                        active = false;
                        return;
                    }
                    if (type != 344 && !npcProj)
                    {
                        if (Main.player[owner].frostBurn && (melee || ranged) && (friendly && !hostile && (!noEnchantments && Main.rand.Next(2 * (1 + extraUpdates)) == 0)))
                        {
                            int index = Dust.NewDust(position, width, height, 135, velocity.X * 0.2f + (direction * 3), velocity.Y * 0.2f, 100, new Color(), 2f);
                            Main.dust[index].noGravity = true;
                            Main.dust[index].velocity *= 0.7f;
                            Main.dust[index].velocity.Y -= 0.5f;
                        }
                        if (melee && Main.player[owner].meleeEnchant > 0 && !noEnchantments)
                        {
                            if (Main.player[owner].meleeEnchant == 1 && Main.rand.Next(3) == 0)
                            {
                                int index = Dust.NewDust(position, width, height, 171, 0.0f, 0.0f, 100, new Color(), 1f);
                                Main.dust[index].noGravity = true;
                                Main.dust[index].fadeIn = 1.5f;
                                Main.dust[index].velocity *= 0.25f;
                            }
                            if (Main.player[owner].meleeEnchant == 1)
                            {
                                if (Main.rand.Next(3) == 0)
                                {
                                    int index = Dust.NewDust(position, width, height, 171, 0.0f, 0.0f, 100, new Color(), 1f);
                                    Main.dust[index].noGravity = true;
                                    Main.dust[index].fadeIn = 1.5f;
                                    Main.dust[index].velocity *= 0.25f;
                                }
                            }
                            else if (Main.player[owner].meleeEnchant == 2)
                            {
                                if (Main.rand.Next(2) == 0)
                                {
                                    int index = Dust.NewDust(position, width, height, 75, velocity.X * 0.2f + (direction * 3), velocity.Y * 0.2f, 100, new Color(), 2.5f);
                                    Main.dust[index].noGravity = true;
                                    Main.dust[index].velocity *= 0.7f;
                                    Main.dust[index].velocity.Y -= 0.5f;
                                }
                            }
                            else if (Main.player[owner].meleeEnchant == 3)
                            {
                                if (Main.rand.Next(2) == 0)
                                {
                                    int index = Dust.NewDust(position, width, height, 6, velocity.X * 0.2f + (direction * 3), velocity.Y * 0.2f, 100, new Color(), 2.5f);
                                    Main.dust[index].noGravity = true;
                                    Main.dust[index].velocity *= 0.7f;
                                    Main.dust[index].velocity.Y -= 0.5f;
                                }
                            }
                            else if (Main.player[owner].meleeEnchant == 4)
                            {
                                if (Main.rand.Next(2) == 0)
                                {
                                    int index = Dust.NewDust(position, width, height, 57, velocity.X * 0.2f + (direction * 3), velocity.Y * 0.2f, 100, new Color(), 1.1f);
                                    Main.dust[index].noGravity = true;
                                    Main.dust[index].velocity.X /= 2f;
                                    Main.dust[index].velocity.Y /= 2f;
                                }
                            }
                            else if (Main.player[owner].meleeEnchant == 5)
                            {
                                if (Main.rand.Next(2) == 0)
                                {
                                    int index = Dust.NewDust(position, width, height, 169, 0.0f, 0.0f, 100, new Color(), 1f);
                                    Main.dust[index].velocity.X += direction;
                                    Main.dust[index].velocity.Y += 0.2f;
                                    Main.dust[index].noGravity = true;
                                }
                            }
                            else if (Main.player[owner].meleeEnchant == 6)
                            {
                                if (Main.rand.Next(2) == 0)
                                {
                                    int index = Dust.NewDust(position, width, height, 135, 0.0f, 0.0f, 100, new Color(), 1f);
                                    Main.dust[index].velocity.X += direction;
                                    Main.dust[index].velocity.Y += 0.2f;
                                    Main.dust[index].noGravity = true;
                                }
                            }
                            else if (Main.player[owner].meleeEnchant == 7)
                            {
                                if (Main.rand.Next(20) == 0)
                                {
                                    int index = Dust.NewDust(position, width, height, Main.rand.Next(139, 143), velocity.X, velocity.Y, 0, new Color(), 1.2f);
                                    Main.dust[index].velocity.X *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                                    Main.dust[index].velocity.Y *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                                    Main.dust[index].velocity.X += Main.rand.Next(-50, 51) * 0.05f;
                                    Main.dust[index].velocity.Y += Main.rand.Next(-50, 51) * 0.05f;
                                    Main.dust[index].scale *= (float)(1.0 + Main.rand.Next(-30, 31) * 0.00999999977648258);
                                }
                                if (Main.rand.Next(40) == 0)
                                {
                                    int index = Gore.NewGore(position, velocity, Main.rand.Next(276, 283), 1f);
                                    Main.gore[index].velocity.X *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                                    Main.gore[index].velocity.Y *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                                    Main.gore[index].scale *= (float)(1.0 + Main.rand.Next(-20, 21) * 0.00999999977648258);
                                    Main.gore[index].velocity.X += Main.rand.Next(-50, 51) * 0.05f;
                                    Main.gore[index].velocity.Y += Main.rand.Next(-50, 51) * 0.05f;
                                }
                            }
                            else if (Main.player[owner].meleeEnchant == 8 && Main.rand.Next(4) == 0)
                            {
                                int index = Dust.NewDust(position, width, height, 46, 0.0f, 0.0f, 100, new Color(), 1f);
                                Main.dust[index].noGravity = true;
                                Main.dust[index].fadeIn = 1.5f;
                                Main.dust[index].velocity *= 0.25f;
                            }
                        }
                        if (melee && Main.player[owner].magmaStone && (!noEnchantments && Main.rand.Next(3) != 0))
                        {
                            int index = Dust.NewDust(new Vector2(position.X - 4f, position.Y - 4f), width + 8, height + 8, 6, velocity.X * 0.2f, velocity.Y * 0.2f, 100, new Color(), 2f);
                            if (Main.rand.Next(2) == 0)
                                Main.dust[index].scale = 1.5f;
                            Main.dust[index].noGravity = true;
                            Main.dust[index].velocity.X *= 2f;
                            Main.dust[index].velocity.Y *= 2f;
                        }
                    }
                    if (minion && numUpdates == -1 && (type != 625 && type != 628))
                    {
                        minionPos = Main.player[owner].numMinions;
                        if (Main.player[owner].slotsMinions + minionSlots > Main.player[owner].maxMinions && owner == Main.myPlayer)
                        {
                            if (type == 627 || type == 626)
                            {
                                Projectile projectile1 = Main.projectile[(int)ai[0]];
                                if (projectile1.type != 625)
                                    projectile1.localAI[1] = localAI[1];
                                Projectile projectile2 = Main.projectile[(int)localAI[1]];
                                projectile2.ai[0] = ai[0];
                                projectile2.ai[1] = 1f;
                                projectile2.netUpdate = true;
                            }
                            Kill();
                        }
                        else
                        {
                            ++Main.player[owner].numMinions;
                            Main.player[owner].slotsMinions += minionSlots;
                        }
                    }
                    float num1 = (float)(1.0 + Math.Abs(velocity.X) / 3.0);
                    if (gfxOffY > 0.0)
                    {
                        gfxOffY -= num1 * stepSpeed;
                        if (gfxOffY < 0.0)
                            gfxOffY = 0.0f;
                    }
                    else if (gfxOffY < 0.0)
                    {
                        gfxOffY += num1 * stepSpeed;
                        if (gfxOffY > 0.0)
                            gfxOffY = 0.0f;
                    }
                    if (gfxOffY > 16.0)
                        gfxOffY = 16f;
                    if (gfxOffY < -16.0)
                        gfxOffY = -16f;
                    Vector2 vector2_1 = velocity;
                    oldVelocity = velocity;
                    whoAmI = i;
                    if (soundDelay > 0)
                        --soundDelay;
                    netUpdate = false;
                    for (int index = 0; index < 255; ++index)
                    {
                        if (playerImmune[index] > 0)
                            --playerImmune[index];
                    }
                    if (updatedNPCImmunity)
                    {
                        for (int index = 0; index < 200; ++index)
                        {
                            if (npcImmune[index] > 0)
                                --npcImmune[index];
                        }
                    }
                    AI();
                    if (owner < 255 && !Main.player[owner].active)
                        Kill();
                    if (type == 242 || type == 302 || type == 638)
                        wet = false;
                    if (!ignoreWater)
                    {
                        bool flag1;
                        bool flag2;
                        try
                        {
                            flag1 = Collision.LavaCollision(position, width, height);
                            flag2 = Collision.WetCollision(position, width, height);
                            if (flag1)
                                lavaWet = true;
                            if (Collision.honey)
                                honeyWet = true;
                        }
                        catch
                        {
                            active = false;
                            return;
                        }
                        if (wet && !lavaWet)
                        {
                            if (type == 85 || type == 15 || (type == 34 || type == 188))
                                Kill();
                            if (type == 2)
                            {
                                type = 1;
                                light = 0.0f;
                            }
                        }
                        if (type == 80)
                        {
                            flag2 = false;
                            wet = false;
                            if (flag1 && ai[0] >= 0.0)
                                Kill();
                        }
                        if (flag2)
                        {
                            if (type != 155 && wetCount == 0 && !wet)
                            {
                                if (!flag1)
                                {
                                    if (honeyWet)
                                    {
                                        for (int index1 = 0; index1 < 10; ++index1)
                                        {
                                            int index2 = Dust.NewDust(new Vector2(position.X - 6f, (float)(position.Y + (height / 2) - 8.0)), width + 12, 24, 152, 0.0f, 0.0f, 0, new Color(), 1f);
                                            --Main.dust[index2].velocity.Y;
                                            Main.dust[index2].velocity.X *= 2.5f;
                                            Main.dust[index2].scale = 1.3f;
                                            Main.dust[index2].alpha = 100;
                                            Main.dust[index2].noGravity = true;
                                        }
                                        Main.PlaySound(19, (int)position.X, (int)position.Y, 1);
                                    }
                                    else
                                    {
                                        for (int index1 = 0; index1 < 10; ++index1)
                                        {
                                            int index2 = Dust.NewDust(new Vector2(position.X - 6f, (float)(position.Y + (height / 2) - 8.0)), width + 12, 24, Dust.dustWater(), 0.0f, 0.0f, 0, new Color(), 1f);
                                            Main.dust[index2].velocity.Y -= 4f;
                                            Main.dust[index2].velocity.X *= 2.5f;
                                            Main.dust[index2].scale = 1.3f;
                                            Main.dust[index2].alpha = 100;
                                            Main.dust[index2].noGravity = true;
                                        }
                                        Main.PlaySound(19, (int)position.X, (int)position.Y, 1);
                                    }
                                }
                                else
                                {
                                    for (int index1 = 0; index1 < 10; ++index1)
                                    {
                                        int index2 = Dust.NewDust(new Vector2(position.X - 6f, (float)(position.Y + (height / 2) - 8.0)), width + 12, 24, 35, 0.0f, 0.0f, 0, new Color(), 1f);
                                        Main.dust[index2].velocity.Y -= 1.5f;
                                        Main.dust[index2].velocity.X *= 2.5f;
                                        Main.dust[index2].scale = 1.3f;
                                        Main.dust[index2].alpha = 100;
                                        Main.dust[index2].noGravity = true;
                                    }
                                    Main.PlaySound(19, (int)position.X, (int)position.Y, 1);
                                }
                            }
                            wet = true;
                        }
                        else if (wet)
                        {
                            wet = false;
                            if (type == 155)
                                velocity.Y *= 0.5f;
                            else if (wetCount == 0)
                            {
                                wetCount = 10;
                                if (!lavaWet)
                                {
                                    if (honeyWet)
                                    {
                                        for (int index1 = 0; index1 < 10; ++index1)
                                        {
                                            int index2 = Dust.NewDust(new Vector2(position.X - 6f, (float)(position.Y + (height / 2) - 8.0)), width + 12, 24, 152, 0.0f, 0.0f, 0, new Color(), 1f);
                                            --Main.dust[index2].velocity.Y;
                                            Main.dust[index2].velocity.X *= 2.5f;
                                            Main.dust[index2].scale = 1.3f;
                                            Main.dust[index2].alpha = 100;
                                            Main.dust[index2].noGravity = true;
                                        }
                                        Main.PlaySound(19, (int)position.X, (int)position.Y, 1);
                                    }
                                    else
                                    {
                                        for (int index1 = 0; index1 < 10; ++index1)
                                        {
                                            int index2 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (height / 2)), width + 12, 24, Dust.dustWater(), 0.0f, 0.0f, 0, new Color(), 1f);
                                            Main.dust[index2].velocity.Y -= 4f;
                                            Main.dust[index2].velocity.X *= 2.5f;
                                            Main.dust[index2].scale = 1.3f;
                                            Main.dust[index2].alpha = 100;
                                            Main.dust[index2].noGravity = true;
                                        }
                                        Main.PlaySound(19, (int)position.X, (int)position.Y, 1);
                                    }
                                }
                                else
                                {
                                    for (int index1 = 0; index1 < 10; ++index1)
                                    {
                                        int index2 = Dust.NewDust(new Vector2(position.X - 6f, (float)(position.Y + (height / 2) - 8.0)), width + 12, 24, 35, 0.0f, 0.0f, 0, new Color(), 1f);
                                        Main.dust[index2].velocity.Y -= 1.5f;
                                        Main.dust[index2].velocity.X *= 2.5f;
                                        Main.dust[index2].scale = 1.3f;
                                        Main.dust[index2].alpha = 100;
                                        Main.dust[index2].noGravity = true;
                                    }
                                    Main.PlaySound(19, (int)position.X, (int)position.Y, 1);
                                }
                            }
                        }
                        if (!wet)
                        {
                            lavaWet = false;
                            honeyWet = false;
                        }
                        if (wetCount > 0)
                            --wetCount;
                    }
                    oldPosition = position;
                    oldDirection = direction;
                    bool flag3 = false;
                    if (tileCollide)
                    {
                        Vector2 vector2_2 = velocity;
                        bool flag1 = true;
                        int num2 = -1;
                        int num3 = -1;
                        if (Main.projPet[type])
                        {
                            flag1 = false;
                            if (Main.player[owner].position.Y + Main.player[owner].height - 12.0 > position.Y + height)
                                flag1 = true;
                        }
                        if (type == 500)
                        {
                            flag1 = false;
                            if (Main.player[owner].Bottom.Y > Bottom.Y + 4.0)
                                flag1 = true;
                        }
                        if (aiStyle == 62)
                            flag1 = true;
                        if (aiStyle == 66)
                            flag1 = true;
                        if (type == 317)
                            flag1 = true;
                        if (type == 373)
                            flag1 = true;
                        if (aiStyle == 53)
                            flag1 = false;
                        if (type == 9 || type == 12 || (type == 15 || type == 13) || (type == 31 || type == 39 || type == 40))
                            flag1 = false;
                        if (type == 24)
                            flag1 = false;
                        if (aiStyle == 29 || type == 28 || aiStyle == 49)
                        {
                            num2 = width - 8;
                            num3 = height - 8;
                        }
                        else if (type == 250 || type == 267 || (type == 297 || type == 323) || type == 3)
                        {
                            num2 = 6;
                            num3 = 6;
                        }
                        else if (type == 308)
                        {
                            num2 = 26;
                            num3 = height;
                        }
                        else if (type == 261 || type == 277)
                        {
                            num2 = 26;
                            num3 = 26;
                        }
                        else if (type == 481 || type == 491 || (type == 106 || type == 262) || (type == 271 || type == 270 || (type == 272 || type == 273)) || (type == 274 || type == 280 || (type == 288 || type == 301) || (type == 320 || type == 333 || (type == 335 || type == 343))) || (type == 344 || type == 497 || (type == 496 || type == 6) || (type == 19 || type == 113 || (type == 520 || type == 523)) || (type == 585 || type == 598 || (type == 599 || type == 636))))
                        {
                            num2 = 10;
                            num3 = 10;
                        }
                        else if (type == 514)
                        {
                            num2 = 4;
                            num3 = 4;
                        }
                        else if (type == 248 || type == 247 || (type == 507 || type == 508))
                        {
                            num2 = width - 12;
                            num3 = height - 12;
                        }
                        else if (aiStyle == 18 || type == 254)
                        {
                            num2 = width - 36;
                            num3 = height - 36;
                        }
                        else if (type == 182 || type == 190 || (type == 33 || type == 229) || (type == 237 || type == 243))
                        {
                            num2 = width - 20;
                            num3 = height - 20;
                        }
                        else if (aiStyle == 27)
                        {
                            num2 = width - 12;
                            num3 = height - 12;
                        }
                        else if (type == 533 && ai[0] >= 6.0)
                        {
                            num2 = width + 6;
                            num3 = height + 6;
                        }
                        else if (type == 582 || type == 634 || type == 635)
                        {
                            num2 = 8;
                            num3 = 8;
                        }
                        else if (type == 617)
                        {
                            num2 = (int)(20.0 * scale);
                            num3 = (int)(20.0 * scale);
                        }
                        if ((type != 440 && type != 449 && type != 606 || ai[1] != 1.0) && ((type != 466 || localAI[1] != 1.0) && (type != 580 || localAI[1] <= 0.0) && (type != 640 || localAI[1] <= 0.0)))
                        {
                            if (aiStyle == 10)
                            {
                                if (type == 42 || type == 65 || (type == 68 || type == 354) || type == 31 && ai[0] == 2.0)
                                    velocity = Collision.TileCollision(position, velocity, width, height, flag1, flag1, 1);
                                else
                                    velocity = Collision.AnyCollision(position, velocity, width, height);
                            }
                            else
                            {
                                Vector2 Position = position;
                                int Width = num2 != -1 ? num2 : width;
                                int Height = num3 != -1 ? num3 : height;
                                if (num3 != -1 || num2 != -1)
                                    Position = new Vector2(position.X + (width / 2) - (Width / 2), position.Y + (height / 2) - (Height / 2));
                                if (wet)
                                {
                                    if (honeyWet)
                                    {
                                        Vector2 vector2_3 = velocity;
                                        velocity = Collision.TileCollision(Position, velocity, Width, Height, flag1, flag1, 1);
                                        vector2_1 = velocity * 0.25f;
                                        if (velocity.X != vector2_3.X)
                                            vector2_1.X = velocity.X;
                                        if (velocity.Y != vector2_3.Y)
                                            vector2_1.Y = velocity.Y;
                                    }
                                    else
                                    {
                                        Vector2 vector2_3 = velocity;
                                        velocity = Collision.TileCollision(Position, velocity, Width, Height, flag1, flag1, 1);
                                        vector2_1 = velocity * 0.5f;
                                        if (velocity.X != vector2_3.X)
                                            vector2_1.X = velocity.X;
                                        if (velocity.Y != vector2_3.Y)
                                            vector2_1.Y = velocity.Y;
                                    }
                                }
                                else
                                {
                                    velocity = Collision.TileCollision(Position, velocity, Width, Height, flag1, flag1, 1);
                                    if (!Main.projPet[type])
                                    {
                                        Vector4 vector4 = Collision.SlopeCollision(Position, velocity, Width, Height, 0.0f, true);
                                        Vector2 vector2_3 = position - Position;
                                        if (Position.X != vector4.X)
                                            flag3 = true;
                                        if (Position.Y != vector4.Y)
                                            flag3 = true;
                                        if (velocity.X != vector4.Z)
                                            flag3 = true;
                                        if (velocity.Y != vector4.W)
                                            flag3 = true;
                                        Position.X = vector4.X;
                                        Position.Y = vector4.Y;
                                        position = Position + vector2_3;
                                        velocity.X = vector4.Z;
                                        velocity.Y = vector4.W;
                                    }
                                }
                            }
                        }
                        if (vector2_2 != velocity)
                            flag3 = true;
                        if (flag3)
                        {
                            if (type == 434)
                            {
                                Projectile projectile = this;
                                Vector2 vector2_3 = projectile.position + velocity;
                                projectile.position = vector2_3;
                                numUpdates = 0;
                            }
                            else if (type == 601)
                            {
                                if (owner == Main.myPlayer)
                                    PortalHelper.TryPlacingPortal(this, vector2_2, velocity);
                                Projectile projectile = this;
                                Vector2 vector2_3 = projectile.position + velocity;
                                projectile.position = vector2_3;
                                Kill();
                            }
                            else if (type == 451)
                            {
                                ai[0] = 1f;
                                ai[1] = 0.0f;
                                netUpdate = true;
                                velocity = vector2_2 / 2f;
                            }
                            else if (type == 645)
                            {
                                ai[0] = 0.0f;
                                ai[1] = -1f;
                                netUpdate = true;
                            }
                            else if (type == 584)
                            {
                                bool flag2 = false;
                                if (velocity.X != vector2_2.X)
                                {
                                    velocity.X = vector2_2.X * -0.75f;
                                    flag2 = true;
                                }
                                if (velocity.Y != vector2_2.Y && vector2_2.Y > 2.0 || velocity.Y == 0.0)
                                {
                                    velocity.Y = vector2_2.Y * -0.75f;
                                    flag2 = true;
                                }
                                if (flag2)
                                {
                                    float num4 = vector2_2.Length() / velocity.Length();
                                    if (num4 == 0.0)
                                        num4 = 1f;
                                    Projectile projectile = this;
                                    Vector2 vector2_3 = projectile.velocity / num4;
                                    projectile.velocity = vector2_3;
                                    --penetrate;
                                }
                            }
                            else if (type == 532)
                            {
                                bool flag2 = false;
                                if (velocity.X != vector2_2.X)
                                {
                                    velocity.X = vector2_2.X * -0.75f;
                                    flag2 = true;
                                }
                                if (velocity.Y != vector2_2.Y && vector2_2.Y > 2.0 || velocity.Y == 0.0)
                                {
                                    velocity.Y = vector2_2.Y * -0.75f;
                                    flag2 = true;
                                }
                                if (flag2)
                                {
                                    float num4 = vector2_2.Length() / velocity.Length();
                                    if (num4 == 0.0)
                                        num4 = 1f;
                                    Projectile projectile = this;
                                    Vector2 vector2_3 = projectile.velocity / num4;
                                    projectile.velocity = vector2_3;
                                    --penetrate;
                                    Collision.HitTiles(position, vector2_2, width, height);
                                }
                            }
                            else if (type == 533)
                            {
                                float num4 = 1f;
                                bool flag2 = false;
                                if (velocity.X != vector2_2.X)
                                {
                                    velocity.X = vector2_2.X * -num4;
                                    flag2 = true;
                                }
                                if (velocity.Y != vector2_2.Y || velocity.Y == 0.0)
                                {
                                    velocity.Y = (float)(vector2_2.Y * -num4 * 0.5);
                                    flag2 = true;
                                }
                                if (flag2)
                                {
                                    float num5 = vector2_2.Length() / velocity.Length();
                                    if (num5 == 0.0)
                                        num5 = 1f;
                                    Projectile projectile = this;
                                    Vector2 vector2_3 = projectile.velocity / num5;
                                    projectile.velocity = vector2_3;
                                    if (ai[0] == 7.0 && velocity.Y < -0.1)
                                        velocity.Y += 0.1f;
                                    if (ai[0] >= 6.0 && ai[0] < 9.0)
                                        Collision.HitTiles(position, vector2_2, width, height);
                                }
                            }
                            else if (type == 502)
                            {
                                ++ai[0];
                                Main.PlaySound(37, (int)position.X, (int)position.Y, 5 + (int)ai[0]);
                                if (ai[0] >= 5.0)
                                {
                                    Projectile projectile = this;
                                    Vector2 vector2_3 = projectile.position + velocity;
                                    projectile.position = vector2_3;
                                    Kill();
                                }
                                else
                                {
                                    if (velocity.Y != vector2_2.Y)
                                        velocity.Y = -vector2_2.Y;
                                    if (velocity.X != vector2_2.X)
                                        velocity.X = -vector2_2.X;
                                }
                                Vector2 spinningpoint = Utils.RotatedByRandom(new Vector2(0.0f, -3f - ai[0]), 3.14159274101257);
                                float num4 = (float)(10.0 + ai[0] * 4.0);
                                Vector2 vector2_4 = new Vector2(1.05f, 1f);
                                for (float num5 = 0.0f; num5 < num4; ++num5)
                                {
                                    int index = Dust.NewDust(Center, 0, 0, 66, 0.0f, 0.0f, 0, Color.Transparent, 1f);
                                    Main.dust[index].position = Center;
                                    Main.dust[index].velocity = Utils.RotatedBy(spinningpoint, 6.28318548202515 * num5 / num4, new Vector2()) * vector2_4 * (float)(0.800000011920929 + Utils.NextFloat(Main.rand) * 0.400000005960464);
                                    Main.dust[index].color = Main.hslToRgb(num5 / num4, 1f, 0.5f);
                                    Main.dust[index].noGravity = true;
                                    Main.dust[index].scale = (float)(1.0 + ai[0] / 3.0);
                                }
                                if (Main.myPlayer == owner)
                                {
                                    int num5 = width;
                                    int num6 = height;
                                    int num7 = penetrate;
                                    position = Center;
                                    width = height = 40 + 8 * (int)ai[0];
                                    Center = position;
                                    penetrate = -1;
                                    Damage();
                                    penetrate = num7;
                                    position = Center;
                                    width = num5;
                                    height = num6;
                                    Center = position;
                                }
                            }
                            else if (type == 444)
                            {
                                if (velocity.X != vector2_2.X)
                                    velocity.X = -vector2_2.X;
                                if (velocity.Y != vector2_2.Y)
                                    velocity.Y = -vector2_2.Y;
                                ai[0] = Utils.ToRotation(velocity);
                            }
                            else if (type == 617)
                            {
                                if (velocity.X != vector2_2.X)
                                    velocity.X = (float)(-vector2_2.X * 0.349999994039536);
                                if (velocity.Y != vector2_2.Y)
                                    velocity.Y = (float)(-vector2_2.Y * 0.349999994039536);
                            }
                            else if (type == 440 || type == 449 || type == 606)
                            {
                                if (ai[1] != 1.0)
                                {
                                    ai[1] = 1f;
                                    Projectile projectile = this;
                                    Vector2 vector2_3 = projectile.position + velocity;
                                    projectile.position = vector2_3;
                                    velocity = vector2_2;
                                }
                            }
                            else if (type == 466 || type == 580 || type == 640)
                            {
                                if (localAI[1] < 1.0)
                                {
                                    localAI[1] += 2f;
                                    Projectile projectile = this;
                                    Vector2 vector2_3 = projectile.position + velocity;
                                    projectile.position = vector2_3;
                                    velocity = Vector2.Zero;
                                }
                            }
                            else if (aiStyle == 54)
                            {
                                if (velocity.X != vector2_2.X)
                                    velocity.X = vector2_2.X * -0.6f;
                                if (velocity.Y != vector2_2.Y)
                                    velocity.Y = vector2_2.Y * -0.6f;
                            }
                            else if (!Main.projPet[type] && type != 500 && type != 650)
                            {
                                if (aiStyle == 99)
                                {
                                    if (type >= 556 && type <= 561)
                                    {
                                        bool flag2 = false;
                                        if (velocity.X != (double)oldVelocity.X)
                                        {
                                            flag2 = true;
                                            velocity.X = oldVelocity.X * -1f;
                                        }
                                        if (velocity.Y != (double)oldVelocity.Y)
                                        {
                                            flag2 = true;
                                            velocity.Y = oldVelocity.Y * -1f;
                                        }
                                        if (flag2)
                                        {
                                            Vector2 vector2_3 = Main.player[owner].Center - Center;
                                            vector2_3.Normalize();
                                            Vector2 vector2_4 = vector2_3 * velocity.Length() * 0.25f;
                                            Projectile projectile1 = this;
                                            Vector2 vector2_5 = projectile1.velocity * 0.75f;
                                            projectile1.velocity = vector2_5;
                                            Projectile projectile2 = this;
                                            Vector2 vector2_6 = projectile2.velocity + vector2_4;
                                            projectile2.velocity = vector2_6;
                                            if (velocity.Length() > 6.0)
                                            {
                                                Projectile projectile3 = this;
                                                Vector2 vector2_7 = projectile3.velocity * 0.5f;
                                                projectile3.velocity = vector2_7;
                                            }
                                        }
                                    }
                                }
                                else if (type == 604)
                                {
                                    if (velocity.X != vector2_2.X)
                                        velocity.X = -vector2_2.X;
                                    if (velocity.Y != vector2_2.Y)
                                        velocity.Y = -vector2_2.Y;
                                }
                                else if (type == 379)
                                {
                                    if (velocity.X != vector2_2.X)
                                        velocity.X = vector2_2.X * -0.6f;
                                    if (velocity.Y != vector2_2.Y && vector2_2.Y > 2.0)
                                        velocity.Y = vector2_2.Y * -0.6f;
                                }
                                else if (type == 491)
                                {
                                    if (ai[0] <= 0.0)
                                        ai[0] = -10f;
                                    if (velocity.X != vector2_2.X && Math.Abs(vector2_2.X) > 0.0)
                                        velocity.X = vector2_2.X * -1f;
                                    if (velocity.Y != vector2_2.Y && Math.Abs(vector2_2.Y) > 0.0)
                                        velocity.Y = vector2_2.Y * -1f;
                                }
                                else if (type >= 515 && type <= 517 || type == 637)
                                {
                                    if (velocity.X != vector2_2.X && Math.Abs(vector2_2.X) > 1.0)
                                        velocity.X = vector2_2.X * -0.9f;
                                    if (velocity.Y != vector2_2.Y && Math.Abs(vector2_2.Y) > 1.0)
                                        velocity.Y = vector2_2.Y * -0.9f;
                                }
                                else if (type == 409)
                                {
                                    if (velocity.X != vector2_2.X)
                                        velocity.X = vector2_2.X * -1f;
                                    if (velocity.Y != vector2_2.Y)
                                        velocity.Y = vector2_2.Y * -1f;
                                }
                                else if (type == 254)
                                {
                                    tileCollide = false;
                                    velocity = vector2_2;
                                    if (timeLeft > 30)
                                        timeLeft = 30;
                                }
                                else if (type == 225 && penetrate > 0)
                                {
                                    velocity.X = -vector2_2.X;
                                    velocity.Y = -vector2_2.Y;
                                    --penetrate;
                                }
                                else if (type == 155)
                                {
                                    if (ai[1] > 10.0)
                                    {
                                        string str = string.Concat(new object[4]
                    {
                       name,
                       " was hit ",
                       ai[1],
                       " times before touching the ground!"
                    });
                                        if (Main.netMode == 0)
                                            Main.NewText(str, byte.MaxValue, 240, 20, false);
                                        else if (Main.netMode == 2)
                                            NetMessage.SendData(25, -1, -1, str, 255, 255, 240f, 20f, 0, 0, 0);
                                    }
                                    ai[1] = 0.0f;
                                    if (velocity.X != vector2_2.X)
                                        velocity.X = vector2_2.X * -0.6f;
                                    if (velocity.Y != vector2_2.Y && vector2_2.Y > 2.0)
                                        velocity.Y = vector2_2.Y * -0.6f;
                                }
                                else if (aiStyle == 33)
                                {
                                    if (localAI[0] == 0.0)
                                    {
                                        if (wet)
                                        {
                                            Projectile projectile = this;
                                            Vector2 vector2_3 = projectile.position + vector2_2 / 2f;
                                            projectile.position = vector2_3;
                                        }
                                        else
                                        {
                                            Projectile projectile = this;
                                            Vector2 vector2_3 = projectile.position + vector2_2;
                                            projectile.position = vector2_3;
                                        }
                                        Projectile projectile1 = this;
                                        Vector2 vector2_4 = projectile1.velocity * 0.0f;
                                        projectile1.velocity = vector2_4;
                                        localAI[0] = 1f;
                                    }
                                }
                                else if (type != 308)
                                {
                                    if (type == 477)
                                    {
                                        if (velocity.Y != vector2_2.Y || velocity.X != vector2_2.X)
                                        {
                                            --penetrate;
                                            if (penetrate <= 0)
                                                Kill();
                                            if (velocity.X != vector2_2.X)
                                                velocity.X = -vector2_2.X;
                                            if (velocity.Y != vector2_2.Y)
                                                velocity.Y = -vector2_2.Y;
                                        }
                                        if (penetrate > 0 && owner == Main.myPlayer)
                                        {
                                            int[] numArray = new int[10];
                                            int maxValue = 0;
                                            int num4 = 700;
                                            int num5 = 20;
                                            for (int index = 0; index < 200; ++index)
                                            {
                                                if (Main.npc[index].CanBeChasedBy(this, false))
                                                {
                                                    float num6 = (Center - Main.npc[index].Center).Length();
                                                    if (num6 > num5 && num6 < num4 && Collision.CanHitLine(Center, 1, 1, Main.npc[index].Center, 1, 1))
                                                    {
                                                        numArray[maxValue] = index;
                                                        ++maxValue;
                                                        if (maxValue >= 9)
                                                            break;
                                                    }
                                                }
                                            }
                                            if (maxValue > 0)
                                            {
                                                int index = Main.rand.Next(maxValue);
                                                Vector2 vector2_3 = Main.npc[numArray[index]].Center - Center;
                                                float num6 = velocity.Length();
                                                vector2_3.Normalize();
                                                velocity = vector2_3 * num6;
                                                netUpdate = true;
                                            }
                                        }
                                    }
                                    else if (type == 94 || type == 496)
                                    {
                                        if (velocity.X != vector2_2.X)
                                        {
                                            if (Math.Abs(velocity.X) < 1.0)
                                                velocity.X = -vector2_2.X;
                                            else
                                                Kill();
                                        }
                                        if (velocity.Y != vector2_2.Y)
                                        {
                                            if (Math.Abs(velocity.Y) < 1.0)
                                                velocity.Y = -vector2_2.Y;
                                            else
                                                Kill();
                                        }
                                    }
                                    else if (type == 311)
                                    {
                                        if (velocity.X != vector2_2.X)
                                        {
                                            velocity.X = -vector2_2.X;
                                            ++ai[1];
                                        }
                                        if (velocity.Y != vector2_2.Y)
                                        {
                                            velocity.Y = -vector2_2.Y;
                                            ++ai[1];
                                        }
                                        if (ai[1] > 4.0)
                                            Kill();
                                    }
                                    else if (type == 312)
                                    {
                                        if (velocity.X != vector2_2.X)
                                        {
                                            velocity.X = -vector2_2.X;
                                            ++ai[1];
                                        }
                                        if (velocity.Y != vector2_2.Y)
                                        {
                                            velocity.Y = -vector2_2.Y;
                                            ++ai[1];
                                        }
                                    }
                                    else if (type == 522 || type == 620)
                                    {
                                        if (velocity.X != vector2_2.X)
                                            velocity.X = -vector2_2.X;
                                        if (velocity.Y != vector2_2.Y)
                                            velocity.Y = -vector2_2.Y;
                                    }
                                    else if (type == 524)
                                    {
                                        ai[0] += 100f;
                                        if (velocity.X != vector2_2.X)
                                            velocity.X = -vector2_2.X;
                                        if (velocity.Y != vector2_2.Y)
                                            velocity.Y = -vector2_2.Y;
                                    }
                                    else if (aiStyle == 93)
                                    {
                                        if (velocity != vector2_2)
                                        {
                                            ai[1] = 0.0f;
                                            ai[0] = 1f;
                                            netUpdate = true;
                                            tileCollide = false;
                                            Projectile projectile1 = this;
                                            Vector2 vector2_3 = projectile1.position + velocity;
                                            projectile1.position = vector2_3;
                                            velocity = vector2_2;
                                            velocity.Normalize();
                                            Projectile projectile2 = this;
                                            Vector2 vector2_4 = projectile2.velocity * 3f;
                                            projectile2.velocity = vector2_4;
                                        }
                                    }
                                    else if (type == 281)
                                    {
                                        if ((Math.Abs(velocity.X) + Math.Abs(velocity.Y)) < 2.0 || ai[1] == 2.0)
                                        {
                                            ai[1] = 2f;
                                        }
                                        else
                                        {
                                            if (velocity.X != vector2_2.X)
                                                velocity.X = (float)(-vector2_2.X * 0.5);
                                            if (velocity.Y != vector2_2.Y)
                                                velocity.Y = (float)(-vector2_2.Y * 0.5);
                                        }
                                    }
                                    else if (type == 290 || type == 294)
                                    {
                                        if (velocity.X != vector2_2.X)
                                        {
                                            position.X += velocity.X;
                                            velocity.X = -vector2_2.X;
                                        }
                                        if (velocity.Y != vector2_2.Y)
                                        {
                                            position.Y += velocity.Y;
                                            velocity.Y = -vector2_2.Y;
                                        }
                                    }
                                    else if ((type == 181 || type == 189 || (type == 357 || type == 566)) && penetrate > 0)
                                    {
                                        if (type == 357)
                                            damage = (int)(damage * 0.9);
                                        --penetrate;
                                        if (velocity.X != vector2_2.X)
                                            velocity.X = -vector2_2.X;
                                        if (velocity.Y != vector2_2.Y)
                                            velocity.Y = -vector2_2.Y;
                                    }
                                    else if (type == 307 && ai[1] < 5.0)
                                    {
                                        ++ai[1];
                                        if (velocity.X != vector2_2.X)
                                            velocity.X = -vector2_2.X;
                                        if (velocity.Y != vector2_2.Y)
                                            velocity.Y = -vector2_2.Y;
                                    }
                                    else if (type == 99)
                                    {
                                        if (velocity.Y != vector2_2.Y && vector2_2.Y > 5.0)
                                        {
                                            Collision.HitTiles(position, velocity, width, height);
                                            Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                                            velocity.Y = (float)(-vector2_2.Y * 0.200000002980232);
                                        }
                                        if (velocity.X != vector2_2.X)
                                            Kill();
                                    }
                                    else if (type == 36)
                                    {
                                        if (penetrate > 1)
                                        {
                                            Collision.HitTiles(position, velocity, width, height);
                                            Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                                            --penetrate;
                                            if (velocity.X != vector2_2.X)
                                                velocity.X = -vector2_2.X;
                                            if (velocity.Y != vector2_2.Y)
                                                velocity.Y = -vector2_2.Y;
                                        }
                                        else
                                            Kill();
                                    }
                                    else if (aiStyle == 21)
                                    {
                                        if (velocity.X != vector2_2.X)
                                            velocity.X = -vector2_2.X;
                                        if (velocity.Y != vector2_2.Y)
                                            velocity.Y = -vector2_2.Y;
                                    }
                                    else if (aiStyle == 17)
                                    {
                                        if (velocity.X != vector2_2.X)
                                            velocity.X = vector2_2.X * -0.75f;
                                        if (velocity.Y != vector2_2.Y && vector2_2.Y > 1.5)
                                            velocity.Y = vector2_2.Y * -0.7f;
                                    }
                                    else if (aiStyle == 15)
                                    {
                                        bool flag2 = false;
                                        if (vector2_2.X != velocity.X)
                                        {
                                            if (Math.Abs(vector2_2.X) > 4.0)
                                                flag2 = true;
                                            position.X += velocity.X;
                                            velocity.X = (float)(-vector2_2.X * 0.200000002980232);
                                        }
                                        if (vector2_2.Y != velocity.Y)
                                        {
                                            if (Math.Abs(vector2_2.Y) > 4.0)
                                                flag2 = true;
                                            position.Y += velocity.Y;
                                            velocity.Y = (float)(-vector2_2.Y * 0.200000002980232);
                                        }
                                        ai[0] = 1f;
                                        if (flag2)
                                        {
                                            netUpdate = true;
                                            Collision.HitTiles(position, velocity, width, height);
                                            Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                                        }
                                        if (wet)
                                            vector2_1 = velocity;
                                    }
                                    else if (aiStyle == 39)
                                    {
                                        Collision.HitTiles(position, velocity, width, height);
                                        if (type == 33 || type == 106)
                                        {
                                            if (velocity.X != vector2_2.X)
                                                velocity.X = -vector2_2.X;
                                            if (velocity.Y != vector2_2.Y)
                                                velocity.Y = -vector2_2.Y;
                                        }
                                        else
                                        {
                                            ai[0] = 1f;
                                            if (aiStyle == 3)
                                            {
                                                velocity.X = -vector2_2.X;
                                                velocity.Y = -vector2_2.Y;
                                            }
                                        }
                                        netUpdate = true;
                                        Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                                    }
                                    else if (aiStyle == 3 || aiStyle == 13 || (aiStyle == 69 || aiStyle == 109))
                                    {
                                        Collision.HitTiles(position, velocity, width, height);
                                        if (type == 33 || type == 106)
                                        {
                                            if (velocity.X != vector2_2.X)
                                                velocity.X = -vector2_2.X;
                                            if (velocity.Y != vector2_2.Y)
                                                velocity.Y = -vector2_2.Y;
                                        }
                                        else
                                        {
                                            ai[0] = 1f;
                                            if ((aiStyle == 3 || aiStyle == 109) && type != 383)
                                            {
                                                velocity.X = -vector2_2.X;
                                                velocity.Y = -vector2_2.Y;
                                            }
                                        }
                                        netUpdate = true;
                                        Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                                    }
                                    else if (aiStyle == 8 && type != 96)
                                    {
                                        Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                                        ++ai[0];
                                        if (ai[0] >= 5.0 && type != 253 || type == 253 && ai[0] >= 8.0)
                                        {
                                            Projectile projectile = this;
                                            Vector2 vector2_3 = projectile.position + velocity;
                                            projectile.position = vector2_3;
                                            Kill();
                                        }
                                        else
                                        {
                                            if (type == 15 && velocity.Y > 4.0)
                                            {
                                                if (velocity.Y != vector2_2.Y)
                                                    velocity.Y = (float)(-vector2_2.Y * 0.800000011920929);
                                            }
                                            else if (velocity.Y != vector2_2.Y)
                                                velocity.Y = -vector2_2.Y;
                                            if (velocity.X != vector2_2.X)
                                                velocity.X = -vector2_2.X;
                                        }
                                    }
                                    else if (aiStyle == 61)
                                    {
                                        if (velocity.X != vector2_2.X)
                                            velocity.X = vector2_2.X * -0.3f;
                                        if (velocity.Y != vector2_2.Y && vector2_2.Y > 1.0)
                                            velocity.Y = vector2_2.Y * -0.3f;
                                    }
                                    else if (aiStyle == 14)
                                    {
                                        if (type == 261 && (velocity.X != vector2_2.X && (vector2_2.X < -3.0 || vector2_2.X > 3.0) || velocity.Y != vector2_2.Y && (vector2_2.Y < -3.0 || vector2_2.Y > 3.0)))
                                        {
                                            Collision.HitTiles(position, velocity, width, height);
                                            Main.PlaySound(0, (int)Center.X, (int)Center.Y, 1);
                                        }
                                        if (type >= 326 && type <= 328 && velocity.X != vector2_2.X)
                                            velocity.X = vector2_2.X * -0.1f;
                                        if (type >= 400 && type <= 402)
                                        {
                                            if (velocity.X != vector2_2.X)
                                                velocity.X = vector2_2.X * -0.1f;
                                        }
                                        else if (type == 50)
                                        {
                                            if (velocity.X != vector2_2.X)
                                                velocity.X = vector2_2.X * -0.2f;
                                            if (velocity.Y != vector2_2.Y && vector2_2.Y > 1.5)
                                                velocity.Y = vector2_2.Y * -0.2f;
                                        }
                                        else if (type == 185)
                                        {
                                            if (velocity.X != vector2_2.X)
                                                velocity.X = vector2_2.X * -0.9f;
                                            if (velocity.Y != vector2_2.Y && vector2_2.Y > 1.0)
                                                velocity.Y = vector2_2.Y * -0.9f;
                                        }
                                        else if (type == 277)
                                        {
                                            if (velocity.X != vector2_2.X)
                                                velocity.X = vector2_2.X * -0.9f;
                                            if (velocity.Y != vector2_2.Y && vector2_2.Y > 3.0)
                                                velocity.Y = vector2_2.Y * -0.9f;
                                        }
                                        else if (type != 480)
                                        {
                                            if (type == 450)
                                            {
                                                if (velocity.X != vector2_2.X)
                                                    velocity.X = vector2_2.X * -0.1f;
                                            }
                                            else
                                            {
                                                if (velocity.X != vector2_2.X)
                                                    velocity.X = vector2_2.X * -0.5f;
                                                if (velocity.Y != vector2_2.Y && vector2_2.Y > 1.0)
                                                    velocity.Y = vector2_2.Y * -0.5f;
                                            }
                                        }
                                    }
                                    else if (aiStyle == 16)
                                    {
                                        if (velocity.X != vector2_2.X)
                                        {
                                            velocity.X = vector2_2.X * -0.4f;
                                            if (type == 29)
                                                velocity.X = velocity.X * 0.8f;
                                        }
                                        if (velocity.Y != vector2_2.Y && vector2_2.Y > 0.7 && type != 102)
                                        {
                                            velocity.Y = vector2_2.Y * -0.4f;
                                            if (type == 29)
                                                velocity.Y = velocity.Y * 0.8f;
                                        }
                                        if (type == 134 || type == 137 || (type == 140 || type == 143) || (type == 303 || type >= 338 && type <= 341))
                                        {
                                            Projectile projectile = this;
                                            Vector2 vector2_3 = projectile.velocity * 0.0f;
                                            projectile.velocity = vector2_3;
                                            alpha = 255;
                                            timeLeft = 3;
                                        }
                                    }
                                    else if (aiStyle == 68)
                                    {
                                        Projectile projectile = this;
                                        Vector2 vector2_3 = projectile.velocity * 0.0f;
                                        projectile.velocity = vector2_3;
                                        alpha = 255;
                                        timeLeft = 3;
                                    }
                                    else if (aiStyle != 9 || owner == Main.myPlayer)
                                    {
                                        Projectile projectile = this;
                                        Vector2 vector2_3 = projectile.position + velocity;
                                        projectile.position = vector2_3;
                                        Kill();
                                    }
                                }
                            }
                        }
                    }
                    if (aiStyle != 4 && aiStyle != 38 && aiStyle != 84 && (aiStyle != 7 || ai[0] != 2.0) && (type != 440 && type != 449 && type != 606 || ai[1] != 1.0) && ((aiStyle != 93 || ai[0] >= 0.0) && type != 540))
                    {
                        if (wet)
                        {
                            Projectile projectile = this;
                            Vector2 vector2_2 = projectile.position + vector2_1;
                            projectile.position = vector2_2;
                        }
                        else
                        {
                            Projectile projectile = this;
                            Vector2 vector2_2 = projectile.position + velocity;
                            projectile.position = vector2_2;
                        }
                        if (Main.projPet[type] && tileCollide)
                        {
                            Vector4 vector4 = Collision.SlopeCollision(position, velocity, width, height, 0.0f, false);
                            position.X = vector4.X;
                            position.Y = vector4.Y;
                            velocity.X = vector4.Z;
                            velocity.Y = vector4.W;
                        }
                    }
                    if ((aiStyle != 3 || ai[0] != 1.0) && (aiStyle != 7 || ai[0] != 1.0) && ((aiStyle != 13 || ai[0] != 1.0) && (aiStyle != 65 && aiStyle != 69)) && (aiStyle != 114 && aiStyle != 123 && (aiStyle != 112 && !manualDirectionChange) && (aiStyle != 67 && aiStyle != 26 && aiStyle != 15)))
                    {
                        if (velocity.X < 0.0)
                            direction = -1;
                        else
                            direction = 1;
                    }
                    if (!active)
                        return;
                    ProjLight();
                    if (!npcProj && friendly && (Main.player[owner].magicQuiver && extraUpdates < 1) && arrow)
                        extraUpdates = 1;
                    if (type == 2 || type == 82)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1f);
                    else if (type == 172)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 135, 0.0f, 0.0f, 100, new Color(), 1f);
                    else if (type == 103)
                    {
                        int index = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 75, 0.0f, 0.0f, 100, new Color(), 1f);
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dust[index].noGravity = true;
                            Main.dust[index].scale *= 2f;
                        }
                    }
                    else if (type == 278)
                    {
                        int index = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 169, 0.0f, 0.0f, 100, new Color(), 1f);
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dust[index].noGravity = true;
                            Main.dust[index].scale *= 1.5f;
                        }
                    }
                    else if (type == 4)
                    {
                        if (Main.rand.Next(5) == 0)
                            Dust.NewDust(new Vector2(position.X, position.Y), width, height, 14, 0.0f, 0.0f, 150, new Color(), 1.1f);
                    }
                    else if (type == 5)
                    {
                        int Type;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                Type = 15;
                                break;
                            case 1:
                                Type = 57;
                                break;
                            default:
                                Type = 58;
                                break;
                        }
                        Dust.NewDust(position, width, height, Type, velocity.X * 0.5f, velocity.Y * 0.5f, 150, new Color(), 1.2f);
                    }
                    Damage();
                    if (type == 434 && localAI[0] == 0.0 && numUpdates == 0)
                    {
                        extraUpdates = 1;
                        velocity = Vector2.Zero;
                        localAI[0] = 1f;
                        localAI[1] = 0.9999f;
                        netUpdate = true;
                    }
                    if (Main.netMode != 1 && type == 99)
                        Collision.SwitchTiles(position, width, height, oldPosition, 3);
                    if (ProjectileID.Sets.TrailingMode[type] == 0)
                    {
                        for (int index = oldPos.Length - 1; index > 0; --index)
                            oldPos[index] = oldPos[index - 1];
                        oldPos[0] = position;
                    }
                    else if (ProjectileID.Sets.TrailingMode[type] == 1)
                    {
                        if (frameCounter == 0 || oldPos[0] == Vector2.Zero)
                        {
                            for (int index = oldPos.Length - 1; index > 0; --index)
                                oldPos[index] = oldPos[index - 1];
                            oldPos[0] = position;
                            if (velocity == Vector2.Zero && type == 466)
                            {
                                float num2 = (float)(rotation + 1.57079637050629 + (Main.rand.Next(2) == 1 ? -1.0 : 1.0) * 1.57079637050629);
                                float num3 = (float)(Main.rand.NextDouble() * 2.0 + 2.0);
                                Vector2 vector2_2 = new Vector2((float)Math.Cos(num2) * num3, (float)Math.Sin(num2) * num3);
                                int index = Dust.NewDust(oldPos[oldPos.Length - 1], 0, 0, 229, vector2_2.X, vector2_2.Y, 0, new Color(), 1f);
                                Main.dust[index].noGravity = true;
                                Main.dust[index].scale = 1.7f;
                            }
                            if (velocity == Vector2.Zero && type == 580)
                            {
                                float num2 = (float)(rotation + 1.57079637050629 + (Main.rand.Next(2) == 1 ? -1.0 : 1.0) * 1.57079637050629);
                                float num3 = (float)(Main.rand.NextDouble() * 2.0 + 2.0);
                                Vector2 vector2_2 = new Vector2((float)Math.Cos(num2) * num3, (float)Math.Sin(num2) * num3);
                                int index = Dust.NewDust(oldPos[oldPos.Length - 1], 0, 0, 229, vector2_2.X, vector2_2.Y, 0, new Color(), 1f);
                                Main.dust[index].noGravity = true;
                                Main.dust[index].scale = 1.7f;
                            }
                        }
                    }
                    else if (ProjectileID.Sets.TrailingMode[type] == 2)
                    {
                        for (int index = oldPos.Length - 1; index > 0; --index)
                        {
                            oldPos[index] = oldPos[index - 1];
                            oldRot[index] = oldRot[index - 1];
                            oldSpriteDirection[index] = oldSpriteDirection[index - 1];
                        }
                        oldPos[0] = position;
                        oldRot[0] = rotation;
                        oldSpriteDirection[0] = spriteDirection;
                    }
                    --timeLeft;
                    if (timeLeft <= 0)
                        Kill();
                    if (penetrate == 0)
                        Kill();
                    if (active && owner == Main.myPlayer)
                    {
                        if (netUpdate2)
                            netUpdate = true;
                        if (!active)
                            netSpam = 0;
                        if (netUpdate)
                        {
                            if (netSpam < 60)
                            {
                                netSpam += 5;
                                NetMessage.SendData(27, -1, -1, "", i, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                                netUpdate2 = false;
                            }
                            else
                                netUpdate2 = true;
                        }
                        if (netSpam > 0)
                            --netSpam;
                    }
                }
            }
            netUpdate = false;
        }

        public void FishingCheck()
        {
            int index = (int)(Center.X / 16.0);
            int j1 = (int)(Center.Y / 16.0);
            if (Main.tile[index, j1].liquid < 0)
                ++j1;
            bool flag1 = false;
            bool flag2 = false;
            int i1 = index;
            int i2 = index;
            while (i1 > 10 && Main.tile[i1, j1].liquid > 0 && !WorldGen.SolidTile(i1, j1))
                --i1;
            while (i2 < Main.maxTilesX - 10 && Main.tile[i2, j1].liquid > 0 && !WorldGen.SolidTile(i2, j1))
                ++i2;
            int num1 = 0;
            for (int i3 = i1; i3 <= i2; ++i3)
            {
                int j2 = j1;
                while (Main.tile[i3, j2].liquid > 0 && !WorldGen.SolidTile(i3, j2) && j2 < Main.maxTilesY - 10)
                {
                    ++num1;
                    ++j2;
                    if (Main.tile[i3, j2].lava())
                        flag1 = true;
                    else if (Main.tile[i3, j2].honey())
                        flag2 = true;
                }
            }
            if (flag2)
                num1 = (int)(num1 * 1.5);
            if (num1 < 75)
            {
                Main.player[owner].displayedFishingInfo = "Not Enough Water!";
            }
            else
            {
                int num2 = Main.player[owner].FishingLevel();
                if (num2 == 0)
                    return;
                Main.player[owner].displayedFishingInfo = (string)(object)num2 + (object)" Fishing Power";
                if (num2 < 0)
                {
                    if (num2 != -1)
                        return;
                    Main.player[owner].displayedFishingInfo = "Warning!";
                    if (index >= 380 && index <= Main.maxTilesX - 380 || (num1 <= 1000 || NPC.AnyNPCs(370)))
                        return;
                    ai[1] = (Main.rand.Next(-180, -60) - 100);
                    localAI[1] = num2;
                    netUpdate = true;
                }
                else
                {
                    int num3 = 300;
                    float num4 = (Main.maxTilesX / 4200);
                    float num5 = (float)((position.Y / 16.0 - (60.0 + 10.0 * (num4 * num4))) / (Main.worldSurface / 6.0));
                    if (num5 < 0.25)
                        num5 = 0.25f;
                    if (num5 > 1.0)
                        num5 = 1f;
                    int num6 = (int)(num3 * num5);
                    float num7 = num1 / num6;
                    if (num7 < 1.0)
                        num2 = (int)(num2 * num7);
                    float num8 = 1f - num7;
                    if (num1 < num6)
                        Main.player[owner].displayedFishingInfo = string.Concat(new object[4]
            {
               num2,
               " (",
               -Math.Round(num8 * 100.0),
               "%) Fishing Power"
            });
                    int num9 = (num2 + 75) / 2;
                    if (Main.player[owner].wet || Main.rand.Next(100) > num9)
                        return;
                    int Type = 0;
                    int num10 = j1 >= Main.worldSurface * 0.5 ? (j1 >= Main.worldSurface ? (j1 >= Main.rockLayer ? (j1 >= Main.maxTilesY - 300 ? 4 : 3) : 2) : 1) : 0;
                    int num11 = 150;
                    int maxValue1 = num11 / num2;
                    int maxValue2 = num11 * 2 / num2;
                    int maxValue3 = num11 * 7 / num2;
                    int maxValue4 = num11 * 15 / num2;
                    int maxValue5 = num11 * 30 / num2;
                    if (maxValue1 < 2)
                        maxValue1 = 2;
                    if (maxValue2 < 3)
                        maxValue2 = 3;
                    if (maxValue3 < 4)
                        maxValue3 = 4;
                    if (maxValue4 < 5)
                        maxValue4 = 5;
                    if (maxValue5 < 6)
                        maxValue5 = 6;
                    bool flag3 = false;
                    bool flag4 = false;
                    bool flag5 = false;
                    bool flag6 = false;
                    bool flag7 = false;
                    if (Main.rand.Next(maxValue1) == 0)
                        flag3 = true;
                    if (Main.rand.Next(maxValue2) == 0)
                        flag4 = true;
                    if (Main.rand.Next(maxValue3) == 0)
                        flag5 = true;
                    if (Main.rand.Next(maxValue4) == 0)
                        flag6 = true;
                    if (Main.rand.Next(maxValue5) == 0)
                        flag7 = true;
                    int num12 = 10;
                    if (Main.player[owner].cratePotion)
                        num12 += 10;
                    int type = Main.anglerQuestItemNetIDs[Main.anglerQuest];
                    if (Main.player[owner].HasItem(type))
                        type = -1;
                    if (Main.anglerQuestFinished)
                        type = -1;
                    if (flag1)
                    {
                        if (Main.player[owner].inventory[Main.player[owner].selectedItem].type != 2422)
                            return;
                        if (flag7)
                            Type = 2331;
                        else if (flag6)
                            Type = 2312;
                        else if (flag5)
                            Type = 2315;
                    }
                    else if (flag2)
                    {
                        if (flag5 || flag4 && Main.rand.Next(2) == 0)
                            Type = 2314;
                        else if (flag4 && type == 2451)
                            Type = 2451;
                    }
                    else if (Main.rand.Next(50) > num2 && Main.rand.Next(50) > num2 && num1 < num6)
                        Type = Main.rand.Next(2337, 2340);
                    else if (Main.rand.Next(100) < num12)
                        Type = flag6 || flag7 ? 2336 : (!flag5 || !Main.player[owner].ZoneCorrupt ? (!flag5 || !Main.player[owner].ZoneCrimson ? (!flag5 || !Main.player[owner].ZoneHoly ? (!flag5 || !Main.player[owner].ZoneDungeon ? (!flag5 || !Main.player[owner].ZoneJungle ? (!flag5 || num10 != 0 ? (!flag4 ? 2334 : 2335) : 3206) : 3208) : 3205) : 3207) : 3204) : 3203);
                    else if (flag7 && Main.rand.Next(5) == 0)
                        Type = 2423;
                    else if (flag7 && Main.rand.Next(5) == 0)
                        Type = 3225;
                    else if (flag7 && Main.rand.Next(10) == 0)
                        Type = 2420;
                    else if (!flag7 && !flag6 && (flag4 && Main.rand.Next(5) == 0))
                    {
                        Type = 3196;
                    }
                    else
                    {
                        bool flag8 = Main.player[owner].ZoneCorrupt;
                        bool flag9 = Main.player[owner].ZoneCrimson;
                        if (flag8 && flag9)
                        {
                            if (Main.rand.Next(2) == 0)
                                flag9 = false;
                            else
                                flag8 = false;
                        }
                        if (flag8)
                        {
                            if (flag7 && Main.hardMode && (Main.player[owner].ZoneSnow && num10 == 3) && Main.rand.Next(3) != 0)
                                Type = 2429;
                            else if (flag7 && Main.hardMode && Main.rand.Next(2) == 0)
                                Type = 3210;
                            else if (flag5)
                                Type = 2330;
                            else if (flag4 && type == 2454)
                                Type = 2454;
                            else if (flag4 && type == 2485)
                                Type = 2485;
                            else if (flag4 && type == 2457)
                                Type = 2457;
                            else if (flag4)
                                Type = 2318;
                        }
                        else if (flag9)
                        {
                            if (flag7 && Main.hardMode && (Main.player[owner].ZoneSnow && num10 == 3) && Main.rand.Next(3) != 0)
                                Type = 2429;
                            else if (flag7 && Main.hardMode && Main.rand.Next(2) == 0)
                                Type = 3211;
                            else if (flag4 && type == 2477)
                                Type = 2477;
                            else if (flag4 && type == 2463)
                                Type = 2463;
                            else if (flag4)
                                Type = 2319;
                            else if (flag3)
                                Type = 2305;
                        }
                        else if (Main.player[owner].ZoneHoly)
                        {
                            if (flag7 && Main.hardMode && (Main.player[owner].ZoneSnow && num10 == 3) && Main.rand.Next(3) != 0)
                                Type = 2429;
                            else if (flag7 && Main.hardMode && Main.rand.Next(2) == 0)
                                Type = 3209;
                            else if (num10 > 1 && flag6)
                                Type = 2317;
                            else if (num10 > 1 && flag5 && type == 2465)
                                Type = 2465;
                            else if (num10 < 2 && flag5 && type == 2468)
                                Type = 2468;
                            else if (flag5)
                                Type = 2310;
                            else if (flag4 && type == 2471)
                                Type = 2471;
                            else if (flag4)
                                Type = 2307;
                        }
                        if (Type == 0 && Main.player[owner].ZoneSnow)
                        {
                            if (num10 < 2 && flag4 && type == 2467)
                                Type = 2467;
                            else if (num10 == 1 && flag4 && type == 2470)
                                Type = 2470;
                            else if (num10 >= 2 && flag4 && type == 2484)
                                Type = 2484;
                            else if (num10 > 1 && flag4 && type == 2466)
                                Type = 2466;
                            else if (flag3 && Main.rand.Next(12) == 0 || flag4 && Main.rand.Next(6) == 0)
                                Type = 3197;
                            else if (flag4)
                                Type = 2306;
                            else if (flag3)
                                Type = 2299;
                            else if (num10 > 1 && Main.rand.Next(3) == 0)
                                Type = 2309;
                        }
                        if (Type == 0 && Main.player[owner].ZoneJungle)
                        {
                            if (Main.hardMode && flag7 && Main.rand.Next(2) == 0)
                                Type = 3018;
                            else if (num10 == 1 && flag4 && type == 2452)
                                Type = 2452;
                            else if (num10 == 1 && flag4 && type == 2483)
                                Type = 2483;
                            else if (num10 == 1 && flag4 && type == 2488)
                                Type = 2488;
                            else if (num10 >= 1 && flag4 && type == 2486)
                                Type = 2486;
                            else if (num10 > 1 && flag4)
                                Type = 2311;
                            else if (flag4)
                                Type = 2313;
                            else if (flag3)
                                Type = 2302;
                        }
                        if (Type == 0 && Main.shroomTiles > 200 && (flag4 && type == 2475))
                            Type = 2475;
                        if (Type == 0)
                        {
                            if (num10 <= 1 && (index < 380 || index > Main.maxTilesX - 380) && num1 > 1000)
                            {
                                Type = !flag6 || Main.rand.Next(2) != 0 ? (!flag6 ? (!flag5 || Main.rand.Next(5) != 0 ? (!flag5 || Main.rand.Next(2) != 0 ? (!flag4 || type != 2480 ? (!flag4 || type != 2481 ? (!flag4 ? (!flag3 || Main.rand.Next(2) != 0 ? (!flag3 ? 2297 : 2300) : 2301) : 2316) : 2481) : 2480) : 2332) : 2438) : 2342) : 2341;
                            }
                            else
                            {
                                int num13 = Main.sandTiles;
                            }
                        }
                        if (Type == 0)
                            Type = num10 >= 2 || !flag4 || type != 2461 ? (num10 != 0 || !flag4 || type != 2453 ? (num10 != 0 || !flag4 || type != 2473 ? (num10 != 0 || !flag4 || type != 2476 ? (num10 >= 2 || !flag4 || type != 2458 ? (num10 >= 2 || !flag4 || type != 2459 ? (num10 != 0 || !flag4 ? (num10 <= 0 || num10 >= 3 || (!flag4 || type != 2455) ? (num10 != 1 || !flag4 || type != 2479 ? (num10 != 1 || !flag4 || type != 2456 ? (num10 != 1 || !flag4 || type != 2474 ? (num10 <= 1 || !flag5 || Main.rand.Next(5) != 0 ? (num10 <= 1 || !flag7 ? (num10 <= 1 || !flag6 || Main.rand.Next(2) != 0 ? (num10 <= 1 || !flag5 ? (num10 <= 1 || !flag4 || type != 2478 ? (num10 <= 1 || !flag4 || type != 2450 ? (num10 <= 1 || !flag4 || type != 2464 ? (num10 <= 1 || !flag4 || type != 2469 ? (num10 <= 2 || !flag4 || type != 2462 ? (num10 <= 2 || !flag4 || type != 2482 ? (num10 <= 2 || !flag4 || type != 2472 ? (num10 <= 2 || !flag4 || type != 2460 ? (num10 <= 1 || !flag4 || Main.rand.Next(4) == 0 ? (num10 <= 1 || !flag4 && !flag3 && Main.rand.Next(4) != 0 ? (!flag4 || type != 2487 ? (num1 <= 1000 || !flag3 ? 2290 : 2298) : 2487) : (Main.rand.Next(4) != 0 ? 2309 : 2303)) : 2303) : 2460) : 2472) : 2482) : 2462) : 2469) : 2464) : 2450) : 2478) : 2321) : 2320) : 2308) : (!Main.hardMode || Main.rand.Next(2) != 0 ? 2436 : 2437)) : 2474) : 2456) : 2479) : 2455) : 2304) : 2459) : 2458) : 2476) : 2473) : 2453) : 2461;
                    }
                    if (Type <= 0)
                        return;
                    if (Main.player[owner].sonarPotion)
                    {
                        Item newItem = new Item();
                        newItem.SetDefaults(Type, false);
                        newItem.position = position;
                        ItemText.NewText(newItem, 1, true, false);
                    }
                    float num14 = num2;
                    ai[1] = Main.rand.Next(-240, -90) - num14;
                    localAI[1] = Type;
                    netUpdate = true;
                }
            }
        }

        public bool CanReflect()
        {
            return active && friendly && (!hostile && damage > 0) && (aiStyle == 1 || aiStyle == 2 || (aiStyle == 8 || aiStyle == 21) || (aiStyle == 24 || aiStyle == 28 || aiStyle == 29));
        }

        public float GetPrismHue(float indexing)
        {
            if (Main.player[owner].active)
            {
                switch (Main.player[owner].name)
                {
                    case "Tsuki":
                    case "Yoraiz0r":
                        return 0.85f;
                    case "Ghostar":
                        return 0.33f;
                    case "Devalaous":
                        return (float)(0.660000026226044 + Math.Cos(Main.time / 180.0 * 6.28318548202515) * 0.100000001490116);
                    case "Leinfors":
                        return 0.77f;
                    case "Aeroblop":
                        return (float)(0.25 + Math.Cos(Main.time / 180.0 * 6.28318548202515) * 0.100000001490116);
                    case "Doylee":
                        return 0.0f;
                    case "Darkhalis":
                    case "Arkhalis":
                        return (float)(0.75 + Math.Cos(Main.time / 180.0 * 6.28318548202515) * 0.0700000002980232);
                    case "Nike Leon":
                        return (float)(0.0750000029802322 + Math.Cos(Main.time / 180.0 * 6.28318548202515) * 0.0700000002980232);
                    case "Suweeka":
                        return (float)(0.5 + Math.Cos(Main.time / 180.0 * 6.28318548202515) * 0.180000007152557);
                    case "W1K":
                        return (float)(0.75 + Math.Cos(Main.time / 120.0 * 6.28318548202515) * 0.0500000007450581);
                    case "Random":
                        return Utils.NextFloat(Main.rand);
                }
            }
            return (int)indexing / 6f;
        }

        public void ProjectileFixDesperation(int own)
        {
            switch (type)
            {
                case 461:
                case 632:
                case 642:
                case 644:
                    for (int index = 0; index < 1000; ++index)
                    {
                        if (Main.projectile[index].owner == owner && Main.projectile[index].identity == ai[1] && Main.projectile[index].active)
                        {
                            ai[1] = index;
                            break;
                        }
                    }
                    break;
                case 626:
                case 627:
                case 628:
                    for (int index = 0; index < 1000; ++index)
                    {
                        if (Main.projectile[index].owner == owner && Main.projectile[index].identity == ai[0] && Main.projectile[index].active)
                        {
                            ai[0] = index;
                            break;
                        }
                    }
                    break;
            }
        }

        public void AI()
        {
            // ISSUE: unable to decompile the method.
        }

        private void AI_001()
        {
            if (type == 469 && wet && !honeyWet)
                Kill();
            if (type == 601)
            {
                Color portalColor = PortalHelper.GetPortalColor(owner, (int)ai[0]);
                Lighting.AddLight(Center + velocity * 3f, portalColor.ToVector3() * 0.5f);
                if (alpha > 0 && alpha <= 15)
                {
                    Color color = portalColor;
                    color.A = byte.MaxValue;
                    for (int index = 0; index < 4; ++index)
                    {
                        Vector2 vector2 = Utils.RotatedByRandom(Vector2.UnitY, 6.28318548202515) * (3f * Utils.NextFloat(Main.rand));
                        Dust dust = Main.dust[Dust.NewDust(Center, 0, 0, 264, 0.0f, 0.0f, 0, new Color(), 1f)];
                        dust.position = Center;
                        dust.velocity = velocity * 2f + Utils.RandomVector2(Main.rand, -1f, 1f);
                        dust.color = color;
                        dust.scale = 1.2f;
                        dust.noLight = true;
                        dust.noGravity = true;
                        dust.customData = Main.player[owner];
                    }
                }
                alpha -= 15;
                if (alpha < 0)
                    alpha = 0;
                if (++frameCounter >= 4)
                {
                    frameCounter = 0;
                    if (++frame >= Main.projFrames[type])
                        frame = 0;
                }
                if (alpha == 0)
                {
                    Color color = portalColor;
                    color.A = byte.MaxValue;
                    Dust dust = Main.dust[Dust.NewDust(Center, 0, 0, 263, 0.0f, 0.0f, 0, new Color(), 1f)];
                    dust.position = Center;
                    dust.velocity = velocity / 4f;
                    dust.color = color;
                    dust.noGravity = true;
                    dust.scale = 0.6f;
                }
            }
            if (type == 472)
            {
                int index1 = Dust.NewDust(position, width, height, 30, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index1].noGravity = true;
                Main.dust[index1].velocity *= 0.25f;
                Main.dust[index1].velocity += velocity * 0.75f;
                if (localAI[0] == 0.0)
                {
                    localAI[0] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
                    for (int index2 = 0; index2 < 20; ++index2)
                    {
                        int index3 = Dust.NewDust(position, width, height, 30, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index3].noGravity = true;
                        Main.dust[index3].velocity *= 0.25f;
                        Main.dust[index3].velocity += velocity;
                        Main.dust[index3].velocity.X *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                        Main.dust[index3].velocity.Y *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                    }
                }
            }
            if (type == 323)
            {
                alpha -= 50;
                if (alpha < 0)
                    alpha = 0;
            }
            if (type == 436)
            {
                if (ai[1] == 0.0)
                {
                    ai[1] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 12);
                }
                alpha -= 40;
                if (alpha < 0)
                    alpha = 0;
                spriteDirection = direction;
                ++frameCounter;
                if (frameCounter >= 3)
                {
                    ++frame;
                    frameCounter = 0;
                    if (frame >= 4)
                        frame = 0;
                }
                Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.3f, 1.1f, 0.5f);
            }
            if (type == 467)
            {
                if (ai[1] == 0.0)
                {
                    ai[1] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 34);
                }
                else if (ai[1] == 1.0 && Main.netMode != 1)
                {
                    int num1 = -1;
                    float num2 = 2000f;
                    for (int index = 0; index < 255; ++index)
                    {
                        if (Main.player[index].active && !Main.player[index].dead)
                        {
                            Vector2 center = Main.player[index].Center;
                            float num3 = Vector2.Distance(center, Center);
                            if ((num3 < num2 || num1 == -1) && Collision.CanHit(Center, 1, 1, center, 1, 1))
                            {
                                num2 = num3;
                                num1 = index;
                            }
                        }
                    }
                    if (num2 < 20.0)
                    {
                        Kill();
                        return;
                    }
                    if (num1 != -1)
                    {
                        ai[1] = 21f;
                        ai[0] = num1;
                        netUpdate = true;
                    }
                }
                else if (ai[1] > 20.0 && ai[1] < 200.0)
                {
                    ++ai[1];
                    int index = (int)ai[0];
                    if (!Main.player[index].active || Main.player[index].dead)
                    {
                        ai[1] = 1f;
                        ai[0] = 0.0f;
                        netUpdate = true;
                    }
                    else
                    {
                        float curAngle = Utils.ToRotation(velocity);
                        Vector2 v = Main.player[index].Center - Center;
                        if (v.Length() < 20.0)
                        {
                            Kill();
                            return;
                        }
                        float targetAngle = Utils.ToRotation(v);
                        if (v == Vector2.Zero)
                            targetAngle = curAngle;
                        velocity = Utils.RotatedBy(new Vector2(velocity.Length(), 0.0f), Utils.AngleLerp(curAngle, targetAngle, 0.008f), new Vector2());
                    }
                }
                if (ai[1] >= 1.0 && ai[1] < 20.0)
                {
                    ++ai[1];
                    if (ai[1] == 20.0)
                        ai[1] = 1f;
                }
                alpha -= 40;
                if (alpha < 0)
                    alpha = 0;
                spriteDirection = direction;
                ++frameCounter;
                if (frameCounter >= 3)
                {
                    ++frame;
                    frameCounter = 0;
                    if (frame >= 4)
                        frame = 0;
                }
                Lighting.AddLight(Center, 1.1f, 0.9f, 0.4f);
                ++localAI[0];
                if (localAI[0] == 12.0)
                {
                    localAI[0] = 0.0f;
                    for (int index1 = 0; index1 < 12; ++index1)
                    {
                        Vector2 vector2 = Utils.RotatedBy(Vector2.UnitX * (float)-width / 2f + -Utils.RotatedBy(Vector2.UnitY, (double)index1 * 3.14159274101257 / 6.0, new Vector2()) * new Vector2(8f, 16f), rotation - 1.57079637050629, new Vector2());
                        int index2 = Dust.NewDust(Center, 0, 0, 6, 0.0f, 0.0f, 160, new Color(), 1f);
                        Main.dust[index2].scale = 1.1f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].position = Center + vector2;
                        Main.dust[index2].velocity = velocity * 0.1f;
                        Main.dust[index2].velocity = Vector2.Normalize(Center - velocity * 3f - Main.dust[index2].position) * 1.25f;
                    }
                }
                if (Main.rand.Next(4) == 0)
                {
                    for (int index1 = 0; index1 < 1; ++index1)
                    {
                        Vector2 vector2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.196349546313286), Utils.ToRotation(velocity), new Vector2());
                        int index2 = Dust.NewDust(position, width, height, 31, 0.0f, 0.0f, 100, new Color(), 1f);
                        Main.dust[index2].velocity *= 0.1f;
                        Main.dust[index2].position = Center + vector2 * width / 2f;
                        Main.dust[index2].fadeIn = 0.9f;
                    }
                }
                if (Main.rand.Next(32) == 0)
                {
                    for (int index1 = 0; index1 < 1; ++index1)
                    {
                        Vector2 vector2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.392699092626572), Utils.ToRotation(velocity), new Vector2());
                        int index2 = Dust.NewDust(position, width, height, 31, 0.0f, 0.0f, 155, new Color(), 0.8f);
                        Main.dust[index2].velocity *= 0.3f;
                        Main.dust[index2].position = Center + vector2 * width / 2f;
                        if (Main.rand.Next(2) == 0)
                            Main.dust[index2].fadeIn = 1.4f;
                    }
                }
                if (Main.rand.Next(2) == 0)
                {
                    for (int index1 = 0; index1 < 2; ++index1)
                    {
                        Vector2 vector2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.785398185253143), Utils.ToRotation(velocity), new Vector2());
                        int index2 = Dust.NewDust(position, width, height, 6, 0.0f, 0.0f, 0, new Color(), 1.2f);
                        Main.dust[index2].velocity *= 0.3f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].position = Center + vector2 * width / 2f;
                        if (Main.rand.Next(2) == 0)
                            Main.dust[index2].fadeIn = 1.4f;
                    }
                }
            }
            if (type == 468)
            {
                if (ai[1] == 0.0)
                {
                    ai[1] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 34);
                }
                else if (ai[1] == 1.0 && Main.netMode != 1)
                {
                    int num1 = -1;
                    float num2 = 2000f;
                    for (int index = 0; index < 255; ++index)
                    {
                        if (Main.player[index].active && !Main.player[index].dead)
                        {
                            Vector2 center = Main.player[index].Center;
                            float num3 = Vector2.Distance(center, Center);
                            if ((num3 < num2 || num1 == -1) && Collision.CanHit(Center, 1, 1, center, 1, 1))
                            {
                                num2 = num3;
                                num1 = index;
                            }
                        }
                    }
                    if (num2 < 20.0)
                    {
                        Kill();
                        return;
                    }
                    if (num1 != -1)
                    {
                        ai[1] = 21f;
                        ai[0] = num1;
                        netUpdate = true;
                    }
                }
                else if (ai[1] > 20.0 && ai[1] < 200.0)
                {
                    ++ai[1];
                    int index = (int)ai[0];
                    if (!Main.player[index].active || Main.player[index].dead)
                    {
                        ai[1] = 1f;
                        ai[0] = 0.0f;
                        netUpdate = true;
                    }
                    else
                    {
                        float curAngle = Utils.ToRotation(velocity);
                        Vector2 v = Main.player[index].Center - Center;
                        if ((double)v.Length() < 20.0)
                        {
                            Kill();
                            return;
                        }
                        float targetAngle = Utils.ToRotation(v);
                        if (v == Vector2.Zero)
                            targetAngle = curAngle;
                        velocity = Utils.RotatedBy(new Vector2(velocity.Length(), 0.0f), Utils.AngleLerp(curAngle, targetAngle, 0.01f), new Vector2());
                    }
                }
                if (ai[1] >= 1.0 && ai[1] < 20.0)
                {
                    ++ai[1];
                    if (ai[1] == 20.0)
                        ai[1] = 1f;
                }
                alpha -= 40;
                if (alpha < 0)
                    alpha = 0;
                spriteDirection = direction;
                ++frameCounter;
                if (frameCounter >= 3)
                {
                    ++frame;
                    frameCounter = 0;
                    if (frame >= 4)
                        frame = 0;
                }
                Lighting.AddLight(Center, 0.2f, 0.1f, 0.6f);
                ++localAI[0];
                if (localAI[0] == 12.0)
                {
                    localAI[0] = 0.0f;
                    for (int index1 = 0; index1 < 12; ++index1)
                    {
                        Vector2 vector2 = Utils.RotatedBy(Vector2.UnitX * -width / 2f + -Utils.RotatedBy(Vector2.UnitY, index1 * 3.14159274101257 / 6.0, new Vector2()) * new Vector2(8f, 16f), rotation - 1.57079637050629, new Vector2());
                        int index2 = Dust.NewDust(Center, 0, 0, 27, 0.0f, 0.0f, 160, new Color(), 1f);
                        Main.dust[index2].scale = 1.1f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].position = Center + vector2;
                        Main.dust[index2].velocity = velocity * 0.1f;
                        Main.dust[index2].velocity = Vector2.Normalize(Center - velocity * 3f - Main.dust[index2].position) * 1.25f;
                    }
                }
                if (Main.rand.Next(4) == 0)
                {
                    for (int index1 = 0; index1 < 1; ++index1)
                    {
                        Vector2 vector2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.196349546313286), Utils.ToRotation(velocity), new Vector2());
                        int index2 = Dust.NewDust(position, width, height, 31, 0.0f, 0.0f, 100, new Color(), 1f);
                        Main.dust[index2].velocity *= 0.1f;
                        Main.dust[index2].position = Center + vector2 * width / 2f;
                        Main.dust[index2].fadeIn = 0.9f;
                    }
                }
                if (Main.rand.Next(32) == 0)
                {
                    for (int index1 = 0; index1 < 1; ++index1)
                    {
                        Vector2 vector2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.392699092626572), Utils.ToRotation(velocity), new Vector2());
                        int index2 = Dust.NewDust(position, width, height, 31, 0.0f, 0.0f, 155, new Color(), 0.8f);
                        Main.dust[index2].velocity *= 0.3f;
                        Main.dust[index2].position = Center + vector2 * width / 2f;
                        if (Main.rand.Next(2) == 0)
                            Main.dust[index2].fadeIn = 1.4f;
                    }
                }
                if (Main.rand.Next(2) == 0)
                {
                    for (int index1 = 0; index1 < 2; ++index1)
                    {
                        Vector2 vector2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.785398185253143), Utils.ToRotation(velocity), new Vector2());
                        int index2 = Dust.NewDust(position, width, height, 27, 0.0f, 0.0f, 0, new Color(), 1.2f);
                        Main.dust[index2].velocity *= 0.3f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].position = Center + vector2 * width / 2f;
                        if (Main.rand.Next(2) == 0)
                            Main.dust[index2].fadeIn = 1.4f;
                    }
                }
            }
            if (type == 634 || type == 635)
            {
                float num1 = 5f;
                float num2 = 250f;
                float num3 = 6f;
                Vector2 vector2_1 = new Vector2(8f, 10f);
                float num4 = 1.2f;
                Vector3 rgb = new Vector3(0.7f, 0.1f, 0.5f);
                int num5 = 4 * MaxUpdates;
                int Type1 = Utils.SelectRandom(Main.rand, 242, 73, 72, 71, 255);
                int Type2 = 255;
                if (type == 635)
                {
                    vector2_1 = new Vector2(10f, 20f);
                    num4 = 1f;
                    num2 = 500f;
                    Type2 = 88;
                    num5 = 3 * MaxUpdates;
                    rgb = new Vector3(0.4f, 0.6f, 0.9f);
                    Type1 = Utils.SelectRandom(Main.rand, 242, 59, 88);
                }
                if (ai[1] == 0.0)
                {
                    ai[1] = 1f;
                    localAI[0] = -Main.rand.Next(48);
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 34);
                }
                else if (ai[1] == 1.0 && owner == Main.myPlayer)
                {
                    int num6 = -1;
                    float num7 = num2;
                    for (int index = 0; index < 200; ++index)
                    {
                        if (Main.npc[index].active && Main.npc[index].CanBeChasedBy(this, false))
                        {
                            Vector2 center = Main.npc[index].Center;
                            float num8 = Vector2.Distance(center, Center);
                            if (num8 < num7 && num6 == -1 && Collision.CanHitLine(Center, 1, 1, center, 1, 1))
                            {
                                num7 = num8;
                                num6 = index;
                            }
                        }
                    }
                    if (num7 < 20.0)
                    {
                        Kill();
                        return;
                    }
                    if (num6 != -1)
                    {
                        ai[1] = num1 + 1f;
                        ai[0] = num6;
                        netUpdate = true;
                    }
                }
                else if (ai[1] > num1)
                {
                    ++ai[1];
                    int index = (int)ai[0];
                    if (!Main.npc[index].active || !Main.npc[index].CanBeChasedBy(this, false))
                    {
                        ai[1] = 1f;
                        ai[0] = 0.0f;
                        netUpdate = true;
                    }
                    else
                    {
                        double num6 = Utils.ToRotation(velocity);
                        Vector2 vector2_2 = Main.npc[index].Center - Center;
                        if (vector2_2.Length() < 20.0)
                        {
                            Kill();
                            return;
                        }
                        if (vector2_2 != Vector2.Zero)
                        {
                            vector2_2.Normalize();
                            vector2_2 *= num3;
                        }
                        float num7 = 30f;
                        velocity = (velocity * (num7 - 1f) + vector2_2) / num7;
                    }
                }
                if (ai[1] >= 1.0 && ai[1] < num1)
                {
                    ++ai[1];
                    if (ai[1] == num1)
                        ai[1] = 1f;
                }
                alpha -= 40;
                if (alpha < 0)
                    alpha = 0;
                spriteDirection = direction;
                ++frameCounter;
                if (frameCounter >= num5)
                {
                    ++frame;
                    frameCounter = 0;
                    if (frame >= 4)
                        frame = 0;
                }
                Lighting.AddLight(Center, rgb);
                rotation = Utils.ToRotation(velocity);
                ++localAI[0];
                if (localAI[0] == 48.0)
                    localAI[0] = 0.0f;
                else if (alpha == 0)
                {
                    for (int index1 = 0; index1 < 2; ++index1)
                    {
                        Vector2 vector2_2 = Vector2.UnitX * -30f;
                        Vector2 vector2_3 = -Utils.RotatedBy(Vector2.UnitY, localAI[0] * 0.130899697542191 + index1 * 3.14159274101257, new Vector2()) * vector2_1 - Utils.ToRotationVector2(rotation) * 10f;
                        int index2 = Dust.NewDust(Center, 0, 0, Type2, 0.0f, 0.0f, 160, new Color(), 1f);
                        Main.dust[index2].scale = num4;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].position = Center + vector2_3 + velocity * 2f;
                        Main.dust[index2].velocity = Vector2.Normalize(Center + velocity * 2f * 8f - Main.dust[index2].position) * 2f + velocity * 2f;
                    }
                }
                if (Main.rand.Next(12) == 0)
                {
                    for (int index1 = 0; index1 < 1; ++index1)
                    {
                        Vector2 vector2_2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.196349546313286), Utils.ToRotation(velocity), new Vector2());
                        int index2 = Dust.NewDust(position, width, height, 31, 0.0f, 0.0f, 100, new Color(), 1f);
                        Main.dust[index2].velocity *= 0.1f;
                        Main.dust[index2].position = Center + vector2_2 * width / 2f + velocity * 2f;
                        Main.dust[index2].fadeIn = 0.9f;
                    }
                }
                if (Main.rand.Next(64) == 0)
                {
                    for (int index1 = 0; index1 < 1; ++index1)
                    {
                        Vector2 vector2_2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.392699092626572), Utils.ToRotation(velocity), new Vector2());
                        int index2 = Dust.NewDust(position, width, height, 31, 0.0f, 0.0f, 155, new Color(), 0.8f);
                        Main.dust[index2].velocity *= 0.3f;
                        Main.dust[index2].position = Center + vector2_2 * width / 2f;
                        if (Main.rand.Next(2) == 0)
                            Main.dust[index2].fadeIn = 1.4f;
                    }
                }
                if (Main.rand.Next(4) == 0)
                {
                    for (int index1 = 0; index1 < 2; ++index1)
                    {
                        Vector2 vector2_2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.785398185253143), Utils.ToRotation(velocity), new Vector2());
                        int index2 = Dust.NewDust(position, width, height, Type1, 0.0f, 0.0f, 0, new Color(), 1.2f);
                        Main.dust[index2].velocity *= 0.3f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].position = Center + vector2_2 * width / 2f;
                        if (Main.rand.Next(2) == 0)
                            Main.dust[index2].fadeIn = 1.4f;
                    }
                }
                if (Main.rand.Next(12) == 0 && type == 634)
                {
                    Vector2 vector2_2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.196349546313286), Utils.ToRotation(velocity), new Vector2());
                    int index = Dust.NewDust(position, width, height, Type2, 0.0f, 0.0f, 100, new Color(), 1f);
                    Main.dust[index].velocity *= 0.3f;
                    Main.dust[index].position = Center + vector2_2 * width / 2f;
                    Main.dust[index].fadeIn = 0.9f;
                    Main.dust[index].noGravity = true;
                }
                if (Main.rand.Next(3) == 0 && type == 635)
                {
                    Vector2 vector2_2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.196349546313286), Utils.ToRotation(velocity), new Vector2());
                    int index = Dust.NewDust(position, width, height, Type2, 0.0f, 0.0f, 100, new Color(), 1f);
                    Main.dust[index].velocity *= 0.3f;
                    Main.dust[index].position = Center + vector2_2 * width / 2f;
                    Main.dust[index].fadeIn = 1.2f;
                    Main.dust[index].scale = 1.5f;
                    Main.dust[index].noGravity = true;
                }
            }
            if (type == 459)
            {
                alpha -= 30;
                if (alpha < 0)
                    alpha = 0;
                spriteDirection = direction;
                ++frameCounter;
                if (frameCounter >= 3)
                {
                    ++frame;
                    frameCounter = 0;
                    if (frame >= 3)
                        frame = 0;
                }
                position = Center;
                scale = ai[1];
                width = height = (int)(22.0 * scale);
                Center = position;
                Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.4f, 0.85f, 0.9f);
                int num;
                if (scale < 0.85)
                {
                    num = Main.rand.Next(3) == 0 ? 1 : 0;
                }
                else
                {
                    num = 1;
                    penetrate = -1;
                    maxPenetrate = -1;
                }
                for (int index1 = 0; index1 < num; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 226, velocity.X, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].position -= Vector2.One * 3f;
                    Main.dust[index2].scale = 0.5f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity = velocity / 3f;
                    Main.dust[index2].alpha = 255 - (int)(255 * scale);
                }
            }
            if (type == 442)
            {
                frame = 0;
                if (alpha != 0)
                {
                    ++localAI[0];
                    if (localAI[0] >= 4.0)
                    {
                        alpha -= 90;
                        if (alpha < 0)
                        {
                            alpha = 0;
                            localAI[0] = 2f;
                        }
                    }
                }
                if (Vector2.Distance(Center, new Vector2(ai[0], ai[1]) * 16f + Vector2.One * 8f) <= 16.0)
                {
                    Kill();
                    return;
                }
                if (alpha == 0)
                {
                    ++localAI[1];
                    if (localAI[1] >= 120.0)
                    {
                        Kill();
                        return;
                    }
                    Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.3f, 0.45f, 0.8f);
                    ++localAI[0];
                    if (localAI[0] == 3.0)
                    {
                        localAI[0] = 0.0f;
                        for (int index1 = 0; index1 < 8; ++index1)
                        {
                            Vector2 vector2 = Utils.RotatedBy(Vector2.UnitX * -8f + -Utils.RotatedBy(Vector2.UnitY, (double)index1 * 3.14159274101257 / 4.0, new Vector2()) * new Vector2(2f, 4f), rotation - 1.57079637050629, new Vector2());
                            int index2 = Dust.NewDust(Center, 0, 0, 135, 0.0f, 0.0f, 0, new Color(), 1f);
                            Main.dust[index2].scale = 1.5f;
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].position = Center + vector2;
                            Main.dust[index2].velocity = velocity * 0.66f;
                        }
                    }
                }
            }
            if (type == 440 || type == 449 || type == 606)
            {
                if (alpha > 0)
                    alpha -= 25;
                if (alpha < 0)
                    alpha = 0;
                if (type == 440)
                    Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.25f, 0.4f, 0.7f);
                if (type == 449)
                    Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.7f, 0.65f, 0.3f);
                if (type == 606)
                    Lighting.AddLight(Center, 0.7f, 0.3f, 0.3f);
                float num1 = 100f;
                float num2 = 3f;
                if (type == 606)
                {
                    num1 = 150f;
                    num2 = 5f;
                }
                if (ai[1] == 0.0)
                {
                    localAI[0] += num2;
                    if (localAI[0] == num2 * 1.0 && type == 606)
                    {
                        for (int index1 = 0; index1 < 4; ++index1)
                        {
                            int index2 = Dust.NewDust(Center - velocity / 2f, 0, 0, 182, 0.0f, 0.0f, 100, new Color(), 1.4f);
                            Main.dust[index2].velocity *= 0.2f;
                            Main.dust[index2].velocity += velocity / 10f;
                            Main.dust[index2].noGravity = true;
                        }
                    }
                    if (localAI[0] > num1)
                        localAI[0] = num1;
                }
                else
                {
                    localAI[0] -= num2;
                    if (localAI[0] <= 0.0)
                    {
                        Kill();
                        return;
                    }
                }
            }
            if (type == 438)
                Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.4f, 0.1f, 0.2f);
            if (type == 593)
            {
                Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.4f, 0.1f, 0.3f);
                if (++frameCounter >= 12)
                {
                    if (++frame >= Main.projFrames[type])
                        frame = 0;
                    frameCounter = 0;
                }
                if (Main.rand.Next(2) == 0)
                {
                    Vector2 spinningpoint = Utils.RotatedByRandom(Vector2.UnitY, 6.28318548202515);
                    Dust dust = Main.dust[Dust.NewDust(Center - spinningpoint * 8f, 0, 0, 240, 0.0f, 0.0f, 0, new Color(), 1f)];
                    dust.noGravity = true;
                    dust.position = Center - spinningpoint * 8f * scale;
                    dust.velocity = Utils.RotatedBy(spinningpoint, -1.57079637050629, new Vector2()) * 2f;
                    dust.velocity = Vector2.Zero;
                    dust.scale = 0.5f + Utils.NextFloat(Main.rand);
                    dust.fadeIn = 0.5f;
                }
            }
            if (type == 592)
                Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.15f, 0.15f, 0.4f);
            if (type == 462)
            {
                int index = Dust.NewDust(Center, 0, 0, 229, 0.0f, 0.0f, 100, new Color(), 1f);
                Main.dust[index].noLight = true;
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = velocity;
                Main.dust[index].position -= Vector2.One * 4f;
                Main.dust[index].scale = 0.8f;
                if (++frameCounter >= 9)
                {
                    frameCounter = 0;
                    if (++frame >= 5)
                        frame = 0;
                }
            }
            if (type == 437)
            {
                if (ai[1] == 0.0)
                {
                    ai[1] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 12);
                }
                if (localAI[0] == 0.0)
                {
                    localAI[0] = 1f;
                    for (int index1 = 0; index1 < 4; ++index1)
                    {
                        int index2 = Dust.NewDust(position, width, height, 226, velocity.X, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Center, 0.25f);
                        Main.dust[index2].scale = 0.5f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity /= 2f;
                        Main.dust[index2].velocity += velocity * 0.66f;
                    }
                }
                if (ai[0] < 16.0)
                {
                    for (int index1 = 0; index1 < 2; ++index1)
                    {
                        int index2 = Dust.NewDust(position, width, height, 226, velocity.X, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].position = position + new Vector2(((direction == 1 ? 1 : 0) * width), (2 + (height - 4) * index1));
                        Main.dust[index2].scale = 0.3f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity = Vector2.Zero;
                    }
                }
            }
            if (type == 435)
            {
                if (ai[1] == 0.0)
                {
                    ai[1] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 12);
                }
                alpha -= 40;
                if (alpha < 0)
                    alpha = 0;
                spriteDirection = direction;
                ++frameCounter;
                if (frameCounter >= 3)
                {
                    ++frame;
                    frameCounter = 0;
                    if (frame >= 4)
                        frame = 0;
                }
                Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.3f, 0.8f, 1.1f);
            }
            if (type == 408)
            {
                alpha -= 40;
                if (alpha < 0)
                    alpha = 0;
                spriteDirection = direction;
            }
            if (type == 282)
            {
                int index = Dust.NewDust(position, width, height, 171, 0.0f, 0.0f, 100, new Color(), 1f);
                Main.dust[index].scale = Main.rand.Next(1, 10) * 0.1f;
                Main.dust[index].noGravity = true;
                Main.dust[index].fadeIn = 1.5f;
                Main.dust[index].velocity *= 0.25f;
                Main.dust[index].velocity += velocity * 0.25f;
            }
            if (type == 275 || type == 276)
            {
                ++frameCounter;
                if (frameCounter > 1)
                {
                    frameCounter = 0;
                    ++frame;
                    if (frame > 1)
                        frame = 0;
                }
            }
            if (type == 225)
            {
                int index = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 40, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index].noGravity = true;
                Main.dust[index].scale = 1.3f;
                Main.dust[index].velocity *= 0.5f;
            }
            if (type == 174)
            {
                if (alpha == 0)
                {
                    int index = Dust.NewDust(oldPosition - velocity * 3f, width, height, 76, 0.0f, 0.0f, 50, new Color(), 1f);
                    Main.dust[index].noGravity = true;
                    Main.dust[index].noLight = true;
                    Main.dust[index].velocity *= 0.5f;
                }
                alpha -= 50;
                if (alpha < 0)
                    alpha = 0;
                if (ai[1] == 0.0)
                {
                    ai[1] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
                }
            }
            else if (type == 605)
            {
                if (alpha == 0 && Main.rand.Next(3) == 0)
                {
                    int index = Dust.NewDust(position - velocity * 3f, width, height, 4, 0.0f, 0.0f, 50, new Color(78, 136, 255, 150), 1.2f);
                    Main.dust[index].velocity *= 0.3f;
                    Main.dust[index].velocity += velocity * 0.3f;
                    Main.dust[index].noGravity = true;
                }
                alpha -= 50;
                if (alpha < 0)
                    alpha = 0;
                if (ai[1] == 0.0)
                {
                    ai[1] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
                }
            }
            else if (type == 176)
            {
                if (alpha == 0)
                {
                    int index = Dust.NewDust(oldPosition, width, height, 22, 0.0f, 0.0f, 100, new Color(), 0.5f);
                    Main.dust[index].noGravity = true;
                    Main.dust[index].noLight = true;
                    Main.dust[index].velocity *= 0.15f;
                    Main.dust[index].fadeIn = 0.8f;
                }
                alpha -= 50;
                if (alpha < 0)
                    alpha = 0;
                if (ai[1] == 0.0)
                {
                    ai[1] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
                }
            }
            if (type == 350)
            {
                alpha -= 100;
                if (alpha < 0)
                    alpha = 0;
                Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.9f, 0.6f, 0.2f);
                if (alpha == 0)
                {
                    int num = 2;
                    if (Main.rand.Next(2) == 0)
                    {
                        int index = Dust.NewDust(new Vector2(Center.X - num, (float)(Center.Y - num - 2.0)) - velocity * 0.5f, num * 2, num * 2, 6, 0.0f, 0.0f, 100, new Color(), 1f);
                        Main.dust[index].scale *= (float)(1.79999995231628 + Main.rand.Next(10) * 0.100000001490116);
                        Main.dust[index].velocity *= 0.2f;
                        Main.dust[index].noGravity = true;
                    }
                    if (Main.rand.Next(4) == 0)
                    {
                        int index = Dust.NewDust(new Vector2(Center.X - num, (float)(Center.Y - num - 2.0)) - velocity * 0.5f, num * 2, num * 2, 31, 0.0f, 0.0f, 100, new Color(), 0.5f);
                        Main.dust[index].fadeIn = (float)(1.0 + Main.rand.Next(5) * 0.100000001490116);
                        Main.dust[index].velocity *= 0.05f;
                    }
                }
                if (ai[1] == 0.0)
                {
                    ai[1] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 42);
                }
            }
            if (type == 325)
            {
                alpha -= 100;
                if (alpha < 0)
                    alpha = 0;
                Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.9f, 0.6f, 0.2f);
                if (alpha == 0)
                {
                    int num = 2;
                    if (Main.rand.Next(2) == 0)
                    {
                        int index = Dust.NewDust(new Vector2(Center.X - num, (float)(Center.Y - num - 2.0)) - velocity * 0.5f, num * 2, num * 2, 6, 0.0f, 0.0f, 100, new Color(), 1f);
                        Main.dust[index].scale *= (float)(1.79999995231628 + Main.rand.Next(10) * 0.100000001490116);
                        Main.dust[index].velocity *= 0.2f;
                        Main.dust[index].noGravity = true;
                    }
                    if (Main.rand.Next(4) == 0)
                    {
                        int index = Dust.NewDust(new Vector2(Center.X - num, (float)(Center.Y - num - 2.0)) - velocity * 0.5f, num * 2, num * 2, 31, 0.0f, 0.0f, 100, new Color(), 0.5f);
                        Main.dust[index].fadeIn = (float)(1.0 + Main.rand.Next(5) * 0.100000001490116);
                        Main.dust[index].velocity *= 0.05f;
                    }
                }
                if (ai[1] == 0.0)
                {
                    ai[1] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 42);
                }
            }
            if (type == 469)
            {
                ++localAI[1];
                if (localAI[1] > 2.0)
                {
                    alpha -= 50;
                    if (alpha < 0)
                        alpha = 0;
                }
            }
            else if (type == 83 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 33);
            }
            else if (type == 408 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(4, (int)position.X, (int)position.Y, 19);
            }
            else if (type == 259 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 33);
            }
            else if (type == 110 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 11);
            }
            else if (type == 302 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 11);
            }
            else if (type == 438 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 12);
            }
            else if (type == 593 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 11);
            }
            else if (type == 592 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 12);
            }
            else if (type == 462 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, Main.rand.Next(124, 126));
                Vector2 vector2 = Vector2.Normalize(velocity);
                int num = Main.rand.Next(5, 10);
                for (int index1 = 0; index1 < num; ++index1)
                {
                    int index2 = Dust.NewDust(Center, 0, 0, 229, 0.0f, 0.0f, 100, new Color(), 1f);
                    --Main.dust[index2].velocity.Y;
                    Main.dust[index2].velocity += vector2 * 2f;
                    Main.dust[index2].position -= Vector2.One * 4f;
                    Main.dust[index2].noGravity = true;
                }
            }
            else if (type == 84 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 12);
            }
            else if (type == 389 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 12);
            }
            else if (type == 257 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 12);
            }
            else if (type == 100 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 33);
            }
            else if (type == 98 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
            }
            else if (type == 184 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
            }
            else if (type == 195 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
            }
            else if (type == 275 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
            }
            else if (type == 276 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
            }
            else if ((type == 81 || type == 82) && ai[1] == 0.0)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 5);
                ai[1] = 1f;
            }
            else if (type == 180 && ai[1] == 0.0)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 11);
                ai[1] = 1f;
            }
            else if (type == 248 && ai[1] == 0.0)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
                ai[1] = 1f;
            }
            else if (type == 576 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 12);
            }
            else if (type == 577 && ai[1] == 0.0)
            {
                ai[1] = 1f;
                Main.PlaySound(2, (int)position.X, (int)position.Y, 36);
            }
            else if (type == 639)
            {
                if (localAI[0] == 0.0 && localAI[1] == 0.0)
                {
                    localAI[0] = Center.X;
                    localAI[1] = Center.Y;
                    ai[0] = velocity.X;
                    ai[1] = velocity.Y;
                }
                alpha -= 25;
                if (alpha < 0)
                    alpha = 0;
            }
            else if (type == 640)
            {
                alpha -= 25;
                if (alpha < 0)
                    alpha = 0;
                if (velocity == Vector2.Zero)
                {
                    ai[0] = 0.0f;
                    bool flag = true;
                    for (int index = 1; index < oldPos.Length; ++index)
                    {
                        if (oldPos[index] != oldPos[0])
                            flag = false;
                    }
                    if (flag)
                    {
                        Kill();
                        return;
                    }
                    if (Main.rand.Next(extraUpdates) == 0 && (velocity != Vector2.Zero || Main.rand.Next(localAI[1] == 2.0 ? 2 : 6) == 0))
                    {
                        for (int index1 = 0; index1 < 2; ++index1)
                        {
                            float num1 = rotation + (float)((Main.rand.Next(2) == 1 ? -1.0 : 1.0) * 1.57079637050629);
                            float num2 = (float)(Main.rand.NextDouble() * 0.800000011920929 + 1.0);
                            Vector2 vector2 = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
                            int index2 = Dust.NewDust(Center, 0, 0, 229, vector2.X, vector2.Y, 0, new Color(), 1f);
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].scale = 1.2f;
                        }
                        if (Main.rand.Next(10) == 0)
                        {
                            int index = Dust.NewDust(Center + Utils.RotatedBy(velocity, 1.57079637050629, new Vector2()) * ((float)Main.rand.NextDouble() - 0.5f) * width - Vector2.One * 4f, 8, 8, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                            Main.dust[index].velocity *= 0.5f;
                            Main.dust[index].velocity.Y = -Math.Abs(Main.dust[index].velocity.Y);
                        }
                    }
                }
                else if (numUpdates == 1)
                {
                    float num1 = (float)(rotation + 1.57079637050629 + (Main.rand.Next(2) == 1 ? -1.0 : 1.0) * 1.57079637050629);
                    float num2 = (float)(Main.rand.NextDouble() * 0.25 + 0.25);
                    Vector2 vector2 = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
                    int index = Dust.NewDust(position, 0, 0, 229, vector2.X, vector2.Y, 0, new Color(), 1f);
                    Main.dust[index].noGravity = true;
                    Main.dust[index].scale = 1.2f;
                }
            }
            if (type == 41)
            {
                int index1 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.6f);
                Main.dust[index1].noGravity = true;
                int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2f);
                Main.dust[index2].noGravity = true;
            }
            else if (type == 55)
            {
                int index = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 18, 0.0f, 0.0f, 0, new Color(), 0.9f);
                Main.dust[index].noGravity = true;
            }
            else if (type == 374)
            {
                if (localAI[0] == 0.0)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
                    localAI[0] = 1f;
                }
                if (Main.rand.Next(2) == 0)
                {
                    int index = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 18, 0.0f, 0.0f, 0, new Color(), 0.9f);
                    Main.dust[index].noGravity = true;
                    Main.dust[index].velocity *= 0.5f;
                }
            }
            else if (type == 376)
            {
                if (localAI[0] == 0.0)
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 20);
                ++localAI[0];
                if (localAI[0] > 3.0)
                {
                    int num = 1;
                    if (localAI[0] > 5.0)
                        num = 2;
                    for (int index1 = 0; index1 < num; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y + 2f), width, height, 6, velocity.X * 0.2f, velocity.Y * 0.2f, 100, new Color(), 2f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity.X *= 0.3f;
                        Main.dust[index2].velocity.Y *= 0.3f;
                        Main.dust[index2].noLight = true;
                    }
                    if (wet && !lavaWet)
                    {
                        Kill();
                        return;
                    }
                }
            }
            else if (type == 91 && Main.rand.Next(2) == 0)
            {
                int index = Dust.NewDust(position, width, height, Main.rand.Next(2) != 0 ? 58 : 15, velocity.X * 0.25f, velocity.Y * 0.25f, 150, new Color(), 0.9f);
                Main.dust[index].velocity *= 0.25f;
            }
            if (type == 163 || type == 310)
            {
                if (alpha > 0)
                    alpha -= 25;
                if (alpha < 0)
                    alpha = 0;
            }
            switch (type)
            {
                case 389:
                case 180:
                case 279:
                case 283:
                case 284:
                case 285:
                case 286:
                case 287:
                case 104:
                case 110:
                case 158:
                case 159:
                case 160:
                case 161:
                case 83:
                case 84:
                case 89:
                case 100:
                case 14:
                case 20:
                case 36:
                    if (alpha > 0)
                        alpha -= 15;
                    if (alpha < 0)
                    {
                        alpha = 0;
                        break;
                    }
                    break;
                case 576:
                case 577:
                    ++localAI[1];
                    if (localAI[1] > 2.0)
                    {
                        if (alpha > 0)
                            alpha -= 15;
                        if (alpha < 0)
                        {
                            alpha = 0;
                            break;
                        }
                        break;
                    }
                    break;
            }
            if (type == 484)
            {
                int index = Dust.NewDust(position, width, height, 78, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity *= 0.1f;
                Main.dust[index].scale = 0.75f;
                Main.dust[index].position = (Main.dust[index].position + Center) / 2f;
                Main.dust[index].position += velocity * Main.rand.Next(0, 101) * 0.01f;
            }
            if (type == 242 || type == 302 || (type == 438 || type == 462) || type == 592)
            {
                float num = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
                if (alpha > 0)
                    alpha -= (byte)(num * 0.9);
                if (alpha < 0)
                    alpha = 0;
            }
            if (type == 638)
            {
                float num1 = velocity.Length();
                if (alpha > 0)
                    alpha -= (byte)(num1 * 0.3);
                if (alpha < 0)
                    alpha = 0;
                Rectangle hitbox = Hitbox;
                hitbox.Offset((int)velocity.X, (int)velocity.Y);
                bool flag = false;
                for (int index = 0; index < 200; ++index)
                {
                    if (Main.npc[index].active && !Main.npc[index].dontTakeDamage && (Main.npc[index].immune[owner] == 0 && npcImmune[index] == 0) && Main.npc[index].Hitbox.Intersects(hitbox))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    int num2 = Main.rand.Next(15, 31);
                    for (int index1 = 0; index1 < num2; ++index1)
                    {
                        int index2 = Dust.NewDust(Center, 0, 0, 229, 0.0f, 0.0f, 100, new Color(), 0.8f);
                        Main.dust[index2].velocity *= 1.6f;
                        --Main.dust[index2].velocity.Y;
                        Main.dust[index2].velocity += velocity;
                        Main.dust[index2].noGravity = true;
                    }
                }
            }
            if (type == 257 || type == 593)
            {
                if (alpha > 0)
                    alpha -= 10;
                if (alpha < 0)
                    alpha = 0;
            }
            if (type == 88)
            {
                if (alpha > 0)
                    alpha -= 10;
                if (alpha < 0)
                    alpha = 0;
            }
            if (type == 532)
                ++ai[0];
            bool flag1 = true;
            switch (type)
            {
                case 606:
                case 616:
                case 634:
                case 635:
                case 638:
                case 639:
                case 592:
                case 593:
                case 601:
                case 498:
                case 576:
                case 577:
                case 585:
                case 467:
                case 468:
                case 469:
                case 472:
                case 483:
                case 484:
                case 485:
                case 449:
                case 459:
                case 462:
                case 389:
                case 435:
                case 436:
                case 438:
                case 440:
                case 442:
                case 348:
                case 349:
                case 350:
                case 355:
                case 374:
                case 376:
                case 302:
                case 323:
                case 325:
                case 270:
                case 279:
                case 283:
                case 284:
                case 285:
                case 286:
                case 287:
                case 299:
                case 257:
                case 259:
                case 265:
                case 184:
                case 242:
                case 248:
                case 158:
                case 159:
                case 160:
                case 161:
                case 180:
                case 98:
                case 100:
                case 104:
                case 110:
                case 55:
                case 83:
                case 84:
                case 88:
                case 89:
                case 20:
                case 36:
                case 38:
                case 5:
                case 14:
                    flag1 = false;
                    break;
            }
            if (flag1)
                ++ai[0];
            if (type == 270)
            {
                int index1 = (int)Player.FindClosest(Center, 1, 1);
                ++ai[1];
                if (ai[1] < 110.0 && ai[1] > 30.0)
                {
                    float num = velocity.Length();
                    Vector2 vector2_1 = Main.player[index1].Center - Center;
                    vector2_1.Normalize();
                    velocity = (velocity * 24f + vector2_1 * num) / 25f;
                    velocity.Normalize();
                    Projectile projectile = this;
                    Vector2 vector2_2 = projectile.velocity * num;
                    projectile.velocity = vector2_2;
                }
                if (ai[0] < 0.0)
                {
                    if (velocity.Length() < 18.0)
                    {
                        Projectile projectile = this;
                        Vector2 vector2 = projectile.velocity * 1.02f;
                        projectile.velocity = vector2;
                    }
                    if (localAI[0] == 0.0)
                    {
                        localAI[0] = 1f;
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 8);
                        for (int index2 = 0; index2 < 10; ++index2)
                        {
                            int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 5, velocity.X, velocity.Y, 0, new Color(), 2f);
                            Main.dust[index3].noGravity = true;
                            Main.dust[index3].velocity = Center - Main.dust[index3].position;
                            Main.dust[index3].velocity.Normalize();
                            Main.dust[index3].velocity *= -5f;
                            Main.dust[index3].velocity += velocity / 2f;
                        }
                    }
                    friendly = false;
                    hostile = true;
                }
            }
            if (type == 585)
            {
                if (localAI[0] == 0.0)
                {
                    localAI[0] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 8);
                    for (int index1 = 0; index1 < 3; ++index1)
                    {
                        int index2 = Dust.NewDust(position, width, height, 27, velocity.X, velocity.Y, 0, new Color(), 2f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity = Center - Main.dust[index2].position;
                        Main.dust[index2].velocity.Normalize();
                        Main.dust[index2].velocity *= -5f;
                        Main.dust[index2].velocity += velocity / 2f;
                        Main.dust[index2].noLight = true;
                    }
                }
                if (alpha > 0)
                    alpha -= 50;
                if (alpha < 0)
                    alpha = 0;
                ++frameCounter;
                if (frameCounter >= 12)
                    frameCounter = 0;
                frame = frameCounter / 2;
                if (frame > 3)
                    frame = 6 - frame;
                Vector3 vector3 = NPCID.Sets.MagicAuraColor[54].ToVector3();
                Lighting.AddLight(Center, vector3.X, vector3.Y, vector3.Z);
                if (Main.rand.Next(3) == 0)
                {
                    int index = Dust.NewDust(new Vector2(position.X + 4f, position.Y + 4f), width - 8, height - 8, 27, velocity.X * 0.2f, velocity.Y * 0.2f, 100, new Color(), 2f);
                    Main.dust[index].position -= velocity * 2f;
                    Main.dust[index].noLight = true;
                    Main.dust[index].noGravity = true;
                    Main.dust[index].velocity.X *= 0.3f;
                    Main.dust[index].velocity.Y *= 0.3f;
                }
            }
            if (type == 594)
            {
                int num = (int)(43.0 - ai[1]) / 13;
                if (num < 1)
                    num = 1;
                int Type = ai[1] < 20.0 ? 6 : 31;
                for (int index1 = 0; index1 < num; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X + 4f, position.Y + 4f), width - 8, height - 8, Type, velocity.X * 0.2f, velocity.Y * 0.2f, 0, new Color(), 2f);
                    Main.dust[index2].position -= velocity * 2f;
                    Main.dust[index2].noLight = true;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity.X *= 0.3f;
                    Main.dust[index2].velocity.Y *= 0.3f;
                    if (Type == 6)
                        Main.dust[index2].fadeIn = Utils.NextFloat(Main.rand) * 2f;
                }
                ++ai[1];
                if (ai[1] > (double)(43 * MaxUpdates))
                {
                    Kill();
                    return;
                }
            }
            if (type == 622)
            {
                int Type = 229;
                if (Main.rand.Next(3) != 0)
                {
                    int index = Dust.NewDust(new Vector2(position.X + 4f, position.Y + 4f), width - 8, height - 8, Type, velocity.X * 0.2f, velocity.Y * 0.2f, 0, new Color(), 1.2f);
                    Main.dust[index].position -= velocity * 2f;
                    Main.dust[index].noLight = true;
                    Main.dust[index].noGravity = true;
                    Main.dust[index].velocity.X *= 0.3f;
                    Main.dust[index].velocity.Y *= 0.3f;
                }
                ++ai[1];
                if (ai[1] > (double)(23 * MaxUpdates))
                {
                    Kill();
                    return;
                }
            }
            if (type == 587)
            {
                Color newColor = Main.hslToRgb(ai[1], 1f, 0.5f);
                newColor.A = 200;
                ++localAI[0];
                if (localAI[0] >= 2.0)
                {
                    if (localAI[0] == 2.0)
                    {
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 5);
                        for (int index1 = 0; index1 < 4; ++index1)
                        {
                            int index2 = Dust.NewDust(position, width, height, 76, velocity.X, velocity.Y, 0, newColor, 1.1f);
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].velocity = Center - Main.dust[index2].position;
                            Main.dust[index2].velocity.Normalize();
                            Main.dust[index2].velocity *= -3f;
                            Main.dust[index2].velocity += velocity / 2f;
                        }
                    }
                    else
                    {
                        ++frame;
                        if (frame > 2)
                            frame = 0;
                        for (int index1 = 0; index1 < 1; ++index1)
                        {
                            int index2 = Dust.NewDust(new Vector2(position.X + 4f, position.Y + 4f), width - 8, height - 8, 76, velocity.X * 0.2f, velocity.Y * 0.2f, 0, newColor, 0.9f);
                            Main.dust[index2].position = Center;
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].velocity = velocity * 0.5f;
                        }
                    }
                }
            }
            if (type == 349)
            {
                frame = (int)ai[0];
                velocity.Y += 0.2f;
                if (localAI[0] == 0.0 || localAI[0] == 2.0)
                {
                    scale += 0.01f;
                    alpha -= 50;
                    if (alpha <= 0)
                    {
                        localAI[0] = 1f;
                        alpha = 0;
                    }
                }
                else if (localAI[0] == 1.0)
                {
                    scale -= 0.01f;
                    alpha += 50;
                    if (alpha >= 255)
                    {
                        localAI[0] = 2f;
                        alpha = 255;
                    }
                }
            }
            if (type == 348)
            {
                if (localAI[1] == 0.0)
                {
                    localAI[1] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 8);
                }
                if (ai[0] == 0.0 || ai[0] == 2.0)
                {
                    scale += 0.01f;
                    alpha -= 50;
                    if (alpha <= 0)
                    {
                        ai[0] = 1f;
                        alpha = 0;
                    }
                }
                else if (ai[0] == 1.0)
                {
                    scale -= 0.01f;
                    alpha += 50;
                    if (alpha >= 255)
                    {
                        ai[0] = 2f;
                        alpha = 255;
                    }
                }
            }
            if (type == 572)
            {
                if (localAI[0] == 0.0)
                {
                    localAI[0] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
                }
                for (int index1 = 0; index1 < 2; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 40, velocity.X, velocity.Y, 100, new Color(), 1f);
                    Main.dust[index2].velocity *= 0.5f;
                    Main.dust[index2].velocity += velocity;
                    Main.dust[index2].velocity *= 0.5f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].scale = 1.2f;
                    Main.dust[index2].position = (Center + position) / 2f;
                }
            }
            if (type == 577)
                Lighting.AddLight(Center, 0.1f, 0.3f, 0.4f);
            else if (type == 576)
            {
                Lighting.AddLight(Center, 0.4f, 0.2f, 0.4f);
                for (int index = 0; index < 5; ++index)
                {
                    Dust dust = Main.dust[Dust.NewDust(position, width, height, 242, velocity.X, velocity.Y, 100, new Color(), 1f)];
                    dust.velocity = Vector2.Zero;
                    dust.position -= velocity / 5f * index;
                    dust.noGravity = true;
                    dust.scale = 0.8f;
                    dust.noLight = true;
                }
            }
            else if (type == 581)
            {
                if (localAI[0] == 0.0)
                {
                    localAI[0] = 1f;
                    Main.PlaySound(2, (int)Center.X, (int)Center.Y, 17);
                }
                for (int index = 0; index < 2; ++index)
                {
                    int Type = Utils.SelectRandom<int>(Main.rand, 229, 161, 161);
                    Dust dust = Main.dust[Dust.NewDust(position, width, height, Type, velocity.X, velocity.Y, 100, new Color(), 1f)];
                    dust.velocity = dust.velocity / 4f + velocity / 2f;
                    dust.noGravity = true;
                    dust.scale = 1.2f;
                    dust.position = Center;
                    dust.noLight = true;
                }
            }
            if (type == 299)
            {
                if (localAI[0] == 6.0)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 8);
                    for (int index1 = 0; index1 < 40; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 181, 0.0f, 0.0f, 100, new Color(), 1f);
                        Main.dust[index2].velocity *= 3f;
                        Main.dust[index2].velocity += velocity * 0.75f;
                        Main.dust[index2].scale *= 1.2f;
                        Main.dust[index2].noGravity = true;
                    }
                }
                ++localAI[0];
                if (localAI[0] > 6.0)
                {
                    for (int index1 = 0; index1 < 3; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 181, velocity.X * 0.2f, velocity.Y * 0.2f, 100, new Color(), 1f);
                        Main.dust[index2].velocity *= 0.6f;
                        Main.dust[index2].scale *= 1.4f;
                        Main.dust[index2].noGravity = true;
                    }
                }
            }
            else if (type == 270)
            {
                if (ai[0] < 0.0)
                    alpha = 0;
                if (alpha > 0)
                    alpha -= 50;
                if (alpha < 0)
                    alpha = 0;
                ++frame;
                if (frame > 2)
                    frame = 0;
                if (ai[0] < 0.0)
                {
                    for (int index1 = 0; index1 < 2; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X + 4f, position.Y + 4f), width - 8, height - 8, 5, velocity.X * 0.2f, velocity.Y * 0.2f, 100, new Color(), 1.5f);
                        Main.dust[index2].position -= velocity;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity.X *= 0.3f;
                        Main.dust[index2].velocity.Y *= 0.3f;
                    }
                }
                else
                {
                    for (int index1 = 0; index1 < 2; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X + 4f, position.Y + 4f), width - 8, height - 8, 6, velocity.X * 0.2f, velocity.Y * 0.2f, 100, new Color(), 2f);
                        Main.dust[index2].position -= velocity * 2f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity.X *= 0.3f;
                        Main.dust[index2].velocity.Y *= 0.3f;
                    }
                }
            }
            if (type == 259)
            {
                if (alpha > 0)
                    alpha -= 10;
                if (alpha < 0)
                    alpha = 0;
            }
            if (type == 265)
            {
                if (alpha > 0)
                    alpha -= 50;
                if (alpha < 0)
                    alpha = 0;
                if (alpha == 0)
                {
                    int index = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 163, velocity.X, velocity.Y, 100, new Color(), 1.2f);
                    Main.dust[index].noGravity = true;
                    Main.dust[index].velocity *= 0.3f;
                    Main.dust[index].velocity -= velocity * 0.4f;
                }
            }
            if (type == 355)
            {
                if (alpha > 0)
                    alpha -= 50;
                if (alpha < 0)
                    alpha = 0;
                if (alpha == 0)
                {
                    int index = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 205, velocity.X, velocity.Y, 100, new Color(), 1.2f);
                    Main.dust[index].noGravity = true;
                    Main.dust[index].velocity *= 0.3f;
                    Main.dust[index].velocity -= velocity * 0.4f;
                }
            }
            if (type == 357)
            {
                if (alpha < 170)
                {
                    for (int index1 = 0; index1 < 10; ++index1)
                    {
                        float x = position.X - velocity.X / 10f * index1;
                        float y = position.Y - velocity.Y / 10f * index1;
                        int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 206, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].alpha = alpha;
                        Main.dust[index2].position.X = x;
                        Main.dust[index2].position.Y = y;
                        Main.dust[index2].velocity *= 0.0f;
                        Main.dust[index2].noGravity = true;
                    }
                }
                if (alpha > 0)
                    alpha -= 25;
                if (alpha < 0)
                    alpha = 0;
            }
            else if (type == 207)
            {
                if (alpha < 170)
                {
                    for (int index1 = 0; index1 < 10; ++index1)
                    {
                        float x = position.X - velocity.X / 10f * index1;
                        float y = position.Y - velocity.Y / 10f * index1;
                        int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 75, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].alpha = alpha;
                        Main.dust[index2].position.X = x;
                        Main.dust[index2].position.Y = y;
                        Main.dust[index2].velocity *= 0.0f;
                        Main.dust[index2].noGravity = true;
                    }
                }
                float num1 = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
                float num2 = localAI[0];
                if (num2 == 0.0)
                {
                    localAI[0] = num1;
                    num2 = num1;
                }
                if (alpha > 0)
                    alpha -= 25;
                if (alpha < 0)
                    alpha = 0;
                float num3 = position.X;
                float num4 = position.Y;
                float num5 = 300f;
                bool flag2 = false;
                int num6 = 0;
                if (ai[1] == 0.0)
                {
                    for (int index = 0; index < 200; ++index)
                    {
                        if (Main.npc[index].CanBeChasedBy(this, false) && (ai[1] == 0.0 || ai[1] == (double)(index + 1)))
                        {
                            float num7 = Main.npc[index].position.X + (float)(Main.npc[index].width / 2);
                            float num8 = Main.npc[index].position.Y + (float)(Main.npc[index].height / 2);
                            float num9 = Math.Abs(position.X + (width / 2) - num7) + Math.Abs(position.Y + (height / 2) - num8);
                            if (num9 < num5 && Collision.CanHit(new Vector2(position.X + (width / 2), position.Y + (height / 2)), 1, 1, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height))
                            {
                                num5 = num9;
                                num3 = num7;
                                num4 = num8;
                                flag2 = true;
                                num6 = index;
                            }
                        }
                    }
                    if (flag2)
                        ai[1] = (float)(num6 + 1);
                    flag2 = false;
                }
                if (ai[1] > 0.0)
                {
                    int index = (int)(ai[1] - 1.0);
                    if (Main.npc[index].active && Main.npc[index].CanBeChasedBy(this, true) && !Main.npc[index].dontTakeDamage)
                    {
                        if ((double)(Math.Abs(position.X + (width / 2) - (Main.npc[index].position.X + (float)(Main.npc[index].width / 2))) + Math.Abs(position.Y + (height / 2) - (Main.npc[index].position.Y + (float)(Main.npc[index].height / 2)))) < 1000.0)
                        {
                            flag2 = true;
                            num3 = Main.npc[index].position.X + (float)(Main.npc[index].width / 2);
                            num4 = Main.npc[index].position.Y + (float)(Main.npc[index].height / 2);
                        }
                    }
                    else
                        ai[1] = 0.0f;
                }
                if (!friendly)
                    flag2 = false;
                if (flag2)
                {
                    float num7 = num2;
                    Vector2 vector2 = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                    float num8 = num3 - vector2.X;
                    float num9 = num4 - vector2.Y;
                    float num10 = (float)Math.Sqrt(num8 * num8 + num9 * num9);
                    float num11 = num7 / num10;
                    float num12 = num8 * num11;
                    float num13 = num9 * num11;
                    int num14 = 8;
                    velocity.X = (velocity.X * (float)(num14 - 1) + num12) / num14;
                    velocity.Y = (velocity.Y * (float)(num14 - 1) + num13) / num14;
                }
            }
            else if (type == 81 || type == 91)
            {
                if (ai[0] >= 20.0)
                {
                    ai[0] = 20f;
                    velocity.Y += 0.07f;
                }
            }
            else if (type == 174 || type == 605)
            {
                if (ai[0] >= 5.0)
                {
                    ai[0] = 5f;
                    velocity.Y += 0.15f;
                }
            }
            else if (type == 337)
            {
                if (position.Y > Main.player[owner].position.Y - 300.0)
                    tileCollide = true;
                if (position.Y < Main.worldSurface * 16.0)
                    tileCollide = true;
                frame = (int)ai[1];
                if (Main.rand.Next(2) == 0)
                {
                    int index = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 197, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index].velocity *= 0.5f;
                    Main.dust[index].noGravity = true;
                }
            }
            else if (type == 645)
            {
                if (ai[1] != -1.0 && position.Y > ai[1])
                    tileCollide = true;
                if (Utils.HasNaNs(position))
                {
                    Kill();
                    return;
                }
                bool flag2 = WorldGen.SolidTile(Framing.GetTileSafely((int)position.X / 16, (int)position.Y / 16));
                Dust dust = Main.dust[Dust.NewDust(new Vector2(position.X, position.Y), width, height, 229, 0.0f, 0.0f, 0, new Color(), 1f)];
                dust.position = Center;
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
                if (flag2)
                    dust.noLight = true;
                if (ai[1] == -1.0)
                {
                    ++ai[0];
                    velocity = Vector2.Zero;
                    tileCollide = false;
                    penetrate = -1;
                    position = Center;
                    width = height = 140;
                    Center = position;
                    alpha -= 10;
                    if (alpha < 0)
                        alpha = 0;
                    if (++frameCounter >= MaxUpdates * 3)
                    {
                        frameCounter = 0;
                        ++frame;
                    }
                    if (ai[0] < (double)(Main.projFrames[type] * MaxUpdates * 3))
                        return;
                    Kill();
                    return;
                }
                alpha = 255;
                if (numUpdates == 0)
                {
                    int num1 = -1;
                    float num2 = 60f;
                    for (int index = 0; index < 200; ++index)
                    {
                        NPC npc = Main.npc[index];
                        if (npc.CanBeChasedBy(this, false))
                        {
                            float num3 = Distance(npc.Center);
                            if (num3 < num2 && Collision.CanHitLine(Center, 0, 0, npc.Center, 0, 0))
                            {
                                num2 = num3;
                                num1 = index;
                            }
                        }
                    }
                    if (num1 != -1)
                    {
                        ai[0] = 0.0f;
                        ai[1] = -1f;
                        netUpdate = true;
                        return;
                    }
                }
            }
            else if (type >= 424 && type <= 426)
            {
                if (position.Y > Main.player[owner].position.Y - 300.0)
                    tileCollide = true;
                if (position.Y < Main.worldSurface * 16.0)
                    tileCollide = true;
                scale = ai[1];
                rotation += velocity.X * 2f;
                Vector2 vector2 = Center + Vector2.Normalize(velocity) * 10f;
                Dust dust1 = Main.dust[Dust.NewDust(position, width, height, 6, 0.0f, 0.0f, 0, new Color(), 1f)];
                dust1.position = vector2;
                dust1.velocity = Utils.RotatedBy(velocity, 1.57079637050629, new Vector2()) * 0.33f + velocity / 4f;
                dust1.position += Utils.RotatedBy(velocity, 1.57079637050629, new Vector2());
                dust1.fadeIn = 0.5f;
                dust1.noGravity = true;
                Dust dust2 = Main.dust[Dust.NewDust(position, width, height, 6, 0.0f, 0.0f, 0, new Color(), 1f)];
                dust2.position = vector2;
                dust2.velocity = Utils.RotatedBy(velocity, -1.57079637050629, new Vector2()) * 0.33f + velocity / 4f;
                dust2.position += Utils.RotatedBy(velocity, -1.57079637050629, new Vector2());
                dust2.fadeIn = 0.5f;
                dust2.noGravity = true;
                for (int index1 = 0; index1 < 1; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].velocity *= 0.5f;
                    Main.dust[index2].scale *= 1.3f;
                    Main.dust[index2].fadeIn = 1f;
                    Main.dust[index2].noGravity = true;
                }
            }
            else if (type == 344)
            {
                ++localAI[1];
                if (localAI[1] > 5.0)
                {
                    alpha -= 50;
                    if (alpha < 0)
                        alpha = 0;
                }
                frame = (int)ai[1];
                if (localAI[1] > 20.0)
                {
                    localAI[1] = 20f;
                    velocity.Y += 0.15f;
                }
                rotation += Main.windSpeed * 0.2f;
                velocity.X += Main.windSpeed * 0.1f;
            }
            else if (type == 336 || type == 345)
            {
                if (type == 345 && localAI[0] == 0.0)
                {
                    localAI[0] = 1f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 1);
                }
                if (ai[0] >= 50.0)
                {
                    ai[0] = 50f;
                    velocity.Y += 0.5f;
                }
            }
            else if (type == 246)
            {
                alpha -= 20;
                if (alpha < 0)
                    alpha = 0;
                if (ai[0] >= 60.0)
                {
                    ai[0] = 60f;
                    velocity.Y += 0.15f;
                }
            }
            else if (type == 311)
            {
                if (alpha > 0)
                    alpha -= 50;
                if (alpha < 0)
                    alpha = 0;
                if (ai[0] >= 30.0)
                {
                    ai[0] = 30f;
                    if (ai[1] == 0.0)
                        ai[1] = 1f;
                    velocity.Y += 0.5f;
                }
            }
            else if (type == 312)
            {
                if (ai[0] >= 5.0)
                    alpha = 0;
                if (ai[0] >= 20.0)
                {
                    ai[0] = 30f;
                    velocity.Y += 0.5f;
                }
            }
            else if (type != 239 && type != 264)
            {
                if (type == 176)
                {
                    if (ai[0] >= 15.0)
                    {
                        ai[0] = 15f;
                        velocity.Y += 0.05f;
                    }
                }
                else if (type == 275 || type == 276)
                {
                    if (alpha > 0)
                        alpha -= 30;
                    if (alpha < 0)
                        alpha = 0;
                    if (ai[0] >= 35.0)
                    {
                        ai[0] = 35f;
                        velocity.Y += 0.025f;
                    }
                    if (Main.expertMode)
                    {
                        float num1 = 18f;
                        int index = (int)Player.FindClosest(Center, 1, 1);
                        Vector2 vector2_1 = Main.player[index].Center - Center;
                        vector2_1.Normalize();
                        vector2_1 *= num1;
                        int num2 = 70;
                        velocity = (velocity * (float)(num2 - 1) + vector2_1) / num2;
                        if (velocity.Length() < 14.0)
                        {
                            velocity.Normalize();
                            Projectile projectile = this;
                            Vector2 vector2_2 = projectile.velocity * 14f;
                            projectile.velocity = vector2_2;
                        }
                        tileCollide = false;
                        if (timeLeft > 180)
                            timeLeft = 180;
                    }
                }
                else if (type == 172)
                {
                    if (ai[0] >= 17.0)
                    {
                        ai[0] = 17f;
                        velocity.Y += 0.085f;
                    }
                }
                else if (type == 117)
                {
                    if (ai[0] >= 35.0)
                    {
                        ai[0] = 35f;
                        velocity.Y += 0.06f;
                    }
                }
                else if (type == 120)
                {
                    int index = Dust.NewDust(new Vector2(position.X - velocity.X, position.Y - velocity.Y), width, height, 67, velocity.X, velocity.Y, 100, new Color(), 1.2f);
                    Main.dust[index].noGravity = true;
                    Main.dust[index].velocity *= 0.3f;
                    if (ai[0] >= 30.0)
                    {
                        ai[0] = 30f;
                        velocity.Y += 0.05f;
                    }
                }
                else if (type == 195)
                {
                    if (ai[0] >= 20.0)
                    {
                        ai[0] = 20f;
                        velocity.Y += 0.075f;
                        tileCollide = true;
                    }
                    else
                        tileCollide = false;
                }
                else if (type == 267 || type == 477 || (type == 478 || type == 479))
                {
                    ++localAI[0];
                    if (localAI[0] > 3.0)
                        alpha = 0;
                    if (ai[0] >= 20.0)
                    {
                        ai[0] = 20f;
                        if (type != 477)
                            velocity.Y += 0.075f;
                    }
                    if (type == 479 && Main.myPlayer == owner)
                    {
                        if (ai[1] >= 0.0)
                            penetrate = -1;
                        else if (penetrate < 0)
                            penetrate = 1;
                        if (ai[1] >= 0.0)
                            ++ai[1];
                        if (ai[1] > Main.rand.Next(5, 30))
                        {
                            ai[1] = -1000f;
                            float num1 = velocity.Length();
                            Vector2 vector2_1 = velocity;
                            vector2_1.Normalize();
                            int num2 = Main.rand.Next(2, 4);
                            if (Main.rand.Next(4) == 0)
                                ++num2;
                            for (int index = 0; index < num2; ++index)
                            {
                                Vector2 vector2_2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                                vector2_2.Normalize();
                                Vector2 vector2_3 = vector2_2 + vector2_1 * 2f;
                                vector2_3.Normalize();
                                vector2_2 = vector2_3 * num1;
                                NewProjectile(Center.X, Center.Y, vector2_2.X, vector2_2.Y, type, damage, knockBack, owner, 0.0f, -1000f);
                            }
                        }
                    }
                    if (type == 478 && Main.myPlayer == owner)
                    {
                        ++ai[1];
                        if (ai[1] > Main.rand.Next(5, 20))
                        {
                            if (timeLeft > 40)
                                timeLeft -= 20;
                            ai[1] = 0.0f;
                            NewProjectile(Center.X, Center.Y, 0.0f, 0.0f, 480, (int)(damage * 0.8), knockBack * 0.5f, owner, 0.0f, 0.0f);
                        }
                    }
                }
                else if (type == 408)
                {
                    if (ai[0] >= 45.0)
                    {
                        ai[0] = 45f;
                        velocity.Y += 0.05f;
                    }
                }
                else if (type == 616)
                {
                    if (alpha < 170)
                    {
                        float num = 3f;
                        for (int index1 = 0; (double)index1 < num; ++index1)
                        {
                            int index2 = Dust.NewDust(position, 1, 1, 229, 0.0f, 0.0f, 0, new Color(), 1f);
                            Main.dust[index2].position = Center - velocity / num * index1;
                            Main.dust[index2].velocity *= 0.0f;
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].alpha = 200;
                            Main.dust[index2].scale = 0.5f;
                        }
                    }
                    float num1 = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
                    float num2 = localAI[0];
                    if (num2 == 0.0)
                    {
                        localAI[0] = num1;
                        num2 = num1;
                    }
                    if (alpha > 0)
                        alpha -= 25;
                    if (alpha < 0)
                        alpha = 0;
                    float num3 = position.X;
                    float num4 = position.Y;
                    float num5 = 800f;
                    bool flag2 = false;
                    int num6 = 0;
                    ++ai[0];
                    if (ai[0] > 20.0)
                    {
                        --ai[0];
                        if (ai[1] == 0.0)
                        {
                            for (int index = 0; index < 200; ++index)
                            {
                                if (Main.npc[index].CanBeChasedBy(this, false) && (ai[1] == 0.0 || ai[1] == (double)(index + 1)))
                                {
                                    float num7 = Main.npc[index].position.X + (float)(Main.npc[index].width / 2);
                                    float num8 = Main.npc[index].position.Y + (float)(Main.npc[index].height / 2);
                                    float num9 = Math.Abs(position.X + (width / 2) - num7) + Math.Abs(position.Y + (height / 2) - num8);
                                    if (num9 < num5 && Collision.CanHit(new Vector2(position.X + (width / 2), position.Y + (height / 2)), 1, 1, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height))
                                    {
                                        num5 = num9;
                                        num3 = num7;
                                        num4 = num8;
                                        flag2 = true;
                                        num6 = index;
                                    }
                                }
                            }
                            if (flag2)
                                ai[1] = (float)(num6 + 1);
                            flag2 = false;
                        }
                        if (ai[1] != 0.0)
                        {
                            int index = (int)(ai[1] - 1.0);
                            if (Main.npc[index].active && Main.npc[index].CanBeChasedBy(this, true) && (double)(Math.Abs(position.X + (width / 2) - (Main.npc[index].position.X + (float)(Main.npc[index].width / 2))) + Math.Abs(position.Y + (height / 2) - (Main.npc[index].position.Y + (float)(Main.npc[index].height / 2)))) < 1000.0)
                            {
                                flag2 = true;
                                num3 = Main.npc[index].position.X + (float)(Main.npc[index].width / 2);
                                num4 = Main.npc[index].position.Y + (float)(Main.npc[index].height / 2);
                            }
                        }
                        if (!friendly)
                            flag2 = false;
                        if (flag2)
                        {
                            float num7 = num2;
                            Vector2 vector2 = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                            float num8 = num3 - vector2.X;
                            float num9 = num4 - vector2.Y;
                            float num10 = (float)Math.Sqrt(num8 * num8 + num9 * num9);
                            float num11 = num7 / num10;
                            float num12 = num8 * num11;
                            float num13 = num9 * num11;
                            int num14 = 8;
                            velocity.X = (velocity.X * (float)(num14 - 1) + num12) / num14;
                            velocity.Y = (velocity.Y * (float)(num14 - 1) + num13) / num14;
                        }
                    }
                }
                else if (type == 507 || type == 508)
                {
                    if (ai[0] > 45.0)
                    {
                        velocity.X *= 0.98f;
                        velocity.Y += 0.3f;
                    }
                }
                else if (type == 495)
                {
                    int index = Dust.NewDust(new Vector2(position.X - velocity.X, position.Y - velocity.Y), width, height, 27, velocity.X, velocity.Y, 100, new Color(), 1.2f);
                    Main.dust[index].noGravity = true;
                    Main.dust[index].velocity *= 0.3f;
                    if (ai[0] >= 30.0)
                    {
                        ai[0] = 30f;
                        velocity.Y += 0.04f;
                    }
                }
                else if (type == 498)
                {
                    if (localAI[0] == 0.0)
                    {
                        ++localAI[0];
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 17);
                    }
                    ++ai[0];
                    if (ai[0] >= 50.0)
                    {
                        velocity.X *= 0.98f;
                        velocity.Y += 0.15f;
                        rotation += direction * 0.5f;
                    }
                    else
                        rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
                }
                else if (type == 437)
                {
                    if (ai[0] >= 12.0)
                    {
                        if (ai[0] >= 20.0)
                            Kill();
                        alpha += 30;
                    }
                }
                else if (type != 442 && type != 634 && type != 635)
                {
                    if (type == 639)
                    {
                        if (timeLeft <= MaxUpdates * 45 - 14)
                            velocity.Y += 0.1f;
                    }
                    else if (ai[0] >= 15.0)
                    {
                        ai[0] = 15f;
                        velocity.Y += 0.1f;
                    }
                }
            }
            if (type == 248)
            {
                if (velocity.X < 0.0)
                    rotation -= (float)((Math.Abs(velocity.X) + Math.Abs(velocity.Y)) * 0.0500000007450581);
                else
                    rotation += (float)((Math.Abs(velocity.X) + Math.Abs(velocity.Y)) * 0.0500000007450581);
            }
            else if (type == 270 || type == 585 || type == 601)
            {
                spriteDirection = direction;
                rotation = direction >= 0 ? (float)Math.Atan2(velocity.Y, velocity.X) : (float)Math.Atan2(-velocity.Y, -velocity.X);
            }
            else if (type == 311)
            {
                if (ai[1] != 0.0)
                    rotation += (float)(velocity.X * 0.100000001490116 + Main.rand.Next(-10, 11) * 0.025000000372529);
                else
                    rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
            }
            else if (type == 312)
                rotation += velocity.X * 0.02f;
            else if (type == 408)
            {
                rotation = Utils.ToRotation(velocity);
                if (direction == -1)
                    rotation += 3.141593f;
            }
            else if (type == 435 || type == 459)
            {
                rotation = Utils.ToRotation(velocity);
                if (direction == -1)
                    rotation += 3.141593f;
            }
            else if (type == 436)
            {
                rotation = Utils.ToRotation(velocity);
                rotation += 3.141593f;
                if (direction == -1)
                    rotation += 3.141593f;
            }
            else if (type == 469)
            {
                if (velocity.X > 0.0)
                {
                    spriteDirection = -1;
                    rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
                }
                else
                {
                    spriteDirection = 1;
                    rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
                }
            }
            else if (type == 477)
            {
                if (localAI[1] < 5.0)
                {
                    rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
                    ++localAI[1];
                }
                else
                    rotation = (float)((rotation * 2.0 + Math.Atan2(velocity.Y, velocity.X) + 1.57000005245209) / 3.0);
            }
            else if (type == 532)
                rotation += (float)(0.200000002980232 + Math.Abs(velocity.X) * 0.100000001490116);
            else if (type == 483)
                rotation += velocity.X * 0.05f;
            else if (type == 485)
            {
                velocity = (velocity * 39f + new Vector2(ai[0], ai[1])) / 40f;
                int index = Dust.NewDust(position, width, height, 6, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity *= 0.2f;
                Main.dust[index].position = (Main.dust[index].position + Center) / 2f;
                ++frameCounter;
                if (frameCounter >= 2)
                {
                    frameCounter = 0;
                    ++frame;
                    if (frame >= 5)
                        frame = 0;
                }
                if (velocity.X < 0.0)
                {
                    spriteDirection = -1;
                    rotation = (float)Math.Atan2(-velocity.Y, -velocity.X);
                }
                else
                {
                    spriteDirection = 1;
                    rotation = (float)Math.Atan2(velocity.Y, velocity.X);
                }
            }
            else if (type == 640)
            {
                if (velocity != Vector2.Zero)
                    rotation = Utils.ToRotation(velocity) + 1.570796f;
            }
            else if (type != 344 && type != 498)
                rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
            if (velocity.Y <= 16.0)
                return;
            velocity.Y = 16f;
        }

        private void AI_026()
        {
            if (!Main.player[owner].active)
            {
                active = false;
            }
            else
            {
                bool flag1 = false;
                bool flag2 = false;
                bool flag3 = false;
                bool flag4 = false;
                int num1 = 85;
                if (type == 324)
                    num1 = 120;
                if (type == 112)
                    num1 = 100;
                if (type == 127)
                    num1 = 50;
                if (type >= 191 && type <= 194)
                {
                    if (lavaWet)
                    {
                        ai[0] = 1f;
                        ai[1] = 0.0f;
                    }
                    num1 = 60 + 30 * minionPos;
                }
                else if (type == 266)
                    num1 = 60 + 30 * minionPos;
                if (type == 111)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].bunny = false;
                    if (Main.player[owner].bunny)
                        timeLeft = 2;
                }
                if (type == 112)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].penguin = false;
                    if (Main.player[owner].penguin)
                        timeLeft = 2;
                }
                if (type == 334)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].puppy = false;
                    if (Main.player[owner].puppy)
                        timeLeft = 2;
                }
                if (type == 353)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].grinch = false;
                    if (Main.player[owner].grinch)
                        timeLeft = 2;
                }
                if (type == 127)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].turtle = false;
                    if (Main.player[owner].turtle)
                        timeLeft = 2;
                }
                if (type == 175)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].eater = false;
                    if (Main.player[owner].eater)
                        timeLeft = 2;
                }
                if (type == 197)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].skeletron = false;
                    if (Main.player[owner].skeletron)
                        timeLeft = 2;
                }
                if (type == 198)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].hornet = false;
                    if (Main.player[owner].hornet)
                        timeLeft = 2;
                }
                if (type == 199)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].tiki = false;
                    if (Main.player[owner].tiki)
                        timeLeft = 2;
                }
                if (type == 200)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].lizard = false;
                    if (Main.player[owner].lizard)
                        timeLeft = 2;
                }
                if (type == 208)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].parrot = false;
                    if (Main.player[owner].parrot)
                        timeLeft = 2;
                }
                if (type == 209)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].truffle = false;
                    if (Main.player[owner].truffle)
                        timeLeft = 2;
                }
                if (type == 210)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].sapling = false;
                    if (Main.player[owner].sapling)
                        timeLeft = 2;
                }
                if (type == 324)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].cSapling = false;
                    if (Main.player[owner].cSapling)
                        timeLeft = 2;
                }
                if (type == 313)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].spider = false;
                    if (Main.player[owner].spider)
                        timeLeft = 2;
                }
                if (type == 314)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].squashling = false;
                    if (Main.player[owner].squashling)
                        timeLeft = 2;
                }
                if (type == 211)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].wisp = false;
                    if (Main.player[owner].wisp)
                        timeLeft = 2;
                }
                if (type == 236)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].dino = false;
                    if (Main.player[owner].dino)
                        timeLeft = 2;
                }
                if (type == 499)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].babyFaceMonster = false;
                    if (Main.player[owner].babyFaceMonster)
                        timeLeft = 2;
                }
                if (type == 266)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].slime = false;
                    if (Main.player[owner].slime)
                        timeLeft = 2;
                }
                if (type == 268)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].eyeSpring = false;
                    if (Main.player[owner].eyeSpring)
                        timeLeft = 2;
                }
                if (type == 269)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].snowman = false;
                    if (Main.player[owner].snowman)
                        timeLeft = 2;
                }
                if (type == 319)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].blackCat = false;
                    if (Main.player[owner].blackCat)
                        timeLeft = 2;
                }
                if (type == 380)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].zephyrfish = false;
                    if (Main.player[owner].zephyrfish)
                        timeLeft = 2;
                }
                if (type >= 191 && type <= 194)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].pygmy = false;
                    if (Main.player[owner].pygmy)
                        timeLeft = Main.rand.Next(2, 10);
                }
                if (type >= 390 && type <= 392)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].spiderMinion = false;
                    if (Main.player[owner].spiderMinion)
                        timeLeft = 2;
                }
                if (type == 398)
                {
                    if (Main.player[owner].dead)
                        Main.player[owner].miniMinotaur = false;
                    if (Main.player[owner].miniMinotaur)
                        timeLeft = 2;
                }
                if (type >= 191 && type <= 194 || type == 266 || type >= 390 && type <= 392)
                {
                    int num2 = 10;
                    int num3 = 40 * (minionPos + 1) * Main.player[owner].direction;
                    if (Main.player[owner].position.X + (Main.player[owner].width / 2) < position.X + (width / 2) - num2 + num3)
                        flag1 = true;
                    else if (Main.player[owner].position.X + (Main.player[owner].width / 2) > position.X + (width / 2) + num2 + num3)
                        flag2 = true;
                }
                else if (Main.player[owner].position.X + (Main.player[owner].width / 2) < position.X + (width / 2) - num1)
                    flag1 = true;
                else if (Main.player[owner].position.X + (Main.player[owner].width / 2) > position.X + (width / 2) + num1)
                    flag2 = true;
                if (type == 175)
                {
                    float num2 = 0.1f;
                    tileCollide = false;
                    int num3 = 300;
                    Vector2 vector2_1 = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                    float num4 = Main.player[owner].position.X + (Main.player[owner].width / 2) - vector2_1.X;
                    float num5 = Main.player[owner].position.Y + (Main.player[owner].height / 2) - vector2_1.Y;
                    if (type == 127)
                        num5 = Main.player[owner].position.Y - vector2_1.Y;
                    float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                    float num7 = 7f;
                    if (num6 < num3 && Main.player[owner].velocity.Y == 0.0 && (position.Y + (double)height <= Main.player[owner].position.Y + Main.player[owner].height && !Collision.SolidCollision(position, width, height)))
                    {
                        ai[0] = 0.0f;
                        if (velocity.Y < -6.0)
                            velocity.Y = -6f;
                    }
                    if (num6 < 150.0)
                    {
                        if (Math.Abs(velocity.X) > 2.0 || Math.Abs(velocity.Y) > 2.0)
                        {
                            Projectile projectile = this;
                            Vector2 vector2_2 = projectile.velocity * 0.99f;
                            projectile.velocity = vector2_2;
                        }
                        num2 = 0.01f;
                        if (num4 < -2.0)
                            num4 = -2f;
                        if (num4 > 2.0)
                            num4 = 2f;
                        if (num5 < -2.0)
                            num5 = -2f;
                        if (num5 > 2.0)
                            num5 = 2f;
                    }
                    else
                    {
                        if (num6 > 300.0)
                            num2 = 0.2f;
                        float num8 = num7 / num6;
                        num4 *= num8;
                        num5 *= num8;
                    }
                    if (Math.Abs(num4) > Math.Abs(num5) || num2 == 0.0500000007450581)
                    {
                        if (velocity.X < num4)
                        {
                            velocity.X += num2;
                            if (num2 > 0.0500000007450581 && velocity.X < 0.0)
                                velocity.X += num2;
                        }
                        if (velocity.X > num4)
                        {
                            velocity.X -= num2;
                            if (num2 > 0.0500000007450581 && velocity.X > 0.0)
                                velocity.X -= num2;
                        }
                    }
                    if (Math.Abs(num4) <= Math.Abs(num5) || num2 == 0.0500000007450581)
                    {
                        if (velocity.Y < num5)
                        {
                            velocity.Y += num2;
                            if (num2 > 0.0500000007450581 && velocity.Y < 0.0)
                                velocity.Y += num2;
                        }
                        if (velocity.Y > num5)
                        {
                            velocity.Y -= num2;
                            if (num2 > 0.0500000007450581 && velocity.Y > 0.0)
                                velocity.Y -= num2;
                        }
                    }
                    rotation = (float)Math.Atan2(velocity.Y, velocity.X) - 1.57f;
                    ++frameCounter;
                    if (frameCounter > 6)
                    {
                        ++frame;
                        frameCounter = 0;
                    }
                    if (frame <= 1)
                        return;
                    frame = 0;
                }
                else if (type == 197)
                {
                    float num2 = 0.1f;
                    tileCollide = false;
                    int num3 = 300;
                    Vector2 vector2_1 = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                    float num4 = Main.player[owner].position.X + (Main.player[owner].width / 2) - vector2_1.X;
                    float num5 = Main.player[owner].position.Y + (Main.player[owner].height / 2) - vector2_1.Y;
                    if (type == 127)
                        num5 = Main.player[owner].position.Y - vector2_1.Y;
                    float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                    float num7 = 3f;
                    if (num6 > 500.0)
                        localAI[0] = 10000f;
                    if (localAI[0] >= 10000.0)
                        num7 = 14f;
                    if (num6 < num3 && Main.player[owner].velocity.Y == 0.0 && (position.Y + (double)height <= Main.player[owner].position.Y + Main.player[owner].height && !Collision.SolidCollision(position, width, height)))
                    {
                        ai[0] = 0.0f;
                        if (velocity.Y < -6.0)
                            velocity.Y = -6f;
                    }
                    if (num6 < 150.0)
                    {
                        if (Math.Abs(velocity.X) > 2.0 || Math.Abs(velocity.Y) > 2.0)
                        {
                            Projectile projectile = this;
                            Vector2 vector2_2 = projectile.velocity * 0.99f;
                            projectile.velocity = vector2_2;
                        }
                        num2 = 0.01f;
                        if (num4 < -2.0)
                            num4 = -2f;
                        if (num4 > 2.0)
                            num4 = 2f;
                        if (num5 < -2.0)
                            num5 = -2f;
                        if (num5 > 2.0)
                            num5 = 2f;
                    }
                    else
                    {
                        if (num6 > 300.0)
                            num2 = 0.2f;
                        float num8 = num7 / num6;
                        num4 *= num8;
                        num5 *= num8;
                    }
                    if (velocity.X < num4)
                    {
                        velocity.X += num2;
                        if (num2 > 0.0500000007450581 && velocity.X < 0.0)
                            velocity.X += num2;
                    }
                    if (velocity.X > num4)
                    {
                        velocity.X -= num2;
                        if (num2 > 0.0500000007450581 && velocity.X > 0.0)
                            velocity.X -= num2;
                    }
                    if (velocity.Y < num5)
                    {
                        velocity.Y += num2;
                        if (num2 > 0.0500000007450581 && velocity.Y < 0.0)
                            velocity.Y += num2;
                    }
                    if (velocity.Y > num5)
                    {
                        velocity.Y -= num2;
                        if (num2 > 0.0500000007450581 && velocity.Y > 0.0)
                            velocity.Y -= num2;
                    }
                    localAI[0] += Main.rand.Next(10);
                    if (localAI[0] > 10000.0)
                    {
                        if (localAI[1] == 0.0)
                            localAI[1] = velocity.X >= 0.0 ? 1f : -1f;
                        rotation += 0.25f * localAI[1];
                        if (localAI[0] > 12000.0)
                            localAI[0] = 0.0f;
                    }
                    else
                    {
                        localAI[1] = 0.0f;
                        float num8 = velocity.X * 0.1f;
                        if (rotation > num8)
                        {
                            rotation -= (float)((Math.Abs(velocity.X) + Math.Abs(velocity.Y)) * 0.00999999977648258);
                            if (rotation < num8)
                                rotation = num8;
                        }
                        if (rotation < num8)
                        {
                            rotation += (float)((Math.Abs(velocity.X) + Math.Abs(velocity.Y)) * 0.00999999977648258);
                            if (rotation > num8)
                                rotation = num8;
                        }
                    }
                    if (rotation > 6.28)
                        rotation -= 6.28f;
                    if (rotation >= -6.28)
                        return;
                    rotation += 6.28f;
                }
                else if (type == 198 || type == 380)
                {
                    float num2 = 0.4f;
                    if (type == 380)
                        num2 = 0.3f;
                    tileCollide = false;
                    int num3 = 100;
                    Vector2 vector2_1 = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                    float num4 = Main.player[owner].position.X + (Main.player[owner].width / 2) - vector2_1.X;
                    float num5 = Main.player[owner].position.Y + (Main.player[owner].height / 2) - vector2_1.Y + Main.rand.Next(-10, 21);
                    float num6 = num4 + Main.rand.Next(-10, 21) + (60 * -Main.player[owner].direction);
                    float num7 = num5 - 60f;
                    if (type == 127)
                        num7 = Main.player[owner].position.Y - vector2_1.Y;
                    float num8 = (float)Math.Sqrt(num6 * num6 + num7 * num7);
                    float num9 = 14f;
                    if (type == 380)
                        num9 = 6f;
                    if (num8 < num3 && Main.player[owner].velocity.Y == 0.0 && (position.Y + (double)height <= Main.player[owner].position.Y + Main.player[owner].height && !Collision.SolidCollision(position, width, height)))
                    {
                        ai[0] = 0.0f;
                        if (velocity.Y < -6.0)
                            velocity.Y = -6f;
                    }
                    if (num8 < 50.0)
                    {
                        if (Math.Abs(velocity.X) > 2.0 || Math.Abs(velocity.Y) > 2.0)
                        {
                            Projectile projectile = this;
                            Vector2 vector2_2 = projectile.velocity * 0.99f;
                            projectile.velocity = vector2_2;
                        }
                        num2 = 0.01f;
                    }
                    else
                    {
                        if (type == 380)
                        {
                            if (num8 < 100.0)
                                num2 = 0.1f;
                            if (num8 > 300.0)
                                num2 = 0.4f;
                        }
                        else if (type == 198)
                        {
                            if (num8 < 100.0)
                                num2 = 0.1f;
                            if (num8 > 300.0)
                                num2 = 0.6f;
                        }
                        float num10 = num9 / num8;
                        num6 *= num10;
                        num7 *= num10;
                    }
                    if (velocity.X < num6)
                    {
                        velocity.X += num2;
                        if (num2 > 0.0500000007450581 && velocity.X < 0.0)
                            velocity.X += num2;
                    }
                    if (velocity.X > num6)
                    {
                        velocity.X -= num2;
                        if (num2 > 0.0500000007450581 && velocity.X > 0.0)
                            velocity.X -= num2;
                    }
                    if (velocity.Y < num7)
                    {
                        velocity.Y += num2;
                        if (num2 > 0.0500000007450581 && velocity.Y < 0.0)
                            velocity.Y += num2 * 2f;
                    }
                    if (velocity.Y > num7)
                    {
                        velocity.Y -= num2;
                        if (num2 > 0.0500000007450581 && velocity.Y > 0.0)
                            velocity.Y -= num2 * 2f;
                    }
                    if (velocity.X > 0.25)
                        direction = -1;
                    else if (velocity.X < -0.25)
                        direction = 1;
                    spriteDirection = direction;
                    rotation = velocity.X * 0.05f;
                    ++frameCounter;
                    int num11 = 2;
                    if (type == 380)
                        num11 = 5;
                    if (frameCounter > num11)
                    {
                        ++frame;
                        frameCounter = 0;
                    }
                    if (frame <= 3)
                        return;
                    frame = 0;
                }
                else if (type == 211)
                {
                    float num2 = 0.2f;
                    float num3 = 5f;
                    tileCollide = false;
                    Vector2 vector2_1 = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                    float num4 = Main.player[owner].position.X + (Main.player[owner].width / 2) - vector2_1.X;
                    float num5 = Main.player[owner].position.Y + Main.player[owner].gfxOffY + (Main.player[owner].height / 2) - vector2_1.Y;
                    if (Main.player[owner].controlLeft)
                        num4 -= 120f;
                    else if (Main.player[owner].controlRight)
                        num4 += 120f;
                    float num6;
                    if (Main.player[owner].controlDown)
                    {
                        num6 = num5 + 120f;
                    }
                    else
                    {
                        if (Main.player[owner].controlUp)
                            num5 -= 120f;
                        num6 = num5 - 60f;
                    }
                    float num7 = (float)Math.Sqrt(num4 * num4 + num6 * num6);
                    if (num7 > 1000.0)
                    {
                        position.X += num4;
                        position.Y += num6;
                    }
                    if (localAI[0] == 1.0)
                    {
                        if (num7 < 10.0 && Math.Abs(Main.player[owner].velocity.X) + Math.Abs(Main.player[owner].velocity.Y) < num3 && Main.player[owner].velocity.Y == 0.0)
                            localAI[0] = 0.0f;
                        float num8 = 12f;
                        if (num7 < num8)
                        {
                            velocity.X = num4;
                            velocity.Y = num6;
                        }
                        else
                        {
                            float num9 = num8 / num7;
                            velocity.X = num4 * num9;
                            velocity.Y = num6 * num9;
                        }
                        if (velocity.X > 0.5)
                            direction = -1;
                        else if (velocity.X < -0.5)
                            direction = 1;
                        spriteDirection = direction;
                        rotation -= (float)(0.200000002980232 + Math.Abs(velocity.X) * 0.025000000372529) * direction;
                        ++frameCounter;
                        if (frameCounter > 3)
                        {
                            ++frame;
                            frameCounter = 0;
                        }
                        if (frame < 5)
                            frame = 5;
                        if (frame > 9)
                            frame = 5;
                        for (int index1 = 0; index1 < 2; ++index1)
                        {
                            int index2 = Dust.NewDust(new Vector2(position.X + 3f, position.Y + 4f), 14, 14, 156, 0.0f, 0.0f, 0, new Color(), 1f);
                            Main.dust[index2].velocity *= 0.2f;
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].scale = 1.25f;
                            Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(Main.player[owner].cLight, Main.player[owner]);
                        }
                    }
                    else
                    {
                        if (num7 > 200.0)
                            localAI[0] = 1f;
                        if (velocity.X > 0.5)
                            direction = -1;
                        else if (velocity.X < -0.5)
                            direction = 1;
                        spriteDirection = direction;
                        if (num7 < 10.0)
                        {
                            velocity.X = num4;
                            velocity.Y = num6;
                            rotation = velocity.X * 0.05f;
                            if (num7 < num3)
                            {
                                Projectile projectile1 = this;
                                Vector2 vector2_2 = projectile1.position + velocity;
                                projectile1.position = vector2_2;
                                Projectile projectile2 = this;
                                Vector2 vector2_3 = projectile2.velocity * 0.0f;
                                projectile2.velocity = vector2_3;
                                num2 = 0.0f;
                            }
                            direction = -Main.player[owner].direction;
                        }
                        float num8 = num3 / num7;
                        float num9 = num4 * num8;
                        float num10 = num6 * num8;
                        if (velocity.X < num9)
                        {
                            velocity.X += num2;
                            if (velocity.X < 0.0)
                                velocity.X *= 0.99f;
                        }
                        if (velocity.X > num9)
                        {
                            velocity.X -= num2;
                            if (velocity.X > 0.0)
                                velocity.X *= 0.99f;
                        }
                        if (velocity.Y < num10)
                        {
                            velocity.Y += num2;
                            if (velocity.Y < 0.0)
                                velocity.Y *= 0.99f;
                        }
                        if (velocity.Y > num10)
                        {
                            velocity.Y -= num2;
                            if (velocity.Y > 0.0)
                                velocity.Y *= 0.99f;
                        }
                        if (velocity.X != 0.0 || velocity.Y != 0.0)
                            rotation = velocity.X * 0.05f;
                        ++frameCounter;
                        if (frameCounter > 3)
                        {
                            ++frame;
                            frameCounter = 0;
                        }
                        if (frame <= 4)
                            return;
                        frame = 0;
                    }
                }
                else if (type == 199)
                {
                    float num2 = 0.1f;
                    tileCollide = false;
                    int num3 = 200;
                    Vector2 vector2 = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                    float num4 = Main.player[owner].position.X + (Main.player[owner].width / 2) - vector2.X;
                    float num5 = Main.player[owner].position.Y + (Main.player[owner].height / 2) - vector2.Y - 60f;
                    float num6 = num4 - 2f;
                    if (type == 127)
                        num5 = Main.player[owner].position.Y - vector2.Y;
                    float num7 = (float)Math.Sqrt(num6 * num6 + num5 * num5);
                    float num8 = 4f;
                    float num9 = num7;
                    if (num7 < num3 && Main.player[owner].velocity.Y == 0.0 && (position.Y + (double)height <= Main.player[owner].position.Y + Main.player[owner].height && !Collision.SolidCollision(position, width, height)))
                    {
                        ai[0] = 0.0f;
                        if (velocity.Y < -6.0)
                            velocity.Y = -6f;
                    }
                    if (num7 < 4.0)
                    {
                        velocity.X = num6;
                        velocity.Y = num5;
                        num2 = 0.0f;
                    }
                    else
                    {
                        if (num7 > 350.0)
                        {
                            num2 = 0.2f;
                            num8 = 10f;
                        }
                        float num10 = num8 / num7;
                        num6 *= num10;
                        num5 *= num10;
                    }
                    if (velocity.X < num6)
                    {
                        velocity.X += num2;
                        if (velocity.X < 0.0)
                            velocity.X += num2;
                    }
                    if (velocity.X > num6)
                    {
                        velocity.X -= num2;
                        if (velocity.X > 0.0)
                            velocity.X -= num2;
                    }
                    if (velocity.Y < num5)
                    {
                        velocity.Y += num2;
                        if (velocity.Y < 0.0)
                            velocity.Y += num2;
                    }
                    if (velocity.Y > num5)
                    {
                        velocity.Y -= num2;
                        if (velocity.Y > 0.0)
                            velocity.Y -= num2;
                    }
                    direction = -Main.player[owner].direction;
                    spriteDirection = 1;
                    rotation = velocity.Y * 0.05f * (float)-direction;
                    if (num9 >= 50.0)
                    {
                        ++frameCounter;
                        if (frameCounter <= 6)
                            return;
                        frameCounter = 0;
                        if (velocity.X < 0.0)
                        {
                            if (frame < 2)
                                ++frame;
                            if (frame <= 2)
                                return;
                            --frame;
                        }
                        else
                        {
                            if (frame < 6)
                                ++frame;
                            if (frame <= 6)
                                return;
                            --frame;
                        }
                    }
                    else
                    {
                        ++frameCounter;
                        if (frameCounter > 6)
                        {
                            frame += direction;
                            frameCounter = 0;
                        }
                        if (frame > 7)
                            frame = 0;
                        if (frame >= 0)
                            return;
                        frame = 7;
                    }
                }
                else
                {
                    if (ai[1] == 0.0)
                    {
                        int num2 = 500;
                        if (type == 127)
                            num2 = 200;
                        if (type == 208)
                            num2 = 300;
                        if (type >= 191 && type <= 194 || type == 266 || type >= 390 && type <= 392)
                        {
                            num2 += 40 * minionPos;
                            if (localAI[0] > 0.0)
                                num2 += 500;
                            if (type == 266 && localAI[0] > 0.0)
                                num2 += 100;
                            if (type >= 390 && type <= 392 && localAI[0] > 0.0)
                                num2 += 400;
                        }
                        if (Main.player[owner].rocketDelay2 > 0)
                            ai[0] = 1f;
                        Vector2 vector2 = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                        float num3 = Main.player[owner].position.X + (Main.player[owner].width / 2) - vector2.X;
                        if (type >= 191)
                        {
                            int num4 = type;
                        }
                        float num5 = Main.player[owner].position.Y + (Main.player[owner].height / 2) - vector2.Y;
                        float num6 = (float)Math.Sqrt(num3 * num3 + num5 * num5);
                        if (num6 > 2000.0)
                        {
                            position.X = Main.player[owner].position.X + (Main.player[owner].width / 2) - (width / 2);
                            position.Y = Main.player[owner].position.Y + (Main.player[owner].height / 2) - (height / 2);
                        }
                        else if (num6 > num2 || Math.Abs(num5) > 300.0 && ((type < 191 || type > 194) && type != 266 && (type < 390 || type > 392) || localAI[0] <= 0.0))
                        {
                            if (type != 324)
                            {
                                if (num5 > 0.0 && velocity.Y < 0.0)
                                    velocity.Y = 0.0f;
                                if (num5 < 0.0 && velocity.Y > 0.0)
                                    velocity.Y = 0.0f;
                            }
                            ai[0] = 1f;
                        }
                    }
                    if (type == 209 && ai[0] != 0.0)
                    {
                        if (Main.player[owner].velocity.Y == 0.0 && alpha >= 100)
                        {
                            position.X = Main.player[owner].position.X + (Main.player[owner].width / 2) - (width / 2);
                            position.Y = Main.player[owner].position.Y + (float)Main.player[owner].height - height;
                            ai[0] = 0.0f;
                        }
                        else
                        {
                            velocity.X = 0.0f;
                            velocity.Y = 0.0f;
                            alpha += 5;
                            if (alpha <= 255)
                                return;
                            alpha = 255;
                        }
                    }
                    else if (ai[0] != 0.0)
                    {
                        float num2 = 0.2f;
                        int num3 = 200;
                        if (type == 127)
                            num3 = 100;
                        if (type >= 191 && type <= 194)
                        {
                            num2 = 0.5f;
                            num3 = 100;
                        }
                        tileCollide = false;
                        Vector2 vector2_1 = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                        float num4 = Main.player[owner].position.X + (Main.player[owner].width / 2) - vector2_1.X;
                        if (type >= 191 && type <= 194 || type == 266 || type >= 390 && type <= 392)
                        {
                            num4 -= (float)(40 * Main.player[owner].direction);
                            float num5 = 700f;
                            if (type >= 191 && type <= 194)
                                num5 += 100f;
                            bool flag5 = false;
                            int num6 = -1;
                            for (int index = 0; index < 200; ++index)
                            {
                                if (Main.npc[index].CanBeChasedBy(this, false))
                                {
                                    float num7 = Main.npc[index].position.X + (float)(Main.npc[index].width / 2);
                                    float num8 = Main.npc[index].position.Y + (float)(Main.npc[index].height / 2);
                                    if ((double)(Math.Abs(Main.player[owner].position.X + (Main.player[owner].width / 2) - num7) + Math.Abs(Main.player[owner].position.Y + (Main.player[owner].height / 2) - num8)) < num5)
                                    {
                                        if (Collision.CanHit(position, width, height, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height))
                                            num6 = index;
                                        flag5 = true;
                                        break;
                                    }
                                }
                            }
                            if (!flag5)
                                num4 -= (float)(40 * minionPos * Main.player[owner].direction);
                            if (flag5 && num6 >= 0)
                                ai[0] = 0.0f;
                        }
                        float num9 = Main.player[owner].position.Y + (Main.player[owner].height / 2) - vector2_1.Y;
                        if (type == 127)
                            num9 = Main.player[owner].position.Y - vector2_1.Y;
                        float num10 = (float)Math.Sqrt(num4 * num4 + num9 * num9);
                        float num11 = 10f;
                        float num12 = num10;
                        if (type == 111)
                            num11 = 11f;
                        if (type == 127)
                            num11 = 9f;
                        if (type == 324)
                            num11 = 20f;
                        if (type >= 191 && type <= 194)
                        {
                            num2 = 0.4f;
                            num11 = 12f;
                            if (num11 < Math.Abs(Main.player[owner].velocity.X) + Math.Abs(Main.player[owner].velocity.Y))
                                num11 = Math.Abs(Main.player[owner].velocity.X) + Math.Abs(Main.player[owner].velocity.Y);
                        }
                        if (type == 208 && Math.Abs(Main.player[owner].velocity.X) + Math.Abs(Main.player[owner].velocity.Y) > 4.0)
                            num3 = -1;
                        if (num10 < num3 && Main.player[owner].velocity.Y == 0.0 && (position.Y + (double)height <= Main.player[owner].position.Y + Main.player[owner].height && !Collision.SolidCollision(position, width, height)))
                        {
                            ai[0] = 0.0f;
                            if (velocity.Y < -6.0)
                                velocity.Y = -6f;
                        }
                        float num13;
                        float num14;
                        if (num10 < 60.0)
                        {
                            num13 = velocity.X;
                            num14 = velocity.Y;
                        }
                        else
                        {
                            float num5 = num11 / num10;
                            num13 = num4 * num5;
                            num14 = num9 * num5;
                        }
                        if (type == 324)
                        {
                            if (num12 > 1000.0)
                            {
                                if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) < num11 - 1.25)
                                {
                                    Projectile projectile = this;
                                    Vector2 vector2_2 = projectile.velocity * 1.025f;
                                    projectile.velocity = vector2_2;
                                }
                                if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) > num11 + 1.25)
                                {
                                    Projectile projectile = this;
                                    Vector2 vector2_2 = projectile.velocity * 0.975f;
                                    projectile.velocity = vector2_2;
                                }
                            }
                            else if (num12 > 600.0)
                            {
                                if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) < num11 - 1.0)
                                {
                                    Projectile projectile = this;
                                    Vector2 vector2_2 = projectile.velocity * 1.05f;
                                    projectile.velocity = vector2_2;
                                }
                                if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) > num11 + 1.0)
                                {
                                    Projectile projectile = this;
                                    Vector2 vector2_2 = projectile.velocity * 0.95f;
                                    projectile.velocity = vector2_2;
                                }
                            }
                            else if (num12 > 400.0)
                            {
                                if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) < num11 - 0.5)
                                {
                                    Projectile projectile = this;
                                    Vector2 vector2_2 = projectile.velocity * 1.075f;
                                    projectile.velocity = vector2_2;
                                }
                                if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) > num11 + 0.5)
                                {
                                    Projectile projectile = this;
                                    Vector2 vector2_2 = projectile.velocity * 0.925f;
                                    projectile.velocity = vector2_2;
                                }
                            }
                            else
                            {
                                if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) < num11 - 0.25)
                                {
                                    Projectile projectile = this;
                                    Vector2 vector2_2 = projectile.velocity * 1.1f;
                                    projectile.velocity = vector2_2;
                                }
                                if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) > num11 + 0.25)
                                {
                                    Projectile projectile = this;
                                    Vector2 vector2_2 = projectile.velocity * 0.9f;
                                    projectile.velocity = vector2_2;
                                }
                            }
                            velocity.X = (float)((velocity.X * 34.0 + num13) / 35.0);
                            velocity.Y = (float)((velocity.Y * 34.0 + num14) / 35.0);
                        }
                        else
                        {
                            if (velocity.X < num13)
                            {
                                velocity.X += num2;
                                if (velocity.X < 0.0)
                                    velocity.X += num2 * 1.5f;
                            }
                            if (velocity.X > num13)
                            {
                                velocity.X -= num2;
                                if (velocity.X > 0.0)
                                    velocity.X -= num2 * 1.5f;
                            }
                            if (velocity.Y < num14)
                            {
                                velocity.Y += num2;
                                if (velocity.Y < 0.0)
                                    velocity.Y += num2 * 1.5f;
                            }
                            if (velocity.Y > num14)
                            {
                                velocity.Y -= num2;
                                if (velocity.Y > 0.0)
                                    velocity.Y -= num2 * 1.5f;
                            }
                        }
                        if (type == 111)
                            frame = 7;
                        if (type == 112)
                            frame = 2;
                        if (type >= 191 && type <= 194 && frame < 12)
                        {
                            frame = Main.rand.Next(12, 18);
                            frameCounter = 0;
                        }
                        if (type != 313)
                        {
                            if (velocity.X > 0.5)
                                spriteDirection = -1;
                            else if (velocity.X < -0.5)
                                spriteDirection = 1;
                        }
                        if (type == 398)
                        {
                            if (velocity.X > 0.5)
                                spriteDirection = 1;
                            else if (velocity.X < -0.5)
                                spriteDirection = -1;
                        }
                        if (type == 112)
                            rotation = spriteDirection != -1 ? (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f : (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
                        else if (type >= 390 && type <= 392)
                        {
                            int index1 = (int)(Center.X / 16.0);
                            int index2 = (int)(Center.Y / 16.0);
                            if (Main.tile[index1, index2] != null && Main.tile[index1, index2].wall > 0)
                            {
                                rotation = Utils.ToRotation(velocity) + 1.570796f;
                                frameCounter = frameCounter + (int)(Math.Abs(velocity.X) + Math.Abs(velocity.Y));
                                if (frameCounter > 5)
                                {
                                    ++frame;
                                    frameCounter = 0;
                                }
                                if (frame > 7)
                                    frame = 4;
                                if (frame < 4)
                                    frame = 7;
                            }
                            else
                            {
                                frameCounter = frameCounter + 1;
                                if (frameCounter > 2)
                                {
                                    ++frame;
                                    frameCounter = 0;
                                }
                                if (frame < 8 || frame > 10)
                                    frame = 8;
                                rotation = velocity.X * 0.1f;
                            }
                        }
                        else if (type == 334)
                        {
                            frameCounter = frameCounter + 1;
                            if (frameCounter > 1)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame < 7 || frame > 10)
                                frame = 7;
                            rotation = velocity.X * 0.1f;
                        }
                        else if (type == 353)
                        {
                            frameCounter = frameCounter + 1;
                            if (frameCounter > 6)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame < 10 || frame > 13)
                                frame = 10;
                            rotation = velocity.X * 0.05f;
                        }
                        else if (type == 127)
                        {
                            frameCounter = frameCounter + 3;
                            if (frameCounter > 6)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame <= 5 || frame > 15)
                                frame = 6;
                            rotation = velocity.X * 0.1f;
                        }
                        else if (type == 269)
                        {
                            if (frame == 6)
                                frameCounter = 0;
                            else if (frame < 4 || frame > 6)
                            {
                                frameCounter = 0;
                                frame = 4;
                            }
                            else
                            {
                                frameCounter = frameCounter + 1;
                                if (frameCounter > 6)
                                {
                                    ++frame;
                                    frameCounter = 0;
                                }
                            }
                            rotation = velocity.X * 0.05f;
                        }
                        else if (type == 266)
                        {
                            frameCounter = frameCounter + 1;
                            if (frameCounter > 6)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame < 2 || frame > 5)
                                frame = 2;
                            rotation = velocity.X * 0.1f;
                        }
                        else if (type == 324)
                        {
                            frameCounter = frameCounter + 1;
                            if (frameCounter > 1)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame < 6 || frame > 9)
                                frame = 6;
                            rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.58f;
                            Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.9f, 0.6f, 0.2f);
                            for (int index1 = 0; index1 < 2; ++index1)
                            {
                                int num5 = 4;
                                int index2 = Dust.NewDust(new Vector2(Center.X - num5, Center.Y - num5) - velocity * 0.0f, num5 * 2, num5 * 2, 6, 0.0f, 0.0f, 100, new Color(), 1f);
                                Main.dust[index2].scale *= (float)(1.79999995231628 + Main.rand.Next(10) * 0.100000001490116);
                                Main.dust[index2].velocity *= 0.2f;
                                if (index1 == 1)
                                    Main.dust[index2].position -= velocity * 0.5f;
                                Main.dust[index2].noGravity = true;
                                int index3 = Dust.NewDust(new Vector2(Center.X - num5, Center.Y - num5) - velocity * 0.0f, num5 * 2, num5 * 2, 31, 0.0f, 0.0f, 100, new Color(), 0.5f);
                                Main.dust[index3].fadeIn = (float)(1.0 + Main.rand.Next(5) * 0.100000001490116);
                                Main.dust[index3].velocity *= 0.05f;
                                if (index1 == 1)
                                    Main.dust[index3].position -= velocity * 0.5f;
                            }
                        }
                        else if (type == 268)
                        {
                            frameCounter = frameCounter + 1;
                            if (frameCounter > 4)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame < 6 || frame > 7)
                                frame = 6;
                            rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.58f;
                        }
                        else if (type == 200)
                        {
                            frameCounter = frameCounter + 3;
                            if (frameCounter > 6)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame <= 5 || frame > 9)
                                frame = 6;
                            rotation = velocity.X * 0.1f;
                        }
                        else if (type == 208)
                        {
                            rotation = velocity.X * 0.075f;
                            ++frameCounter;
                            if (frameCounter > 6)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame > 4)
                                frame = 1;
                            if (frame < 1)
                                frame = 1;
                        }
                        else if (type == 236)
                        {
                            rotation = velocity.Y * 0.05f * direction;
                            if (velocity.Y < 0.0)
                                frameCounter += 2;
                            else
                                ++frameCounter;
                            if (frameCounter >= 6)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame > 12)
                                frame = 9;
                            if (frame < 9)
                                frame = 9;
                        }
                        else if (type == 499)
                        {
                            rotation = velocity.Y * 0.05f * direction;
                            if (velocity.Y < 0.0)
                                frameCounter += 2;
                            else
                                ++frameCounter;
                            if (frameCounter >= 6)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame >= 12)
                                frame = 8;
                            if (frame < 8)
                                frame = 8;
                        }
                        else if (type == 314)
                        {
                            rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.58f;
                            ++frameCounter;
                            if (frameCounter >= 3)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame > 12)
                                frame = 7;
                            if (frame < 7)
                                frame = 7;
                        }
                        else if (type == 319)
                        {
                            rotation = velocity.X * 0.05f;
                            ++frameCounter;
                            if (frameCounter >= 6)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame > 10)
                                frame = 6;
                            if (frame < 6)
                                frame = 6;
                        }
                        else if (type == 210)
                        {
                            rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.58f;
                            frameCounter += 3;
                            if (frameCounter > 6)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame > 11)
                                frame = 7;
                            if (frame < 7)
                                frame = 7;
                        }
                        else if (type == 313)
                        {
                            position.Y += height;
                            height = 54;
                            position.Y -= height;
                            position.X += (width / 2);
                            width = 54;
                            position.X -= (width / 2);
                            rotation += velocity.X * 0.01f;
                            frameCounter = 0;
                            frame = 11;
                        }
                        else if (type == 398)
                        {
                            frameCounter = frameCounter + 1;
                            if (frameCounter > 1)
                            {
                                ++frame;
                                frameCounter = 0;
                            }
                            if (frame < 6 || frame > 9)
                                frame = 6;
                            rotation = velocity.X * 0.1f;
                        }
                        else
                            rotation = spriteDirection != -1 ? (float)Math.Atan2(velocity.Y, velocity.X) + 3.14f : (float)Math.Atan2(velocity.Y, velocity.X);
                        if (type >= 191 && type <= 194 || (type == 499 || type == 398) || (type == 390 || type == 391 || (type == 392 || type == 127)) || (type == 200 || type == 208 || (type == 210 || type == 236) || (type == 266 || type == 268 || (type == 269 || type == 313))) || (type == 314 || type == 319 || (type == 324 || type == 334) || type == 353))
                            return;
                        int index4 = Dust.NewDust(new Vector2((float)(position.X + (width / 2) - 4.0), (float)(position.Y + (height / 2) - 4.0)) - velocity, 8, 8, 16, (float)(-velocity.X * 0.5), velocity.Y * 0.5f, 50, new Color(), 1.7f);
                        Main.dust[index4].velocity.X = Main.dust[index4].velocity.X * 0.2f;
                        Main.dust[index4].velocity.Y = Main.dust[index4].velocity.Y * 0.2f;
                        Main.dust[index4].noGravity = true;
                    }
                    else
                    {
                        if (type >= 191 && type <= 194)
                        {
                            float num2 = (40 * minionPos);
                            int num3 = 30;
                            int num4 = 60;
                            --localAI[0];
                            if (localAI[0] < 0.0)
                                localAI[0] = 0.0f;
                            if (ai[1] > 0.0)
                            {
                                --ai[1];
                            }
                            else
                            {
                                float num5 = position.X;
                                float num6 = position.Y;
                                float num7 = 100000f;
                                float num8 = num7;
                                int num9 = -1;
                                for (int index = 0; index < 200; ++index)
                                {
                                    if (Main.npc[index].CanBeChasedBy(this, false))
                                    {
                                        float num10 = Main.npc[index].position.X + (Main.npc[index].width / 2);
                                        float num11 = Main.npc[index].position.Y + (Main.npc[index].height / 2);
                                        float num12 = Math.Abs(position.X + (width / 2) - num10) + Math.Abs(position.Y + (height / 2) - num11);
                                        if (num12 < num7)
                                        {
                                            if (num9 == -1 && num12 <= num8)
                                            {
                                                num8 = num12;
                                                num5 = num10;
                                                num6 = num11;
                                            }
                                            if (Collision.CanHit(position, width, height, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height))
                                            {
                                                num7 = num12;
                                                num5 = num10;
                                                num6 = num11;
                                                num9 = index;
                                            }
                                        }
                                    }
                                }
                                if (num9 == -1 && num8 < num7)
                                    num7 = num8;
                                float num13 = 400f;
                                if (position.Y > Main.worldSurface * 16.0)
                                    num13 = 200f;
                                if (num7 < num13 + num2 && num9 == -1)
                                {
                                    float num10 = num5 - (position.X + (width / 2));
                                    if (num10 < -5.0)
                                    {
                                        flag1 = true;
                                        flag2 = false;
                                    }
                                    else if (num10 > 5.0)
                                    {
                                        flag2 = true;
                                        flag1 = false;
                                    }
                                }
                                else if (num9 >= 0 && num7 < 800.0 + num2)
                                {
                                    localAI[0] = num4;
                                    float num10 = num5 - (position.X + (width / 2));
                                    if (num10 > 300.0 || num10 < -300.0)
                                    {
                                        if (num10 < -50.0)
                                        {
                                            flag1 = true;
                                            flag2 = false;
                                        }
                                        else if (num10 > 50.0)
                                        {
                                            flag2 = true;
                                            flag1 = false;
                                        }
                                    }
                                    else if (owner == Main.myPlayer)
                                    {
                                        ai[1] = num3;
                                        float num11 = 12f;
                                        Vector2 vector2 = new Vector2(position.X + width * 0.5f, (float)(position.Y + (height / 2) - 8.0));
                                        float num12 = num5 - vector2.X + Main.rand.Next(-20, 21);
                                        float num14 = (float)((double)(Math.Abs(num12) * 0.1f) * Main.rand.Next(0, 100) * (1.0 / 1000.0));
                                        float num15 = num6 - vector2.Y + Main.rand.Next(-20, 21) - num14;
                                        float num16 = (float)Math.Sqrt(num12 * num12 + num15 * num15);
                                        float num17 = num11 / num16;
                                        float SpeedX = num12 * num17;
                                        float SpeedY = num15 * num17;
                                        int Damage = damage;
                                        int Type = 195;
                                        int index = NewProjectile(vector2.X, vector2.Y, SpeedX, SpeedY, Type, Damage, knockBack, Main.myPlayer, 0.0f, 0.0f);
                                        Main.projectile[index].timeLeft = 300;
                                        if (SpeedX < 0.0)
                                            direction = -1;
                                        if (SpeedX > 0.0)
                                            direction = 1;
                                        netUpdate = true;
                                    }
                                }
                            }
                        }
                        bool flag5 = false;
                        Vector2 vector2_1 = Vector2.Zero;
                        bool flag6 = false;
                        if (type == 266 || type >= 390 && type <= 392)
                        {
                            float num2 = (40 * minionPos);
                            int num3 = 60;
                            --localAI[0];
                            if (localAI[0] < 0.0)
                                localAI[0] = 0.0f;
                            if (ai[1] > 0.0)
                            {
                                --ai[1];
                            }
                            else
                            {
                                float x = position.X;
                                float y = position.Y;
                                float num4 = 100000f;
                                float num5 = num4;
                                int index1 = -1;
                                for (int index2 = 0; index2 < 200; ++index2)
                                {
                                    if (Main.npc[index2].CanBeChasedBy(this, false))
                                    {
                                        float num6 = Main.npc[index2].position.X + (Main.npc[index2].width / 2);
                                        float num7 = Main.npc[index2].position.Y + (Main.npc[index2].height / 2);
                                        float num8 = Math.Abs(position.X + (width / 2) - num6) + Math.Abs(position.Y + (height / 2) - num7);
                                        if (num8 < num4)
                                        {
                                            if (index1 == -1 && num8 <= num5)
                                            {
                                                num5 = num8;
                                                x = num6;
                                                y = num7;
                                            }
                                            if (Collision.CanHit(position, width, height, Main.npc[index2].position, Main.npc[index2].width, Main.npc[index2].height))
                                            {
                                                num4 = num8;
                                                x = num6;
                                                y = num7;
                                                index1 = index2;
                                            }
                                        }
                                    }
                                }
                                if (type >= 390 && type <= 392 && !Collision.SolidCollision(position, width, height))
                                    tileCollide = true;
                                if (index1 == -1 && num5 < num4)
                                    num4 = num5;
                                else if (index1 >= 0)
                                {
                                    flag5 = true;
                                    vector2_1 = new Vector2(x, y) - Center;
                                    if (type >= 390 && type <= 392)
                                    {
                                        if (Main.npc[index1].position.Y > position.Y + (double)height)
                                        {
                                            int index2 = (int)(Center.X / 16.0);
                                            int index3 = (int)((position.Y + (double)height + 1.0) / 16.0);
                                            if (Main.tile[index2, index3] != null && Main.tile[index2, index3].active() && Main.tile[index2, index3].type == 19)
                                                tileCollide = false;
                                        }
                                        Rectangle rectangle1 = new Rectangle((int)position.X, (int)position.Y, width, height);
                                        Rectangle rectangle2 = new Rectangle((int)Main.npc[index1].position.X, (int)Main.npc[index1].position.Y, Main.npc[index1].width, Main.npc[index1].height);
                                        int num6 = 10;
                                        rectangle2.X -= num6;
                                        rectangle2.Y -= num6;
                                        rectangle2.Width += num6 * 2;
                                        rectangle2.Height += num6 * 2;
                                        if (rectangle1.Intersects(rectangle2))
                                        {
                                            flag6 = true;
                                            Vector2 v = Main.npc[index1].Center - Center;
                                            if (velocity.Y > 0.0 && v.Y < 0.0)
                                                velocity.Y *= 0.5f;
                                            if (velocity.Y < 0.0 && v.Y > 0.0)
                                                velocity.Y *= 0.5f;
                                            if (velocity.X > 0.0 && v.X < 0.0)
                                                velocity.X *= 0.5f;
                                            if (velocity.X < 0.0 && v.X > 0.0)
                                                velocity.X *= 0.5f;
                                            if (v.Length() > 14.0)
                                            {
                                                v.Normalize();
                                                v *= 14f;
                                            }
                                            rotation = (float)((rotation * 5.0 + Utils.ToRotation(v) + 1.57079637050629) / 6.0);
                                            velocity = (velocity * 9f + v) / 10f;
                                            for (int index2 = 0; index2 < 1000; ++index2)
                                            {
                                                if (whoAmI != index2 && owner == Main.projectile[index2].owner && (Main.projectile[index2].type >= 390 && Main.projectile[index2].type <= 392) && (Main.projectile[index2].Center - Center).Length() < 15.0)
                                                {
                                                    float num7 = 0.5f;
                                                    if (Center.Y > Main.projectile[index2].Center.Y)
                                                    {
                                                        Main.projectile[index2].velocity.Y -= num7;
                                                        velocity.Y += num7;
                                                    }
                                                    else
                                                    {
                                                        Main.projectile[index2].velocity.Y += num7;
                                                        velocity.Y -= num7;
                                                    }
                                                    if (Center.X > Main.projectile[index2].Center.X)
                                                    {
                                                        velocity.X += num7;
                                                        Main.projectile[index2].velocity.X -= num7;
                                                    }
                                                    else
                                                    {
                                                        velocity.X -= num7;
                                                        Main.projectile[index2].velocity.Y += num7;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                float num9 = 300f;
                                if (position.Y > Main.worldSurface * 16.0)
                                    num9 = 150f;
                                if (type >= 390 && type <= 392)
                                {
                                    num9 = 500f;
                                    if (position.Y > Main.worldSurface * 16.0)
                                        num9 = 250f;
                                }
                                if (num4 < num9 + num2 && index1 == -1)
                                {
                                    float num6 = x - (position.X + (width / 2));
                                    if (num6 < -5.0)
                                    {
                                        flag1 = true;
                                        flag2 = false;
                                    }
                                    else if (num6 > 5.0)
                                    {
                                        flag2 = true;
                                        flag1 = false;
                                    }
                                }
                                bool flag7 = false;
                                if (type >= 390 && type <= 392 && localAI[1] > 0.0)
                                {
                                    flag7 = true;
                                    --localAI[1];
                                }
                                if (index1 >= 0 && num4 < 800.0 + num2)
                                {
                                    friendly = true;
                                    localAI[0] = num3;
                                    float num6 = x - (position.X + (width / 2));
                                    if (num6 < -10.0)
                                    {
                                        flag1 = true;
                                        flag2 = false;
                                    }
                                    else if (num6 > 10.0)
                                    {
                                        flag2 = true;
                                        flag1 = false;
                                    }
                                    if (y < Center.Y - 100.0 && num6 > -50.0 && (num6 < 50.0 && velocity.Y == 0.0))
                                    {
                                        float num7 = Math.Abs(y - Center.Y);
                                        if (num7 < 120.0)
                                            velocity.Y = -10f;
                                        else if (num7 < 210.0)
                                            velocity.Y = -13f;
                                        else if (num7 < 270.0)
                                            velocity.Y = -15f;
                                        else if (num7 < 310.0)
                                            velocity.Y = -17f;
                                        else if (num7 < 380.0)
                                            velocity.Y = -18f;
                                    }
                                    if (flag7)
                                    {
                                        friendly = false;
                                        if (velocity.X < 0.0)
                                            flag1 = true;
                                        else if (velocity.X > 0.0)
                                            flag2 = true;
                                    }
                                }
                                else
                                    friendly = false;
                            }
                        }
                        if (ai[1] != 0.0)
                        {
                            flag1 = false;
                            flag2 = false;
                        }
                        else if (type >= 191 && type <= 194 && localAI[0] == 0.0)
                            direction = Main.player[owner].direction;
                        else if (type >= 390 && type <= 392)
                        {
                            int index1 = (int)(Center.X / 16.0);
                            int index2 = (int)(Center.Y / 16.0);
                            if (Main.tile[index1, index2] != null && Main.tile[index1, index2].wall > 0)
                                flag1 = flag2 = false;
                        }
                        if (type == 127)
                        {
                            if (rotation > -0.1 && rotation < 0.1)
                                rotation = 0.0f;
                            else if (rotation < 0.0)
                                rotation += 0.1f;
                            else
                                rotation -= 0.1f;
                        }
                        else if (type != 313 && !flag6)
                            rotation = 0.0f;
                        if (type < 390 || type > 392)
                            tileCollide = true;
                        float num18 = 0.08f;
                        float num19 = 6.5f;
                        if (type == 127)
                        {
                            num19 = 2f;
                            num18 = 0.04f;
                        }
                        if (type == 112)
                        {
                            num19 = 6f;
                            num18 = 0.06f;
                        }
                        if (type == 334)
                        {
                            num19 = 8f;
                            num18 = 0.08f;
                        }
                        if (type == 268)
                        {
                            num19 = 8f;
                            num18 = 0.4f;
                        }
                        if (type == 324)
                        {
                            num18 = 0.1f;
                            num19 = 3f;
                        }
                        if (type >= 191 && type <= 194 || type == 266 || type >= 390 && type <= 392)
                        {
                            num19 = 6f;
                            num18 = 0.2f;
                            if (num19 < Math.Abs(Main.player[owner].velocity.X) + Math.Abs(Main.player[owner].velocity.Y))
                            {
                                num19 = Math.Abs(Main.player[owner].velocity.X) + Math.Abs(Main.player[owner].velocity.Y);
                                num18 = 0.3f;
                            }
                        }
                        if (type >= 390 && type <= 392)
                            num18 *= 2f;
                        if (flag1)
                        {
                            if (velocity.X > -3.5)
                                velocity.X -= num18;
                            else
                                velocity.X -= num18 * 0.25f;
                        }
                        else if (flag2)
                        {
                            if (velocity.X < 3.5)
                                velocity.X += num18;
                            else
                                velocity.X += num18 * 0.25f;
                        }
                        else
                        {
                            velocity.X *= 0.9f;
                            if (velocity.X >= -num18 && velocity.X <= num18)
                                velocity.X = 0.0f;
                        }
                        if (type == 208)
                        {
                            velocity.X *= 0.95f;
                            if (velocity.X > -0.1 && velocity.X < 0.1)
                                velocity.X = 0.0f;
                            flag1 = false;
                            flag2 = false;
                        }
                        if (flag1 || flag2)
                        {
                            int num2 = (int)(position.X + (width / 2)) / 16;
                            int j = (int)(position.Y + (height / 2)) / 16;
                            if (type == 236)
                                num2 += direction;
                            if (flag1)
                                --num2;
                            if (flag2)
                                ++num2;
                            if (WorldGen.SolidTile(num2 + (int)velocity.X, j))
                                flag4 = true;
                        }
                        if (Main.player[owner].position.Y + Main.player[owner].height - 8.0 > position.Y + (double)height)
                            flag3 = true;
                        if (type == 268 && frameCounter < 10)
                            flag4 = false;
                        Collision.StepUp(ref position, ref velocity, width, height, ref stepSpeed, ref gfxOffY, 1, false, 0);
                        if (velocity.Y == 0.0 || type == 200)
                        {
                            if (!flag3 && (velocity.X < 0.0 || velocity.X > 0.0))
                            {
                                int i = (int)(position.X + (width / 2)) / 16;
                                int j = (int)(position.Y + (height / 2)) / 16 + 1;
                                if (flag1)
                                    --i;
                                if (flag2)
                                    ++i;
                                WorldGen.SolidTile(i, j);
                            }
                            if (flag4)
                            {
                                int i1 = (int)(position.X + (width / 2)) / 16;
                                int j = (int)(position.Y + (double)height) / 16 + 1;
                                if (WorldGen.SolidTile(i1, j) || Main.tile[i1, j].halfBrick() || (Main.tile[i1, j].slope() > 0 || type == 200))
                                {
                                    if (type == 200)
                                    {
                                        velocity.Y = -3.1f;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int num2 = (int)(position.X + (width / 2)) / 16;
                                            int num3 = (int)(position.Y + (height / 2)) / 16;
                                            if (flag1)
                                                --num2;
                                            if (flag2)
                                                ++num2;
                                            int i2 = num2 + (int)velocity.X;
                                            if (!WorldGen.SolidTile(i2, num3 - 1) && !WorldGen.SolidTile(i2, num3 - 2))
                                                velocity.Y = -5.1f;
                                            else if (!WorldGen.SolidTile(i2, num3 - 2))
                                                velocity.Y = -7.1f;
                                            else if (WorldGen.SolidTile(i2, num3 - 5))
                                                velocity.Y = -11.1f;
                                            else if (WorldGen.SolidTile(i2, num3 - 4))
                                                velocity.Y = -10.1f;
                                            else
                                                velocity.Y = -9.1f;
                                        }
                                        catch
                                        {
                                            velocity.Y = -9.1f;
                                        }
                                    }
                                    if (type == 127)
                                        ai[0] = 1f;
                                }
                            }
                            else if (type == 266 && (flag1 || flag2))
                                velocity.Y -= 6f;
                        }
                        if (velocity.X > num19)
                            velocity.X = num19;
                        if (velocity.X < -num19)
                            velocity.X = -num19;
                        if (velocity.X < 0.0)
                            direction = -1;
                        if (velocity.X > 0.0)
                            direction = 1;
                        if (velocity.X > num18 && flag2)
                            direction = 1;
                        if (velocity.X < -num18 && flag1)
                            direction = -1;
                        if (type != 313)
                        {
                            if (direction == -1)
                                spriteDirection = 1;
                            if (direction == 1)
                                spriteDirection = -1;
                        }
                        if (type == 398)
                            spriteDirection = direction;
                        if (type >= 191 && type <= 194)
                        {
                            if (ai[1] > 0.0)
                            {
                                if (localAI[1] == 0.0)
                                {
                                    localAI[1] = 1f;
                                    frame = 1;
                                }
                                if (frame != 0)
                                {
                                    ++frameCounter;
                                    if (frameCounter > 4)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame == 4)
                                        frame = 0;
                                }
                            }
                            else if (velocity.Y == 0.0)
                            {
                                localAI[1] = 0.0f;
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.8 || velocity.X > 0.8)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame < 5)
                                        frame = 5;
                                    if (frame >= 11)
                                        frame = 5;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else if (velocity.Y < 0.0)
                            {
                                frameCounter = 0;
                                frame = 4;
                            }
                            else if (velocity.Y > 0.0)
                            {
                                frameCounter = 0;
                                frame = 4;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y > 10.0)
                                velocity.Y = 10f;
                            double num2 = velocity.Y;
                        }
                        else if (type == 268)
                        {
                            if (velocity.Y == 0.0)
                            {
                                if (frame > 5)
                                    frameCounter = 0;
                                if (velocity.X == 0.0)
                                {
                                    int num2 = 3;
                                    ++frameCounter;
                                    if (frameCounter < num2)
                                        frame = 0;
                                    else if (frameCounter < num2 * 2)
                                        frame = 1;
                                    else if (frameCounter < num2 * 3)
                                        frame = 2;
                                    else if (frameCounter < num2 * 4)
                                        frame = 3;
                                    else
                                        frameCounter = num2 * 4;
                                }
                                else
                                {
                                    velocity.X *= 0.8f;
                                    ++frameCounter;
                                    int num2 = 3;
                                    if (frameCounter < num2)
                                        frame = 0;
                                    else if (frameCounter < num2 * 2)
                                        frame = 1;
                                    else if (frameCounter < num2 * 3)
                                        frame = 2;
                                    else if (frameCounter < num2 * 4)
                                        frame = 3;
                                    else if (flag1 || flag2)
                                    {
                                        velocity.X *= 2f;
                                        frame = 4;
                                        velocity.Y = -6.1f;
                                        frameCounter = 0;
                                        for (int index1 = 0; index1 < 4; ++index1)
                                        {
                                            int index2 = Dust.NewDust(new Vector2(position.X, (float)(position.Y + (double)height - 2.0)), width, 4, 5, 0.0f, 0.0f, 0, new Color(), 1f);
                                            Main.dust[index2].velocity += velocity;
                                            Main.dust[index2].velocity *= 0.4f;
                                        }
                                    }
                                    else
                                        frameCounter = num2 * 4;
                                }
                            }
                            else if (velocity.Y < 0.0)
                            {
                                frameCounter = 0;
                                frame = 5;
                            }
                            else
                            {
                                frame = 4;
                                frameCounter = 3;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 269)
                        {
                            if (velocity.Y >= 0.0 && velocity.Y <= 0.8)
                            {
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.8 || velocity.X > 0.8)
                                {
                                    int index = Dust.NewDust(new Vector2(position.X, (float)(position.Y + (double)height - 2.0)), width, 6, 76, 0.0f, 0.0f, 0, new Color(), 1f);
                                    Main.dust[index].noGravity = true;
                                    Main.dust[index].velocity *= 0.3f;
                                    Main.dust[index].noLight = true;
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame > 3)
                                        frame = 0;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else
                            {
                                frameCounter = 0;
                                frame = 2;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 313)
                        {
                            int index1 = (int)(Center.X / 16.0);
                            int index2 = (int)(Center.Y / 16.0);
                            if (Main.tile[index1, index2] != null && Main.tile[index1, index2].wall > 0)
                            {
                                position.Y += height;
                                height = 34;
                                position.Y -= height;
                                position.X += (width / 2);
                                width = 34;
                                position.X -= (width / 2);
                                float num2 = 4f;
                                Vector2 vector2_2 = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                                float num3 = Main.player[owner].Center.X - vector2_2.X;
                                float num4 = Main.player[owner].Center.Y - vector2_2.Y;
                                float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
                                float num6 = num2 / num5;
                                float num7 = num3 * num6;
                                float num8 = num4 * num6;
                                if (num5 < 120.0)
                                {
                                    velocity.X *= 0.9f;
                                    velocity.Y *= 0.9f;
                                    if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) < 0.1)
                                    {
                                        Projectile projectile = this;
                                        Vector2 vector2_3 = projectile.velocity * 0.0f;
                                        projectile.velocity = vector2_3;
                                    }
                                }
                                else
                                {
                                    velocity.X = (float)((velocity.X * 9.0 + num7) / 10.0);
                                    velocity.Y = (float)((velocity.Y * 9.0 + num8) / 10.0);
                                }
                                if (num5 >= 120.0)
                                {
                                    spriteDirection = direction;
                                    rotation = (float)Math.Atan2(velocity.Y * (double)-direction, velocity.X * (double)-direction);
                                }
                                frameCounter = frameCounter + (int)(Math.Abs(velocity.X) + Math.Abs(velocity.Y));
                                if (frameCounter > 6)
                                {
                                    ++frame;
                                    frameCounter = 0;
                                }
                                if (frame > 10)
                                    frame = 5;
                                if (frame >= 5)
                                    return;
                                frame = 10;
                            }
                            else
                            {
                                rotation = 0.0f;
                                if (direction == -1)
                                    spriteDirection = 1;
                                if (direction == 1)
                                    spriteDirection = -1;
                                position.Y += height;
                                height = 30;
                                position.Y -= height;
                                position.X += (width / 2);
                                width = 30;
                                position.X -= (width / 2);
                                if (velocity.Y >= 0.0 && velocity.Y <= 0.8)
                                {
                                    if (velocity.X == 0.0)
                                    {
                                        frame = 0;
                                        frameCounter = 0;
                                    }
                                    else if (velocity.X < -0.8 || velocity.X > 0.8)
                                    {
                                        frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                        ++frameCounter;
                                        if (frameCounter > 6)
                                        {
                                            ++frame;
                                            frameCounter = 0;
                                        }
                                        if (frame > 3)
                                            frame = 0;
                                    }
                                    else
                                    {
                                        frame = 0;
                                        frameCounter = 0;
                                    }
                                }
                                else
                                {
                                    frameCounter = 0;
                                    frame = 4;
                                }
                                velocity.Y += 0.4f;
                                if (velocity.Y <= 10.0)
                                    return;
                                velocity.Y = 10f;
                            }
                        }
                        else if (type >= 390 && type <= 392)
                        {
                            int index1 = (int)(Center.X / 16.0);
                            int index2 = (int)(Center.Y / 16.0);
                            if (Main.tile[index1, index2] != null && Main.tile[index1, index2].wall > 0)
                            {
                                position.Y += height;
                                height = 34;
                                position.Y -= height;
                                position.X += (width / 2);
                                width = 34;
                                position.X -= (width / 2);
                                float num2 = 9f;
                                float num3 = (float)(40 * (minionPos + 1));
                                Vector2 v = Main.player[owner].Center - Center;
                                if (flag5)
                                {
                                    v = vector2_1;
                                    num3 = 10f;
                                }
                                else if (!Collision.CanHitLine(Center, 1, 1, Main.player[owner].Center, 1, 1))
                                    ai[0] = 1f;
                                if ((double)v.Length() < num3)
                                {
                                    Projectile projectile1 = this;
                                    Vector2 vector2_2 = projectile1.velocity * 0.9f;
                                    projectile1.velocity = vector2_2;
                                    if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) < 0.1)
                                    {
                                        Projectile projectile2 = this;
                                        Vector2 vector2_3 = projectile2.velocity * 0.0f;
                                        projectile2.velocity = vector2_3;
                                    }
                                }
                                else if (v.Length() < 800.0 || !flag5)
                                    velocity = (velocity * 9f + Vector2.Normalize(v) * num2) / 10f;
                                if ((double)v.Length() >= num3)
                                {
                                    spriteDirection = direction;
                                    rotation = Utils.ToRotation(velocity) + 1.570796f;
                                }
                                else
                                    rotation = Utils.ToRotation(v) + 1.570796f;
                                frameCounter = frameCounter + (int)(Math.Abs(velocity.X) + Math.Abs(velocity.Y));
                                if (frameCounter > 5)
                                {
                                    ++frame;
                                    frameCounter = 0;
                                }
                                if (frame > 7)
                                    frame = 4;
                                if (frame >= 4)
                                    return;
                                frame = 7;
                            }
                            else
                            {
                                if (!flag6)
                                    rotation = 0.0f;
                                if (direction == -1)
                                    spriteDirection = 1;
                                if (direction == 1)
                                    spriteDirection = -1;
                                position.Y += height;
                                height = 30;
                                position.Y -= height;
                                position.X += (width / 2);
                                width = 30;
                                position.X -= (width / 2);
                                if (!flag5 && !Collision.CanHitLine(Center, 1, 1, Main.player[owner].Center, 1, 1))
                                    ai[0] = 1f;
                                if (!flag6 && frame >= 4 && frame <= 7)
                                {
                                    Vector2 vector2_2 = Main.player[owner].Center - Center;
                                    if (flag5)
                                        vector2_2 = vector2_1;
                                    float num2 = -vector2_2.Y;
                                    if (vector2_2.Y <= 0.0)
                                    {
                                        if (num2 < 120.0)
                                            velocity.Y = -10f;
                                        else if (num2 < 210.0)
                                            velocity.Y = -13f;
                                        else if (num2 < 270.0)
                                            velocity.Y = -15f;
                                        else if (num2 < 310.0)
                                            velocity.Y = -17f;
                                        else if (num2 < 380.0)
                                            velocity.Y = -18f;
                                    }
                                }
                                if (flag6)
                                {
                                    ++frameCounter;
                                    if (frameCounter > 3)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame >= 8)
                                        frame = 4;
                                    if (frame <= 3)
                                        frame = 7;
                                }
                                else if (velocity.Y >= 0.0 && velocity.Y <= 0.8)
                                {
                                    if (velocity.X == 0.0)
                                    {
                                        frame = 0;
                                        frameCounter = 0;
                                    }
                                    else if (velocity.X < -0.8 || velocity.X > 0.8)
                                    {
                                        frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                        ++frameCounter;
                                        if (frameCounter > 5)
                                        {
                                            ++frame;
                                            frameCounter = 0;
                                        }
                                        if (frame > 2)
                                            frame = 0;
                                    }
                                    else
                                    {
                                        frame = 0;
                                        frameCounter = 0;
                                    }
                                }
                                else
                                {
                                    frameCounter = 0;
                                    frame = 3;
                                }
                                velocity.Y += 0.4f;
                                if (velocity.Y <= 10.0)
                                    return;
                                velocity.Y = 10f;
                            }
                        }
                        else if (type == 314)
                        {
                            if (velocity.Y >= 0.0 && velocity.Y <= 0.8)
                            {
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.8 || velocity.X > 0.8)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame > 6)
                                        frame = 1;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else
                            {
                                frameCounter = 0;
                                frame = 7;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 319)
                        {
                            if (velocity.Y >= 0.0 && velocity.Y <= 0.8)
                            {
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.8 || velocity.X > 0.8)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 8)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame > 5)
                                        frame = 2;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else
                            {
                                frameCounter = 0;
                                frame = 1;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 236)
                        {
                            if (velocity.Y >= 0.0 && velocity.Y <= 0.8)
                            {
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.8 || velocity.X > 0.8)
                                {
                                    if (frame < 2)
                                        frame = 2;
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame > 8)
                                        frame = 2;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else
                            {
                                frameCounter = 0;
                                frame = 1;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 499)
                        {
                            if (velocity.Y >= 0.0 && velocity.Y <= 0.8)
                            {
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.8 || velocity.X > 0.8)
                                {
                                    if (frame < 2)
                                        frame = 2;
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame >= 8)
                                        frame = 2;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else
                            {
                                frameCounter = 0;
                                frame = 1;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 266)
                        {
                            if (velocity.Y >= 0.0 && velocity.Y <= 0.8)
                            {
                                if (velocity.X == 0.0)
                                    ++frameCounter;
                                else
                                    frameCounter += 3;
                            }
                            else
                                frameCounter += 5;
                            if (frameCounter >= 20)
                            {
                                frameCounter -= 20;
                                ++frame;
                            }
                            if (frame > 1)
                                frame = 0;
                            if (wet && Main.player[owner].position.Y + Main.player[owner].height < position.Y + (double)height && localAI[0] == 0.0)
                            {
                                if (velocity.Y > -4.0)
                                    velocity.Y -= 0.2f;
                                if (velocity.Y > 0.0)
                                    velocity.Y *= 0.95f;
                            }
                            else
                                velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 334)
                        {
                            if (velocity.Y == 0.0)
                            {
                                if (velocity.X == 0.0)
                                {
                                    if (frame > 0)
                                    {
                                        frameCounter += 2;
                                        if (frameCounter > 6)
                                        {
                                            ++frame;
                                            frameCounter = 0;
                                        }
                                        if (frame >= 7)
                                            frame = 0;
                                    }
                                    else
                                    {
                                        frame = 0;
                                        frameCounter = 0;
                                    }
                                }
                                else if (velocity.X < -0.8 || velocity.X > 0.8)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X * 0.75);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame >= 7 || frame < 1)
                                        frame = 1;
                                }
                                else if (frame > 0)
                                {
                                    frameCounter += 2;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame >= 7)
                                        frame = 0;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else if (velocity.Y < 0.0)
                            {
                                frameCounter = 0;
                                frame = 2;
                            }
                            else if (velocity.Y > 0.0)
                            {
                                frameCounter = 0;
                                frame = 4;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 353)
                        {
                            if (velocity.Y == 0.0)
                            {
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.8 || velocity.X > 0.8)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame > 9)
                                        frame = 2;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else if (velocity.Y < 0.0)
                            {
                                frameCounter = 0;
                                frame = 1;
                            }
                            else if (velocity.Y > 0.0)
                            {
                                frameCounter = 0;
                                frame = 1;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 111)
                        {
                            if (velocity.Y == 0.0)
                            {
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.8 || velocity.X > 0.8)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame >= 7)
                                        frame = 0;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else if (velocity.Y < 0.0)
                            {
                                frameCounter = 0;
                                frame = 4;
                            }
                            else if (velocity.Y > 0.0)
                            {
                                frameCounter = 0;
                                frame = 6;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 112)
                        {
                            if (velocity.Y == 0.0)
                            {
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.8 || velocity.X > 0.8)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame >= 3)
                                        frame = 0;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else if (velocity.Y < 0.0)
                            {
                                frameCounter = 0;
                                frame = 2;
                            }
                            else if (velocity.Y > 0.0)
                            {
                                frameCounter = 0;
                                frame = 2;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 127)
                        {
                            if (velocity.Y == 0.0)
                            {
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.1 || velocity.X > 0.1)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame > 5)
                                        frame = 0;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else
                            {
                                frame = 0;
                                frameCounter = 0;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 200)
                        {
                            if (velocity.Y == 0.0)
                            {
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.1 || velocity.X > 0.1)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame > 5)
                                        frame = 0;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else
                            {
                                rotation = velocity.X * 0.1f;
                                ++frameCounter;
                                if (velocity.Y < 0.0)
                                    frameCounter += 2;
                                if (frameCounter > 6)
                                {
                                    ++frame;
                                    frameCounter = 0;
                                }
                                if (frame > 9)
                                    frame = 6;
                                if (frame < 6)
                                    frame = 6;
                            }
                            velocity.Y += 0.1f;
                            if (velocity.Y <= 4.0)
                                return;
                            velocity.Y = 4f;
                        }
                        else if (type == 208)
                        {
                            if (velocity.Y == 0.0 && velocity.X == 0.0)
                            {
                                if (Main.player[owner].position.X + (Main.player[owner].width / 2) < position.X + (width / 2))
                                    direction = -1;
                                else if (Main.player[owner].position.X + (Main.player[owner].width / 2) > position.X + (width / 2))
                                    direction = 1;
                                rotation = 0.0f;
                                frame = 0;
                            }
                            else
                            {
                                rotation = velocity.X * 0.075f;
                                ++frameCounter;
                                if (frameCounter > 6)
                                {
                                    ++frame;
                                    frameCounter = 0;
                                }
                                if (frame > 4)
                                    frame = 1;
                                if (frame < 1)
                                    frame = 1;
                            }
                            velocity.Y += 0.1f;
                            if (velocity.Y <= 4.0)
                                return;
                            velocity.Y = 4f;
                        }
                        else if (type == 209)
                        {
                            if (alpha > 0)
                            {
                                alpha -= 5;
                                if (alpha < 0)
                                    alpha = 0;
                            }
                            if (velocity.Y == 0.0)
                            {
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.1 || velocity.X > 0.1)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame > 11)
                                        frame = 2;
                                    if (frame < 2)
                                        frame = 2;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else
                            {
                                frame = 1;
                                frameCounter = 0;
                                rotation = 0.0f;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else if (type == 324)
                        {
                            if (velocity.Y == 0.0)
                            {
                                if (velocity.X < -0.1 || velocity.X > 0.1)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame > 5)
                                        frame = 2;
                                    if (frame < 2)
                                        frame = 2;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else
                            {
                                frameCounter = 0;
                                frame = 1;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 14.0)
                                return;
                            velocity.Y = 14f;
                        }
                        else if (type == 210)
                        {
                            if (velocity.Y == 0.0)
                            {
                                if (velocity.X < -0.1 || velocity.X > 0.1)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame > 6)
                                        frame = 0;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else
                            {
                                rotation = velocity.X * 0.05f;
                                ++frameCounter;
                                if (frameCounter > 6)
                                {
                                    ++frame;
                                    frameCounter = 0;
                                }
                                if (frame > 11)
                                    frame = 7;
                                if (frame < 7)
                                    frame = 7;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                        else
                        {
                            if (type != 398)
                                return;
                            if (velocity.Y == 0.0)
                            {
                                if (velocity.X == 0.0)
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                                else if (velocity.X < -0.8 || velocity.X > 0.8)
                                {
                                    frameCounter = frameCounter + (int)Math.Abs(velocity.X);
                                    ++frameCounter;
                                    if (frameCounter > 6)
                                    {
                                        ++frame;
                                        frameCounter = 0;
                                    }
                                    if (frame >= 5)
                                        frame = 0;
                                }
                                else
                                {
                                    frame = 0;
                                    frameCounter = 0;
                                }
                            }
                            else if (velocity.Y != 0.0)
                            {
                                frameCounter = 0;
                                frame = 5;
                            }
                            velocity.Y += 0.4f;
                            if (velocity.Y <= 10.0)
                                return;
                            velocity.Y = 10f;
                        }
                    }
                }
            }
        }

        private void AI_062()
        {
            if (type == 373)
            {
                if (Main.player[owner].dead)
                    Main.player[owner].hornetMinion = false;
                if (Main.player[owner].hornetMinion)
                    timeLeft = 2;
            }
            if (type == 375)
            {
                if (Main.player[owner].dead)
                    Main.player[owner].impMinion = false;
                if (Main.player[owner].impMinion)
                    timeLeft = 2;
            }
            if (type == 407)
            {
                if (Main.player[owner].dead)
                    Main.player[owner].sharknadoMinion = false;
                if (Main.player[owner].sharknadoMinion)
                    timeLeft = 2;
            }
            if (type == 423)
            {
                if (Main.player[owner].dead)
                    Main.player[owner].UFOMinion = false;
                if (Main.player[owner].UFOMinion)
                    timeLeft = 2;
            }
            if (type == 613)
            {
                if (Main.player[owner].dead)
                    Main.player[owner].stardustMinion = false;
                if (Main.player[owner].stardustMinion)
                    timeLeft = 2;
                Lighting.AddLight(Center, 0.2f, 0.6f, 0.7f);
                if (localAI[1] > 0.0)
                    --localAI[1];
            }
            if (type == 423)
            {
                if (ai[0] == 2.0)
                {
                    --ai[1];
                    tileCollide = false;
                    if (ai[1] > 3.0)
                    {
                        int index = Dust.NewDust(Center, 0, 0, 220 + Main.rand.Next(2), velocity.X, velocity.Y, 100, new Color(), 1f);
                        Main.dust[index].scale = (float)(0.5 + Main.rand.NextDouble() * 0.300000011920929);
                        Main.dust[index].velocity /= 2.5f;
                        Main.dust[index].noGravity = true;
                        Main.dust[index].noLight = true;
                        Main.dust[index].frame.Y = 80;
                    }
                    if (ai[1] != 0.0)
                        return;
                    ai[1] = 30f;
                    ai[0] = 0.0f;
                    Projectile projectile = this;
                    Vector2 vector2 = projectile.velocity / 5f;
                    projectile.velocity = vector2;
                    velocity.Y = 0.0f;
                    extraUpdates = 0;
                    numUpdates = 0;
                    netUpdate = true;
                    extraUpdates = 0;
                    numUpdates = 0;
                }
                if (extraUpdates > 1)
                    extraUpdates = 0;
                if (numUpdates > 1)
                    numUpdates = 0;
            }
            if (type == 613)
            {
                if (ai[0] == 2.0)
                {
                    --ai[1];
                    tileCollide = false;
                    if (ai[1] > 3.0)
                    {
                        if (numUpdates < 20)
                        {
                            for (int index = 0; index < 3; ++index)
                            {
                                Dust dust = Main.dust[Dust.NewDust(position, width, height, 229, 0.0f, 0.0f, 0, new Color(), 1f)];
                                dust.noGravity = true;
                                dust.position = Center;
                                dust.velocity *= 3f;
                                dust.velocity += velocity * 3f;
                                dust.fadeIn = 1f;
                            }
                        }
                        float num1 = (float)(2.0 - numUpdates / 30.0);
                        if (scale > 0.0)
                        {
                            float num2 = 2f;
                            for (int index = 0; (double)index < num2; ++index)
                            {
                                Dust dust = Main.dust[Dust.NewDust(position, width, height, 229, 0.0f, 0.0f, 0, new Color(), 1f)];
                                dust.noGravity = true;
                                dust.position = Center + Utils.RotatedBy(Vector2.UnitY, numUpdates * 0.104719758033752 + whoAmI * 0.785398185253143 + 1.57079637050629, new Vector2()) * (height / 2) - velocity * ((float)index / num2);
                                dust.velocity = velocity / 3f;
                                dust.fadeIn = num1 / 2f;
                                dust.scale = num1;
                            }
                        }
                    }
                    if (ai[1] != 0.0)
                        return;
                    ai[1] = 30f;
                    ai[0] = 0.0f;
                    Projectile projectile = this;
                    Vector2 vector2 = projectile.velocity / 5f;
                    projectile.velocity = vector2;
                    velocity.Y = 0.0f;
                    extraUpdates = 0;
                    numUpdates = 0;
                    netUpdate = true;
                    float num = 15f;
                    for (int index = 0; index < num; ++index)
                    {
                        Dust dust = Main.dust[Dust.NewDust(position, width, height, 229, 0.0f, 0.0f, 0, new Color(), 1f)];
                        dust.noGravity = true;
                        dust.position = Center - velocity * 5f;
                        dust.velocity *= 3f;
                        dust.velocity += velocity * 3f;
                        dust.fadeIn = 1f;
                        if (Main.rand.Next(3) != 0)
                        {
                            dust.fadeIn = 2f;
                            dust.scale = 2f;
                            dust.velocity /= 8f;
                        }
                    }
                    for (int index = 0; index < num; ++index)
                    {
                        Dust dust = Main.dust[Dust.NewDust(position, width, height, 229, 0.0f, 0.0f, 0, new Color(), 1f)];
                        dust.noGravity = true;
                        dust.position = Center;
                        dust.velocity *= 3f;
                        dust.velocity += velocity * 3f;
                        dust.fadeIn = 1f;
                        if (Main.rand.Next(3) != 0)
                        {
                            dust.fadeIn = 2f;
                            dust.scale = 2f;
                            dust.velocity /= 8f;
                        }
                    }
                    extraUpdates = 0;
                    numUpdates = 0;
                }
                if (extraUpdates > 1)
                    extraUpdates = 0;
                if (numUpdates > 1)
                    numUpdates = 0;
            }
            if (type == 423 && localAI[0] > 0.0)
                --localAI[0];
            if (type == 613 && localAI[0] > 0.0)
                --localAI[0];
            float num3 = 0.05f;
            float num4 = width;
            if (type == 407)
            {
                num3 = 0.1f;
                num4 *= 2f;
            }
            for (int index = 0; index < 1000; ++index)
            {
                if (index != whoAmI && Main.projectile[index].active && (Main.projectile[index].owner == owner && Main.projectile[index].type == type) && Math.Abs(position.X - Main.projectile[index].position.X) + Math.Abs(position.Y - Main.projectile[index].position.Y) < num4)
                {
                    if (position.X < Main.projectile[index].position.X)
                        velocity.X -= num3;
                    else
                        velocity.X += num3;
                    if (position.Y < Main.projectile[index].position.Y)
                        velocity.Y -= num3;
                    else
                        velocity.Y += num3;
                }
            }
            Vector2 vector2_1 = position;
            float num5 = 400f;
            if (type == 423)
                num5 = 300f;
            if (type == 613)
                num5 = 300f;
            bool flag = false;
            int ai1 = -1;
            tileCollide = true;
            if (type == 407)
            {
                tileCollide = false;
                if (Collision.SolidCollision(position, width, height))
                {
                    alpha += 20;
                    if (alpha > 150)
                        alpha = 150;
                }
                else
                {
                    alpha -= 50;
                    if (alpha < 60)
                        alpha = 60;
                }
            }
            if (type == 407 || type == 613 || type == 423)
            {
                Vector2 center = Main.player[owner].Center;
                Vector2 vector2_2 = new Vector2(0.5f);
                if (type == 423)
                    vector2_2.Y = 0.0f;
                for (int index = 0; index < 200; ++index)
                {
                    NPC npc = Main.npc[index];
                    if (npc.CanBeChasedBy(this, false))
                    {
                        Vector2 vector2_3 = npc.position + npc.Size * vector2_2;
                        float num1 = Vector2.Distance(vector2_3, center);
                        if ((Vector2.Distance(center, vector2_1) > num1 && num1 < num5 || !flag) && Collision.CanHitLine(position, width, height, npc.position, npc.width, npc.height))
                        {
                            num5 = num1;
                            vector2_1 = vector2_3;
                            flag = true;
                            ai1 = index;
                        }
                    }
                }
            }
            else
            {
                for (int index = 0; index < 200; ++index)
                {
                    NPC npc = Main.npc[index];
                    if (npc.CanBeChasedBy(this, false))
                    {
                        float num1 = Vector2.Distance(npc.Center, Center);
                        if ((Vector2.Distance(Center, vector2_1) > num1 && num1 < num5 || !flag) && Collision.CanHitLine(position, width, height, npc.position, npc.width, npc.height))
                        {
                            num5 = num1;
                            vector2_1 = npc.Center;
                            flag = true;
                            ai1 = index;
                        }
                    }
                }
            }
            int num7 = 500;
            if (flag)
                num7 = 1000;
            if (flag && type == 423)
                num7 = 1200;
            if (flag && type == 613)
                num7 = 1350;
            Player player = Main.player[owner];
            if (Vector2.Distance(player.Center, Center) > num7)
            {
                ai[0] = 1f;
                netUpdate = true;
            }
            if (ai[0] == 1.0)
                tileCollide = false;
            if (flag && ai[0] == 0.0)
            {
                Vector2 vector2_2 = vector2_1 - Center;
                float num1 = vector2_2.Length();
                vector2_2.Normalize();
                if (type == 423)
                {
                    Vector2 vector2_3 = vector2_1 - Vector2.UnitY * 80f;
                    int index = (int)vector2_3.Y / 16;
                    if (index < 0)
                        index = 0;
                    Tile tile1 = Main.tile[(int)vector2_3.X / 16, index];
                    if (tile1 != null && tile1.active() && (Main.tileSolid[tile1.type] && !Main.tileSolidTop[tile1.type]))
                    {
                        vector2_3 += Vector2.UnitY * 16f;
                        Tile tile2 = Main.tile[(int)vector2_3.X / 16, (int)vector2_3.Y / 16];
                        if (tile2 != null && tile2.active() && (Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type]))
                            vector2_3 += Vector2.UnitY * 16f;
                    }
                    vector2_2 = vector2_3 - Center;
                    num1 = vector2_2.Length();
                    vector2_2.Normalize();
                    if (num1 > 300.0 && num1 <= 800.0 && localAI[0] == 0.0)
                    {
                        ai[0] = 2f;
                        ai[1] = (int)(num1 / 10.0);
                        extraUpdates = (int)ai[1];
                        velocity = vector2_2 * 10f;
                        localAI[0] = 60f;
                        return;
                    }
                }
                if (type == 613)
                {
                    Vector2 vector2_3 = vector2_1;
                    Vector2 vector2_4 = Center - vector2_3;
                    if (vector2_4 == Vector2.Zero)
                        vector2_4 = -Vector2.UnitY;
                    vector2_4.Normalize();
                    Vector2 vector2_5 = vector2_3 + vector2_4 * 60f;
                    int index = (int)vector2_5.Y / 16;
                    if (index < 0)
                        index = 0;
                    Tile tile1 = Main.tile[(int)vector2_5.X / 16, index];
                    if (tile1 != null && tile1.active() && (Main.tileSolid[tile1.type] && !Main.tileSolidTop[tile1.type]))
                    {
                        vector2_5 += Vector2.UnitY * 16f;
                        Tile tile2 = Main.tile[(int)vector2_5.X / 16, (int)vector2_5.Y / 16];
                        if (tile2 != null && tile2.active() && (Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type]))
                            vector2_5 += Vector2.UnitY * 16f;
                    }
                    vector2_2 = vector2_5 - Center;
                    num1 = vector2_2.Length();
                    vector2_2.Normalize();
                    if (num1 > 400.0 && num1 <= 800.0 && localAI[0] == 0.0)
                    {
                        ai[0] = 2f;
                        ai[1] = (float)(int)(num1 / 10.0);
                        extraUpdates = (int)ai[1];
                        velocity = vector2_2 * 10f;
                        localAI[0] = 60f;
                        return;
                    }
                }
                if (type == 407)
                {
                    if (num1 > 400.0)
                    {
                        float num2 = 2f;
                        vector2_2 *= num2;
                        velocity = (velocity * 20f + vector2_2) / 21f;
                    }
                    else
                    {
                        Projectile projectile = this;
                        Vector2 vector2_3 = projectile.velocity * 0.96f;
                        projectile.velocity = vector2_3;
                    }
                }
                if (num1 > 200.0)
                {
                    float num2 = 6f;
                    Vector2 vector2_3 = vector2_2 * num2;
                    velocity.X = (float)((velocity.X * 40.0 + vector2_3.X) / 41.0);
                    velocity.Y = (float)((velocity.Y * 40.0 + vector2_3.Y) / 41.0);
                }
                else if (type == 423 || type == 613)
                {
                    if (num1 > 70.0 && num1 < 130.0)
                    {
                        float num2 = 7f;
                        if (num1 < 100.0)
                            num2 = -3f;
                        Vector2 vector2_3 = vector2_2 * num2;
                        velocity = (velocity * 20f + vector2_3) / 21f;
                        if (Math.Abs(vector2_3.X) > Math.Abs(vector2_3.Y))
                            velocity.X = (float)((velocity.X * 10.0 + vector2_3.X) / 11.0);
                    }
                    else
                    {
                        Projectile projectile = this;
                        Vector2 vector2_3 = projectile.velocity * 0.97f;
                        projectile.velocity = vector2_3;
                    }
                }
                else if (type == 375)
                {
                    if (num1 < 150.0)
                    {
                        float num2 = 4f;
                        Vector2 vector2_3 = vector2_2 * -num2;
                        velocity.X = (float)((velocity.X * 40.0 + vector2_3.X) / 41.0);
                        velocity.Y = (float)((velocity.Y * 40.0 + vector2_3.Y) / 41.0);
                    }
                    else
                    {
                        Projectile projectile = this;
                        Vector2 vector2_3 = projectile.velocity * 0.97f;
                        projectile.velocity = vector2_3;
                    }
                }
                else if (velocity.Y > -1.0)
                    velocity.Y -= 0.1f;
            }
            else
            {
                if (!Collision.CanHitLine(Center, 1, 1, Main.player[owner].Center, 1, 1))
                    ai[0] = 1f;
                float num1 = 6f;
                if (ai[0] == 1.0)
                    num1 = 15f;
                if (type == 407)
                    num1 = 9f;
                Vector2 center = Center;
                Vector2 vector2_2 = player.Center - center + new Vector2(0.0f, -60f);
                if (type == 407)
                    vector2_2 += new Vector2(0.0f, 40f);
                if (type == 375)
                {
                    ai[1] = 3600f;
                    netUpdate = true;
                    vector2_2 = player.Center - center;
                    int num2 = 1;
                    for (int index = 0; index < whoAmI; ++index)
                    {
                        if (Main.projectile[index].active && Main.projectile[index].owner == owner && Main.projectile[index].type == type)
                            ++num2;
                    }
                    vector2_2.X -= (10 * Main.player[owner].direction);
                    vector2_2.X -= (num2 * 40 * Main.player[owner].direction);
                    vector2_2.Y -= 10f;
                }
                float num8 = vector2_2.Length();
                if (num8 > 200.0 && num1 < 9.0)
                    num1 = 9f;
                if (type == 375)
                    num1 = (int)(num1 * 0.75);
                if (num8 < 100.0 && ai[0] == 1.0 && !Collision.SolidCollision(position, width, height))
                {
                    ai[0] = 0.0f;
                    netUpdate = true;
                }
                if (num8 > 2000.0)
                {
                    position.X = Main.player[owner].Center.X - (width / 2);
                    position.Y = Main.player[owner].Center.Y - (width / 2);
                }
                if (type == 375)
                {
                    if (num8 > 10.0)
                    {
                        vector2_2.Normalize();
                        if (num8 < 50.0)
                            num1 /= 2f;
                        vector2_2 *= num1;
                        velocity = (velocity * 20f + vector2_2) / 21f;
                    }
                    else
                    {
                        direction = Main.player[owner].direction;
                        Projectile projectile = this;
                        Vector2 vector2_3 = projectile.velocity * 0.9f;
                        projectile.velocity = vector2_3;
                    }
                }
                else if (type == 407)
                {
                    if (Math.Abs(vector2_2.X) > 40.0 || Math.Abs(vector2_2.Y) > 10.0)
                    {
                        vector2_2.Normalize();
                        vector2_2 *= num1;
                        vector2_2 *= new Vector2(1.25f, 0.65f);
                        velocity = (velocity * 20f + vector2_2) / 21f;
                    }
                    else
                    {
                        if (velocity.X == 0.0 && velocity.Y == 0.0)
                        {
                            velocity.X = -0.15f;
                            velocity.Y = -0.05f;
                        }
                        Projectile projectile = this;
                        Vector2 vector2_3 = projectile.velocity * 1.01f;
                        projectile.velocity = vector2_3;
                    }
                }
                else if (num8 > 70.0)
                {
                    vector2_2.Normalize();
                    vector2_2 *= num1;
                    velocity = (velocity * 20f + vector2_2) / 21f;
                }
                else
                {
                    if (velocity.X == 0.0 && velocity.Y == 0.0)
                    {
                        velocity.X = -0.15f;
                        velocity.Y = -0.05f;
                    }
                    Projectile projectile = this;
                    Vector2 vector2_3 = projectile.velocity * 1.01f;
                    projectile.velocity = vector2_3;
                }
            }
            rotation = velocity.X * 0.05f;
            ++frameCounter;
            if (type == 373)
            {
                if (frameCounter > 1)
                {
                    ++frame;
                    frameCounter = 0;
                }
                if (frame > 2)
                    frame = 0;
            }
            if (type == 375)
            {
                if (frameCounter >= 16)
                    frameCounter = 0;
                frame = frameCounter / 4;
                if (ai[1] > 0.0 && ai[1] < 16.0)
                    frame += 4;
                if (Main.rand.Next(6) == 0)
                {
                    int index = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2f);
                    Main.dust[index].velocity *= 0.3f;
                    Main.dust[index].noGravity = true;
                    Main.dust[index].noLight = true;
                }
            }
            if (type == 407)
            {
                int num1 = 2;
                if (frameCounter >= 6 * num1)
                    frameCounter = 0;
                frame = frameCounter / num1;
                if (Main.rand.Next(5) == 0)
                {
                    int index = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 217, 0.0f, 0.0f, 100, new Color(), 2f);
                    Main.dust[index].velocity *= 0.3f;
                    Main.dust[index].noGravity = true;
                    Main.dust[index].noLight = true;
                }
            }
            if (type == 423 || type == 613)
            {
                int num1 = 3;
                if (frameCounter >= 4 * num1)
                    frameCounter = 0;
                frame = frameCounter / num1;
            }
            if (velocity.X > 0.0)
                spriteDirection = direction = -1;
            else if (velocity.X < 0.0)
                spriteDirection = direction = 1;
            if (type == 373)
            {
                if (ai[1] > 0.0)
                    ai[1] += Main.rand.Next(1, 4);
                if (ai[1] > 90.0)
                {
                    ai[1] = 0.0f;
                    netUpdate = true;
                }
            }
            else if (type == 375)
            {
                if (ai[1] > 0.0)
                {
                    ++ai[1];
                    if (Main.rand.Next(3) == 0)
                        ++ai[1];
                }
                if (ai[1] > Main.rand.Next(180, 900))
                {
                    ai[1] = 0.0f;
                    netUpdate = true;
                }
            }
            else if (type == 407)
            {
                if (ai[1] > 0.0)
                {
                    ++ai[1];
                    if (Main.rand.Next(3) != 0)
                        ++ai[1];
                }
                if (ai[1] > 60.0)
                {
                    ai[1] = 0.0f;
                    netUpdate = true;
                }
            }
            else if (type == 423)
            {
                if (ai[1] > 0.0)
                {
                    ++ai[1];
                    if (Main.rand.Next(3) != 0)
                        ++ai[1];
                }
                if (ai[1] > 30.0)
                {
                    ai[1] = 0.0f;
                    netUpdate = true;
                }
            }
            else if (type == 613)
            {
                if (ai[1] > 0.0)
                {
                    ++ai[1];
                    if (Main.rand.Next(3) != 0)
                        ++ai[1];
                }
                if (ai[1] > 60.0)
                {
                    ai[1] = 0.0f;
                    netUpdate = true;
                }
            }
            if (ai[0] != 0.0)
                return;
            float num9 = 0.0f;
            int Type = 0;
            if (type == 373)
            {
                num9 = 10f;
                Type = 374;
            }
            else if (type == 375)
            {
                num9 = 11f;
                Type = 376;
            }
            else if (type == 407)
            {
                num9 = 14f;
                Type = 408;
            }
            else if (type == 423)
            {
                num9 = 4f;
                Type = 433;
            }
            else if (type == 613)
            {
                num9 = 14f;
                Type = 614;
            }
            if (!flag)
                return;
            if (type == 375)
            {
                if ((vector2_1 - Center).X > 0.0)
                    spriteDirection = direction = -1;
                else if ((vector2_1 - Center).X < 0.0)
                    spriteDirection = direction = 1;
            }
            if (type == 407 && Collision.SolidCollision(position, width, height))
                return;
            if (type == 423)
            {
                if (Math.Abs(Utils.ToRotation(vector2_1 - Center) - 1.570796f) > 0.785398185253143)
                {
                    Projectile projectile = this;
                    Vector2 vector2_2 = projectile.velocity + Vector2.Normalize(vector2_1 - Center - Vector2.UnitY * 80f);
                    projectile.velocity = vector2_2;
                }
                else
                {
                    if ((vector2_1 - Center).Length() > 400.0 || ai[1] != 0.0)
                        return;
                    ++ai[1];
                    if (Main.myPlayer != owner)
                        return;
                    Vector2 vector2_2 = vector2_1 - Center;
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * num9;
                    NewProjectile(Center.X, Center.Y, vector2_3.X, vector2_3.Y, Type, damage, 0.0f, Main.myPlayer, 0.0f, 0.0f);
                    netUpdate = true;
                }
            }
            else if (ai[1] == 0.0 && type == 613)
            {
                if ((double)(vector2_1 - Center).Length() > 500.0 || ai[1] != 0.0)
                    return;
                ++ai[1];
                if (Main.myPlayer == owner)
                {
                    Vector2 vector2_2 = vector2_1 - Center;
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * num9;
                    int index = NewProjectile(Center.X, Center.Y, vector2_3.X, vector2_3.Y, Type, damage, 0.0f, Main.myPlayer, 0.0f, ai1);
                    Main.projectile[index].timeLeft = 300;
                    Main.projectile[index].netUpdate = true;
                    Projectile projectile = this;
                    Vector2 vector2_4 = projectile.velocity - vector2_3 / 3f;
                    projectile.velocity = vector2_4;
                    netUpdate = true;
                }
                for (int index1 = 0; index1 < 5; ++index1)
                {
                    int num1 = width / 4;
                    Vector2 vector2_2 = Utils.ToRotationVector2((float)Main.rand.NextDouble() * 6.283185f) * Main.rand.Next(24, 41) / 8f;
                    int index2 = Dust.NewDust(Center - Vector2.One * num1, num1 * 2, num1 * 2, 88, 0.0f, 0.0f, 0, new Color(), 1f);
                    Dust dust = Main.dust[index2];
                    Vector2 vector2_3 = Vector2.Normalize(dust.position - Center);
                    dust.position = Center + vector2_3 * num1 * scale - new Vector2(4f);
                    dust.velocity = index1 >= 30 ? 2f * vector2_3 * Main.rand.Next(45, 91) / 10f : vector2_3 * dust.velocity.Length() * 2f;
                    dust.noGravity = true;
                    dust.scale = 0.7f + Utils.NextFloat(Main.rand);
                }
            }
            else
            {
                if (ai[1] != 0.0)
                    return;
                ++ai[1];
                if (Main.myPlayer != owner)
                    return;
                Vector2 vector2_2 = vector2_1 - Center;
                vector2_2.Normalize();
                Vector2 vector2_3 = vector2_2 * num9;
                int index = NewProjectile(Center.X, Center.Y, vector2_3.X, vector2_3.Y, Type, damage, 0.0f, Main.myPlayer, 0.0f, 0.0f);
                Main.projectile[index].timeLeft = 300;
                Main.projectile[index].netUpdate = true;
                netUpdate = true;
            }
        }

        private void AI_075()
        {
            Player player = Main.player[owner];
            float num1 = 1.570796f;
            Vector2 vector2_1 = player.RotatedRelativePoint(player.MountedCenter, true);
            if (type == 439)
            {
                ++ai[0];
                int num2 = 0;
                if (ai[0] >= 40.0)
                    ++num2;
                if (ai[0] >= 80.0)
                    ++num2;
                if (ai[0] >= 120.0)
                    ++num2;
                int num3 = 24;
                int num4 = 6;
                ++ai[1];
                bool flag = false;
                if (ai[1] >= (double)(num3 - num4 * num2))
                {
                    ai[1] = 0.0f;
                    flag = true;
                }
                frameCounter += 1 + num2;
                if (frameCounter >= 4)
                {
                    frameCounter = 0;
                    ++frame;
                    if (frame >= 6)
                        frame = 0;
                }
                if (soundDelay <= 0)
                {
                    soundDelay = num3 - num4 * num2;
                    if (ai[0] != 1.0)
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 91);
                }
                if (ai[1] == 1.0 && ai[0] != 1.0)
                {
                    Vector2 vector2_2 = Center + Utils.RotatedBy(Vector2.UnitX * 24f, rotation - 1.57079637050629, new Vector2());
                    for (int index1 = 0; index1 < 2; ++index1)
                    {
                        int index2 = Dust.NewDust(vector2_2 - Vector2.One * 8f, 16, 16, 135, velocity.X / 2f, velocity.Y / 2f, 100, new Color(), 1f);
                        Main.dust[index2].velocity *= 0.66f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].scale = 1.4f;
                    }
                }
                if (flag && Main.myPlayer == owner)
                {
                    if (player.channel && player.CheckMana(player.inventory[player.selectedItem].mana, true, false) && !player.noItems)
                    {
                        float num5 = player.inventory[player.selectedItem].shootSpeed * scale;
                        Vector2 vector2_2 = vector2_1;
                        Vector2 vector2_3 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector2_2;
                        if (player.gravDir == -1.0)
                            vector2_3.Y = (Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y;
                        Vector2 vector2_4 = Vector2.Normalize(vector2_3);
                        if (float.IsNaN(vector2_4.X) || float.IsNaN(vector2_4.Y))
                            vector2_4 = -Vector2.UnitY;
                        vector2_4 *= num5;
                        if (vector2_4.X != velocity.X || vector2_4.Y != velocity.Y)
                            netUpdate = true;
                        velocity = vector2_4;
                        int Type = 440;
                        float num6 = 14f;
                        int num7 = 7;
                        for (int index = 0; index < 2; ++index)
                        {
                            Vector2 vector2_5 = Center + new Vector2(Main.rand.Next(-num7, num7 + 1), Main.rand.Next(-num7, num7 + 1));
                            Vector2 spinningpoint = Vector2.Normalize(velocity) * num6;
                            spinningpoint = Utils.RotatedBy(spinningpoint, Main.rand.NextDouble() * 0.196349546313286 - 0.0981747731566429, new Vector2());
                            if (float.IsNaN(spinningpoint.X) || float.IsNaN(spinningpoint.Y))
                                spinningpoint = -Vector2.UnitY;
                            NewProjectile(vector2_5.X, vector2_5.Y, spinningpoint.X, spinningpoint.Y, Type, damage, knockBack, owner, 0.0f, 0.0f);
                        }
                    }
                    else
                        Kill();
                }
            }
            if (type == 445)
            {
                ++localAI[0];
                if (localAI[0] >= 60.0)
                    localAI[0] = 0.0f;
                if (Vector2.Distance(vector2_1, Center) >= 5.0)
                {
                    float num2 = localAI[0] / 60f;
                    if (num2 > 0.5)
                        num2 = 1f - num2;
                    Vector3 vector3 = Vector3.Lerp(new Vector3(0.0f, 1f, 0.7f), new Vector3(0.0f, 0.7f, 1f), (float)(1.0 - num2 * 2.0)) * 0.5f;
                    if (Vector2.Distance(vector2_1, Center) >= 30.0)
                    {
                        Vector2 vector2_2 = Center - vector2_1;
                        vector2_2.Normalize();
                        Vector2 vector2_3 = vector2_2 * (Vector2.Distance(vector2_1, Center) - 30f);
                        DelegateMethods.v3_1 = vector3 * 0.8f;
                        Utils.PlotTileLine(Center - vector2_3, Center, 8f, new Utils.PerLinePoint(DelegateMethods.CastLightOpen));
                    }
                    Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, vector3.X, vector3.Y, vector3.Z);
                }
                if (Main.myPlayer == owner)
                {
                    if (localAI[1] > 0.0)
                        --localAI[1];
                    if (!player.channel || player.noItems)
                        Kill();
                    else if (localAI[1] == 0.0)
                    {
                        Vector2 vector2_2 = vector2_1;
                        Vector2 vector2_3 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector2_2;
                        if (player.gravDir == -1.0)
                            vector2_3.Y = (Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y;
                        if (Main.tile[Player.tileTargetX, Player.tileTargetY].active())
                        {
                            vector2_3 = new Vector2(Player.tileTargetX, Player.tileTargetY) * 16f + Vector2.One * 8f - vector2_2;
                            localAI[1] = 2f;
                        }
                        Vector2 vector2_4 = Vector2.Lerp(vector2_3, velocity, 0.7f);
                        if (float.IsNaN(vector2_4.X) || float.IsNaN(vector2_4.Y))
                            vector2_4 = -Vector2.UnitY;
                        float num2 = 30f;
                        if (vector2_4.Length() < num2)
                            vector2_4 = Vector2.Normalize(vector2_4) * num2;
                        int num3 = player.inventory[player.selectedItem].tileBoost;
                        int num4 = -Player.tileRangeX - num3 + 1;
                        int num5 = Player.tileRangeX + num3 - 1;
                        int num6 = -Player.tileRangeY - num3;
                        int num7 = Player.tileRangeY + num3 - 1;
                        int num8 = 12;
                        bool flag = false;
                        if (vector2_4.X < (num4 * 16 - num8))
                            flag = true;
                        if (vector2_4.Y < (num6 * 16 - num8))
                            flag = true;
                        if (vector2_4.X > (num5 * 16 + num8))
                            flag = true;
                        if (vector2_4.Y > (num7 * 16 + num8))
                            flag = true;
                        if (flag)
                        {
                            Vector2 vector2_5 = Vector2.Normalize(vector2_4);
                            float num9 = -1f;
                            if (vector2_5.X < 0.0 && ((num4 * 16 - num8) / vector2_5.X < num9 || num9 == -1.0))
                                num9 = (num4 * 16 - num8) / vector2_5.X;
                            if (vector2_5.X > 0.0 && ((num5 * 16 + num8) / vector2_5.X < num9 || num9 == -1.0))
                                num9 = (num5 * 16 + num8) / vector2_5.X;
                            if (vector2_5.Y < 0.0 && ((num6 * 16 - num8) / vector2_5.Y < num9 || num9 == -1.0))
                                num9 = (num6 * 16 - num8) / vector2_5.Y;
                            if (vector2_5.Y > 0.0 && ((num7 * 16 + num8) / vector2_5.Y < num9 || num9 == -1.0))
                                num9 = (num7 * 16 + num8) / vector2_5.Y;
                            vector2_4 = vector2_5 * num9;
                        }
                        if (vector2_4.X != velocity.X || vector2_4.Y != velocity.Y)
                            netUpdate = true;
                        velocity = vector2_4;
                    }
                }
            }
            if (type == 460)
            {
                ++ai[0];
                int num2 = 0;
                if (ai[0] >= 60.0)
                    ++num2;
                if (ai[0] >= 180.0)
                    ++num2;
                bool flag1 = false;
                if (ai[0] == 60.0 || ai[0] == 180.0 || ai[0] > 180.0 && ai[0] % 20.0 == 0.0)
                    flag1 = true;
                bool flag2 = ai[0] >= 180.0;
                int num3 = 10;
                if (!flag2)
                    ++ai[1];
                bool flag3 = false;
                if (flag2 && ai[0] % 20.0 == 0.0)
                    flag3 = true;
                if (ai[1] >= num3 && !flag2)
                {
                    ai[1] = 0.0f;
                    flag3 = true;
                    if (!flag2)
                    {
                        float num4 = player.inventory[player.selectedItem].shootSpeed * scale;
                        Vector2 vector2_2 = vector2_1;
                        Vector2 vector2_3 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector2_2;
                        if (player.gravDir == -1.0)
                            vector2_3.Y = (Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y;
                        Vector2 vector2_4 = Vector2.Normalize(vector2_3);
                        if (float.IsNaN(vector2_4.X) || float.IsNaN(vector2_4.Y))
                            vector2_4 = -Vector2.UnitY;
                        vector2_4 *= num4;
                        if (vector2_4.X != velocity.X || vector2_4.Y != velocity.Y)
                            netUpdate = true;
                        velocity = vector2_4;
                    }
                }
                if (soundDelay <= 0 && !flag2)
                {
                    soundDelay = num3 - num2;
                    soundDelay *= 2;
                    if (ai[0] != 1.0)
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 15);
                }
                if (ai[0] > 10.0 && !flag2)
                {
                    Vector2 vector2_2 = Center + Utils.RotatedBy(Vector2.UnitX * 18f, rotation - 1.57079637050629, new Vector2());
                    for (int index1 = 0; index1 < num2 + 1; ++index1)
                    {
                        int Type = 226;
                        float num4 = 0.4f;
                        if (index1 % 2 == 1)
                        {
                            Type = 226;
                            num4 = 0.65f;
                        }
                        Vector2 vector2_3 = vector2_2 + Utils.ToRotationVector2((float)Main.rand.NextDouble() * 6.283185f) * (12f - (num2 * 2));
                        int index2 = Dust.NewDust(vector2_3 - Vector2.One * 8f, 16, 16, Type, velocity.X / 2f, velocity.Y / 2f, 0, new Color(), 1f);
                        Main.dust[index2].velocity = Vector2.Normalize(vector2_2 - vector2_3) * 1.5f * (float)(10.0 - num2 * 2.0) / 10f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].scale = num4;
                        Main.dust[index2].customData = player;
                    }
                }
                if (flag3 && Main.myPlayer == owner)
                {
                    bool flag4 = !flag1 || player.CheckMana(player.inventory[player.selectedItem].mana, true, false);
                    if (player.channel && flag4 && !player.noItems)
                    {
                        if (ai[0] == 180.0)
                        {
                            Vector2 center = Center;
                            Vector2 vector2_2 = Vector2.Normalize(velocity);
                            if (float.IsNaN(vector2_2.X) || float.IsNaN(vector2_2.Y))
                                vector2_2 = -Vector2.UnitY;
                            int Damage = (int)(damage * 3.0);
                            ai[1] = NewProjectile(center.X, center.Y, vector2_2.X, vector2_2.Y, 461, Damage, knockBack, owner, 0.0f, whoAmI);
                            netUpdate = true;
                        }
                        else if (flag2)
                        {
                            Projectile projectile = Main.projectile[(int)ai[1]];
                            if (!projectile.active || projectile.type != 461)
                            {
                                Kill();
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (!flag2)
                        {
                            int Type = 459;
                            float num4 = 10f;
                            Vector2 center = Center;
                            Vector2 vector2_2 = Vector2.Normalize(velocity) * num4;
                            if (float.IsNaN(vector2_2.X) || float.IsNaN(vector2_2.Y))
                                vector2_2 = -Vector2.UnitY;
                            float ai1 = (float)(0.699999988079071 + num2 * 0.300000011920929);
                            int Damage = ai1 < 1.0 ? damage : (int)(damage * 2.0);
                            NewProjectile(center.X, center.Y, vector2_2.X, vector2_2.Y, Type, Damage, knockBack, owner, 0.0f, ai1);
                        }
                        Kill();
                    }
                }
            }
            if (type == 633)
            {
                float num2 = 30f;
                if (ai[0] > 90.0)
                    num2 = 15f;
                if (ai[0] > 120.0)
                    num2 = 5f;
                damage = (int)(player.inventory[player.selectedItem].damage * player.magicDamage);
                ++ai[0];
                ++ai[1];
                bool flag1 = false;
                if (ai[0] % num2 == 0.0)
                    flag1 = true;
                int num3 = 10;
                bool flag2 = false;
                if (ai[0] % num2 == 0.0)
                    flag2 = true;
                if (ai[1] >= 1.0)
                {
                    ai[1] = 0.0f;
                    flag2 = true;
                    if (Main.myPlayer == owner)
                    {
                        float num4 = player.inventory[player.selectedItem].shootSpeed * scale;
                        Vector2 vector2_2 = vector2_1;
                        Vector2 vector2_3 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector2_2;
                        if (player.gravDir == -1.0)
                            vector2_3.Y = (Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y;
                        Vector2 vector2_4 = Vector2.Normalize(vector2_3);
                        if (float.IsNaN(vector2_4.X) || float.IsNaN(vector2_4.Y))
                            vector2_4 = -Vector2.UnitY;
                        vector2_4 = Vector2.Normalize(Vector2.Lerp(vector2_4, Vector2.Normalize(velocity), 0.92f));
                        vector2_4 *= num4;
                        if (vector2_4.X != velocity.X || vector2_4.Y != velocity.Y)
                            netUpdate = true;
                        velocity = vector2_4;
                    }
                }
                ++frameCounter;
                if (frameCounter >= (ai[0] < 120.0 ? 4 : 1))
                {
                    frameCounter = 0;
                    if (++frame >= 5)
                        frame = 0;
                }
                if (soundDelay <= 0)
                {
                    soundDelay = num3;
                    soundDelay *= 2;
                    if (ai[0] != 1.0)
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 15);
                }
                if (flag2 && Main.myPlayer == owner)
                {
                    bool flag3 = !flag1 || player.CheckMana(player.inventory[player.selectedItem].mana, true, false);
                    if (player.channel && flag3 && !player.noItems)
                    {
                        if (ai[0] == 1.0)
                        {
                            Vector2 center = Center;
                            Vector2 vector2_2 = Vector2.Normalize(velocity);
                            if (float.IsNaN(vector2_2.X) || float.IsNaN(vector2_2.Y))
                                vector2_2 = -Vector2.UnitY;
                            int Damage = damage;
                            for (int index = 0; index < 6; ++index)
                                NewProjectile(center.X, center.Y, vector2_2.X, vector2_2.Y, 632, Damage, knockBack, owner, index, whoAmI);
                            netUpdate = true;
                        }
                    }
                    else
                        Kill();
                }
            }
            if (type == 595)
            {
                num1 = 0.0f;
                if (spriteDirection == -1)
                    num1 = 3.141593f;
                if (++frame >= Main.projFrames[type])
                    frame = 0;
                --soundDelay;
                if (soundDelay <= 0)
                {
                    Main.PlaySound(2, (int)Center.X, (int)Center.Y, 1);
                    soundDelay = 12;
                }
                if (Main.myPlayer == owner)
                {
                    if (player.channel && !player.noItems)
                    {
                        float num2 = 1f;
                        if (player.inventory[player.selectedItem].shoot == type)
                            num2 = player.inventory[player.selectedItem].shootSpeed * scale;
                        Vector2 vec = Main.MouseWorld - vector2_1;
                        vec.Normalize();
                        if (Utils.HasNaNs(vec))
                            vec = Vector2.UnitX * player.direction;
                        vec *= num2;
                        if ((double)vec.X != velocity.X || (double)vec.Y != velocity.Y)
                            netUpdate = true;
                        velocity = vec;
                    }
                    else
                        Kill();
                }
                Vector2 position = Center + velocity * 3f;
                Lighting.AddLight(position, 0.8f, 0.8f, 0.8f);
                if (Main.rand.Next(3) == 0)
                {
                    int index = Dust.NewDust(position - Size / 2f, width, height, 63, velocity.X, velocity.Y, 100, new Color(), 2f);
                    Main.dust[index].noGravity = true;
                    Main.dust[index].position -= velocity;
                }
            }
            if (type == 600)
            {
                if (ai[0] == 0.0)
                {
                    if (ai[1] != 0.0)
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 114);
                    else
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 115);
                }
                ++ai[0];
                if (Main.myPlayer == owner && ai[0] == 1.0)
                {
                    float num2 = player.inventory[player.selectedItem].shootSpeed * scale;
                    Vector2 vector2_2 = vector2_1;
                    Vector2 vector2_3 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector2_2;
                    if (player.gravDir == -1.0)
                        vector2_3.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y;
                    Vector2 vector2_4 = Vector2.Normalize(vector2_3);
                    if (float.IsNaN(vector2_4.X) || float.IsNaN(vector2_4.Y))
                        vector2_4 = -Vector2.UnitY;
                    vector2_4 *= num2;
                    if (vector2_4.X != velocity.X || vector2_4.Y != velocity.Y)
                        netUpdate = true;
                    velocity = vector2_4;
                    int Type = 601;
                    float num3 = 3f;
                    Vector2 center = Center;
                    Vector2 vector2_5 = Vector2.Normalize(velocity) * num3;
                    if (float.IsNaN(vector2_5.X) || float.IsNaN(vector2_5.Y))
                        vector2_5 = -Vector2.UnitY;
                    NewProjectile(center.X, center.Y, vector2_5.X, vector2_5.Y, Type, damage, knockBack, owner, ai[1], 0.0f);
                }
                if (ai[0] >= 30.0)
                    Kill();
            }
            if (type == 611)
            {
                if (localAI[1] > 0.0)
                    --localAI[1];
                alpha -= 42;
                if (alpha < 0)
                    alpha = 0;
                if (localAI[0] == 0.0)
                    localAI[0] = Utils.ToRotation(velocity);
                float num2 = Utils.ToRotationVector2(localAI[0]).X >= 0.0 ? 1f : -1f;
                if (ai[1] <= 0.0)
                    num2 *= -1f;
                Vector2 spinningpoint = Utils.ToRotationVector2(num2 * (float)(ai[0] / 30.0 * 6.28318548202515 - 1.57079637050629));
                spinningpoint.Y *= (float)Math.Sin(ai[1]);
                if (ai[1] <= 0.0)
                    spinningpoint.Y *= -1f;
                Vector2 vector2_2 = Utils.RotatedBy(spinningpoint, localAI[0], new Vector2());
                ++ai[0];
                if (ai[0] < 30.0)
                {
                    Projectile projectile = this;
                    Vector2 vector2_3 = projectile.velocity + 48f * vector2_2;
                    projectile.velocity = vector2_3;
                }
                else
                    Kill();
            }
            if (type == 615)
            {
                num1 = 0.0f;
                if (spriteDirection == -1)
                    num1 = 3.141593f;
                ++ai[0];
                int num2 = 0;
                if (ai[0] >= 40.0)
                    ++num2;
                if (ai[0] >= 80.0)
                    ++num2;
                if (ai[0] >= 120.0)
                    ++num2;
                int num3 = 5;
                int num4 = 0;
                --ai[1];
                bool flag = false;
                int num5 = -1;
                if (ai[1] <= 0.0)
                {
                    ai[1] = (float)(num3 - num4 * num2);
                    flag = true;
                    if ((int)ai[0] / (num3 - num4 * num2) % 7 == 0)
                        num5 = 0;
                }
                frameCounter += 1 + num2;
                if (frameCounter >= 4)
                {
                    frameCounter = 0;
                    ++frame;
                    if (frame >= Main.projFrames[type])
                        frame = 0;
                }
                if (soundDelay <= 0)
                {
                    soundDelay = num3 - num4 * num2;
                    if (ai[0] != 1.0)
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 36);
                }
                if (flag && Main.myPlayer == owner)
                {
                    bool canShoot = player.channel && player.HasAmmo(player.inventory[player.selectedItem], true) && !player.noItems;
                    int shoot = 14;
                    float speed = 14f;
                    int Damage = player.inventory[player.selectedItem].damage;
                    float KnockBack = player.inventory[player.selectedItem].knockBack;
                    if (canShoot)
                    {
                        player.PickAmmo(player.inventory[player.selectedItem], ref shoot, ref speed, ref canShoot, ref Damage, ref KnockBack, false);
                        float num6 = player.inventory[player.selectedItem].shootSpeed * scale;
                        Vector2 vector2_2 = vector2_1;
                        Vector2 vector2_3 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector2_2;
                        if (player.gravDir == -1.0)
                            vector2_3.Y = (Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y;
                        Vector2 spinningpoint1 = Vector2.Normalize(vector2_3);
                        if (float.IsNaN(spinningpoint1.X) || float.IsNaN(spinningpoint1.Y))
                            spinningpoint1 = -Vector2.UnitY;
                        spinningpoint1 *= num6;
                        spinningpoint1 = Utils.RotatedBy(spinningpoint1, Main.rand.NextDouble() * 0.130899697542191 - 0.0654498487710953, new Vector2());
                        if ((double)spinningpoint1.X != velocity.X || (double)spinningpoint1.Y != velocity.Y)
                            netUpdate = true;
                        velocity = spinningpoint1;
                        for (int index = 0; index < 1; ++index)
                        {
                            Vector2 spinningpoint2 = Vector2.Normalize(velocity) * speed;
                            spinningpoint2 = Utils.RotatedBy(spinningpoint2, Main.rand.NextDouble() * 0.196349546313286 - 0.0981747731566429, new Vector2());
                            if (float.IsNaN(spinningpoint2.X) || float.IsNaN(spinningpoint2.Y))
                                spinningpoint2 = -Vector2.UnitY;
                            NewProjectile(vector2_2.X, vector2_2.Y, spinningpoint2.X, spinningpoint2.Y, shoot, Damage, KnockBack, owner, 0.0f, 0.0f);
                        }
                        if (num5 == 0)
                        {
                            shoot = 616;
                            float num7 = 8f;
                            for (int index = 0; index < 1; ++index)
                            {
                                Vector2 spinningpoint2 = Vector2.Normalize(velocity) * num7;
                                spinningpoint2 = Utils.RotatedBy(spinningpoint2, Main.rand.NextDouble() * 0.392699092626572 - 0.196349546313286, new Vector2());
                                if (float.IsNaN(spinningpoint2.X) || float.IsNaN(spinningpoint2.Y))
                                    spinningpoint2 = -Vector2.UnitY;
                                NewProjectile(vector2_2.X, vector2_2.Y, spinningpoint2.X, spinningpoint2.Y, shoot, Damage + 20, KnockBack * 1.25f, owner, 0.0f, 0.0f);
                            }
                        }
                    }
                    else
                        Kill();
                }
            }
            if (type == 630)
            {
                num1 = 0.0f;
                if (spriteDirection == -1)
                    num1 = 3.141593f;
                ++ai[0];
                int num2 = 0;
                if (ai[0] >= 40.0)
                    ++num2;
                if (ai[0] >= 80.0)
                    ++num2;
                if (ai[0] >= 120.0)
                    ++num2;
                int num3 = 24;
                int num4 = 2;
                --ai[1];
                bool flag = false;
                if (ai[1] <= 0.0)
                {
                    ai[1] = (num3 - num4 * num2);
                    flag = true;
                    int num5 = (int)ai[0] / (num3 - num4 * num2);
                }
                bool canShoot = player.channel && player.HasAmmo(player.inventory[player.selectedItem], true) && !player.noItems;
                if (localAI[0] > 0.0)
                    --localAI[0];
                if (soundDelay <= 0 && canShoot)
                {
                    soundDelay = num3 - num4 * num2;
                    if (ai[0] != 1.0)
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 5);
                    localAI[0] = 12f;
                }
                player.phantasmTime = 2;
                if (flag && Main.myPlayer == owner)
                {
                    int shoot = 14;
                    float speed = 14f;
                    int Damage = player.inventory[player.selectedItem].damage;
                    float KnockBack = player.inventory[player.selectedItem].knockBack;
                    if (canShoot)
                    {
                        player.PickAmmo(player.inventory[player.selectedItem], ref shoot, ref speed, ref canShoot, ref Damage, ref KnockBack, false);
                        float num5 = player.inventory[player.selectedItem].shootSpeed * scale;
                        Vector2 vector2_2 = vector2_1;
                        Vector2 vector2_3 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector2_2;
                        if (player.gravDir == -1.0)
                            vector2_3.Y = (Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y;
                        Vector2 vector2_4 = Vector2.Normalize(vector2_3);
                        if (float.IsNaN(vector2_4.X) || float.IsNaN(vector2_4.Y))
                            vector2_4 = -Vector2.UnitY;
                        vector2_4 *= num5;
                        if (vector2_4.X != velocity.X || vector2_4.Y != velocity.Y)
                            netUpdate = true;
                        velocity = vector2_4 * 0.55f;
                        for (int index1 = 0; index1 < 4; ++index1)
                        {
                            Vector2 vector2_5 = Vector2.Normalize(velocity) * speed * (float)(0.600000023841858 + Utils.NextFloat(Main.rand) * 0.800000011920929);
                            if (float.IsNaN(vector2_5.X) || float.IsNaN(vector2_5.Y))
                                vector2_5 = -Vector2.UnitY;
                            Vector2 vector2_6 = vector2_2 + Utils.RandomVector2(Main.rand, -15f, 15f);
                            int index2 = NewProjectile(vector2_6.X, vector2_6.Y, vector2_5.X, vector2_5.Y, shoot, Damage, KnockBack, owner, 0.0f, 0.0f);
                            Main.projectile[index2].noDropItem = true;
                        }
                    }
                    else
                        Kill();
                }
            }
            position = player.RotatedRelativePoint(player.MountedCenter, true) - Size / 2f;
            rotation = Utils.ToRotation(velocity) + num1;
            spriteDirection = direction;
            timeLeft = 2;
            player.ChangeDir(direction);
            player.heldProj = whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(velocity.Y * (double)direction, velocity.X * (double)direction);
            if (type == 460 || type == 611)
            {
                Vector2 vector2_2 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
                if (player.direction != 1)
                    vector2_2.X = (float)player.bodyFrame.Width - vector2_2.X;
                if (player.gravDir != 1.0)
                    vector2_2.Y = (float)player.bodyFrame.Height - vector2_2.Y;
                Vector2 vector2_3 = vector2_2 - new Vector2((player.bodyFrame.Width - player.width), (player.bodyFrame.Height - 42)) / 2f;
                Center = player.RotatedRelativePoint(player.position + vector2_3, true) - velocity;
            }
            if (type == 615)
                position.Y += player.gravDir * 2f;
            if (type != 611 || alpha != 0)
                return;
            for (int index = 0; index < 2; ++index)
            {
                Dust dust = Main.dust[Dust.NewDust(position + velocity * 2f, width, height, 6, 0.0f, 0.0f, 100, Color.Transparent, 2f)];
                dust.noGravity = true;
                dust.velocity *= 2f;
                dust.velocity += Utils.ToRotationVector2(localAI[0]);
                dust.fadeIn = 1.5f;
            }
            float num10 = 18f;
            for (int index = 0; (double)index < num10; ++index)
            {
                if (Main.rand.Next(4) == 0)
                {
                    Vector2 Position = position + velocity + velocity * ((float)index / num10);
                    Dust dust = Main.dust[Dust.NewDust(Position, width, height, 6, 0.0f, 0.0f, 100, Color.Transparent, 1f)];
                    dust.noGravity = true;
                    dust.fadeIn = 0.5f;
                    dust.velocity += Utils.ToRotationVector2(localAI[0]);
                    dust.noLight = true;
                }
            }
        }

        private void AI_099_1()
        {
            timeLeft = 6;
            bool flag1 = true;
            float num1 = 250f;
            float num2 = 0.1f;
            float num3 = 15f;
            float num4 = 12f;
            float num5 = num1 * 0.5f;
            float num6 = num3 * 0.8f;
            float num7 = num4 * 1.5f;
            if (owner == Main.myPlayer)
            {
                bool flag2 = false;
                for (int index = 0; index < 1000; ++index)
                {
                    if (Main.projectile[index].active && Main.projectile[index].owner == owner && Main.projectile[index].aiStyle == 99 && (Main.projectile[index].type < 556 || Main.projectile[index].type > 561))
                        flag2 = true;
                }
                if (!flag2)
                {
                    ai[0] = -1f;
                    netUpdate = true;
                }
            }
            if (Main.player[owner].yoyoString)
                num5 += (float)(num5 * 0.25 + 10.0);
            rotation += 0.5f;
            if (Main.player[owner].dead)
            {
                Kill();
            }
            else
            {
                if (!flag1)
                {
                    Main.player[owner].heldProj = whoAmI;
                    Main.player[owner].itemAnimation = 2;
                    Main.player[owner].itemTime = 2;
                    if (position.X + (width / 2) > Main.player[owner].position.X + (Main.player[owner].width / 2))
                    {
                        Main.player[owner].ChangeDir(1);
                        direction = 1;
                    }
                    else
                    {
                        Main.player[owner].ChangeDir(-1);
                        direction = -1;
                    }
                }
                if (ai[0] == 0.0 || ai[0] == 1.0)
                {
                    if (ai[0] == 1.0)
                        num5 *= 0.75f;
                    float num8 = num7 * 0.5f;
                    bool flag2 = false;
                    Vector2 vector2_1 = Main.player[owner].Center - Center;
                    if ((double)vector2_1.Length() > num5 * 0.9)
                        flag2 = true;
                    if ((double)vector2_1.Length() > num5)
                    {
                        float num9 = vector2_1.Length() - num5;
                        Vector2 vector2_2;
                        vector2_2.X = vector2_1.Y;
                        vector2_2.Y = vector2_1.X;
                        vector2_1.Normalize();
                        vector2_1 *= num5;
                        position = Main.player[owner].Center - vector2_1;
                        position.X -= (width / 2);
                        position.Y -= (height / 2);
                        float num10 = velocity.Length();
                        velocity.Normalize();
                        if (num9 > num10 - 1.0)
                            num9 = num10 - 1f;
                        Projectile projectile = this;
                        Vector2 vector2_3 = projectile.velocity * (num10 - num9);
                        projectile.velocity = vector2_3;
                        velocity.Length();
                        Vector2 vector2_4 = new Vector2(Center.X, Center.Y);
                        Vector2 vector2_5 = new Vector2(Main.player[owner].Center.X, Main.player[owner].Center.Y);
                        if (vector2_4.Y < vector2_5.Y)
                            vector2_2.Y = Math.Abs(vector2_2.Y);
                        else if (vector2_4.Y > vector2_5.Y)
                            vector2_2.Y = -Math.Abs(vector2_2.Y);
                        if (vector2_4.X < vector2_5.X)
                            vector2_2.X = Math.Abs(vector2_2.X);
                        else if (vector2_4.X > vector2_5.X)
                            vector2_2.X = -Math.Abs(vector2_2.X);
                        vector2_2.Normalize();
                        Vector2 vector2_6 = vector2_2 * velocity.Length();
                        Vector2 vector2_7 = new Vector2(vector2_6.X, vector2_6.Y);
                        if (Math.Abs(velocity.X) > Math.Abs(velocity.Y))
                        {
                            Vector2 vector2_8 = velocity;
                            vector2_8.Y += vector2_6.Y;
                            vector2_8.Normalize();
                            vector2_8 *= velocity.Length();
                            if (Math.Abs(vector2_6.X) < 0.1 || Math.Abs(vector2_6.Y) < 0.1)
                                velocity = vector2_8;
                            else
                                velocity = (vector2_8 + velocity * 2f) / 3f;
                        }
                        else
                        {
                            Vector2 vector2_8 = velocity;
                            vector2_8.X += vector2_6.X;
                            vector2_8.Normalize();
                            vector2_8 *= velocity.Length();
                            if (Math.Abs(vector2_6.X) < 0.2 || Math.Abs(vector2_6.Y) < 0.2)
                                velocity = vector2_8;
                            else
                                velocity = (vector2_8 + velocity * 2f) / 3f;
                        }
                    }
                    if (Main.myPlayer == owner)
                    {
                        if (Main.player[owner].channel)
                        {
                            Vector2 vector2_2 = new Vector2((Main.mouseX - Main.lastMouseX), (Main.mouseY - Main.lastMouseY));
                            if (velocity.X != 0.0 || velocity.Y != 0.0)
                            {
                                if (flag1)
                                    vector2_2 *= -1f;
                                if (flag2)
                                {
                                    if (Center.X < Main.player[owner].Center.X && vector2_2.X < 0.0)
                                        vector2_2.X = 0.0f;
                                    if (Center.X > Main.player[owner].Center.X && vector2_2.X > 0.0)
                                        vector2_2.X = 0.0f;
                                    if (Center.Y < Main.player[owner].Center.Y && vector2_2.Y < 0.0)
                                        vector2_2.Y = 0.0f;
                                    if (Center.Y > Main.player[owner].Center.Y && vector2_2.Y > 0.0)
                                        vector2_2.Y = 0.0f;
                                }
                                Projectile projectile = this;
                                Vector2 vector2_3 = projectile.velocity + vector2_2 * num2;
                                projectile.velocity = vector2_3;
                                netUpdate = true;
                            }
                        }
                        else
                        {
                            ai[0] = 10f;
                            netUpdate = true;
                        }
                    }
                    if (flag1 || type == 562 || (type == 547 || type == 555) || (type == 564 || type == 552 || (type == 563 || type == 549)) || (type == 550 || type == 554 || (type == 553 || type == 603)))
                    {
                        float num9 = 800f;
                        Vector2 vector2_2 = new Vector2();
                        bool flag3 = false;
                        if (type == 549)
                            num9 = 200f;
                        if (type == 554)
                            num9 = 400f;
                        if (type == 553)
                            num9 = 250f;
                        if (type == 603)
                            num9 = 320f;
                        for (int index = 0; index < 200; ++index)
                        {
                            if (Main.npc[index].CanBeChasedBy(this, false))
                            {
                                float num10 = Main.npc[index].position.X + (Main.npc[index].width / 2);
                                float num11 = Main.npc[index].position.Y + (Main.npc[index].height / 2);
                                float num12 = Math.Abs(position.X + (width / 2) - num10) + Math.Abs(position.Y + (height / 2) - num11);
                                if (num12 < num9 && (type != 563 || num12 >= 200.0) && Collision.CanHit(position, width, height, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height) && (Main.npc[index].Center - Main.player[owner].Center).Length() < num5 * 0.9)
                                {
                                    num9 = num12;
                                    vector2_2.X = num10;
                                    vector2_2.Y = num11;
                                    flag3 = true;
                                }
                            }
                        }
                        if (flag3)
                        {
                            vector2_2 -= Center;
                            vector2_2.Normalize();
                            if (type == 563)
                            {
                                vector2_2 *= 4f;
                                velocity = (velocity * 14f + vector2_2) / 15f;
                            }
                            else if (type == 553)
                            {
                                vector2_2 *= 5f;
                                velocity = (velocity * 12f + vector2_2) / 13f;
                            }
                            else if (type == 603)
                            {
                                vector2_2 *= 16f;
                                velocity = (velocity * 9f + vector2_2) / 10f;
                            }
                            else if (type == 554)
                            {
                                vector2_2 *= 8f;
                                velocity = (velocity * 6f + vector2_2) / 7f;
                            }
                            else
                            {
                                vector2_2 *= 6f;
                                velocity = (velocity * 7f + vector2_2) / 8f;
                            }
                        }
                    }
                    if (velocity.Length() > num6)
                    {
                        velocity.Normalize();
                        Projectile projectile = this;
                        Vector2 vector2_2 = projectile.velocity * num6;
                        projectile.velocity = vector2_2;
                    }
                    if (velocity.Length() >= num8)
                        return;
                    velocity.Normalize();
                    Projectile projectile1 = this;
                    Vector2 vector2_9 = projectile1.velocity * num8;
                    projectile1.velocity = vector2_9;
                }
                else
                {
                    tileCollide = false;
                    Vector2 vec = Main.player[owner].Center - Center;
                    if (vec.Length() < 40.0 || Utils.HasNaNs(vec))
                    {
                        Kill();
                    }
                    else
                    {
                        float num8 = num6 * 1.5f;
                        if (type == 546)
                            num8 *= 1.5f;
                        if (type == 554)
                            num8 *= 1.25f;
                        if (type == 555)
                            num8 *= 1.35f;
                        if (type == 562)
                            num8 *= 1.25f;
                        float num9 = 12f;
                        vec.Normalize();
                        vec *= num8;
                        velocity = (velocity * (num9 - 1f) + vec) / num9;
                    }
                }
            }
        }

        private void AI_099_2()
        {
            bool flag1 = false;
            for (int index = 0; index < whoAmI; ++index)
            {
                if (Main.projectile[index].active && Main.projectile[index].owner == owner && Main.projectile[index].type == type)
                    flag1 = true;
            }
            if (owner == Main.myPlayer)
            {
                ++localAI[0];
                if (flag1)
                    localAI[0] += Main.rand.Next(10, 31) * 0.1f;
                float num = localAI[0] / 60f / (float)((1.0 + Main.player[owner].meleeSpeed) / 2.0);
                if (type == 541 && num > 3.0)
                    ai[0] = -1f;
                if (type == 548 && num > 5.0)
                    ai[0] = -1f;
                if (type == 542 && num > 7.0)
                    ai[0] = -1f;
                if (type == 543 && num > 6.0)
                    ai[0] = -1f;
                if (type == 544 && num > 8.0)
                    ai[0] = -1f;
                if (type == 534 && num > 9.0)
                    ai[0] = -1f;
                if (type == 564 && num > 11.0)
                    ai[0] = -1f;
                if (type == 545 && num > 13.0)
                    ai[0] = -1f;
                if (type == 563 && num > 10.0)
                    ai[0] = -1f;
                if (type == 562 && num > 8.0)
                    ai[0] = -1f;
                if (type == 553 && num > 12.0)
                    ai[0] = -1f;
                if (type == 546 && num > 16.0)
                    ai[0] = -1f;
                if (type == 552 && num > 15.0)
                    ai[0] = -1f;
                if (type == 549 && num > 14.0)
                    ai[0] = -1f;
            }
            if (type == 603 && owner == Main.myPlayer)
            {
                ++localAI[1];
                if (localAI[1] >= 6.0)
                {
                    float num1 = 400f;
                    Vector2 vector2_1 = velocity;
                    Vector2 vector2_2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * (Main.rand.Next(10, 41) * 0.1f);
                    if (Main.rand.Next(3) == 0)
                        vector2_3 *= 2f;
                    Vector2 vector2_4 = vector2_1 * 0.25f + vector2_3;
                    for (int index = 0; index < 200; ++index)
                    {
                        if (Main.npc[index].CanBeChasedBy(this, false))
                        {
                            float num2 = Main.npc[index].position.X + (float)(Main.npc[index].width / 2);
                            float num3 = Main.npc[index].position.Y + (float)(Main.npc[index].height / 2);
                            float num4 = Math.Abs(position.X + (width / 2) - num2) + Math.Abs(position.Y + (height / 2) - num3);
                            if (num4 < num1 && Collision.CanHit(position, width, height, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height))
                            {
                                num1 = num4;
                                vector2_4.X = num2;
                                vector2_4.Y = num3;
                                Vector2 vector2_5 = vector2_4 - Center;
                                vector2_5.Normalize();
                                vector2_4 = vector2_5 * 8f;
                            }
                        }
                    }
                    Vector2 vector2_6 = vector2_4 * 0.8f;
                    NewProjectile(Center.X, Center.Y, vector2_6.X, vector2_6.Y, 604, damage, knockBack, owner, 0.0f, 0.0f);
                    localAI[1] = 0.0f;
                }
            }
            bool flag2 = false;
            if (type >= 556 && type <= 561)
                flag2 = true;
            if (Main.player[owner].dead)
            {
                Kill();
            }
            else
            {
                if (!flag2 && !flag1)
                {
                    Main.player[owner].heldProj = whoAmI;
                    Main.player[owner].itemAnimation = 2;
                    Main.player[owner].itemTime = 2;
                    if (position.X + (width / 2) > Main.player[owner].position.X + (Main.player[owner].width / 2))
                    {
                        Main.player[owner].ChangeDir(1);
                        direction = 1;
                    }
                    else
                    {
                        Main.player[owner].ChangeDir(-1);
                        direction = -1;
                    }
                }
                if (Utils.HasNaNs(velocity))
                    Kill();
                timeLeft = 6;
                float num1 = 10f;
                float num2 = 200f;
                if (type == 541)
                {
                    num2 = 130f;
                    num1 = 9f;
                }
                else if (type == 548)
                {
                    num2 = 170f;
                    num1 = 11f;
                }
                else if (type == 542)
                {
                    num2 = 195f;
                    num1 = 12.5f;
                }
                else if (type == 543)
                {
                    num2 = 207f;
                    num1 = 12f;
                }
                else if (type == 544)
                {
                    num2 = 215f;
                    num1 = 13f;
                }
                else if (type == 534)
                {
                    num2 = 220f;
                    num1 = 13f;
                }
                else if (type == 564)
                {
                    num2 = 225f;
                    num1 = 14f;
                }
                else if (type == 545)
                {
                    if (Main.rand.Next(6) == 0)
                    {
                        int index = Dust.NewDust(position, width, height, 6, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index].noGravity = true;
                    }
                    num2 = 235f;
                    num1 = 14f;
                }
                else if (type == 562)
                {
                    num2 = 235f;
                    num1 = 15f;
                }
                else if (type == 563)
                {
                    num2 = 250f;
                    num1 = 12f;
                }
                else if (type == 546)
                {
                    num2 = 275f;
                    num1 = 17f;
                }
                else if (type == 552)
                {
                    num2 = 270f;
                    num1 = 14f;
                }
                else if (type == 553)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        int index = Dust.NewDust(position, width, height, 6, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index].noGravity = true;
                        Main.dust[index].scale = 1.6f;
                    }
                    num2 = 275f;
                    num1 = 15f;
                }
                else if (type == 547)
                {
                    num2 = 280f;
                    num1 = 17f;
                }
                else if (type == 549)
                {
                    num2 = 290f;
                    num1 = 16f;
                }
                else if (type == 554)
                {
                    num2 = 340f;
                    num1 = 16f;
                }
                else if (type == 550 || type == 551)
                {
                    num2 = 370f;
                    num1 = 16f;
                }
                else if (type == 555)
                {
                    num2 = 360f;
                    num1 = 16.5f;
                }
                else if (type == 603)
                {
                    num2 = 400f;
                    num1 = 17.5f;
                }
                if (Main.player[owner].yoyoString)
                    num2 = (float)(num2 * 1.25 + 30.0);
                float num3 = num2 / (float)((1.0 + Main.player[owner].meleeSpeed * 3.0) / 4.0);
                float num4 = num1 / (float)((1.0 + Main.player[owner].meleeSpeed * 3.0) / 4.0);
                float num5 = (float)(14.0 - num4 / 2.0);
                float num6 = (float)(5.0 + num4 / 2.0);
                if (flag1)
                    num6 += 20f;
                if (ai[0] >= 0.0)
                {
                    if (velocity.Length() > num4)
                    {
                        Projectile projectile = this;
                        Vector2 vector2 = projectile.velocity * 0.98f;
                        projectile.velocity = vector2;
                    }
                    bool flag3 = false;
                    bool flag4 = false;
                    Vector2 vector2_1 = Main.player[owner].Center - Center;
                    if ((double)vector2_1.Length() > num3)
                    {
                        flag3 = true;
                        if ((double)vector2_1.Length() > num3 * 1.3)
                            flag4 = true;
                    }
                    if (owner == Main.myPlayer)
                    {
                        if (!Main.player[owner].channel || Main.player[owner].stoned || Main.player[owner].frozen)
                        {
                            ai[0] = -1f;
                            ai[1] = 0.0f;
                            netUpdate = true;
                        }
                        else
                        {
                            Vector2 vector2_2 = Main.ReverseGravitySupport(Main.MouseScreen, 0.0f) + Main.screenPosition;
                            float x = vector2_2.X;
                            float y = vector2_2.Y;
                            Vector2 vector2_3 = new Vector2(x, y) - Main.player[owner].Center;
                            if (vector2_3.Length() > num3)
                            {
                                vector2_3.Normalize();
                                Vector2 vector2_4 = vector2_3 * num3;
                                Vector2 vector2_5 = Main.player[owner].Center + vector2_4;
                                x = vector2_5.X;
                                y = vector2_5.Y;
                            }
                            if (ai[0] != (double)x || ai[1] != (double)y)
                            {
                                Vector2 vector2_4 = new Vector2(x, y) - Main.player[owner].Center;
                                if (vector2_4.Length() > num3 - 1.0)
                                {
                                    vector2_4.Normalize();
                                    Vector2 vector2_5 = vector2_4 * (num3 - 1f);
                                    Vector2 vector2_6 = Main.player[owner].Center + vector2_5;
                                    x = vector2_6.X;
                                    y = vector2_6.Y;
                                }
                                ai[0] = x;
                                ai[1] = y;
                                netUpdate = true;
                            }
                        }
                    }
                    if (flag4 && owner == Main.myPlayer)
                    {
                        ai[0] = -1f;
                        netUpdate = true;
                    }
                    if (ai[0] >= 0.0)
                    {
                        if (flag3)
                        {
                            num5 /= 2f;
                            num4 *= 2f;
                            if (Center.X > Main.player[owner].Center.X && velocity.X > 0.0)
                                velocity.X *= 0.5f;
                            if (Center.Y > Main.player[owner].Center.Y && velocity.Y > 0.0)
                                velocity.Y *= 0.5f;
                            if (Center.X < Main.player[owner].Center.X && velocity.X > 0.0)
                                velocity.X *= 0.5f;
                            if (Center.Y < Main.player[owner].Center.Y && velocity.Y > 0.0)
                                velocity.Y *= 0.5f;
                        }
                        Vector2 vector2_2 = new Vector2(ai[0], ai[1]) - Center;
                        double num7 = velocity.Length();
                        if (vector2_2.Length() > num6)
                        {
                            vector2_2.Normalize();
                            vector2_2 *= num4;
                            velocity = (velocity * (num5 - 1f) + vector2_2) / num5;
                        }
                        else if (flag1)
                        {
                            if (velocity.Length() < num4 * 0.6)
                            {
                                vector2_2 = velocity;
                                vector2_2.Normalize();
                                vector2_2 *= num4 * 0.6f;
                                velocity = (velocity * (num5 - 1f) + vector2_2) / num5;
                            }
                        }
                        else
                        {
                            Projectile projectile = this;
                            Vector2 vector2_3 = projectile.velocity * 0.8f;
                            projectile.velocity = vector2_3;
                        }
                        if (flag1 && !flag3 && velocity.Length() < num4 * 0.6)
                        {
                            velocity.Normalize();
                            Projectile projectile = this;
                            Vector2 vector2_3 = projectile.velocity * (num4 * 0.6f);
                            projectile.velocity = vector2_3;
                        }
                    }
                }
                else
                {
                    float num7 = (float)(int)(num5 * 0.8);
                    float num8 = num4 * 1.5f;
                    tileCollide = false;
                    Vector2 vector2 = Main.player[owner].position - Center;
                    float num9 = vector2.Length();
                    if (num9 < num8 + 10.0 || num9 == 0.0)
                    {
                        Kill();
                    }
                    else
                    {
                        vector2.Normalize();
                        vector2 *= num8;
                        velocity = (velocity * (num7 - 1f) + vector2) / num7;
                    }
                }
                rotation += 0.45f;
            }
        }

        public void Kill()
        {
            if (!active)
                return;
            int num1 = timeLeft;
            timeLeft = 0;
            if (type == 634 || type == 635)
            {
                int num2 = Utils.SelectRandom<int>(Main.rand, 242, 73, 72, 71, 255);
                int Type1 = 255;
                int Type2 = 255;
                int num3 = 50;
                float Scale1 = 1.7f;
                float Scale2 = 0.8f;
                float Scale3 = 2f;
                Vector2 vector2 = Utils.ToRotationVector2(rotation - 1.570796f) * velocity.Length() * (float)MaxUpdates;
                if (type == 635)
                {
                    Type1 = 88;
                    Type2 = 88;
                    num2 = Utils.SelectRandom<int>(Main.rand, 242, 59, 88);
                    Scale1 = 3.7f;
                    Scale2 = 1.5f;
                    Scale3 = 2.2f;
                    vector2 *= 0.5f;
                }
                Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                position = Center;
                width = height = num3;
                Center = position;
                maxPenetrate = -1;
                penetrate = -1;
                Damage();
                for (int index1 = 0; index1 < 40; ++index1)
                {
                    int Type3 = Utils.SelectRandom<int>(Main.rand, 242, 73, 72, 71, 255);
                    if (type == 635)
                        Type3 = Utils.SelectRandom<int>(Main.rand, 242, 59, 88);
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, Type3, 0.0f, 0.0f, 200, new Color(), Scale1);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                    Main.dust[index2].velocity += vector2 * Utils.NextFloat(Main.rand);
                    int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, Type1, 0.0f, 0.0f, 100, new Color(), Scale2);
                    Main.dust[index3].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.dust[index3].velocity *= 2f;
                    Main.dust[index3].noGravity = true;
                    Main.dust[index3].fadeIn = 1f;
                    Main.dust[index3].color = Color.Crimson * 0.5f;
                    Main.dust[index3].velocity += vector2 * Utils.NextFloat(Main.rand);
                }
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, Type2, 0.0f, 0.0f, 0, new Color(), Scale3);
                    Main.dust[index2].position = Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.14159274101257), Utils.ToRotation(velocity), new Vector2()) * width / 3f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 0.5f;
                    Main.dust[index2].velocity += vector2 * (float)(0.600000023841858 + 0.600000023841858 * (double)Utils.NextFloat(Main.rand));
                }
            }
            else if (type == 641)
            {
                if (owner == Main.myPlayer)
                {
                    for (int index = 0; index < 1000; ++index)
                    {
                        if (Main.projectile[index].active && Main.projectile[index].owner == owner && Main.projectile[index].type == 642)
                            Main.projectile[index].Kill();
                    }
                }
            }
            else if (type == 643)
            {
                if (owner == Main.myPlayer)
                {
                    for (int index = 0; index < 1000; ++index)
                    {
                        if (Main.projectile[index].active && Main.projectile[index].owner == owner && Main.projectile[index].type == 644)
                            Main.projectile[index].Kill();
                    }
                }
            }
            else if (type == 645)
            {
                bool flag = WorldGen.SolidTile(Framing.GetTileSafely((int)position.X / 16, (int)position.Y / 16));
                for (int index = 0; index < 4; ++index)
                    Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                for (int index1 = 0; index1 < 4; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 229, 0.0f, 0.0f, 0, new Color(), 2.5f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                    if (flag)
                        Main.dust[index2].noLight = true;
                    int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 229, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index3].velocity *= 2f;
                    Main.dust[index3].noGravity = true;
                    if (flag)
                        Main.dust[index3].noLight = true;
                }
                for (int index1 = 0; index1 < 1; ++index1)
                {
                    int index2 = Gore.NewGore(position + new Vector2((width * Main.rand.Next(100)) / 100f, (height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index2].velocity *= 0.3f;
                    Main.gore[index2].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
            }
            else if (type == 636)
            {
                Rectangle hitbox = Hitbox;
                int index1 = 0;
                while (index1 < 6)
                {
                    hitbox.X = (int)oldPos[index1].X;
                    hitbox.Y = (int)oldPos[index1].Y;
                    for (int index2 = 0; index2 < 5; ++index2)
                    {
                        int Type = Utils.SelectRandom<int>(Main.rand, 6, 259, 158);
                        int index3 = Dust.NewDust(Utils.TopLeft(hitbox), width, height, Type, 2.5f * direction, -2.5f, 0, new Color(), 1f);
                        Main.dust[index3].alpha = 200;
                        Main.dust[index3].velocity *= 2.4f;
                        Main.dust[index3].scale += Utils.NextFloat(Main.rand);
                    }
                    index1 += 3;
                }
            }
            else if (type == 614)
            {
                for (int index = 0; index < 10; ++index)
                {
                    Dust dust = Main.dust[Dust.NewDust(position, width, height, 229, 0.0f, 0.0f, 0, new Color(), 1f)];
                    dust.noGravity = true;
                    dust.velocity *= 3f;
                }
            }
            if (type == 644)
            {
                Vector2 spinningpoint = Utils.RotatedByRandom(new Vector2(0.0f, -3f), 3.14159274101257);
                float num2 = Main.rand.Next(7, 13);
                Vector2 vector2 = new Vector2(2.1f, 2f);
                Color newColor = Main.hslToRgb(ai[0], 1f, 0.5f);
                newColor.A = byte.MaxValue;
                for (float num3 = 0.0f; num3 < num2; ++num3)
                {
                    int dustIndex = Dust.NewDust(Center, 0, 0, 267, 0.0f, 0.0f, 0, newColor, 1f);
                    Main.dust[dustIndex].position = Center;
                    Main.dust[dustIndex].velocity = Utils.RotatedBy(spinningpoint, 6.28318548202515 * num3 / num2, new Vector2()) * vector2 * (float)(0.800000011920929 + (double)Utils.NextFloat(Main.rand) * 0.400000005960464);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].scale = 2f;
                    Main.dust[dustIndex].fadeIn = Utils.NextFloat(Main.rand) * 2f;
                    Dust dust = Dust.CloneDust(dustIndex);
                    dust.scale /= 2f;
                    dust.fadeIn /= 2f;
                    dust.color = new Color(255, 255, 255, 255);
                }
                for (float num3 = 0.0f; num3 < num2; ++num3)
                {
                    int dustIndex = Dust.NewDust(Center, 0, 0, 267, 0.0f, 0.0f, 0, newColor, 1f);
                    Main.dust[dustIndex].position = Center;
                    Main.dust[dustIndex].velocity = Utils.RotatedBy(spinningpoint, 6.28318548202515 * num3 / num2, new Vector2()) * vector2 * (float)(0.800000011920929 + (double)Utils.NextFloat(Main.rand) * 0.400000005960464);
                    Main.dust[dustIndex].velocity *= Utils.NextFloat(Main.rand) * 0.8f;
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].scale = Utils.NextFloat(Main.rand) * 1f;
                    Main.dust[dustIndex].fadeIn = Utils.NextFloat(Main.rand) * 2f;
                    Dust dust = Dust.CloneDust(dustIndex);
                    dust.scale /= 2f;
                    dust.fadeIn /= 2f;
                    dust.color = new Color(255, 255, 255, 255);
                }
                if (Main.myPlayer == owner)
                {
                    friendly = true;
                    int num3 = width;
                    int num4 = height;
                    int num5 = penetrate;
                    position = Center;
                    width = height = 60;
                    Center = position;
                    penetrate = -1;
                    maxPenetrate = -1;
                    Damage();
                    penetrate = num5;
                    position = Center;
                    width = num3;
                    height = num4;
                    Center = position;
                }
            }
            if (type == 608)
            {
                maxPenetrate = -1;
                penetrate = -1;
                Damage();
                Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                for (int index1 = 0; index1 < 4; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                }
                for (int index1 = 0; index1 < 30; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 200, new Color(), 3.7f);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                    Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(Main.player[owner].ArmorSetDye(), Main.player[owner]);
                    int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index3].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.dust[index3].velocity *= 2f;
                    Main.dust[index3].noGravity = true;
                    Main.dust[index3].fadeIn = 2.5f;
                    Main.dust[index3].shader = GameShaders.Armor.GetSecondaryShader(Main.player[owner].ArmorSetDye(), Main.player[owner]);
                }
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 0, new Color(), 2.7f);
                    Main.dust[index2].position = Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.14159274101257), Utils.ToRotation(velocity), new Vector2()) * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                    Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(Main.player[owner].ArmorSetDye(), Main.player[owner]);
                }
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 0, new Color(), 1.5f);
                    Main.dust[index2].position = Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.14159274101257), Utils.ToRotation(velocity), new Vector2()) * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                }
                for (int index1 = 0; index1 < 2; ++index1)
                {
                    int index2 = Gore.NewGore(position + new Vector2((width * Main.rand.Next(100)) / 100f, (height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.gore[index2].velocity *= 0.3f;
                    Main.gore[index2].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
            }
            else if (type == 617)
            {
                position = Center;
                width = height = 176;
                Center = position;
                maxPenetrate = -1;
                penetrate = -1;
                Damage();
                Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                for (int index1 = 0; index1 < 4; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 240, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                }
                for (int index1 = 0; index1 < 30; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 62, 0.0f, 0.0f, 200, new Color(), 3.7f);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                    int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 90, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index3].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.dust[index3].velocity *= 2f;
                    Main.dust[index3].noGravity = true;
                    Main.dust[index3].fadeIn = 1f;
                    Main.dust[index3].color = Color.Crimson * 0.5f;
                }
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 62, 0.0f, 0.0f, 0, new Color(), 2.7f);
                    Main.dust[index2].position = Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.14159274101257), Utils.ToRotation(velocity), new Vector2()) * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                }
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 240, 0.0f, 0.0f, 0, new Color(), 1.5f);
                    Main.dust[index2].position = Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.14159274101257), Utils.ToRotation(velocity), new Vector2()) * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                }
                for (int index1 = 0; index1 < 2; ++index1)
                {
                    int index2 = Gore.NewGore(position + new Vector2((width * Main.rand.Next(100)) / 100f, (height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.gore[index2].velocity *= 0.3f;
                    Main.gore[index2].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
                if (Main.myPlayer == owner)
                {
                    for (int index = 0; index < 1000; ++index)
                    {
                        if (Main.projectile[index].active && Main.projectile[index].type == 618 && Main.projectile[index].ai[1] == (double)whoAmI)
                            Main.projectile[index].Kill();
                    }
                    int num2 = Main.rand.Next(5, 9);
                    int num3 = Main.rand.Next(5, 9);
                    int num4 = Utils.SelectRandom<int>(Main.rand, 86, 90);
                    int num5 = num4 == 86 ? 90 : 86;
                    for (int index = 0; index < num2; ++index)
                    {
                        Vector2 vector2_1 = Center + Utils.RandomVector2(Main.rand, -30f, 30f);
                        Vector2 vector2_2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        while (vector2_2.X == 0.0 && vector2_2.Y == 0.0)
                            vector2_2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        vector2_2.Normalize();
                        if (vector2_2.Y > 0.200000002980232)
                            vector2_2.Y *= -1f;
                        vector2_2 *= Main.rand.Next(70, 101) * 0.1f;
                        NewProjectile(vector2_1.X, vector2_1.Y, vector2_2.X, vector2_2.Y, 620, (int)(damage * 0.8), knockBack * 0.8f, owner, num4, 0.0f);
                    }
                    for (int index = 0; index < num3; ++index)
                    {
                        Vector2 vector2_1 = Center + Utils.RandomVector2(Main.rand, -30f, 30f);
                        Vector2 vector2_2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        while (vector2_2.X == 0.0 && vector2_2.Y == 0.0)
                            vector2_2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        vector2_2.Normalize();
                        if (vector2_2.Y > 0.400000005960464)
                            vector2_2.Y *= -1f;
                        vector2_2 *= Main.rand.Next(40, 81) * 0.1f;
                        NewProjectile(vector2_1.X, vector2_1.Y, vector2_2.X, vector2_2.Y, 620, (int)(damage * 0.8), knockBack * 0.8f, owner, num5, 0.0f);
                    }
                }
            }
            else if (type == 620 || type == 618)
            {
                if (type == 618)
                    ai[0] = 86f;
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, (int)ai[0], velocity.X * 0.1f, velocity.Y * 0.1f, 0, new Color(), 0.5f);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.dust[index2].fadeIn = (float)(0.75 + Main.rand.Next(-10, 11) * 0.00999999977648258);
                        Main.dust[index2].scale = (float)(0.25 + Main.rand.Next(-10, 11) * 0.00499999988824129);
                        ++Main.dust[index2].type;
                    }
                    else
                        Main.dust[index2].scale = (float)(1.0 + Main.rand.Next(-10, 11) * 0.00999999977648258);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 1.25f;
                    Main.dust[index2].velocity -= oldVelocity / 10f;
                }
            }
            else if (type == 619)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 50);
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, (int)ai[0], velocity.X * 0.1f, velocity.Y * 0.1f, 0, new Color(), 0.5f);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.dust[index2].fadeIn = (float)(1.10000002384186 + Main.rand.Next(-10, 11) * 0.00999999977648258);
                        Main.dust[index2].scale = (float)(0.349999994039536 + Main.rand.Next(-10, 11) * 0.00999999977648258);
                        ++Main.dust[index2].type;
                    }
                    else
                        Main.dust[index2].scale = (float)(1.20000004768372 + Main.rand.Next(-10, 11) * 0.00999999977648258);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 2.5f;
                    Main.dust[index2].velocity -= oldVelocity / 10f;
                }
                if (Main.myPlayer == owner)
                {
                    int num2 = Main.rand.Next(3, 6);
                    for (int index = 0; index < num2; ++index)
                    {
                        Vector2 vector2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        while (vector2.X == 0.0 && vector2.Y == 0.0)
                            vector2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        vector2.Normalize();
                        vector2 *= Main.rand.Next(70, 101) * 0.1f;
                        NewProjectile(oldPosition.X + (width / 2), oldPosition.Y + (height / 2), vector2.X, vector2.Y, 620, (int)(damage * 0.8), knockBack * 0.8f, owner, ai[0], 0.0f);
                    }
                }
            }
            if (type == 601)
            {
                Color portalColor = PortalHelper.GetPortalColor(owner, (int)ai[0]);
                portalColor.A = byte.MaxValue;
                for (int index = 0; index < 6; ++index)
                {
                    Vector2 vector2 = Utils.RotatedByRandom(Vector2.UnitY, 6.28318548202515) * (3f * Utils.NextFloat(Main.rand));
                    Dust dust = Main.dust[Dust.NewDust(Center, 0, 0, 263, 0.0f, 0.0f, 0, new Color(), 1f)];
                    dust.position = Center;
                    dust.velocity = vector2 + velocity / 5f;
                    dust.color = portalColor;
                    dust.scale = 2f;
                    dust.noLight = true;
                    dust.noGravity = true;
                }
            }
            if (type == 596)
            {
                position = Center;
                width = height = 60;
                Center = position;
                int num2 = 40;
                if (Main.expertMode)
                    num2 = 30;
                damage = num2;
                Damage();
                Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                for (int index1 = 0; index1 < 4; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                }
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 27, 0.0f, 0.0f, 0, new Color(), 2.5f);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 2f;
                }
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 0, new Color(), 1.5f);
                    Main.dust[index2].position = Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.14159274101257), Utils.ToRotation(velocity), new Vector2()) * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 2f;
                }
            }
            else if (type >= 625 && type <= 628)
            {
                for (int index1 = 0; index1 < 6; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 135, 0.0f, 0.0f, 100, new Color(), 2f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].noLight = true;
                }
            }
            if (type == 631)
            {
                int num2 = Main.rand.Next(5, 10);
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = Dust.NewDust(Center, 0, 0, 229, 0.0f, 0.0f, 100, new Color(), 1f);
                    Main.dust[index2].velocity *= 1.6f;
                    --Main.dust[index2].velocity.Y;
                    Main.dust[index2].position -= Vector2.One * 4f;
                    Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Center, 0.5f);
                    Main.dust[index2].noGravity = true;
                }
            }
            if (type == 539)
            {
                position = Center;
                width = height = 80;
                Center = position;
                Damage();
                Main.PlaySound(4, (int)position.X, (int)position.Y, 7);
                for (int index1 = 0; index1 < 4; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                }
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 176, 0.0f, 0.0f, 200, new Color(), 3.7f);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                }
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 180, 0.0f, 0.0f, 0, new Color(), 2.7f);
                    Main.dust[index2].position = Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.14159274101257), Utils.ToRotation(velocity), new Vector2()) * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                }
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 0, new Color(), 1.5f);
                    Main.dust[index2].position = Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.14159274101257), Utils.ToRotation(velocity), new Vector2()) * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                }
            }
            else if (type == 585)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 27);
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 26, 0.0f, 0.0f, 100, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 1.2f;
                    Main.dust[index2].scale = 1.3f;
                    Main.dust[index2].velocity -= oldVelocity * 0.3f;
                    int index3 = Dust.NewDust(new Vector2(position.X + 4f, position.Y + 4f), width - 8, height - 8, 27, 0.0f, 0.0f, 100, new Color(), 2f);
                    Main.dust[index3].noGravity = true;
                    Main.dust[index3].velocity *= 3f;
                }
            }
            else if (type == 590)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 27);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 165, 0.0f, 0.0f, 50, new Color(), 1.5f);
                    Main.dust[index2].velocity *= 2f;
                    Main.dust[index2].noGravity = true;
                }
                float Scale = (float)(0.600000023841858 + (double)Utils.NextFloat(Main.rand) * 0.400000005960464);
                int index3 = Gore.NewGore(position, Vector2.Zero, 375, Scale);
                Main.gore[index3].velocity *= 0.3f;
                int index4 = Gore.NewGore(position, Vector2.Zero, 376, Scale);
                Main.gore[index4].velocity *= 0.3f;
                int index5 = Gore.NewGore(position, Vector2.Zero, 377, Scale);
                Main.gore[index5].velocity *= 0.3f;
            }
            else if (type == 587)
            {
                Color newColor = Main.hslToRgb(ai[1], 1f, 0.5f);
                newColor.A = (byte)200;
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 76, 0.0f, 0.0f, 0, newColor, 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 1.2f;
                    Main.dust[index2].scale = 0.9f;
                    Main.dust[index2].velocity -= oldVelocity * 0.3f;
                    int index3 = Dust.NewDust(new Vector2(position.X + 4f, position.Y + 4f), width - 8, height - 8, 76, 0.0f, 0.0f, 0, newColor, 1.1f);
                    Main.dust[index3].noGravity = true;
                    Main.dust[index3].velocity *= 2f;
                }
            }
            else if (type == 572)
            {
                for (int index1 = 0; index1 < 15; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 40, velocity.X * 0.1f, velocity.Y * 0.1f, 100, new Color(), 1f);
                    Main.dust[index2].velocity *= 3f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].scale = 1.25f;
                    Main.dust[index2].position = (Center + position) / 2f;
                }
            }
            else if (type == 581)
            {
                for (int index = 0; index < 30; ++index)
                {
                    int Type = Utils.SelectRandom<int>(Main.rand, 229, 229, 161);
                    Dust dust = Main.dust[Dust.NewDust(position, width, height, Type, 0.0f, 0.0f, 0, new Color(), 1f)];
                    dust.noGravity = true;
                    dust.scale = 1.25f + Utils.NextFloat(Main.rand);
                    dust.fadeIn = 0.25f;
                    dust.velocity *= 2f;
                    dust.noLight = true;
                }
            }
            if (type == 405)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 54);
                Vector2 center = Center;
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int num2 = 10;
                    Vector2 vector2_1 = Utils.ToRotationVector2((float)Main.rand.NextDouble() * 6.283185f) * Main.rand.Next(24, 41) / 8f;
                    int index2 = Dust.NewDust(Center - Vector2.One * num2, num2 * 2, num2 * 2, 212, 0.0f, 0.0f, 0, new Color(), 1f);
                    Dust dust = Main.dust[index2];
                    Vector2 vector2_2 = Vector2.Normalize(dust.position - Center);
                    dust.position = Center + vector2_2 * num2 * scale;
                    dust.velocity = index1 >= 30 ? vector2_2 * Main.rand.Next(45, 91) / 10f : vector2_2 * dust.velocity.Length();
                    dust.color = Main.hslToRgb((float)(0.400000005960464 + Main.rand.NextDouble() * 0.200000002980232), 0.9f, 0.5f);
                    dust.color = Color.Lerp(dust.color, Color.White, 0.3f);
                    dust.noGravity = true;
                    dust.scale = 0.7f;
                }
            }
            if (type == 501)
            {
                Main.PlaySound(13, (int)position.X, (int)position.Y, 1);
                int num2 = 20;
                position.X -= num2;
                position.Y -= num2;
                width += num2 * 2;
                height += num2 * 2;
                int num3 = num2 + 20;
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 188, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index2].velocity *= 0.5f;
                }
                for (int index1 = 0; index1 < 5; ++index1)
                {
                    int index2 = Gore.NewGore(new Vector2(position.X + Main.rand.Next(width), position.Y + Main.rand.Next(height)), new Vector2(), Main.rand.Next(435, 438), 1f);
                    Main.gore[index2].velocity *= 0.5f;
                    if (index1 == 0)
                    {
                        ++Main.gore[index2].velocity.X;
                        ++Main.gore[index2].velocity.Y;
                    }
                    else if (index1 == 1)
                    {
                        --Main.gore[index2].velocity.X;
                        ++Main.gore[index2].velocity.Y;
                    }
                    else if (index1 == 2)
                    {
                        ++Main.gore[index2].velocity.X;
                        --Main.gore[index2].velocity.Y;
                    }
                    else
                    {
                        --Main.gore[index2].velocity.X;
                        --Main.gore[index2].velocity.Y;
                    }
                    Main.gore[index2].velocity *= 0.5f;
                }
                position.X -= num3;
                position.Y -= num3;
                width += num3 * 2;
                height += num3 * 2;
                Damage();
            }
            if (type == 410)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 54);
                Vector2 center = Center;
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int num2 = (int)(10.0 * ai[1]);
                    Vector2 vector2_1 = Utils.ToRotationVector2((float)Main.rand.NextDouble() * 6.283185f) * Main.rand.Next(24, 41) / 8f;
                    int index2 = Dust.NewDust(Center - Vector2.One * num2, num2 * 2, num2 * 2, 212, 0.0f, 0.0f, 0, new Color(), 1f);
                    Dust dust = Main.dust[index2];
                    Vector2 vector2_2 = Vector2.Normalize(dust.position - Center);
                    dust.position = Center + vector2_2 * num2 * scale;
                    dust.velocity = index1 >= 30 ? vector2_2 * Main.rand.Next(45, 91) / 10f : vector2_2 * dust.velocity.Length();
                    dust.color = Main.hslToRgb((float)(0.400000005960464 + Main.rand.NextDouble() * 0.200000002980232), 0.9f, 0.5f);
                    dust.color = Color.Lerp(dust.color, Color.White, 0.3f);
                    dust.noGravity = true;
                    dust.scale = 0.7f;
                }
            }
            if (type == 629 && Main.netMode != 1)
            {
                switch (Main.npc[(int)ai[0]].type)
                {
                    case 507:
                        if (NPC.ShieldStrengthTowerNebula != 0)
                            Main.npc[(int)ai[0]].ai[3] = 1f;
                        NPC.ShieldStrengthTowerNebula = (int)MathHelper.Clamp((float)(NPC.ShieldStrengthTowerNebula - 1), 0.0f, (float)NPC.ShieldStrengthTowerMax);
                        break;
                    case 517:
                        if (NPC.ShieldStrengthTowerSolar != 0)
                            Main.npc[(int)ai[0]].ai[3] = 1f;
                        NPC.ShieldStrengthTowerSolar = (int)MathHelper.Clamp((float)(NPC.ShieldStrengthTowerSolar - 1), 0.0f, (float)NPC.ShieldStrengthTowerMax);
                        break;
                    case 422:
                        if (NPC.ShieldStrengthTowerVortex != 0)
                            Main.npc[(int)ai[0]].ai[3] = 1f;
                        NPC.ShieldStrengthTowerVortex = (int)MathHelper.Clamp((float)(NPC.ShieldStrengthTowerVortex - 1), 0.0f, (float)NPC.ShieldStrengthTowerMax);
                        break;
                    case 493:
                        if (NPC.ShieldStrengthTowerStardust != 0)
                            Main.npc[(int)ai[0]].ai[3] = 1f;
                        NPC.ShieldStrengthTowerStardust = (int)MathHelper.Clamp((float)(NPC.ShieldStrengthTowerStardust - 1), 0.0f, (float)NPC.ShieldStrengthTowerMax);
                        break;
                }
                Main.npc[(int)ai[0]].netUpdate = true;
                NetMessage.SendData(101, -1, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
            }
            if (aiStyle == 105 && owner == Main.myPlayer && ai[1] == 0.0)
            {
                Vector2 vector2_1 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                vector2_1.Normalize();
                Vector2 vector2_2 = vector2_1 * 0.3f;
                NewProjectile(Center.X, Center.Y, vector2_2.X, vector2_2.Y, Main.rand.Next(569, 572), damage, 0.0f, owner, 0.0f, 0.0f);
            }
            if (type == 452)
            {
                Main.PlaySound(29, (int)position.X, (int)position.Y, 103);
                position = Center;
                width = height = 144;
                position.X -= (width / 2);
                position.Y -= (height / 2);
                for (int index = 0; index < 4; ++index)
                    Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                for (int index1 = 0; index1 < 40; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 229, 0.0f, 0.0f, 0, new Color(), 2.5f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                    int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 229, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index3].velocity *= 2f;
                    Main.dust[index3].noGravity = true;
                }
                for (int index1 = 0; index1 < 1; ++index1)
                {
                    int index2 = Gore.NewGore(position + new Vector2((width * Main.rand.Next(100)) / 100f, (height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index2].velocity *= 0.3f;
                    Main.gore[index2].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
                Damage();
            }
            if (type == 454)
            {
                Main.PlaySound(4, (int)position.X, (int)position.Y, 6);
                position = Center;
                width = height = 208;
                position.X -= (width / 2);
                position.Y -= (height / 2);
                for (int index1 = 0; index1 < 7; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index2].position = Utils.RotatedBy(new Vector2((width / 2), 0.0f), 6.28318548202515 * (float)Main.rand.NextDouble(), new Vector2()) * (float)Main.rand.NextDouble() + Center;
                }
                for (int index1 = 0; index1 < 60; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 229, 0.0f, 0.0f, 0, new Color(), 2.5f);
                    Main.dust[index2].position = Utils.RotatedBy(new Vector2((width / 2), 0.0f), 6.28318548202515 * (float)Main.rand.NextDouble(), new Vector2()) * (float)Main.rand.NextDouble() + Center;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 1f;
                    int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 229, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index3].position = Utils.RotatedBy(new Vector2((width / 2), 0.0f), 6.28318548202515 * (float)Main.rand.NextDouble(), new Vector2()) * (float)Main.rand.NextDouble() + Center;
                    Main.dust[index3].velocity *= 1f;
                    Main.dust[index3].noGravity = true;
                }
                for (int index1 = 0; index1 < 3; ++index1)
                {
                    int index2 = Gore.NewGore(position + new Vector2((width * Main.rand.Next(100)) / 100f, (height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index2].velocity *= 0.3f;
                    Main.gore[index2].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
                Damage();
            }
            if (type == 467)
            {
                position = Center;
                width = height = 176;
                Center = position;
                Damage();
                Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                for (int index1 = 0; index1 < 4; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                }
                for (int index1 = 0; index1 < 30; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 200, new Color(), 3.7f);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                    int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index3].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.dust[index3].velocity *= 2f;
                    Main.dust[index3].noGravity = true;
                    Main.dust[index3].fadeIn = 2.5f;
                }
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 0, new Color(), 2.7f);
                    Main.dust[index2].position = Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.14159274101257), Utils.ToRotation(velocity), new Vector2()) * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                }
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 0, new Color(), 1.5f);
                    Main.dust[index2].position = Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.14159274101257), Utils.ToRotation(velocity), new Vector2()) * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                }
                for (int index1 = 0; index1 < 2; ++index1)
                {
                    int index2 = Gore.NewGore(position + new Vector2((width * Main.rand.Next(100)) / 100f, (height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.gore[index2].velocity *= 0.3f;
                    Main.gore[index2].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
            }
            if (type == 468)
            {
                position = Center;
                width = height = 176;
                Center = position;
                Damage();
                Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                for (int index1 = 0; index1 < 4; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                }
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 27, 0.0f, 0.0f, 200, new Color(), 3.7f);
                    Main.dust[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                    int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 27, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index3].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.dust[index3].velocity *= 2f;
                    Main.dust[index3].noGravity = true;
                    Main.dust[index3].fadeIn = 2.5f;
                }
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 27, 0.0f, 0.0f, 0, new Color(), 2.7f);
                    Main.dust[index2].position = Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.14159274101257), Utils.ToRotation(velocity), new Vector2()) * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                }
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 0, new Color(), 1.5f);
                    Main.dust[index2].position = Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.14159274101257), Utils.ToRotation(velocity), new Vector2()) * width / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                }
                for (int index1 = 0; index1 < 2; ++index1)
                {
                    int index2 = Gore.NewGore(position + new Vector2((width * Main.rand.Next(100)) / 100f, (height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index2].position = Center + Utils.RotatedByRandom(Vector2.UnitY, 3.14159274101257) * (float)Main.rand.NextDouble() * width / 2f;
                    Main.gore[index2].velocity *= 0.3f;
                    Main.gore[index2].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
            }
            if (type == 485)
            {
                for (int index1 = 0; index1 < 15; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 6, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity -= oldVelocity * Main.rand.Next(20, 60) * 0.01f;
                }
            }
            else if (type == 484)
            {
                for (int index1 = 0; index1 < 5; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 78, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity -= oldVelocity / 5f;
                    Main.dust[index2].scale = 0.85f;
                }
            }
            else if (type == 483)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                if (owner == Main.myPlayer)
                {
                    int length = Main.rand.Next(4, 8);
                    int[] numArray = new int[length];
                    int maxValue = 0;
                    for (int index = 0; index < 200; ++index)
                    {
                        if (Main.npc[index].CanBeChasedBy(this, true) && Collision.CanHitLine(position, width, height, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height))
                        {
                            numArray[maxValue] = index;
                            ++maxValue;
                            if (maxValue == length)
                                break;
                        }
                    }
                    if (maxValue > 1)
                    {
                        for (int index1 = 0; index1 < 100; ++index1)
                        {
                            int index2 = Main.rand.Next(maxValue);
                            int index3 = index2;
                            while (index3 == index2)
                                index3 = Main.rand.Next(maxValue);
                            int num2 = numArray[index2];
                            numArray[index2] = numArray[index3];
                            numArray[index3] = num2;
                        }
                    }
                    Vector2 vector2_1 = new Vector2(-1f, -1f);
                    for (int index = 0; index < maxValue; ++index)
                    {
                        Vector2 vector2_2 = Main.npc[numArray[index]].Center - Center;
                        vector2_2.Normalize();
                        vector2_1 += vector2_2;
                    }
                    vector2_1.Normalize();
                    for (int index = 0; index < length; ++index)
                    {
                        float num2 = Main.rand.Next(8, 15);
                        Vector2 vector2_2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        vector2_2.Normalize();
                        if (maxValue > 0)
                        {
                            vector2_2 += vector2_1;
                            vector2_2.Normalize();
                        }
                        vector2_2 *= num2;
                        if (maxValue > 0)
                        {
                            --maxValue;
                            vector2_2 = Main.npc[numArray[maxValue]].Center - Center;
                            vector2_2.Normalize();
                            vector2_2 *= num2;
                        }
                        NewProjectile(Center.X, Center.Y, vector2_2.X, vector2_2.Y, 484, (int)(damage * 0.7), knockBack * 0.7f, owner, 0.0f, 0.0f);
                    }
                }
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 78, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 4f;
                }
                for (int index1 = 0; index1 < 7; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index2].velocity *= 0.9f;
                    Main.dust[index2].scale = 0.9f;
                }
                for (int index1 = 0; index1 < 3; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2.5f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                    int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index3].velocity *= 2f;
                }
                int index4 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                Main.gore[index4].velocity *= 0.3f;
                Main.gore[index4].velocity.X += Main.rand.Next(-1, 2);
                Main.gore[index4].velocity.Y += Main.rand.Next(-1, 2);
                if (owner == Main.myPlayer)
                {
                    int num2 = 100;
                    position.X -= (float)(num2 / 2);
                    position.Y -= (float)(num2 / 2);
                    width += num2;
                    ++height;
                    penetrate = -1;
                    Damage();
                }
            }
            if (type == 523)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 54);
                for (int index1 = 0; index1 < 25; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 256, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].position = (Main.dust[index2].position + position) / 2f;
                    Main.dust[index2].velocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    Main.dust[index2].velocity.Normalize();
                    Main.dust[index2].velocity *= Main.rand.Next(1, 30) * 0.1f;
                    Main.dust[index2].alpha = alpha;
                }
            }
            else if (type == 522)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 118);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 254, velocity.X * 0.1f, velocity.Y * 0.1f, 0, new Color(), 0.5f);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.dust[index2].fadeIn = (float)(0.75 + Main.rand.Next(-10, 11) * 0.00999999977648258);
                        Main.dust[index2].scale = (float)(0.25 + Main.rand.Next(-10, 11) * 0.00499999988824129);
                        ++Main.dust[index2].type;
                    }
                    else
                        Main.dust[index2].scale = (float)(1.0 + Main.rand.Next(-10, 11) * 0.00999999977648258);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 1.25f;
                    Main.dust[index2].velocity -= oldVelocity / 10f;
                }
            }
            else if (type == 521)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 110);
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 254, velocity.X * 0.1f, velocity.Y * 0.1f, 0, new Color(), 0.5f);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.dust[index2].fadeIn = (float)(1.10000002384186 + Main.rand.Next(-10, 11) * 0.00999999977648258);
                        Main.dust[index2].scale = (float)(0.349999994039536 + Main.rand.Next(-10, 11) * 0.00999999977648258);
                        ++Main.dust[index2].type;
                    }
                    else
                        Main.dust[index2].scale = (float)(1.20000004768372 + Main.rand.Next(-10, 11) * 0.00999999977648258);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 2.5f;
                    Main.dust[index2].velocity -= oldVelocity / 10f;
                }
                if (Main.myPlayer == owner)
                {
                    int num2 = Main.rand.Next(3, 6);
                    for (int index = 0; index < num2; ++index)
                    {
                        Vector2 vector2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        while (vector2.X == 0.0 && vector2.Y == 0.0)
                            vector2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        vector2.Normalize();
                        vector2 *= Main.rand.Next(70, 101) * 0.1f;
                        NewProjectile(oldPosition.X + (width / 2), oldPosition.Y + (height / 2), vector2.X, vector2.Y, 522, (int)(damage * 0.8), knockBack * 0.8f, owner, 0.0f, 0.0f);
                    }
                }
            }
            if (type == 520)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 50);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 252, velocity.X * 0.1f, velocity.Y * 0.1f, 0, new Color(), 0.75f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity -= oldVelocity / 3f;
                }
            }
            if (type == 459)
            {
                int num2 = 3;
                int num3 = 10;
                int num4 = 0;
                if (scale >= 1.0)
                {
                    position = Center;
                    width = height = 144;
                    Center = position;
                    num2 = 7;
                    num3 = 30;
                    num4 = 2;
                    Damage();
                }
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index2].position = Utils.RotatedBy(new Vector2((width / 2), 0.0f), 6.28318548202515 * (float)Main.rand.NextDouble(), new Vector2()) * (float)Main.rand.NextDouble() + Center;
                }
                for (int index1 = 0; index1 < num3; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 226, 0.0f, 0.0f, 0, new Color(), 1.5f);
                    Main.dust[index2].position = Utils.RotatedBy(new Vector2((width / 2), 0.0f), 6.28318548202515 * (float)Main.rand.NextDouble(), new Vector2()) * (float)Main.rand.NextDouble() + Center;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 1f;
                }
                for (int index1 = 0; index1 < num4; ++index1)
                {
                    int index2 = Gore.NewGore(position + new Vector2((width * Main.rand.Next(100)) / 100f, (height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index2].velocity *= 0.3f;
                    Main.gore[index2].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
            }
            if (owner != Main.myPlayer && type == 453 && Main.player[owner].mount.AbilityActive)
                Main.player[owner].mount.UseAbility(Main.player[owner], position, false);
            if (type == 441)
                Main.player[owner].mount.StopAbilityCharge();
            if (type == 444)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 96);
                int num2 = Main.rand.Next(5, 9);
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = Dust.NewDust(Center, 0, 0, 171, 0.0f, 0.0f, 100, new Color(), 1.4f);
                    Main.dust[index2].velocity *= 0.8f;
                    Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Center, 0.5f);
                    Main.dust[index2].noGravity = true;
                }
                if (owner == Main.myPlayer)
                {
                    Vector2 vector2_1 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                    if (Main.player[owner].gravDir == -1.0)
                        vector2_1.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y;
                    Vector2 vector2_2 = Vector2.Normalize(vector2_1 - Center) * localAI[1];
                    NewProjectile(Center.X, Center.Y, vector2_2.X, vector2_2.Y, (int)localAI[0], damage, knockBack, owner, 0.0f, 0.0f);
                }
            }
            if (type == 472)
            {
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 30, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 0.45f;
                    Main.dust[index2].velocity += velocity * 0.9f;
                }
            }
            if (type == 639 || type == 640)
            {
                int num2 = Main.rand.Next(5, 10);
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = Dust.NewDust(Center, 0, 0, 220, 0.0f, 0.0f, 100, new Color(), 0.5f);
                    Main.dust[index2].velocity *= 1.6f;
                    --Main.dust[index2].velocity.Y;
                    Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Center, 0.5f);
                    Main.dust[index2].noGravity = true;
                }
                if (owner == Main.myPlayer && type == 639)
                {
                    int num3 = num1 + 1;
                    int nextSlot = Projectile.GetNextSlot();
                    if (Main.ProjectileUpdateLoopIndex < nextSlot && Main.ProjectileUpdateLoopIndex != -1)
                        ++num3;
                    Vector2 vector2 = new Vector2(ai[0], ai[1]);
                    NewProjectile(localAI[0], localAI[1], vector2.X, vector2.Y, 640, damage, knockBack, owner, 0.0f, num3);
                }
            }
            if (type == 435)
            {
                int num2 = Main.rand.Next(5, 10);
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = Dust.NewDust(Center, 0, 0, 226, 0.0f, 0.0f, 100, new Color(), 0.5f);
                    Main.dust[index2].velocity *= 1.6f;
                    --Main.dust[index2].velocity.Y;
                    Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Center, 0.5f);
                    Main.dust[index2].noGravity = true;
                }
            }
            if (type == 436)
            {
                int num2 = Main.rand.Next(5, 10);
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = Dust.NewDust(Center, 0, 0, 220, 0.0f, 0.0f, 100, new Color(), 0.5f);
                    Main.dust[index2].velocity *= 1.6f;
                    --Main.dust[index2].velocity.Y;
                    Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Center, 0.5f);
                    Main.dust[index2].noGravity = true;
                }
            }
            if (type == 462)
            {
                int num2 = Main.rand.Next(5, 10);
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = Dust.NewDust(Center, 0, 0, 229, 0.0f, 0.0f, 100, new Color(), 0.5f);
                    Main.dust[index2].velocity *= 1.6f;
                    --Main.dust[index2].velocity.Y;
                    Main.dust[index2].position -= Vector2.One * 4f;
                    Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Center, 0.5f);
                    Main.dust[index2].noGravity = true;
                }
            }
            if (type == 442)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 94);
                int num2 = Main.rand.Next(3, 7);
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 135, 0.0f, 0.0f, 100, new Color(), 2.1f);
                    Main.dust[index2].velocity *= 2f;
                    Main.dust[index2].noGravity = true;
                }
                if (Main.myPlayer == owner)
                {
                    Rectangle rectangle = new Rectangle((int)Center.X - 40, (int)Center.Y - 40, 80, 80);
                    for (int index = 0; index < 1000; ++index)
                    {
                        if (index != whoAmI && Main.projectile[index].active && (Main.projectile[index].owner == owner && Main.projectile[index].type == 443) && Main.projectile[index].getRect().Intersects(rectangle))
                        {
                            Main.projectile[index].ai[1] = 1f;
                            Main.projectile[index].velocity = (Center - Main.projectile[index].Center) / 5f;
                            Main.projectile[index].netUpdate = true;
                        }
                    }
                    NewProjectile(Center.X, Center.Y, 0.0f, 0.0f, 443, damage, 0.0f, owner, 0.0f, 0.0f);
                }
            }
            if (type == 440)
            {
                int num2 = Main.rand.Next(3, 7);
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = Dust.NewDust(Center - velocity / 2f, 0, 0, 135, 0.0f, 0.0f, 100, new Color(), 2.1f);
                    Main.dust[index2].velocity *= 2f;
                    Main.dust[index2].noGravity = true;
                }
            }
            if (type == 606)
            {
                int num2 = Main.rand.Next(3, 7);
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = Dust.NewDust(Center - velocity / 2f, 0, 0, 182, 0.0f, 0.0f, 100, new Color(), 1.6f);
                    Main.dust[index2].velocity *= 1.5f;
                    Main.dust[index2].noGravity = true;
                }
            }
            if (type == 449)
            {
                int num2 = Main.rand.Next(3, 7);
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = Dust.NewDust(Center - velocity / 2f, 0, 0, 228, 0.0f, 0.0f, 100, new Color(), 2.1f);
                    Main.dust[index2].velocity *= 2f;
                    Main.dust[index2].noGravity = true;
                }
            }
            if (type == 495)
            {
                for (int index1 = 0; index1 < 15; ++index1)
                {
                    int index2 = Dust.NewDust(Center, 10, 10, 27, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity -= oldVelocity * 0.3f;
                }
            }
            if (type == 497)
            {
                for (int index1 = 0; index1 < 15; ++index1)
                {
                    int index2 = Dust.NewDust(Center, 10, 10, 27, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 2f;
                    Main.dust[index2].velocity -= oldVelocity * 0.3f;
                    Main.dust[index2].scale += Main.rand.Next(150) * (1.0f / 1000.0f);
                }
            }
            if (type == 448)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                position = Center;
                width = height = 112;
                position.X -= (width / 2);
                position.Y -= (height / 2);
                for (int index = 0; index < 4; ++index)
                    Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                for (int index1 = 0; index1 < 40; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 228, 0.0f, 0.0f, 0, new Color(), 2.5f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                    int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 228, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index3].velocity *= 2f;
                    Main.dust[index3].noGravity = true;
                }
                for (int index1 = 0; index1 < 1; ++index1)
                {
                    int index2 = Gore.NewGore(position + new Vector2((width * Main.rand.Next(100)) / 100f, (height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index2].velocity *= 0.3f;
                    Main.gore[index2].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
                Damage();
            }
            if (type == 616)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                position = Center;
                width = height = 80;
                position.X -= (width / 2);
                position.Y -= (height / 2);
                for (int index = 0; index < 4; ++index)
                    Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                for (int index1 = 0; index1 < 40; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 229, 0.0f, 0.0f, 200, new Color(), 2.5f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 2f;
                    int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 229, 0.0f, 0.0f, 200, new Color(), 1.5f);
                    Main.dust[index3].velocity *= 1.2f;
                    Main.dust[index3].noGravity = true;
                }
                for (int index1 = 0; index1 < 1; ++index1)
                {
                    int index2 = Gore.NewGore(position + new Vector2((width * Main.rand.Next(100)) / 100f, (height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index2].velocity *= 0.3f;
                    Main.gore[index2].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
                Damage();
            }
            if (type == 502)
            {
                Vector2 vector2 = new Vector2(width, height) / 2f;
                for (int index1 = 0; index1 < oldPos.Length; ++index1)
                {
                    if (!(oldPos[index1] == Vector2.Zero))
                    {
                        int index2 = Dust.NewDust(oldPos[index1] + vector2, 0, 0, 66, 0.0f, 0.0f, 150, Color.Transparent, 0.7f);
                        Main.dust[index2].color = Main.hslToRgb(Utils.NextFloat(Main.rand), 1f, 0.5f);
                        Main.dust[index2].noGravity = true;
                    }
                }
            }
            if (type == 510)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 107);
                Gore.NewGore(Center, -oldVelocity * 0.2f, 704, 1f);
                Gore.NewGore(Center, -oldVelocity * 0.2f, 705, 1f);
                if (owner == Main.myPlayer)
                {
                    int num2 = Main.rand.Next(20, 31);
                    for (int index = 0; index < num2; ++index)
                    {
                        Vector2 vector2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        vector2.Normalize();
                        vector2 *= Main.rand.Next(10, 201) * 0.01f;
                        NewProjectile(Center.X, Center.Y, vector2.X, vector2.Y, 511 + Main.rand.Next(3), damage, 1f, owner, 0.0f, Main.rand.Next(-45, 1));
                    }
                }
            }
            if (type == 408)
            {
                for (int index1 = 0; index1 < 15; ++index1)
                {
                    int index2 = Dust.NewDust(Center - Vector2.One * 10f, 50, 50, 5, 0.0f, -2f, 0, new Color(), 1f);
                    Main.dust[index2].velocity /= 2f;
                }
                int num2 = 10;
                int index3 = Gore.NewGore(Center, velocity * 0.8f, 584, 1f);
                Main.gore[index3].timeLeft /= num2;
                int index4 = Gore.NewGore(Center, velocity * 0.9f, 585, 1f);
                Main.gore[index4].timeLeft /= num2;
                int index5 = Gore.NewGore(Center, velocity * 1f, 586, 1f);
                Main.gore[index5].timeLeft /= num2;
            }
            if (type == 385)
            {
                Main.PlaySound(4, (int)Center.X, (int)Center.Y, 19);
                int num2 = 36;
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    Vector2 vector2_1 = Utils.RotatedBy(Vector2.Normalize(velocity) * new Vector2(width / 2f, height) * 0.75f, (double)(index1 - (num2 / 2 - 1)) * 6.28318548202515 / num2, new Vector2()) + Center;
                    Vector2 vector2_2 = vector2_1 - Center;
                    int index2 = Dust.NewDust(vector2_1 + vector2_2, 0, 0, 172, vector2_2.X * 2f, vector2_2.Y * 2f, 100, new Color(), 1.4f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].noLight = true;
                    Main.dust[index2].velocity = vector2_2;
                }
                if (owner == Main.myPlayer)
                {
                    if (ai[1] < 1.0)
                    {
                        int index = NewProjectile(Center.X - (direction * 30), Center.Y - 4f, (float)-direction * 0.01f, 0.0f, 384, Main.expertMode ? 25 : 40, 4f, owner, 16f, 15f);
                        Main.projectile[index].netUpdate = true;
                    }
                    else
                    {
                        int num3 = (int)(Center.Y / 16.0);
                        int index1 = (int)(Center.X / 16.0);
                        int num4 = 100;
                        if (index1 < 10)
                            index1 = 10;
                        if (index1 > Main.maxTilesX - 10)
                            index1 = Main.maxTilesX - 10;
                        if (num3 < 10)
                            num3 = 10;
                        if (num3 > Main.maxTilesY - num4 - 10)
                            num3 = Main.maxTilesY - num4 - 10;
                        for (int index2 = num3; index2 < num3 + num4; ++index2)
                        {
                            Tile tile = Main.tile[index1, index2];
                            if (tile.active() && (Main.tileSolid[(int)tile.type] || (int)tile.liquid != 0))
                            {
                                num3 = index2;
                                break;
                            }
                        }
                        int Damage = Main.expertMode ? 50 : 80;
                        int index3 = NewProjectile((float)(index1 * 16 + 8), (float)(num3 * 16 - 24), 0.0f, 0.0f, 386, Damage, 4f, Main.myPlayer, 16f, 24f);
                        Main.projectile[index3].netUpdate = true;
                    }
                }
            }
            else if (type >= 424 && type <= 426)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 89);
                position.X += (width / 2);
                position.Y += (height / 2);
                width = (int)(128.0 * scale);
                height = (int)(128.0 * scale);
                position.X -= (width / 2);
                position.Y -= (height / 2);
                for (int index = 0; index < 8; ++index)
                    Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                for (int index1 = 0; index1 < 32; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2.5f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 3f;
                    int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index3].velocity *= 2f;
                    Main.dust[index3].noGravity = true;
                }
                for (int index1 = 0; index1 < 2; ++index1)
                {
                    int index2 = Gore.NewGore(position + new Vector2((width * Main.rand.Next(100)) / 100f, (height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index2].velocity *= 0.3f;
                    Main.gore[index2].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
                if (owner == Main.myPlayer)
                {
                    localAI[1] = -1f;
                    maxPenetrate = 0;
                    Damage();
                }
                for (int index1 = 0; index1 < 5; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, Utils.SelectRandom<int>(Main.rand, 6, 259, 158), 2.5f * direction, -2.5f, 0, new Color(), 1f);
                    Main.dust[index2].alpha = 200;
                    Main.dust[index2].velocity *= 2.4f;
                    Main.dust[index2].scale += Utils.NextFloat(Main.rand);
                }
            }
            if (type == 399)
            {
                Main.PlaySound(13, (int)position.X, (int)position.Y, 1);
                Vector2 vector2 = new Vector2(20f, 20f);
                for (int index = 0; index < 5; ++index)
                    Dust.NewDust(Center - vector2 / 2f, (int)vector2.X, (int)vector2.Y, 12, 0.0f, 0.0f, 0, Color.Red, 1f);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(Center - vector2 / 2f, (int)vector2.X, (int)vector2.Y, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index2].velocity *= 1.4f;
                }
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(Center - vector2 / 2f, (int)vector2.X, (int)vector2.Y, 6, 0.0f, 0.0f, 100, new Color(), 2.5f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 5f;
                    int index3 = Dust.NewDust(Center - vector2 / 2f, (int)vector2.X, (int)vector2.Y, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index3].velocity *= 3f;
                }
                if (Main.myPlayer == owner)
                {
                    for (int index = 0; index < 6; ++index)
                    {
                        float SpeedX = (float)(-velocity.X * Main.rand.Next(20, 50) * 0.00999999977648258 + Main.rand.Next(-20, 21) * 0.400000005960464);
                        float SpeedY = (float)(-Math.Abs(velocity.Y) * Main.rand.Next(30, 50) * 0.00999999977648258 + Main.rand.Next(-20, 5) * 0.400000005960464);
                        NewProjectile(Center.X + SpeedX, Center.Y + SpeedY, SpeedX, SpeedY, 400 + Main.rand.Next(3), (int)(damage * 0.5), 0.0f, owner, 0.0f, 0.0f);
                    }
                }
            }
            if (type == 384 || type == 386)
            {
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 212, (direction * 2), 0.0f, 100, new Color(), 1.4f);
                    Dust dust = Main.dust[index2];
                    dust.color = Color.CornflowerBlue;
                    dust.color = Color.Lerp(dust.color, Color.White, 0.3f);
                    dust.noGravity = true;
                }
            }
            if (type == 507 || type == 508)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                Vector2 vector2_1 = position;
                Vector2 vector2_2 = oldVelocity;
                vector2_2.Normalize();
                Vector2 Position = vector2_1 + vector2_2 * 16f;
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(Position, width, height, 81, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].position = (Main.dust[index2].position + Center) / 2f;
                    Main.dust[index2].velocity += oldVelocity * 0.4f;
                    Main.dust[index2].velocity *= 0.5f;
                    Main.dust[index2].noGravity = true;
                    Position -= vector2_2 * 8f;
                }
            }
            if (type == 598)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                Vector2 vector2_1 = position;
                Vector2 vector2_2 = Utils.ToRotationVector2(rotation - 1.570796f);
                Vector2 Position = vector2_1 + vector2_2 * 16f;
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(Position, width, height, 81, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].position = (Main.dust[index2].position + Center) / 2f;
                    Main.dust[index2].velocity += vector2_2 * 2f;
                    Main.dust[index2].velocity *= 0.5f;
                    Main.dust[index2].noGravity = true;
                    Position -= vector2_2 * 8f;
                }
            }
            if (type == 1 || type == 81 || type == 98)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index = 0; index < 10; ++index)
                    Dust.NewDust(new Vector2(position.X, position.Y), width, height, 7, 0.0f, 0.0f, 0, new Color(), 1f);
            }
            if (type == 336 || type == 345)
            {
                for (int index1 = 0; index1 < 6; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 196, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].scale = scale;
                }
            }
            if (type == 358)
            {
                velocity = oldVelocity * 0.2f;
                for (int index1 = 0; index1 < 100; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 211, 0.0f, 0.0f, 75, new Color(), 1.2f);
                    if (Main.rand.Next(2) == 0)
                        Main.dust[index2].alpha += 25;
                    if (Main.rand.Next(2) == 0)
                        Main.dust[index2].alpha += 25;
                    if (Main.rand.Next(2) == 0)
                        Main.dust[index2].alpha += 25;
                    if (Main.rand.Next(2) == 0)
                        Main.dust[index2].scale = 0.6f;
                    else
                        Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 0.3f;
                    Main.dust[index2].velocity += velocity;
                    Main.dust[index2].velocity *= (float)(1.0 + Main.rand.Next(-100, 101) * 0.00999999977648258);
                    Main.dust[index2].velocity.X += Main.rand.Next(-50, 51) * 0.015f;
                    Main.dust[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.015f;
                    Main.dust[index2].position = Center;
                }
            }
            if (type == 406)
            {
                int Alpha = 175;
                Color newColor = new Color(0, 80, 255, 100);
                velocity = oldVelocity * 0.2f;
                for (int index1 = 0; index1 < 40; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 4, 0.0f, 0.0f, Alpha, newColor, 1.6f);
                    if (Main.rand.Next(2) == 0)
                        Main.dust[index2].alpha += 25;
                    if (Main.rand.Next(2) == 0)
                        Main.dust[index2].alpha += 25;
                    if (Main.rand.Next(2) == 0)
                        Main.dust[index2].alpha += 25;
                    if (Main.rand.Next(2) == 0)
                        Main.dust[index2].scale = 0.6f;
                    else
                        Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 0.3f;
                    Main.dust[index2].velocity += velocity;
                    Main.dust[index2].velocity *= (float)(1.0 + Main.rand.Next(-100, 101) * 0.00999999977648258);
                    Main.dust[index2].velocity.X += Main.rand.Next(-50, 51) * 0.015f;
                    Main.dust[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.015f;
                    Main.dust[index2].position = Center;
                }
            }
            if (type == 344)
            {
                for (int index1 = 0; index1 < 3; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 197, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].scale = scale;
                }
            }
            else if (type == 343)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 27);
                for (int index1 = 4; index1 < 31; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(oldPosition.X - oldVelocity.X * (30f / index1), oldPosition.Y - oldVelocity.Y * (30f / index1)), 8, 8, 197, oldVelocity.X, oldVelocity.Y, 100, new Color(), 1.2f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 0.5f;
                }
            }
            else if (type == 349)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 27);
                for (int index1 = 0; index1 < 3; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 76, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].noLight = true;
                    Main.dust[index2].scale = 0.7f;
                }
            }
            if (type == 323)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 7, 0.0f, 0.0f, 0, new Color(), 1f);
                    if (Main.rand.Next(2) == 0)
                    {
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].scale = 1.3f;
                        Main.dust[index2].velocity *= 1.5f;
                        Main.dust[index2].velocity -= oldVelocity * 0.5f;
                        Main.dust[index2].velocity *= 1.5f;
                    }
                    else
                    {
                        Main.dust[index2].velocity *= 0.75f;
                        Main.dust[index2].velocity -= oldVelocity * 0.25f;
                        Main.dust[index2].scale = 0.8f;
                    }
                }
            }
            if (type == 589)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 27);
                Color newColor = Color.Red;
                if (ai[1] == 1.0)
                    newColor = Color.Green;
                if (ai[1] == 2.0)
                    newColor = Color.Purple;
                if (ai[1] == 3.0)
                    newColor = Color.Gold;
                if (ai[1] == 4.0)
                    newColor = Color.White;
                newColor.A = (byte)100;
                for (int index1 = 0; index1 < 30; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 11, 0.0f, 0.0f, 0, newColor, 1f);
                    Main.dust[index2].velocity *= (float)(1.0 + (double)Utils.NextFloat(Main.rand) * 1.0);
                    if (index1 < 10)
                    {
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 0.5f;
                    }
                }
            }
            if (type == 346)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 27);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int Type = 10;
                    if (ai[1] == 1.0)
                        Type = 4;
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, Type, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                }
            }
            if (type == 335)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 27);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 90 - (int)ai[1], 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noLight = true;
                    Main.dust[index2].scale = 0.8f;
                }
            }
            if (type == 318)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 30, 0.0f, 0.0f, 0, new Color(), 1f);
                    if (Main.rand.Next(2) == 0)
                        Main.dust[index2].noGravity = true;
                }
            }
            if (type == 378)
            {
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 30, 0.0f, 0.0f, 0, new Color(), 1f);
                    if (Main.rand.Next(2) == 0)
                        Main.dust[index2].noGravity = true;
                }
            }
            else if (type == 311)
            {
                for (int index1 = 0; index1 < 5; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 189, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].scale = 0.85f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity += velocity * 0.5f;
                }
            }
            else if (type == 316)
            {
                for (int index1 = 0; index1 < 5; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 195, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].scale = 0.85f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity += velocity * 0.5f;
                }
            }
            else if (type == 184 || type == 195)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index = 0; index < 5; ++index)
                    Dust.NewDust(new Vector2(position.X, position.Y), width, height, 7, 0.0f, 0.0f, 0, new Color(), 1f);
            }
            else if (type == 275 || type == 276)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index = 0; index < 5; ++index)
                    Dust.NewDust(new Vector2(position.X, position.Y), width, height, 7, 0.0f, 0.0f, 0, new Color(), 1f);
            }
            else if (type == 291)
            {
                if (owner == Main.myPlayer)
                    NewProjectile(Center.X, Center.Y, 0.0f, 0.0f, 292, damage, knockBack, owner, 0.0f, 0.0f);
            }
            else if (type == 295)
            {
                if (owner == Main.myPlayer)
                    NewProjectile(Center.X, Center.Y, 0.0f, 0.0f, 296, (int)(damage * 0.65), knockBack, owner, 0.0f, 0.0f);
            }
            else if (type == 270)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 27);
                if (ai[0] < 0.0)
                {
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(position, width, height, 26, 0.0f, 0.0f, 100, new Color(), 1f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 1.2f;
                        Main.dust[index2].scale = 1.3f;
                        Main.dust[index2].velocity -= oldVelocity * 0.3f;
                        int index3 = Dust.NewDust(new Vector2(position.X + 4f, position.Y + 4f), width - 8, height - 8, 5, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index3].noGravity = true;
                        Main.dust[index3].velocity *= 3f;
                    }
                }
                else
                {
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(position, width, height, 26, 0.0f, 0.0f, 100, new Color(), 1f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 1.2f;
                        Main.dust[index2].scale = 1.3f;
                        Main.dust[index2].velocity -= oldVelocity * 0.3f;
                        int index3 = Dust.NewDust(new Vector2(position.X + 4f, position.Y + 4f), width - 8, height - 8, 6, 0.0f, 0.0f, 100, new Color(), 2f);
                        Main.dust[index3].noGravity = true;
                        Main.dust[index3].velocity *= 3f;
                    }
                }
            }
            else if (type == 265)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 27);
                for (int index1 = 0; index1 < 15; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 163, 0.0f, 0.0f, 100, new Color(), 1.2f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 1.2f;
                    Main.dust[index2].velocity -= oldVelocity * 0.3f;
                }
            }
            else if (type == 355)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 27);
                for (int index1 = 0; index1 < 15; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 205, 0.0f, 0.0f, 100, new Color(), 1.2f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 1.2f;
                    Main.dust[index2].velocity -= oldVelocity * 0.3f;
                }
            }
            else if (type == 304)
            {
                for (int index1 = 0; index1 < 3; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 182, 0.0f, 0.0f, 100, new Color(), 0.8f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 1.2f;
                    Main.dust[index2].velocity -= oldVelocity * 0.3f;
                }
            }
            else if (type == 263)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 27);
                for (int index1 = 0; index1 < 15; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 92, velocity.X, velocity.Y, Main.rand.Next(0, 101), new Color(), (float)(1.0 + Main.rand.Next(40) * 0.00999999977648258));
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 2f;
                }
            }
            else if (type == 261)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index = 0; index < 5; ++index)
                    Dust.NewDust(new Vector2(position.X, position.Y), width, height, 148, 0.0f, 0.0f, 0, new Color(), 1f);
            }
            else if (type == 229)
            {
                for (int index1 = 0; index1 < 25; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 157, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 1.5f;
                    Main.dust[index2].scale = 1.5f;
                }
            }
            else if (type == 239)
            {
                int index = Dust.NewDust(new Vector2(position.X, (float)(position.Y + (double)height - 2.0)), 2, 2, 154, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index].position.X -= 2f;
                Main.dust[index].alpha = 38;
                Main.dust[index].velocity *= 0.1f;
                Main.dust[index].velocity += -oldVelocity * 0.25f;
                Main.dust[index].scale = 0.95f;
            }
            else if (type == 245)
            {
                int index = Dust.NewDust(new Vector2(position.X, (float)(position.Y + (double)height - 2.0)), 2, 2, 114, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index].noGravity = true;
                Main.dust[index].position.X -= 2f;
                Main.dust[index].alpha = 38;
                Main.dust[index].velocity *= 0.1f;
                Main.dust[index].velocity += -oldVelocity * 0.25f;
                Main.dust[index].scale = 0.95f;
            }
            else if (type == 264)
            {
                int index = Dust.NewDust(new Vector2(position.X, (float)(position.Y + (double)height - 2.0)), 2, 2, 54, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index].noGravity = true;
                Main.dust[index].position.X -= 2f;
                Main.dust[index].alpha = 38;
                Main.dust[index].velocity *= 0.1f;
                Main.dust[index].velocity += -oldVelocity * 0.25f;
                Main.dust[index].scale = 0.95f;
            }
            else if (type == 206 || type == 225)
            {
                Main.PlaySound(6, (int)position.X, (int)position.Y, 1);
                for (int index = 0; index < 5; ++index)
                    Dust.NewDust(new Vector2(position.X, position.Y), width, height, 40, 0.0f, 0.0f, 0, new Color(), 1f);
            }
            else if (type == 227)
            {
                Main.PlaySound(6, (int)position.X, (int)position.Y, 1);
                for (int index1 = 0; index1 < 15; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 157, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity += oldVelocity;
                    Main.dust[index2].scale = 1.5f;
                }
            }
            else if (type == 237 && owner == Main.myPlayer)
                NewProjectile(Center.X, Center.Y, 0.0f, 0.0f, 238, damage, knockBack, owner, 0.0f, 0.0f);
            else if (type == 243 && owner == Main.myPlayer)
                NewProjectile(Center.X, Center.Y, 0.0f, 0.0f, 244, damage, knockBack, owner, 0.0f, 0.0f);
            else if (type == 120)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X - velocity.X, position.Y - velocity.Y), width, height, 67, velocity.X, velocity.Y, 100, new Color(), 1f);
                    if (index1 < 5)
                        Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 0.2f;
                }
            }
            else if (type == 181 || type == 189 || type == 566)
            {
                for (int index1 = 0; index1 < 6; ++index1)
                {
                    int index2 = Dust.NewDust(position, width, height, 150, velocity.X, velocity.Y, 50, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].scale = 1f;
                }
            }
            else if (type == 178)
            {
                for (int index1 = 0; index1 < 85; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, Main.rand.Next(139, 143), velocity.X, velocity.Y, 0, new Color(), 1.2f);
                    Main.dust[index2].velocity.X += Main.rand.Next(-50, 51) * 0.01f;
                    Main.dust[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.01f;
                    Main.dust[index2].velocity.X *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                    Main.dust[index2].velocity.Y *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                    Main.dust[index2].velocity.X += Main.rand.Next(-50, 51) * 0.05f;
                    Main.dust[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.05f;
                    Main.dust[index2].scale *= (float)(1.0 + Main.rand.Next(-30, 31) * 0.00999999977648258);
                }
                for (int index1 = 0; index1 < 40; ++index1)
                {
                    int index2 = Gore.NewGore(position, velocity, Main.rand.Next(276, 283), 1f);
                    Main.gore[index2].velocity.X += Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[index2].velocity.X *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                    Main.gore[index2].velocity.Y *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                    Main.gore[index2].scale *= (float)(1.0 + Main.rand.Next(-20, 21) * 0.00999999977648258);
                    Main.gore[index2].velocity.X += Main.rand.Next(-50, 51) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.05f;
                }
            }
            else if (type == 289)
            {
                for (int index1 = 0; index1 < 30; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, Main.rand.Next(139, 143), velocity.X, velocity.Y, 0, new Color(), 1.2f);
                    Main.dust[index2].velocity.X += Main.rand.Next(-50, 51) * 0.01f;
                    Main.dust[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.01f;
                    Main.dust[index2].velocity.X *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                    Main.dust[index2].velocity.Y *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                    Main.dust[index2].velocity.X += Main.rand.Next(-50, 51) * 0.05f;
                    Main.dust[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.05f;
                    Main.dust[index2].scale *= (float)(1.0 + Main.rand.Next(-30, 31) * 0.00999999977648258);
                }
                for (int index1 = 0; index1 < 15; ++index1)
                {
                    int index2 = Gore.NewGore(position, velocity, Main.rand.Next(276, 283), 1f);
                    Main.gore[index2].velocity.X += Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[index2].velocity.X *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                    Main.gore[index2].velocity.Y *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                    Main.gore[index2].scale *= (float)(1.0 + Main.rand.Next(-20, 21) * 0.00999999977648258);
                    Main.gore[index2].velocity.X += Main.rand.Next(-50, 51) * 0.05f;
                    Main.gore[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.05f;
                }
            }
            else if (type == 475 || type == 505 || type == 506)
            {
                if (ai[1] == 0.0)
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                if (ai[1] < 10.0)
                {
                    Vector2 Position = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                    float num2 = -velocity.X;
                    float num3 = -velocity.Y;
                    float num4 = 1f;
                    if (ai[0] <= 17.0)
                        num4 = ai[0] / 17f;
                    int num5 = (int)(30.0 * num4);
                    float num6 = 1f;
                    if (ai[0] <= 30.0)
                        num6 = ai[0] / 30f;
                    float num7 = 0.4f * num6;
                    float num8 = num7;
                    float num9 = num3 + num8;
                    for (int index1 = 0; index1 < num5; ++index1)
                    {
                        float num10 = (float)Math.Sqrt(num2 * num2 + num9 * num9);
                        float num11 = 5.6f;
                        if (Math.Abs(num2) + Math.Abs(num9) < 1.0)
                            num11 *= Math.Abs(num2) + Math.Abs(num9) / 1f;
                        float num12 = num11 / num10;
                        float num13 = num2 * num12;
                        float num14 = num9 * num12;
                        Math.Atan2(num14, num13);
                        int Type = 3;
                        if (type == 506)
                            Type = 30;
                        if (type == 505)
                            Type = 239;
                        if ((double)index1 > ai[1])
                        {
                            for (int index2 = 0; index2 < 4; ++index2)
                            {
                                int index3 = Dust.NewDust(Position, width, height, Type, 0.0f, 0.0f, 0, new Color(), 1f);
                                Main.dust[index3].noGravity = true;
                                Main.dust[index3].velocity *= 0.3f;
                            }
                        }
                        Position.X += num13;
                        Position.Y += num14;
                        num2 = -velocity.X;
                        float num15 = -velocity.Y;
                        num8 += num7;
                        num9 = num15 + num8;
                    }
                }
            }
            else if (type == 171)
            {
                if (ai[1] == 0.0)
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                if (ai[1] < 10.0)
                {
                    Vector2 Position = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                    float num2 = -velocity.X;
                    float num3 = -velocity.Y;
                    float num4 = 1f;
                    if (ai[0] <= 17.0)
                        num4 = ai[0] / 17f;
                    int num5 = (int)(30.0 * num4);
                    float num6 = 1f;
                    if (ai[0] <= 30.0)
                        num6 = ai[0] / 30f;
                    float num7 = 0.4f * num6;
                    float num8 = num7;
                    float num9 = num3 + num8;
                    for (int index1 = 0; index1 < num5; ++index1)
                    {
                        float num10 = (float)Math.Sqrt(num2 * num2 + num9 * num9);
                        float num11 = 5.6f;
                        if (Math.Abs(num2) + Math.Abs(num9) < 1.0)
                            num11 *= Math.Abs(num2) + Math.Abs(num9) / 1f;
                        float num12 = num11 / num10;
                        float num13 = num2 * num12;
                        float num14 = num9 * num12;
                        Math.Atan2(num14, num13);
                        if ((double)index1 > ai[1])
                        {
                            for (int index2 = 0; index2 < 4; ++index2)
                            {
                                int index3 = Dust.NewDust(Position, width, height, 129, 0.0f, 0.0f, 0, new Color(), 1f);
                                Main.dust[index3].noGravity = true;
                                Main.dust[index3].velocity *= 0.3f;
                            }
                        }
                        Position.X += num13;
                        Position.Y += num14;
                        num2 = -velocity.X;
                        float num15 = -velocity.Y;
                        num8 += num7;
                        num9 = num15 + num8;
                    }
                }
            }
            else if (type == 117)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index = 0; index < 10; ++index)
                    Dust.NewDust(new Vector2(position.X, position.Y), width, height, 26, 0.0f, 0.0f, 0, new Color(), 1f);
            }
            else if (type == 166)
            {
                Main.PlaySound(2, (int)position.X, (int)position.Y, 51);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 76, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity -= oldVelocity * 0.25f;
                }
            }
            else if (type == 158)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 9, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity -= velocity * 0.5f;
                }
            }
            else if (type == 159)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 11, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity -= velocity * 0.5f;
                }
            }
            else if (type == 160)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 19, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity -= velocity * 0.5f;
                }
            }
            else if (type == 161)
            {
                Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 11, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity -= velocity * 0.5f;
                }
            }
            else if (type >= 191 && type <= 194)
            {
                int index = Gore.NewGore(new Vector2(position.X - (width / 2), position.Y - (height / 2)), new Vector2(0.0f, 0.0f), Main.rand.Next(61, 64), scale);
                Main.gore[index].velocity *= 0.1f;
            }
            else if (!Main.projPet[type])
            {
                if (type == 93)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index1 = 0; index1 < 10; ++index1)
                    {
                        int index2 = Dust.NewDust(position, width, height, 57, 0.0f, 0.0f, 100, new Color(), 0.5f);
                        Main.dust[index2].velocity.X *= 2f;
                        Main.dust[index2].velocity.Y *= 2f;
                    }
                }
                else if (type == 99)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index1 = 0; index1 < 30; ++index1)
                    {
                        int index2 = Dust.NewDust(position, width, height, 1, 0.0f, 0.0f, 0, new Color(), 1f);
                        if (Main.rand.Next(2) == 0)
                            Main.dust[index2].scale *= 1.4f;
                        Projectile projectile = this;
                        Vector2 vector2 = projectile.velocity * 1.9f;
                        projectile.velocity = vector2;
                    }
                }
                else if (type == 91 || type == 92)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index = 0; index < 10; ++index)
                        Dust.NewDust(position, width, height, 58, velocity.X * 0.1f, velocity.Y * 0.1f, 150, new Color(), 1.2f);
                    for (int index = 0; index < 3; ++index)
                        Gore.NewGore(position, new Vector2(velocity.X * 0.05f, velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
                    if (type == 12 && damage < 500)
                    {
                        for (int index = 0; index < 10; ++index)
                            Dust.NewDust(position, width, height, 57, velocity.X * 0.1f, velocity.Y * 0.1f, 150, new Color(), 1.2f);
                        for (int index = 0; index < 3; ++index)
                            Gore.NewGore(position, new Vector2(velocity.X * 0.05f, velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
                    }
                    if ((type == 91 || type == 92 && ai[0] > 0.0) && owner == Main.myPlayer)
                    {
                        float num2 = position.X + Main.rand.Next(-400, 400);
                        float num3 = position.Y - Main.rand.Next(600, 900);
                        Vector2 vector2 = new Vector2(num2, num3);
                        float num4 = position.X + (width / 2) - vector2.X;
                        float num5 = position.Y + (height / 2) - vector2.Y;
                        float num6 = 22f / (float)Math.Sqrt(num4 * num4 + num5 * num5);
                        float SpeedX = num4 * num6;
                        float SpeedY = num5 * num6;
                        int Damage = damage;
                        int index = NewProjectile(num2, num3, SpeedX, SpeedY, 92, Damage, knockBack, owner, 0.0f, 0.0f);
                        if (type == 91)
                        {
                            Main.projectile[index].ai[1] = position.Y;
                            Main.projectile[index].ai[0] = 1f;
                        }
                        else
                            Main.projectile[index].ai[1] = position.Y;
                    }
                }
                else if (type == 89)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 68, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 1.5f;
                        Main.dust[index2].scale *= 0.9f;
                    }
                    if (type == 89 && owner == Main.myPlayer)
                    {
                        for (int index = 0; index < 3; ++index)
                        {
                            float SpeedX = (float)(-velocity.X * Main.rand.Next(40, 70) * 0.00999999977648258 + Main.rand.Next(-20, 21) * 0.400000005960464);
                            float SpeedY = (float)(-velocity.Y * Main.rand.Next(40, 70) * 0.00999999977648258 + Main.rand.Next(-20, 21) * 0.400000005960464);
                            NewProjectile(position.X + SpeedX, position.Y + SpeedY, SpeedX, SpeedY, 90, (int)(damage * 0.5), 0.0f, owner, 0.0f, 0.0f);
                        }
                    }
                }
                else if (type == 177)
                {
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 137, 0.0f, 0.0f, Main.rand.Next(0, 101), new Color(), (float)(1.0 + Main.rand.Next(-20, 40) * 0.00999999977648258));
                        Main.dust[index2].velocity -= oldVelocity * 0.2f;
                        if (Main.rand.Next(3) == 0)
                        {
                            Main.dust[index2].scale *= 0.8f;
                            Main.dust[index2].velocity *= 0.5f;
                        }
                        else
                            Main.dust[index2].noGravity = true;
                    }
                }
                else if (type == 119 || type == 118 || (type == 128 || type == 359))
                {
                    int num2 = 10;
                    if (type == 119 || type == 359)
                        num2 = 20;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 27);
                    for (int index1 = 0; index1 < num2; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 92, 0.0f, 0.0f, 0, new Color(), 1f);
                        if (Main.rand.Next(3) != 0)
                        {
                            Main.dust[index2].velocity *= 2f;
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].scale *= 1.75f;
                        }
                        else
                            Main.dust[index2].scale *= 0.5f;
                    }
                }
                else if (type == 309)
                {
                    int num2 = 10;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 27);
                    for (int index1 = 0; index1 < num2; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 185, 0.0f, 0.0f, 0, new Color(), 1f);
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dust[index2].velocity *= 2f;
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].scale *= 1.75f;
                        }
                    }
                }
                else if (type == 308)
                {
                    int num2 = 80;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 27);
                    for (int index1 = 0; index1 < num2; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y + 16f), width, height - 16, 185, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].velocity *= 2f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].scale *= 1.15f;
                    }
                }
                else if (aiStyle == 29 && type <= 126)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    int Type = type - 121 + 86;
                    for (int index1 = 0; index1 < 15; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, Type, oldVelocity.X, oldVelocity.Y, 50, new Color(), 1.2f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].scale *= 1.25f;
                        Main.dust[index2].velocity *= 0.5f;
                    }
                }
                else if (type == 597)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index1 = 0; index1 < 15; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 262, oldVelocity.X, oldVelocity.Y, 50, new Color(), 1.2f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].scale *= 1.25f;
                        Main.dust[index2].velocity *= 0.5f;
                    }
                }
                else if (type == 337)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 27);
                    for (int index1 = 0; index1 < 10; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 197, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].noGravity = true;
                    }
                }
                else if (type == 379 || type == 377)
                {
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(position, width, height, 171, 0.0f, 0.0f, 100, new Color(), 1f);
                        Main.dust[index2].scale = Main.rand.Next(1, 10) * 0.1f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].fadeIn = 1.5f;
                        Main.dust[index2].velocity *= 0.75f;
                    }
                }
                else if (type == 80)
                {
                    if (ai[0] >= 0.0)
                    {
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 27);
                        for (int index = 0; index < 10; ++index)
                            Dust.NewDust(new Vector2(position.X, position.Y), width, height, 67, 0.0f, 0.0f, 0, new Color(), 1f);
                    }
                    int i = (int)position.X / 16;
                    int j = (int)position.Y / 16;
                    if (Main.tile[i, j] == null)
                        Main.tile[i, j] = new Tile();
                    if (Main.tile[i, j].type == 127 && Main.tile[i, j].active())
                        WorldGen.KillTile(i, j, false, false, false);
                }
                else if (type == 76 || type == 77 || type == 78)
                {
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(position, width, height, 27, 0.0f, 0.0f, 80, new Color(), 1.5f);
                        Main.dust[index2].noGravity = true;
                    }
                }
                else if (type == 55)
                {
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 18, 0.0f, 0.0f, 0, new Color(), 1.5f);
                        Main.dust[index2].noGravity = true;
                    }
                }
                else if (type == 51 || type == 267)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 5; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 0, 0.0f, 0.0f, 0, new Color(), 0.7f);
                }
                else if (type == 478)
                {
                    if (owner == Main.myPlayer)
                        NewProjectile(Center.X, Center.Y, 0.0f, 0.0f, 480, (int)(damage * 0.8), knockBack * 0.5f, owner, 0.0f, 0.0f);
                }
                else if (type == 477 || type == 479)
                {
                    int num2 = 0;
                    while (num2 < 5)
                        ++num2;
                    Collision.HitTiles(position, velocity, width, height);
                }
                else if (type == 2 || type == 82)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 10; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1f);
                }
                else if (type == 474)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 20; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 26, 0.0f, 0.0f, 0, new Color(), 0.9f);
                }
                else if (type == 172)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 20; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 135, 0.0f, 0.0f, 100, new Color(), 1f);
                }
                else if (type == 103)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 75, 0.0f, 0.0f, 100, new Color(), 1f);
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dust[index2].scale *= 2.5f;
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].velocity *= 5f;
                        }
                    }
                }
                else if (type == 278)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 169, 0.0f, 0.0f, 100, new Color(), 1f);
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dust[index2].scale *= 1.5f;
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].velocity *= 5f;
                        }
                    }
                }
                else if (type == 3 || type == 48 || (type == 54 || type == 599))
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 10; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 1, velocity.X * 0.1f, velocity.Y * 0.1f, 0, new Color(), 0.75f);
                }
                else if (type == 330)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 10; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 0, velocity.X * 0.4f, velocity.Y * 0.4f, 0, new Color(), 0.75f);
                }
                else if (type == 4)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 10; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 14, 0.0f, 0.0f, 150, new Color(), 1.1f);
                }
                else if (type == 5)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index = 0; index < 60; ++index)
                    {
                        int Type;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                Type = 15;
                                break;
                            case 1:
                                Type = 57;
                                break;
                            default:
                                Type = 58;
                                break;
                        }
                        Dust.NewDust(position, width, height, Type, velocity.X * 0.5f, velocity.Y * 0.5f, 150, new Color(), 1.5f);
                    }
                }
                else if (type == 9 || type == 12 || type == 503)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    int num2 = 10;
                    int num3 = 3;
                    if (type == 503)
                    {
                        num2 = 40;
                        num3 = 2;
                        Projectile projectile = this;
                        Vector2 vector2 = projectile.velocity / 2f;
                        projectile.velocity = vector2;
                    }
                    for (int index = 0; index < num2; ++index)
                        Dust.NewDust(position, width, height, 58, velocity.X * 0.1f, velocity.Y * 0.1f, 150, new Color(), 1.2f);
                    for (int index = 0; index < num3; ++index)
                    {
                        int Type = Main.rand.Next(16, 18);
                        if (type == 503)
                            Type = 16;
                        Gore.NewGore(position, new Vector2(velocity.X * 0.05f, velocity.Y * 0.05f), Type, 1f);
                    }
                    if (type == 12 && damage < 100)
                    {
                        for (int index = 0; index < 10; ++index)
                            Dust.NewDust(position, width, height, 57, velocity.X * 0.1f, velocity.Y * 0.1f, 150, new Color(), 1.2f);
                        for (int index = 0; index < 3; ++index)
                            Gore.NewGore(position, new Vector2(velocity.X * 0.05f, velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
                    }
                }
                else if (type == 281)
                {
                    Main.PlaySound(4, (int)position.X, (int)position.Y, 1);
                    int index1 = Gore.NewGore(position, new Vector2(Main.rand.Next(-20, 21) * 0.2f, Main.rand.Next(-20, 21) * 0.2f), 76, 1f);
                    Main.gore[index1].velocity -= velocity * 0.5f;
                    int index2 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(Main.rand.Next(-20, 21) * 0.2f, Main.rand.Next(-20, 21) * 0.2f), 77, 1f);
                    Main.gore[index2].velocity -= velocity * 0.5f;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    for (int index3 = 0; index3 < 20; ++index3)
                    {
                        int index4 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index4].velocity *= 1.4f;
                    }
                    for (int index3 = 0; index3 < 10; ++index3)
                    {
                        int index4 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2.5f);
                        Main.dust[index4].noGravity = true;
                        Main.dust[index4].velocity *= 5f;
                        int index5 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index5].velocity *= 3f;
                    }
                    int index6 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index6].velocity *= 0.4f;
                    ++Main.gore[index6].velocity.X;
                    ++Main.gore[index6].velocity.Y;
                    int index7 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index7].velocity *= 0.4f;
                    --Main.gore[index7].velocity.X;
                    ++Main.gore[index7].velocity.Y;
                    int index8 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index8].velocity *= 0.4f;
                    ++Main.gore[index8].velocity.X;
                    --Main.gore[index8].velocity.Y;
                    int index9 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index9].velocity *= 0.4f;
                    --Main.gore[index9].velocity.X;
                    --Main.gore[index9].velocity.Y;
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 128;
                    height = 128;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                    Damage();
                }
                else if (type == 162)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index2].velocity *= 1.4f;
                    }
                    for (int index1 = 0; index1 < 10; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2.5f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 5f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index3].velocity *= 3f;
                    }
                    int index4 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index4].velocity *= 0.4f;
                    ++Main.gore[index4].velocity.X;
                    ++Main.gore[index4].velocity.Y;
                    int index5 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index5].velocity *= 0.4f;
                    --Main.gore[index5].velocity.X;
                    ++Main.gore[index5].velocity.Y;
                    int index6 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index6].velocity *= 0.4f;
                    ++Main.gore[index6].velocity.X;
                    --Main.gore[index6].velocity.Y;
                    int index7 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index7].velocity *= 0.4f;
                    --Main.gore[index7].velocity.X;
                    --Main.gore[index7].velocity.Y;
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 128;
                    height = 128;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                    Damage();
                }
                else if (type == 240)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index2].velocity *= 1.4f;
                    }
                    for (int index1 = 0; index1 < 10; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2.5f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 5f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index3].velocity *= 3f;
                    }
                    int index4 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index4].velocity *= 0.4f;
                    ++Main.gore[index4].velocity.X;
                    ++Main.gore[index4].velocity.Y;
                    int index5 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index5].velocity *= 0.4f;
                    --Main.gore[index5].velocity.X;
                    ++Main.gore[index5].velocity.Y;
                    int index6 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index6].velocity *= 0.4f;
                    ++Main.gore[index6].velocity.X;
                    --Main.gore[index6].velocity.Y;
                    int index7 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index7].velocity *= 0.4f;
                    --Main.gore[index7].velocity.X;
                    --Main.gore[index7].velocity.Y;
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 96;
                    height = 96;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                    Damage();
                }
                else if (type == 283 || type == 282)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 0; index1 < 10; ++index1)
                    {
                        int index2 = Dust.NewDust(position, width, height, 171, 0.0f, 0.0f, 100, new Color(), 1f);
                        Main.dust[index2].scale = Main.rand.Next(1, 10) * 0.1f;
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].fadeIn = 1.5f;
                        Main.dust[index2].velocity *= 0.75f;
                    }
                }
                else if (type == 284)
                {
                    for (int index1 = 0; index1 < 10; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, Main.rand.Next(139, 143), (float)(-velocity.X * 0.300000011920929), (float)(-velocity.Y * 0.300000011920929), 0, new Color(), 1.2f);
                        Main.dust[index2].velocity.X += Main.rand.Next(-50, 51) * 0.01f;
                        Main.dust[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.01f;
                        Main.dust[index2].velocity.X *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                        Main.dust[index2].velocity.Y *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                        Main.dust[index2].velocity.X += Main.rand.Next(-50, 51) * 0.05f;
                        Main.dust[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.05f;
                        Main.dust[index2].scale *= (float)(1.0 + Main.rand.Next(-30, 31) * 0.00999999977648258);
                    }
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Gore.NewGore(position, -velocity * 0.3f, Main.rand.Next(276, 283), 1f);
                        Main.gore[index2].velocity.X += Main.rand.Next(-50, 51) * 0.01f;
                        Main.gore[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.01f;
                        Main.gore[index2].velocity.X *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                        Main.gore[index2].velocity.Y *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.00999999977648258);
                        Main.gore[index2].scale *= (float)(1.0 + Main.rand.Next(-20, 21) * 0.00999999977648258);
                        Main.gore[index2].velocity.X += Main.rand.Next(-50, 51) * 0.05f;
                        Main.gore[index2].velocity.Y += Main.rand.Next(-50, 51) * 0.05f;
                    }
                }
                else if (type == 286)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    for (int index = 0; index < 7; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    for (int index1 = 0; index1 < 3; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2.5f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 3f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index3].velocity *= 2f;
                    }
                    int index4 = Gore.NewGore(new Vector2(position.X - 10f, position.Y - 10f), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index4].velocity *= 0.3f;
                    Main.gore[index4].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    Main.gore[index4].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                    if (owner == Main.myPlayer)
                    {
                        localAI[1] = -1f;
                        maxPenetrate = 0;
                        position.X += (width / 2);
                        position.Y += (height / 2);
                        width = 80;
                        height = 80;
                        position.X -= (width / 2);
                        position.Y -= (height / 2);
                        Damage();
                    }
                }
                else if (type == 14 || type == 20 || (type == 36 || type == 83) || (type == 84 || type == 389 || (type == 104 || type == 279)) || (type == 100 || type == 110 || (type == 180 || type == 207) || (type == 357 || type == 242 || (type == 302 || type == 257))) || (type == 259 || type == 285 || (type == 287 || type == 576) || type == 577))
                {
                    Collision.HitTiles(position, velocity, width, height);
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                }
                else if (type == 638)
                {
                    Collision.HitTiles(position, velocity, width, height);
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    int num2 = Main.rand.Next(2, 5);
                    for (int index1 = 0; index1 < num2; ++index1)
                    {
                        int index2 = Dust.NewDust(Center, 0, 0, 229, 0.0f, 0.0f, 100, new Color(), 1f);
                        Main.dust[index2].velocity *= 1.6f;
                        --Main.dust[index2].velocity.Y;
                        Main.dust[index2].position -= Vector2.One * 4f;
                        Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Center, 0.5f);
                        Main.dust[index2].noGravity = true;
                    }
                }
                else if (type == 15 || type == 34 || type == 321)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, (float)(-velocity.X * 0.200000002980232), (float)(-velocity.Y * 0.200000002980232), 100, new Color(), 2f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 2f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, (float)(-velocity.X * 0.200000002980232), (float)(-velocity.Y * 0.200000002980232), 100, new Color(), 1f);
                        Main.dust[index3].velocity *= 2f;
                    }
                }
                else if (type == 253)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 135, (float)(-velocity.X * 0.200000002980232), (float)(-velocity.Y * 0.200000002980232), 100, new Color(), 2f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 2f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 135, (float)(-velocity.X * 0.200000002980232), (float)(-velocity.Y * 0.200000002980232), 100, new Color(), 1f);
                        Main.dust[index3].velocity *= 2f;
                    }
                }
                else if (type == 95 || type == 96)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 75, (float)(-velocity.X * 0.200000002980232), (float)(-velocity.Y * 0.200000002980232), 100, new Color(), 2f * scale);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 2f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 75, (float)(-velocity.X * 0.200000002980232), (float)(-velocity.Y * 0.200000002980232), 100, new Color(), 1f * scale);
                        Main.dust[index3].velocity *= 2f;
                    }
                }
                else if (type == 79)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 66, 0.0f, 0.0f, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 2f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 4f;
                    }
                }
                else if (type == 16)
                {
                    if (type == 16 && penetrate == 1)
                    {
                        maxPenetrate = -1;
                        penetrate = -1;
                        int num2 = 60;
                        position.X -= (float)(num2 / 2);
                        position.Y -= (float)(num2 / 2);
                        width += num2;
                        height += num2;
                        tileCollide = false;
                        Projectile projectile = this;
                        Vector2 vector2 = projectile.velocity * 0.01f;
                        projectile.velocity = vector2;
                        Damage();
                        scale = 0.01f;
                    }
                    position.X += (width / 2);
                    width = 10;
                    position.X -= (width / 2);
                    position.Y += (height / 2);
                    height = 10;
                    position.Y -= (height / 2);
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X - velocity.X, position.Y - velocity.Y), width, height, 15, 0.0f, 0.0f, 100, new Color(), 2f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 2f;
                        Dust.NewDust(new Vector2(position.X - velocity.X, position.Y - velocity.Y), width, height, 15, 0.0f, 0.0f, 100, new Color(), 1f);
                    }
                }
                else if (type == 17)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 5; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 0, 0.0f, 0.0f, 0, new Color(), 1f);
                }
                else if (type == 31 || type == 42)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 32, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].velocity *= 0.6f;
                    }
                }
                else if (type >= 411 && type <= 414)
                {
                    int Type = 9;
                    if (type == 412 || type == 414)
                        Type = 11;
                    if (type == 413)
                        Type = 19;
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, Type, 0.0f, velocity.Y / 2f, 0, new Color(), 1f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity -= velocity * 0.5f;
                    }
                }
                else if (type == 109)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 51, 0.0f, 0.0f, 0, new Color(), 0.6f);
                        Main.dust[index2].velocity *= 0.6f;
                    }
                }
                else if (type == 39)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 38, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].velocity *= 0.6f;
                    }
                }
                else if (type == 71)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 53, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].velocity *= 0.6f;
                    }
                }
                else if (type == 40)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 36, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].velocity *= 0.6f;
                    }
                }
                else if (type == 21 || type == 471 || type == 532)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 10; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 26, 0.0f, 0.0f, 0, new Color(), 0.8f);
                }
                else if (type == 583)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 10; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 4, 0.0f, 0.0f, 100, new Color(20, 250, 20, 240), 0.8f);
                }
                else if (type == 584)
                {
                    Main.PlaySound(0, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 10; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 4, 0.0f, 0.0f, 100, new Color(250, 20, 120, 240), 0.8f);
                }
                else if (type == 24)
                {
                    for (int index = 0; index < 10; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 1, velocity.X * 0.1f, velocity.Y * 0.1f, 0, new Color(), 0.75f);
                }
                else if (type == 27)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 0; index1 < 30; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 172, velocity.X * 0.1f, velocity.Y * 0.1f, 100, new Color(), 1f);
                        Main.dust[index2].noGravity = true;
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 172, velocity.X * 0.1f, velocity.Y * 0.1f, 100, new Color(), 0.5f);
                    }
                }
                else if (type == 38)
                {
                    for (int index = 0; index < 10; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 42, velocity.X * 0.1f, velocity.Y * 0.1f, 0, new Color(), 1f);
                }
                else if (type == 44 || type == 45)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 0; index1 < 30; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 27, velocity.X, velocity.Y, 100, new Color(), 1.7f);
                        Main.dust[index2].noGravity = true;
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 27, velocity.X, velocity.Y, 100, new Color(), 1f);
                    }
                }
                else if (type == 41)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    for (int index = 0; index < 10; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2.5f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 3f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index3].velocity *= 2f;
                    }
                    int index4 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index4].velocity *= 0.4f;
                    Main.gore[index4].velocity.X += Main.rand.Next(-10, 11) * 0.1f;
                    Main.gore[index4].velocity.Y += Main.rand.Next(-10, 11) * 0.1f;
                    int index5 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index5].velocity *= 0.4f;
                    Main.gore[index5].velocity.X += Main.rand.Next(-10, 11) * 0.1f;
                    Main.gore[index5].velocity.Y += Main.rand.Next(-10, 11) * 0.1f;
                    if (owner == Main.myPlayer)
                    {
                        penetrate = -1;
                        position.X += (width / 2);
                        position.Y += (height / 2);
                        width = 64;
                        height = 64;
                        position.X -= (width / 2);
                        position.Y -= (height / 2);
                        Damage();
                    }
                }
                else if (type == 514)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    for (int index1 = 0; index1 < 10; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.3f);
                        Main.dust[index2].velocity *= 1.4f;
                    }
                    for (int index1 = 0; index1 < 6; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2.1f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 4.6f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.3f);
                        Main.dust[index3].velocity *= 3.3f;
                        if (Main.rand.Next(2) == 0)
                        {
                            int index4 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.1f);
                            Main.dust[index4].velocity *= 2.7f;
                        }
                    }
                    if (owner == Main.myPlayer)
                    {
                        penetrate = -1;
                        position.X += (width / 2);
                        position.Y += (height / 2);
                        width = 112;
                        height = 112;
                        position.X -= (width / 2);
                        position.Y -= (height / 2);
                        ai[0] = 2f;
                        Damage();
                    }
                }
                else if (type == 306)
                {
                    Main.PlaySound(3, (int)position.X, (int)position.Y, 1);
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 184, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].scale *= 1.1f;
                        Main.dust[index2].noGravity = true;
                    }
                    for (int index1 = 0; index1 < 30; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 184, 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].velocity *= 2.5f;
                        Main.dust[index2].scale *= 0.8f;
                        Main.dust[index2].noGravity = true;
                    }
                    if (owner == Main.myPlayer)
                    {
                        int num2 = 2;
                        if (Main.rand.Next(10) == 0)
                            ++num2;
                        if (Main.rand.Next(10) == 0)
                            ++num2;
                        if (Main.rand.Next(10) == 0)
                            ++num2;
                        for (int index = 0; index < num2; ++index)
                            NewProjectile(position.X, position.Y, Main.rand.Next(-35, 36) * 0.02f * 10f, Main.rand.Next(-35, 36) * 0.02f * 10f, 307, (int)(damage * 0.7), (int)(knockBack * 0.35), Main.myPlayer, 0.0f, 0.0f);
                    }
                }
                else if (type == 469)
                {
                    if (owner == Main.myPlayer)
                    {
                        int num2 = 6;
                        for (int index1 = 0; index1 < num2; ++index1)
                        {
                            if (index1 % 2 != 1 || Main.rand.Next(3) == 0)
                            {
                                Vector2 vector2_1 = position;
                                Vector2 vector2_2 = oldVelocity;
                                vector2_2.Normalize();
                                vector2_2 *= 8f;
                                float num3 = Main.rand.Next(-35, 36) * 0.01f;
                                float num4 = Main.rand.Next(-35, 36) * 0.01f;
                                Vector2 vector2_3 = vector2_1 - vector2_2 * index1;
                                float SpeedX = num3 + oldVelocity.X / 6f;
                                float SpeedY = num4 + oldVelocity.Y / 6f;
                                int index2 = NewProjectile(vector2_3.X, vector2_3.Y, SpeedX, SpeedY, Main.player[owner].beeType(), Main.player[owner].beeDamage(damage / 3), Main.player[owner].beeKB(0.0f), Main.myPlayer, 0.0f, 0.0f);
                                Main.projectile[index2].magic = false;
                                Main.projectile[index2].ranged = true;
                                Main.projectile[index2].penetrate = 2;
                            }
                        }
                    }
                }
                else if (type == 183)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index2].velocity *= 1f;
                    }
                    int index3 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    ++Main.gore[index3].velocity.X;
                    ++Main.gore[index3].velocity.Y;
                    Main.gore[index3].velocity *= 0.3f;
                    int index4 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    --Main.gore[index4].velocity.X;
                    ++Main.gore[index4].velocity.Y;
                    Main.gore[index4].velocity *= 0.3f;
                    int index5 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    ++Main.gore[index5].velocity.X;
                    --Main.gore[index5].velocity.Y;
                    Main.gore[index5].velocity *= 0.3f;
                    int index6 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    --Main.gore[index6].velocity.X;
                    --Main.gore[index6].velocity.Y;
                    Main.gore[index6].velocity *= 0.3f;
                    if (owner == Main.myPlayer)
                    {
                        int num2 = Main.rand.Next(15, 25);
                        for (int index1 = 0; index1 < num2; ++index1)
                            NewProjectile(position.X, position.Y, Main.rand.Next(-35, 36) * 0.02f, Main.rand.Next(-35, 36) * 0.02f, Main.player[owner].beeType(), Main.player[owner].beeDamage(damage), Main.player[owner].beeKB(0.0f), Main.myPlayer, 0.0f, 0.0f);
                    }
                }
                else if (aiStyle == 34)
                {
                    if (owner != Main.myPlayer)
                        timeLeft = 60;
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    if (type == 167)
                    {
                        for (int index1 = 0; index1 < 400; ++index1)
                        {
                            float num2 = 16f;
                            if (index1 < 300)
                                num2 = 12f;
                            if (index1 < 200)
                                num2 = 8f;
                            if (index1 < 100)
                                num2 = 4f;
                            int index2 = Dust.NewDust(new Vector2(position.X, position.Y), 6, 6, 130, 0.0f, 0.0f, 100, new Color(), 1f);
                            float num3 = Main.dust[index2].velocity.X;
                            float num4 = Main.dust[index2].velocity.Y;
                            if (num3 == 0.0 && num4 == 0.0)
                                num3 = 1f;
                            float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
                            float num6 = num2 / num5;
                            float num7 = num3 * num6;
                            float num8 = num4 * num6;
                            Main.dust[index2].velocity *= 0.5f;
                            Main.dust[index2].velocity.X += num7;
                            Main.dust[index2].velocity.Y += num8;
                            Main.dust[index2].scale = 1.3f;
                            Main.dust[index2].noGravity = true;
                        }
                    }
                    if (type == 168)
                    {
                        for (int index1 = 0; index1 < 400; ++index1)
                        {
                            float num2 = (float)(2.0 * ((double)index1 / 100.0));
                            if (index1 > 100)
                                num2 = 10f;
                            if (index1 > 250)
                                num2 = 13f;
                            int index2 = Dust.NewDust(new Vector2(position.X, position.Y), 6, 6, 131, 0.0f, 0.0f, 100, new Color(), 1f);
                            float num3 = Main.dust[index2].velocity.X;
                            float num4 = Main.dust[index2].velocity.Y;
                            if (num3 == 0.0 && num4 == 0.0)
                                num3 = 1f;
                            float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
                            float num6 = num2 / num5;
                            float num7;
                            float num8;
                            if (index1 <= 200)
                            {
                                num7 = num3 * num6;
                                num8 = num4 * num6;
                            }
                            else
                            {
                                num7 = (float)(num3 * num6 * 1.25);
                                num8 = (float)(num4 * num6 * 0.75);
                            }
                            Main.dust[index2].velocity *= 0.5f;
                            Main.dust[index2].velocity.X += num7;
                            Main.dust[index2].velocity.Y += num8;
                            if (index1 > 100)
                            {
                                Main.dust[index2].scale = 1.3f;
                                Main.dust[index2].noGravity = true;
                            }
                        }
                    }
                    if (type == 169)
                    {
                        Vector2 spinningpoint = Utils.ToRotationVector2((float)Main.rand.NextDouble() * 6.283185f);
                        float num2 = Main.rand.Next(5, 9);
                        float num3 = Main.rand.Next(12, 17);
                        float num4 = Main.rand.Next(3, 7);
                        float num5 = 20f;
                        for (float num6 = 0.0f; num6 < num2; ++num6)
                        {
                            for (int index1 = 0; index1 < 2; ++index1)
                            {
                                Vector2 vector2_1 = Utils.RotatedBy(spinningpoint, (index1 == 0 ? 1.0 : -1.0) * 6.28318548202515 / (num2 * 2.0), new Vector2());
                                for (float num7 = 0.0f; num7 < num5; ++num7)
                                {
                                    Vector2 vector2_2 = Vector2.Lerp(spinningpoint, vector2_1, num7 / num5);
                                    float num8 = MathHelper.Lerp(num3, num4, num7 / num5);
                                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), 6, 6, 133, 0.0f, 0.0f, 100, new Color(), 1.3f);
                                    Main.dust[index2].velocity *= 0.1f;
                                    Main.dust[index2].noGravity = true;
                                    Main.dust[index2].velocity += vector2_2 * num8;
                                }
                            }
                            spinningpoint = Utils.RotatedBy(spinningpoint, 6.28318548202515 / num2, new Vector2());
                        }
                        for (float num6 = 0.0f; num6 < num2; ++num6)
                        {
                            for (int index1 = 0; index1 < 2; ++index1)
                            {
                                Vector2 vector2_1 = Utils.RotatedBy(spinningpoint, (index1 == 0 ? 1.0 : -1.0) * 6.28318548202515 / (num2 * 2.0), new Vector2());
                                for (float num7 = 0.0f; num7 < num5; ++num7)
                                {
                                    Vector2 vector2_2 = Vector2.Lerp(spinningpoint, vector2_1, num7 / num5);
                                    float num8 = MathHelper.Lerp(num3, num4, num7 / num5) / 2f;
                                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), 6, 6, 133, 0.0f, 0.0f, 100, new Color(), 1.3f);
                                    Main.dust[index2].velocity *= 0.1f;
                                    Main.dust[index2].noGravity = true;
                                    Main.dust[index2].velocity += vector2_2 * num8;
                                }
                            }
                            spinningpoint = Utils.RotatedBy(spinningpoint, 6.28318548202515 / num2, new Vector2());
                        }
                        for (int index1 = 0; index1 < 100; ++index1)
                        {
                            float num6 = num3;
                            int index2 = Dust.NewDust(new Vector2(position.X, position.Y), 6, 6, 132, 0.0f, 0.0f, 100, new Color(), 1f);
                            float num7 = Main.dust[index2].velocity.X;
                            float num8 = Main.dust[index2].velocity.Y;
                            if (num7 == 0.0 && num8 == 0.0)
                                num7 = 1f;
                            float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
                            float num10 = num6 / num9;
                            float num11 = num7 * num10;
                            float num12 = num8 * num10;
                            Main.dust[index2].velocity *= 0.5f;
                            Main.dust[index2].velocity.X += num11;
                            Main.dust[index2].velocity.Y += num12;
                            Main.dust[index2].scale = 1.3f;
                            Main.dust[index2].noGravity = true;
                        }
                    }
                    if (type == 170)
                    {
                        for (int index1 = 0; index1 < 400; ++index1)
                        {
                            int Type = 133;
                            float num2 = 16f;
                            if (index1 > 100)
                                num2 = 11f;
                            if (index1 > 100)
                                Type = 134;
                            if (index1 > 200)
                                num2 = 8f;
                            if (index1 > 200)
                                Type = 133;
                            if (index1 > 300)
                                num2 = 5f;
                            if (index1 > 300)
                                Type = 134;
                            int index2 = Dust.NewDust(new Vector2(position.X, position.Y), 6, 6, Type, 0.0f, 0.0f, 100, new Color(), 1f);
                            float num3 = Main.dust[index2].velocity.X;
                            float num4 = Main.dust[index2].velocity.Y;
                            if (num3 == 0.0 && num4 == 0.0)
                                num3 = 1f;
                            float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
                            float num6 = num2 / num5;
                            float num7;
                            float num8;
                            if (index1 > 300)
                            {
                                num7 = (float)(num3 * num6 * 0.699999988079071);
                                num8 = num4 * num6;
                            }
                            else if (index1 > 200)
                            {
                                num7 = num3 * num6;
                                num8 = (float)(num4 * num6 * 0.699999988079071);
                            }
                            else if (index1 > 100)
                            {
                                num7 = (float)(num3 * num6 * 0.699999988079071);
                                num8 = num4 * num6;
                            }
                            else
                            {
                                num7 = num3 * num6;
                                num8 = (float)(num4 * num6 * 0.699999988079071);
                            }
                            Main.dust[index2].velocity *= 0.5f;
                            Main.dust[index2].velocity.X += num7;
                            Main.dust[index2].velocity.Y += num8;
                            if (Main.rand.Next(3) != 0)
                            {
                                Main.dust[index2].scale = 1.3f;
                                Main.dust[index2].noGravity = true;
                            }
                        }
                    }
                    if (type == 415)
                    {
                        Vector2 spinningpoint = Utils.ToRotationVector2((float)Main.rand.NextDouble() * 6.283185f);
                        float num2 = Main.rand.Next(5, 9);
                        float num3 = Main.rand.Next(10, 15) * 0.66f;
                        float num4 = Main.rand.Next(4, 7) / 2f;
                        int num5 = 30;
                        for (int index1 = 0; (double)index1 < num5 * num2; ++index1)
                        {
                            if (index1 % num5 == 0)
                                spinningpoint = Utils.RotatedBy(spinningpoint, 6.28318548202515 / num2, new Vector2());
                            float num6 = MathHelper.Lerp(num4, num3, (float)(index1 % num5) / num5);
                            int index2 = Dust.NewDust(new Vector2(position.X, position.Y), 6, 6, 130, 0.0f, 0.0f, 100, new Color(), 1f);
                            Main.dust[index2].velocity *= 0.1f;
                            Main.dust[index2].velocity += spinningpoint * num6;
                            Main.dust[index2].scale = 1.3f;
                            Main.dust[index2].noGravity = true;
                        }
                        for (int index1 = 0; index1 < 100; ++index1)
                        {
                            float num6 = num3;
                            if (index1 < 30)
                                num6 = (float)((num4 + num3) / 2.0);
                            int index2 = Dust.NewDust(new Vector2(position.X, position.Y), 6, 6, 130, 0.0f, 0.0f, 100, new Color(), 1f);
                            float num7 = Main.dust[index2].velocity.X;
                            float num8 = Main.dust[index2].velocity.Y;
                            if (num7 == 0.0 && num8 == 0.0)
                                num7 = 1f;
                            float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
                            float num10 = num6 / num9;
                            float num11 = num7 * num10;
                            float num12 = num8 * num10;
                            Main.dust[index2].velocity *= 0.5f;
                            Main.dust[index2].velocity.X += num11;
                            Main.dust[index2].velocity.Y += num12;
                            Main.dust[index2].scale = 1.3f;
                            Main.dust[index2].noGravity = true;
                        }
                    }
                    if (type == 416)
                    {
                        Vector2 spinningpoint1 = Utils.ToRotationVector2((float)Main.rand.NextDouble() * 6.283185f);
                        Vector2 spinningpoint2 = spinningpoint1;
                        float num2 = (float)(Main.rand.Next(3, 6) * 2);
                        int num3 = 20;
                        float num4 = Main.rand.Next(2) == 0 ? 1f : -1f;
                        bool flag = true;
                        for (int index1 = 0; (double)index1 < num3 * num2; ++index1)
                        {
                            if (index1 % num3 == 0)
                            {
                                spinningpoint2 = Utils.RotatedBy(spinningpoint2, num4 * (6.28318548202515 / num2), new Vector2());
                                spinningpoint1 = spinningpoint2;
                                flag = !flag;
                            }
                            else
                            {
                                float num5 = 6.283185f / (num3 * num2);
                                spinningpoint1 = Utils.RotatedBy(spinningpoint1, num5 * num4 * 3.0, new Vector2());
                            }
                            float num6 = MathHelper.Lerp(1f, 8f, (float)(index1 % num3) / num3);
                            int index2 = Dust.NewDust(new Vector2(position.X, position.Y), 6, 6, 131, 0.0f, 0.0f, 100, new Color(), 1.4f);
                            Main.dust[index2].velocity *= 0.1f;
                            Main.dust[index2].velocity += spinningpoint1 * num6;
                            if (flag)
                                Main.dust[index2].scale = 0.9f;
                            Main.dust[index2].noGravity = true;
                        }
                    }
                    if (type == 417)
                    {
                        float num2 = (float)Main.rand.NextDouble() * 6.283185f;
                        float num3 = (float)Main.rand.NextDouble() * 6.283185f;
                        float num4 = (float)(4.0 + Main.rand.NextDouble() * 3.0);
                        float num5 = (float)(4.0 + Main.rand.NextDouble() * 3.0);
                        float num6 = num4;
                        if (num5 > num6)
                            num6 = num5;
                        for (int index1 = 0; index1 < 150; ++index1)
                        {
                            int Type = 132;
                            float num7 = num6;
                            if (index1 > 50)
                                num7 = num5;
                            if (index1 > 50)
                                Type = 133;
                            if (index1 > 100)
                                num7 = num4;
                            if (index1 > 100)
                                Type = 132;
                            int index2 = Dust.NewDust(new Vector2(position.X, position.Y), 6, 6, Type, 0.0f, 0.0f, 100, new Color(), 1f);
                            Vector2 vector2 = Main.dust[index2].velocity;
                            vector2.Normalize();
                            Vector2 spinningpoint = vector2 * num7;
                            if (index1 > 100)
                            {
                                spinningpoint.X *= 0.5f;
                                spinningpoint = Utils.RotatedBy(spinningpoint, num2, new Vector2());
                            }
                            else if (index1 > 50)
                            {
                                spinningpoint.Y *= 0.5f;
                                spinningpoint = Utils.RotatedBy(spinningpoint, num3, new Vector2());
                            }
                            Main.dust[index2].velocity *= 0.2f;
                            Main.dust[index2].velocity += spinningpoint;
                            if (index1 <= 200)
                            {
                                Main.dust[index2].scale = 1.3f;
                                Main.dust[index2].noGravity = true;
                            }
                        }
                    }
                    if (type == 418)
                    {
                        Vector2 spinningpoint = Utils.ToRotationVector2((float)Main.rand.NextDouble() * 6.283185f);
                        float num2 = Main.rand.Next(5, 12);
                        float num3 = Main.rand.Next(9, 14) * 0.66f;
                        float num4 = Main.rand.Next(2, 4) * 0.66f;
                        float num5 = 15f;
                        for (float num6 = 0.0f; num6 < num2; ++num6)
                        {
                            for (int index1 = 0; index1 < 2; ++index1)
                            {
                                Vector2 vector2_1 = Utils.RotatedBy(spinningpoint, (index1 == 0 ? 1.0 : -1.0) * 6.28318548202515 / (num2 * 2.0), new Vector2());
                                for (float num7 = 0.0f; num7 < num5; ++num7)
                                {
                                    Vector2 vector2_2 = Vector2.SmoothStep(spinningpoint, vector2_1, num7 / num5);
                                    float num8 = MathHelper.SmoothStep(num3, num4, num7 / num5);
                                    int index2 = Dust.NewDust(new Vector2(position.X, position.Y), 6, 6, 134, 0.0f, 0.0f, 100, new Color(), 1.3f);
                                    Main.dust[index2].velocity *= 0.1f;
                                    Main.dust[index2].noGravity = true;
                                    Main.dust[index2].velocity += vector2_2 * num8;
                                }
                            }
                            spinningpoint = Utils.RotatedBy(spinningpoint, 6.28318548202515 / num2, new Vector2());
                        }
                        for (int index1 = 0; index1 < 120; ++index1)
                        {
                            float num6 = num3;
                            int Type = 133;
                            if (index1 < 80)
                                num6 = num4 - 0.5f;
                            else
                                Type = 131;
                            int index2 = Dust.NewDust(new Vector2(position.X, position.Y), 6, 6, Type, 0.0f, 0.0f, 100, new Color(), 1f);
                            float num7 = Main.dust[index2].velocity.X;
                            float num8 = Main.dust[index2].velocity.Y;
                            if (num7 == 0.0 && num8 == 0.0)
                                num7 = 1f;
                            float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
                            float num10 = num6 / num9;
                            float num11 = num7 * num10;
                            float num12 = num8 * num10;
                            Main.dust[index2].velocity *= 0.2f;
                            Main.dust[index2].velocity.X += num11;
                            Main.dust[index2].velocity.Y += num12;
                            Main.dust[index2].scale = 1.3f;
                            Main.dust[index2].noGravity = true;
                        }
                    }
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 192;
                    height = 192;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                    penetrate = -1;
                    Damage();
                }
                else if (type == 312)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 22;
                    height = 22;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                    for (int index1 = 0; index1 < 30; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index2].velocity *= 1.4f;
                    }
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 3.5f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 7f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index3].velocity *= 3f;
                    }
                    for (int index1 = 0; index1 < 2; ++index1)
                    {
                        float num2 = 0.4f;
                        if (index1 == 1)
                            num2 = 0.8f;
                        int index2 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index2].velocity *= num2;
                        ++Main.gore[index2].velocity.X;
                        ++Main.gore[index2].velocity.Y;
                        int index3 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index3].velocity *= num2;
                        --Main.gore[index3].velocity.X;
                        ++Main.gore[index3].velocity.Y;
                        int index4 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index4].velocity *= num2;
                        ++Main.gore[index4].velocity.X;
                        --Main.gore[index4].velocity.Y;
                        int index5 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index5].velocity *= num2;
                        --Main.gore[index5].velocity.X;
                        --Main.gore[index5].velocity.Y;
                    }
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 128;
                    height = 128;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                    Damage();
                }
                else if (type == 133 || type == 134 || (type == 135 || type == 136) || (type == 137 || type == 138 || (type == 303 || type == 338)) || type == 339)
                {
                    if (type == 30 || type == 133 || (type == 136 || type == 139))
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 62);
                    else
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 22;
                    height = 22;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                    for (int index1 = 0; index1 < 30; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index2].velocity *= 1.4f;
                    }
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 3.5f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 7f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index3].velocity *= 3f;
                    }
                    for (int index1 = 0; index1 < 2; ++index1)
                    {
                        float num2 = 0.4f;
                        if (index1 == 1)
                            num2 = 0.8f;
                        int index2 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index2].velocity *= num2;
                        ++Main.gore[index2].velocity.X;
                        ++Main.gore[index2].velocity.Y;
                        int index3 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index3].velocity *= num2;
                        --Main.gore[index3].velocity.X;
                        ++Main.gore[index3].velocity.Y;
                        int index4 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index4].velocity *= num2;
                        ++Main.gore[index4].velocity.X;
                        --Main.gore[index4].velocity.Y;
                        int index5 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index5].velocity *= num2;
                        --Main.gore[index5].velocity.X;
                        --Main.gore[index5].velocity.Y;
                    }
                }
                else if (type == 139 || type == 140 || (type == 141 || type == 142) || (type == 143 || type == 144 || (type == 340 || type == 341)))
                {
                    if (type == 30 || type == 133 || (type == 136 || type == 139))
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 62);
                    else
                        Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 80;
                    height = 80;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                    for (int index1 = 0; index1 < 40; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 2f);
                        Main.dust[index2].velocity *= 3f;
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dust[index2].scale = 0.5f;
                            Main.dust[index2].fadeIn = (float)(1.0 + Main.rand.Next(10) * 0.100000001490116);
                        }
                    }
                    for (int index1 = 0; index1 < 70; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 3f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 5f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2f);
                        Main.dust[index3].velocity *= 2f;
                    }
                    for (int index1 = 0; index1 < 3; ++index1)
                    {
                        float num2 = 0.33f;
                        if (index1 == 1)
                            num2 = 0.66f;
                        if (index1 == 2)
                            num2 = 1f;
                        int index2 = Gore.NewGore(new Vector2((float)(position.X + (width / 2) - 24.0), (float)(position.Y + (height / 2) - 24.0)), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index2].velocity *= num2;
                        ++Main.gore[index2].velocity.X;
                        ++Main.gore[index2].velocity.Y;
                        int index3 = Gore.NewGore(new Vector2((float)(position.X + (width / 2) - 24.0), (float)(position.Y + (height / 2) - 24.0)), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index3].velocity *= num2;
                        --Main.gore[index3].velocity.X;
                        ++Main.gore[index3].velocity.Y;
                        int index4 = Gore.NewGore(new Vector2((float)(position.X + (width / 2) - 24.0), (float)(position.Y + (height / 2) - 24.0)), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index4].velocity *= num2;
                        ++Main.gore[index4].velocity.X;
                        --Main.gore[index4].velocity.Y;
                        int index5 = Gore.NewGore(new Vector2((float)(position.X + (width / 2) - 24.0), (float)(position.Y + (height / 2) - 24.0)), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index5].velocity *= num2;
                        --Main.gore[index5].velocity.X;
                        --Main.gore[index5].velocity.Y;
                    }
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 10;
                    height = 10;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                }
                else if (type == 246)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    for (int index1 = 0; index1 < 10; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index2].velocity *= 0.9f;
                    }
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2.5f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 3f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index3].velocity *= 2f;
                    }
                    int index4 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index4].velocity *= 0.3f;
                    Main.gore[index4].velocity.X += Main.rand.Next(-1, 2);
                    Main.gore[index4].velocity.Y += Main.rand.Next(-1, 2);
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 150;
                    height = 150;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                    penetrate = -1;
                    maxPenetrate = 0;
                    Damage();
                    if (owner == Main.myPlayer)
                    {
                        int num2 = Main.rand.Next(2, 6);
                        for (int index1 = 0; index1 < num2; ++index1)
                        {
                            float num3 = Main.rand.Next(-100, 101) + 0.01f;
                            float num4 = Main.rand.Next(-100, 101);
                            float num5 = num3 - 0.01f;
                            float num6 = 8f / (float)Math.Sqrt(num5 * num5 + num4 * num4);
                            int index2 = NewProjectile(Center.X - oldVelocity.X, Center.Y - oldVelocity.Y, num5 * num6, num4 * num6, 249, damage, knockBack, owner, 0.0f, 0.0f);
                            Main.projectile[index2].maxPenetrate = 0;
                        }
                    }
                }
                else if (type == 249)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    for (int index1 = 0; index1 < 7; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index2].velocity *= 0.8f;
                    }
                    for (int index1 = 0; index1 < 2; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2.5f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 2.5f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index3].velocity *= 1.5f;
                    }
                    int index = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index].velocity *= 0.2f;
                    Main.gore[index].velocity.X += Main.rand.Next(-1, 2);
                    Main.gore[index].velocity.Y += Main.rand.Next(-1, 2);
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 100;
                    height = 100;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                    penetrate = -1;
                    Damage();
                }
                else if (type == 588)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    position = Center;
                    width = height = 22;
                    Center = position;
                    for (int index1 = 0; index1 < 8; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 219 + Main.rand.Next(5), 0.0f, 0.0f, 0, new Color(), 1f);
                        Main.dust[index2].velocity *= 1.4f;
                        Main.dust[index2].fadeIn = 1f;
                        Main.dust[index2].noGravity = true;
                    }
                    for (int index1 = 0; index1 < 15; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 139 + Main.rand.Next(4), 0.0f, 0.0f, 0, new Color(), 1.6f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 5f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 139 + Main.rand.Next(4), 0.0f, 0.0f, 0, new Color(), 1.9f);
                        Main.dust[index3].velocity *= 3f;
                    }
                    if (Main.rand.Next(2) == 0)
                    {
                        int index = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(276, 283), 1f);
                        Main.gore[index].velocity *= 0.4f;
                        ++Main.gore[index].velocity.X;
                        ++Main.gore[index].velocity.Y;
                    }
                    if (Main.rand.Next(2) == 0)
                    {
                        int index = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(276, 283), 1f);
                        Main.gore[index].velocity *= 0.4f;
                        --Main.gore[index].velocity.X;
                        ++Main.gore[index].velocity.Y;
                    }
                    if (Main.rand.Next(2) == 0)
                    {
                        int index = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(276, 283), 1f);
                        Main.gore[index].velocity *= 0.4f;
                        ++Main.gore[index].velocity.X;
                        --Main.gore[index].velocity.Y;
                    }
                    if (Main.rand.Next(2) == 0)
                    {
                        int index = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(276, 283), 1f);
                        Main.gore[index].velocity *= 0.4f;
                        --Main.gore[index].velocity.X;
                        --Main.gore[index].velocity.Y;
                    }
                }
                else if (type == 28 || type == 30 || (type == 37 || type == 75) || (type == 102 || type == 164 || (type == 397 || type == 517)) || (type == 516 || type == 519))
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 22;
                    height = 22;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                    for (int index1 = 0; index1 < 20; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index2].velocity *= 1.4f;
                    }
                    for (int index1 = 0; index1 < 10; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2.5f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 5f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 1.5f);
                        Main.dust[index3].velocity *= 3f;
                    }
                    int index4 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index4].velocity *= 0.4f;
                    ++Main.gore[index4].velocity.X;
                    ++Main.gore[index4].velocity.Y;
                    int index5 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index5].velocity *= 0.4f;
                    --Main.gore[index5].velocity.X;
                    ++Main.gore[index5].velocity.Y;
                    int index6 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index6].velocity *= 0.4f;
                    ++Main.gore[index6].velocity.X;
                    --Main.gore[index6].velocity.Y;
                    int index7 = Gore.NewGore(new Vector2(position.X, position.Y), new Vector2(), Main.rand.Next(61, 64), 1f);
                    Main.gore[index7].velocity *= 0.4f;
                    --Main.gore[index7].velocity.X;
                    --Main.gore[index7].velocity.Y;
                    if (type == 102)
                    {
                        position.X += (width / 2);
                        position.Y += (height / 2);
                        width = 128;
                        height = 128;
                        position.X -= (width / 2);
                        position.Y -= (height / 2);
                        damage = 40;
                        Damage();
                    }
                }
                else if (type == 29 || type == 108 || (type == 470 || type == 637))
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
                    if (type == 29)
                    {
                        position.X += (width / 2);
                        position.Y += (height / 2);
                        width = 200;
                        height = 200;
                        position.X -= (width / 2);
                        position.Y -= (height / 2);
                    }
                    for (int index1 = 0; index1 < 50; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 31, 0.0f, 0.0f, 100, new Color(), 2f);
                        Main.dust[index2].velocity *= 1.4f;
                    }
                    for (int index1 = 0; index1 < 80; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 3f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 5f;
                        int index3 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 6, 0.0f, 0.0f, 100, new Color(), 2f);
                        Main.dust[index3].velocity *= 3f;
                    }
                    for (int index1 = 0; index1 < 2; ++index1)
                    {
                        int index2 = Gore.NewGore(new Vector2((float)(position.X + (width / 2) - 24.0), (float)(position.Y + (height / 2) - 24.0)), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index2].scale = 1.5f;
                        Main.gore[index2].velocity.X += 1.5f;
                        Main.gore[index2].velocity.Y += 1.5f;
                        int index3 = Gore.NewGore(new Vector2((float)(position.X + (width / 2) - 24.0), (float)(position.Y + (height / 2) - 24.0)), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index3].scale = 1.5f;
                        Main.gore[index3].velocity.X -= 1.5f;
                        Main.gore[index3].velocity.Y += 1.5f;
                        int index4 = Gore.NewGore(new Vector2((float)(position.X + (width / 2) - 24.0), (float)(position.Y + (height / 2) - 24.0)), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index4].scale = 1.5f;
                        Main.gore[index4].velocity.X += 1.5f;
                        Main.gore[index4].velocity.Y -= 1.5f;
                        int index5 = Gore.NewGore(new Vector2((float)(position.X + (width / 2) - 24.0), (float)(position.Y + (height / 2) - 24.0)), new Vector2(), Main.rand.Next(61, 64), 1f);
                        Main.gore[index5].scale = 1.5f;
                        Main.gore[index5].velocity.X -= 1.5f;
                        Main.gore[index5].velocity.Y -= 1.5f;
                    }
                    position.X += (width / 2);
                    position.Y += (height / 2);
                    width = 10;
                    height = 10;
                    position.X -= (width / 2);
                    position.Y -= (height / 2);
                }
                else if (type == 69)
                {
                    Main.PlaySound(13, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 5; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 13, 0.0f, 0.0f, 0, new Color(), 1f);
                    for (int index1 = 0; index1 < 30; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 33, 0.0f, -2f, 0, new Color(), 1.1f);
                        Main.dust[index2].alpha = 100;
                        Main.dust[index2].velocity.X *= 1.5f;
                        Main.dust[index2].velocity *= 3f;
                    }
                }
                else if (type == 70)
                {
                    Main.PlaySound(13, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 5; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 13, 0.0f, 0.0f, 0, new Color(), 1f);
                    for (int index1 = 0; index1 < 30; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 52, 0.0f, -2f, 0, new Color(), 1.1f);
                        Main.dust[index2].alpha = 100;
                        Main.dust[index2].velocity.X *= 1.5f;
                        Main.dust[index2].velocity *= 3f;
                    }
                }
                else if (type == 621)
                {
                    Main.PlaySound(13, (int)position.X, (int)position.Y, 1);
                    for (int index = 0; index < 5; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 13, 0.0f, 0.0f, 0, new Color(), 1f);
                    for (int index1 = 0; index1 < 30; ++index1)
                    {
                        int index2 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 266, 0.0f, -2f, 0, new Color(), 1.1f);
                        Main.dust[index2].alpha = 100;
                        Main.dust[index2].velocity.X *= 1.5f;
                        Main.dust[index2].velocity *= 3f;
                    }
                }
                else if (type == 114 || type == 115)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 4; index1 < 31; ++index1)
                    {
                        float num2 = oldVelocity.X * (30f / index1);
                        float num3 = oldVelocity.Y * (30f / index1);
                        int index2 = Dust.NewDust(new Vector2(position.X - num2, position.Y - num3), 8, 8, 27, oldVelocity.X, oldVelocity.Y, 100, new Color(), 1.4f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 0.5f;
                        int index3 = Dust.NewDust(new Vector2(position.X - num2, position.Y - num3), 8, 8, 27, oldVelocity.X, oldVelocity.Y, 100, new Color(), 0.9f);
                        Main.dust[index3].velocity *= 0.5f;
                    }
                }
                else if (type == 116)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 4; index1 < 31; ++index1)
                    {
                        float num2 = oldVelocity.X * (30f / index1);
                        float num3 = oldVelocity.Y * (30f / index1);
                        int index2 = Dust.NewDust(new Vector2(position.X - num2, position.Y - num3), 8, 8, 64, oldVelocity.X, oldVelocity.Y, 100, new Color(), 1.8f);
                        Main.dust[index2].noGravity = true;
                        int index3 = Dust.NewDust(new Vector2(position.X - num2, position.Y - num3), 8, 8, 64, oldVelocity.X, oldVelocity.Y, 100, new Color(), 1.4f);
                        Main.dust[index3].noGravity = true;
                    }
                }
                else if (type == 173)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 4; index1 < 24; ++index1)
                    {
                        float num2 = oldVelocity.X * (30f / index1);
                        float num3 = oldVelocity.Y * (30f / index1);
                        int Type;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                Type = 15;
                                break;
                            case 1:
                                Type = 57;
                                break;
                            default:
                                Type = 58;
                                break;
                        }
                        int index2 = Dust.NewDust(new Vector2(position.X - num2, position.Y - num3), 8, 8, Type, oldVelocity.X * 0.2f, oldVelocity.Y * 0.2f, 100, new Color(), 1.8f);
                        Main.dust[index2].velocity *= 1.5f;
                        Main.dust[index2].noGravity = true;
                    }
                }
                else if (type == 132)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 4; index1 < 31; ++index1)
                    {
                        float num2 = oldVelocity.X * (30f / index1);
                        float num3 = oldVelocity.Y * (30f / index1);
                        int index2 = Dust.NewDust(new Vector2(oldPosition.X - num2, oldPosition.Y - num3), 8, 8, 107, oldVelocity.X, oldVelocity.Y, 100, new Color(), 1.8f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 0.5f;
                        int index3 = Dust.NewDust(new Vector2(oldPosition.X - num2, oldPosition.Y - num3), 8, 8, 107, oldVelocity.X, oldVelocity.Y, 100, new Color(), 1.4f);
                        Main.dust[index3].velocity *= 0.05f;
                    }
                }
                else if (type == 156)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 4; index1 < 31; ++index1)
                    {
                        float num2 = oldVelocity.X * (30f / index1);
                        float num3 = oldVelocity.Y * (30f / index1);
                        int index2 = Dust.NewDust(new Vector2(oldPosition.X - num2, oldPosition.Y - num3), 8, 8, 73, oldVelocity.X, oldVelocity.Y, 255, new Color(), 1.8f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 0.5f;
                        int index3 = Dust.NewDust(new Vector2(oldPosition.X - num2, oldPosition.Y - num3), 8, 8, 73, oldVelocity.X, oldVelocity.Y, 255, new Color(), 1.4f);
                        Main.dust[index3].velocity *= 0.05f;
                        Main.dust[index3].noGravity = true;
                    }
                }
                else if (type == 157)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 10);
                    for (int index1 = 4; index1 < 31; ++index1)
                    {
                        int index2 = Dust.NewDust(position, width, height, 107, oldVelocity.X, oldVelocity.Y, 100, new Color(), 1.8f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].velocity *= 0.5f;
                    }
                }
                else if (type == 370)
                {
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 4);
                    for (int index = 0; index < 5; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 13, 0.0f, 0.0f, 0, new Color(), 1f);
                    for (int index1 = 0; index1 < 30; ++index1)
                    {
                        Vector2 vector2 = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                        vector2.Normalize();
                        int index2 = Gore.NewGore(Center + vector2 * 10f, vector2 * Main.rand.Next(4, 9) * 0.66f + Vector2.UnitY * 1.5f, 331, Main.rand.Next(40, 141) * 0.01f);
                        Main.gore[index2].sticky = false;
                    }
                }
                else if (type == 371)
                {
                    Main.PlaySound(13, (int)position.X, (int)position.Y, 1);
                    Main.PlaySound(2, (int)position.X, (int)position.Y, 16);
                    for (int index = 0; index < 5; ++index)
                        Dust.NewDust(new Vector2(position.X, position.Y), width, height, 13, 0.0f, 0.0f, 0, new Color(), 1f);
                    for (int index1 = 0; index1 < 30; ++index1)
                    {
                        Vector2 vector2 = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                        vector2.Normalize();
                        vector2 *= 0.4f;
                        int index2 = Gore.NewGore(Center + vector2 * 10f, vector2 * Main.rand.Next(4, 9) * 0.66f + Vector2.UnitY * 1.5f, Main.rand.Next(435, 438), Main.rand.Next(20, 100) * 0.01f);
                        Main.gore[index2].sticky = false;
                    }
                }
            }
            if (owner == Main.myPlayer)
            {
                if (type == 28 || type == 29 || (type == 37 || type == 108) || (type == 136 || type == 137 || (type == 138 || type == 142)) || (type == 143 || type == 144 || (type == 339 || type == 341) || (type == 470 || type == 516 || (type == 519 || type == 637))))
                {
                    int num2 = 3;
                    if (type == 28 || type == 37 || (type == 516 || type == 519))
                        num2 = 4;
                    if (type == 29 || type == 470 || type == 637)
                        num2 = 7;
                    if (type == 142 || type == 143 || (type == 144 || type == 341))
                        num2 = 5;
                    if (type == 108)
                        num2 = 10;
                    int num3 = (int)(position.X / 16.0 - num2);
                    int num4 = (int)(position.X / 16.0 + num2);
                    int num5 = (int)(position.Y / 16.0 - num2);
                    int num6 = (int)(position.Y / 16.0 + num2);
                    if (num3 < 0)
                        num3 = 0;
                    if (num4 > Main.maxTilesX)
                        num4 = Main.maxTilesX;
                    if (num5 < 0)
                        num5 = 0;
                    if (num6 > Main.maxTilesY)
                        num6 = Main.maxTilesY;
                    bool flag1 = false;
                    for (int index1 = num3; index1 <= num4; ++index1)
                    {
                        for (int index2 = num5; index2 <= num6; ++index2)
                        {
                            float num7 = Math.Abs(index1 - position.X / 16f);
                            float num8 = Math.Abs(index2 - position.Y / 16f);
                            if (Math.Sqrt(num7 * num7 + num8 * num8) < num2 && Main.tile[index1, index2] != null && Main.tile[index1, index2].wall == 0)
                            {
                                flag1 = true;
                                break;
                            }
                        }
                    }
                    AchievementsHelper.CurrentlyMining = true;
                    for (int i1 = num3; i1 <= num4; ++i1)
                    {
                        for (int j1 = num5; j1 <= num6; ++j1)
                        {
                            float num7 = Math.Abs(i1 - position.X / 16f);
                            float num8 = Math.Abs(j1 - position.Y / 16f);
                            if (Math.Sqrt(num7 * num7 + num8 * num8) < num2)
                            {
                                bool flag2 = true;
                                if (Main.tile[i1, j1] != null && Main.tile[i1, j1].active())
                                {
                                    flag2 = true;
                                    if (Main.tileDungeon[Main.tile[i1, j1].type] || Main.tile[i1, j1].type == 21 || (Main.tile[i1, j1].type == 26 || Main.tile[i1, j1].type == 107) || (Main.tile[i1, j1].type == 108 || Main.tile[i1, j1].type == 111 || (Main.tile[i1, j1].type == 226 || Main.tile[i1, j1].type == 237)) || (Main.tile[i1, j1].type == 221 || Main.tile[i1, j1].type == 222 || (Main.tile[i1, j1].type == 223 || Main.tile[i1, j1].type == 211) || Main.tile[i1, j1].type == 404))
                                        flag2 = false;
                                    if (!Main.hardMode && Main.tile[i1, j1].type == 58)
                                        flag2 = false;
                                    if (flag2)
                                    {
                                        WorldGen.KillTile(i1, j1, false, false, false);
                                        if (!Main.tile[i1, j1].active() && Main.netMode != 0)
                                            NetMessage.SendData(17, -1, -1, "", 0, i1, j1, 0.0f, 0, 0, 0);
                                    }
                                }
                                if (flag2)
                                {
                                    for (int i2 = i1 - 1; i2 <= i1 + 1; ++i2)
                                    {
                                        for (int j2 = j1 - 1; j2 <= j1 + 1; ++j2)
                                        {
                                            if (Main.tile[i2, j2] != null && Main.tile[i2, j2].wall > 0 && flag1)
                                            {
                                                WorldGen.KillWall(i2, j2, false);
                                                if (Main.tile[i2, j2].wall == 0 && Main.netMode != 0)
                                                    NetMessage.SendData(17, -1, -1, "", 2, i2, j2, 0.0f, 0, 0, 0);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    AchievementsHelper.CurrentlyMining = false;
                }
                if (Main.netMode != 0)
                    NetMessage.SendData(29, -1, -1, "", identity, owner, 0.0f, 0.0f, 0, 0, 0);
                if (!noDropItem)
                {
                    int number = -1;
                    if (aiStyle == 10)
                    {
                        int i = (int)(position.X + (width / 2)) / 16;
                        int j = (int)(position.Y + (width / 2)) / 16;
                        int tempType = 0;
                        int Type = 2;
                        if (this.type == 109)
                        {
                            tempType = 147;
                            Type = 0;
                        }
                        if (this.type == 31)
                        {
                            tempType = 53;
                            Type = 0;
                        }
                        if (this.type == 42)
                        {
                            tempType = 53;
                            Type = 0;
                        }
                        if (this.type == 56)
                        {
                            tempType = 112;
                            Type = 0;
                        }
                        if (this.type == 65)
                        {
                            tempType = 112;
                            Type = 0;
                        }
                        if (this.type == 67)
                        {
                            tempType = 116;
                            Type = 0;
                        }
                        if (this.type == 68)
                        {
                            tempType = 116;
                            Type = 0;
                        }
                        if (this.type == 71)
                        {
                            tempType = 123;
                            Type = 0;
                        }
                        if (this.type == 39)
                        {
                            tempType = 59;
                            Type = 176;
                        }
                        if (this.type == 40)
                        {
                            tempType = 57;
                            Type = 172;
                        }
                        if (this.type == 179)
                        {
                            tempType = 224;
                            Type = 0;
                        }
                        if (this.type == 241)
                        {
                            tempType = 234;
                            Type = 0;
                        }
                        if (this.type == 354)
                        {
                            tempType = 234;
                            Type = 0;
                        }
                        if (this.type == 411)
                        {
                            tempType = 330;
                            Type = 71;
                        }
                        if (this.type == 412)
                        {
                            tempType = 331;
                            Type = 72;
                        }
                        if (this.type == 413)
                        {
                            tempType = 332;
                            Type = 73;
                        }
                        if (this.type == 414)
                        {
                            tempType = 333;
                            Type = 74;
                        }
                        if (this.type == 109)
                        {
                            int index = Player.FindClosest(position, width, height);
                            if ((Center - Main.player[index].Center).Length() > Main.maxScreenW * 0.75)
                            {
                                tempType = -1;
                                Type = 593;
                            }
                        }
                        if (Main.tile[i, j].halfBrick() && velocity.Y > 0.0 && Math.Abs(velocity.Y) > Math.Abs(velocity.X))
                            --j;
                        if (!Main.tile[i, j].active() && tempType >= 0)
                        {
                            bool flag = false;
                            if (j < Main.maxTilesY - 2 && Main.tile[i, j + 1] != null && (Main.tile[i, j + 1].active() && Main.tile[i, j + 1].type == 314))
                                flag = true;
                            if (!flag)
                                WorldGen.PlaceTile(i, j, tempType, false, true, -1, 0);
                            if (!flag && Main.tile[i, j].active() && Main.tile[i, j].type == tempType)
                            {
                                if (Main.tile[i, j + 1].halfBrick() || Main.tile[i, j + 1].slope() != 0)
                                {
                                    WorldGen.SlopeTile(i, j + 1, 0);
                                    if (Main.netMode == 2)
                                        NetMessage.SendData(17, -1, -1, "", 14, i, (j + 1), 0.0f, 0, 0, 0);
                                }
                                if (Main.netMode != 0)
                                    NetMessage.SendData(17, -1, -1, "", 1, i, j, tempType, 0, 0, 0);
                            }
                            else if (Type > 0)
                                number = Item.NewItem((int)position.X, (int)position.Y, width, height, Type, 1, false, 0, false);
                        }
                        else if (Type > 0)
                            number = Item.NewItem((int)position.X, (int)position.Y, width, height, Type, 1, false, 0, false);
                    }
                    if (this.type == 1 && Main.rand.Next(3) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 40, 1, false, 0, false);
                    if (this.type == 474 && Main.rand.Next(3) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 3003, 1, false, 0, false);
                    if (this.type == 103 && Main.rand.Next(6) == 0)
                        number = Main.rand.Next(3) != 0 ? Item.NewItem((int)position.X, (int)position.Y, width, height, 40, 1, false, 0, false) : Item.NewItem((int)position.X, (int)position.Y, width, height, 545, 1, false, 0, false);
                    if (this.type == 2 && Main.rand.Next(3) == 0)
                        number = Main.rand.Next(3) != 0 ? Item.NewItem((int)position.X, (int)position.Y, width, height, 40, 1, false, 0, false) : Item.NewItem((int)position.X, (int)position.Y, width, height, 41, 1, false, 0, false);
                    if (this.type == 172 && Main.rand.Next(3) == 0)
                        number = Main.rand.Next(3) != 0 ? Item.NewItem((int)position.X, (int)position.Y, width, height, 40, 1, false, 0, false) : Item.NewItem((int)position.X, (int)position.Y, width, height, 988, 1, false, 0, false);
                    if (this.type == 171)
                    {
                        if (ai[1] == 0.0)
                        {
                            number = Item.NewItem((int)position.X, (int)position.Y, width, height, 985, 1, false, 0, false);
                            Main.item[number].noGrabDelay = 0;
                        }
                        else if (ai[1] < 10.0)
                        {
                            number = Item.NewItem((int)position.X, (int)position.Y, width, height, 965, (int)(10.0 - ai[1]), false, 0, false);
                            Main.item[number].noGrabDelay = 0;
                        }
                    }
                    if (this.type == 475)
                    {
                        if (ai[1] == 0.0)
                        {
                            number = Item.NewItem((int)position.X, (int)position.Y, width, height, 3005, 1, false, 0, false);
                            Main.item[number].noGrabDelay = 0;
                        }
                        else if (ai[1] < 10.0)
                        {
                            number = Item.NewItem((int)position.X, (int)position.Y, width, height, 2996, (int)(10.0 - ai[1]), false, 0, false);
                            Main.item[number].noGrabDelay = 0;
                        }
                    }
                    if (this.type == 505)
                    {
                        if (ai[1] == 0.0)
                        {
                            number = Item.NewItem((int)position.X, (int)position.Y, width, height, 3079, 1, false, 0, false);
                            Main.item[number].noGrabDelay = 0;
                        }
                        else if (ai[1] < 10.0)
                        {
                            number = Item.NewItem((int)position.X, (int)position.Y, width, height, 3077, (int)(10.0 - ai[1]), false, 0, false);
                            Main.item[number].noGrabDelay = 0;
                        }
                    }
                    if (this.type == 506)
                    {
                        if (ai[1] == 0.0)
                        {
                            number = Item.NewItem((int)position.X, (int)position.Y, width, height, 3080, 1, false, 0, false);
                            Main.item[number].noGrabDelay = 0;
                        }
                        else if (ai[1] < 10.0)
                        {
                            number = Item.NewItem((int)position.X, (int)position.Y, width, height, 3078, (int)(10.0 - ai[1]), false, 0, false);
                            Main.item[number].noGrabDelay = 0;
                        }
                    }
                    if (this.type == 91 && Main.rand.Next(6) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 516, 1, false, 0, false);
                    if (this.type == 50 && Main.rand.Next(3) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 282, 1, false, 0, false);
                    if (this.type == 515 && Main.rand.Next(3) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 3112, 1, false, 0, false);
                    if (this.type == 53 && Main.rand.Next(3) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 286, 1, false, 0, false);
                    if (this.type == 48 && Main.rand.Next(2) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 279, 1, false, 0, false);
                    if (this.type == 54 && Main.rand.Next(2) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 287, 1, false, 0, false);
                    if (this.type == 3 && Main.rand.Next(2) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 42, 1, false, 0, false);
                    if (this.type == 4 && Main.rand.Next(4) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 47, 1, false, 0, false);
                    if (this.type == 12 && damage > 500)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 75, 1, false, 0, false);
                    if (this.type == 155)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 859, 1, false, 0, false);
                    if (this.type == 598 && Main.rand.Next(4) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 3378, 1, false, 0, false);
                    if (this.type == 599 && Main.rand.Next(4) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 3379, 1, false, 0, false);
                    if (this.type == 21 && Main.rand.Next(2) == 0)
                        number = Item.NewItem((int)position.X, (int)position.Y, width, height, 154, 1, false, 0, false);
                    if (Main.netMode == 1 && number >= 0)
                        NetMessage.SendData(21, -1, -1, "", number, 1f, 0.0f, 0.0f, 0, 0, 0);
                }
                if (type == 69 || type == 70 || type == 621)
                {
                    int i = (int)(position.X + (width / 2)) / 16;
                    int j = (int)(position.Y + (height / 2)) / 16;
                    if (type == 69)
                        WorldGen.Convert(i, j, 2, 4);
                    if (type == 70)
                        WorldGen.Convert(i, j, 1, 4);
                    if (type == 621)
                        WorldGen.Convert(i, j, 4, 4);
                }
                if (this.type == 370 || this.type == 371)
                {
                    float num2 = 80f;
                    int tempType = 119;
                    if (this.type == 371)
                        tempType = 120;
                    for (int index = 0; index < 255; ++index)
                    {
                        Player player = Main.player[index];
                        if (player.active && !player.dead && Vector2.Distance(Center, player.Center) < num2)
                            player.AddBuff(tempType, 1800, true);
                    }
                    for (int index = 0; index < 200; ++index)
                    {
                        NPC npc = Main.npc[index];
                        if (npc.active && npc.life > 0 && Vector2.Distance(Center, npc.Center) < num2)
                            npc.AddBuff(tempType, 1800, false);
                    }
                }
                if (type == 378)
                {
                    int num2 = Main.rand.Next(2, 4);
                    if (Main.rand.Next(5) == 0)
                        ++num2;
                    for (int index = 0; index < num2; ++index)
                    {
                        float num3 = velocity.X;
                        float num4 = velocity.Y;
                        NewProjectile(Center.X, Center.Y, num3 * (float)(1.0 + Main.rand.Next(-20, 21) * 0.00999999977648258), num4 * (float)(1.0 + Main.rand.Next(-20, 21) * 0.00999999977648258), 379, damage, knockBack, owner, 0.0f, 0.0f);
                    }
                }
            }
            active = false;
        }

        public Color GetAlpha(Color newColor)
        {
            if (type == 270)
                return new Color(255, 255, 255, Main.rand.Next(0, 255));
            int num1;
            int num2;
            int num3;
            if (type == 650)
            {
                int num4 = (int)(newColor.R * 1.5);
                int num5 = (int)(newColor.G * 1.5);
                int num6 = (int)(newColor.B * 1.5);
                if (num4 > 255)
                    num1 = 255;
                if (num5 > 255)
                    num2 = 255;
                if (num6 > 255)
                    num3 = 255;
            }
            else
            {
                if (type == 604 || type == 631)
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 0);
                if (type == 636)
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 64 - alpha / 4);
                if (type == 603 || type == 633)
                    return new Color(255, 255, 255, 200);
                if (type == 623 || type >= 625 && type <= 628)
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 255 - alpha);
                if (type == 645 || type == 643)
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 127 - alpha / 2);
                if (type == 611)
                    return new Color(255, 255, 255, 200);
                if (type == 640 || type == 644)
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 0);
                if (type == 612)
                    return new Color(255, 255, 255, 127);
                if (aiStyle == 105)
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 255 - alpha);
                if (type == 554)
                    return new Color(200, 200, 200, 200);
                if (type == 601)
                    return PortalHelper.GetPortalColor(owner, (int)ai[0]);
                if (type == 602)
                {
                    Color portalColor = PortalHelper.GetPortalColor(owner, (int)ai[1]);
                    portalColor.A = 227;
                    return portalColor;
                }
                if (type == 585)
                {
                    byte a = newColor.A;
                    newColor = Color.Lerp(newColor, Color.White, 0.5f);
                    newColor.A = a;
                    return newColor;
                }
                if (type == 573 || type == 578 || (type == 579 || type == 617) || type == 641)
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 255 - alpha);
                if (type == 9 || type == 490)
                    return Color.White;
                if (type == 575 || type == 596)
                {
                    if (timeLeft < 30)
                        alpha = (int)(255 - 255 * (double)(timeLeft / 30f));
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 128 - alpha / 2);
                }
                if (type == 546)
                    return new Color(255, 200, 255, 200);
                if (type == 553)
                    return new Color(255, 255, 200, 200);
                if (type == 540)
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 0);
                if (type == 498)
                    return new Color(255, 100, 20, 200);
                if (type == 538)
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 255 - alpha);
                if (type == 518)
                {
                    float num4 = (float)(1.0 - (double)alpha / 255);
                    return new Color((int)(200.0 * num4), (int)(200.0 * num4), (int)(200.0 * num4), (int)(100.0 * num4));
                }
                if (type == 518 || type == 595)
                {
                    Color color = Color.Lerp(newColor, Color.White, 0.85f);
                    color.A = Convert.ToByte(-128);
                    return color * (float)(1.0 - (double)alpha / 255);
                }
                if (type == 536 || type == 607)
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 63 - alpha / 4);
                if (type == 591)
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 63 - alpha / 4);
                if (type == 493 || type == 494)
                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 255 - alpha);
                if (type == 492)
                    return new Color(255, 255, 255, 255);
                if (type == 491)
                    return new Color(255, 255, 255, 255);
                if (type == 485 || type == 502)
                    return new Color(255, 255, 255, 200);
                if (type == 488)
                    return new Color(255, 255, 255, 255);
                if (type == 477 || type == 478 || type == 479)
                {
                    if (alpha == 0)
                        return new Color(255, 255, 255, 200);
                    return new Color(0, 0, 0, 0);
                }
                if (type == 473)
                    return new Color(255, 255, 255, 255);
                if (type == 50 || type == 53 || type == 515)
                    return new Color(255, 255, 255, 0);
                if (type == 92)
                    return new Color(255, 255, 255, 0);
                if (type == 91)
                    return new Color(200, 200, 200, 0);
                if (type == 34 || type == 15 || (type == 93 || type == 94) || (type == 95 || type == 96 || (type == 253 || type == 258)) || type == 102 && alpha < 255)
                    return new Color(200, 200, 200, 25);
                if (type == 465)
                    return new Color(255, 255, 255, 0) * (float)(1.0 - (double)alpha / 255);
                if (type == 503)
                    return Color.Lerp(Color.Lerp(newColor, Color.White, 0.5f) * (float)(1.0 - (double)alpha / 255), Color.Lerp(Color.Purple, Color.White, 0.33f), (float)(0.25 + Math.Cos(localAI[0]) * 0.25));
                if (type == 467)
                    return new Color(255, 255, 255, 255) * (float)(1.0 - (double)alpha / 255);
                if (type == 634 || type == 635)
                    return new Color(255, 255, 255, 127) * Opacity;
                if (type == 451)
                    return new Color(255, 255, 255, 200) * (float)((255 - (double)alpha) / 255);
                if (type == 454 || type == 452)
                    return new Color(255, 255, 255, 255) * (float)(1.0 - (double)alpha / 255);
                if (type == 464)
                    return new Color(255, 255, 255, 255) * (float)((255 - (double)alpha) / 255);
                if (type == 450)
                    return new Color(200, 200, 200, 255 - alpha);
                if (type == 459)
                    return new Color(255, 255, 255, 200);
                if (type == 447)
                    return new Color(255, 255, 255, 200);
                if (type == 446)
                    return Color.Lerp(newColor, Color.White, 0.8f) * (float)(1.0 - (double)alpha / 255);
                if (type >= 646 && type <= 649)
                    return Color.Lerp(newColor, Color.White, 0.8f) * (float)(1.0 - (double)alpha / 255);
                if (type == 445)
                    return new Color(255, 255, 255, 128) * (float)(1.0 - (double)alpha / 255);
                if (type == 440 || type == 449 || type == 606)
                {
                    num1 = 255 - alpha;
                    num2 = 255 - alpha;
                    num3 = 255 - alpha;
                }
                else
                {
                    if (type == 444)
                        return newColor * (float)(1.0 - (double)alpha / 255);
                    if (type == 443)
                        return new Color(255, 255, 255, 128) * (float)(1.0 - (double)alpha / 255);
                    if (type == 438)
                        return new Color(255, 255, 255, 128) * (float)(1.0 - (double)alpha / 255);
                    if (type == 592)
                        return new Color(255, 255, 255, 128) * (float)(1.0 - (double)alpha / 255);
                    if (type == 437)
                        return new Color(255, 255, 255, 0) * (float)(1.0 - (double)alpha / 255);
                    if (type == 462)
                        return new Color(255, 255, 255, 128) * (float)(1.0 - (double)alpha / 255);
                    if (type == 352)
                        return new Color(250, 250, 250, alpha);
                    if (type == 435)
                    {
                        newColor = Color.Lerp(newColor, Color.White, 0.8f);
                        return new Color(newColor.R, newColor.G, newColor.B, 25);
                    }
                    if (type == 436)
                    {
                        newColor = Color.Lerp(newColor, Color.White, 0.8f);
                        return new Color(newColor.R, newColor.G, newColor.B, 25);
                    }
                    if (type == 409)
                        return new Color(250, 250, 250, 200);
                    if (type == 348 || type == 349)
                        return new Color(200, 200, 200, alpha);
                    if (type == 337)
                        return new Color(250, 250, 250, 150);
                    if (type >= 424 && type <= 426)
                    {
                        byte num4 = 150;
                        if (newColor.R < num4)
                            newColor.R = num4;
                        if (newColor.G < num4)
                            newColor.G = num4;
                        if (newColor.B < num4)
                            newColor.B = num4;
                        return new Color(newColor.R, newColor.G, newColor.B, 255);
                    }
                    if (type == 431 || type == 432)
                        return new Color(250, 250, 250, 255 - alpha);
                    if (type == 343 || type == 344)
                    {
                        float num4 = (float)(1.0 - (double)alpha / 255);
                        return new Color((int)(250.0 * num4), (int)(250.0 * num4), (int)(250.0 * num4), (int)(100.0 * num4));
                    }
                    if (type == 332)
                        return new Color(255, 255, 255, 255);
                    if (type == 329)
                        return new Color(200, 200, 200, 50);
                    if (type >= 326 && type <= 328 || type >= 400 && type <= 402)
                        return Color.Transparent;
                    if (type == 324 && frame >= 6 && frame <= 9)
                        return new Color(255, 255, 255, 255);
                    if (type == 16)
                        return new Color(255, 255, 255, 0);
                    if (type == 321)
                        return new Color(200, 200, 200, 0);
                    if (type == 76 || type == 77 || type == 78)
                        return new Color(255, 255, 255, 0);
                    if (type == 308)
                        return new Color(200, 200, 255, 125);
                    if (type == 263)
                    {
                        if (timeLeft < 255)
                            return new Color(255, 255, 255, (byte)timeLeft);
                        return new Color(255, 255, 255, 255);
                    }
                    if (type == 274)
                    {
                        if (timeLeft >= 85)
                            return new Color(255, 255, 255, 100);
                        byte num4 = (byte)(timeLeft * 3);
                        byte num5 = (byte)(100.0 * (num4 / 255));
                        return new Color(num4, num4, num4, num5);
                    }
                    if (type == 5)
                        return new Color(255, 255, 255, 0);
                    if (type == 300 || type == 301)
                        return new Color(250, 250, 250, 50);
                    if (type == 304)
                        return new Color(255 - alpha, 255 - alpha, 255 - alpha, (byte)((255 - alpha) / 3.0));
                    if (type == 116 || type == 132 || (type == 156 || type == 157) || (type == 157 || type == 173))
                    {
                        if (localAI[1] >= 15.0)
                            return new Color(255, 255, 255, alpha);
                        if (localAI[1] < 5.0)
                            return Color.Transparent;
                        int num4 = (int)((localAI[1] - 5.0) / 10.0 * 255);
                        return new Color(num4, num4, num4, num4);
                    }
                    if (type == 254)
                    {
                        if (timeLeft < 30)
                            alpha = (int)(255 - 255 * (double)(timeLeft / 30f));
                        return new Color(255 - alpha, 255 - alpha, 255 - alpha, 0);
                    }
                    if (type == 265 || type == 355)
                    {
                        if (alpha > 0)
                            return Color.Transparent;
                        return new Color(255, 255, 255, 0);
                    }
                    if (type == 270 && ai[0] >= 0.0)
                    {
                        if (alpha > 0)
                            return Color.Transparent;
                        return new Color(255, 255, 255, 200);
                    }
                    if (type == 257)
                    {
                        if (alpha > 200)
                            return Color.Transparent;
                        return new Color(255 - alpha, 255 - alpha, 255 - alpha, 0);
                    }
                    if (type == 259)
                    {
                        if (alpha > 200)
                            return Color.Transparent;
                        return new Color(255 - alpha, 255 - alpha, 255 - alpha, 0);
                    }
                    if (type >= 150 && type <= 152)
                        return new Color(255 - alpha, 255 - alpha, 255 - alpha, 255 - alpha);
                    if (type == 250)
                        return Color.Transparent;
                    if (type == 251)
                        return new Color(255 - alpha, 255 - alpha, 255 - alpha, 0);
                    if (type == 131)
                        return new Color(255 - alpha, 255 - alpha, 255 - alpha, 0);
                    if (type == 211)
                        return new Color(255, 255, 255, 0);
                    if (type == 229)
                        return new Color(255, 255, 255, 50);
                    if (type == 221)
                        return new Color(255, 255, 255, 200);
                    if (type == 20)
                    {
                        if (alpha <= 150)
                            return new Color(255, 255, 255, 0);
                        return new Color(0, 0, 0, 0);
                    }
                    if (type == 207)
                    {
                        num1 = 255 - alpha;
                        num2 = 255 - alpha;
                        num3 = 255 - alpha;
                    }
                    else
                    {
                        if (type == 242)
                        {
                            if (alpha < 140)
                                return new Color(255, 255, 255, 100);
                            return Color.Transparent;
                        }
                        if (type == 638)
                            return new Color(255, 255, 255, 100) * Opacity;
                        if (type == 209)
                        {
                            num1 = newColor.R - alpha;
                            num2 = newColor.G - alpha;
                            num3 = newColor.B - alpha / 2;
                        }
                        else
                        {
                            if (type == 130)
                                return new Color(255, 255, 255, 175);
                            if (type == 182)
                                return new Color(255, 255, 255, 200);
                            if (type == 226)
                            {
                                int num4 = 255;
                                int num5 = 255;
                                int num6 = 255;
                                float num7 = (float)(Main.mouseTextColor / 200.0 - 0.300000011920929);
                                int num8 = (int)(num4 * num7);
                                int num9 = (int)(num5 * num7);
                                int num10 = (int)(num6 * num7);
                                int r = num8 + 50;
                                if (r > 255)
                                    r = 255;
                                int g = num9 + 50;
                                if (g > 255)
                                    g = 255;
                                int b = num10 + 50;
                                if (b > 255)
                                    b = 255;
                                return new Color(r, g, b, 200);
                            }
                            if (type == 227)
                            {
                                int num4;
                                int num5 = num4 = 255;
                                int num6 = num4;
                                int num7 = num4;
                                float num8 = (float)(Main.mouseTextColor / 100.0 - 1.60000002384186);
                                int num9 = (int)(num7 * num8);
                                int num10 = (int)(num6 * num8);
                                int num11 = (int)(num5 * num8);
                                int a = (int)(100.0 * num8);
                                int r = num9 + 50;
                                if (r > 255)
                                    r = 255;
                                int g = num10 + 50;
                                if (g > 255)
                                    g = 255;
                                int b = num11 + 50;
                                if (b > 255)
                                    b = 255;
                                return new Color(r, g, b, a);
                            }
                            if (type == 114 || type == 115)
                            {
                                if (localAI[1] >= 15.0)
                                    return new Color(255, 255, 255, alpha);
                                if (localAI[1] < 5.0)
                                    return Color.Transparent;
                                int num4 = (int)((localAI[1] - 5.0) / 10.0 * 255);
                                return new Color(num4, num4, num4, num4);
                            }
                            if (type == 83 || type == 88 || (type == 89 || type == 90) || (type == 100 || type == 104 || type == 279) || type >= 283 && type <= 287)
                            {
                                if (alpha < 200)
                                    return new Color(255 - alpha, 255 - alpha, 255 - alpha, 0);
                                return Color.Transparent;
                            }
                            if (type == 34 || type == 35 || (type == 15 || type == 19) || (type == 44 || type == 45))
                                return Color.White;
                            if (type == 79)
                            {
                                num1 = Main.DiscoR;
                                num2 = Main.DiscoG;
                                num3 = Main.DiscoB;
                                return new Color();
                            }
                            if (type == 9 || type == 15 || (type == 34 || type == 50) || (type == 53 || type == 76 || (type == 77 || type == 78)) || (type == 92 || type == 91))
                            {
                                num1 = newColor.R - alpha / 3;
                                num2 = newColor.G - alpha / 3;
                                num3 = newColor.B - alpha / 3;
                            }
                            else
                            {
                                if (type == 18)
                                    return new Color(255, 255, 255, 50);
                                if (type == 16 || type == 44 || type == 45)
                                {
                                    num1 = newColor.R;
                                    num2 = newColor.G;
                                    num3 = newColor.B;
                                }
                                else if (type == 12 || type == 72 || (type == 86 || type == 87))
                                    return new Color(255, 255, 255, newColor.A - alpha);
                            }
                        }
                    }
                }
            }
            float num12 = (float)(255 - alpha) / 255;
            int r1 = (int)(newColor.R * num12);
            int g1 = (int)(newColor.G * num12);
            int b1 = (int)(newColor.B * num12);
            int a1 = newColor.A - alpha;
            if (a1 < 0)
                a1 = 0;
            if (a1 > 255)
                a1 = 255;
            return new Color(r1, g1, b1, a1);
        }

        public override string ToString()
        {
            return "name:" + (object)name + ", active:" + (string)(object)(active ? 1 : 0) + ", whoAmI:" + (string)(object)whoAmI;
        }
    }
}
