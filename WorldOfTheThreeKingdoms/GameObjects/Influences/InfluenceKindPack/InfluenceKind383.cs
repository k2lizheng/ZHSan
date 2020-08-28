﻿using GameObjects;
using GameObjects.Influences;
using System;


using System.Runtime.Serialization;namespace GameObjects.Influences.InfluenceKindPack
{

    [DataContract]public class InfluenceKind383 : InfluenceKind
    {

        public override void ApplyInfluenceKind(Troop troop)
        {
            troop.BasePierceAttack = true;
        }

        public override void PurifyInfluenceKind(Troop troop)
        {
            troop.BasePierceAttack = false;
        }

    }
}

