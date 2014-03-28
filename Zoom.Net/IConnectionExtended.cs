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
    /// The ConnectionExtended class supports methods for instantiation and
    /// searching of targets, together with the housekeeping and option 
    /// management methods provided on all classes. It allows for extended 
    /// services
    /// </summary>
    /// <remarks>
    /// The interface IConnectionExtended can be used to search and scan
    /// exactly as IConnection interface implementations, but 
    /// extends it with a Package  Zoom.Net::IPackage factory call. 
    /// </remarks>
    /// <remarks>
    /// Search and Scan operations are supported.
    /// </remarks>
    /// <remarks>
    /// IConnectionExtended creation without specifying an actual 
    /// connection target is not supported.
    /// </remarks>
    /// <remarks>
    /// The exact range of Extended Services implemented on top of the
    /// ZOOM YAZ C API, including package types and package options, is
    /// described at http://www.indexdata.dk/yaz/doc/zoom.ext.tkl 
    /// </remarks>
    /// <remarks>
    /// See also Zoom.Net::IConnection interface 
    /// and Zoom.Net::YazSharp::ConnectionExtended implementation class.
    /// </remarks>
    /// <remarks>
    /// See also http://www.indexdata.dk/yaz/doc/zoom.ext.tkl
    /// </remarks>
    public interface IConnectionExtended : IConnection 
    {
        /// <summary>
        /// The factory call constructs Packages, of type 'itemorder', 
        /// 'update', or of one of the extention types 'create', 
        /// 'drop', 'commit'. 
        /// These packages can be configures through setting appropriate      
        /// Zoom.Net::IPackageOptionsCollection Options.
        /// </summary>
        IPackage Package(string type);
    }
}
