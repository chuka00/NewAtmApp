﻿using ATM.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DAL
{
    public class ATMDBContext : IDisposable
    {

        private readonly string _connString;

        private bool _disposed;

        private SqlConnection _dbConnection = null;

        public ATMDBContext() : this(@"Data Source=.;Initial Catalog=BzAtmApp;Integrated Security=True")
        {

        }

        public ATMDBContext(string connString)
        {
            _connString = connString;
        }

        public async Task<SqlConnection> OpenConnection()
        {
            _dbConnection = new SqlConnection(_connString);
            await _dbConnection.OpenAsync();
            return _dbConnection;
        }

        public async Task CloseConnection()
        {
            if (_dbConnection?.State != ConnectionState.Closed)
            {
                await _dbConnection?.CloseAsync();
            }
        }
        protected virtual void Dispose(bool disposing)


        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbConnection.Dispose();
            }

            _disposed = true;
        }
        public void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
