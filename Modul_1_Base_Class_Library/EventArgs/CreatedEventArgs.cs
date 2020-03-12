
namespace Modul_1_Base_Class_Library.EventArgs
{
    public class CreatedEventArgs<TModel> : System.EventArgs
    {
        public TModel CreatedItem { get; set; }
    }
}
