/*
 * Copyright (c) 2005, Talis Information Limited.
 *
 * Permission to use, copy, modify, distribute, and sell this software and
 * its documentation, in whole or in part, for any purpose, is hereby granted,
 * provided that:
 *
 * 1. This copyright and permission notice appear in all copies of the
 * software and its documentation. Notices of copyright or attribution
 * which appear at the beginning of any file must remain unchanged.
 *
 * 2. The names of BLCMP, Talis Information Limited or the individual authors
 * may not be used to endorse or promote products derived from this software
 * without specific prior written permission.
 *
 * 3. Users of this software agree to make their best efforts, when
 * documenting their use of the software, to acknowledge Zoom.Net
 * and the role played by the software in their work.
 *
 * THIS SOFTWARE IS PROVIDED "AS IS" AND WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS, IMPLIED, OR OTHERWISE, INCLUDING WITHOUT LIMITATION, ANY
 * WARRANTY OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
 * IN NO EVENT SHALL INDEX DATA BE LIABLE FOR ANY SPECIAL, INCIDENTAL,
 * INDIRECT OR CONSEQUENTIAL DAMAGES OF ANY KIND, OR ANY DAMAGES
 * WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER OR
 * NOT ADVISED OF THE POSSIBILITY OF DAMAGE, AND ON ANY THEORY OF
 * LIABILITY, ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE
 * OF THIS SOFTWARE.
 *
 */
using System;

namespace Zoom.Net.YazSharp
{
	public class Connection : IDisposable, IConnection
	{
		static Connection()
		{
		}

		public Connection(string host, int port)
		{
			_host = host;
			_port = port;

			_options = new ConnectionOptionsCollection();
			_zoomConnection = Yaz.ZOOM_connection_create(_options._zoomOptions);

			int errorCode = Yaz.ZOOM_connection_errcode(_zoomConnection);
			CheckErrorCodeAndThrow(errorCode);
			//Yaz.yaz_log(Yaz.LogLevel.LOG, "Connection Created");
		}

		~Connection()
		{
                    //Yaz.yaz_log(Yaz.LogLevel.LOG, "Connection Destroyed");
			Dispose();
		}

		private string _host;
		private int _port;

		private void CheckErrorCodeAndThrow(int errorCode)
		{
			string message;
			switch (errorCode)
			{
					
				case Yaz.ZOOM_ERROR_NONE:
					break;

				case Yaz.ZOOM_ERROR_CONNECT:
					message = string.Format("Connection could not be made to {0}:{1}", _host, _port);
					throw new ConnectionUnavailableException(message);
				
				case Yaz.ZOOM_ERROR_INVALID_QUERY:
					message = string.Format("The query requested is not valid or not supported");
					throw new InvalidQueryException(message);

				case Yaz.ZOOM_ERROR_INIT:
					message = string.Format("Server {0}:{1} rejected our init request", _host, _port);
					throw new InitRejectedException(message);

				case Yaz.ZOOM_ERROR_TIMEOUT:
					message = string.Format("Server {0}:{1} timed out handling our request", _host, _port);
					throw new ConnectionTimeoutException(message);

				case Yaz.ZOOM_ERROR_MEMORY:
				case Yaz.ZOOM_ERROR_ENCODE:
				case Yaz.ZOOM_ERROR_DECODE:
				case Yaz.ZOOM_ERROR_CONNECTION_LOST:
				case Yaz.ZOOM_ERROR_INTERNAL:
				case Yaz.ZOOM_ERROR_UNSUPPORTED_PROTOCOL:
				case Yaz.ZOOM_ERROR_UNSUPPORTED_QUERY:
					message = Yaz.ZOOM_connection_errmsg(_zoomConnection);
					throw new ZoomImplementationException("A fatal error occurred in Yaz: " + errorCode + " - " + message);

				default:
					Bib1Diagnostic code = (Bib1Diagnostic) errorCode;
					throw new Bib1Exception(code, Enum.GetName(typeof(Bib1Diagnostic), code));
			}		
		}

		public IResultSet Search(IQuery query)
		{
			EnsureConnected();
			IntPtr yazQuery = Yaz.ZOOM_query_create();
                        ResultSet resultSet = null;

                        try {
                            // branching out to right YAZ-C call
                            if (query is ICQLQuery)
                                Yaz.ZOOM_query_cql(yazQuery, query.QueryString);
                            else if (query is IPrefixQuery)
                                Yaz.ZOOM_query_prefix(yazQuery, query.QueryString);
                            else
                                throw new NotImplementedException();

                            IntPtr yazResultSet = Yaz.ZOOM_connection_search(_zoomConnection, yazQuery);
                            // error checking C-style
                            int errorCode = Yaz.ZOOM_connection_errcode(_zoomConnection);
                            if (errorCode != Yaz.ZOOM_ERROR_NONE)
                                {
                                    Yaz.ZOOM_resultset_destroy(yazResultSet);
                                }
                            CheckErrorCodeAndThrow(errorCode);

                            // everything ok, create result set
                            resultSet = new ResultSet(yazResultSet, this);
                        }
                        finally {
                            // deallocate yazQuery also when exceptions
                            Yaz.ZOOM_query_destroy(yazQuery);
                            yazQuery = IntPtr.Zero;
                        }
                        return resultSet;
		}


            /*            
		public IResultSet Search(ICQLQuery query)
		{
			EnsureConnected();
			IntPtr yazQuery = Yaz.ZOOM_query_create();
                        Yaz.ZOOM_query_cql(yazQuery, query.QueryString);

			IntPtr yazResultSet = Yaz.ZOOM_connection_search(_zoomConnection, yazQuery);

			int errorCode = Yaz.ZOOM_connection_errcode(_zoomConnection);
			if (errorCode != Yaz.ZOOM_ERROR_NONE)
			{
				Yaz.ZOOM_resultset_destroy(yazResultSet);
			}
			Yaz.ZOOM_query_destroy(yazQuery);
			yazQuery = IntPtr.Zero;
			CheckErrorCodeAndThrow(errorCode);

			ResultSet resultSet = new ResultSet(yazResultSet, this);
			return resultSet;
		}

		public IResultSet Search(IPrefixQuery query)
		{
			EnsureConnected();
			IntPtr yazQuery = Yaz.ZOOM_query_create();

                        Yaz.ZOOM_query_prefix(yazQuery, query.QueryString);

			IntPtr yazResultSet = Yaz.ZOOM_connection_search(_zoomConnection, yazQuery);

			int errorCode = Yaz.ZOOM_connection_errcode(_zoomConnection);
			if (errorCode != Yaz.ZOOM_ERROR_NONE)
			{
				Yaz.ZOOM_resultset_destroy(yazResultSet);
			}
			Yaz.ZOOM_query_destroy(yazQuery);
			yazQuery = IntPtr.Zero;
			CheckErrorCodeAndThrow(errorCode);

			ResultSet resultSet = new ResultSet(yazResultSet, this);
			return resultSet;
		}
            
            */
		public IScanSet Scan(IPrefixQuery query)
		{
			EnsureConnected();
			IntPtr yazScanSet = Yaz.ZOOM_connection_scan(_zoomConnection, query.QueryString);

			int errorCode = Yaz.ZOOM_connection_errcode(_zoomConnection);
			if (errorCode != Yaz.ZOOM_ERROR_NONE)
			{
				Yaz.ZOOM_scanset_destroy(yazScanSet);
			}
			CheckErrorCodeAndThrow(errorCode);

			ScanSet scanSet = new ScanSet(yazScanSet, this);
			return scanSet;
		}

		private ConnectionOptionsCollection _options;
		public IConnectionOptionsCollection Options
		{
			get
			{
				return _options;
			}
		}

		protected IntPtr _zoomConnection;

		private bool _disposed = false;

		private bool _connected = false;

		protected void EnsureConnected()
		{
			if (!_connected)
				Connect();
		}

		public void Connect()
		{
			Yaz.ZOOM_connection_connect(_zoomConnection, _host, _port);
			int errorCode = Yaz.ZOOM_connection_errcode(_zoomConnection);
			CheckErrorCodeAndThrow(errorCode);
			_connected = true;
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				Yaz.ZOOM_connection_destroy(_zoomConnection);

				//Yaz.yaz_log(Yaz.LogLevel.LOG, "Connection Disposed");
				_zoomConnection = IntPtr.Zero;
				_disposed = true;
			}
		}
	
		public RecordSyntax Syntax
		{
			get
			{
				RecordSyntax syntax = (RecordSyntax) Enum.Parse(typeof(RecordSyntax), Options["preferredRecordSyntax"]);
				return syntax;
			}
			set
			{
				Options["preferredRecordSyntax"] = Enum.GetName(typeof(RecordSyntax), value);
			}
		}

		public string DatabaseName
		{
			get
			{
				return Options["databaseName"];
			}
			set
			{
				Options["databaseName"] = value;
			}
		}

		public string Username
		{
			get
			{
				return Options["user"];
			}
			set
			{
				Options["user"] = value;
			}
		}

		public string Password
		{
			get
			{
				return Options["password"];
			}
			set
			{
				Options["password"] = value;
			}
		}
	}
}
