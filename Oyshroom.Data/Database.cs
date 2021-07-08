using Microsoft.Data.Sqlite;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Oyshroom.Data
{
    public class Database
    {
        private readonly string _dbFile;

        public Database()
        {

        }

        public Database(string filePath)
        {
            _dbFile = filePath;
        }

        public SqliteConnection Connection()
        {
            return new SqliteConnection($"Data Source={_dbFile}");
        }


    }
}
