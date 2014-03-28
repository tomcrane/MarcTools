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
	public class Package : IPackage
	{
		internal Package(IntPtr pack, 
                                 ConnectionExtended connection,
                                 string type)
		{
                    _type = type;
                    _connection = connection;
                    _package = pack;
                    //Yaz.yaz_log(Yaz.LogLevel.LOG, "Package Created");
		}

            ~Package()
		{
                    //Yaz.yaz_log(Yaz.LogLevel.LOG, "Package Destroyed");
			((IDisposable)this).Dispose();
		}

            IPackageOptionsCollection IPackage.Options
            {
                get
                    {
                        return new PackageOptionsCollection(_package);
                    }
            }

            
            void IPackage.Send()
		{ 
                    Yaz.ZOOM_package_send(_package, _type);
                }

            private string _type;
            private ConnectionExtended _connection;
            private IntPtr _package = IntPtr.Zero;
	
            void IDisposable.Dispose()
            {
                if (IntPtr.Zero != _package){
                    Yaz.ZOOM_package_destroy(_package);
                    _connection = null;
                    _package = IntPtr.Zero;
                }
            }
	}
}
