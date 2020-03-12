using System;
using System.Threading.Tasks;

namespace Modul_1_Base_Class_Library.Interfases
{

        public interface IDistributor<TModel>
        {
            Task MoveAsync(TModel item);
        }
    
}
