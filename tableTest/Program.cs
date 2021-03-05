using System;
using TableDependency.SqlClient.Base.EventArgs;

namespace tableTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string connString = "Data Source=DESKTOP-79B5CPA;Initial Catalog=notDB;Integrated Security=True";
            using (TableDependency.SqlClient.SqlTableDependency<Client> dep = new TableDependency.SqlClient.SqlTableDependency<Client>(connString))
            {
                dep.OnChanged += Dep_OnChanged;
                dep.Start();

                Console.WriteLine("Presione <enter> para salir");
                Console.ReadLine();

                dep.Stop();
            }
        }

        private static void Dep_OnChanged(object sender, RecordChangedEventArgs<Client> e)
        {
            switch (e.ChangeType)
            {
                case TableDependency.SqlClient.Base.Enums.ChangeType.None:
                    break;
                case TableDependency.SqlClient.Base.Enums.ChangeType.Delete:
                    Console.WriteLine(e.Entity.title + " delete");
                    break;
                case TableDependency.SqlClient.Base.Enums.ChangeType.Insert:
                    Console.WriteLine(e.Entity.title + " add");
                    break;
                case TableDependency.SqlClient.Base.Enums.ChangeType.Update:
                    Console.WriteLine(e.Entity.title + " update");
                    break;
                default:
                    break;
            }

        }
    }
}
