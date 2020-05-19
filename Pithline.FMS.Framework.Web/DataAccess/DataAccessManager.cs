using Pithline.FMS.Framework.Web.Caching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Pithline.FMS.Framework.Web.DataAccess
{
    [Export]
    public class DataAccessManager
    {
        [ImportMany(AllowRecomposition = true)]
        public List<Lazy<IDataProvider, IDataProviderMetadata>> Providers { get; set; }

        [Import]
        public CacheManager Cache { get; set; }

        #region Private Members
        private IDataProvider FindProvider(string provider)
        {
            var prov = (from p in this.Providers where p.Metadata.Name.Equals(provider) select p).FirstOrDefault();
            return (prov != null) ? prov.Value : null;
        }

        private string GenerateKey(string provider, object[] param)
        {
            var key = new StringBuilder();
            key.Append(provider);
            key.Append("_");
            if (param != null)
                foreach (var p in param)
                {
                    key.Append(p);
                    key.Append("_");
                }

            return key.ToString();
        }
        #endregion

        /// <summary>
        /// Get the item list based on provider and paramters
        /// </summary>
        /// <param name="provider">the provider name, the name that has been used in dataprovider attribute</param>
        /// <param name="param">the list of params used</param>
        /// <param name="isCached">whether the data should be cached or not</param>
        /// <returns></returns>
        public IList GetList(string provider, object[] param, bool isCached)
        {
            if (isCached)
            {
                var key = this.GenerateKey(provider, param);
                var value = this.Cache[key];
                if (value != null)
                {
                    return (IList)value;
                }
                else
                {
                    var pro = this.FindProvider(provider);
                    var list = pro.GetDataList(param);
                    this.Cache[key] = list;
                    return list;
                }
            }
            else
            {
                var pro = this.FindProvider(provider);
                return (pro != null) ? pro.GetDataList(param) : null;
            }
        }

        public object GetSingle(string provider, object[] param, bool isCached)
        {
            if (isCached)
            {
                var key = this.GenerateKey(provider, param);
                var value = this.Cache[key];
                if (value != null)
                {
                    return value;
                }
                else
                {
                    var pro = this.FindProvider(provider);
                    var elem = pro.GetSingleData(param);
                    this.Cache[key] = elem;
                    return elem;
                }
            }
            else
            {
                var pro = this.FindProvider(provider);
                return (pro != null) ? pro.GetSingleData(param) : null;
            }
        }

        public object Save(string provider, object[] param)
        {
            var pro = this.FindProvider(provider);
            return (pro != null) ? pro.SaveData(param) : false;
        }

        public bool Delete(string provider, object[] param)
        {
            var pro = this.FindProvider(provider);
            return (pro != null) ? pro.DeleteData(param) : false;
        }

        public object CallServiceMethod(string provider, string methodname, object[] parameters)
        {
            var pro = this.FindProvider(provider);
            if (pro != null)
            {
                var obj = pro.GetService();
                var method = obj.GetType().GetMethod(methodname);
                if (method != null)
                {
                    return method.Invoke(obj, parameters);
                }
                else
                {
                    return null;
                }
            }
            else return null;
        }
    }
}
