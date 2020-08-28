﻿using GameManager;
using GameObjects;
using System;


namespace GameObjects.FactionDetail
{

    public class DiplomaticRelationDisplay : GameObject
    {
        private string factionName;
        private DiplomaticRelation LinkedDiplomaticRelation;

        public DiplomaticRelationDisplay(DiplomaticRelation linked, string displayName)
        {
            this.LinkedDiplomaticRelation = linked;
            this.factionName = displayName;
        }

        public string FactionName
        {
            get
            {
                return this.factionName;
            }
        }

        public Faction LinkedFaction1
        {
            get
            {
                return this.LinkedDiplomaticRelation.RelationFaction1;
            }
        }

        public Faction LinkedFaction2
        {
            get
            {
                return this.LinkedDiplomaticRelation.RelationFaction2;
            }
        }

        public int Relation
        {
            get
            {
                return this.LinkedDiplomaticRelation.Relation;
            }
            set
            {
                this.LinkedDiplomaticRelation.Relation = value;
            }
        }
        public int Truce
        {
            get
            {
                return this.LinkedDiplomaticRelation.Truce;
            }
            set
            {
                this.LinkedDiplomaticRelation.Truce = value;
            }
        }

        public int TruceDays
        {
            get
            {
                return Truce * Session.Current.Scenario.Parameters.DayInTurn;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DiplomaticRelationDisplay)) return false;
            return this.LinkedDiplomaticRelation.Equals(((DiplomaticRelationDisplay)obj).LinkedDiplomaticRelation);
        }

        public override int GetHashCode()
        {
            return this.LinkedDiplomaticRelation.GetHashCode();
        }
    }
}

