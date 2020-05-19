using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Commercial
{
    public class CMechanicalCond : BaseModel
    {
        public CMechanicalCond()
        {
            this.EngineImgList = new ObservableCollection<ImageCapture>();
            this.RearSuspImgList = new ObservableCollection<ImageCapture>();
            this.FrontSuspImgList = new ObservableCollection<ImageCapture>();
            this.SteeringImgList = new ObservableCollection<ImageCapture>();
            this.ExhaustImgList = new ObservableCollection<ImageCapture>();
            this.BatteryImgList = new ObservableCollection<ImageCapture>();
            this.HandBrakeImgList = new ObservableCollection<ImageCapture>();
            this.FootBrakeImgList = new ObservableCollection<ImageCapture>();
            this.GearboxImgList = new ObservableCollection<ImageCapture>();
            this.DifferentialImgList = new ObservableCollection<ImageCapture>();
            this.AutoTransmissionImgList = new ObservableCollection<ImageCapture>();
            this.OilLeaksImgList = new ObservableCollection<ImageCapture>();
            this.HPSImgList = new ObservableCollection<ImageCapture>();

        }

        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CMechanicalCond>(x => x.VehicleInsRecID == vehicleInsRecID);
        }

        private string engineComment;

        public string EngineComment
        {
            get { return engineComment; }

            set { SetProperty(ref  engineComment, value); }
        }
        private bool isEngineDmg;

        public bool IsEngineDmg
        {
            get { return isEngineDmg; }

            set
            {
                if (SetProperty(ref  isEngineDmg, value) && !this.IsEngineDmg)
                {
                    this.EngineImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> engineImgList;

        [Ignore, DamageSnapshotRequired("Engine snapshot(s) required", "IsEngineDmg")]
        public ObservableCollection<ImageCapture> EngineImgList
        {
            get { return engineImgList; }

            set { SetProperty(ref  engineImgList, value); }
        }

        private string frontSuspComment;

        public string FrontSuspComment
        {
            get { return frontSuspComment; }

            set { SetProperty(ref  frontSuspComment, value); }
        }
        private bool isFrontSuspDmg;

        public bool IsFrontSuspDmg
        {
            get { return isFrontSuspDmg; }

            set
            {
                if (SetProperty(ref  isFrontSuspDmg, value) && !this.IsFrontSuspDmg)
                {
                    this.FrontSuspImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> frontSuspImgList;

        [Ignore, DamageSnapshotRequired("Front susp snapshot(s) required", "IsFrontSuspDmg")]
        public ObservableCollection<ImageCapture> FrontSuspImgList
        {
            get { return frontSuspImgList; }

            set { SetProperty(ref  frontSuspImgList, value); }
        }

        private string rearSuspComment;

        public string RearSuspComment
        {
            get { return rearSuspComment; }

            set { SetProperty(ref  rearSuspComment, value); }
        }
        private bool isRearSuspDmg;

        public bool IsRearSuspDmg
        {
            get { return isRearSuspDmg; }

            set
            {
                if (SetProperty(ref  isRearSuspDmg, value) && !this.IsRearSuspDmg)
                {
                    this.RearSuspImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> rearSuspImgList;

        [Ignore, DamageSnapshotRequired("Rear susp snapshot(s) required", "IsRearSuspDmg")]
        public ObservableCollection<ImageCapture> RearSuspImgList
        {
            get { return rearSuspImgList; }

            set { SetProperty(ref  rearSuspImgList, value); }
        }

        private string steeringComment;

        public string SteeringComment
        {
            get { return steeringComment; }

            set { SetProperty(ref  steeringComment, value); }
        }
        private bool isSteeringDmg;

        public bool IsSteeringDmg
        {
            get { return isSteeringDmg; }

            set
            {
                if (SetProperty(ref  isSteeringDmg, value) && !this.IsSteeringDmg)
                {
                    this.SteeringImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> steeringImgList;

        [Ignore, DamageSnapshotRequired("Steering snapshot(s) required", "IsSteeringDmg")]
        public ObservableCollection<ImageCapture> SteeringImgList
        {
            get { return steeringImgList; }

            set { SetProperty(ref  steeringImgList, value); }
        }


        private string exhaustComment;

        public string ExhaustComment
        {
            get { return exhaustComment; }

            set { SetProperty(ref  exhaustComment, value); }
        }
        private bool isExhaustDmg;

        public bool IsExhaustDmg
        {
            get { return isExhaustDmg; }

            set
            {
                if (SetProperty(ref  isExhaustDmg, value) && !this.IsExhaustDmg)
                {
                    this.ExhaustImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> exhaustImgList;

        [Ignore, DamageSnapshotRequired("Exhaust snapshot(s) required", "IsExhaustDmg")]
        public ObservableCollection<ImageCapture> ExhaustImgList
        {
            get { return exhaustImgList; }

            set { SetProperty(ref  exhaustImgList, value); }
        }

        private string batteryComment;

        public string BatteryComment
        {
            get { return batteryComment; }

            set { SetProperty(ref  batteryComment, value); }
        }
        private bool isBatteryDmg;

        public bool IsBatteryDmg
        {
            get { return isBatteryDmg; }

            set
            {
                if (SetProperty(ref  isBatteryDmg, value) && !this.IsBatteryDmg)
                {
                    this.BatteryImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> batteryImgList;

        [Ignore, DamageSnapshotRequired("Battery snapshot(s) required", "IsBatteryDmg")]
        public ObservableCollection<ImageCapture> BatteryImgList
        {
            get { return batteryImgList; }

            set { SetProperty(ref  batteryImgList, value); }
        }

        private string handBrakeComment;

        public string HandBrakeComment
        {
            get { return handBrakeComment; }

            set { SetProperty(ref  handBrakeComment, value); }
        }
        private bool isHandBrakeDmg;

        public bool IsHandBrakeDmg
        {
            get { return isHandBrakeDmg; }

            set
            {
                if (SetProperty(ref  isHandBrakeDmg, value) && !this.IsHandBrakeDmg)
                {
                    this.HandBrakeImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> handBrakeImgList;

        [Ignore, DamageSnapshotRequired("Hand brake snapshot(s) required", "IsHandBrakeDmg")]
        public ObservableCollection<ImageCapture> HandBrakeImgList
        {
            get { return handBrakeImgList; }

            set { SetProperty(ref  handBrakeImgList, value); }
        }

        private string footBrakeComment;

        public string FootBrakeComment
        {
            get { return footBrakeComment; }

            set { SetProperty(ref  footBrakeComment, value); }
        }
        private bool isFootBrakeDmg;

        public bool IsFootBrakeDmg
        {
            get { return isFootBrakeDmg; }

            set
            {
                if (SetProperty(ref  isFootBrakeDmg, value) && !this.IsFootBrakeDmg)
                {
                    this.FootBrakeImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> footBrakeImgList;

        [Ignore, DamageSnapshotRequired("Foot brake snapshot(s) required", "IsFootBrakeDmg")]
        public ObservableCollection<ImageCapture> FootBrakeImgList
        {
            get { return footBrakeImgList; }

            set { SetProperty(ref  footBrakeImgList, value); }
        }


        private string gearboxComment;

        public string GearboxComment
        {
            get { return gearboxComment; }

            set { SetProperty(ref  gearboxComment, value); }
        }
        private bool isGearboxDmg;

        public bool IsGearboxDmg
        {
            get { return isGearboxDmg; }

            set
            {
                if (SetProperty(ref  isGearboxDmg, value) && !this.IsGearboxDmg)
                {
                    this.GearboxImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> gearboxImgList;

        [Ignore, DamageSnapshotRequired("Gearbox snapshot(s) required", "IsGearboxDmg")]
        public ObservableCollection<ImageCapture> GearboxImgList
        {
            get { return gearboxImgList; }

            set { SetProperty(ref  gearboxImgList, value); }
        }

        private string differentialComment;

        public string DifferentialComment
        {
            get { return differentialComment; }

            set { SetProperty(ref  differentialComment, value); }
        }
        private bool isDifferentialDmg;

        public bool IsDifferentialDmg
        {
            get { return isDifferentialDmg; }

            set
            {
                if (SetProperty(ref  isDifferentialDmg, value) && !this.IsDifferentialDmg)
                {
                    this.DifferentialImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> differentialImgList;

        [Ignore, DamageSnapshotRequired("Differential snapshot(s) required", "IsDifferentialDmg")]
        public ObservableCollection<ImageCapture> DifferentialImgList
        {
            get { return differentialImgList; }

            set { SetProperty(ref  differentialImgList, value); }
        }

        private string autoTransmissionComment;

        public string AutoTransmissionComment
        {
            get { return autoTransmissionComment; }

            set { SetProperty(ref  autoTransmissionComment, value); }
        }
        private bool isAutoTransmissionDmg;

        public bool IsAutoTransmissionDmg
        {
            get { return isAutoTransmissionDmg; }

            set
            {
                if (SetProperty(ref  isAutoTransmissionDmg, value) && !this.IsAutoTransmissionDmg)
                {
                    this.AutoTransmissionImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> autoTransmissionImgList;

        [Ignore, DamageSnapshotRequired("Auto transmission snapshot(s) required", "IsAutoTransmissionDmg")]
        public ObservableCollection<ImageCapture> AutoTransmissionImgList
        {
            get { return autoTransmissionImgList; }

            set { SetProperty(ref  autoTransmissionImgList, value); }
        }

        private string oilLeaksComment;

        public string OilLeaksComment
        {
            get { return oilLeaksComment; }

            set { SetProperty(ref  oilLeaksComment, value); }
        }
        private bool isOilLeaksDmg;

        public bool IsOilLeaksDmg
        {
            get { return isOilLeaksDmg; }

            set
            {
                if (SetProperty(ref  isOilLeaksDmg, value) && !this.IsOilLeaksDmg)
                {
                    this.OilLeaksImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> oilLeaksImgList;

        [Ignore, DamageSnapshotRequired("Oil leaks snapshot(s) required", "IsOilLeaksDmg")]
        public ObservableCollection<ImageCapture> OilLeaksImgList
        {
            get { return oilLeaksImgList; }

            set { SetProperty(ref  oilLeaksImgList, value); }
        }

        private string hPSComment;

        public string HPSComment
        {
            get { return hPSComment; }

            set { SetProperty(ref  hPSComment, value); }
        }
        private bool isHPSDmg;

        public bool IsHPSDmg
        {
            get { return isHPSDmg; }

            set
            {
                if (SetProperty(ref  isHPSDmg, value) && !this.IsHPSDmg)
                {
                    this.HPSImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> hPSImgList;

        [Ignore, DamageSnapshotRequired("HPS snapshot(s) required", "IsHPSDmg")]
        public ObservableCollection<ImageCapture> HPSImgList
        {
            get { return hPSImgList; }

            set { SetProperty(ref  hPSImgList, value); }
        }


        private bool isRunningDmg;

        public bool IsRunning
        {
            get { return isRunningDmg; }

            set
            {
                if (SetProperty(ref  isRunningDmg, value) && !this.IsRunning)
                {

                }
            }
        }

        private bool hasPetrol;

        public bool HasPetrol
        {
            get { return hasPetrol; }

            set { SetProperty(ref  hasPetrol, value); }
        }

        private bool hasDiesel;

        public bool HasDiesel
        {
            get { return hasDiesel; }

            set { SetProperty(ref  hasDiesel, value); }
        }

        private bool hasLPG;

        public bool HasLPG
        {
            get { return hasLPG; }

            set { SetProperty(ref  hasLPG, value); }
        }

        private bool hasMainBattery;

        public bool HasMainBattery
        {
            get { return hasMainBattery; }

            set { SetProperty(ref  hasMainBattery, value); }
        }

        public string engineImgPathList;
        public string EngineImgPathList
        {
            get { return string.Join("~", EngineImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref engineImgPathList, value); }
        }

        public string rearSuspImgPathList;
        public string RearSuspImgPathList
        {
            get { return string.Join("~", RearSuspImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rearSuspImgPathList, value); }
        }

        public string frontSuspImgPathList;
        public string FrontSuspImgPathList
        {
            get { return string.Join("~", FrontSuspImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref frontSuspImgPathList, value); }
        }

        public string steeringImgPathList;
        public string SteeringImgPathList
        {
            get { return string.Join("~", SteeringImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref steeringImgPathList, value); }
        }

        public string exhaustImgPathList;
        public string ExhaustImgPathList
        {
            get { return string.Join("~", ExhaustImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref exhaustImgPathList, value); }
        }

        public string batteryImgPathList;
        public string BatteryImgPathList
        {
            get { return string.Join("~", BatteryImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref batteryImgPathList, value); }
        }

        public string handBrakeImgPathList;
        public string HandBrakeImgPathList
        {
            get { return string.Join("~", HandBrakeImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref handBrakeImgPathList, value); }
        }

        public string footBrakeImgPathList;
        public string FootBrakeImgPathList
        {
            get { return string.Join("~", FootBrakeImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref footBrakeImgPathList, value); }
        }

        public string gearboxImgPathList;
        public string GearboxImgPathList
        {
            get { return string.Join("~", GearboxImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gearboxImgPathList, value); }
        }

        public string differentialImgPathList;
        public string DifferentialImgPathList
        {
            get { return string.Join("~", DifferentialImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref differentialImgPathList, value); }
        }

        public string autoTransmissionImgPathList;
        public string AutoTransmissionImgPathList
        {
            get { return string.Join("~", AutoTransmissionImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref autoTransmissionImgPathList, value); }
        }

        public string oilLeaksImgPathList;
        public string OilLeaksImgPathList
        {
            get { return string.Join("~", OilLeaksImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref oilLeaksImgPathList, value); }
        }

        public string hPSImgPathList;
        public string HPSImgPathList
        {
            get { return string.Join("~", HPSImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref hPSImgPathList, value); }
        }
    }
}
