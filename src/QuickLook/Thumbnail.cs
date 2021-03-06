//
// MacOS Quicklook Thumbnail support
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2012, Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#if MONOMAC
using System;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.CoreGraphics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MonoMac.QuickLook {
	public static partial class QLThumbnailImage {
		[DllImport(Constants.QuickLookLibrary)]
		extern static IntPtr QLThumbnailImageCreate (IntPtr allocator, IntPtr url, System.Drawing.SizeF maxThumbnailSize, IntPtr options);		

		public static CGImage Create (NSUrl url, SizeF maxThumbnailSize, float scaleFactor = 1, bool iconMode = false)
		{
			NSMutableDictionary dictionary = null;

			if (scaleFactor != 1 && iconMode != false) {
				dictionary = new NSMutableDictionary ();
				dictionary.LowlevelSetObject ((NSNumber) scaleFactor, OptionScaleFactorKey.Handle);
				dictionary.LowlevelSetObject (iconMode ? CFBoolean.True.Handle : CFBoolean.False.Handle, OptionIconModeKey.Handle);
			}
			
			var handle = QLThumbnailImageCreate (IntPtr.Zero, url.Handle, maxThumbnailSize, dictionary == null ? IntPtr.Zero : dictionary.Handle);
			GC.KeepAlive (dictionary);
			if (handle != IntPtr.Zero)
				return new CGImage (handle, true);
			
			return null;
		}
	}
}
#endif