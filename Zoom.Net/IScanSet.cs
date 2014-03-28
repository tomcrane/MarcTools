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
    /// The Scan Set 
    /// represents a set of potential query terms which a server suggests
    /// may be useful. In some cases, the terms are accompanied by hit
    /// counts.
    /// </summary>
    /// Term supported. In Zoom::Net the return values of the Term
    /// operation are ScanTerm objects rather than strings, which is a
    /// logical extension of the similar functionality in Record sets
    /// and Record objects. This is not forseen in the Zoom specifications.
    /// <remarks>
    /// </remarks>
    /// <remarks>
    /// Display strings for use in client are not supported.
    /// </remarks>
    /// <remarks> 
    /// See also the following Class/Interface definitions
    /// Zoom.Net::IScanTerm and Zoom.Net::YazSharp::ScanTerm 
    /// </remarks>
    /// <remarks>
    /// See also http://www.indexdata.dk/yaz/doc/zoom.scan.tkl
    /// </remarks>
    public interface IScanSet : IDisposable
    {
        /// <summary>
        /// Getting the n-th Scan Term
        /// </summary>
        IScanTerm this[uint index]
        {
            get;
        }
		
        /// <summary>
        /// Getting the size of the Scan Set in number of terms
        /// </summary>
        uint Size
        {
            get;
        }
    }
}
