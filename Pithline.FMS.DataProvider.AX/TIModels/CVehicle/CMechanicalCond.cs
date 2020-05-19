using System;
using System.Collections.Generic;
namespace Pithline.FMS.DataProvider.AX.TIModels.CVehicle
{
    public class CMechanicalCond
    {

        public List<string> EngineImgList { get; set; }
        public String EngineComment { get; set; }
        public Boolean IsEngineDmg { get; set; }
        public String FrontSuspComment { get; set; }
        public Boolean IsFrontSuspDmg { get; set; }
        public List<string> FrontSuspImgList { get; set; }
        public String RearSuspComment { get; set; }
        public Boolean IsRearSuspDmg { get; set; }
        public List<string> RearSuspImgList { get; set; }
        public String SteeringComment { get; set; }
        public Boolean IsSteeringDmg { get; set; }
        public List<string> SteeringImgList { get; set; }
        public String ExhaustComment { get; set; }
        public Boolean IsExhaustDmg { get; set; }
        public List<string> ExhaustImgList { get; set; }
        public String BatteryComment { get; set; }
        public Boolean IsBatteryDmg { get; set; }
        public List<string> BatteryImgList { get; set; }
        public String HandBrakeComment { get; set; }
        public Boolean IsHandBrakeDmg { get; set; }
        public List<string> HandBrakeImgList { get; set; }
        public String FootBrakeComment { get; set; }
        public Boolean IsFootBrakeDmg { get; set; }
        public List<string> FootBrakeImgList { get; set; }
        public String GearboxComment { get; set; }
        public Boolean IsGearboxDmg { get; set; }
        public List<string> GearboxImgList { get; set; }
        public String DifferentialComment { get; set; }
        public Boolean IsDifferentialDmg { get; set; }
        public List<string> DifferentialImgList { get; set; }
        public String AutoTransmissionComment { get; set; }
        public Boolean IsAutoTransmissionDmg { get; set; }
        public List<string> AutoTransmissionImgList { get; set; }
        public String OilLeaksComment { get; set; }
        public Boolean IsOilLeaksDmg { get; set; }
        public List<string> OilLeaksImgList { get; set; }
        public String HPSComment { get; set; }
        public Boolean IsHPSDmg { get; set; }
        public List<string> HPSImgList { get; set; }
        public Boolean IsRunning { get; set; }
        public Boolean HasPetrol { get; set; }
        public Boolean HasDiesel { get; set; }
        public Boolean HasLPG { get; set; }
        public Boolean HasMainBattery { get; set; }
        public String EngineImgPathList { get; set; }
        public String RearSuspImgPathList { get; set; }
        public String FrontSuspImgPathList { get; set; }
        public String SteeringImgPathList { get; set; }
        public String ExhaustImgPathList { get; set; }
        public String BatteryImgPathList { get; set; }
        public String HandBrakeImgPathList { get; set; }
        public String FootBrakeImgPathList { get; set; }
        public String GearboxImgPathList { get; set; }
        public String DifferentialImgPathList { get; set; }
        public String AutoTransmissionImgPathList { get; set; }
        public String OilLeaksImgPathList { get; set; }
        public String HPSImgPathList { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public List<string> Errors { get; set; }
        public Boolean ShouldSave { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

