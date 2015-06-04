﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace L10NSharp
{
	/// <summary>
	/// This class exists only to override Microsoft's NativeName for the Azeri neutral culture.
	/// According to Ken Keyes the NativeName on the 'az-Latn' culture is correct, while the
	/// NativeName on the 'az' culture is incorrect. So we do a swifty swap.
	/// </summary>
	public class L10NCultureInfo: CultureInfo
	{
		private string _nativeName = null;

		public L10NCultureInfo(string name)
			:base(name)
		{
			// Why the Substring? The full NativeName of 'az-Latn' is 'Azərbaycan dili (Azərbaycan)'
			// We just need the part up through 'dili'
			if (name == "az")
				_nativeName = GetCultureInfo("az-Latn").NativeName.Substring(0, 15);
		}

		public override string NativeName
		{
			get
			{
				return _nativeName ?? base.NativeName;
			}
		}

		/// <summary>
		/// Gets the list of supported cultures in the form of L10NCultureInfo objects.
		/// There is some danger in calling this repeatedly in that it creates new objects,
		/// whereas the CultureInfo version appears to return cached objects.
		/// </summary>
		/// <param name="types"></param>
		/// <returns></returns>
		public new static IEnumerable<L10NCultureInfo> GetCultures(CultureTypes types)
		{
			return CultureInfo.GetCultures(types).Select(culture => new L10NCultureInfo(culture.Name));
		}

		/// <summary>
		/// Retrieves a new instance of a culture by using the specified culture name.
		/// The CultureInfo version of this method returns a cached read-only instance.
		/// </summary>
		/// <param name="culture"></param>
		/// <returns></returns>
		public new static L10NCultureInfo GetCultureInfo(string culture)
		{
			return new L10NCultureInfo(culture);
		}
	}
}
