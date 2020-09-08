﻿using GameObjects;
using GameObjects.Conditions;
using System;


using System.Runtime.Serialization;namespace GameObjects.Conditions.ConditionKindPack
{

    [DataContract]public class ConditionKind1051 : ConditionKind
    {
        public override bool CheckConditionKind(Troop troop)
        {
            return (troop.Army.FollowedLeader != troop.Leader);
        }
    }
}
