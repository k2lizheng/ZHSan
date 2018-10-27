﻿using GameObjects;
using GameObjects.Conditions;
using System;


using System.Runtime.Serialization;namespace GameObjects.Conditions.ConditionKindPack
{

    [DataContract]public class ConditionKind4302 : ConditionKind
    {
 
        public override bool CheckConditionKind(Person person)
        {
            return person.WorkKind == GameGlobal.ArchitectureWorkKind.商业;
        }
    }
}

