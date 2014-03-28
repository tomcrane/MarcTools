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
using System.Collections.Specialized;
using System.Configuration;

namespace Zoom.Net
{
    internal class Config
    {
        public static string Get(string key)
        {
            NameValueCollection settings 
                = (NameValueCollection) ConfigurationSettings.GetConfig("Zoom.Net.Factory");
            
            if (settings == null){
                string message
                    = "\n\nZoom.Net was unable to load the "
                    + "'Zoom.Net.Factory' settings\n"
                    + "This is usually because the Zoom.Net.Factory section "
                    + "is missing \n"
                    + "from the application configuration.";
                throw new ConfigurationException(message);
            }
            string setting = settings[key];
            if (setting == null || setting ==  string.Empty){
                string message
                    = "\n\nZoom.Net was unable to resolve the "
                    + "'Zoom.Net.Factory' setting\n'"
                    + key + "'\n"
                    + "This is usually because the Zoom.Net.Factory section "
                    + "of the application\n"
                    + "configuration is missing this key.";
                throw new ConfigurationException(message);
            }

 
            return setting;
        }
    }
}
