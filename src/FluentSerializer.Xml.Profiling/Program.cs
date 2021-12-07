using FluentSerializer.Core.Profiling.Runner;
using FluentSerializer.Xml.Profiling.Data;
using System;
using System.Security.Permissions;

namespace FluentSerializer.Xml.Profiling
{
    public static class Program
    {
        [STAThread, PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
		public static void Main()
		{
			StaticTestRunner.RequireElevatedPermissions();
			XmlDataCollection.Default.GenerateTestCaseFiles();

            StaticTestRunner.Run(typeof(Program).Assembly, "xml");
        }
    }
}
