using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_LifeHistoryModel
    {
        public int Id{get;set;}
        public long FeeNo{get;set;}
        public string Name{get;set;}
        public string NickName{get;set;}
        public string Nick { get; set; }
        public string ResidentNo{get;set;}
        public string BirthPlace{get;set;}
        public string FamilyEnvironment{get;set;}
        public string ChildHoodExperience{get;set;}
        public string School{get;set;}
        public string ProudDeeds{get;set;}
        public string Romance{get;set;}
        public string MerryInfo{get;set;}
        public string MportantPeople{get;set;}
        public string WorkHistory{get;set;}
        public string ServiceHistory{get;set;}
        public string Religious{get;set;}
        public string Living{get;set;}
        public string PositivePersonality{get;set;}
        public string NegativePersonality{get;set;}
        public string FamilyTroubled{get;set;}
        public string SoothingEmotion{get;set;}
        public string Skill{get;set;}
        public string FavoriteDress{get;set;}
        public string Foodlike{get;set;}
        public string Animallike{get;set;}
        public string HolidayActivity{get;set;}
        public string NotlikeThings{get;set;}
        public string InterestedThings{get;set;}
        public DateTime? CreateDate{get;set;}
        public string CreateBy{get;set;}
        public string OrgId{get;set;}
    }
}
