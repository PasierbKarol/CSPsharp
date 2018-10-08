using System;
using System.Collections.Generic;
using CSPlang;

namespace CsharpGroovyCSP
{
    public class PAR : CSPParallel
    {
        public PAR(List<Object> processList) : base()
        {
            foreach (object o in processList)
            {
                
            }

            //processList.each{
            //    p->
            //    this.addProcess((CSProcess)p)
            //}
        }

        public PAR() : base()
        {
            
        }
    }
}
