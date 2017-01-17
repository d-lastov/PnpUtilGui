using System;
using System.Collections.Generic;
using System.Linq;
using PnpUtilGui.Models;

namespace PnpUtilGui.Utils
{
    internal class PnpUtilOutputParser
    {
        public List<Driver> ParseEnumDriverOutput(IEnumerable<string> enumerable)
        {
            var driverList = new List<Driver>();
            var stringArr = enumerable.ToArray();

            for (var i = 2; i < stringArr.Length - 1; ++i)
            {
                var driverInfo = new List<string>();

                do
                {
                    driverInfo.Add(stringArr[i]);
                    ++i;
                } while (!string.IsNullOrEmpty(stringArr[i]));

                var driver = new Driver
                {
                    FileName = GetLineValue(driverInfo[0]),
                    SourceName = GetLineValue(driverInfo[1]),
                    Publisher = GetLineValue(driverInfo[2]),
                    DriverClass = GetLineValue(driverInfo[3]),
                    ClassGuid = GetLineValue(driverInfo[4]),
                };

                if (driverInfo.Count == 7)
                {
                    driver.DateAndVersion = GetLineValue(driverInfo[5]);
                    driver.CertificateSignerName = GetLineValue(driverInfo[6]);
                }
                else
                {
                    driver.ClassVersion = GetLineValue(driverInfo[5]);
                    driver.DateAndVersion = GetLineValue(driverInfo[6]);
                    driver.CertificateSignerName = GetLineValue(driverInfo[7]);
                }
                driverList.Add(driver);
            }

            return driverList;
        }

        private static string GetLineValue(string line)
        {
            return line.Split(':')[1].Trim();
        }
    }
}