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

namespace Zoom.Net
{
    /// <summary>
    /// The Connection class supports methods for instantiation and
    /// searching of targets, together with the housekeeping and option 
    /// management methods provided on all classes.
    /// </summary>
    /// <remarks>
    /// Connection
    /// or an ``application association'', as the OSI terminology in the
    /// standard has it - represents an ongoing dialogue between the
    /// client (``origin'' in the standard) and server (``target''). A
    /// connection is forged by the act of creating a Connection object;
    /// and initialization is performed implicitly, so that the new object
    /// is ready to be used immediately. 
    /// </remarks>
    /// <remarks>
    /// Search and Scan operations are supported.
    /// </remarks>
    /// <remarks>
    /// IConnection creation without specifying an actual connection target
    /// is not supported.
    /// </remarks>
    /// <remarks>See also Zoom.Net::IConnectionExtended interface 
    /// and Zoom.Net::YazSharp::Connection implementation class</remarks>
    public interface IConnection : IDisposable
    {
        /// <summary>
        /// Submitting a Query to a Connection. The resultset is held 
        /// on the server.
        /// </summary>
        /// <param name="query">
        /// The query is either a PQF or a CQL query.
        /// </param>
        IResultSet Search(IQuery query);
        
        /// <summary>
        /// Submitting a Scan to a Connection. The scanset is held 
        /// on the server.
        /// </summary>
        /// <param name="query">
        /// The scan query is a subset of PQF, namely the 
        /// Attributes+Term part.
        /// </param>
        IScanSet Scan(IPrefixQuery query);
        
        /// <summary>
        /// Setting and getting the databaseName option
        /// </summary>
        string DatabaseName
        {
            get;
            set;
        }
        
        /// <summary>
        /// Setting and getting the username option
        /// </summary>
        string Username
        {
            get;
            set;
        }
        
        /// <summary>
        /// Setting and getting the password option
        /// </summary>
        string Password
        {
            get;
            set;
        }
        
        /// <summary>
        /// Setting and getting recordSyntax option
        /// </summary>
        RecordSyntax Syntax
        {
            get;
            set;
        }
        
        /// <summary>
        /// Other standard options described at 
        /// http://zoom.z3950.org/api/zoom-1.4.html#3.8 are
        /// implemented using the Zoom::Net::IConnectionOptions
        /// interface.
        /// </summary>
        /// <remarks>
        /// See the following info for all possible values:   
        /// http://www.indexdata.dk/yaz/doc/zoom.tkl#zoom.connections
        /// </remarks>
        IConnectionOptionsCollection Options
        {
            get;
        }
    }
}
