﻿using GameManager;
using GameObjects;
using GameObjects.Conditions;
using System;


using System.Runtime.Serialization;namespace GameObjects.Conditions.ConditionKindPack
{

    [DataContract]public class ConditionKind1241 : ConditionKind
    {
        public override bool CheckConditionKind(Troop troop)
        {
            GameObjectList viewingArchitecturesByPosition = Session.Current.Scenario.GetViewingArchitecturesByPosition(troop.Position);
            if (viewingArchitecturesByPosition.Count > 0)
            {
                foreach (Architecture architecture in viewingArchitecturesByPosition)
                {
                    if (troop.IsFriendly(architecture.BelongedFaction))
                    {
                        return false;
                    }
                }
                return true;
            }
            return true;
        }
    }
}

