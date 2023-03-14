using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadEye.GlobalClasses;
public class GlobalSingletonClass
{
    private static GlobalSingletonClass? instance;
    public long HomeModuleSelectedModuleID
    {
        get; set;
    }

    private GlobalSingletonClass()
    {
        // Private constructor to prevent instantiation
    }

    public static GlobalSingletonClass Instance
    {
        get
        {
            instance ??= new GlobalSingletonClass();
            return instance;
        }
    }
}
