using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Pithline.FMS.BusinessLogic.Helpers
{
    public sealed class SqliteHelper
    {
        private SqliteHelper()
        {


        }

        private static readonly SqliteHelper instance = new SqliteHelper();

        public static SqliteHelper Storage
        {
            get { return instance; }

        }

        private SQLite.SQLiteAsyncConnection connection;

        public SQLite.SQLiteAsyncConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        public async void ConnectionDatabaseAsync()
        {
            if (connection == null)
            {
                var db = await ApplicationData.Current.RoamingFolder.GetFileAsync("SQLiteDB\\Pithline.FMSmobility.sqlite");
                connection = new SQLite.SQLiteAsyncConnection(db.Path);
            }
        }

        public async Task<int> DropTableAsync<T>() where T : new()
        {
            return await this.Connection.DropTableAsync<T>();
        }

        public Task<List<T>> LoadTableAsync<T>()
            where T : ValidatableBindableBase, new()
        {
            try
            {
                return this.Connection.Table<T>().ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<T> GetSingleRecordAsync<T>(Expression<Func<T, bool>> criteria)
            where T : ValidatableBindableBase, new()
        {
            return this.connection.Table<T>().Where(criteria).FirstOrDefaultAsync();            
        }

        public async Task<int> InsertAllAsync<T>(IEnumerable<T> items) where T : new()
        {
            try
            {
                return await this.Connection.InsertAllAsync(items);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> UpdateAllAsync<T>(IEnumerable<T> items) where T : new()
        {
            return await this.Connection.UpdateAllAsync(items);
        }

        public async Task<int> InsertSingleRecordAsync<T>(T obj)
        {
            try
            {
                return await this.Connection.InsertAsync(obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteSingleRecordAsync<T>(T obj)
        {
            try
            {
                return await this.Connection.DeleteAsync(obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteBulkAsync<T>(IEnumerable<T> obj)
        {
            try
            {
                return await this.Connection.DeleteBulkAsync(obj);
            }
            catch (Exception)
            {

                throw;
            }
        }

        async public Task<int> UpdateSingleRecordAsync<T>(T obj)
        {
            try
            {
                return await this.Connection.UpdateAsync(obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CreateTablesResult> CreateTableAsync<T>() where T:new()
        {
           return await this.Connection.CreateTableAsync<T>();
        }

        public async System.Threading.Tasks.Task DropnCreateTableAsync<T>()where T:new()
        {
            await this.Connection.DropTableAsync<T>();
            await this.Connection.CreateTableAsync<T>();
        }
    }
}
