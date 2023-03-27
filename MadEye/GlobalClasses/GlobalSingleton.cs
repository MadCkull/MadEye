using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadEye.GlobalClasses;
public class GlobalSingletonClass
{
    private static GlobalSingletonClass? instance;

    public long SelectedHomeModuleID //Stores the ID of Selected Module (When Clicked)
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
