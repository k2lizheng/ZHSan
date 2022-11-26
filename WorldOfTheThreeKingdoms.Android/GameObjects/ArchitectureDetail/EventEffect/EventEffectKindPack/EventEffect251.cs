using GameObjects;
using System;
using GameManager;


using System.Runtime.Serialization;
namespace GameObjects.ArchitectureDetail.EventEffect
{

    [DataContract]
    public class EventEffect251 : EventEffectKind
    {
        public override void ApplyEffectKind(Person person, Event e)
        {
            if (Session.Current.Scenario.CurrentPlayer != null)
            {
                Faction f = Session.Current.Scenario.CurrentPlayer;
                person.Alive = true;
                person.Available = true;
                if (person.Age<1)
                {                  
                    person.YearBorn = Session.Current.Scenario.Date.Year-16;                   
                }
                //person.Status = GameObjects.PersonDetail.PersonStatus.Normal;
                person.LocationArchitecture=f.Capital;
                person.ChangeFaction(f);
            }
           
        }

    }
}

