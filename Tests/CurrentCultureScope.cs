using System;
using System.Globalization;

namespace Tests
{
    internal class CurrentCultureScope : IDisposable
    {
        private readonly CultureInfo _originalCurrentCulture;

        public CurrentCultureScope(CultureInfo cultureInfo)
        {
            _originalCurrentCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = cultureInfo;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = _originalCurrentCulture;
        }
    }
}
