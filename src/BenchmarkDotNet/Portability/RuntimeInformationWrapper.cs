using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Environments;

namespace BenchmarkDotNet.Portability
{
    public class RuntimeInformationWrapper
    {
        public Runtime GetCurrentRuntime()
        {
            return RuntimeInformation.GetCurrentRuntime();
        }

        internal Platform GetCurrentPlatform()
        {
            return RuntimeInformation.GetCurrentPlatform();
        }

        public Jit GetCurrentJit()
        {
            return RuntimeInformation.GetCurrentJit();
        }

        public IntPtr GetCurrentAffinity()
        {
            return RuntimeInformation.GetCurrentAffinity();
        }
    }
}
