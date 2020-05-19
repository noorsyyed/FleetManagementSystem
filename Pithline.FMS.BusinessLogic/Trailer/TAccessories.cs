using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic
{
    public class TAccessories : BaseModel
    {
        public TAccessories()
        {
            this.DecalSignWritingImgList = new ObservableCollection<ImageCapture>();
            this.ReflectiveTapeImgList = new ObservableCollection<ImageCapture>();
        }

        private string decalSignWritingComment;

        public string DecalSignWritingComment
        {
            get { return decalSignWritingComment; }

            set { SetProperty(ref  decalSignWritingComment, value); }
        }
        private bool isDecalSignWritingDmg;

        public bool IsDecalSignWritingDmg
        {
            get { return isDecalSignWritingDmg; }

            set
            {
                if (SetProperty(ref  isDecalSignWritingDmg, value) && !this.IsDecalSignWritingDmg)
                {
                    this.DecalSignWritingImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> decalSignWritingImgList;

        [Ignore, DamageSnapshotRequired("Decal sign writing snapshot(s) required", "IsDecalSignWritingDmg")]
        public ObservableCollection<ImageCapture> DecalSignWritingImgList
        {
            get { return decalSignWritingImgList; }

            set { SetProperty(ref  decalSignWritingImgList, value); }
        }

        private string reflectiveTapeComment;

        public string ReflectiveTapeComment
        {
            get { return reflectiveTapeComment; }

            set { SetProperty(ref  reflectiveTapeComment, value); }
        }
        private bool isReflectiveTapeDmg;

        public bool IsReflectiveTapeDmg
        {
            get { return isReflectiveTapeDmg; }

            set
            {
                if (SetProperty(ref  isReflectiveTapeDmg, value) && !this.IsReflectiveTapeDmg)
                {
                    this.ReflectiveTapeImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> reflectiveTapeImgList;

        [Ignore, DamageSnapshotRequired("Reflective tape snapshot(s) required", "IsReflectiveTapeDmg")]
        public ObservableCollection<ImageCapture> ReflectiveTapeImgList
        {
            get { return reflectiveTapeImgList; }

            set { SetProperty(ref  reflectiveTapeImgList, value); }
        }


        public string decalSignWritingImgPathList;
        public string DecalSignWritingImgPathList
        {
            get { return string.Join("~", DecalSignWritingImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref decalSignWritingImgPathList, value); }
        }

        public string reflectiveTapeImgPathList;
        public string ReflectiveTapeImgPathList
        {
            get { return string.Join("~", ReflectiveTapeImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref reflectiveTapeImgPathList, value); }
        }

        private bool hasDecalSignWritingDmg;
        public bool HasDecalSignWritingDmg
        {
            get { return hasDecalSignWritingDmg; }

            set { SetProperty(ref  hasDecalSignWritingDmg, value); }
        }

        private bool hasReflectiveTapeDmg;
        public bool HasReflectiveTapeDmg
        {
            get { return hasReflectiveTapeDmg; }

            set { SetProperty(ref  hasReflectiveTapeDmg, value); }
        }
        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<TAccessories>(x => x.VehicleInsRecID == vehicleInsRecID);
        }
    }
}
