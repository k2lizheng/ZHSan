﻿using GameManager;
using GameObjects;
using GameObjects.PersonDetail;
using System;


using System.Runtime.Serialization;namespace GameObjects.ArchitectureDetail.EventEffect
{

    [DataContract]public class EventEffect290 : EventEffectKind
    {
        
        public override void ApplyEffectKind(Person person, Event e)
        {

            if (person.BelongedFaction == null && person.LocationArchitecture != null && person .BelongedCaptive == null )
            {
                Session.Current.Scenario.CreateNewFaction (person);
            }
            else if (person.BelongedFaction != null && person != person.BelongedFaction.Leader && person.LocationArchitecture != person.BelongedFaction.Capital)
            {
                Session.Current.Scenario.CreateNewFaction(person);
            }

        }



    }
}
