using System;
using AppGet.Exceptions;

namespace AppGet.Options
{
    public class PackageNameMissingException : AppGetException
    {
        public PackageNameMissingException() :
            base("this command requires a packages name")
        {
        }


    }
}