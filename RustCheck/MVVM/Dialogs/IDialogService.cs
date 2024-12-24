using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustCheck.MVVM.Dialogs
{
    interface IDialogService
    {
        void ShowMessage(String message);
        String FilePath { get; set; }
        bool OpenFileDialog();
        bool SaveFileDialog();
    }
}
