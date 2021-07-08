using Dapper.Contrib.Extensions;
using Oyshroom.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Z.Dapper.Plus;


namespace Oyshroom.Data
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection GetConnection();
    }

    public abstract class Repository<T> where T : class
    {
        private protected Database _database;
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public Repository(Database database)
        {
            _database = database;
        }

        public Repository(IDatabaseConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

  
        public long Add(T item)
        {
            long id = 0;

            using (var connection = _database.Connection())
            {
                id = connection.Insert<T>(item); 
            }

            return id;
        }

        public void Add(List<T> list)
        {
            using (var connection = _database.Connection())
            {
                connection.Insert(list);
            }
        }


        public List<T> Get()
        {
            using (var connection = _database.Connection())
            {
                return connection.GetAll<T>().ToList();
            }
        }

        public T GetById(int id)
        {
            using (var connection = _database.Connection())
            {
               return connection.Get<T>(id);
            }
        }

      
    }

    public class StableTemperatureRepository : Repository<LatestTemperature>
    {
        public StableTemperatureRepository(Database database): base(database) 
        {

        }

    }
}
