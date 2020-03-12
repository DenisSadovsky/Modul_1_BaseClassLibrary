using System;
using Modul_1_Base_Class_Library.EventArgs;

namespace Modul_1_Base_Class_Library.Interfases
{
    public interface ILocationsWatcher<TModel>
    {
        event EventHandler<CreatedEventArgs<TModel>> Created;
    }
}
