using System;
using WixSharp;

namespace BuildInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            var project = new Project("PnpUtilGui")
            {
                GUID = new Guid("b9bcb379-628f-4c7b-9863-6643cbddc70c"),
                Version = new Version(1,0,0),
                Dirs = new[]
                {
                    new Dir(
                        @"%ProgramFiles%\Tairiku\PnpUtilHelper", 
                        new DirFiles(@"C:\Users\mmwar_000\Documents\Visual Studio 2017\Projects\PnpUtilGui\PnpUtilGui\bin\Release\*.*"),
                        new Dir(
                            "ru-RU",
                            new DirFiles(
                                @"C:\Users\mmwar_000\Documents\Visual Studio 2017\Projects\PnpUtilGui\PnpUtilGui\bin\Release\ru-RU\*.*"
                            )
                        )
                    ), 
                }
            };
            Compiler.BuildMsi(project);
        }
    }
}
