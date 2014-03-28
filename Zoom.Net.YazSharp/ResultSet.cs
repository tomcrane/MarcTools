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
using System.Collections;

namespace Zoom.Net.YazSharp
{
	public class ResultSet : IResultSet
	{
		internal ResultSet(IntPtr resultSet, Connection connection)
		{
			_connection = connection;
			_resultSet = resultSet;
			_size = Yaz.ZOOM_resultset_size(_resultSet);
                        if (0 == _size){
                            Console.Out.WriteLine("Yaz.ZOOM_resultset_size zero");
                        }
			_records = new Record[_size];
			//Yaz.yaz_log(Yaz.LogLevel.LOG, "ResultSet Created");
		}

		~ResultSet()
		{
			//Yaz.yaz_log(Yaz.LogLevel.LOG, "ResultSet Destroyed");
			((IDisposable)this).Dispose();
		}

            IResultSetOptionsCollection IResultSet.Options
            {
                get
                    {
                        return new ResultSetOptionsCollection(_resultSet);
                    }
            }
            

		IRecord IResultSet.this[uint index]
		{
			get
			{
				if (_records[index] == null)
				{
					IntPtr record = Yaz.ZOOM_resultset_record(_resultSet, index);
					_records[index] = new Record(record, this);
				}
				return _records[index];
			}
		}

		IRecord IResultSet.this[int index]
		{
			get
			{
				uint uindex = (uint) index;
				return ((IResultSet)this)[uindex];
			}
		}

		uint IResultSet.Size
		{
			get
			{
				return _size;
			}
		}

		private Connection _connection;
		private IntPtr _resultSet;
		private uint _size;
		private Record[] _records;

		private bool _disposed = false;
		void IDisposable.Dispose()
		{   
                    //Console.Out.WriteLine("IDisposable.Dispose {0} {1}",  
                    //                       this, this.GetHashCode());
                   
			if (!_disposed)
			{
				foreach (Record record in _records)
				{
					if (record != null)
						record.Dispose();
				}
				Yaz.ZOOM_resultset_destroy(_resultSet);
				//Yaz.yaz_log(Yaz.LogLevel.LOG, "ResultSet Disposed");
				_connection = null;
				_resultSet = IntPtr.Zero;
				_disposed = true;
			}
		}

		#region IList Members

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return ((IResultSet)this)[(uint)index];
			}
			set
			{
				throw new NotImplementedException("Underlying ResultSet is readonly");
			}
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException("Underlying ResultSet is readonly");
		}

		public void Insert(int index, object value)
		{
			throw new NotImplementedException("Underlying ResultSet is readonly");
		}

		public void Remove(object value)
		{
			throw new NotImplementedException("Underlying ResultSet is readonly");
		}

		public bool Contains(object value)
		{
			throw new NotImplementedException("Underlying ResultSet is not searchable");
		}

		public void Clear()
		{
			throw new NotImplementedException("Underlying ResultSet is readonly");
		}

		public int IndexOf(object value)
		{
			throw new NotImplementedException("Underlying ResultSet is not searchable");
		}

		public int Add(object value)
		{
			throw new NotImplementedException("Underlying ResultSet is readonly");
		}

		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public int Count
		{
			get
			{
				return (int) ((IResultSet)this).Size;
			}
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException("Underlying ResultSet is not copyable");
		}

		public object SyncRoot
		{
			get
			{
				throw new NotImplementedException("Underlying ResultSet is not synchronised");
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return new ResultSetEnumerator(this);
		}

		#endregion
	}
}
