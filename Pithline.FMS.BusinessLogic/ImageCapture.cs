using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Pithline.FMS.BusinessLogic
{

    public class ImageCapture : ValidatableBindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        private string imagePath;
        [RestorableState]
        public string ImagePath
        {
            get { return imagePath; }
            set { SetProperty(ref imagePath, value); }
        }


        private string imageBinary;

        public string ImageBinary
        {
            get { return imageBinary; }
            set { SetProperty(ref imageBinary, value); }
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { SetProperty(ref fileName, value); }
        }

        private long caseServiceRecId;
        public long CaseServiceRecId
        {
            get { return caseServiceRecId; }
            set { SetProperty(ref caseServiceRecId, value); }
        }

        private long primeId;
        /// <summary>
        /// RepaireId
        /// </summary>
        public long PrimeId
        {
            get { return primeId; }
            set { SetProperty(ref primeId, value); }
        }

        private bool isSynced;

        public bool IsSynced
        {
            get { return isSynced; }
            set { SetProperty(ref isSynced, value); }
        }


    }
}
