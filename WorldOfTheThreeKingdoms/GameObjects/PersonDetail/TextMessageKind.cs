﻿using System;


namespace GameObjects.PersonDetail
{

    public enum TextMessageKind
    {
        None,
        Critical, // 1
        CriticalArchitecture, // 2
        BeCritical, // 3
        Surround, // 4
        Rout, // 5
        DualActiveWin, // 6
        DualPassiveWin, // 7
        ControversyActiveWin, //8 
        ControversyPassiveWin, // 9
        Chaos, // 10
        DeepChaos, // 11
        CastDeepChaos, // 12
        RecoverChaos, // 13
        TrappedByStratagem, // 14
        HelpedByStratagem, // 15
        ResistHarmfulStratagem, // 16
        ResistHelpfulStratagem, // 17
        AntiAttack, // 18
        BreakWall, // 19
        Angry, // 20
        Calm, // 21
        StartWork, // 22
        StudySkillSuccess, // 23
        StudySkillFailure, // 24
        StudyStuntSuccess, // 25
        StudyStuntFailure, // 26
        StudyTitleSuccess, // 27
        StudyTitleFailure, // 28
        HiredPerson, // 29
        Rewarded, // 30
        BeAwardedTreasure, // 31
        BeConfiscatedTreasure, // 32
        TreasureFound, // 33
        InformationSuccess, // 34
        InformationFailure, // 35
        SearchFundFound, // 36
        SearchFoodFound, // 37
        SearchTechniqueFound, // 38
        SearchSpyFound, // 39
        SearchPersonFound,
        PersonTreasureFound,// 40
        LeaveFaction, // 41
        CaptiveEscape, // 42
        StartCampaign, // 43
        TroopMoveTo, // 44
        TransportReturn, // 45
        GetSpreadBurnt, // 46
        UseCombatMethod, // 47
        SetCombatMethod, // 48
        UseStunt, // 49
        NoFactionUseStratagemFriendly, // 50
        NoFactionUseStratagemHostile, // 51
        UseStratagem0, // 52
        UseStratagem1, // 53
        UseStratagem2, // 54
        UseStratagem3, // 55
        UseStratagem4, // 56
        UseStratagem5, // 57
        UseStratagem6, // 58
        UseStratagem7, // 59
        UseStratagem8, // 60
        UseStratagem9, // 61
        UseStratagem10, // 62
        UseStratagem11, // 63
        SetStratagem, // 64
        StartAmbush, // 65
        StopAmbush, // 66
        Ambush, // 67
        BeAmbush, // 68
        DiscoverAmbush, // 69
        BeDiscoverAmbush, // 70
        TroopNewCaptive, // 71
        StartCutRouteway, // 72
        StopCutRouteway, // 73
        CutRoutewaySuccess, // 74
        CutRoutewayFail, // 75
        Died, // 76
        DiedInChallenge, // 77
        DiedChangeFaction, // 78
        CreateBrother, // 79
        CreateSister, // 80
        CreateSpouse, // 81
        TakePrincess, // 82
        Hougong, // 83
        SelfFoundPregnant, // 84
        CoupleFoundPregnant, // 85
        FoundPregnant, // 86
        ChildrenBorn, // 87
        BeChildrenBorn, // 88
        BeTakenSpouse, // 89
        ChildJoin, // 90
        ChildJoinSelfTalk, // 91
        FemaleSpouseJoin, // 92
        MaleSpouseJoin, // 93
        EnhanceDiplomaticRelation, // 94
        EncircleDiplomaticRelation, // 95
        BreakDiplomaticRelation, // 96
        ResetDiplomaticRelation, // 97
        CreateAlly, // 98
        CreateAllyFailed, // 99
        Truce, // 100
        TruceFailed, // 101
        AsLeaderCaught, // 102
        ReleaseCaptive, // 103
        KillCaptive, // 104
        ReleaseSelfPerson, // 105
        GetTurn, // 106
        FacilityCompleted, // 107
        LeaderOccupy, // 108
        DisasterHappened, // 109
        FactionTechniqueFinished, // 110
        ArchitectureUnderAttack, // 111
        RiseEmperorClass, // 112
        BecomeEmperorLegally, // 113
        BecomeEmperorIllegally, // 114
        SelfBecomeInfluenceConsequence, // 115
        CreateNewFaction, // 116
        ChangeLeaderKeepName, // 117
        ChangeLeaderChangeName, // 118
        EndWithUnite, // 119
        SelfRiseEmperorClass, // 120
        Rumour, // 121
        Attract, // 122
        PersonJoin, // 123
        ObtainMilitaryKind, // 124
        MakeMarriage, // 125
        SelectPrince, // 126
        ZhaoXian, // 127
        AppointMayor, // 128
        QuanXiang, //129
        QuanXiangFailed, // 130
        GeDi, // 131
        AIMergeAgainstPlayer, //132
        ZhiBao,
    }
}
