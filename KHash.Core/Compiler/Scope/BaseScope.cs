using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Scope
{
    public class BaseScope
    {
        public List<InnerScope> Scopes = new List<InnerScope>();
        public Memory Memory = new Memory();
    }
}
