using Pithline.FMS.BusinessLogic.Enums;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic
{
    [Table("Vehicle")]
    public class Vehicle : ValidatableBindableBase
    {
        
        private string registrationNumber;
        [SQLite.Column("RegistrationNumber"), PrimaryKey, SQLite.Unique,]
        [RestorableState]
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
        }

        private string color;
        [Column("Color")]
        [RestorableState]
        public string Color
        {
            get { return color; }
            set { SetProperty(ref color, value); }
        }

        private string chassisNumber;
        [Column("ChassisNumber"),]
        [RestorableState]
        public string ChassisNumber
        {
            get { return chassisNumber; }
            set { SetProperty(ref chassisNumber, value); }
        }


        private DateTime modelYear;
        [Column("ModelYear")]
        [RestorableState]
        public DateTime ModelYear
        {
            get { return modelYear; }
            set { SetProperty(ref modelYear, value); }
        }

        private VehicleTypeEnum vehicleType;
        [Column("VehicleType")]
        [RestorableState]
        public VehicleTypeEnum VehicleType
        {
            get { return vehicleType; }
            set { SetProperty(ref vehicleType, value); }
        }

    }
}
