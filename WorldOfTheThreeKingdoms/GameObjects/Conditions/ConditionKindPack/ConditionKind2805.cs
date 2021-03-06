﻿using GameObjects;
using GameObjects.Conditions;
using System;


using System.Runtime.Serialization;namespace GameObjects.Conditions.ConditionKindPack
{

    [DataContract]public class ConditionKind2805 : ConditionKind
    {
        private int number = 0;

        public override bool CheckConditionKind(Architecture a)
        {
            foreach (Person p in a.Persons)
            {
                if (p.ID == number && p.Status == GameObjects.PersonDetail.PersonStatus.Normal)
                {
                    return false;
                }
            }
            return true;
        }

        public override void InitializeParameter(string parameter)
        {
            try
            {
                this.number = int.Parse(parameter);
            }
            catch
            {
            }
        }

    }
}

