using CADLib;

namespace TBS_CADLib_demo_sql_connector
{
    public class CADLibPluginEntryPoint
    {
        /// <summary>
        /// Статический метод регистрации плагин, вызываемый родительским приложением
        /// </summary>
        /// <param name="manager">Объект текущего окружения плагина</param>
        /// <returns>Интерфейс плагина</returns>
        public static ICADLibPlugin RegisterPlugin(PluginsManager manager)
        {
            CADLib_data.m_mainForm = manager.MainForm;
            CADLib_data.m_mainDBBrowser = manager.MainDBBrowser;
            CADLib_data.m_library = manager.Library;

            return new TBS_Demo_Form();
            //return new TBS_Menu(manager.MainForm, manager.MainDBBrowser, manager.Library);
        }
    }
}