using LayUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayUp.Interface
{
    interface IDataRepository
    {
        ErrorRecord GetErrorRecord(long id);
        void SaveErrorRecord(ErrorRecord customer);

    }
}
