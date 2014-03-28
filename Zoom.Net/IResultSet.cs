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

namespace Zoom.Net
{
    /// <summary>
    /// The Result Set
    /// represents a set of records, held on a server, which have been
    /// identified by searching (that is, submitting a Query to a
    /// Connection). 
    /// </summary>
    /// <remarks>
    /// The Result Set class supports methods for discovering the number
    /// of its records, and fetching records either one by one or all at
    /// once. 
    /// </remarks>
    /// <remarks>
    /// Result Sets essentially are a collection of Records, which can be
    /// fetched individually by the '[]' operator.
    /// </remarks>
    /// <remarks>
    /// The size operation is supported. 
    /// </remarks>
    /// <remarks>
    /// See also
    /// Zoom::Net.IResultSetOptionsCollection interface, and
    /// Zoom.Net::YazSharp::ResultSet implementation class.
    /// </remarks>
    public interface IResultSet : IDisposable, IList
    {
            
        /// <summary>
        /// Getting the IResultSetOptionsCollection options
        /// </summary>
        IResultSetOptionsCollection Options
        {
            get;
        }
            

        /// <summary>
        /// Fetching a record
        /// </summary>
        IRecord this[uint index]
        {
            get;
        }

        /// <summary>
        /// Fetching a record
        /// </summary>
        new IRecord this[int index]
        {
            get;
        }

        /// <summary>
        /// Get size of Result Set in number of Records
        /// </summary>
        uint Size
        {
            get;
        }
    }
}
