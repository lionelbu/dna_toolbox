
using ExcelDna.Integration;
using ExcelDna.ComInterop;
using System.Runtime.InteropServices;
namespace ToolBoxCOM
{
    [ComVisible(false)]
    class ExcelAddin : IExcelAddIn
    {
        public void AutoOpen()
        {
            ComServer.DllRegisterServer();
        }
        public void AutoClose()
        {
            ComServer.DllUnregisterServer();
        }
    }
}
