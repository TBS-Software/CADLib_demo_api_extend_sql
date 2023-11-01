using CADLib;
using CADLibKernel;
using CdeLib;
using LightweightDataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TBS_CADLib_demo_sql_connector.CADLib_data;

namespace TBS_CADLib_demo_sql_connector
{
    partial class TBS_Demo_Form
    {
        private void _DReportExchangeProgress(EDataExchangeProgress p, object value) { }
        private Stopwatch timer = new Stopwatch();
        private void MenuItem_UsingAPI_Click(object sender, EventArgs e)
        {
            timer = new Stopwatch();
            timer.Start();

            int param_COLLISION_OBJECT1_id = m_library.GetParamDefId("COLLISION_OBJECT1");
            int param_COLLISION_OBJECT2_id = m_library.GetParamDefId("COLLISION_OBJECT2");


            CLibObjectInfo[] collisions = m_library.GetLibraryObjectsByCategory("COLLISIONS");
            List<Guid> object_UIDs_in_collisions = new List<Guid>();

            if (collisions != null)
            {
                foreach (CLibObjectInfo one_collision in collisions)
                {
                    string param_COLLISION_OBJECT1 = m_library.GetParameterValue(one_collision.idObject, param_COLLISION_OBJECT1_id).ToString();
                    string param_COLLISION_OBJECT2 = m_library.GetParameterValue(one_collision.idObject, param_COLLISION_OBJECT2_id).ToString();

                    Guid OBJECT1_UID = Guid.Empty;
                    Guid OBJECT2_UID = Guid.Empty;

                    if (Guid.TryParse(param_COLLISION_OBJECT1, out OBJECT1_UID))
                    {
                        if (!object_UIDs_in_collisions.Contains(OBJECT1_UID)) object_UIDs_in_collisions.Add(OBJECT1_UID);
                    }
                    if (Guid.TryParse(param_COLLISION_OBJECT2, out OBJECT2_UID))
                    {
                        if (!object_UIDs_in_collisions.Contains(OBJECT2_UID)) object_UIDs_in_collisions.Add(OBJECT2_UID);
                    }
                }
            }
            //Получение всех объектов модели, чьи UID равны в списке object_ids_in_collisions
            //и запрос для них значения параметра PROJECT_CHECKIN_FILE_NAME
            int param_PROJECT_CHECKIN_FILE_NAME_id = m_library.GetParamDefId("PROJECT_CHECKIN_FILE_NAME");
            List<string> PROJECT_CHECKIN_FILE_NAMES = new List<string>();
            foreach (var Category in m_library.GetCategoriesList())
            {
                var category_objects = m_library.GetLibraryObjectsByCategory(Category.Value.mSysName);
                if (category_objects != null)
                {
                    foreach (CLibObjectInfo object_in_category in category_objects)
                    {
                        if (object_UIDs_in_collisions.Contains(object_in_category.UID))
                        {
                            var param_PROJECT_CHECKIN_FILE_NAME = m_library.GetParameterValue(object_in_category.idObject, param_PROJECT_CHECKIN_FILE_NAME_id);
                            if (param_PROJECT_CHECKIN_FILE_NAME != null)
                            {
                                string param_PROJECT_CHECKIN_FILE_NAME_str = param_PROJECT_CHECKIN_FILE_NAME.ToString();
                                if (!PROJECT_CHECKIN_FILE_NAMES.Contains(param_PROJECT_CHECKIN_FILE_NAME_str)) PROJECT_CHECKIN_FILE_NAMES.Add(param_PROJECT_CHECKIN_FILE_NAME_str);
                            }
                        }
                    }
                }
            }

            timer.Stop();
            MessageBox.Show($"API запрос \n Файлы публикации с ошибками: {string.Join("\r\n", PROJECT_CHECKIN_FILE_NAMES)} \n Время выполнения составило {timer.ElapsedMilliseconds} ms");
        }

        private void MenuItem_UsingOpenSQL_Click(object sender, EventArgs e)
        {
            timer = new Stopwatch();
            timer.Start();

            OpenFileDialog sql_open = new OpenFileDialog();
            sql_open.Title = "Выбор SQL файла запроса";
            sql_open.Filter = "SQL файлы (*.sql;*.SQL) | *.sql;*.SQL";
            DialogResult dr = sql_open.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                m_library.RunSqlScript(sql_open, m_mainForm);
                wait_executing();

            }
            wait_executing();

            void wait_executing()
            {
                if (!File.Exists(file_cadlib_output) || (File.Exists(file_cadlib_output) && new FileInfo(file_cadlib_output).Length < 8))
                {
                    Thread.Sleep(1000);
                    wait_executing();
                }
            }

            WorkWithSQL_Result();
        }

        private void MenuItem_UsingAutoSQL_Click(object sender, EventArgs e)
        {
            timer = new Stopwatch();
            timer.Start();

            var s_type = m_library.Connection.ServerType.ToString().ToLower();
            string pattern = "GettingObjects";

            /*Так как мы сохраняем наш SQL файл в ресурсы проекта (Embedded resource), мы должны получить путь до него*/
            var assembly = Assembly.GetExecutingAssembly();
            string[] res_names = assembly.GetManifestResourceNames();

            string sql_temp_file = Path.Combine(dir_cadlib_output, "query.sql");
            if (File.Exists(sql_temp_file)) File.Delete(sql_temp_file);

            IEnumerable<string> res_need = res_names.Where(a => a.Contains(s_type) && a.Contains(pattern));
            if (res_need.Any())
            {
                using (Stream stream = assembly.GetManifestResourceStream(res_need.First()))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    File.WriteAllText(sql_temp_file, result);
                }
            }
            else
            {
                throw new Exception("Не найдено файлов для данного запроса!");
            }

            RunSQL(sql_temp_file);
            WorkWithSQL_Result();

        }
        private void RunSQL(string sql_path)
        {
            bool need_check = false;
            if (!File.Exists(sql_path))
            {
                throw new Exception("Файл не сущестует или не найден " + sql_path);
            }
            try
            {
                m_library.RunSqlScripts(new DMakeDbCommandInfo(m_library.Connection.Command), dir_cadlib_output,
                            new string[] { sql_path }, _DReportExchangeProgress);
            }
            catch (Exception ex)
            {
                need_check = false;
                throw new Exception(ex.Message, ex);
            }

            wait_executing();

            void wait_executing()
            {
                if (need_check && !File.Exists(file_cadlib_output) || (File.Exists(file_cadlib_output) && new FileInfo(file_cadlib_output).Length < 8))
                {
                    Thread.Sleep(1000);
                    wait_executing();
                }
            }
        }
        private void WorkWithSQL_Result()
        {
            if (File.Exists(file_cadlib_output))
            {
                List<string> PROJECT_CHECKIN_FILE_NAMES = new List<string>();
                foreach (string str in File.ReadLines(file_cadlib_output).Skip(1))
                {
                    PROJECT_CHECKIN_FILE_NAMES.Add(str);
                }
                string PROJECT_CHECKIN_FILE_NAMES_str = string.Join("\r\n", PROJECT_CHECKIN_FILE_NAMES);
                timer.Stop();
                MessageBox.Show($"SQL запрос \n Файлы публикации с ошибками: {PROJECT_CHECKIN_FILE_NAMES_str} \n Время выполнения составило {timer.ElapsedMilliseconds} ms");
            }
        }
    }
}
